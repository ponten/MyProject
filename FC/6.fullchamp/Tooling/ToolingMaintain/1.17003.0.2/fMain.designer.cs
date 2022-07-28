namespace ToolingMaintaindll
{
    partial class fMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectALL = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.editMemo = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.labStatus = new System.Windows.Forms.Label();
            this.chkbDays = new System.Windows.Forms.CheckBox();
            this.chkbCount = new System.Windows.Forms.CheckBox();
            this.btnFilterTooling = new System.Windows.Forms.Button();
            this.editToolingNo = new System.Windows.Forms.TextBox();
            this.labToolingNo = new System.Windows.Forms.Label();
            this.combToolingType = new System.Windows.Forms.ComboBox();
            this.labToolingType = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.CHECKED = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TOOLING_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOOLING_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOOLING_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOOLING_DESC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOTAL_USED_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAX_USED_COUNT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAST_MAINTAIN_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOOLING_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COMPANY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USE_COUNT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOOL_LOCATION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "0.bmp");
            this.imageList1.Images.SetKeyName(1, "RouteStage.bmp");
            this.imageList1.Images.SetKeyName(2, "RouteProcess.bmp");
            this.imageList1.Images.SetKeyName(3, "2.bmp");
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dgvData, "dgvData");
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CHECKED,
            this.TOOLING_TYPE,
            this.TOOLING_NO,
            this.TOOLING_NAME,
            this.TOOLING_DESC,
            this.TOTAL_USED_TIME,
            this.MAX_USED_COUNT,
            this.LAST_MAINTAIN_TIME,
            this.STATUS,
            this.TOOLING_ID,
            this.COMPANY,
            this.USE_COUNT,
            this.TOOL_LOCATION});
            this.dgvData.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.TabStop = false;
            this.dgvData.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellDoubleClick);
            this.dgvData.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellValueChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            resources.ApplyResources(this.clearToolStripMenuItem, "clearToolStripMenuItem");
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.panel3);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvData);
            this.groupBox2.Controls.Add(this.panel2);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.btnSelectNone);
            this.panel2.Controls.Add(this.btnSelectALL);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnSelectNone
            // 
            resources.ApplyResources(this.btnSelectNone, "btnSelectNone");
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Tag = "1";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectALL_Click);
            // 
            // btnSelectALL
            // 
            resources.ApplyResources(this.btnSelectALL, "btnSelectALL");
            this.btnSelectALL.Name = "btnSelectALL";
            this.btnSelectALL.Tag = "0";
            this.btnSelectALL.UseVisualStyleBackColor = true;
            this.btnSelectALL.Click += new System.EventHandler(this.btnSelectALL_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.editMemo);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.SteelBlue;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // editMemo
            // 
            this.editMemo.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editMemo, "editMemo");
            this.editMemo.Name = "editMemo";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.btnClear);
            this.panel3.Controls.Add(this.labStatus);
            this.panel3.Controls.Add(this.chkbDays);
            this.panel3.Controls.Add(this.chkbCount);
            this.panel3.Controls.Add(this.btnFilterTooling);
            this.panel3.Controls.Add(this.editToolingNo);
            this.panel3.Controls.Add(this.labToolingNo);
            this.panel3.Controls.Add(this.combToolingType);
            this.panel3.Controls.Add(this.labToolingType);
            this.panel3.Controls.Add(this.btnQuery);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.MediumSeaGreen;
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // labStatus
            // 
            this.labStatus.BackColor = System.Drawing.Color.DodgerBlue;
            resources.ApplyResources(this.labStatus, "labStatus");
            this.labStatus.ForeColor = System.Drawing.Color.White;
            this.labStatus.Name = "labStatus";
            // 
            // chkbDays
            // 
            resources.ApplyResources(this.chkbDays, "chkbDays");
            this.chkbDays.Name = "chkbDays";
            this.chkbDays.UseVisualStyleBackColor = true;
            // 
            // chkbCount
            // 
            resources.ApplyResources(this.chkbCount, "chkbCount");
            this.chkbCount.Name = "chkbCount";
            this.chkbCount.UseVisualStyleBackColor = true;
            // 
            // btnFilterTooling
            // 
            resources.ApplyResources(this.btnFilterTooling, "btnFilterTooling");
            this.btnFilterTooling.Name = "btnFilterTooling";
            this.btnFilterTooling.TabStop = false;
            this.btnFilterTooling.UseVisualStyleBackColor = true;
            this.btnFilterTooling.Click += new System.EventHandler(this.btnFilterTooling_Click);
            // 
            // editToolingNo
            // 
            resources.ApplyResources(this.editToolingNo, "editToolingNo");
            this.editToolingNo.Name = "editToolingNo";
            this.editToolingNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editToolingNo_KeyPress);
            // 
            // labToolingNo
            // 
            resources.ApplyResources(this.labToolingNo, "labToolingNo");
            this.labToolingNo.Name = "labToolingNo";
            // 
            // combToolingType
            // 
            this.combToolingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combToolingType, "combToolingType");
            this.combToolingType.FormattingEnabled = true;
            this.combToolingType.Name = "combToolingType";
            // 
            // labToolingType
            // 
            resources.ApplyResources(this.labToolingType, "labToolingType");
            this.labToolingType.Name = "labToolingType";
            // 
            // btnQuery
            // 
            this.btnQuery.BackColor = System.Drawing.Color.SteelBlue;
            resources.ApplyResources(this.btnQuery, "btnQuery");
            this.btnQuery.ForeColor = System.Drawing.Color.White;
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.UseVisualStyleBackColor = false;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // CHECKED
            // 
            this.CHECKED.FalseValue = "N";
            resources.ApplyResources(this.CHECKED, "CHECKED");
            this.CHECKED.Name = "CHECKED";
            this.CHECKED.TrueValue = "Y";
            // 
            // TOOLING_TYPE
            // 
            resources.ApplyResources(this.TOOLING_TYPE, "TOOLING_TYPE");
            this.TOOLING_TYPE.Name = "TOOLING_TYPE";
            // 
            // TOOLING_NO
            // 
            resources.ApplyResources(this.TOOLING_NO, "TOOLING_NO");
            this.TOOLING_NO.Name = "TOOLING_NO";
            // 
            // TOOLING_NAME
            // 
            resources.ApplyResources(this.TOOLING_NAME, "TOOLING_NAME");
            this.TOOLING_NAME.Name = "TOOLING_NAME";
            // 
            // TOOLING_DESC
            // 
            resources.ApplyResources(this.TOOLING_DESC, "TOOLING_DESC");
            this.TOOLING_DESC.Name = "TOOLING_DESC";
            // 
            // TOTAL_USED_TIME
            // 
            resources.ApplyResources(this.TOTAL_USED_TIME, "TOTAL_USED_TIME");
            this.TOTAL_USED_TIME.Name = "TOTAL_USED_TIME";
            // 
            // MAX_USED_COUNT
            // 
            resources.ApplyResources(this.MAX_USED_COUNT, "MAX_USED_COUNT");
            this.MAX_USED_COUNT.Name = "MAX_USED_COUNT";
            // 
            // LAST_MAINTAIN_TIME
            // 
            resources.ApplyResources(this.LAST_MAINTAIN_TIME, "LAST_MAINTAIN_TIME");
            this.LAST_MAINTAIN_TIME.Name = "LAST_MAINTAIN_TIME";
            // 
            // STATUS
            // 
            resources.ApplyResources(this.STATUS, "STATUS");
            this.STATUS.Name = "STATUS";
            // 
            // TOOLING_ID
            // 
            resources.ApplyResources(this.TOOLING_ID, "TOOLING_ID");
            this.TOOLING_ID.Name = "TOOLING_ID";
            // 
            // COMPANY
            // 
            resources.ApplyResources(this.COMPANY, "COMPANY");
            this.COMPANY.Name = "COMPANY";
            // 
            // USE_COUNT
            // 
            resources.ApplyResources(this.USE_COUNT, "USE_COUNT");
            this.USE_COUNT.Name = "USE_COUNT";
            // 
            // TOOL_LOCATION
            // 
            resources.ApplyResources(this.TOOL_LOCATION, "TOOL_LOCATION");
            this.TOOL_LOCATION.Name = "TOOL_LOCATION";
            this.TOOL_LOCATION.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Name = "fMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnFilterTooling;
        private System.Windows.Forms.TextBox editToolingNo;
        private System.Windows.Forms.Label labToolingNo;
        private System.Windows.Forms.ComboBox combToolingType;
        private System.Windows.Forms.Label labToolingType;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.CheckBox chkbDays;
        private System.Windows.Forms.CheckBox chkbCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editMemo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnSelectALL;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOOL_LOCATION;
        private System.Windows.Forms.DataGridViewTextBoxColumn USE_COUNT;
        private System.Windows.Forms.DataGridViewTextBoxColumn COMPANY;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOOLING_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn STATUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAST_MAINTAIN_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAX_USED_COUNT;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOTAL_USED_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOOLING_DESC;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOOLING_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOOLING_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOOLING_TYPE;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CHECKED;
    }
}
