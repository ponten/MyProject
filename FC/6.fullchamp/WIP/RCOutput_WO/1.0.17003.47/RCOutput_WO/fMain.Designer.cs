namespace RCOutput_WO
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TsmiSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiSelectNone = new System.Windows.Forms.ToolStripMenuItem();
            this.TlpInfo = new System.Windows.Forms.TableLayoutPanel();
            this.TsmiDetial = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiDll = new System.Windows.Forms.ToolStripMenuItem();
            this.Ts1 = new System.Windows.Forms.ToolStrip();
            this.TslParam = new System.Windows.Forms.ToolStripLabel();
            this.TbInput = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.TslEmp = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TslForm = new System.Windows.Forms.ToolStripLabel();
            this.CmsTabpage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TcParams = new System.Windows.Forms.TabControl();
            this.TpParams = new System.Windows.Forms.TabPage();
            this.TlpParam = new System.Windows.Forms.TableLayoutPanel();
            this.LbCurrentRC6 = new System.Windows.Forms.Label();
            this.ScParam = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DgvCondition = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DgvInput = new System.Windows.Forms.DataGridView();
            this.TpMachine = new System.Windows.Forms.TabPage();
            this.TlpMachine = new System.Windows.Forms.TableLayoutPanel();
            this.DgvMachine = new System.Windows.Forms.DataGridView();
            this.selectDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.mACHINEIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mACHINECODEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mACHINEDESCDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTATUSNAMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tYPEIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rEASONIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTARTTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eNDTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dATECODEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTOVESEQDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lOADQTYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rEMARKDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machineDownModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BtnMachineChange = new System.Windows.Forms.Button();
            this.LbCurrentRC1 = new System.Windows.Forms.Label();
            this.CbShareSetting = new System.Windows.Forms.CheckBox();
            this.LbT4T6 = new System.Windows.Forms.Label();
            this.TpDefect = new System.Windows.Forms.TabPage();
            this.TlpDefect = new System.Windows.Forms.TableLayoutPanel();
            this.DgvDefect = new System.Windows.Forms.DataGridView();
            this.dEFECTCODEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dEFECTDESCDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qTYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.defectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.LbCurrentRC5 = new System.Windows.Forms.Label();
            this.TpOutTime = new System.Windows.Forms.TabPage();
            this.CbReportTime = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.LbLastReportTime = new System.Windows.Forms.Label();
            this.LbCurrentRC4 = new System.Windows.Forms.Label();
            this.DtpOutDate = new System.Windows.Forms.DateTimePicker();
            this.TpRework = new System.Windows.Forms.TabPage();
            this.TbReworkQty = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.LbCurrentRC2 = new System.Windows.Forms.Label();
            this.TpBonus = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.LbCurrentRC3 = new System.Windows.Forms.Label();
            this.TbWH = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TbBonus = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TpSN = new System.Windows.Forms.TabPage();
            this.DgvSN = new System.Windows.Forms.DataGridView();
            this.TpKeyparts = new System.Windows.Forms.TabPage();
            this.ScKeypart = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DgvBOM = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.DgvKeypart = new System.Windows.Forms.DataGridView();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.EditKPSN = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.EditCount = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.BtnAppend = new System.Windows.Forms.ToolStripButton();
            this.BtnDelete = new System.Windows.Forms.ToolStripButton();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.TlpRemark = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.TbMemo = new System.Windows.Forms.TextBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SsMessage = new System.Windows.Forms.StatusStrip();
            this.Tssl1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.TsslMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.TlpRC = new System.Windows.Forms.TableLayoutPanel();
            this.BtnReset = new System.Windows.Forms.Button();
            this.BtnSelectAll = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.LbCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TbCheckRC = new System.Windows.Forms.TextBox();
            this.DgvRC = new System.Windows.Forms.DataGridView();
            this.TlpLayout = new System.Windows.Forms.TableLayoutPanel();
            this.ScMain = new System.Windows.Forms.SplitContainer();
            this.GbRC = new System.Windows.Forms.GroupBox();
            this.TlpTop2 = new System.Windows.Forms.TableLayoutPanel();
            this.TlpTop = new System.Windows.Forms.TableLayoutPanel();
            this.LbProcess = new System.Windows.Forms.Label();
            this.LbFormerNO = new System.Windows.Forms.Label();
            this.LbBluePrint = new System.Windows.Forms.Label();
            this.TlpInfo2 = new System.Windows.Forms.TableLayoutPanel();
            this.CHECK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RC_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GOOD_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SCRAP_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CURRENT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROCESSED_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REWORK_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WORK_HOUR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BONUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INITIAL_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1.SuspendLayout();
            this.Ts1.SuspendLayout();
            this.CmsTabpage.SuspendLayout();
            this.TcParams.SuspendLayout();
            this.TpParams.SuspendLayout();
            this.TlpParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScParam)).BeginInit();
            this.ScParam.Panel1.SuspendLayout();
            this.ScParam.Panel2.SuspendLayout();
            this.ScParam.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvCondition)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvInput)).BeginInit();
            this.TpMachine.SuspendLayout();
            this.TlpMachine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDownModelBindingSource)).BeginInit();
            this.TpDefect.SuspendLayout();
            this.TlpDefect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvDefect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defectBindingSource)).BeginInit();
            this.TpOutTime.SuspendLayout();
            this.TpRework.SuspendLayout();
            this.TpBonus.SuspendLayout();
            this.TpSN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSN)).BeginInit();
            this.TpKeyparts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScKeypart)).BeginInit();
            this.ScKeypart.Panel1.SuspendLayout();
            this.ScKeypart.Panel2.SuspendLayout();
            this.ScKeypart.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvBOM)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvKeypart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.TlpRemark.SuspendLayout();
            this.SsMessage.SuspendLayout();
            this.TlpRC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvRC)).BeginInit();
            this.TlpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScMain)).BeginInit();
            this.ScMain.Panel1.SuspendLayout();
            this.ScMain.Panel2.SuspendLayout();
            this.ScMain.SuspendLayout();
            this.GbRC.SuspendLayout();
            this.TlpTop2.SuspendLayout();
            this.TlpTop.SuspendLayout();
            this.TlpInfo2.SuspendLayout();
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
            this.TsmiSelectAll,
            this.TsmiSelectNone});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(144, 48);
            // 
            // TsmiSelectAll
            // 
            this.TsmiSelectAll.Name = "TsmiSelectAll";
            this.TsmiSelectAll.Size = new System.Drawing.Size(143, 22);
            this.TsmiSelectAll.Text = "Select All";
            // 
            // TsmiSelectNone
            // 
            this.TsmiSelectNone.Name = "TsmiSelectNone";
            this.TsmiSelectNone.Size = new System.Drawing.Size(143, 22);
            this.TsmiSelectNone.Text = "Select None";
            // 
            // TlpInfo
            // 
            this.TlpInfo.BackColor = System.Drawing.Color.Transparent;
            this.TlpInfo.ColumnCount = 4;
            this.TlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpInfo.Location = new System.Drawing.Point(2, 2);
            this.TlpInfo.Margin = new System.Windows.Forms.Padding(0);
            this.TlpInfo.Name = "TlpInfo";
            this.TlpInfo.RowCount = 1;
            this.TlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TlpInfo.Size = new System.Drawing.Size(1180, 39);
            this.TlpInfo.TabIndex = 11;
            // 
            // TsmiDetial
            // 
            this.TsmiDetial.Name = "TsmiDetial";
            this.TsmiDetial.Size = new System.Drawing.Size(116, 22);
            this.TsmiDetial.Text = "tsDetail";
            // 
            // TsmiDll
            // 
            this.TsmiDll.Name = "TsmiDll";
            this.TsmiDll.Size = new System.Drawing.Size(116, 22);
            this.TsmiDll.Text = "tsDll";
            // 
            // Ts1
            // 
            this.Ts1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ts1.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.Ts1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Ts1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TslParam,
            this.TbInput,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.TslEmp,
            this.toolStripSeparator1,
            this.TslForm});
            this.Ts1.Location = new System.Drawing.Point(0, 0);
            this.Ts1.Name = "Ts1";
            this.Ts1.Size = new System.Drawing.Size(1184, 25);
            this.Ts1.TabIndex = 37;
            this.Ts1.Text = "toolStrip1";
            // 
            // TslParam
            // 
            this.TslParam.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TslParam.Name = "TslParam";
            this.TslParam.Size = new System.Drawing.Size(46, 22);
            this.TslParam.Text = "RC No";
            // 
            // TbInput
            // 
            this.TbInput.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.TbInput.Name = "TbInput";
            this.TbInput.Size = new System.Drawing.Size(200, 25);
            this.TbInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbInput_KeyPress);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(63, 22);
            this.toolStripLabel2.Text = "Employee";
            // 
            // TslEmp
            // 
            this.TslEmp.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TslEmp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.TslEmp.Name = "TslEmp";
            this.TslEmp.Size = new System.Drawing.Size(0, 22);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // TslForm
            // 
            this.TslForm.ForeColor = System.Drawing.Color.Red;
            this.TslForm.Name = "TslForm";
            this.TslForm.Size = new System.Drawing.Size(83, 22);
            this.TslForm.Text = "RC Output WO";
            // 
            // CmsTabpage
            // 
            this.CmsTabpage.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.CmsTabpage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiDll,
            this.TsmiDetial});
            this.CmsTabpage.Name = "cmsTabpage";
            this.CmsTabpage.Size = new System.Drawing.Size(117, 48);
            // 
            // TcParams
            // 
            this.TcParams.Controls.Add(this.TpParams);
            this.TcParams.Controls.Add(this.TpMachine);
            this.TcParams.Controls.Add(this.TpDefect);
            this.TcParams.Controls.Add(this.TpOutTime);
            this.TcParams.Controls.Add(this.TpRework);
            this.TcParams.Controls.Add(this.TpBonus);
            this.TcParams.Controls.Add(this.TpSN);
            this.TcParams.Controls.Add(this.TpKeyparts);
            this.TcParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TcParams.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TcParams.Location = new System.Drawing.Point(0, 8);
            this.TcParams.Margin = new System.Windows.Forms.Padding(0);
            this.TcParams.Name = "TcParams";
            this.TcParams.SelectedIndex = 0;
            this.TcParams.Size = new System.Drawing.Size(604, 377);
            this.TcParams.TabIndex = 42;
            // 
            // TpParams
            // 
            this.TpParams.BackColor = System.Drawing.SystemColors.Control;
            this.TpParams.Controls.Add(this.TlpParam);
            this.TpParams.Location = new System.Drawing.Point(4, 26);
            this.TpParams.Margin = new System.Windows.Forms.Padding(0);
            this.TpParams.Name = "TpParams";
            this.TpParams.Size = new System.Drawing.Size(596, 347);
            this.TpParams.TabIndex = 0;
            this.TpParams.Text = "Process Params";
            // 
            // TlpParam
            // 
            this.TlpParam.ColumnCount = 1;
            this.TlpParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpParam.Controls.Add(this.LbCurrentRC6, 0, 0);
            this.TlpParam.Controls.Add(this.ScParam, 0, 1);
            this.TlpParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpParam.Location = new System.Drawing.Point(0, 0);
            this.TlpParam.Name = "TlpParam";
            this.TlpParam.RowCount = 2;
            this.TlpParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.TlpParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpParam.Size = new System.Drawing.Size(596, 347);
            this.TlpParam.TabIndex = 1;
            // 
            // LbCurrentRC6
            // 
            this.LbCurrentRC6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LbCurrentRC6.AutoSize = true;
            this.LbCurrentRC6.Location = new System.Drawing.Point(40, 17);
            this.LbCurrentRC6.Margin = new System.Windows.Forms.Padding(40, 10, 3, 0);
            this.LbCurrentRC6.Name = "LbCurrentRC6";
            this.LbCurrentRC6.Size = new System.Drawing.Size(61, 16);
            this.LbCurrentRC6.TabIndex = 0;
            this.LbCurrentRC6.Text = "Runcard";
            // 
            // ScParam
            // 
            this.ScParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScParam.Location = new System.Drawing.Point(3, 43);
            this.ScParam.Name = "ScParam";
            this.ScParam.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ScParam.Panel1
            // 
            this.ScParam.Panel1.Controls.Add(this.groupBox1);
            // 
            // ScParam.Panel2
            // 
            this.ScParam.Panel2.AutoScroll = true;
            this.ScParam.Panel2.Controls.Add(this.groupBox2);
            this.ScParam.Size = new System.Drawing.Size(590, 301);
            this.ScParam.SplitterDistance = 154;
            this.ScParam.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DgvCondition);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(590, 154);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Condition";
            // 
            // DgvCondition
            // 
            this.DgvCondition.AllowUserToAddRows = false;
            this.DgvCondition.AllowUserToDeleteRows = false;
            this.DgvCondition.BackgroundColor = System.Drawing.Color.White;
            this.DgvCondition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvCondition.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvCondition.Location = new System.Drawing.Point(3, 23);
            this.DgvCondition.MultiSelect = false;
            this.DgvCondition.Name = "DgvCondition";
            this.DgvCondition.ReadOnly = true;
            this.DgvCondition.RowHeadersWidth = 25;
            this.DgvCondition.RowTemplate.Height = 24;
            this.DgvCondition.Size = new System.Drawing.Size(584, 128);
            this.DgvCondition.TabIndex = 3;
            this.DgvCondition.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvCondition_CellClick);
            this.DgvCondition.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DgvCondition_DataBindingComplete);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DgvInput);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(590, 143);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Collection";
            // 
            // DgvInput
            // 
            this.DgvInput.AllowUserToAddRows = false;
            this.DgvInput.AllowUserToDeleteRows = false;
            this.DgvInput.BackgroundColor = System.Drawing.Color.White;
            this.DgvInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvInput.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvInput.Location = new System.Drawing.Point(3, 23);
            this.DgvInput.MultiSelect = false;
            this.DgvInput.Name = "DgvInput";
            this.DgvInput.RowHeadersWidth = 25;
            this.DgvInput.RowTemplate.Height = 24;
            this.DgvInput.Size = new System.Drawing.Size(584, 117);
            this.DgvInput.TabIndex = 4;
            this.DgvInput.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvInput_CellEndEdit);
            // 
            // TpMachine
            // 
            this.TpMachine.BackColor = System.Drawing.SystemColors.Control;
            this.TpMachine.Controls.Add(this.TlpMachine);
            this.TpMachine.Location = new System.Drawing.Point(4, 26);
            this.TpMachine.Margin = new System.Windows.Forms.Padding(0);
            this.TpMachine.Name = "TpMachine";
            this.TpMachine.Size = new System.Drawing.Size(596, 347);
            this.TpMachine.TabIndex = 1;
            this.TpMachine.Text = "Machine";
            // 
            // TlpMachine
            // 
            this.TlpMachine.ColumnCount = 4;
            this.TlpMachine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpMachine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpMachine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpMachine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpMachine.Controls.Add(this.DgvMachine, 0, 1);
            this.TlpMachine.Controls.Add(this.BtnMachineChange, 1, 0);
            this.TlpMachine.Controls.Add(this.LbCurrentRC1, 0, 0);
            this.TlpMachine.Controls.Add(this.CbShareSetting, 3, 0);
            this.TlpMachine.Controls.Add(this.LbT4T6, 2, 0);
            this.TlpMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpMachine.Location = new System.Drawing.Point(0, 0);
            this.TlpMachine.Margin = new System.Windows.Forms.Padding(0);
            this.TlpMachine.Name = "TlpMachine";
            this.TlpMachine.RowCount = 2;
            this.TlpMachine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TlpMachine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpMachine.Size = new System.Drawing.Size(596, 347);
            this.TlpMachine.TabIndex = 5;
            // 
            // DgvMachine
            // 
            this.DgvMachine.AllowUserToAddRows = false;
            this.DgvMachine.AllowUserToDeleteRows = false;
            this.DgvMachine.AutoGenerateColumns = false;
            this.DgvMachine.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvMachine.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DgvMachine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMachine.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectDataGridViewCheckBoxColumn,
            this.mACHINEIDDataGridViewTextBoxColumn,
            this.mACHINECODEDataGridViewTextBoxColumn,
            this.mACHINEDESCDataGridViewTextBoxColumn,
            this.sTATUSNAMEDataGridViewTextBoxColumn,
            this.tYPEIDDataGridViewTextBoxColumn,
            this.rEASONIDDataGridViewTextBoxColumn,
            this.sTARTTIMEDataGridViewTextBoxColumn,
            this.eNDTIMEDataGridViewTextBoxColumn,
            this.dATECODEDataGridViewTextBoxColumn,
            this.sTOVESEQDataGridViewTextBoxColumn,
            this.lOADQTYDataGridViewTextBoxColumn,
            this.rEMARKDataGridViewTextBoxColumn});
            this.TlpMachine.SetColumnSpan(this.DgvMachine, 4);
            this.DgvMachine.DataSource = this.machineDownModelBindingSource;
            this.DgvMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvMachine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvMachine.Location = new System.Drawing.Point(0, 50);
            this.DgvMachine.Margin = new System.Windows.Forms.Padding(0);
            this.DgvMachine.Name = "DgvMachine";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DgvMachine.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DgvMachine.RowHeadersWidth = 25;
            this.DgvMachine.RowTemplate.Height = 24;
            this.DgvMachine.Size = new System.Drawing.Size(596, 297);
            this.DgvMachine.TabIndex = 4;
            this.DgvMachine.VisibleChanged += new System.EventHandler(this.DgvMachine_VisibleChanged);
            // 
            // selectDataGridViewCheckBoxColumn
            // 
            this.selectDataGridViewCheckBoxColumn.DataPropertyName = "Select";
            this.selectDataGridViewCheckBoxColumn.HeaderText = "Select";
            this.selectDataGridViewCheckBoxColumn.Name = "selectDataGridViewCheckBoxColumn";
            this.selectDataGridViewCheckBoxColumn.ReadOnly = true;
            this.selectDataGridViewCheckBoxColumn.Visible = false;
            // 
            // mACHINEIDDataGridViewTextBoxColumn
            // 
            this.mACHINEIDDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_ID";
            this.mACHINEIDDataGridViewTextBoxColumn.HeaderText = "MACHINE_ID";
            this.mACHINEIDDataGridViewTextBoxColumn.Name = "mACHINEIDDataGridViewTextBoxColumn";
            this.mACHINEIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINEIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.mACHINEIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // mACHINECODEDataGridViewTextBoxColumn
            // 
            this.mACHINECODEDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_CODE";
            this.mACHINECODEDataGridViewTextBoxColumn.HeaderText = "Machine code";
            this.mACHINECODEDataGridViewTextBoxColumn.Name = "mACHINECODEDataGridViewTextBoxColumn";
            this.mACHINECODEDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINECODEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // mACHINEDESCDataGridViewTextBoxColumn
            // 
            this.mACHINEDESCDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_DESC";
            this.mACHINEDESCDataGridViewTextBoxColumn.HeaderText = "Machine";
            this.mACHINEDESCDataGridViewTextBoxColumn.Name = "mACHINEDESCDataGridViewTextBoxColumn";
            this.mACHINEDESCDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINEDESCDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // sTATUSNAMEDataGridViewTextBoxColumn
            // 
            this.sTATUSNAMEDataGridViewTextBoxColumn.DataPropertyName = "STATUS_NAME";
            this.sTATUSNAMEDataGridViewTextBoxColumn.HeaderText = "STATUS_NAME";
            this.sTATUSNAMEDataGridViewTextBoxColumn.Name = "sTATUSNAMEDataGridViewTextBoxColumn";
            this.sTATUSNAMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTATUSNAMEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sTATUSNAMEDataGridViewTextBoxColumn.Visible = false;
            // 
            // tYPEIDDataGridViewTextBoxColumn
            // 
            this.tYPEIDDataGridViewTextBoxColumn.DataPropertyName = "TYPE_ID";
            this.tYPEIDDataGridViewTextBoxColumn.HeaderText = "TYPE_ID";
            this.tYPEIDDataGridViewTextBoxColumn.Name = "tYPEIDDataGridViewTextBoxColumn";
            this.tYPEIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.tYPEIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tYPEIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // rEASONIDDataGridViewTextBoxColumn
            // 
            this.rEASONIDDataGridViewTextBoxColumn.DataPropertyName = "REASON_ID";
            this.rEASONIDDataGridViewTextBoxColumn.HeaderText = "REASON_ID";
            this.rEASONIDDataGridViewTextBoxColumn.Name = "rEASONIDDataGridViewTextBoxColumn";
            this.rEASONIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.rEASONIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rEASONIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // sTARTTIMEDataGridViewTextBoxColumn
            // 
            this.sTARTTIMEDataGridViewTextBoxColumn.DataPropertyName = "START_TIME";
            dataGridViewCellStyle2.Format = "yyyy/ MM/ dd HH: mm: ss";
            this.sTARTTIMEDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.sTARTTIMEDataGridViewTextBoxColumn.HeaderText = "Start time";
            this.sTARTTIMEDataGridViewTextBoxColumn.Name = "sTARTTIMEDataGridViewTextBoxColumn";
            this.sTARTTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTARTTIMEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // eNDTIMEDataGridViewTextBoxColumn
            // 
            this.eNDTIMEDataGridViewTextBoxColumn.DataPropertyName = "END_TIME";
            this.eNDTIMEDataGridViewTextBoxColumn.HeaderText = "END_TIME";
            this.eNDTIMEDataGridViewTextBoxColumn.Name = "eNDTIMEDataGridViewTextBoxColumn";
            this.eNDTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.eNDTIMEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.eNDTIMEDataGridViewTextBoxColumn.Visible = false;
            // 
            // dATECODEDataGridViewTextBoxColumn
            // 
            this.dATECODEDataGridViewTextBoxColumn.DataPropertyName = "DATE_CODE";
            this.dATECODEDataGridViewTextBoxColumn.HeaderText = "Stove sequence";
            this.dATECODEDataGridViewTextBoxColumn.Name = "dATECODEDataGridViewTextBoxColumn";
            this.dATECODEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // sTOVESEQDataGridViewTextBoxColumn
            // 
            this.sTOVESEQDataGridViewTextBoxColumn.DataPropertyName = "STOVE_SEQ";
            this.sTOVESEQDataGridViewTextBoxColumn.HeaderText = "--";
            this.sTOVESEQDataGridViewTextBoxColumn.Name = "sTOVESEQDataGridViewTextBoxColumn";
            this.sTOVESEQDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // lOADQTYDataGridViewTextBoxColumn
            // 
            this.lOADQTYDataGridViewTextBoxColumn.DataPropertyName = "LOAD_QTY";
            this.lOADQTYDataGridViewTextBoxColumn.HeaderText = "Load";
            this.lOADQTYDataGridViewTextBoxColumn.Name = "lOADQTYDataGridViewTextBoxColumn";
            this.lOADQTYDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // rEMARKDataGridViewTextBoxColumn
            // 
            this.rEMARKDataGridViewTextBoxColumn.DataPropertyName = "REMARK";
            this.rEMARKDataGridViewTextBoxColumn.HeaderText = "REMARK";
            this.rEMARKDataGridViewTextBoxColumn.Name = "rEMARKDataGridViewTextBoxColumn";
            this.rEMARKDataGridViewTextBoxColumn.ReadOnly = true;
            this.rEMARKDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rEMARKDataGridViewTextBoxColumn.Visible = false;
            // 
            // machineDownModelBindingSource
            // 
            this.machineDownModelBindingSource.DataSource = typeof(RCOutput_WO.Models.MachineDownModel);
            // 
            // BtnMachineChange
            // 
            this.BtnMachineChange.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnMachineChange.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnMachineChange.Location = new System.Drawing.Point(255, 10);
            this.BtnMachineChange.Margin = new System.Windows.Forms.Padding(10);
            this.BtnMachineChange.Name = "BtnMachineChange";
            this.BtnMachineChange.Size = new System.Drawing.Size(150, 30);
            this.BtnMachineChange.TabIndex = 5;
            this.BtnMachineChange.Text = "Machine Change";
            this.BtnMachineChange.UseVisualStyleBackColor = true;
            this.BtnMachineChange.Click += new System.EventHandler(this.BtnMachineChange_Click);
            // 
            // LbCurrentRC1
            // 
            this.LbCurrentRC1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LbCurrentRC1.AutoSize = true;
            this.LbCurrentRC1.Location = new System.Drawing.Point(40, 17);
            this.LbCurrentRC1.Margin = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.LbCurrentRC1.Name = "LbCurrentRC1";
            this.LbCurrentRC1.Size = new System.Drawing.Size(61, 16);
            this.LbCurrentRC1.TabIndex = 6;
            this.LbCurrentRC1.Text = "Runcard";
            // 
            // CbShareSetting
            // 
            this.CbShareSetting.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CbShareSetting.AutoSize = true;
            this.CbShareSetting.Enabled = false;
            this.CbShareSetting.Location = new System.Drawing.Point(442, 15);
            this.CbShareSetting.Name = "CbShareSetting";
            this.CbShareSetting.Size = new System.Drawing.Size(151, 20);
            this.CbShareSetting.TabIndex = 7;
            this.CbShareSetting.Text = "Shared data settings";
            this.CbShareSetting.UseVisualStyleBackColor = true;
            // 
            // LbT4T6
            // 
            this.LbT4T6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.LbT4T6.AutoSize = true;
            this.LbT4T6.ForeColor = System.Drawing.Color.Red;
            this.LbT4T6.Location = new System.Drawing.Point(418, 17);
            this.LbT4T6.Name = "LbT4T6";
            this.LbT4T6.Size = new System.Drawing.Size(18, 16);
            this.LbT4T6.TabIndex = 8;
            this.LbT4T6.Text = "--";
            // 
            // TpDefect
            // 
            this.TpDefect.BackColor = System.Drawing.SystemColors.Control;
            this.TpDefect.Controls.Add(this.TlpDefect);
            this.TpDefect.Location = new System.Drawing.Point(4, 26);
            this.TpDefect.Margin = new System.Windows.Forms.Padding(0);
            this.TpDefect.Name = "TpDefect";
            this.TpDefect.Size = new System.Drawing.Size(596, 347);
            this.TpDefect.TabIndex = 4;
            this.TpDefect.Text = "Defect";
            // 
            // TlpDefect
            // 
            this.TlpDefect.ColumnCount = 1;
            this.TlpDefect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpDefect.Controls.Add(this.DgvDefect, 0, 1);
            this.TlpDefect.Controls.Add(this.LbCurrentRC5, 0, 0);
            this.TlpDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpDefect.Location = new System.Drawing.Point(0, 0);
            this.TlpDefect.Margin = new System.Windows.Forms.Padding(0);
            this.TlpDefect.Name = "TlpDefect";
            this.TlpDefect.RowCount = 2;
            this.TlpDefect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TlpDefect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpDefect.Size = new System.Drawing.Size(596, 347);
            this.TlpDefect.TabIndex = 6;
            // 
            // DgvDefect
            // 
            this.DgvDefect.AllowUserToAddRows = false;
            this.DgvDefect.AllowUserToDeleteRows = false;
            this.DgvDefect.AutoGenerateColumns = false;
            this.DgvDefect.BackgroundColor = System.Drawing.Color.White;
            this.DgvDefect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvDefect.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dEFECTCODEDataGridViewTextBoxColumn,
            this.dEFECTDESCDataGridViewTextBoxColumn,
            this.qTYDataGridViewTextBoxColumn});
            this.DgvDefect.DataSource = this.defectBindingSource;
            this.DgvDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvDefect.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvDefect.Location = new System.Drawing.Point(0, 50);
            this.DgvDefect.Margin = new System.Windows.Forms.Padding(0);
            this.DgvDefect.Name = "DgvDefect";
            this.DgvDefect.RowHeadersWidth = 25;
            this.DgvDefect.RowTemplate.Height = 24;
            this.DgvDefect.Size = new System.Drawing.Size(596, 297);
            this.DgvDefect.TabIndex = 5;
            this.DgvDefect.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvDefect_CellEndEdit);
            this.DgvDefect.VisibleChanged += new System.EventHandler(this.DgvDefect_VisibleChanged);
            this.DgvDefect.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DgvDefect_KeyPress);
            this.DgvDefect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DgvDefect_MouseDown);
            // 
            // dEFECTCODEDataGridViewTextBoxColumn
            // 
            this.dEFECTCODEDataGridViewTextBoxColumn.DataPropertyName = "DEFECT_CODE";
            this.dEFECTCODEDataGridViewTextBoxColumn.Frozen = true;
            this.dEFECTCODEDataGridViewTextBoxColumn.HeaderText = "Defect code";
            this.dEFECTCODEDataGridViewTextBoxColumn.Name = "dEFECTCODEDataGridViewTextBoxColumn";
            this.dEFECTCODEDataGridViewTextBoxColumn.ReadOnly = true;
            this.dEFECTCODEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dEFECTDESCDataGridViewTextBoxColumn
            // 
            this.dEFECTDESCDataGridViewTextBoxColumn.DataPropertyName = "DEFECT_DESC";
            this.dEFECTDESCDataGridViewTextBoxColumn.Frozen = true;
            this.dEFECTDESCDataGridViewTextBoxColumn.HeaderText = "Defect";
            this.dEFECTDESCDataGridViewTextBoxColumn.Name = "dEFECTDESCDataGridViewTextBoxColumn";
            this.dEFECTDESCDataGridViewTextBoxColumn.ReadOnly = true;
            this.dEFECTDESCDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // qTYDataGridViewTextBoxColumn
            // 
            this.qTYDataGridViewTextBoxColumn.DataPropertyName = "QTY";
            this.qTYDataGridViewTextBoxColumn.Frozen = true;
            this.qTYDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.qTYDataGridViewTextBoxColumn.Name = "qTYDataGridViewTextBoxColumn";
            this.qTYDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // defectBindingSource
            // 
            this.defectBindingSource.DataSource = typeof(RCOutput_WO.Models.Defect);
            // 
            // LbCurrentRC5
            // 
            this.LbCurrentRC5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LbCurrentRC5.AutoSize = true;
            this.LbCurrentRC5.Location = new System.Drawing.Point(40, 17);
            this.LbCurrentRC5.Margin = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.LbCurrentRC5.Name = "LbCurrentRC5";
            this.LbCurrentRC5.Size = new System.Drawing.Size(61, 16);
            this.LbCurrentRC5.TabIndex = 6;
            this.LbCurrentRC5.Text = "Runcard";
            // 
            // TpOutTime
            // 
            this.TpOutTime.BackColor = System.Drawing.SystemColors.Control;
            this.TpOutTime.Controls.Add(this.CbReportTime);
            this.TpOutTime.Controls.Add(this.label8);
            this.TpOutTime.Controls.Add(this.LbLastReportTime);
            this.TpOutTime.Controls.Add(this.LbCurrentRC4);
            this.TpOutTime.Controls.Add(this.DtpOutDate);
            this.TpOutTime.Location = new System.Drawing.Point(4, 26);
            this.TpOutTime.Margin = new System.Windows.Forms.Padding(0);
            this.TpOutTime.Name = "TpOutTime";
            this.TpOutTime.Size = new System.Drawing.Size(596, 347);
            this.TpOutTime.TabIndex = 7;
            this.TpOutTime.Text = "Out Process Time";
            // 
            // CbReportTime
            // 
            this.CbReportTime.AutoSize = true;
            this.CbReportTime.Location = new System.Drawing.Point(25, 134);
            this.CbReportTime.Name = "CbReportTime";
            this.CbReportTime.Size = new System.Drawing.Size(101, 20);
            this.CbReportTime.TabIndex = 6;
            this.CbReportTime.Text = "Output time";
            this.CbReportTime.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(40, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 16);
            this.label8.TabIndex = 5;
            this.label8.Text = "Last report time";
            // 
            // LbLastReportTime
            // 
            this.LbLastReportTime.AutoSize = true;
            this.LbLastReportTime.Location = new System.Drawing.Point(160, 74);
            this.LbLastReportTime.Name = "LbLastReportTime";
            this.LbLastReportTime.Size = new System.Drawing.Size(18, 16);
            this.LbLastReportTime.TabIndex = 4;
            this.LbLastReportTime.Text = "--";
            // 
            // LbCurrentRC4
            // 
            this.LbCurrentRC4.AutoSize = true;
            this.LbCurrentRC4.Location = new System.Drawing.Point(40, 14);
            this.LbCurrentRC4.Name = "LbCurrentRC4";
            this.LbCurrentRC4.Size = new System.Drawing.Size(61, 16);
            this.LbCurrentRC4.TabIndex = 3;
            this.LbCurrentRC4.Text = "Runcard";
            // 
            // DtpOutDate
            // 
            this.DtpOutDate.CustomFormat = "yyyy/ MM/ dd HH: mm: ss";
            this.DtpOutDate.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.DtpOutDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpOutDate.Location = new System.Drawing.Point(160, 130);
            this.DtpOutDate.Margin = new System.Windows.Forms.Padding(2);
            this.DtpOutDate.Name = "DtpOutDate";
            this.DtpOutDate.Size = new System.Drawing.Size(250, 27);
            this.DtpOutDate.TabIndex = 0;
            this.DtpOutDate.Value = new System.DateTime(2020, 9, 23, 0, 0, 0, 0);
            this.DtpOutDate.ValueChanged += new System.EventHandler(this.DtpOutDate_ValueChanged);
            // 
            // TpRework
            // 
            this.TpRework.BackColor = System.Drawing.SystemColors.Control;
            this.TpRework.Controls.Add(this.TbReworkQty);
            this.TpRework.Controls.Add(this.label10);
            this.TpRework.Controls.Add(this.label9);
            this.TpRework.Controls.Add(this.LbCurrentRC2);
            this.TpRework.Location = new System.Drawing.Point(4, 26);
            this.TpRework.Margin = new System.Windows.Forms.Padding(0);
            this.TpRework.Name = "TpRework";
            this.TpRework.Size = new System.Drawing.Size(596, 347);
            this.TpRework.TabIndex = 8;
            this.TpRework.Text = "Rework quantity";
            // 
            // TbReworkQty
            // 
            this.TbReworkQty.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TbReworkQty.Location = new System.Drawing.Point(160, 70);
            this.TbReworkQty.Name = "TbReworkQty";
            this.TbReworkQty.Size = new System.Drawing.Size(150, 27);
            this.TbReworkQty.TabIndex = 8;
            this.TbReworkQty.Text = "1";
            this.TbReworkQty.Click += new System.EventHandler(this.TbReworkQty_Click);
            this.TbReworkQty.TextChanged += new System.EventHandler(this.TbReworkQty_TextChanged);
            this.TbReworkQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbReworkQty_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(40, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 16);
            this.label10.TabIndex = 7;
            this.label10.Text = "Rework quantity:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(40, 134);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(189, 16);
            this.label9.TabIndex = 6;
            this.label9.Text = "Finish editing, press ENTER.";
            // 
            // LbCurrentRC2
            // 
            this.LbCurrentRC2.AutoSize = true;
            this.LbCurrentRC2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbCurrentRC2.Location = new System.Drawing.Point(40, 14);
            this.LbCurrentRC2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LbCurrentRC2.Name = "LbCurrentRC2";
            this.LbCurrentRC2.Size = new System.Drawing.Size(61, 16);
            this.LbCurrentRC2.TabIndex = 5;
            this.LbCurrentRC2.Text = "Runcard";
            // 
            // TpBonus
            // 
            this.TpBonus.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TpBonus.Controls.Add(this.label7);
            this.TpBonus.Controls.Add(this.LbCurrentRC3);
            this.TpBonus.Controls.Add(this.TbWH);
            this.TpBonus.Controls.Add(this.label3);
            this.TpBonus.Controls.Add(this.TbBonus);
            this.TpBonus.Controls.Add(this.label2);
            this.TpBonus.Location = new System.Drawing.Point(4, 26);
            this.TpBonus.Margin = new System.Windows.Forms.Padding(0);
            this.TpBonus.Name = "TpBonus";
            this.TpBonus.Size = new System.Drawing.Size(596, 347);
            this.TpBonus.TabIndex = 5;
            this.TpBonus.Text = "Bonus / Work Hour";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(40, 194);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(189, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "Finish editing, press ENTER.";
            // 
            // LbCurrentRC3
            // 
            this.LbCurrentRC3.AutoSize = true;
            this.LbCurrentRC3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbCurrentRC3.Location = new System.Drawing.Point(40, 14);
            this.LbCurrentRC3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LbCurrentRC3.Name = "LbCurrentRC3";
            this.LbCurrentRC3.Size = new System.Drawing.Size(61, 16);
            this.LbCurrentRC3.TabIndex = 4;
            this.LbCurrentRC3.Text = "Runcard";
            // 
            // TbWH
            // 
            this.TbWH.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TbWH.Location = new System.Drawing.Point(160, 130);
            this.TbWH.Name = "TbWH";
            this.TbWH.Size = new System.Drawing.Size(100, 27);
            this.TbWH.TabIndex = 3;
            this.TbWH.Text = "1";
            this.TbWH.Click += new System.EventHandler(this.EditWH_Click);
            this.TbWH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditWH_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(40, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Work Hour";
            // 
            // TbBonus
            // 
            this.TbBonus.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TbBonus.Location = new System.Drawing.Point(160, 70);
            this.TbBonus.Name = "TbBonus";
            this.TbBonus.Size = new System.Drawing.Size(100, 27);
            this.TbBonus.TabIndex = 1;
            this.TbBonus.Text = "0";
            this.TbBonus.Click += new System.EventHandler(this.EditBonus_Click);
            this.TbBonus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditBonus_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(40, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Bonus";
            // 
            // TpSN
            // 
            this.TpSN.Controls.Add(this.DgvSN);
            this.TpSN.Location = new System.Drawing.Point(4, 26);
            this.TpSN.Margin = new System.Windows.Forms.Padding(0);
            this.TpSN.Name = "TpSN";
            this.TpSN.Size = new System.Drawing.Size(596, 347);
            this.TpSN.TabIndex = 3;
            this.TpSN.Text = "SN";
            this.TpSN.UseVisualStyleBackColor = true;
            // 
            // DgvSN
            // 
            this.DgvSN.AllowUserToAddRows = false;
            this.DgvSN.AllowUserToDeleteRows = false;
            this.DgvSN.BackgroundColor = System.Drawing.Color.White;
            this.DgvSN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvSN.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvSN.Location = new System.Drawing.Point(0, 0);
            this.DgvSN.Margin = new System.Windows.Forms.Padding(0);
            this.DgvSN.Name = "DgvSN";
            this.DgvSN.RowHeadersWidth = 25;
            this.DgvSN.RowTemplate.Height = 24;
            this.DgvSN.Size = new System.Drawing.Size(596, 347);
            this.DgvSN.TabIndex = 4;
            this.DgvSN.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSN_CellEndEdit);
            // 
            // TpKeyparts
            // 
            this.TpKeyparts.Controls.Add(this.ScKeypart);
            this.TpKeyparts.Controls.Add(this.bindingNavigator1);
            this.TpKeyparts.Location = new System.Drawing.Point(4, 26);
            this.TpKeyparts.Margin = new System.Windows.Forms.Padding(0);
            this.TpKeyparts.Name = "TpKeyparts";
            this.TpKeyparts.Size = new System.Drawing.Size(596, 347);
            this.TpKeyparts.TabIndex = 6;
            this.TpKeyparts.Text = "Keyparts Collection";
            this.TpKeyparts.UseVisualStyleBackColor = true;
            // 
            // ScKeypart
            // 
            this.ScKeypart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScKeypart.Location = new System.Drawing.Point(0, 40);
            this.ScKeypart.Name = "ScKeypart";
            // 
            // ScKeypart.Panel1
            // 
            this.ScKeypart.Panel1.Controls.Add(this.groupBox3);
            // 
            // ScKeypart.Panel2
            // 
            this.ScKeypart.Panel2.AutoScroll = true;
            this.ScKeypart.Panel2.Controls.Add(this.groupBox4);
            this.ScKeypart.Size = new System.Drawing.Size(596, 307);
            this.ScKeypart.SplitterDistance = 302;
            this.ScKeypart.TabIndex = 10;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DgvBOM);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(302, 307);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "BOM";
            // 
            // DgvBOM
            // 
            this.DgvBOM.AllowUserToAddRows = false;
            this.DgvBOM.AllowUserToDeleteRows = false;
            this.DgvBOM.BackgroundColor = System.Drawing.Color.White;
            this.DgvBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvBOM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvBOM.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvBOM.Location = new System.Drawing.Point(3, 23);
            this.DgvBOM.Name = "DgvBOM";
            this.DgvBOM.ReadOnly = true;
            this.DgvBOM.RowHeadersWidth = 25;
            this.DgvBOM.RowTemplate.Height = 24;
            this.DgvBOM.Size = new System.Drawing.Size(296, 281);
            this.DgvBOM.TabIndex = 3;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.DgvKeypart);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(290, 307);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Keyparts List";
            // 
            // DgvKeypart
            // 
            this.DgvKeypart.AllowUserToAddRows = false;
            this.DgvKeypart.AllowUserToDeleteRows = false;
            this.DgvKeypart.BackgroundColor = System.Drawing.Color.White;
            this.DgvKeypart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvKeypart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvKeypart.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvKeypart.Location = new System.Drawing.Point(3, 23);
            this.DgvKeypart.Name = "DgvKeypart";
            this.DgvKeypart.RowHeadersWidth = 25;
            this.DgvKeypart.RowTemplate.Height = 24;
            this.DgvKeypart.Size = new System.Drawing.Size(284, 281);
            this.DgvKeypart.TabIndex = 4;
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
            this.EditKPSN,
            this.toolStripLabel4,
            this.toolStripLabel5,
            this.toolStripLabel6,
            this.EditCount,
            this.toolStripLabel7,
            this.BtnAppend,
            this.BtnDelete});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(596, 40);
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
            // EditKPSN
            // 
            this.EditKPSN.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.EditKPSN.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.EditKPSN.Name = "EditKPSN";
            this.EditKPSN.Size = new System.Drawing.Size(150, 40);
            this.EditKPSN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditKPSN_KeyPress);
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
            // EditCount
            // 
            this.EditCount.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.EditCount.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.EditCount.Name = "EditCount";
            this.EditCount.Size = new System.Drawing.Size(100, 40);
            this.EditCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditCount_KeyPress);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(40, 37);
            this.toolStripLabel7.Text = "           ";
            // 
            // BtnAppend
            // 
            this.BtnAppend.Image = ((System.Drawing.Image)(resources.GetObject("BtnAppend.Image")));
            this.BtnAppend.Name = "BtnAppend";
            this.BtnAppend.RightToLeftAutoMirrorImage = true;
            this.BtnAppend.Size = new System.Drawing.Size(49, 37);
            this.BtnAppend.Text = "Append";
            this.BtnAppend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnAppend.Click += new System.EventHandler(this.BtnAppend_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("BtnDelete.Image")));
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.RightToLeftAutoMirrorImage = true;
            this.BtnDelete.Size = new System.Drawing.Size(41, 37);
            this.BtnDelete.Text = "Delete";
            this.BtnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // TlpRemark
            // 
            this.TlpRemark.ColumnCount = 4;
            this.TlpRemark.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpRemark.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpRemark.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpRemark.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpRemark.Controls.Add(this.label1, 0, 0);
            this.TlpRemark.Controls.Add(this.TbMemo, 1, 0);
            this.TlpRemark.Controls.Add(this.BtnOK, 2, 0);
            this.TlpRemark.Controls.Add(this.BtnCancel, 3, 0);
            this.TlpRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpRemark.Location = new System.Drawing.Point(0, 533);
            this.TlpRemark.Margin = new System.Windows.Forms.Padding(0);
            this.TlpRemark.Name = "TlpRemark";
            this.TlpRemark.RowCount = 1;
            this.TlpRemark.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpRemark.Size = new System.Drawing.Size(1184, 36);
            this.TlpRemark.TabIndex = 44;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Remark";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TbMemo
            // 
            this.TbMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TbMemo.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TbMemo.Location = new System.Drawing.Point(66, 4);
            this.TbMemo.Name = "TbMemo";
            this.TbMemo.Size = new System.Drawing.Size(953, 27);
            this.TbMemo.TabIndex = 1;
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnOK.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnOK.Location = new System.Drawing.Point(1025, 4);
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
            this.BtnCancel.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnCancel.Location = new System.Drawing.Point(1106, 4);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 27);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // SsMessage
            // 
            this.SsMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SsMessage.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.SsMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tssl1,
            this.TsslMsg});
            this.SsMessage.Location = new System.Drawing.Point(0, 569);
            this.SsMessage.Name = "SsMessage";
            this.SsMessage.Size = new System.Drawing.Size(1184, 22);
            this.SsMessage.TabIndex = 45;
            this.SsMessage.Text = "statusStrip1";
            // 
            // Tssl1
            // 
            this.Tssl1.Name = "Tssl1";
            this.Tssl1.Size = new System.Drawing.Size(58, 17);
            this.Tssl1.Text = "Message";
            // 
            // TsslMsg
            // 
            this.TsslMsg.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TsslMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.TsslMsg.Name = "TsslMsg";
            this.TsslMsg.Size = new System.Drawing.Size(0, 17);
            // 
            // TlpRC
            // 
            this.TlpRC.ColumnCount = 5;
            this.TlpRC.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.TlpRC.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpRC.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpRC.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpRC.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpRC.Controls.Add(this.BtnReset, 3, 1);
            this.TlpRC.Controls.Add(this.BtnSelectAll, 3, 0);
            this.TlpRC.Controls.Add(this.label5, 1, 0);
            this.TlpRC.Controls.Add(this.LbCount, 2, 1);
            this.TlpRC.Controls.Add(this.label6, 1, 1);
            this.TlpRC.Controls.Add(this.TbCheckRC, 2, 0);
            this.TlpRC.Controls.Add(this.DgvRC, 0, 2);
            this.TlpRC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpRC.Location = new System.Drawing.Point(2, 22);
            this.TlpRC.Margin = new System.Windows.Forms.Padding(2);
            this.TlpRC.Name = "TlpRC";
            this.TlpRC.RowCount = 3;
            this.TlpRC.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TlpRC.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TlpRC.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpRC.Size = new System.Drawing.Size(573, 361);
            this.TlpRC.TabIndex = 43;
            // 
            // BtnReset
            // 
            this.BtnReset.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BtnReset.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnReset.Location = new System.Drawing.Point(283, 33);
            this.BtnReset.Margin = new System.Windows.Forms.Padding(2);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(75, 27);
            this.BtnReset.TabIndex = 5;
            this.BtnReset.Text = "Reset";
            this.BtnReset.UseVisualStyleBackColor = true;
            this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // BtnSelectAll
            // 
            this.BtnSelectAll.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BtnSelectAll.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnSelectAll.Location = new System.Drawing.Point(283, 2);
            this.BtnSelectAll.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSelectAll.Name = "BtnSelectAll";
            this.BtnSelectAll.Size = new System.Drawing.Size(75, 27);
            this.BtnSelectAll.TabIndex = 4;
            this.BtnSelectAll.Text = "Select all";
            this.BtnSelectAll.UseVisualStyleBackColor = true;
            this.BtnSelectAll.Click += new System.EventHandler(this.BtnSelectAll_Click);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(12, 7);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "Number";
            // 
            // LbCount
            // 
            this.LbCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LbCount.AutoSize = true;
            this.LbCount.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbCount.Location = new System.Drawing.Point(79, 38);
            this.LbCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LbCount.Name = "LbCount";
            this.LbCount.Size = new System.Drawing.Size(16, 16);
            this.LbCount.TabIndex = 3;
            this.LbCount.Text = "0";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(12, 38);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 16);
            this.label6.TabIndex = 2;
            this.label6.Text = "Checked";
            // 
            // TbCheckRC
            // 
            this.TbCheckRC.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TbCheckRC.Location = new System.Drawing.Point(79, 2);
            this.TbCheckRC.Margin = new System.Windows.Forms.Padding(2);
            this.TbCheckRC.Name = "TbCheckRC";
            this.TbCheckRC.Size = new System.Drawing.Size(200, 27);
            this.TbCheckRC.TabIndex = 0;
            // 
            // DgvRC
            // 
            this.DgvRC.AllowUserToAddRows = false;
            this.DgvRC.AllowUserToDeleteRows = false;
            this.DgvRC.AllowUserToResizeRows = false;
            this.DgvRC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DgvRC.BackgroundColor = System.Drawing.Color.White;
            this.DgvRC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvRC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CHECK,
            this.RC_NO,
            this.GOOD_QTY,
            this.SCRAP_QTY,
            this.CURRENT_QTY,
            this.PROCESSED_QTY,
            this.REWORK_QTY,
            this.WORK_HOUR,
            this.BONUS,
            this.INITIAL_QTY});
            this.TlpRC.SetColumnSpan(this.DgvRC, 5);
            this.DgvRC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvRC.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvRC.Location = new System.Drawing.Point(2, 64);
            this.DgvRC.Margin = new System.Windows.Forms.Padding(2);
            this.DgvRC.Name = "DgvRC";
            this.DgvRC.RowTemplate.Height = 27;
            this.DgvRC.Size = new System.Drawing.Size(569, 295);
            this.DgvRC.TabIndex = 0;
            this.DgvRC.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvRC_CellClick);
            this.DgvRC.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvRC_CellValueChanged);
            this.DgvRC.CurrentCellDirtyStateChanged += new System.EventHandler(this.DgvRC_CurrentCellDirtyStateChanged);
            // 
            // TlpLayout
            // 
            this.TlpLayout.ColumnCount = 1;
            this.TlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpLayout.Controls.Add(this.Ts1, 0, 0);
            this.TlpLayout.Controls.Add(this.TlpRemark, 0, 4);
            this.TlpLayout.Controls.Add(this.SsMessage, 0, 5);
            this.TlpLayout.Controls.Add(this.ScMain, 0, 3);
            this.TlpLayout.Controls.Add(this.TlpTop2, 0, 1);
            this.TlpLayout.Controls.Add(this.TlpInfo2, 0, 2);
            this.TlpLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpLayout.Location = new System.Drawing.Point(0, 0);
            this.TlpLayout.Margin = new System.Windows.Forms.Padding(2);
            this.TlpLayout.Name = "TlpLayout";
            this.TlpLayout.RowCount = 6;
            this.TlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.TlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TlpLayout.Size = new System.Drawing.Size(1184, 591);
            this.TlpLayout.TabIndex = 47;
            // 
            // ScMain
            // 
            this.ScMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScMain.Location = new System.Drawing.Point(0, 148);
            this.ScMain.Margin = new System.Windows.Forms.Padding(0);
            this.ScMain.Name = "ScMain";
            // 
            // ScMain.Panel1
            // 
            this.ScMain.Panel1.Controls.Add(this.GbRC);
            // 
            // ScMain.Panel2
            // 
            this.ScMain.Panel2.Controls.Add(this.TcParams);
            this.ScMain.Panel2.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.ScMain.Size = new System.Drawing.Size(1184, 385);
            this.ScMain.SplitterDistance = 577;
            this.ScMain.SplitterWidth = 3;
            this.ScMain.TabIndex = 47;
            // 
            // GbRC
            // 
            this.GbRC.Controls.Add(this.TlpRC);
            this.GbRC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GbRC.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.GbRC.Location = new System.Drawing.Point(0, 0);
            this.GbRC.Margin = new System.Windows.Forms.Padding(0);
            this.GbRC.Name = "GbRC";
            this.GbRC.Padding = new System.Windows.Forms.Padding(2);
            this.GbRC.Size = new System.Drawing.Size(577, 385);
            this.GbRC.TabIndex = 44;
            this.GbRC.TabStop = false;
            this.GbRC.Text = "RunCards";
            // 
            // TlpTop2
            // 
            this.TlpTop2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TlpTop2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.TlpTop2.ColumnCount = 1;
            this.TlpTop2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpTop2.Controls.Add(this.TlpTop, 0, 0);
            this.TlpTop2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpTop2.Location = new System.Drawing.Point(0, 25);
            this.TlpTop2.Margin = new System.Windows.Forms.Padding(0);
            this.TlpTop2.Name = "TlpTop2";
            this.TlpTop2.RowCount = 1;
            this.TlpTop2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpTop2.Size = new System.Drawing.Size(1184, 80);
            this.TlpTop2.TabIndex = 50;
            // 
            // TlpTop
            // 
            this.TlpTop.ColumnCount = 4;
            this.TlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpTop.Controls.Add(this.LbProcess, 0, 0);
            this.TlpTop.Controls.Add(this.LbFormerNO, 1, 0);
            this.TlpTop.Controls.Add(this.LbBluePrint, 2, 0);
            this.TlpTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpTop.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TlpTop.Location = new System.Drawing.Point(2, 2);
            this.TlpTop.Margin = new System.Windows.Forms.Padding(0);
            this.TlpTop.Name = "TlpTop";
            this.TlpTop.RowCount = 1;
            this.TlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpTop.Size = new System.Drawing.Size(1180, 76);
            this.TlpTop.TabIndex = 48;
            // 
            // LbProcess
            // 
            this.LbProcess.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LbProcess.AutoSize = true;
            this.LbProcess.Font = new System.Drawing.Font("新細明體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbProcess.ForeColor = System.Drawing.Color.Red;
            this.LbProcess.Location = new System.Drawing.Point(5, 18);
            this.LbProcess.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LbProcess.Name = "LbProcess";
            this.LbProcess.Size = new System.Drawing.Size(226, 40);
            this.LbProcess.TabIndex = 48;
            this.LbProcess.Text = "Process name";
            this.LbProcess.Visible = false;
            // 
            // LbFormerNO
            // 
            this.LbFormerNO.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LbFormerNO.AutoSize = true;
            this.LbFormerNO.ForeColor = System.Drawing.Color.Red;
            this.LbFormerNO.Location = new System.Drawing.Point(241, 24);
            this.LbFormerNO.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LbFormerNO.Name = "LbFormerNO";
            this.LbFormerNO.Size = new System.Drawing.Size(129, 27);
            this.LbFormerNO.TabIndex = 49;
            this.LbFormerNO.Text = "Former NO";
            // 
            // LbBluePrint
            // 
            this.LbBluePrint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LbBluePrint.AutoSize = true;
            this.LbBluePrint.ForeColor = System.Drawing.Color.Red;
            this.LbBluePrint.Location = new System.Drawing.Point(380, 24);
            this.LbBluePrint.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LbBluePrint.Name = "LbBluePrint";
            this.LbBluePrint.Size = new System.Drawing.Size(108, 27);
            this.LbBluePrint.TabIndex = 50;
            this.LbBluePrint.Text = "Blueprint";
            // 
            // TlpInfo2
            // 
            this.TlpInfo2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TlpInfo2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.TlpInfo2.ColumnCount = 1;
            this.TlpInfo2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpInfo2.Controls.Add(this.TlpInfo, 0, 0);
            this.TlpInfo2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpInfo2.Location = new System.Drawing.Point(0, 105);
            this.TlpInfo2.Margin = new System.Windows.Forms.Padding(0);
            this.TlpInfo2.Name = "TlpInfo2";
            this.TlpInfo2.RowCount = 1;
            this.TlpInfo2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpInfo2.Size = new System.Drawing.Size(1184, 43);
            this.TlpInfo2.TabIndex = 51;
            // 
            // CHECK
            // 
            this.CHECK.DataPropertyName = "CHECK";
            this.CHECK.FalseValue = "0";
            this.CHECK.Frozen = true;
            this.CHECK.HeaderText = "Check";
            this.CHECK.Name = "CHECK";
            this.CHECK.ReadOnly = true;
            this.CHECK.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CHECK.TrueValue = "1";
            this.CHECK.Width = 54;
            // 
            // RC_NO
            // 
            this.RC_NO.DataPropertyName = "RC_NO";
            this.RC_NO.HeaderText = "RC_NO";
            this.RC_NO.Name = "RC_NO";
            this.RC_NO.ReadOnly = true;
            this.RC_NO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RC_NO.Width = 64;
            // 
            // GOOD_QTY
            // 
            this.GOOD_QTY.DataPropertyName = "GOOD_QTY";
            this.GOOD_QTY.HeaderText = "GOOD_QTY";
            this.GOOD_QTY.Name = "GOOD_QTY";
            this.GOOD_QTY.ReadOnly = true;
            this.GOOD_QTY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.GOOD_QTY.Width = 97;
            // 
            // SCRAP_QTY
            // 
            this.SCRAP_QTY.DataPropertyName = "SCRAP_QTY";
            this.SCRAP_QTY.HeaderText = "SCRAP_QTY";
            this.SCRAP_QTY.Name = "SCRAP_QTY";
            this.SCRAP_QTY.ReadOnly = true;
            this.SCRAP_QTY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CURRENT_QTY
            // 
            this.CURRENT_QTY.DataPropertyName = "CURRENT_QTY";
            this.CURRENT_QTY.HeaderText = "CURRENT_QTY";
            this.CURRENT_QTY.Name = "CURRENT_QTY";
            this.CURRENT_QTY.ReadOnly = true;
            this.CURRENT_QTY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CURRENT_QTY.Visible = false;
            this.CURRENT_QTY.Width = 123;
            // 
            // PROCESSED_QTY
            // 
            this.PROCESSED_QTY.DataPropertyName = "PROCESSED_QTY";
            this.PROCESSED_QTY.HeaderText = "PROCESSED_QTY";
            this.PROCESSED_QTY.Name = "PROCESSED_QTY";
            this.PROCESSED_QTY.ReadOnly = true;
            this.PROCESSED_QTY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PROCESSED_QTY.Width = 137;
            // 
            // REWORK_QTY
            // 
            this.REWORK_QTY.DataPropertyName = "REWORK_QTY";
            this.REWORK_QTY.HeaderText = "REWORK_QTY";
            this.REWORK_QTY.Name = "REWORK_QTY";
            this.REWORK_QTY.ReadOnly = true;
            this.REWORK_QTY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.REWORK_QTY.Width = 118;
            // 
            // WORK_HOUR
            // 
            this.WORK_HOUR.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.WORK_HOUR.DataPropertyName = "WORK_HOUR";
            this.WORK_HOUR.HeaderText = "WORK_HOUR";
            this.WORK_HOUR.Name = "WORK_HOUR";
            this.WORK_HOUR.ReadOnly = true;
            this.WORK_HOUR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.WORK_HOUR.Visible = false;
            this.WORK_HOUR.Width = 111;
            // 
            // BONUS
            // 
            this.BONUS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.BONUS.DataPropertyName = "BONUS";
            this.BONUS.HeaderText = "BONUS";
            this.BONUS.Name = "BONUS";
            this.BONUS.ReadOnly = true;
            this.BONUS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BONUS.Visible = false;
            this.BONUS.Width = 65;
            // 
            // INITIAL_QTY
            // 
            this.INITIAL_QTY.DataPropertyName = "INITIAL_QTY";
            this.INITIAL_QTY.HeaderText = "INITIAL_QTY";
            this.INITIAL_QTY.Name = "INITIAL_QTY";
            this.INITIAL_QTY.ReadOnly = true;
            this.INITIAL_QTY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.INITIAL_QTY.Visible = false;
            this.INITIAL_QTY.Width = 108;
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 591);
            this.Controls.Add(this.TlpLayout);
            this.Name = "fMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RC Output WO";
            this.Load += new System.EventHandler(this.FMain_Load);
            this.Shown += new System.EventHandler(this.FMain_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.Ts1.ResumeLayout(false);
            this.Ts1.PerformLayout();
            this.CmsTabpage.ResumeLayout(false);
            this.TcParams.ResumeLayout(false);
            this.TpParams.ResumeLayout(false);
            this.TlpParam.ResumeLayout(false);
            this.TlpParam.PerformLayout();
            this.ScParam.Panel1.ResumeLayout(false);
            this.ScParam.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScParam)).EndInit();
            this.ScParam.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvCondition)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvInput)).EndInit();
            this.TpMachine.ResumeLayout(false);
            this.TlpMachine.ResumeLayout(false);
            this.TlpMachine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDownModelBindingSource)).EndInit();
            this.TpDefect.ResumeLayout(false);
            this.TlpDefect.ResumeLayout(false);
            this.TlpDefect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvDefect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defectBindingSource)).EndInit();
            this.TpOutTime.ResumeLayout(false);
            this.TpOutTime.PerformLayout();
            this.TpRework.ResumeLayout(false);
            this.TpRework.PerformLayout();
            this.TpBonus.ResumeLayout(false);
            this.TpBonus.PerformLayout();
            this.TpSN.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvSN)).EndInit();
            this.TpKeyparts.ResumeLayout(false);
            this.TpKeyparts.PerformLayout();
            this.ScKeypart.Panel1.ResumeLayout(false);
            this.ScKeypart.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScKeypart)).EndInit();
            this.ScKeypart.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvBOM)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvKeypart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.TlpRemark.ResumeLayout(false);
            this.TlpRemark.PerformLayout();
            this.SsMessage.ResumeLayout(false);
            this.SsMessage.PerformLayout();
            this.TlpRC.ResumeLayout(false);
            this.TlpRC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvRC)).EndInit();
            this.TlpLayout.ResumeLayout(false);
            this.TlpLayout.PerformLayout();
            this.ScMain.Panel1.ResumeLayout(false);
            this.ScMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScMain)).EndInit();
            this.ScMain.ResumeLayout(false);
            this.GbRC.ResumeLayout(false);
            this.TlpTop2.ResumeLayout(false);
            this.TlpTop.ResumeLayout(false);
            this.TlpTop.PerformLayout();
            this.TlpInfo2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem TsmiSelectAll;
        private System.Windows.Forms.ToolStripMenuItem TsmiSelectNone;
        private System.Windows.Forms.TableLayoutPanel TlpInfo;
        private System.Windows.Forms.ToolStripMenuItem TsmiDetial;
        private System.Windows.Forms.ToolStripMenuItem TsmiDll;
        private System.Windows.Forms.ToolStrip Ts1;
        private System.Windows.Forms.ToolStripTextBox TbInput;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip CmsTabpage;
        private System.Windows.Forms.ToolStripLabel TslParam;
        private System.Windows.Forms.TabControl TcParams;
        private System.Windows.Forms.TabPage TpParams;
        private System.Windows.Forms.TabPage TpMachine;
        private System.Windows.Forms.TabPage TpSN;
        private System.Windows.Forms.SplitContainer ScParam;
        private System.Windows.Forms.DataGridView DgvCondition;
        private System.Windows.Forms.DataGridView DgvSN;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel TslEmp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel TlpRemark;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbMemo;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.DataGridView DgvMachine;
        private System.Windows.Forms.DataGridView DgvInput;
        private System.Windows.Forms.StatusStrip SsMessage;
        private System.Windows.Forms.ToolStripStatusLabel Tssl1;
        private System.Windows.Forms.ToolStripStatusLabel TsslMsg;
        private System.Windows.Forms.TabPage TpDefect;
        public System.Windows.Forms.DataGridView DgvDefect;
        private System.Windows.Forms.TabPage TpBonus;
        private System.Windows.Forms.TextBox TbBonus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage TpKeyparts;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox EditKPSN;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripTextBox EditCount;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripButton BtnAppend;
        private System.Windows.Forms.ToolStripButton BtnDelete;
        private System.Windows.Forms.SplitContainer ScKeypart;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView DgvBOM;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView DgvKeypart;
        private System.Windows.Forms.TextBox TbWH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage TpOutTime;
        private System.Windows.Forms.DateTimePicker DtpOutDate;
        private System.Windows.Forms.TableLayoutPanel TlpRC;
        private System.Windows.Forms.DataGridView DgvRC;
        private System.Windows.Forms.TableLayoutPanel TlpLayout;
        private System.Windows.Forms.SplitContainer ScMain;
        private System.Windows.Forms.GroupBox GbRC;
        private System.Windows.Forms.TableLayoutPanel TlpDefect;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Button BtnSelectAll;
        private System.Windows.Forms.Label LbCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TbCheckRC;
        private System.Windows.Forms.Label LbCurrentRC3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LbProcess;
        private System.Windows.Forms.TableLayoutPanel TlpTop;
        private System.Windows.Forms.Label LbFormerNO;
        private System.Windows.Forms.Label LbBluePrint;
        private System.Windows.Forms.TabPage TpRework;
        private System.Windows.Forms.TextBox TbReworkQty;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label LbCurrentRC2;
        private System.Windows.Forms.TableLayoutPanel TlpMachine;
        private System.Windows.Forms.Button BtnMachineChange;
        private System.Windows.Forms.Label LbCurrentRC1;
        private System.Windows.Forms.Label LbCurrentRC4;
        private System.Windows.Forms.Label LbCurrentRC5;
        private System.Windows.Forms.BindingSource defectBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dEFECTCODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dEFECTDESCDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn qTYDataGridViewTextBoxColumn;
        private System.Windows.Forms.TableLayoutPanel TlpParam;
        private System.Windows.Forms.Label LbCurrentRC6;
        private System.Windows.Forms.CheckBox CbShareSetting;
        private System.Windows.Forms.BindingSource machineDownModelBindingSource;
        private System.Windows.Forms.Label LbT4T6;
        private System.Windows.Forms.TableLayoutPanel TlpTop2;
        private System.Windows.Forms.TableLayoutPanel TlpInfo2;
        private System.Windows.Forms.Label LbLastReportTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox CbReportTime;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel TslForm;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mACHINEIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mACHINECODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mACHINEDESCDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sTATUSNAMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tYPEIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEASONIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sTARTTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eNDTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dATECODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn sTOVESEQDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lOADQTYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEMARKDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CHECK;
        private System.Windows.Forms.DataGridViewTextBoxColumn RC_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn GOOD_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn SCRAP_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn CURRENT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROCESSED_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn REWORK_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn WORK_HOUR;
        private System.Windows.Forms.DataGridViewTextBoxColumn BONUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn INITIAL_QTY;
    }
}