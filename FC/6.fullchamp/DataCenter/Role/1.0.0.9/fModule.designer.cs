namespace CRole
{
    partial class fModule
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該公開 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fModule));
            this.panel1 = new System.Windows.Forms.Panel();
            this.LabRoleName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bbtnCancel = new System.Windows.Forms.Button();
            this.bbtnOK = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LVData = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.TreeViewAll = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TreeViewSelect = new System.Windows.Forms.TreeView();
            this.popMenu2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AlltoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemCombAuth = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.combAuth = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.popMenu2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.LabRoleName);
            this.panel1.Controls.Add(this.label1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // LabRoleName
            // 
            resources.ApplyResources(this.LabRoleName, "LabRoleName");
            this.LabRoleName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabRoleName.Name = "LabRoleName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.bbtnCancel);
            this.panel2.Controls.Add(this.bbtnOK);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // bbtnCancel
            // 
            this.bbtnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.bbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.bbtnCancel, "bbtnCancel");
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.UseVisualStyleBackColor = false;
            // 
            // bbtnOK
            // 
            this.bbtnOK.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.bbtnOK, "bbtnOK");
            this.bbtnOK.Name = "bbtnOK";
            this.bbtnOK.UseVisualStyleBackColor = false;
            this.bbtnOK.Click += new System.EventHandler(this.bbtnOK_Click);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.LVData);
            this.groupBox1.Controls.Add(this.TreeViewAll);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // LVData
            // 
            this.LVData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            resources.ApplyResources(this.LVData, "LVData");
            this.LVData.Name = "LVData";
            this.LVData.UseCompatibleStateImageBehavior = false;
            this.LVData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // TreeViewAll
            // 
            resources.ApplyResources(this.TreeViewAll, "TreeViewAll");
            this.TreeViewAll.ImageList = this.imageList1;
            this.TreeViewAll.Name = "TreeViewAll";
            this.TreeViewAll.ShowNodeToolTips = true;
            this.TreeViewAll.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeView_DragEnter);
            this.TreeViewAll.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeView_ItemDrag);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1.bmp");
            this.imageList1.Images.SetKeyName(1, "2.bmp");
            this.imageList1.Images.SetKeyName(2, "role3.bmp");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TreeViewSelect);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // TreeViewSelect
            // 
            this.TreeViewSelect.AllowDrop = true;
            this.TreeViewSelect.ContextMenuStrip = this.popMenu2;
            resources.ApplyResources(this.TreeViewSelect, "TreeViewSelect");
            this.TreeViewSelect.HideSelection = false;
            this.TreeViewSelect.ImageList = this.imageList1;
            this.TreeViewSelect.Name = "TreeViewSelect";
            this.TreeViewSelect.ShowNodeToolTips = true;
            this.TreeViewSelect.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TreeViewSelect_MouseUp);
            this.TreeViewSelect.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeViewSelect_DragDrop);
            this.TreeViewSelect.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewSelect_AfterSelect);
            this.TreeViewSelect.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeView_DragEnter);
            this.TreeViewSelect.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeView_ItemDrag);
            // 
            // popMenu2
            // 
            this.popMenu2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AlltoolStripMenuItem,
            this.menuitemCombAuth,
            this.toolStripSeparator2,
            this.collapseToolStripMenuItem,
            this.expandToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem});
            this.popMenu2.Name = "popMenu2";
            resources.ApplyResources(this.popMenu2, "popMenu2");
            this.popMenu2.Opening += new System.ComponentModel.CancelEventHandler(this.popMenu2_Opening);
            // 
            // AlltoolStripMenuItem
            // 
            this.AlltoolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minToolStripMenuItem,
            this.maxToolStripMenuItem});
            this.AlltoolStripMenuItem.Name = "AlltoolStripMenuItem";
            resources.ApplyResources(this.AlltoolStripMenuItem, "AlltoolStripMenuItem");
            // 
            // minToolStripMenuItem
            // 
            this.minToolStripMenuItem.Name = "minToolStripMenuItem";
            resources.ApplyResources(this.minToolStripMenuItem, "minToolStripMenuItem");
            this.minToolStripMenuItem.Click += new System.EventHandler(this.minToolStripMenuItem_Click);
            // 
            // maxToolStripMenuItem
            // 
            this.maxToolStripMenuItem.Name = "maxToolStripMenuItem";
            resources.ApplyResources(this.maxToolStripMenuItem, "maxToolStripMenuItem");
            this.maxToolStripMenuItem.Click += new System.EventHandler(this.maxToolStripMenuItem_Click);
            // 
            // menuitemCombAuth
            // 
            this.menuitemCombAuth.BackColor = System.Drawing.Color.White;
            this.menuitemCombAuth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.menuitemCombAuth.DropDownWidth = 200;
            resources.ApplyResources(this.menuitemCombAuth, "menuitemCombAuth");
            this.menuitemCombAuth.Name = "menuitemCombAuth";
            this.menuitemCombAuth.SelectedIndexChanged += new System.EventHandler(this.menuitemCombAuth_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // collapseToolStripMenuItem
            // 
            this.collapseToolStripMenuItem.Name = "collapseToolStripMenuItem";
            resources.ApplyResources(this.collapseToolStripMenuItem, "collapseToolStripMenuItem");
            this.collapseToolStripMenuItem.Click += new System.EventHandler(this.collapseToolStripMenuItem_Click);
            // 
            // expandToolStripMenuItem
            // 
            this.expandToolStripMenuItem.Name = "expandToolStripMenuItem";
            resources.ApplyResources(this.expandToolStripMenuItem, "expandToolStripMenuItem");
            this.expandToolStripMenuItem.Click += new System.EventHandler(this.expandToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.combAuth);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // combAuth
            // 
            this.combAuth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combAuth.FormattingEnabled = true;
            resources.ApplyResources(this.combAuth, "combAuth");
            this.combAuth.Name = "combAuth";
            this.combAuth.SelectedIndexChanged += new System.EventHandler(this.combAuth_SelectedIndexChanged);
            // 
            // fModule
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "fModule";
            this.Load += new System.EventHandler(this.fModule_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.popMenu2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button bbtnCancel;
        private System.Windows.Forms.Button bbtnOK;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView TreeViewAll;
        private System.Windows.Forms.TreeView TreeViewSelect;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView LVData;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox combAuth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip popMenu2;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox menuitemCombAuth;
        private System.Windows.Forms.ToolStripMenuItem AlltoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem maxToolStripMenuItem;
        private System.Windows.Forms.Label LabRoleName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}