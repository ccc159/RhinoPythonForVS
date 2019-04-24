//------------------------------------------------------------------------------
// <copyright file="RunFunctionCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json;

namespace RhinoPythonForVisualStudio
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class RunFunctionCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 256;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("16554048-73ad-4f0d-921a-d42c45514a41");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// isRunning Flag
        /// </summary>
        public static bool IsSending = false;

        /// <summary>
        /// Output Pane GUID.
        /// </summary>
        public static Guid PaneGuid = new Guid("82BF54A1-9EF7-42E0-9842-0E1D16FF6B8C");

        private int initLineIndex;
        private string tempFileName = "_temp_ui_script.py";

        /// <summary>
        /// Initializes a new instance of the <see cref="RunFunctionCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private RunFunctionCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static RunFunctionCommand Instance
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
            Instance = new RunFunctionCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            // get function name, class name, class path
            var funcName = GetFunctionName();
            if (funcName == null) return;
            var className = GetClassName();
            if (className == null) return;
            var classPath = GetClassPath();

            // prepare python file
            var pythonTemplate = GetPythonTemplateString()
                .Replace("CLASSPATH", classPath)
                .Replace("CLASSNAME", className)
                .Replace("FUNCTIONNAME", funcName);

            // save python file to root folder of the project
            saveTempFile(pythonTemplate);

            // run the file through code listener
            SendCodeToRhino(true);

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
                OLEMSGICON.OLEMSGICON_NOICON,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        /// <summary>
        /// Return current function name at the right click point
        /// </summary>
        /// <returns></returns>
        internal string GetFunctionName()
        {
            // get the top level vs instance object
            var dte = this.ServiceProvider.GetService(typeof(_DTE)) as _DTE;

            if (dte == null)
            {
                string message = "Cannot init RhinoPython extension.\nTry to restart visual studio.";
                Alert(message);
                return null;
            }
            // save all the documents
            dte.Documents.SaveAll();

            // get current line text
            var selection = (TextSelection)dte.ActiveDocument.Selection;
            initLineIndex = selection.CurrentLine;

            selection.SelectLine();
            string text = selection.Text.Trim();   
            while (text.EndsWith(","))
            {
                selection.SelectLine();  // select line automatically moves the cursor to next line
                text += selection.Text.Trim();
            }

            // get function name if it matches the pattern, otherwise null
            var funcName = matchFunctionPatternName(text);
            if (funcName == null) Alert("This is not a valid python function");
            selection.GotoLine(initLineIndex);
            return funcName;
        }

        internal string matchFunctionPatternName(string text)
        {
            Regex rx = new Regex(@"def\s+\w+\(.*\)\:\s*\#*.*", RegexOptions.Compiled);
            // Find matches.
            MatchCollection matches = rx.Matches(text);
            if (matches.Count == 0)
            {
                return null;
            }

            // get function name
            rx = new Regex(@"def\s+\w+\(", RegexOptions.Compiled);
            matches = rx.Matches(text);
            var value = matches[0].Groups[0].Value;
            var funcName = value.Remove(value.Length - 1).Remove(0, 3).Trim();

            return funcName;
        }

        internal string matchClassPatternName(string text)
        {
            Regex rx = new Regex(@"class\s+\w+\(.*\)\:\s*\#*.*", RegexOptions.Compiled);
            // Find matches.
            MatchCollection matches = rx.Matches(text);
            if (matches.Count == 0)
            {
                return null;
            }
            // get function name
            rx = new Regex(@"class\s+\w+\(", RegexOptions.Compiled);
            matches = rx.Matches(text);
            var value = matches[0].Groups[0].Value;
            var className = value.Remove(value.Length - 1).Remove(0, 5).Trim();

            return className;
        }


        /// <summary>
        /// Get current class name by looking upwards
        /// </summary>
        internal string GetClassName()
        {
            // get the top level vs instance object
            var dte = this.ServiceProvider.GetService(typeof(_DTE)) as _DTE;
            // get current line text
            var selection = (TextSelection)dte.ActiveDocument.Selection;

            while (true)
            {
                selection.GotoLine(selection.CurrentLine-1, true);
                string text = selection.Text.Trim();
                text = matchClassPatternName(text);
                if (text != null)
                {
                    selection.GotoLine(initLineIndex);
                    return text;
                };
                if (selection.CurrentLine == 1)
                {
                    Alert("Cannot find the class name.");
                    selection.GotoLine(initLineIndex);
                    return null;
                }
            }
        }

        internal string GetClassPath()
        {
            var absolutePath = GetFilePath();
            var splits = absolutePath.Split(new String[] { "\\classes\\" }, StringSplitOptions.None);
            var pathes = new List<string>();
            if (splits.Length == 0)
            {
                Alert("This file does not reside in classes folder.");
                return "";
            }

            var folders = splits[1];
            splits = folders.Split('\\');
            foreach (var split in splits)
            {
                if (split != "") pathes.Add(split);
            }
            string chainedPath = string.Join(".",pathes);
            if (chainedPath != "") return chainedPath + ".";
            return "";
        }

        internal string GetProjectPath()
        {
            var absolutePath = GetFilePath();
            var splits = absolutePath.Split(new String[] { "\\classes\\" }, StringSplitOptions.None);
            if (splits.Length == 0)
            {
                Alert("This file does not reside in classes folder.");
                return absolutePath;
            }

            return splits[0] + "\\" + tempFileName;
        }


        internal string GetFilePath()
        {
            // get the top level vs instance object
            var dte = this.ServiceProvider.GetService(typeof(_DTE)) as _DTE;

            return dte.ActiveDocument.Path;
        }

        internal void saveTempFile(string text)
        {
            try
            {
                System.IO.File.WriteAllText(GetProjectPath(), text);
            }
            catch (Exception e)
            {
                Alert("Run Function failed. Are you running another function at the same time?");
            }
            
        }

        internal void deleteTempFile()
        {
            try
            {
                File.Delete(GetProjectPath());
            }
            catch (Exception e)
            {
                Alert("Temp file didn't clean up successfully.");
            }
        }


        private void SendCodeToRhino(bool resetEngine)
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
               
                // compose the message
                msgObject objMsg = new msgObject();
                objMsg.filename = GetProjectPath();
                objMsg.temp = false;
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

                    // delete the file after it's done.
                    deleteTempFile();
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

        public string GetPythonTemplateString()
        {
            return @"import classes as MOD
import scriptcontext
import rhinoscriptsyntax as rs

def main():
    try:
        name = MOD.CLASSPATHCLASSNAME.DESCRIPTION
    except:
        name = 'CLASSNAME'
    try:
        items = MOD.CLASSPATHCLASSNAME.GetInstances('Select the %ss to process' % name)
        bRedraw = rs.EnableRedraw(False)
        if not items: return
        customFunction(items, name)
    finally:
        rs.EnableRedraw(bRedraw)

def customFunction(items, name):
    length = len(items)
    for i, item in enumerate(items):
        rs.Prompt('Process %ss %d/%d' % (name, i+1, length))
        scriptcontext.escape_test()
        item.FUNCTIONNAME()


if __name__ == '__main__':
    main()
";
        }
    }
}
