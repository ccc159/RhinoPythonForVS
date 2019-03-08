//------------------------------------------------------------------------------
// <copyright file="SendCode.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Net.Sockets;
using System.Runtime.Serialization;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json;


namespace RhinoPythonForVisualStudio
{
    /// <summary>
    /// Defines the sending message object structure
    /// </summary>
    [DataContract]
    public class msgObject
    {
        [DataMember]
        internal bool run;
        [DataMember]
        internal bool temp;
        [DataMember]
        internal bool reset;
        [DataMember]
        internal string filename;
    }
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class SendCode
    {
        /// <summary>
        /// Command Send Code ID.
        /// </summary>
        public const int CommandSendCodeID = 0x0100;

        /// <summary>
        /// Command Send Code Without Reset ID.
        /// </summary>
        public const int CommandSendCodeWRID = 0x0101;

        /// <summary>
        /// Command Import Manager ID.
        /// </summary>
        public const int CommandImportManagerID = 0x0102;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("14cdd510-7d18-400c-b709-7dd8d532a781");
        /// <summary>
        /// Output Pane GUID.
        /// </summary>
        public static Guid PaneGuid = new Guid("82BF54A1-9EF7-42E0-9842-0E1D16FF6B8C");

        /// <summary>
        /// isRunning Flag
        /// </summary>
        public static bool IsSending = false;
        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendCode"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private SendCode(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                // SendCode
                var menuCommandSendCodeID = new CommandID(CommandSet, CommandSendCodeID);
                var menuItemSendCode = new MenuCommand( (sender, args) => SendCodeToRhino(sender,true), menuCommandSendCodeID);
                commandService.AddCommand(menuItemSendCode);
                // SendCodeWithoutReset
                var menuCommandSendCodeWRID = new CommandID(CommandSet, CommandSendCodeWRID);
                var menuItemSendCodeWR = new MenuCommand((sender, args) => SendCodeToRhino(sender, false), menuCommandSendCodeWRID);
                commandService.AddCommand(menuItemSendCodeWR);
                // Open ImportManager dialog
                var menuCommandImportManagerID = new CommandID(CommandSet, CommandImportManagerID);
                var menuItemImportManager = new MenuCommand((sender, args) => OpenImportManagerDialog(), menuCommandImportManagerID);
                commandService.AddCommand(menuItemImportManager);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static SendCode Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new SendCode(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void SendCodeToRhino(object sender, bool resetEngine)
        {
            // bypass the function if the code is running
            if (IsSending)
            {
                Alert("Cannot send code.\nAn existing code is still running.");
                return;
            }
            // run in another thread to send the code
            System.Threading.Tasks.Task.Run(() =>
            {
                // set running flag
                IsSending = true;
                // get the top level vs instance object
                var dte = this.ServiceProvider.GetService(typeof(_DTE)) as _DTE;

                if (dte == null)
                {
                    string message = "Cannot init RhinoPython extension.\nTry to restart visual studio.";
                    Alert(message);
                    IsSending = false;
                    return;
                }
                // save all the documents
                dte.Documents.SaveAll();

                // get the filename with absolute path
                var activeDocument = dte.ActiveDocument;
                if (activeDocument == null)
                {
                    string message = "Cannot read current document.\nMake sure you have the code opening.";
                    Alert(message);
                    IsSending = false;
                    return;
                }
                var activeDocumentName = dte.ActiveDocument.FullName;
                // check if the file is temp file by checking the if the path of file is inside temp folder
                var tempPath = System.IO.Path.GetTempPath();
                var activeDocumentPath = dte.ActiveDocument.Path;
                var isTempFile = tempPath == activeDocumentPath;
                // compose the message
                msgObject objMsg = new msgObject();
                objMsg.filename = activeDocumentName;
                objMsg.temp = isTempFile;
                objMsg.reset = resetEngine;
                objMsg.run = true;

                string sendingMessage = JsonConvert.SerializeObject(objMsg);
                Byte[] sendingBytes = System.Text.Encoding.ASCII.GetBytes(sendingMessage);

                // get output panel
                var outputPane = GetOutputPane();
                outputPane.Clear();
                outputPane.OutputString($"====== {DateTime.Now.ToString(CultureInfo.CurrentCulture)} ======\n");

                // init tcp connection
                const int portNo = 614;
                const string serverIp = "127.0.0.1";
                try
                {
                    TcpClient client = new TcpClient(serverIp, portNo);
                  
                    // Get a client stream for reading and writing.
                    NetworkStream stream = client.GetStream();

                    // Send the message to the connected TcpServer. 
                    stream.Write(sendingBytes, 0, sendingBytes.Length);

                    // Receive the TcpServer.response.
                    bool isConnected = true;
                    while (isConnected)
                    {
                        // Buffer to store the response bytes.
                        var data = new Byte[256];

                        // Read the the TcpServer response bytes.
                        Int32 bytes = stream.Read(data, 0, data.Length);

                        // String to store the response ASCII representation.
                        string responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                        // display message

                        outputPane.OutputString(responseData);

                        // Detect if client disconnected
                        if (client.Client.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            // Client disconnected
                            isConnected = client.Client.Receive(buff, SocketFlags.Peek) != 0;
                        }
                    }

                    // Close everything.
                    stream.Close();
                    client.Close();
                }
                
                catch (SocketException ex)
                {
                    Alert("Cannot connect Rhino.\nPlease make sure Rhino is running CodeListener.");
                    outputPane.OutputString("Failed:\n" + ex.Message);
                }
                catch (Exception ex)
                {
                    Alert("Unexpected Error.\\Please see the error message in the output panel.");
                    outputPane.OutputString("Failed:\n" + ex.Message);
                }
                finally
                {
                    // set running flag
                    IsSending = false;
                }
            });
            
        }


        /// <summary>
        /// This opens the import manager
        /// </summary>
        private void OpenImportManagerDialog()
        {
            ImportManager manager = new ImportManager();
            manager.Show();
        }

        /// <summary>
        /// This is a wrapper function to alert
        /// </summary>
        private void Alert(string message)
        {
            VsShellUtilities.ShowMessageBox(
                this.ServiceProvider,
                message,
                "",
                OLEMSGICON.OLEMSGICON_CRITICAL,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        /// <summary>
        /// This function gets the RhinoPython OutputPane, if not, then create a new one.
        /// </summary>
        private IVsOutputWindowPane GetOutputPane()
        {
            // get output window
            IVsOutputWindow outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            if (outWindow == null) return null;

            // get output pane
            IVsOutputWindowPane rhinoPythonPane;
            outWindow.GetPane(ref PaneGuid, out rhinoPythonPane);

            if (rhinoPythonPane == null)
            {
                outWindow.CreatePane(ref PaneGuid, "RhinoPython", 1, 1);
            }

            outWindow.GetPane(ref PaneGuid, out rhinoPythonPane);
            // activate pane
            rhinoPythonPane.Activate();
            // return pane
            return rhinoPythonPane;
        }
    }
}
