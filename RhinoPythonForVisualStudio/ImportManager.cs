using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using EnvDTE;
using EnvDTE80;
using String = System.String;

namespace RhinoPythonForVisualStudio
{
    public partial class ImportManager : Form
    {
        public ImportManagerModel ImportModel;
        public DLLManagerModel DllModel;
        public _DTE dte;
        
        public ImportManager(string ProjectPath,_DTE _dte)
        {
            InitializeComponent();
            ImportModel = new ImportManagerModel(ProjectPath);
            DllModel = new DLLManagerModel(ProjectPath);
            dte = _dte;
        }

        private void ImportManager_Load(object sender, EventArgs e)
        {
            updateListBoxContent();
            updateDllBoxContent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // switch from text box to list box
            listBoxPath.Visible = true;
            textBoxPath.Visible = false;
            // get raw path
            var rawText = textBoxPath.Text;
            // read its inner text
            string[] paths = rawText.Replace("\n","").Replace("\r", "").Split(';');
            // sanitize paths
            paths = paths.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            // set path to model
            ImportModel.Paths = paths;
            // update listbox
            updateListBoxContent();
            // save the file
            ImportModel.SaveImportFile();
            // disable itself
            buttonSave.Enabled = false;
        }

        private void updateListBoxContent()
        {
            listBoxPath.Items.Clear();
            foreach (var path in ImportModel.Paths) listBoxPath.Items.Add(path);
        }

        private void updateDllBoxContent()
        {
            listBoxDll.Items.Clear();
            foreach (var path in DllModel.Paths) listBoxDll.Items.Add(path);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            // switch from listbox to text box
            listBoxPath.Visible = false;
            textBoxPath.Visible = true;
            // load paths into textbox
            textBoxPath.Clear();
            textBoxPath.Text = String.Join(";\r\n", ImportModel.Paths) + ";";
            textBoxPath.Focus();
            // enable save button
            buttonSave.Enabled = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // close the import manager
            Close();
            Dispose();
        }

        private void textBoxPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // switch from text box to list box
                listBoxPath.Visible = true;
                textBoxPath.Visible = false;
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            ImportModel.ResetToDefaultPaths();
            // switch from text box to list box
            listBoxPath.Visible = true;
            textBoxPath.Visible = false;
            // update list box
            updateListBoxContent();
            // save the file
            ImportModel.SaveImportFile();
        }

        private void FortoopTip_MouseHover(object sender, EventArgs e)
        {
            string tooltip = "This contains user-specific import search paths for IronPython.\n\n";
            tooltip += "For a project, usually:\n";
            tooltip += "* your own python libraries folder.\n";
            tooltip += "* Rhino System folder (for RhinoCommon).\n";
            tooltip += "* rhinoscriptsyntax package (have a look in Rhino's Python Editor (Tools -> Options) to find it.\n";
            tooltip += "* Path of .NET Reference Assemblies.\n\n";
            tooltip += "Be aware that every path has to end with a ';'\n";
            tooltip += "THIS FILE IS PER-USER! Please make sure '*.targets' is in your git ignore list.";
            toolTipModule.SetToolTip(FortoopTip, tooltip);
        }

        private void forDLLTooltip_MouseHover(object sender, EventArgs e)
        {
            string tooltip = "Please edit DLL references directly in '__init__.py' file\n";
            tooltip += "under your project root.";
            toolTipModule.SetToolTip(forDLLTooltip, tooltip);
        }

        private void buttonReloadProject_Click(object sender, EventArgs e)
        {
            // reload project

            string solutionPath = dte.Solution.FullName;

            dte.ExecuteCommand("File.CloseSolution");
            dte.Solution.Open(solutionPath);
        }
    }

    /// <summary>
    /// This is the import manager model that deal with import data
    /// </summary>
    public class ImportManagerModel
    {
        private static readonly string FileName = @"import.targets";
        private static readonly string TemplateDoc = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"+
                                            "<Project DefaultTargets = \"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\" ToolsVersion=\"4.0\">\n"+
                                            "<PropertyGroup>\n"+
                                            "<SearchPath>\n"+
                                            "C:\\Program Files\\Rhinoceros 5 (64-bit)\\Plug-ins\\IronPython\\Lib;\n"+
                                            "</SearchPath>\n"+
                                            "</PropertyGroup>\n"+
                                            "</Project>";

        private static readonly string[] defaultPaths = {"C:\\Program Files\\Rhinoceros 5 (64-bit)\\Plug-ins\\IronPython\\Lib"};
        public string ImportFilePath;
        public string[] Paths;
        public XmlDocument Doc;
        /// <summary>
        /// Load import file from target.import
        /// </summary>
        /// <returns>a list from path read from target.import</returns>
        public string[] LoadImportFile()
        {
            // read import.targets xml file
            try
            {
                Doc.Load(ImportFilePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                Doc.LoadXml(TemplateDoc);
                Doc.Save(ImportFilePath);
            }

            // read search path nodes
            XmlNode node = Doc.DocumentElement?.GetElementsByTagName("SearchPath")[0];
            // read its inner text
            string[] paths = node?.InnerText.Replace("\n", "").Replace("\r", "").Split(';');
             
            // sanitize path
            paths = paths?.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            return paths;
        }

        /// <summary>
        /// Save the file into target.import
        /// </summary>
        public void SaveImportFile()
        {
            // put new paths into doc
            XmlNode node = Doc.DocumentElement?.GetElementsByTagName("SearchPath")[0];
            node.InnerText = String.Join(";\n",Paths) + ";";
            // save
            Doc.Save(ImportFilePath);
        }

        /// <summary>
        /// Reset model path to default path
        /// </summary>
        public void ResetToDefaultPaths()
        {
            Paths = defaultPaths;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public ImportManagerModel(string projectPath)
        {

            ImportFilePath = Path.Combine(projectPath, FileName);
            Doc = new XmlDocument();
            Paths = LoadImportFile();
            
        }
    }

    public class DLLManagerModel
    {
        private static readonly string FileName = @"__init__.py";

        private static readonly string templateDoc = "\"\"\"\n" +
                                                     "This file contains references to external DLLs.\n" +
                                                     "Feel free to add as required.\n" +
                                                     "Please do not put any paths in here, use RhinoPython -> ImportManager instead!\n" +
                                                     "\"\"\"\n\n" +
                                                     "import clr\n" +
                                                     "clr.AddReference(\"RhinoCommon.dll\")";

        public string Doc;
        public string[] Paths;

        /// <summary>
        /// Load import file from target.import
        /// </summary>
        /// <returns>a list from path read from target.import</returns>
        public DLLManagerModel(string projectPath)
        {
            var filePath = Path.Combine(projectPath, FileName);
            try
            {
                // read __init__ file
                Doc = File.ReadAllText(filePath, Encoding.UTF8);
            }
            catch (System.IO.FileNotFoundException)
            {
                // create a init file
                Doc = templateDoc;
                File.WriteAllText(filePath, Doc);
            }
            

            // parse the file into paths
            var matchedGroups = Regex.Matches(Doc, @"AddReference\(([^)]*)\)");
            
            string[] paths = new string[matchedGroups.Count];

            for (int i = 0; i < matchedGroups.Count; i++)
            {
                var value = matchedGroups[i].Value;
                paths[i] = value.Substring(14, value.Length - 16);
            }

            Paths = paths;
        }

    }
}
