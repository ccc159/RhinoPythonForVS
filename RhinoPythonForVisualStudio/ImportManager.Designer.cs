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
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxPath = new System.Windows.Forms.ListBox();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.toolTipModule = new System.Windows.Forms.ToolTip(this.components);
            this.FortoopTip = new System.Windows.Forms.Label();
            this.listBoxDll = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.forDLLTooltip = new System.Windows.Forms.Label();
            this.toolTipDll = new System.Windows.Forms.ToolTip(this.components);
            this.buttonReloadProject = new System.Windows.Forms.Button();
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
            this.listBoxPath.Size = new System.Drawing.Size(472, 212);
            this.listBoxPath.TabIndex = 1;
            this.listBoxPath.DoubleClick += new System.EventHandler(this.buttonEdit_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPath.Location = new System.Drawing.Point(15, 34);
            this.textBoxPath.Multiline = true;
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(472, 212);
            this.textBoxPath.TabIndex = 2;
            this.textBoxPath.Visible = false;
            this.textBoxPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPath_KeyDown);
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Location = new System.Drawing.Point(376, 262);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(54, 28);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(319, 262);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(54, 28);
            this.buttonEdit.TabIndex = 4;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(433, 262);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(54, 28);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDefault
            // 
            this.buttonDefault.Location = new System.Drawing.Point(16, 262);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(94, 28);
            this.buttonDefault.TabIndex = 6;
            this.buttonDefault.Text = "Restore Defaults";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // toolTipModule
            // 
            this.toolTipModule.AutoPopDelay = 20000;
            this.toolTipModule.InitialDelay = 100;
            this.toolTipModule.ReshowDelay = 100;
            // 
            // FortoopTip
            // 
            this.FortoopTip.AutoSize = true;
            this.FortoopTip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FortoopTip.ForeColor = System.Drawing.SystemColors.Highlight;
            this.FortoopTip.Location = new System.Drawing.Point(116, 9);
            this.FortoopTip.Name = "FortoopTip";
            this.FortoopTip.Size = new System.Drawing.Size(13, 13);
            this.FortoopTip.TabIndex = 7;
            this.FortoopTip.Text = "?";
            this.FortoopTip.MouseHover += new System.EventHandler(this.FortoopTip_MouseHover);
            // 
            // listBoxDll
            // 
            this.listBoxDll.BackColor = System.Drawing.Color.Gainsboro;
            this.listBoxDll.FormattingEnabled = true;
            this.listBoxDll.HorizontalScrollbar = true;
            this.listBoxDll.Location = new System.Drawing.Point(496, 34);
            this.listBoxDll.Name = "listBoxDll";
            this.listBoxDll.Size = new System.Drawing.Size(171, 212);
            this.listBoxDll.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(493, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Referenced DLL";
            // 
            // forDLLTooltip
            // 
            this.forDLLTooltip.AutoSize = true;
            this.forDLLTooltip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forDLLTooltip.ForeColor = System.Drawing.SystemColors.Highlight;
            this.forDLLTooltip.Location = new System.Drawing.Point(574, 9);
            this.forDLLTooltip.Name = "forDLLTooltip";
            this.forDLLTooltip.Size = new System.Drawing.Size(13, 13);
            this.forDLLTooltip.TabIndex = 10;
            this.forDLLTooltip.Text = "?";
            this.forDLLTooltip.MouseHover += new System.EventHandler(this.forDLLTooltip_MouseHover);
            // 
            // buttonReloadProject
            // 
            this.buttonReloadProject.Location = new System.Drawing.Point(116, 262);
            this.buttonReloadProject.Name = "buttonReloadProject";
            this.buttonReloadProject.Size = new System.Drawing.Size(94, 28);
            this.buttonReloadProject.TabIndex = 11;
            this.buttonReloadProject.Text = "Reload Project";
            this.buttonReloadProject.UseVisualStyleBackColor = true;
            this.buttonReloadProject.Click += new System.EventHandler(this.buttonReloadProject_Click);
            // 
            // ImportManager
            // 
            this.ClientSize = new System.Drawing.Size(682, 309);
            this.Controls.Add(this.buttonReloadProject);
            this.Controls.Add(this.forDLLTooltip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBoxDll);
            this.Controls.Add(this.FortoopTip);
            this.Controls.Add(this.buttonDefault);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.listBoxPath);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ImportManager";
            this.ShowIcon = false;
            this.Text = "Import Manager";
            this.Load += new System.EventHandler(this.ImportManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxPath;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.ToolTip toolTipModule;
        private System.Windows.Forms.Label FortoopTip;
        private System.Windows.Forms.ListBox listBoxDll;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label forDLLTooltip;
        private System.Windows.Forms.ToolTip toolTipDll;
        private System.Windows.Forms.Button buttonReloadProject;
    }
}