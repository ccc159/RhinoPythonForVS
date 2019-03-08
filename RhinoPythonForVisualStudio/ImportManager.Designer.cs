namespace RhinoPythonForVisualStudio
{
    partial class ImportManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxPath = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Module Search Paths";
            // 
            // listBoxPath
            // 
            this.listBoxPath.FormattingEnabled = true;
            this.listBoxPath.HorizontalScrollbar = true;
            this.listBoxPath.Items.AddRange(new object[] {
            "C:\\Program Files\\Rhinoceros 5 (64-bit)\\Plug-ins\\IronPython\\Lib",
            "C:\\Users\\jch\\AppData\\Roaming\\McNeel\\Rhinoceros\\5.0\\Plug-ins\\IronPython (814d908a-" +
                "e25c-493d-97e9-ee3861957f49)\\settings\\lib",
            "C:\\Users\\jch\\AppData\\Roaming\\McNeel\\Rhinoceros\\5.0\\scripts",
            "C:\\Users\\jch\\Documents\\Projects\\D2P_Library"});
            this.listBoxPath.Location = new System.Drawing.Point(15, 34);
            this.listBoxPath.Name = "listBoxPath";
            this.listBoxPath.Size = new System.Drawing.Size(663, 212);
            this.listBoxPath.TabIndex = 1;
            // 
            // ImportManager
            // 
            this.ClientSize = new System.Drawing.Size(695, 329);
            this.Controls.Add(this.listBoxPath);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ImportManager";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.ImportManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxPath;
    }
}