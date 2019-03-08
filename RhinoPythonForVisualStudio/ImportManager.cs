using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RhinoPythonForVisualStudio
{
    public partial class ImportManager : Form
    {
        public ImportManagerModel Model;
        public ImportManager()
        {
            InitializeComponent();
            Model = new ImportManagerModel();
            
        }

        private void ImportManager_Load(object sender, EventArgs e)
        {
            listBoxPath.Items.Clear();
            foreach (var path in Model.Paths)
            {
                listBoxPath.Items.Add(path);
            }
        }
    }

    /// <summary>
    /// This is the import manager model that deal with import data
    /// </summary>
    public class ImportManagerModel
    {
        private static string ImportFilePath = @"import.targets";
        private static string TemplateDoc = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"+
                                            "<Project DefaultTargets = \"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\" ToolsVersion=\"4.0\">\n"+
                                            "<PropertyGroup>\n"+
                                            "<SearchPath>\n"+
                                            "Hello World\n"+
                                            "</SearchPath>\n"+
                                            "</PropertyGroup>\n"+
                                            "</Project>";

        public string[] Paths;
        public static string[] LoadImportFile()
        {
            // read import.targets xml file
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(ImportFilePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                doc.LoadXml(TemplateDoc);
            }

            // read search path nodes
            XmlNode node = doc.DocumentElement?.GetElementsByTagName("SearchPath")[0];
            // read its inner text
            string[] paths = node?.InnerText.Split(';');

            return paths;
        }

        public ImportManagerModel()
        {
            Paths = LoadImportFile();
        }
    }
}
