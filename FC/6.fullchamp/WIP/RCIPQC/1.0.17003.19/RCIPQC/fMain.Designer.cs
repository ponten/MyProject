namespace RCIPQC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tsDetial = new System.Windows.Forms.ToolStripMenuItem();
            this.tsDll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tsEmp = new System.Windows.Forms.ToolStripLabel();
            this.tslabelFormerNO = new System.Windows.Forms.ToolStripLabel();
            this.tsLabelBluePrint = new System.Windows.Forms.ToolStripLabel();
            this.cmbParam = new System.Windows.Forms.ToolStripLabel();
            this.txtInput = new System.Windows.Forms.ToolStripTextBox();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.lblProcess = new System.Windows.Forms.ToolStripLabel();
            this.btnProcessSet = new System.Windows.Forms.ToolStripButton();
            this.cmsTabpage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tabPanelControl = new System.Windows.Forms.TabControl();
            this.TpParams = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gvCondition = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gvInput = new System.Windows.Forms.DataGridView();
            this.TpQCItems = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.DgvQCItem = new System.Windows.Forms.DataGridView();
            this.LbQCInfo = new System.Windows.Forms.Label();
            this.CbQCItem = new System.Windows.Forms.CheckBox();
            this.TpMachine = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnMachineDown = new System.Windows.Forms.Button();
            this.gvMachine = new System.Windows.Forms.DataGridView();
            this.selectDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.mACHINEIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mACHINECODEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mACHINEDESCDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTATUSNAMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tYPEIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rEASONIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTARTTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eNDTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lOADQTYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dATECODEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTOVESEQDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rEMARKDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machineDownModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TpDefect = new System.Windows.Forms.TabPage();
            this.gvDefect = new System.Windows.Forms.DataGridView();
            this.TpQCMark = new System.Windows.Forms.TabPage();
            this.Lb_Status = new System.Windows.Forms.Label();
            this.BtnMark_NG = new System.Windows.Forms.Button();
            this.DtpInspectTime = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnMark_OK = new System.Windows.Forms.Button();
            this.LbInspector = new System.Windows.Forms.Label();
            this.TpRework = new System.Windows.Forms.TabPage();
            this.TbRework = new System.Windows.Forms.TextBox();
            this.LbRework = new System.Windows.Forms.Label();
            this.TpBonus = new System.Windows.Forms.TabPage();
            this.txtWorkHour = new System.Windows.Forms.TextBox();
            this.lblWorkHour = new System.Windows.Forms.Label();
            this.txtBonus = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TpOutTime = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpOutDate = new System.Windows.Forms.DateTimePicker();
            this.TpSN = new System.Windows.Forms.TabPage();
            this.gvSN = new System.Windows.Forms.DataGridView();
            this.TpKeyparts = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gvBOM = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.gvKeypart = new System.Windows.Forms.DataGridView();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.editKPSN = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.editCount = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.btnAppend = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tsMemo = new System.Windows.Forms.TextBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.cmsTabpage.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPanelControl.SuspendLayout();
            this.TpParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvCondition)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvInput)).BeginInit();
            this.TpQCItems.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvQCItem)).BeginInit();
            this.TpMachine.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDownModelBindingSource)).BeginInit();
            this.TpDefect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDefect)).BeginInit();
            this.TpQCMark.SuspendLayout();
            this.TpRework.SuspendLayout();
            this.TpBonus.SuspendLayout();
            this.TpOutTime.SuspendLayout();
            this.TpSN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSN)).BeginInit();
            this.TpKeyparts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvBOM)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvKeypart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "mouse.gif");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(144, 48);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 243F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(780, 10);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // tsDetial
            // 
            this.tsDetial.Name = "tsDetial";
            this.tsDetial.Size = new System.Drawing.Size(116, 22);
            this.tsDetial.Text = "tsDetail";
            // 
            // tsDll
            // 
            this.tsDll.Name = "tsDll";
            this.tsDll.Size = new System.Drawing.Size(116, 22);
            this.tsDll.Text = "tsDll";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.tsEmp,
            this.tslabelFormerNO,
            this.tsLabelBluePrint,
            this.cmbParam,
            this.txtInput,
            this.btnSearch,
            this.btnClear,
            this.lblProcess,
            this.btnProcessSet});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 25);
            this.toolStrip1.TabIndex = 37;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(63, 22);
            this.toolStripLabel2.Text = "Employee";
            // 
            // tsEmp
            // 
            this.tsEmp.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tsEmp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.tsEmp.Name = "tsEmp";
            this.tsEmp.Size = new System.Drawing.Size(0, 22);
            // 
            // tslabelFormerNO
            // 
            this.tslabelFormerNO.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Bold);
            this.tslabelFormerNO.ForeColor = System.Drawing.Color.Red;
            this.tslabelFormerNO.Margin = new System.Windows.Forms.Padding(20, 1, 0, 2);
            this.tslabelFormerNO.Name = "tslabelFormerNO";
            this.tslabelFormerNO.Size = new System.Drawing.Size(0, 22);
            // 
            // tsLabelBluePrint
            // 
            this.tsLabelBluePrint.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Bold);
            this.tsLabelBluePrint.ForeColor = System.Drawing.Color.Red;
            this.tsLabelBluePrint.Margin = new System.Windows.Forms.Padding(20, 1, 0, 2);
            this.tsLabelBluePrint.Name = "tsLabelBluePrint";
            this.tsLabelBluePrint.Size = new System.Drawing.Size(0, 22);
            // 
            // cmbParam
            // 
            this.cmbParam.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cmbParam.Name = "cmbParam";
            this.cmbParam.Size = new System.Drawing.Size(46, 22);
            this.cmbParam.Text = "RC No";
            // 
            // txtInput
            // 
            this.txtInput.BackColor = System.Drawing.SystemColors.Info;
            this.txtInput.Enabled = false;
            this.txtInput.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.txtInput.Name = "txtInput";
            this.txtInput.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.txtInput.Size = new System.Drawing.Size(210, 25);
            this.txtInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtInput_KeyPress);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSearch.Enabled = false;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnSearch.Size = new System.Drawing.Size(51, 22);
            this.btnSearch.Text = "Query";
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnClear.Enabled = false;
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnClear.Size = new System.Drawing.Size(56, 22);
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // lblProcess
            // 
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.lblProcess.Size = new System.Drawing.Size(74, 22);
            this.lblProcess.Text = "Process";
            // 
            // btnProcessSet
            // 
            this.btnProcessSet.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnProcessSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnProcessSet.Image = ((System.Drawing.Image)(resources.GetObject("btnProcessSet.Image")));
            this.btnProcessSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProcessSet.Name = "btnProcessSet";
            this.btnProcessSet.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.btnProcessSet.Size = new System.Drawing.Size(106, 22);
            this.btnProcessSet.Text = "Process Set";
            this.btnProcessSet.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // cmsTabpage
            // 
            this.cmsTabpage.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsTabpage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsDll,
            this.tsDetial});
            this.cmsTabpage.Name = "cmsTabpage";
            this.cmsTabpage.Size = new System.Drawing.Size(117, 48);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 301F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(784, 29);
            this.tableLayoutPanel2.TabIndex = 41;
            // 
            // tabPanelControl
            // 
            this.tabPanelControl.Controls.Add(this.TpParams);
            this.tabPanelControl.Controls.Add(this.TpQCItems);
            this.tabPanelControl.Controls.Add(this.TpMachine);
            this.tabPanelControl.Controls.Add(this.TpDefect);
            this.tabPanelControl.Controls.Add(this.TpQCMark);
            this.tabPanelControl.Controls.Add(this.TpRework);
            this.tabPanelControl.Controls.Add(this.TpBonus);
            this.tabPanelControl.Controls.Add(this.TpOutTime);
            this.tabPanelControl.Controls.Add(this.TpSN);
            this.tabPanelControl.Controls.Add(this.TpKeyparts);
            this.tabPanelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPanelControl.Enabled = false;
            this.tabPanelControl.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabPanelControl.Location = new System.Drawing.Point(0, 54);
            this.tabPanelControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabPanelControl.Name = "tabPanelControl";
            this.tabPanelControl.SelectedIndex = 0;
            this.tabPanelControl.Size = new System.Drawing.Size(784, 461);
            this.tabPanelControl.TabIndex = 42;
            // 
            // TpParams
            // 
            this.TpParams.Controls.Add(this.splitContainer1);
            this.TpParams.Location = new System.Drawing.Point(4, 26);
            this.TpParams.Name = "TpParams";
            this.TpParams.Size = new System.Drawing.Size(776, 431);
            this.TpParams.TabIndex = 0;
            this.TpParams.Text = "Process Params";
            this.TpParams.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(776, 431);
            this.splitContainer1.SplitterDistance = 365;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gvCondition);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 431);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Condition";
            // 
            // gvCondition
            // 
            this.gvCondition.AllowUserToAddRows = false;
            this.gvCondition.AllowUserToDeleteRows = false;
            this.gvCondition.BackgroundColor = System.Drawing.Color.White;
            this.gvCondition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvCondition.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvCondition.Location = new System.Drawing.Point(3, 23);
            this.gvCondition.Name = "gvCondition";
            this.gvCondition.ReadOnly = true;
            this.gvCondition.RowHeadersWidth = 25;
            this.gvCondition.RowTemplate.Height = 24;
            this.gvCondition.Size = new System.Drawing.Size(359, 405);
            this.gvCondition.TabIndex = 3;
            this.gvCondition.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvCondition_CellClick);
            this.gvCondition.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.GvCondition_DataBindingComplete);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gvInput);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(407, 431);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Collection";
            // 
            // gvInput
            // 
            this.gvInput.AllowUserToAddRows = false;
            this.gvInput.AllowUserToDeleteRows = false;
            this.gvInput.BackgroundColor = System.Drawing.Color.White;
            this.gvInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvInput.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvInput.Location = new System.Drawing.Point(3, 23);
            this.gvInput.Name = "gvInput";
            this.gvInput.RowHeadersWidth = 25;
            this.gvInput.RowTemplate.Height = 24;
            this.gvInput.Size = new System.Drawing.Size(401, 405);
            this.gvInput.TabIndex = 4;
            this.gvInput.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvInput_CellEndEdit);
            // 
            // TpQCItems
            // 
            this.TpQCItems.Controls.Add(this.tableLayoutPanel5);
            this.TpQCItems.Location = new System.Drawing.Point(4, 26);
            this.TpQCItems.Name = "TpQCItems";
            this.TpQCItems.Size = new System.Drawing.Size(776, 431);
            this.TpQCItems.TabIndex = 9;
            this.TpQCItems.Text = "QC Params";
            this.TpQCItems.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.DgvQCItem, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.LbQCInfo, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.CbQCItem, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(776, 431);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // DgvQCItem
            // 
            this.DgvQCItem.AllowUserToAddRows = false;
            this.DgvQCItem.AllowUserToDeleteRows = false;
            this.DgvQCItem.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DgvQCItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel5.SetColumnSpan(this.DgvQCItem, 3);
            this.DgvQCItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvQCItem.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvQCItem.Location = new System.Drawing.Point(3, 29);
            this.DgvQCItem.MultiSelect = false;
            this.DgvQCItem.Name = "DgvQCItem";
            this.DgvQCItem.RowTemplate.Height = 24;
            this.DgvQCItem.Size = new System.Drawing.Size(770, 399);
            this.DgvQCItem.TabIndex = 0;
            // 
            // LbQCInfo
            // 
            this.LbQCInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LbQCInfo.AutoSize = true;
            this.LbQCInfo.Location = new System.Drawing.Point(160, 5);
            this.LbQCInfo.Margin = new System.Windows.Forms.Padding(3);
            this.LbQCInfo.Name = "LbQCInfo";
            this.LbQCInfo.Size = new System.Drawing.Size(18, 16);
            this.LbQCInfo.TabIndex = 1;
            this.LbQCInfo.Text = "--";
            // 
            // CbQCItem
            // 
            this.CbQCItem.AutoSize = true;
            this.CbQCItem.Location = new System.Drawing.Point(3, 3);
            this.CbQCItem.Name = "CbQCItem";
            this.CbQCItem.Size = new System.Drawing.Size(101, 20);
            this.CbQCItem.TabIndex = 2;
            this.CbQCItem.Text = "Collect data";
            this.CbQCItem.UseVisualStyleBackColor = true;
            // 
            // TpMachine
            // 
            this.TpMachine.BackColor = System.Drawing.SystemColors.Control;
            this.TpMachine.Controls.Add(this.tableLayoutPanel4);
            this.TpMachine.Location = new System.Drawing.Point(4, 26);
            this.TpMachine.Name = "TpMachine";
            this.TpMachine.Size = new System.Drawing.Size(776, 431);
            this.TpMachine.TabIndex = 1;
            this.TpMachine.Text = "Machine";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.BtnMachineDown, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.gvMachine, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(776, 431);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // BtnMachineDown
            // 
            this.BtnMachineDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnMachineDown.Font = new System.Drawing.Font("新細明體", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnMachineDown.Location = new System.Drawing.Point(298, 10);
            this.BtnMachineDown.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.BtnMachineDown.Name = "BtnMachineDown";
            this.BtnMachineDown.Size = new System.Drawing.Size(180, 30);
            this.BtnMachineDown.TabIndex = 5;
            this.BtnMachineDown.Text = "Machine Change";
            this.BtnMachineDown.UseVisualStyleBackColor = true;
            this.BtnMachineDown.Click += new System.EventHandler(this.BtnMachineDown_Click);
            // 
            // gvMachine
            // 
            this.gvMachine.AllowUserToAddRows = false;
            this.gvMachine.AllowUserToDeleteRows = false;
            this.gvMachine.AllowUserToResizeColumns = false;
            this.gvMachine.AllowUserToResizeRows = false;
            this.gvMachine.AutoGenerateColumns = false;
            this.gvMachine.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvMachine.BackgroundColor = System.Drawing.Color.White;
            this.gvMachine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMachine.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectDataGridViewCheckBoxColumn,
            this.mACHINEIDDataGridViewTextBoxColumn,
            this.mACHINECODEDataGridViewTextBoxColumn,
            this.mACHINEDESCDataGridViewTextBoxColumn,
            this.sTATUSNAMEDataGridViewTextBoxColumn,
            this.tYPEIDDataGridViewTextBoxColumn,
            this.rEASONIDDataGridViewTextBoxColumn,
            this.sTARTTIMEDataGridViewTextBoxColumn,
            this.eNDTIMEDataGridViewTextBoxColumn,
            this.lOADQTYDataGridViewTextBoxColumn,
            this.dATECODEDataGridViewTextBoxColumn,
            this.sTOVESEQDataGridViewTextBoxColumn,
            this.rEMARKDataGridViewTextBoxColumn});
            this.tableLayoutPanel4.SetColumnSpan(this.gvMachine, 3);
            this.gvMachine.DataSource = this.machineDownModelBindingSource;
            this.gvMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvMachine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvMachine.Location = new System.Drawing.Point(3, 53);
            this.gvMachine.Name = "gvMachine";
            this.gvMachine.RowHeadersWidth = 25;
            this.gvMachine.RowTemplate.Height = 24;
            this.gvMachine.Size = new System.Drawing.Size(770, 375);
            this.gvMachine.TabIndex = 4;
            // 
            // selectDataGridViewCheckBoxColumn
            // 
            this.selectDataGridViewCheckBoxColumn.DataPropertyName = "Select";
            this.selectDataGridViewCheckBoxColumn.HeaderText = "Select";
            this.selectDataGridViewCheckBoxColumn.Name = "selectDataGridViewCheckBoxColumn";
            this.selectDataGridViewCheckBoxColumn.ReadOnly = true;
            this.selectDataGridViewCheckBoxColumn.Visible = false;
            this.selectDataGridViewCheckBoxColumn.Width = 38;
            // 
            // mACHINEIDDataGridViewTextBoxColumn
            // 
            this.mACHINEIDDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_ID";
            this.mACHINEIDDataGridViewTextBoxColumn.HeaderText = "MACHINE_ID";
            this.mACHINEIDDataGridViewTextBoxColumn.Name = "mACHINEIDDataGridViewTextBoxColumn";
            this.mACHINEIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINEIDDataGridViewTextBoxColumn.Visible = false;
            this.mACHINEIDDataGridViewTextBoxColumn.Width = 101;
            // 
            // mACHINECODEDataGridViewTextBoxColumn
            // 
            this.mACHINECODEDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_CODE";
            this.mACHINECODEDataGridViewTextBoxColumn.HeaderText = "Machine code";
            this.mACHINECODEDataGridViewTextBoxColumn.Name = "mACHINECODEDataGridViewTextBoxColumn";
            this.mACHINECODEDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINECODEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.mACHINECODEDataGridViewTextBoxColumn.Width = 92;
            // 
            // mACHINEDESCDataGridViewTextBoxColumn
            // 
            this.mACHINEDESCDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_DESC";
            this.mACHINEDESCDataGridViewTextBoxColumn.HeaderText = "Machine";
            this.mACHINEDESCDataGridViewTextBoxColumn.Name = "mACHINEDESCDataGridViewTextBoxColumn";
            this.mACHINEDESCDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINEDESCDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.mACHINEDESCDataGridViewTextBoxColumn.Width = 68;
            // 
            // sTATUSNAMEDataGridViewTextBoxColumn
            // 
            this.sTATUSNAMEDataGridViewTextBoxColumn.DataPropertyName = "STATUS_NAME";
            this.sTATUSNAMEDataGridViewTextBoxColumn.HeaderText = "STATUS_NAME";
            this.sTATUSNAMEDataGridViewTextBoxColumn.Name = "sTATUSNAMEDataGridViewTextBoxColumn";
            this.sTATUSNAMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTATUSNAMEDataGridViewTextBoxColumn.Visible = false;
            this.sTATUSNAMEDataGridViewTextBoxColumn.Width = 111;
            // 
            // tYPEIDDataGridViewTextBoxColumn
            // 
            this.tYPEIDDataGridViewTextBoxColumn.DataPropertyName = "TYPE_ID";
            this.tYPEIDDataGridViewTextBoxColumn.HeaderText = "TYPE_ID";
            this.tYPEIDDataGridViewTextBoxColumn.Name = "tYPEIDDataGridViewTextBoxColumn";
            this.tYPEIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.tYPEIDDataGridViewTextBoxColumn.Visible = false;
            this.tYPEIDDataGridViewTextBoxColumn.Width = 76;
            // 
            // rEASONIDDataGridViewTextBoxColumn
            // 
            this.rEASONIDDataGridViewTextBoxColumn.DataPropertyName = "REASON_ID";
            this.rEASONIDDataGridViewTextBoxColumn.HeaderText = "REASON_ID";
            this.rEASONIDDataGridViewTextBoxColumn.Name = "rEASONIDDataGridViewTextBoxColumn";
            this.rEASONIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.rEASONIDDataGridViewTextBoxColumn.Visible = false;
            this.rEASONIDDataGridViewTextBoxColumn.Width = 93;
            // 
            // sTARTTIMEDataGridViewTextBoxColumn
            // 
            this.sTARTTIMEDataGridViewTextBoxColumn.DataPropertyName = "START_TIME";
            this.sTARTTIMEDataGridViewTextBoxColumn.HeaderText = "Start time";
            this.sTARTTIMEDataGridViewTextBoxColumn.Name = "sTARTTIMEDataGridViewTextBoxColumn";
            this.sTARTTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTARTTIMEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sTARTTIMEDataGridViewTextBoxColumn.Width = 66;
            // 
            // eNDTIMEDataGridViewTextBoxColumn
            // 
            this.eNDTIMEDataGridViewTextBoxColumn.DataPropertyName = "END_TIME";
            this.eNDTIMEDataGridViewTextBoxColumn.HeaderText = "END_TIME";
            this.eNDTIMEDataGridViewTextBoxColumn.Name = "eNDTIMEDataGridViewTextBoxColumn";
            this.eNDTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.eNDTIMEDataGridViewTextBoxColumn.Visible = false;
            this.eNDTIMEDataGridViewTextBoxColumn.Width = 87;
            // 
            // lOADQTYDataGridViewTextBoxColumn
            // 
            this.lOADQTYDataGridViewTextBoxColumn.DataPropertyName = "LOAD_QTY";
            this.lOADQTYDataGridViewTextBoxColumn.HeaderText = "Load";
            this.lOADQTYDataGridViewTextBoxColumn.Name = "lOADQTYDataGridViewTextBoxColumn";
            this.lOADQTYDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.lOADQTYDataGridViewTextBoxColumn.Width = 46;
            // 
            // dATECODEDataGridViewTextBoxColumn
            // 
            this.dATECODEDataGridViewTextBoxColumn.DataPropertyName = "DATE_CODE";
            this.dATECODEDataGridViewTextBoxColumn.HeaderText = "DateCode";
            this.dATECODEDataGridViewTextBoxColumn.Name = "dATECODEDataGridViewTextBoxColumn";
            this.dATECODEDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dATECODEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dATECODEDataGridViewTextBoxColumn.Width = 76;
            // 
            // sTOVESEQDataGridViewTextBoxColumn
            // 
            this.sTOVESEQDataGridViewTextBoxColumn.DataPropertyName = "STOVE_SEQ";
            this.sTOVESEQDataGridViewTextBoxColumn.HeaderText = "Stove sequence";
            this.sTOVESEQDataGridViewTextBoxColumn.Name = "sTOVESEQDataGridViewTextBoxColumn";
            this.sTOVESEQDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // rEMARKDataGridViewTextBoxColumn
            // 
            this.rEMARKDataGridViewTextBoxColumn.DataPropertyName = "REMARK";
            this.rEMARKDataGridViewTextBoxColumn.HeaderText = "REMARK";
            this.rEMARKDataGridViewTextBoxColumn.Name = "rEMARKDataGridViewTextBoxColumn";
            this.rEMARKDataGridViewTextBoxColumn.ReadOnly = true;
            this.rEMARKDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.rEMARKDataGridViewTextBoxColumn.Visible = false;
            this.rEMARKDataGridViewTextBoxColumn.Width = 79;
            // 
            // machineDownModelBindingSource
            // 
            this.machineDownModelBindingSource.DataSource = typeof(RCIPQC.References.MachineDownModel);
            // 
            // TpDefect
            // 
            this.TpDefect.Controls.Add(this.gvDefect);
            this.TpDefect.Location = new System.Drawing.Point(4, 26);
            this.TpDefect.Name = "TpDefect";
            this.TpDefect.Size = new System.Drawing.Size(776, 431);
            this.TpDefect.TabIndex = 4;
            this.TpDefect.Text = "Defect";
            this.TpDefect.UseVisualStyleBackColor = true;
            // 
            // gvDefect
            // 
            this.gvDefect.AllowUserToAddRows = false;
            this.gvDefect.AllowUserToDeleteRows = false;
            this.gvDefect.BackgroundColor = System.Drawing.Color.White;
            this.gvDefect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvDefect.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvDefect.Location = new System.Drawing.Point(0, 0);
            this.gvDefect.Name = "gvDefect";
            this.gvDefect.RowHeadersWidth = 25;
            this.gvDefect.RowTemplate.Height = 24;
            this.gvDefect.Size = new System.Drawing.Size(776, 431);
            this.gvDefect.TabIndex = 5;
            this.gvDefect.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvDefect_CellValueChanged);
            // 
            // TpQCMark
            // 
            this.TpQCMark.BackColor = System.Drawing.SystemColors.Control;
            this.TpQCMark.Controls.Add(this.Lb_Status);
            this.TpQCMark.Controls.Add(this.BtnMark_NG);
            this.TpQCMark.Controls.Add(this.DtpInspectTime);
            this.TpQCMark.Controls.Add(this.label5);
            this.TpQCMark.Controls.Add(this.label3);
            this.TpQCMark.Controls.Add(this.BtnMark_OK);
            this.TpQCMark.Controls.Add(this.LbInspector);
            this.TpQCMark.Location = new System.Drawing.Point(4, 26);
            this.TpQCMark.Name = "TpQCMark";
            this.TpQCMark.Size = new System.Drawing.Size(776, 431);
            this.TpQCMark.TabIndex = 10;
            this.TpQCMark.Text = "Inspector mark";
            // 
            // Lb_Status
            // 
            this.Lb_Status.AutoSize = true;
            this.Lb_Status.Location = new System.Drawing.Point(37, 160);
            this.Lb_Status.Name = "Lb_Status";
            this.Lb_Status.Size = new System.Drawing.Size(18, 16);
            this.Lb_Status.TabIndex = 7;
            this.Lb_Status.Text = "--";
            // 
            // BtnMark_NG
            // 
            this.BtnMark_NG.Location = new System.Drawing.Point(268, 225);
            this.BtnMark_NG.Name = "BtnMark_NG";
            this.BtnMark_NG.Size = new System.Drawing.Size(200, 45);
            this.BtnMark_NG.TabIndex = 6;
            this.BtnMark_NG.Text = "INSPECT_NG";
            this.BtnMark_NG.UseVisualStyleBackColor = true;
            // 
            // DtpInspectTime
            // 
            this.DtpInspectTime.CustomFormat = "yyyy/ MM/ dd HH: mm: ss";
            this.DtpInspectTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpInspectTime.Location = new System.Drawing.Point(200, 93);
            this.DtpInspectTime.Name = "DtpInspectTime";
            this.DtpInspectTime.Size = new System.Drawing.Size(250, 27);
            this.DtpInspectTime.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Inspect time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Inspector";
            // 
            // BtnMark_OK
            // 
            this.BtnMark_OK.Location = new System.Drawing.Point(40, 225);
            this.BtnMark_OK.Name = "BtnMark_OK";
            this.BtnMark_OK.Size = new System.Drawing.Size(200, 45);
            this.BtnMark_OK.TabIndex = 2;
            this.BtnMark_OK.Text = "INSPECT_OK";
            this.BtnMark_OK.UseVisualStyleBackColor = true;
            // 
            // LbInspector
            // 
            this.LbInspector.AutoSize = true;
            this.LbInspector.Location = new System.Drawing.Point(200, 40);
            this.LbInspector.Name = "LbInspector";
            this.LbInspector.Size = new System.Drawing.Size(18, 16);
            this.LbInspector.TabIndex = 0;
            this.LbInspector.Text = "--";
            // 
            // TpRework
            // 
            this.TpRework.BackColor = System.Drawing.SystemColors.Control;
            this.TpRework.Controls.Add(this.TbRework);
            this.TpRework.Controls.Add(this.LbRework);
            this.TpRework.Location = new System.Drawing.Point(4, 26);
            this.TpRework.Name = "TpRework";
            this.TpRework.Size = new System.Drawing.Size(776, 431);
            this.TpRework.TabIndex = 8;
            this.TpRework.Text = "Rework quantity";
            // 
            // TbRework
            // 
            this.TbRework.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TbRework.Location = new System.Drawing.Point(150, 37);
            this.TbRework.Name = "TbRework";
            this.TbRework.Size = new System.Drawing.Size(150, 27);
            this.TbRework.TabIndex = 1;
            this.TbRework.Click += new System.EventHandler(this.TbRework_Click);
            this.TbRework.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbRework_KeyPress);
            // 
            // LbRework
            // 
            this.LbRework.AutoSize = true;
            this.LbRework.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbRework.Location = new System.Drawing.Point(40, 40);
            this.LbRework.Name = "LbRework";
            this.LbRework.Size = new System.Drawing.Size(112, 16);
            this.LbRework.TabIndex = 0;
            this.LbRework.Text = "Rework quantity";
            // 
            // TpBonus
            // 
            this.TpBonus.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TpBonus.Controls.Add(this.txtWorkHour);
            this.TpBonus.Controls.Add(this.lblWorkHour);
            this.TpBonus.Controls.Add(this.txtBonus);
            this.TpBonus.Controls.Add(this.label2);
            this.TpBonus.Location = new System.Drawing.Point(4, 26);
            this.TpBonus.Name = "TpBonus";
            this.TpBonus.Size = new System.Drawing.Size(776, 431);
            this.TpBonus.TabIndex = 5;
            this.TpBonus.Text = "Bonus / Work Hour";
            // 
            // txtWorkHour
            // 
            this.txtWorkHour.Location = new System.Drawing.Point(140, 97);
            this.txtWorkHour.Name = "txtWorkHour";
            this.txtWorkHour.Size = new System.Drawing.Size(100, 27);
            this.txtWorkHour.TabIndex = 3;
            this.txtWorkHour.Text = "1";
            this.txtWorkHour.Visible = false;
            // 
            // lblWorkHour
            // 
            this.lblWorkHour.AutoSize = true;
            this.lblWorkHour.Location = new System.Drawing.Point(40, 100);
            this.lblWorkHour.Name = "lblWorkHour";
            this.lblWorkHour.Size = new System.Drawing.Size(79, 16);
            this.lblWorkHour.TabIndex = 2;
            this.lblWorkHour.Text = "Work Hour";
            this.lblWorkHour.Visible = false;
            // 
            // txtBonus
            // 
            this.txtBonus.Location = new System.Drawing.Point(140, 37);
            this.txtBonus.Name = "txtBonus";
            this.txtBonus.Size = new System.Drawing.Size(100, 27);
            this.txtBonus.TabIndex = 1;
            this.txtBonus.Text = "0";
            this.txtBonus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBonus_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Bonus";
            // 
            // TpOutTime
            // 
            this.TpOutTime.BackColor = System.Drawing.Color.Transparent;
            this.TpOutTime.Controls.Add(this.label4);
            this.TpOutTime.Controls.Add(this.dtpOutDate);
            this.TpOutTime.Location = new System.Drawing.Point(4, 26);
            this.TpOutTime.Name = "TpOutTime";
            this.TpOutTime.Padding = new System.Windows.Forms.Padding(3);
            this.TpOutTime.Size = new System.Drawing.Size(776, 431);
            this.TpOutTime.TabIndex = 7;
            this.TpOutTime.Text = "Report Time";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(40, 40);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Report time";
            // 
            // dtpOutDate
            // 
            this.dtpOutDate.Checked = false;
            this.dtpOutDate.CustomFormat = "yyyy/ MM/ dd HH: mm: ss";
            this.dtpOutDate.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dtpOutDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpOutDate.Location = new System.Drawing.Point(180, 33);
            this.dtpOutDate.Margin = new System.Windows.Forms.Padding(2);
            this.dtpOutDate.Name = "dtpOutDate";
            this.dtpOutDate.ShowCheckBox = true;
            this.dtpOutDate.Size = new System.Drawing.Size(250, 27);
            this.dtpOutDate.TabIndex = 0;
            this.dtpOutDate.Value = new System.DateTime(2020, 10, 15, 0, 0, 0, 0);
            // 
            // TpSN
            // 
            this.TpSN.Controls.Add(this.gvSN);
            this.TpSN.Location = new System.Drawing.Point(4, 26);
            this.TpSN.Name = "TpSN";
            this.TpSN.Size = new System.Drawing.Size(776, 431);
            this.TpSN.TabIndex = 3;
            this.TpSN.Text = "SN";
            this.TpSN.UseVisualStyleBackColor = true;
            // 
            // gvSN
            // 
            this.gvSN.AllowUserToAddRows = false;
            this.gvSN.AllowUserToDeleteRows = false;
            this.gvSN.BackgroundColor = System.Drawing.Color.White;
            this.gvSN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvSN.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvSN.Location = new System.Drawing.Point(0, 0);
            this.gvSN.Name = "gvSN";
            this.gvSN.RowHeadersWidth = 25;
            this.gvSN.RowTemplate.Height = 24;
            this.gvSN.Size = new System.Drawing.Size(776, 431);
            this.gvSN.TabIndex = 4;
            this.gvSN.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvSN_CellEndEdit);
            // 
            // TpKeyparts
            // 
            this.TpKeyparts.Controls.Add(this.splitContainer2);
            this.TpKeyparts.Controls.Add(this.bindingNavigator1);
            this.TpKeyparts.Location = new System.Drawing.Point(4, 26);
            this.TpKeyparts.Name = "TpKeyparts";
            this.TpKeyparts.Padding = new System.Windows.Forms.Padding(3);
            this.TpKeyparts.Size = new System.Drawing.Size(776, 431);
            this.TpKeyparts.TabIndex = 6;
            this.TpKeyparts.Text = "Keyparts Collection";
            this.TpKeyparts.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 43);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.AutoScroll = true;
            this.splitContainer2.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer2.Size = new System.Drawing.Size(770, 385);
            this.splitContainer2.SplitterDistance = 396;
            this.splitContainer2.TabIndex = 10;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gvBOM);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(396, 385);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "BOM";
            // 
            // gvBOM
            // 
            this.gvBOM.AllowUserToAddRows = false;
            this.gvBOM.AllowUserToDeleteRows = false;
            this.gvBOM.BackgroundColor = System.Drawing.Color.White;
            this.gvBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvBOM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvBOM.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvBOM.Location = new System.Drawing.Point(3, 23);
            this.gvBOM.Name = "gvBOM";
            this.gvBOM.ReadOnly = true;
            this.gvBOM.RowHeadersWidth = 25;
            this.gvBOM.RowTemplate.Height = 24;
            this.gvBOM.Size = new System.Drawing.Size(390, 359);
            this.gvBOM.TabIndex = 3;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.gvKeypart);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(370, 385);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Keyparts List";
            // 
            // gvKeypart
            // 
            this.gvKeypart.AllowUserToAddRows = false;
            this.gvKeypart.AllowUserToDeleteRows = false;
            this.gvKeypart.BackgroundColor = System.Drawing.Color.White;
            this.gvKeypart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvKeypart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvKeypart.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvKeypart.Location = new System.Drawing.Point(3, 23);
            this.gvKeypart.Name = "gvKeypart";
            this.gvKeypart.RowHeadersWidth = 25;
            this.gvKeypart.RowTemplate.Height = 24;
            this.gvKeypart.Size = new System.Drawing.Size(364, 359);
            this.gvKeypart.TabIndex = 4;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.bindingNavigator1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorSeparator,
            this.toolStripLabel1,
            this.toolStripLabel3,
            this.editKPSN,
            this.toolStripLabel4,
            this.toolStripLabel5,
            this.toolStripLabel6,
            this.editCount,
            this.toolStripLabel7,
            this.btnAppend,
            this.btnDelete});
            this.bindingNavigator1.Location = new System.Drawing.Point(3, 3);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(770, 40);
            this.bindingNavigator1.TabIndex = 8;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 40);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(64, 37);
            this.toolStripLabel1.Text = "Keypart SN";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(13, 37);
            this.toolStripLabel3.Text = "  ";
            // 
            // editKPSN
            // 
            this.editKPSN.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.editKPSN.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.editKPSN.Name = "editKPSN";
            this.editKPSN.Size = new System.Drawing.Size(150, 40);
            this.editKPSN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditKPSN_KeyPress);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(40, 37);
            this.toolStripLabel4.Text = "           ";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(61, 37);
            this.toolStripLabel5.Text = "Item Count";
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(13, 37);
            this.toolStripLabel6.Text = "  ";
            // 
            // editCount
            // 
            this.editCount.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.editCount.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.editCount.Name = "editCount";
            this.editCount.Size = new System.Drawing.Size(100, 40);
            this.editCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditCount_KeyPress);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(40, 37);
            this.toolStripLabel7.Text = "           ";
            // 
            // btnAppend
            // 
            this.btnAppend.Image = ((System.Drawing.Image)(resources.GetObject("btnAppend.Image")));
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.RightToLeftAutoMirrorImage = true;
            this.btnAppend.Size = new System.Drawing.Size(49, 37);
            this.btnAppend.Text = "Append";
            this.btnAppend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAppend.Click += new System.EventHandler(this.BtnAppend_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.RightToLeftAutoMirrorImage = true;
            this.btnDelete.Size = new System.Drawing.Size(41, 37);
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tsMemo, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.BtnOK, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.BtnCancel, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 515);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(784, 36);
            this.tableLayoutPanel3.TabIndex = 44;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Remark";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsMemo
            // 
            this.tsMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tsMemo.Enabled = false;
            this.tsMemo.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tsMemo.Location = new System.Drawing.Point(66, 4);
            this.tsMemo.Name = "tsMemo";
            this.tsMemo.Size = new System.Drawing.Size(553, 27);
            this.tsMemo.TabIndex = 1;
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnOK.Enabled = false;
            this.BtnOK.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnOK.Location = new System.Drawing.Point(625, 4);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 27);
            this.BtnOK.TabIndex = 2;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnCancel.Enabled = false;
            this.BtnCancel.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnCancel.Location = new System.Drawing.Point(706, 4);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 27);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 551);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 45;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(58, 17);
            this.toolStripStatusLabel1.Text = "Message";
            // 
            // tsMsg
            // 
            this.tsMsg.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tsMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.tsMsg.Name = "tsMsg";
            this.tsMsg.Size = new System.Drawing.Size(0, 17);
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 573);
            this.Controls.Add(this.tabPanelControl);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.statusStrip1);
            this.DoubleBuffered = true;
            this.Name = "fMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RC Output";
            this.Load += new System.EventHandler(this.FMain_Load);
            this.Shown += new System.EventHandler(this.FMain_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cmsTabpage.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabPanelControl.ResumeLayout(false);
            this.TpParams.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvCondition)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvInput)).EndInit();
            this.TpQCItems.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvQCItem)).EndInit();
            this.TpMachine.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDownModelBindingSource)).EndInit();
            this.TpDefect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvDefect)).EndInit();
            this.TpQCMark.ResumeLayout(false);
            this.TpQCMark.PerformLayout();
            this.TpRework.ResumeLayout(false);
            this.TpRework.PerformLayout();
            this.TpBonus.ResumeLayout(false);
            this.TpBonus.PerformLayout();
            this.TpOutTime.ResumeLayout(false);
            this.TpOutTime.PerformLayout();
            this.TpSN.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvSN)).EndInit();
            this.TpKeyparts.ResumeLayout(false);
            this.TpKeyparts.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvBOM)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvKeypart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem tsDetial;
        private System.Windows.Forms.ToolStripMenuItem tsDll;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox txtInput;
        private System.Windows.Forms.ContextMenuStrip cmsTabpage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStripLabel cmbParam;
        private System.Windows.Forms.TabControl tabPanelControl;
        private System.Windows.Forms.TabPage TpParams;
        private System.Windows.Forms.TabPage TpMachine;
        private System.Windows.Forms.TabPage TpSN;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gvCondition;
        private System.Windows.Forms.DataGridView gvSN;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel tsEmp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tsMemo;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.DataGridView gvMachine;
        private System.Windows.Forms.DataGridView gvInput;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsMsg;
        private System.Windows.Forms.TabPage TpDefect;
        public System.Windows.Forms.DataGridView gvDefect;
        private System.Windows.Forms.TabPage TpBonus;
        private System.Windows.Forms.TextBox txtBonus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage TpKeyparts;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox editKPSN;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripTextBox editCount;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripButton btnAppend;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView gvBOM;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView gvKeypart;
        private System.Windows.Forms.TextBox txtWorkHour;
        private System.Windows.Forms.Label lblWorkHour;
        private System.Windows.Forms.TabPage TpOutTime;
        private System.Windows.Forms.DateTimePicker dtpOutDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripLabel tslabelFormerNO;
        private System.Windows.Forms.ToolStripLabel tsLabelBluePrint;
        private System.Windows.Forms.TabPage TpRework;
        private System.Windows.Forms.Label LbRework;
        private System.Windows.Forms.TextBox TbRework;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button BtnMachineDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn rUNFLAGDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dEFAULTSTATUSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEASONCODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEASONDESCDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn workFlagDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource machineDownModelBindingSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mACHINEIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mACHINECODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mACHINEDESCDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sTATUSNAMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tYPEIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEASONIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sTARTTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eNDTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lOADQTYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dATECODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn sTOVESEQDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEMARKDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripButton btnProcessSet;
        private System.Windows.Forms.ToolStripLabel lblProcess;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.TabPage TpQCItems;
        private System.Windows.Forms.DataGridView DgvQCItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label LbQCInfo;
        private System.Windows.Forms.CheckBox CbQCItem;
        private System.Windows.Forms.TabPage TpQCMark;
        private System.Windows.Forms.Button BtnMark_OK;
        private System.Windows.Forms.Label LbInspector;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DtpInspectTime;
        private System.Windows.Forms.Label Lb_Status;
        private System.Windows.Forms.Button BtnMark_NG;
    }
}