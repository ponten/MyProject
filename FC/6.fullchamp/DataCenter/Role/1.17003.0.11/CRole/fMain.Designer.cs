namespace CRole
{
    partial class fMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.DgvData = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuModule = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.TscbShow = new System.Windows.Forms.ToolStripComboBox();
            this.TsbAppend = new System.Windows.Forms.ToolStripButton();
            this.TsbModify = new System.Windows.Forms.ToolStripButton();
            this.TsbEnabled = new System.Windows.Forms.ToolStripButton();
            this.TsbDisabled = new System.Windows.Forms.ToolStripButton();
            this.TsbDelete = new System.Windows.Forms.ToolStripButton();
            this.Tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.TsbExport = new System.Windows.Forms.ToolStripButton();
            this.TsbModule = new System.Windows.Forms.ToolStripButton();
            this.TsbProcess = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LabFilter = new System.Windows.Forms.Label();
            this.combFilterField = new System.Windows.Forms.ComboBox();
            this.TbFilter = new System.Windows.Forms.TextBox();
            this.combFilter = new System.Windows.Forms.ComboBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.TreeViewSelect = new System.Windows.Forms.TreeView();
            this.popMenu2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.Tss2 = new System.Windows.Forms.ToolStripSeparator();
            this.TsbUser = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.DgvData)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.popMenu2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvData
            // 
            this.DgvData.AllowUserToAddRows = false;
            this.DgvData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.DgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DgvData.BackgroundColor = System.Drawing.Color.White;
            this.DgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvData.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.DgvData, "gvData");
            this.DgvData.MultiSelect = false;
            this.DgvData.Name = "gvData";
            this.DgvData.ReadOnly = true;
            this.DgvData.RowTemplate.Height = 24;
            this.DgvData.VirtualMode = true;
            this.DgvData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvData_CellClick);
            this.DgvData.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.DgvData_CellValueNeeded);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuModule,
            this.toolStripSeparator1,
            this.MenuHistory});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // MenuModule
            // 
            this.MenuModule.Name = "MenuModule";
            resources.ApplyResources(this.MenuModule, "MenuModule");
            this.MenuModule.Click += new System.EventHandler(this.MenuModule_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // MenuHistory
            // 
            this.MenuHistory.Name = "MenuHistory";
            resources.ApplyResources(this.MenuHistory, "MenuHistory");
            this.MenuHistory.Click += new System.EventHandler(this.MenuHistory_Click);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            resources.ApplyResources(this.bindingNavigator1, "bindingNavigator1");
            this.bindingNavigator1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TscbShow,
            this.TsbAppend,
            this.TsbModify,
            this.TsbEnabled,
            this.TsbDisabled,
            this.TsbDelete,
            this.Tss1,
            this.TsbExport,
            this.TsbModule,
            this.Tss2,
            this.TsbUser,
            this.TsbProcess});
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            // 
            // TscbShow
            // 
            this.TscbShow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.TscbShow, "TscbShow");
            this.TscbShow.Items.AddRange(new object[] {
            resources.GetString("TscbShow.Items"),
            resources.GetString("TscbShow.Items1"),
            resources.GetString("TscbShow.Items2")});
            this.TscbShow.Name = "TscbShow";
            this.TscbShow.SelectedIndexChanged += new System.EventHandler(this.TscbShow_SelectedIndexChanged);
            // 
            // TsbAppend
            // 
            resources.ApplyResources(this.TsbAppend, "TsbAppend");
            this.TsbAppend.Name = "TsbAppend";
            this.TsbAppend.Click += new System.EventHandler(this.TsbAppend_Click);
            // 
            // TsbModify
            // 
            resources.ApplyResources(this.TsbModify, "TsbModify");
            this.TsbModify.Name = "TsbModify";
            this.TsbModify.Click += new System.EventHandler(this.TsbModify_Click);
            // 
            // TsbEnabled
            // 
            resources.ApplyResources(this.TsbEnabled, "TsbEnabled");
            this.TsbEnabled.Name = "TsbEnabled";
            this.TsbEnabled.Click += new System.EventHandler(this.TsbDisabled_Click);
            // 
            // TsbDisabled
            // 
            resources.ApplyResources(this.TsbDisabled, "TsbDisabled");
            this.TsbDisabled.Name = "TsbDisabled";
            this.TsbDisabled.Click += new System.EventHandler(this.TsbDisabled_Click);
            // 
            // TsbDelete
            // 
            resources.ApplyResources(this.TsbDelete, "TsbDelete");
            this.TsbDelete.Name = "TsbDelete";
            this.TsbDelete.Click += new System.EventHandler(this.TsbDelete_Click);
            // 
            // Tss1
            // 
            this.Tss1.Name = "Tss1";
            resources.ApplyResources(this.Tss1, "Tss1");
            // 
            // TsbExport
            // 
            resources.ApplyResources(this.TsbExport, "TsbExport");
            this.TsbExport.Name = "TsbExport";
            this.TsbExport.Click += new System.EventHandler(this.TsbExport_Click);
            // 
            // TsbModule
            // 
            resources.ApplyResources(this.TsbModule, "TsbModule");
            this.TsbModule.Name = "TsbModule";
            this.TsbModule.Click += new System.EventHandler(this.MenuModule_Click);
            // 
            // TsbProcess
            // 
            resources.ApplyResources(this.TsbProcess, "TsbProcess");
            this.TsbProcess.Name = "TsbProcess";
            this.TsbProcess.Click += new System.EventHandler(this.TsbProcess_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.LabFilter);
            this.panel1.Controls.Add(this.combFilterField);
            this.panel1.Controls.Add(this.TbFilter);
            this.panel1.Controls.Add(this.combFilter);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // LabFilter
            // 
            resources.ApplyResources(this.LabFilter, "LabFilter");
            this.LabFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LabFilter.Name = "LabFilter";
            // 
            // combFilterField
            // 
            this.combFilterField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combFilterField.FormattingEnabled = true;
            resources.ApplyResources(this.combFilterField, "combFilterField");
            this.combFilterField.Name = "combFilterField";
            // 
            // editFilter
            // 
            resources.ApplyResources(this.TbFilter, "editFilter");
            this.TbFilter.Name = "editFilter";
            this.TbFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbFilter_KeyPress);
            // 
            // combFilter
            // 
            this.combFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combFilter, "combFilter");
            this.combFilter.FormattingEnabled = true;
            this.combFilter.Name = "combFilter";
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
            // 
            // popMenu2
            // 
            this.popMenu2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.popMenu2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseToolStripMenuItem,
            this.expandToolStripMenuItem});
            this.popMenu2.Name = "popMenu2";
            resources.ApplyResources(this.popMenu2, "popMenu2");
            // 
            // collapseToolStripMenuItem
            // 
            this.collapseToolStripMenuItem.Name = "collapseToolStripMenuItem";
            resources.ApplyResources(this.collapseToolStripMenuItem, "collapseToolStripMenuItem");
            this.collapseToolStripMenuItem.Click += new System.EventHandler(this.TsmiCollapse_Click);
            // 
            // expandToolStripMenuItem
            // 
            this.expandToolStripMenuItem.Name = "expandToolStripMenuItem";
            resources.ApplyResources(this.expandToolStripMenuItem, "expandToolStripMenuItem");
            this.expandToolStripMenuItem.Click += new System.EventHandler(this.TsmiExpand_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1.bmp");
            this.imageList1.Images.SetKeyName(1, "2.bmp");
            this.imageList1.Images.SetKeyName(2, "role3.bmp");
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // Tss2
            // 
            this.Tss2.Name = "Tss2";
            resources.ApplyResources(this.Tss2, "Tss2");
            // 
            // TsbUser
            // 
            resources.ApplyResources(this.TsbUser, "TsbUser");
            this.TsbUser.Name = "TsbUser";
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TreeViewSelect);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.DgvData);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bindingNavigator1);
            this.Name = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvData)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.popMenu2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DgvData;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton TsbAppend;
        private System.Windows.Forms.ToolStripButton TsbDelete;
        private System.Windows.Forms.ToolStripSeparator Tss1;
        private System.Windows.Forms.ToolStripButton TsbModify;
        private System.Windows.Forms.ToolStripButton TsbDisabled;
        private System.Windows.Forms.ToolStripButton TsbEnabled;
        private System.Windows.Forms.ToolStripComboBox TscbShow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox TbFilter;
        private System.Windows.Forms.ComboBox combFilter;
        private System.Windows.Forms.ComboBox combFilterField;
        private System.Windows.Forms.ToolStripButton TsbExport;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuHistory;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label LabFilter;
        private System.Windows.Forms.ToolStripMenuItem MenuModule;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton TsbModule;
        private System.Windows.Forms.TreeView TreeViewSelect;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip popMenu2;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton TsbProcess;
        private System.Windows.Forms.ToolStripSeparator Tss2;
        private System.Windows.Forms.ToolStripButton TsbUser;
    }
}