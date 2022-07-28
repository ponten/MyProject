namespace BarCode
{
    partial class fDownLoad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fDownLoad));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnqty = new System.Windows.Forms.Button();
            this.lbBarCode = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnOpFile = new System.Windows.Forms.Button();
            this.btnDLFile = new System.Windows.Forms.Button();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.gvData = new System.Windows.Forms.DataGridView();
            this.gvBarCode = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBarCode)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuHistory});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
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
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtFileName);
            this.panel1.Controls.Add(this.btnqty);
            this.panel1.Controls.Add(this.lbBarCode);
            this.panel1.Controls.Add(this.chkSelectAll);
            this.panel1.Controls.Add(this.txtFile);
            this.panel1.Controls.Add(this.btnOpFile);
            this.panel1.Controls.Add(this.btnDLFile);
            this.panel1.Controls.Add(this.txtTitle);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // txtFileName
            // 
            resources.ApplyResources(this.txtFileName, "txtFileName");
            this.txtFileName.Name = "txtFileName";
            // 
            // btnqty
            // 
            this.btnqty.BackColor = System.Drawing.Color.LightGray;
            resources.ApplyResources(this.btnqty, "btnqty");
            this.btnqty.Name = "btnqty";
            this.btnqty.UseVisualStyleBackColor = false;
            this.btnqty.Click += new System.EventHandler(this.btnqty_Click);
            // 
            // lbBarCode
            // 
            resources.ApplyResources(this.lbBarCode, "lbBarCode");
            this.lbBarCode.BackColor = System.Drawing.Color.Transparent;
            this.lbBarCode.Name = "lbBarCode";
            // 
            // chkSelectAll
            // 
            resources.ApplyResources(this.chkSelectAll, "chkSelectAll");
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // txtFile
            // 
            resources.ApplyResources(this.txtFile, "txtFile");
            this.txtFile.Name = "txtFile";
            // 
            // btnOpFile
            // 
            this.btnOpFile.BackColor = System.Drawing.Color.LightGray;
            resources.ApplyResources(this.btnOpFile, "btnOpFile");
            this.btnOpFile.Name = "btnOpFile";
            this.btnOpFile.UseVisualStyleBackColor = false;
            this.btnOpFile.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // btnDLFile
            // 
            resources.ApplyResources(this.btnDLFile, "btnDLFile");
            this.btnDLFile.Name = "btnDLFile";
            this.btnDLFile.UseVisualStyleBackColor = true;
            this.btnDLFile.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtTitle
            // 
            resources.ApplyResources(this.txtTitle, "txtTitle");
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editFilter_KeyPress);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.VirtualMode = true;
            // 
            // gvData
            // 
            this.gvData.AllowUserToAddRows = false;
            this.gvData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.gvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gvData.BackgroundColor = System.Drawing.Color.White;
            this.gvData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvData.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.gvData, "gvData");
            this.gvData.MultiSelect = false;
            this.gvData.Name = "gvData";
            this.gvData.ReadOnly = true;
            this.gvData.RowTemplate.Height = 24;
            this.gvData.VirtualMode = true;
            // 
            // gvBarCode
            // 
            this.gvBarCode.AllowUserToAddRows = false;
            this.gvBarCode.AllowUserToDeleteRows = false;
            this.gvBarCode.BackgroundColor = System.Drawing.Color.White;
            this.gvBarCode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.gvBarCode, "gvBarCode");
            this.gvBarCode.Name = "gvBarCode";
            this.gvBarCode.RowTemplate.Height = 24;
            // 
            // fDownLoad
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gvBarCode);
            this.Controls.Add(this.gvData);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bindingNavigator1);
            this.Name = "fDownLoad";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBarCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuHistory;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnDLFile;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView gvData;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnOpFile;
        private System.Windows.Forms.DataGridView gvBarCode;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.Label lbBarCode;
        private System.Windows.Forms.Button btnqty;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileName;
    }
}