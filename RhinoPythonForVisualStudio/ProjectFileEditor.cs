using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualStudio.OLE.Interop;

namespace RhinoPythonForVisualStudio
{
    public static class ProjectFileEditor
    {
        private static bool searchRemoved = false;
        private static bool importAdded = false;
        /// <summary>
        /// Edit project file to guarantee there's no search path in the file, and add our import.targets into the file
        /// </summary>
        /// <param name="filePath"></param>
        public static void EditProjectFile(string filePath)
        {
            XmlDocument Doc = new XmlDocument();
            Doc.Load(filePath);

            // read search path nodes and check if the node is empty
            var searchPathNodes = Doc.DocumentElement?.GetElementsByTagName("SearchPath");
            if (searchPathNodes?.Count == 0) searchRemoved = true;

            // read import path nodes and check if import is there
            var importPathNodes = Doc.DocumentElement?.GetElementsByTagName("Import");
            if (importPathNodes?.Count > 0)
            {
                for (int i = 0; i < importPathNodes.Count; i++)
                {
                    var node = importPathNodes[i];
                    if (node.Attributes?["Project"].Value == "import.targets") importAdded = true;
                }
            }

            // either false situation triggers a warning box or return
            if (searchRemoved && importAdded) return;
            
            // show warning box
            DialogResult result = LoadReloadingWarningBox();
            if (result == DialogResult.Cancel) return;
            
            // remove search nodes
            if (!searchRemoved)
            {
                for (int i = 0; i < searchPathNodes?.Count; i++)
                {
                    var node = searchPathNodes[i];
                    node.ParentNode?.RemoveChild(node);
                }
                Doc.Save(filePath);
            }

            if (!importAdded)
            {
                var root = Doc.DocumentElement;
                var importNode = Doc.CreateElement("Import", root?.NamespaceURI);

                importNode.SetAttribute("Condition", "Exists('import.targets')");
                importNode.SetAttribute("Project", "import.targets");
               
                root?.InsertBefore(importNode,root.FirstChild);
                Doc.Save(filePath);
            } 

        }

        private static DialogResult LoadReloadingWarningBox()
        {
            var messageBody = "Your project file is not properly configured. This would lead to an import malfunction.\n\n* Click OK to automatically update project configuration.\n  (A dialog of 'File Modification Detected' will popup, please click 'Reload' or 'Reload All')\n\n* Click Cancel to change it by yourself.";
            var title = "Project Configuration";
            var result = MessageBox.Show(messageBody, title,MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
            return result;
        }
    }
}
