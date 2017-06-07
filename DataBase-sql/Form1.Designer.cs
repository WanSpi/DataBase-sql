namespace DataBaseSQL {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.SaveTableButton = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.DataBaseTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.buttonAddDataBase = new System.Windows.Forms.ToolStripButton();
            this.buttonAddTable = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.SaveTableButton);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tabControl);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.DataBaseTree);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(783, 409);
            this.toolStripContainer1.ContentPanel.Resize += new System.EventHandler(this.toolStripContainer1_ContentPanel_Resize);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(783, 434);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // SaveTableButton
            // 
            this.SaveTableButton.Location = new System.Drawing.Point(696, 374);
            this.SaveTableButton.Name = "SaveTableButton";
            this.SaveTableButton.Size = new System.Drawing.Size(75, 23);
            this.SaveTableButton.TabIndex = 2;
            this.SaveTableButton.Text = "Save";
            this.SaveTableButton.UseVisualStyleBackColor = true;
            this.SaveTableButton.Click += new System.EventHandler(this.SaveTableButton_Click);
            // 
            // tabControl
            // 
            this.tabControl.Location = new System.Drawing.Point(145, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(638, 365);
            this.tabControl.TabIndex = 1;
            this.tabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl_MouseClick);
            // 
            // DataBaseTree
            // 
            this.DataBaseTree.ImageIndex = 0;
            this.DataBaseTree.ImageList = this.imageList;
            this.DataBaseTree.Location = new System.Drawing.Point(0, 3);
            this.DataBaseTree.Name = "DataBaseTree";
            this.DataBaseTree.SelectedImageIndex = 0;
            this.DataBaseTree.Size = new System.Drawing.Size(139, 406);
            this.DataBaseTree.TabIndex = 0;
            this.DataBaseTree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DataBaseTree_MouseDoubleClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "database");
            this.imageList.Images.SetKeyName(1, "table");
            this.imageList.Images.SetKeyName(2, "add_database.png");
            this.imageList.Images.SetKeyName(3, "add_table.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonAddDataBase,
            this.buttonAddTable});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(49, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // buttonAddDataBase
            // 
            this.buttonAddDataBase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonAddDataBase.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddDataBase.Image")));
            this.buttonAddDataBase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonAddDataBase.Name = "buttonAddDataBase";
            this.buttonAddDataBase.Size = new System.Drawing.Size(23, 22);
            this.buttonAddDataBase.Text = "toolStripButton1";
            this.buttonAddDataBase.Click += new System.EventHandler(this.buttonAddDataBase_Click);
            // 
            // buttonAddTable
            // 
            this.buttonAddTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonAddTable.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddTable.Image")));
            this.buttonAddTable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonAddTable.Name = "buttonAddTable";
            this.buttonAddTable.Size = new System.Drawing.Size(23, 22);
            this.buttonAddTable.Text = "toolStripButton1";
            this.buttonAddTable.Click += new System.EventHandler(this.buttonAddTable_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 434);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.TreeView DataBaseTree;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton buttonAddDataBase;
        private System.Windows.Forms.Button SaveTableButton;
        private System.Windows.Forms.ToolStripButton buttonAddTable;
    }
}

