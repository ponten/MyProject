namespace IQCbyLot
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmsdefect = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Panel10 = new System.Windows.Forms.Panel();
            this.btnShowRT = new System.Windows.Forms.Button();
            this.combLot = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.sbtnRollback = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.DateInsp = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.combLotNo = new System.Windows.Forms.ComboBox();
            this.combItemSeq = new System.Windows.Forms.ComboBox();
            this.ckbUrgent = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRDAdmit = new System.Windows.Forms.Button();
            this.sbtnReject = new System.Windows.Forms.Button();
            this.btERP = new System.Windows.Forms.Button();
            this.sbtnSpecialWaive = new System.Windows.Forms.Button();
            this.sbtnWaive = new System.Windows.Forms.Button();
            this.sbtnByPass = new System.Windows.Forms.Button();
            this.sbtnHold = new System.Windows.Forms.Button();
            this.sbtnSorting = new System.Windows.Forms.Button();
            this.sbtnPartialWaive = new System.Windows.Forms.Button();
            this.sbtnPass = new System.Windows.Forms.Button();
            this.cmsTestType = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.inspectItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeSampleTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importTestResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvItemType = new System.Windows.Forms.DataGridView();
            this.TYPE_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RECID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SEQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TYPE_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAMPLE_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAMPLE_LEVEL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAMPLE_SIZE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASS_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FAIL_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QC_RESULT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EXIST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAMPLING_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAMPLE_LEVEL_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TYPE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpNormal = new System.Windows.Forms.TabPage();
            this.editItemTypeName = new System.Windows.Forms.TextBox();
            this.dgvSampleType = new System.Windows.Forms.DataGridView();
            this.TSAMPLE_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TSAMPLE_LEVEL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TSAMPLE_UNIT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TSAMPLE_SIZE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TCRITICAL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TMAJOR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TMINOR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TSAMPLE_LEVEL_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TSAMPLE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvDefect = new System.Windows.Forms.DataGridView();
            this.DEFECT_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEFECT_DESC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEFECT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEFECT_MEMO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEFECT_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.editTestTypeMemo = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tpRoHS = new System.Windows.Forms.TabPage();
            this.dgvRoHSItem = new System.Windows.Forms.DataGridView();
            this.POSITION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEMO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelRoHSResult = new System.Windows.Forms.Panel();
            this.btnRollbackRoHs = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.editRoHSMemo = new System.Windows.Forms.TextBox();
            this.lablRoHSResult = new System.Windows.Forms.Label();
            this.btnRoHSNG = new System.Windows.Forms.Button();
            this.btnRoHSOK = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.PanelRoHS = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.combItemSeq1 = new System.Windows.Forms.ComboBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnAppend = new System.Windows.Forms.Button();
            this.tpSnHistory = new System.Windows.Forms.TabPage();
            this.panel7 = new System.Windows.Forms.Panel();
            this.dgvSN = new System.Windows.Forms.DataGridView();
            this.SERIAL_NUMBER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATECODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TEST_VALUE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REMARK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CREATE_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CREATE_USER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripInsert = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripModify = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDelete = new System.Windows.Forms.ToolStripButton();
            this.tpComponentPic = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.picPart1 = new System.Windows.Forms.PictureBox();
            this.picPart2 = new System.Windows.Forms.PictureBox();
            this.picPart3 = new System.Windows.Forms.PictureBox();
            this.picPart4 = new System.Windows.Forms.PictureBox();
            this.tabPageSize = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.picBoxSize1 = new System.Windows.Forms.PictureBox();
            this.picBoxSize2 = new System.Windows.Forms.PictureBox();
            this.l5 = new System.Windows.Forms.Label();
            this.l3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.l2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lablPONo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.l4 = new System.Windows.Forms.Label();
            this.l = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnHistory = new System.Windows.Forms.ToolStripButton();
            this.tsbtnMPN = new System.Windows.Forms.ToolStripButton();
            this.tsbtnTooling = new System.Windows.Forms.ToolStripButton();
            this.tsbtnNote = new System.Windows.Forms.ToolStripButton();
            this.tsbtnPDM = new System.Windows.Forms.ToolStripButton();
            this.tsbtnPhoto = new System.Windows.Forms.ToolStripButton();
            this.tsbtnMCI = new System.Windows.Forms.ToolStripButton();
            this.tsbtnInspSOP = new System.Windows.Forms.ToolStripButton();
            this.tsbtnQualityReport = new System.Windows.Forms.ToolStripButton();
            this.lablRoHSOn = new System.Windows.Forms.Label();
            this.btnStartRoHS = new System.Windows.Forms.Button();
            this.btnStopRoHS = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgRT = new System.Windows.Forms.DataGridView();
            this.RT_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RT_SEQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ACCEPT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REJECT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel11 = new System.Windows.Forms.Panel();
            this.lablPartNo = new System.Windows.Forms.Label();
            this.lablStartTime = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.lablSpec1 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lablVendor = new System.Windows.Forms.Label();
            this.lablLotSize = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.lablUrgentType = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.rbtnExceptionNo = new System.Windows.Forms.RadioButton();
            this.rbtnExceptionYes = new System.Windows.Forms.RadioButton();
            this.label17 = new System.Windows.Forms.Label();
            this.lablCriticalPartType = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.lablCreateEmp = new System.Windows.Forms.Label();
            this.lablSpecB = new System.Windows.Forms.Label();
            this.lablModelName = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lablSpecA = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lablWareHouse = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panelImg = new System.Windows.Forms.Panel();
            this.lablEndTime = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btWaiveResult = new System.Windows.Forms.Button();
            this.labResult = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cmsdefect.SuspendLayout();
            this.Panel10.SuspendLayout();
            this.panel2.SuspendLayout();
            this.cmsTestType.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemType)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpNormal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSampleType)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tpRoHS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoHSItem)).BeginInit();
            this.PanelRoHSResult.SuspendLayout();
            this.PanelRoHS.SuspendLayout();
            this.tpSnHistory.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSN)).BeginInit();
            this.panel5.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tpComponentPic.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPart3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPart4)).BeginInit();
            this.tabPageSize.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSize1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSize2)).BeginInit();
            this.panel9.SuspendLayout();
            this.panel6.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRT)).BeginInit();
            this.panel11.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsdefect
            // 
            this.cmsdefect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.cmsdefect.Name = "cmsdefect";
            resources.ApplyResources(this.cmsdefect, "cmsdefect");
            this.cmsdefect.Opening += new System.ComponentModel.CancelEventHandler(this.cmsdefect_Opening);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // Panel10
            // 
            this.Panel10.BackColor = System.Drawing.Color.LightBlue;
            this.Panel10.Controls.Add(this.btnShowRT);
            this.Panel10.Controls.Add(this.combLot);
            this.Panel10.Controls.Add(this.button1);
            this.Panel10.Controls.Add(this.sbtnRollback);
            this.Panel10.Controls.Add(this.btnRefresh);
            this.Panel10.Controls.Add(this.label2);
            this.Panel10.Controls.Add(this.DateInsp);
            this.Panel10.Controls.Add(this.label1);
            this.Panel10.Controls.Add(this.combLotNo);
            this.Panel10.Controls.Add(this.combItemSeq);
            resources.ApplyResources(this.Panel10, "Panel10");
            this.Panel10.Name = "Panel10";
            // 
            // btnShowRT
            // 
            resources.ApplyResources(this.btnShowRT, "btnShowRT");
            this.btnShowRT.Name = "btnShowRT";
            this.btnShowRT.UseVisualStyleBackColor = true;
            this.btnShowRT.Click += new System.EventHandler(this.btnShowRT_Click);
            // 
            // combLot
            // 
            this.combLot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.combLot, "combLot");
            this.combLot.FormattingEnabled = true;
            this.combLot.Name = "combLot";
            this.combLot.SelectedIndexChanged += new System.EventHandler(this.combLot_SelectedIndexChanged);
            this.combLot.TextChanged += new System.EventHandler(this.combLot_TextChanged);
            this.combLot.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.combLot_KeyPress);
            // 
            // button1
            // 
            this.button1.AutoEllipsis = true;
            resources.ApplyResources(this.button1, "button1");
            this.button1.ForeColor = System.Drawing.Color.Transparent;
            this.button1.Image = global::IQCbyLot.Properties.Resources.修改;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // sbtnRollback
            // 
            resources.ApplyResources(this.sbtnRollback, "sbtnRollback");
            this.sbtnRollback.Name = "sbtnRollback";
            this.sbtnRollback.UseVisualStyleBackColor = true;
            this.sbtnRollback.Click += new System.EventHandler(this.sbtnRollback_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.AutoEllipsis = true;
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.ImageList = this.imageList1;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "cycle.ico");
            this.imageList1.Images.SetKeyName(1, "NG.png");
            this.imageList1.Images.SetKeyName(2, "OK.png");
            this.imageList1.Images.SetKeyName(3, "WARNING.png");
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // DateInsp
            // 
            resources.ApplyResources(this.DateInsp, "DateInsp");
            this.DateInsp.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateInsp.Name = "DateInsp";
            this.DateInsp.Value = new System.DateTime(2009, 8, 10, 0, 0, 0, 0);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // combLotNo
            // 
            resources.ApplyResources(this.combLotNo, "combLotNo");
            this.combLotNo.FormattingEnabled = true;
            this.combLotNo.Name = "combLotNo";
            this.combLotNo.SelectedIndexChanged += new System.EventHandler(this.combLotNo_SelectedIndexChanged);
            this.combLotNo.TextChanged += new System.EventHandler(this.combLotNo_TextChanged);
            this.combLotNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.combLotNo_KeyPress);
            // 
            // combItemSeq
            // 
            this.combItemSeq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combItemSeq, "combItemSeq");
            this.combItemSeq.FormattingEnabled = true;
            this.combItemSeq.Name = "combItemSeq";
            this.combItemSeq.SelectedIndexChanged += new System.EventHandler(this.combItemSeq_SelectedIndexChanged);
            // 
            // ckbUrgent
            // 
            resources.ApplyResources(this.ckbUrgent, "ckbUrgent");
            this.ckbUrgent.Name = "ckbUrgent";
            this.ckbUrgent.UseVisualStyleBackColor = true;
            this.ckbUrgent.CheckedChanged += new System.EventHandler(this.ckbUrgent_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRDAdmit);
            this.panel2.Controls.Add(this.sbtnReject);
            this.panel2.Controls.Add(this.btERP);
            this.panel2.Controls.Add(this.sbtnSpecialWaive);
            this.panel2.Controls.Add(this.sbtnWaive);
            this.panel2.Controls.Add(this.sbtnByPass);
            this.panel2.Controls.Add(this.sbtnHold);
            this.panel2.Controls.Add(this.sbtnSorting);
            this.panel2.Controls.Add(this.sbtnPartialWaive);
            this.panel2.Controls.Add(this.sbtnPass);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnRDAdmit
            // 
            this.btnRDAdmit.BackColor = System.Drawing.Color.SteelBlue;
            resources.ApplyResources(this.btnRDAdmit, "btnRDAdmit");
            this.btnRDAdmit.ForeColor = System.Drawing.Color.White;
            this.btnRDAdmit.Name = "btnRDAdmit";
            this.btnRDAdmit.Tag = "8";
            this.btnRDAdmit.UseVisualStyleBackColor = false;
            this.btnRDAdmit.Click += new System.EventHandler(this.sbtnPass_Click);
            // 
            // sbtnReject
            // 
            this.sbtnReject.BackColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.sbtnReject, "sbtnReject");
            this.sbtnReject.ForeColor = System.Drawing.Color.White;
            this.sbtnReject.Name = "sbtnReject";
            this.sbtnReject.Tag = "1";
            this.sbtnReject.UseVisualStyleBackColor = false;
            this.sbtnReject.Click += new System.EventHandler(this.sbtnPass_Click);
            // 
            // btERP
            // 
            resources.ApplyResources(this.btERP, "btERP");
            this.btERP.ForeColor = System.Drawing.Color.Black;
            this.btERP.Name = "btERP";
            this.btERP.Tag = "";
            this.btERP.UseVisualStyleBackColor = false;
            this.btERP.Click += new System.EventHandler(this.btERP_Click);
            // 
            // sbtnSpecialWaive
            // 
            this.sbtnSpecialWaive.BackColor = System.Drawing.Color.Orange;
            resources.ApplyResources(this.sbtnSpecialWaive, "sbtnSpecialWaive");
            this.sbtnSpecialWaive.ForeColor = System.Drawing.Color.Black;
            this.sbtnSpecialWaive.Name = "sbtnSpecialWaive";
            this.sbtnSpecialWaive.Tag = "7";
            this.sbtnSpecialWaive.UseVisualStyleBackColor = false;
            this.sbtnSpecialWaive.Click += new System.EventHandler(this.sbtnPass_Click);
            // 
            // sbtnWaive
            // 
            this.sbtnWaive.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.sbtnWaive, "sbtnWaive");
            this.sbtnWaive.Name = "sbtnWaive";
            this.sbtnWaive.Tag = "2";
            this.sbtnWaive.UseVisualStyleBackColor = false;
            this.sbtnWaive.Click += new System.EventHandler(this.sbtnPass_Click);
            // 
            // sbtnByPass
            // 
            this.sbtnByPass.BackColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.sbtnByPass, "sbtnByPass");
            this.sbtnByPass.ForeColor = System.Drawing.Color.White;
            this.sbtnByPass.Name = "sbtnByPass";
            this.sbtnByPass.Tag = "4";
            this.sbtnByPass.UseVisualStyleBackColor = false;
            this.sbtnByPass.Click += new System.EventHandler(this.sbtnPass_Click);
            // 
            // sbtnHold
            // 
            resources.ApplyResources(this.sbtnHold, "sbtnHold");
            this.sbtnHold.ForeColor = System.Drawing.Color.Black;
            this.sbtnHold.Name = "sbtnHold";
            this.sbtnHold.Tag = "5";
            this.sbtnHold.UseVisualStyleBackColor = false;
            this.sbtnHold.Click += new System.EventHandler(this.sbtnPass_Click);
            // 
            // sbtnSorting
            // 
            this.sbtnSorting.BackColor = System.Drawing.Color.Purple;
            resources.ApplyResources(this.sbtnSorting, "sbtnSorting");
            this.sbtnSorting.ForeColor = System.Drawing.Color.White;
            this.sbtnSorting.Name = "sbtnSorting";
            this.sbtnSorting.Tag = "3";
            this.sbtnSorting.UseVisualStyleBackColor = false;
            this.sbtnSorting.Click += new System.EventHandler(this.sbtnPass_Click);
            // 
            // sbtnPartialWaive
            // 
            resources.ApplyResources(this.sbtnPartialWaive, "sbtnPartialWaive");
            this.sbtnPartialWaive.Name = "sbtnPartialWaive";
            this.sbtnPartialWaive.Tag = "6";
            this.sbtnPartialWaive.UseVisualStyleBackColor = true;
            // 
            // sbtnPass
            // 
            this.sbtnPass.BackColor = System.Drawing.Color.Green;
            resources.ApplyResources(this.sbtnPass, "sbtnPass");
            this.sbtnPass.ForeColor = System.Drawing.Color.White;
            this.sbtnPass.Name = "sbtnPass";
            this.sbtnPass.Tag = "0";
            this.sbtnPass.UseVisualStyleBackColor = false;
            this.sbtnPass.Click += new System.EventHandler(this.sbtnPass_Click);
            // 
            // cmsTestType
            // 
            this.cmsTestType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inspectItemToolStripMenuItem,
            this.changeSampleTypeToolStripMenuItem,
            this.importTestResultToolStripMenuItem});
            this.cmsTestType.Name = "cmsTestType";
            resources.ApplyResources(this.cmsTestType, "cmsTestType");
            // 
            // inspectItemToolStripMenuItem
            // 
            this.inspectItemToolStripMenuItem.Name = "inspectItemToolStripMenuItem";
            resources.ApplyResources(this.inspectItemToolStripMenuItem, "inspectItemToolStripMenuItem");
            this.inspectItemToolStripMenuItem.Click += new System.EventHandler(this.inspectItemToolStripMenuItem_Click);
            // 
            // changeSampleTypeToolStripMenuItem
            // 
            this.changeSampleTypeToolStripMenuItem.Name = "changeSampleTypeToolStripMenuItem";
            resources.ApplyResources(this.changeSampleTypeToolStripMenuItem, "changeSampleTypeToolStripMenuItem");
            this.changeSampleTypeToolStripMenuItem.Click += new System.EventHandler(this.btnAQL_Click);
            // 
            // importTestResultToolStripMenuItem
            // 
            this.importTestResultToolStripMenuItem.Name = "importTestResultToolStripMenuItem";
            resources.ApplyResources(this.importTestResultToolStripMenuItem, "importTestResultToolStripMenuItem");
            this.importTestResultToolStripMenuItem.Click += new System.EventHandler(this.importTestResultToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvItemType);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // dgvItemType
            // 
            this.dgvItemType.AllowUserToAddRows = false;
            this.dgvItemType.AllowUserToDeleteRows = false;
            this.dgvItemType.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dgvItemType, "dgvItemType");
            this.dgvItemType.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TYPE_CODE,
            this.RECID,
            this.SEQ,
            this.TYPE_NAME,
            this.SAMPLE_TYPE,
            this.SAMPLE_LEVEL,
            this.SAMPLE_SIZE,
            this.PASS_QTY,
            this.FAIL_QTY,
            this.QC_RESULT,
            this.EXIST,
            this.SAMPLING_ID,
            this.SAMPLE_LEVEL_ID,
            this.TYPE_ID});
            this.dgvItemType.ContextMenuStrip = this.cmsTestType;
            this.dgvItemType.EnableHeadersVisualStyles = false;
            this.dgvItemType.MultiSelect = false;
            this.dgvItemType.Name = "dgvItemType";
            this.dgvItemType.ReadOnly = true;
            this.dgvItemType.RowTemplate.Height = 24;
            this.dgvItemType.SelectionChanged += new System.EventHandler(this.dgvItemType_SelectionChanged);
            // 
            // TYPE_CODE
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TYPE_CODE.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.TYPE_CODE, "TYPE_CODE");
            this.TYPE_CODE.Name = "TYPE_CODE";
            this.TYPE_CODE.ReadOnly = true;
            // 
            // RECID
            // 
            resources.ApplyResources(this.RECID, "RECID");
            this.RECID.Name = "RECID";
            this.RECID.ReadOnly = true;
            // 
            // SEQ
            // 
            resources.ApplyResources(this.SEQ, "SEQ");
            this.SEQ.Name = "SEQ";
            this.SEQ.ReadOnly = true;
            // 
            // TYPE_NAME
            // 
            resources.ApplyResources(this.TYPE_NAME, "TYPE_NAME");
            this.TYPE_NAME.Name = "TYPE_NAME";
            this.TYPE_NAME.ReadOnly = true;
            // 
            // SAMPLE_TYPE
            // 
            resources.ApplyResources(this.SAMPLE_TYPE, "SAMPLE_TYPE");
            this.SAMPLE_TYPE.Name = "SAMPLE_TYPE";
            this.SAMPLE_TYPE.ReadOnly = true;
            // 
            // SAMPLE_LEVEL
            // 
            resources.ApplyResources(this.SAMPLE_LEVEL, "SAMPLE_LEVEL");
            this.SAMPLE_LEVEL.Name = "SAMPLE_LEVEL";
            this.SAMPLE_LEVEL.ReadOnly = true;
            // 
            // SAMPLE_SIZE
            // 
            resources.ApplyResources(this.SAMPLE_SIZE, "SAMPLE_SIZE");
            this.SAMPLE_SIZE.Name = "SAMPLE_SIZE";
            this.SAMPLE_SIZE.ReadOnly = true;
            // 
            // PASS_QTY
            // 
            resources.ApplyResources(this.PASS_QTY, "PASS_QTY");
            this.PASS_QTY.Name = "PASS_QTY";
            this.PASS_QTY.ReadOnly = true;
            // 
            // FAIL_QTY
            // 
            resources.ApplyResources(this.FAIL_QTY, "FAIL_QTY");
            this.FAIL_QTY.Name = "FAIL_QTY";
            this.FAIL_QTY.ReadOnly = true;
            // 
            // QC_RESULT
            // 
            resources.ApplyResources(this.QC_RESULT, "QC_RESULT");
            this.QC_RESULT.Name = "QC_RESULT";
            this.QC_RESULT.ReadOnly = true;
            // 
            // EXIST
            // 
            resources.ApplyResources(this.EXIST, "EXIST");
            this.EXIST.Name = "EXIST";
            this.EXIST.ReadOnly = true;
            // 
            // SAMPLING_ID
            // 
            resources.ApplyResources(this.SAMPLING_ID, "SAMPLING_ID");
            this.SAMPLING_ID.Name = "SAMPLING_ID";
            this.SAMPLING_ID.ReadOnly = true;
            // 
            // SAMPLE_LEVEL_ID
            // 
            resources.ApplyResources(this.SAMPLE_LEVEL_ID, "SAMPLE_LEVEL_ID");
            this.SAMPLE_LEVEL_ID.Name = "SAMPLE_LEVEL_ID";
            this.SAMPLE_LEVEL_ID.ReadOnly = true;
            // 
            // TYPE_ID
            // 
            resources.ApplyResources(this.TYPE_ID, "TYPE_ID");
            this.TYPE_ID.Name = "TYPE_ID";
            this.TYPE_ID.ReadOnly = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpNormal);
            this.tabControl1.Controls.Add(this.tpRoHS);
            this.tabControl1.Controls.Add(this.tpSnHistory);
            this.tabControl1.Controls.Add(this.tpComponentPic);
            this.tabControl1.Controls.Add(this.tabPageSize);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tpNormal
            // 
            this.tpNormal.Controls.Add(this.panel1);
            this.tpNormal.Controls.Add(this.editItemTypeName);
            this.tpNormal.Controls.Add(this.dgvSampleType);
            this.tpNormal.Controls.Add(this.tabControl2);
            this.tpNormal.Controls.Add(this.splitter1);
            resources.ApplyResources(this.tpNormal, "tpNormal");
            this.tpNormal.Name = "tpNormal";
            this.tpNormal.UseVisualStyleBackColor = true;
            // 
            // editItemTypeName
            // 
            this.editItemTypeName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editItemTypeName, "editItemTypeName");
            this.editItemTypeName.ForeColor = System.Drawing.Color.Black;
            this.editItemTypeName.Name = "editItemTypeName";
            this.editItemTypeName.ReadOnly = true;
            // 
            // dgvSampleType
            // 
            this.dgvSampleType.AllowUserToAddRows = false;
            this.dgvSampleType.AllowUserToDeleteRows = false;
            this.dgvSampleType.BackgroundColor = System.Drawing.Color.White;
            this.dgvSampleType.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvSampleType.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            resources.ApplyResources(this.dgvSampleType, "dgvSampleType");
            this.dgvSampleType.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TSAMPLE_TYPE,
            this.TSAMPLE_LEVEL,
            this.TSAMPLE_UNIT,
            this.TSAMPLE_SIZE,
            this.TCRITICAL,
            this.TMAJOR,
            this.TMINOR,
            this.TSAMPLE_LEVEL_ID,
            this.TSAMPLE_ID});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 11.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSampleType.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSampleType.EnableHeadersVisualStyles = false;
            this.dgvSampleType.MultiSelect = false;
            this.dgvSampleType.Name = "dgvSampleType";
            this.dgvSampleType.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 11.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSampleType.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSampleType.RowHeadersVisible = false;
            this.dgvSampleType.RowTemplate.Height = 24;
            this.dgvSampleType.RowTemplate.ReadOnly = true;
            this.dgvSampleType.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // TSAMPLE_TYPE
            // 
            resources.ApplyResources(this.TSAMPLE_TYPE, "TSAMPLE_TYPE");
            this.TSAMPLE_TYPE.Name = "TSAMPLE_TYPE";
            this.TSAMPLE_TYPE.ReadOnly = true;
            // 
            // TSAMPLE_LEVEL
            // 
            resources.ApplyResources(this.TSAMPLE_LEVEL, "TSAMPLE_LEVEL");
            this.TSAMPLE_LEVEL.Name = "TSAMPLE_LEVEL";
            this.TSAMPLE_LEVEL.ReadOnly = true;
            // 
            // TSAMPLE_UNIT
            // 
            resources.ApplyResources(this.TSAMPLE_UNIT, "TSAMPLE_UNIT");
            this.TSAMPLE_UNIT.Name = "TSAMPLE_UNIT";
            this.TSAMPLE_UNIT.ReadOnly = true;
            // 
            // TSAMPLE_SIZE
            // 
            resources.ApplyResources(this.TSAMPLE_SIZE, "TSAMPLE_SIZE");
            this.TSAMPLE_SIZE.Name = "TSAMPLE_SIZE";
            this.TSAMPLE_SIZE.ReadOnly = true;
            // 
            // TCRITICAL
            // 
            resources.ApplyResources(this.TCRITICAL, "TCRITICAL");
            this.TCRITICAL.Name = "TCRITICAL";
            this.TCRITICAL.ReadOnly = true;
            // 
            // TMAJOR
            // 
            resources.ApplyResources(this.TMAJOR, "TMAJOR");
            this.TMAJOR.Name = "TMAJOR";
            this.TMAJOR.ReadOnly = true;
            // 
            // TMINOR
            // 
            resources.ApplyResources(this.TMINOR, "TMINOR");
            this.TMINOR.Name = "TMINOR";
            this.TMINOR.ReadOnly = true;
            // 
            // TSAMPLE_LEVEL_ID
            // 
            resources.ApplyResources(this.TSAMPLE_LEVEL_ID, "TSAMPLE_LEVEL_ID");
            this.TSAMPLE_LEVEL_ID.Name = "TSAMPLE_LEVEL_ID";
            this.TSAMPLE_LEVEL_ID.ReadOnly = true;
            // 
            // TSAMPLE_ID
            // 
            resources.ApplyResources(this.TSAMPLE_ID, "TSAMPLE_ID");
            this.TSAMPLE_ID.Name = "TSAMPLE_ID";
            this.TSAMPLE_ID.ReadOnly = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl2, "tabControl2");
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvDefect);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvDefect
            // 
            this.dgvDefect.AllowUserToAddRows = false;
            this.dgvDefect.AllowUserToDeleteRows = false;
            this.dgvDefect.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dgvDefect, "dgvDefect");
            this.dgvDefect.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DEFECT_CODE,
            this.DEFECT_DESC,
            this.DEFECT_QTY,
            this.DEFECT_MEMO,
            this.DEFECT_ID});
            this.dgvDefect.EnableHeadersVisualStyles = false;
            this.dgvDefect.MultiSelect = false;
            this.dgvDefect.Name = "dgvDefect";
            this.dgvDefect.ReadOnly = true;
            this.dgvDefect.RowHeadersVisible = false;
            this.dgvDefect.RowTemplate.Height = 24;
            // 
            // DEFECT_CODE
            // 
            resources.ApplyResources(this.DEFECT_CODE, "DEFECT_CODE");
            this.DEFECT_CODE.Name = "DEFECT_CODE";
            this.DEFECT_CODE.ReadOnly = true;
            // 
            // DEFECT_DESC
            // 
            resources.ApplyResources(this.DEFECT_DESC, "DEFECT_DESC");
            this.DEFECT_DESC.Name = "DEFECT_DESC";
            this.DEFECT_DESC.ReadOnly = true;
            // 
            // DEFECT_QTY
            // 
            resources.ApplyResources(this.DEFECT_QTY, "DEFECT_QTY");
            this.DEFECT_QTY.Name = "DEFECT_QTY";
            this.DEFECT_QTY.ReadOnly = true;
            // 
            // DEFECT_MEMO
            // 
            resources.ApplyResources(this.DEFECT_MEMO, "DEFECT_MEMO");
            this.DEFECT_MEMO.Name = "DEFECT_MEMO";
            this.DEFECT_MEMO.ReadOnly = true;
            // 
            // DEFECT_ID
            // 
            resources.ApplyResources(this.DEFECT_ID, "DEFECT_ID");
            this.DEFECT_ID.Name = "DEFECT_ID";
            this.DEFECT_ID.ReadOnly = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.editTestTypeMemo);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // editTestTypeMemo
            // 
            this.editTestTypeMemo.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editTestTypeMemo, "editTestTypeMemo");
            this.editTestTypeMemo.Name = "editTestTypeMemo";
            this.editTestTypeMemo.ReadOnly = true;
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // tpRoHS
            // 
            this.tpRoHS.BackColor = System.Drawing.Color.Transparent;
            this.tpRoHS.Controls.Add(this.dgvRoHSItem);
            this.tpRoHS.Controls.Add(this.PanelRoHSResult);
            this.tpRoHS.Controls.Add(this.PanelRoHS);
            resources.ApplyResources(this.tpRoHS, "tpRoHS");
            this.tpRoHS.Name = "tpRoHS";
            this.tpRoHS.UseVisualStyleBackColor = true;
            // 
            // dgvRoHSItem
            // 
            this.dgvRoHSItem.AllowUserToAddRows = false;
            this.dgvRoHSItem.AllowUserToDeleteRows = false;
            this.dgvRoHSItem.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dgvRoHSItem, "dgvRoHSItem");
            this.dgvRoHSItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.POSITION,
            this.PB,
            this.CD,
            this.HG,
            this.CR,
            this.BR,
            this.CL,
            this.MEMO});
            this.dgvRoHSItem.EnableHeadersVisualStyles = false;
            this.dgvRoHSItem.MultiSelect = false;
            this.dgvRoHSItem.Name = "dgvRoHSItem";
            this.dgvRoHSItem.ReadOnly = true;
            this.dgvRoHSItem.RowHeadersVisible = false;
            this.dgvRoHSItem.RowTemplate.Height = 24;
            // 
            // POSITION
            // 
            resources.ApplyResources(this.POSITION, "POSITION");
            this.POSITION.Name = "POSITION";
            this.POSITION.ReadOnly = true;
            // 
            // PB
            // 
            resources.ApplyResources(this.PB, "PB");
            this.PB.Name = "PB";
            this.PB.ReadOnly = true;
            // 
            // CD
            // 
            resources.ApplyResources(this.CD, "CD");
            this.CD.Name = "CD";
            this.CD.ReadOnly = true;
            // 
            // HG
            // 
            resources.ApplyResources(this.HG, "HG");
            this.HG.Name = "HG";
            this.HG.ReadOnly = true;
            // 
            // CR
            // 
            resources.ApplyResources(this.CR, "CR");
            this.CR.Name = "CR";
            this.CR.ReadOnly = true;
            // 
            // BR
            // 
            resources.ApplyResources(this.BR, "BR");
            this.BR.Name = "BR";
            this.BR.ReadOnly = true;
            // 
            // CL
            // 
            resources.ApplyResources(this.CL, "CL");
            this.CL.Name = "CL";
            this.CL.ReadOnly = true;
            // 
            // MEMO
            // 
            resources.ApplyResources(this.MEMO, "MEMO");
            this.MEMO.Name = "MEMO";
            this.MEMO.ReadOnly = true;
            // 
            // PanelRoHSResult
            // 
            this.PanelRoHSResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.PanelRoHSResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelRoHSResult.Controls.Add(this.btnRollbackRoHs);
            this.PanelRoHSResult.Controls.Add(this.label13);
            this.PanelRoHSResult.Controls.Add(this.editRoHSMemo);
            this.PanelRoHSResult.Controls.Add(this.lablRoHSResult);
            this.PanelRoHSResult.Controls.Add(this.btnRoHSNG);
            this.PanelRoHSResult.Controls.Add(this.btnRoHSOK);
            this.PanelRoHSResult.Controls.Add(this.label12);
            resources.ApplyResources(this.PanelRoHSResult, "PanelRoHSResult");
            this.PanelRoHSResult.Name = "PanelRoHSResult";
            // 
            // btnRollbackRoHs
            // 
            resources.ApplyResources(this.btnRollbackRoHs, "btnRollbackRoHs");
            this.btnRollbackRoHs.Name = "btnRollbackRoHs";
            this.btnRollbackRoHs.UseVisualStyleBackColor = true;
            this.btnRollbackRoHs.Click += new System.EventHandler(this.btnRollbackRoHs_Click);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // editRoHSMemo
            // 
            resources.ApplyResources(this.editRoHSMemo, "editRoHSMemo");
            this.editRoHSMemo.Name = "editRoHSMemo";
            // 
            // lablRoHSResult
            // 
            resources.ApplyResources(this.lablRoHSResult, "lablRoHSResult");
            this.lablRoHSResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lablRoHSResult.Name = "lablRoHSResult";
            // 
            // btnRoHSNG
            // 
            resources.ApplyResources(this.btnRoHSNG, "btnRoHSNG");
            this.btnRoHSNG.ForeColor = System.Drawing.Color.Black;
            this.btnRoHSNG.Name = "btnRoHSNG";
            this.btnRoHSNG.Tag = "1";
            this.btnRoHSNG.UseVisualStyleBackColor = true;
            this.btnRoHSNG.Click += new System.EventHandler(this.btnRoHSOK_Click);
            // 
            // btnRoHSOK
            // 
            resources.ApplyResources(this.btnRoHSOK, "btnRoHSOK");
            this.btnRoHSOK.ForeColor = System.Drawing.Color.Black;
            this.btnRoHSOK.Name = "btnRoHSOK";
            this.btnRoHSOK.Tag = "0";
            this.btnRoHSOK.UseVisualStyleBackColor = true;
            this.btnRoHSOK.Click += new System.EventHandler(this.btnRoHSOK_Click);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label12.Name = "label12";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // PanelRoHS
            // 
            this.PanelRoHS.Controls.Add(this.label15);
            this.PanelRoHS.Controls.Add(this.combItemSeq1);
            this.PanelRoHS.Controls.Add(this.btnCopy);
            this.PanelRoHS.Controls.Add(this.btnDelete);
            this.PanelRoHS.Controls.Add(this.btnModify);
            this.PanelRoHS.Controls.Add(this.btnAppend);
            resources.ApplyResources(this.PanelRoHS, "PanelRoHS");
            this.PanelRoHS.Name = "PanelRoHS";
            this.PanelRoHS.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelRoHS_Paint);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // combItemSeq1
            // 
            this.combItemSeq1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combItemSeq1, "combItemSeq1");
            this.combItemSeq1.FormattingEnabled = true;
            this.combItemSeq1.Name = "combItemSeq1";
            // 
            // btnCopy
            // 
            resources.ApplyResources(this.btnCopy, "btnCopy");
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            resources.ApplyResources(this.btnModify, "btnModify");
            this.btnModify.Name = "btnModify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnAppend
            // 
            resources.ApplyResources(this.btnAppend, "btnAppend");
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // tpSnHistory
            // 
            this.tpSnHistory.Controls.Add(this.panel7);
            this.tpSnHistory.Controls.Add(this.panel5);
            resources.ApplyResources(this.tpSnHistory, "tpSnHistory");
            this.tpSnHistory.Name = "tpSnHistory";
            this.tpSnHistory.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.dgvSN);
            resources.ApplyResources(this.panel7, "panel7");
            this.panel7.Name = "panel7";
            // 
            // dgvSN
            // 
            this.dgvSN.AllowUserToAddRows = false;
            this.dgvSN.AllowUserToDeleteRows = false;
            this.dgvSN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSN.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SERIAL_NUMBER,
            this.DATECODE,
            this.TEST_VALUE,
            this.REMARK,
            this.CREATE_TIME,
            this.CREATE_USER});
            resources.ApplyResources(this.dgvSN, "dgvSN");
            this.dgvSN.Name = "dgvSN";
            this.dgvSN.RowTemplate.Height = 24;
            this.dgvSN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // SERIAL_NUMBER
            // 
            resources.ApplyResources(this.SERIAL_NUMBER, "SERIAL_NUMBER");
            this.SERIAL_NUMBER.Name = "SERIAL_NUMBER";
            this.SERIAL_NUMBER.ReadOnly = true;
            // 
            // DATECODE
            // 
            resources.ApplyResources(this.DATECODE, "DATECODE");
            this.DATECODE.Name = "DATECODE";
            this.DATECODE.ReadOnly = true;
            // 
            // TEST_VALUE
            // 
            resources.ApplyResources(this.TEST_VALUE, "TEST_VALUE");
            this.TEST_VALUE.Name = "TEST_VALUE";
            this.TEST_VALUE.ReadOnly = true;
            // 
            // REMARK
            // 
            resources.ApplyResources(this.REMARK, "REMARK");
            this.REMARK.Name = "REMARK";
            this.REMARK.ReadOnly = true;
            // 
            // CREATE_TIME
            // 
            resources.ApplyResources(this.CREATE_TIME, "CREATE_TIME");
            this.CREATE_TIME.Name = "CREATE_TIME";
            this.CREATE_TIME.ReadOnly = true;
            // 
            // CREATE_USER
            // 
            resources.ApplyResources(this.CREATE_USER, "CREATE_USER");
            this.CREATE_USER.Name = "CREATE_USER";
            this.CREATE_USER.ReadOnly = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.toolStrip2);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripInsert,
            this.toolStripSeparator1,
            this.toolStripModify,
            this.toolStripSeparator2,
            this.toolStripDelete});
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Name = "toolStrip2";
            // 
            // toolStripInsert
            // 
            resources.ApplyResources(this.toolStripInsert, "toolStripInsert");
            this.toolStripInsert.Name = "toolStripInsert";
            this.toolStripInsert.Click += new System.EventHandler(this.toolStripInsert_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripModify
            // 
            resources.ApplyResources(this.toolStripModify, "toolStripModify");
            this.toolStripModify.Name = "toolStripModify";
            this.toolStripModify.Click += new System.EventHandler(this.toolStripModify_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripDelete
            // 
            resources.ApplyResources(this.toolStripDelete, "toolStripDelete");
            this.toolStripDelete.Name = "toolStripDelete";
            this.toolStripDelete.Click += new System.EventHandler(this.toolStripDelete_Click);
            // 
            // tpComponentPic
            // 
            this.tpComponentPic.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.tpComponentPic, "tpComponentPic");
            this.tpComponentPic.Name = "tpComponentPic";
            this.tpComponentPic.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.picPart1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.picPart2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.picPart3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.picPart4, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // picPart1
            // 
            resources.ApplyResources(this.picPart1, "picPart1");
            this.picPart1.Name = "picPart1";
            this.picPart1.TabStop = false;
            this.picPart1.Tag = "1";
            this.picPart1.Click += new System.EventHandler(this.picPart1_Click);
            // 
            // picPart2
            // 
            resources.ApplyResources(this.picPart2, "picPart2");
            this.picPart2.Name = "picPart2";
            this.picPart2.TabStop = false;
            this.picPart2.Tag = "2";
            this.picPart2.Click += new System.EventHandler(this.picPart1_Click);
            // 
            // picPart3
            // 
            resources.ApplyResources(this.picPart3, "picPart3");
            this.picPart3.Name = "picPart3";
            this.picPart3.TabStop = false;
            this.picPart3.Tag = "3";
            this.picPart3.Click += new System.EventHandler(this.picPart1_Click);
            // 
            // picPart4
            // 
            resources.ApplyResources(this.picPart4, "picPart4");
            this.picPart4.Name = "picPart4";
            this.picPart4.TabStop = false;
            this.picPart4.Tag = "4";
            this.picPart4.Click += new System.EventHandler(this.picPart1_Click);
            // 
            // tabPageSize
            // 
            this.tabPageSize.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.tabPageSize, "tabPageSize");
            this.tabPageSize.Name = "tabPageSize";
            this.tabPageSize.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.picBoxSize1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.picBoxSize2, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // picBoxSize1
            // 
            resources.ApplyResources(this.picBoxSize1, "picBoxSize1");
            this.picBoxSize1.Name = "picBoxSize1";
            this.picBoxSize1.TabStop = false;
            this.picBoxSize1.Tag = "5";
            this.picBoxSize1.Click += new System.EventHandler(this.picPart1_Click);
            // 
            // picBoxSize2
            // 
            resources.ApplyResources(this.picBoxSize2, "picBoxSize2");
            this.picBoxSize2.Name = "picBoxSize2";
            this.picBoxSize2.TabStop = false;
            this.picBoxSize2.Tag = "6";
            this.picBoxSize2.Click += new System.EventHandler(this.picPart1_Click);
            // 
            // l5
            // 
            this.l5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.l5.ForeColor = System.Drawing.Color.Maroon;
            resources.ApplyResources(this.l5, "l5");
            this.l5.Name = "l5";
            // 
            // l3
            // 
            this.l3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.l3.ForeColor = System.Drawing.Color.Maroon;
            resources.ApplyResources(this.l3, "l3");
            this.l3.Name = "l3";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // l2
            // 
            this.l2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.l2.ForeColor = System.Drawing.Color.Maroon;
            resources.ApplyResources(this.l2, "l2");
            this.l2.Name = "l2";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lablPONo
            // 
            this.lablPONo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablPONo, "lablPONo");
            this.lablPONo.ForeColor = System.Drawing.Color.Maroon;
            this.lablPONo.Name = "lablPONo";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // l4
            // 
            this.l4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.l4.ForeColor = System.Drawing.Color.Maroon;
            resources.ApplyResources(this.l4, "l4");
            this.l4.Name = "l4";
            // 
            // l
            // 
            this.l.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.l.ForeColor = System.Drawing.Color.Maroon;
            resources.ApplyResources(this.l, "l");
            this.l.Name = "l";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.panel6);
            resources.ApplyResources(this.panel9, "panel9");
            this.panel9.Name = "panel9";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnHistory,
            this.tsbtnMPN,
            this.tsbtnTooling,
            this.tsbtnNote,
            this.tsbtnPDM,
            this.tsbtnPhoto,
            this.tsbtnMCI,
            this.tsbtnInspSOP,
            this.tsbtnQualityReport});
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // tsbtnHistory
            // 
            this.tsbtnHistory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tsbtnHistory, "tsbtnHistory");
            this.tsbtnHistory.Name = "tsbtnHistory";
            this.tsbtnHistory.Click += new System.EventHandler(this.tsbtnHistory_Click);
            // 
            // tsbtnMPN
            // 
            this.tsbtnMPN.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tsbtnMPN, "tsbtnMPN");
            this.tsbtnMPN.Name = "tsbtnMPN";
            this.tsbtnMPN.Click += new System.EventHandler(this.tsbtnMPN_Click);
            // 
            // tsbtnTooling
            // 
            this.tsbtnTooling.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tsbtnTooling, "tsbtnTooling");
            this.tsbtnTooling.Name = "tsbtnTooling";
            this.tsbtnTooling.Click += new System.EventHandler(this.tsbtnTooling_Click);
            // 
            // tsbtnNote
            // 
            this.tsbtnNote.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tsbtnNote, "tsbtnNote");
            this.tsbtnNote.Name = "tsbtnNote";
            this.tsbtnNote.Click += new System.EventHandler(this.tsbtnNote_Click);
            // 
            // tsbtnPDM
            // 
            this.tsbtnPDM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tsbtnPDM, "tsbtnPDM");
            this.tsbtnPDM.Name = "tsbtnPDM";
            this.tsbtnPDM.Click += new System.EventHandler(this.tsbtnPDM_Click);
            // 
            // tsbtnPhoto
            // 
            this.tsbtnPhoto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tsbtnPhoto, "tsbtnPhoto");
            this.tsbtnPhoto.Name = "tsbtnPhoto";
            this.tsbtnPhoto.Tag = "1";
            this.tsbtnPhoto.Click += new System.EventHandler(this.tsbtnPhoto_Click);
            // 
            // tsbtnMCI
            // 
            this.tsbtnMCI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tsbtnMCI, "tsbtnMCI");
            this.tsbtnMCI.Name = "tsbtnMCI";
            this.tsbtnMCI.Tag = "2";
            this.tsbtnMCI.Click += new System.EventHandler(this.tsbtnPhoto_Click);
            // 
            // tsbtnInspSOP
            // 
            this.tsbtnInspSOP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tsbtnInspSOP, "tsbtnInspSOP");
            this.tsbtnInspSOP.Name = "tsbtnInspSOP";
            this.tsbtnInspSOP.Tag = "3";
            this.tsbtnInspSOP.Click += new System.EventHandler(this.tsbtnPhoto_Click);
            // 
            // tsbtnQualityReport
            // 
            this.tsbtnQualityReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.tsbtnQualityReport, "tsbtnQualityReport");
            this.tsbtnQualityReport.Name = "tsbtnQualityReport";
            this.tsbtnQualityReport.Tag = "4";
            this.tsbtnQualityReport.Click += new System.EventHandler(this.tsbtnPhoto_Click);
            // 
            // lablRoHSOn
            // 
            this.lablRoHSOn.BackColor = System.Drawing.Color.Red;
            this.lablRoHSOn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.lablRoHSOn, "lablRoHSOn");
            this.lablRoHSOn.ForeColor = System.Drawing.Color.Yellow;
            this.lablRoHSOn.Name = "lablRoHSOn";
            // 
            // btnStartRoHS
            // 
            resources.ApplyResources(this.btnStartRoHS, "btnStartRoHS");
            this.btnStartRoHS.Name = "btnStartRoHS";
            this.btnStartRoHS.UseVisualStyleBackColor = true;
            this.btnStartRoHS.Click += new System.EventHandler(this.btnStartRoHS_Click);
            // 
            // btnStopRoHS
            // 
            resources.ApplyResources(this.btnStopRoHS, "btnStopRoHS");
            this.btnStopRoHS.Name = "btnStopRoHS";
            this.btnStopRoHS.UseVisualStyleBackColor = true;
            this.btnStopRoHS.Click += new System.EventHandler(this.btnStopRoHS_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgRT);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // dgRT
            // 
            this.dgRT.AllowUserToAddRows = false;
            this.dgRT.AllowUserToDeleteRows = false;
            this.dgRT.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgRT.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.dgRT, "dgRT");
            this.dgRT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RT_NO,
            this.PO,
            this.QTY,
            this.RT_SEQ,
            this.ACCEPT_QTY,
            this.REJECT_QTY});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("新細明體", 12F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgRT.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgRT.EnableHeadersVisualStyles = false;
            this.dgRT.MultiSelect = false;
            this.dgRT.Name = "dgRT";
            this.dgRT.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("新細明體", 12F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgRT.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgRT.RowHeadersVisible = false;
            this.dgRT.RowTemplate.Height = 24;
            this.dgRT.RowTemplate.ReadOnly = true;
            this.dgRT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // RT_NO
            // 
            resources.ApplyResources(this.RT_NO, "RT_NO");
            this.RT_NO.Name = "RT_NO";
            this.RT_NO.ReadOnly = true;
            // 
            // PO
            // 
            resources.ApplyResources(this.PO, "PO");
            this.PO.Name = "PO";
            this.PO.ReadOnly = true;
            // 
            // QTY
            // 
            resources.ApplyResources(this.QTY, "QTY");
            this.QTY.Name = "QTY";
            this.QTY.ReadOnly = true;
            // 
            // RT_SEQ
            // 
            resources.ApplyResources(this.RT_SEQ, "RT_SEQ");
            this.RT_SEQ.Name = "RT_SEQ";
            this.RT_SEQ.ReadOnly = true;
            // 
            // ACCEPT_QTY
            // 
            resources.ApplyResources(this.ACCEPT_QTY, "ACCEPT_QTY");
            this.ACCEPT_QTY.Name = "ACCEPT_QTY";
            this.ACCEPT_QTY.ReadOnly = true;
            // 
            // REJECT_QTY
            // 
            resources.ApplyResources(this.REJECT_QTY, "REJECT_QTY");
            this.REJECT_QTY.Name = "REJECT_QTY";
            this.REJECT_QTY.ReadOnly = true;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.tabControl1);
            resources.ApplyResources(this.panel11, "panel11");
            this.panel11.Name = "panel11";
            // 
            // lablPartNo
            // 
            this.lablPartNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablPartNo, "lablPartNo");
            this.lablPartNo.ForeColor = System.Drawing.Color.Maroon;
            this.lablPartNo.Name = "lablPartNo";
            // 
            // lablStartTime
            // 
            this.lablStartTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablStartTime, "lablStartTime");
            this.lablStartTime.ForeColor = System.Drawing.Color.Maroon;
            this.lablStartTime.Name = "lablStartTime";
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // lablSpec1
            // 
            resources.ApplyResources(this.lablSpec1, "lablSpec1");
            this.lablSpec1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablSpec1.ForeColor = System.Drawing.Color.Maroon;
            this.lablSpec1.Name = "lablSpec1";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // lablVendor
            // 
            this.lablVendor.AutoEllipsis = true;
            this.lablVendor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablVendor, "lablVendor");
            this.lablVendor.ForeColor = System.Drawing.Color.Maroon;
            this.lablVendor.Name = "lablVendor";
            // 
            // lablLotSize
            // 
            this.lablLotSize.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablLotSize, "lablLotSize");
            this.lablLotSize.ForeColor = System.Drawing.Color.Maroon;
            this.lablLotSize.Name = "lablLotSize";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.lablUrgentType);
            this.panel8.Controls.Add(this.label20);
            this.panel8.Controls.Add(this.rbtnExceptionNo);
            this.panel8.Controls.Add(this.rbtnExceptionYes);
            this.panel8.Controls.Add(this.label17);
            this.panel8.Controls.Add(this.lablCriticalPartType);
            this.panel8.Controls.Add(this.panel3);
            this.panel8.Controls.Add(this.label11);
            this.panel8.Controls.Add(this.lablCreateEmp);
            this.panel8.Controls.Add(this.lablSpecB);
            this.panel8.Controls.Add(this.lablModelName);
            this.panel8.Controls.Add(this.label14);
            this.panel8.Controls.Add(this.lablSpecA);
            this.panel8.Controls.Add(this.label10);
            this.panel8.Controls.Add(this.lablWareHouse);
            this.panel8.Controls.Add(this.label9);
            this.panel8.Controls.Add(this.panelImg);
            this.panel8.Controls.Add(this.lablEndTime);
            this.panel8.Controls.Add(this.ckbUrgent);
            this.panel8.Controls.Add(this.label4);
            this.panel8.Controls.Add(this.btWaiveResult);
            this.panel8.Controls.Add(this.panel4);
            this.panel8.Controls.Add(this.labResult);
            this.panel8.Controls.Add(this.label16);
            this.panel8.Controls.Add(this.lablLotSize);
            this.panel8.Controls.Add(this.lablVendor);
            this.panel8.Controls.Add(this.label18);
            this.panel8.Controls.Add(this.label19);
            this.panel8.Controls.Add(this.lablSpec1);
            this.panel8.Controls.Add(this.label23);
            this.panel8.Controls.Add(this.label24);
            this.panel8.Controls.Add(this.lablStartTime);
            this.panel8.Controls.Add(this.lablPartNo);
            resources.ApplyResources(this.panel8, "panel8");
            this.panel8.Name = "panel8";
            // 
            // lablUrgentType
            // 
            this.lablUrgentType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablUrgentType, "lablUrgentType");
            this.lablUrgentType.ForeColor = System.Drawing.Color.Maroon;
            this.lablUrgentType.Name = "lablUrgentType";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.ForeColor = System.Drawing.Color.Black;
            this.label20.Name = "label20";
            // 
            // rbtnExceptionNo
            // 
            resources.ApplyResources(this.rbtnExceptionNo, "rbtnExceptionNo");
            this.rbtnExceptionNo.Name = "rbtnExceptionNo";
            this.rbtnExceptionNo.TabStop = true;
            this.rbtnExceptionNo.Tag = "N";
            this.rbtnExceptionNo.UseVisualStyleBackColor = true;
            this.rbtnExceptionNo.Click += new System.EventHandler(this.rbtnExceptionYes_Click);
            // 
            // rbtnExceptionYes
            // 
            resources.ApplyResources(this.rbtnExceptionYes, "rbtnExceptionYes");
            this.rbtnExceptionYes.Name = "rbtnExceptionYes";
            this.rbtnExceptionYes.TabStop = true;
            this.rbtnExceptionYes.Tag = "Y";
            this.rbtnExceptionYes.UseVisualStyleBackColor = true;
            this.rbtnExceptionYes.Click += new System.EventHandler(this.rbtnExceptionYes_Click);
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label17.Name = "label17";
            // 
            // lablCriticalPartType
            // 
            this.lablCriticalPartType.AutoEllipsis = true;
            this.lablCriticalPartType.BackColor = System.Drawing.Color.Orange;
            this.lablCriticalPartType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablCriticalPartType, "lablCriticalPartType");
            this.lablCriticalPartType.ForeColor = System.Drawing.Color.Maroon;
            this.lablCriticalPartType.Name = "lablCriticalPartType";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnStartRoHS);
            this.panel3.Controls.Add(this.lablRoHSOn);
            this.panel3.Controls.Add(this.btnStopRoHS);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // lablCreateEmp
            // 
            this.lablCreateEmp.AutoEllipsis = true;
            this.lablCreateEmp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablCreateEmp, "lablCreateEmp");
            this.lablCreateEmp.ForeColor = System.Drawing.Color.Maroon;
            this.lablCreateEmp.Name = "lablCreateEmp";
            // 
            // lablSpecB
            // 
            this.lablSpecB.AutoEllipsis = true;
            this.lablSpecB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablSpecB, "lablSpecB");
            this.lablSpecB.ForeColor = System.Drawing.Color.Maroon;
            this.lablSpecB.Name = "lablSpecB";
            // 
            // lablModelName
            // 
            this.lablModelName.AutoEllipsis = true;
            this.lablModelName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablModelName, "lablModelName");
            this.lablModelName.ForeColor = System.Drawing.Color.Maroon;
            this.lablModelName.Name = "lablModelName";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // lablSpecA
            // 
            this.lablSpecA.AutoEllipsis = true;
            this.lablSpecA.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablSpecA, "lablSpecA");
            this.lablSpecA.ForeColor = System.Drawing.Color.Maroon;
            this.lablSpecA.Name = "lablSpecA";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // lablWareHouse
            // 
            resources.ApplyResources(this.lablWareHouse, "lablWareHouse");
            this.lablWareHouse.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablWareHouse.ForeColor = System.Drawing.Color.Maroon;
            this.lablWareHouse.Name = "lablWareHouse";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // panelImg
            // 
            resources.ApplyResources(this.panelImg, "panelImg");
            this.panelImg.Name = "panelImg";
            // 
            // lablEndTime
            // 
            this.lablEndTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lablEndTime, "lablEndTime");
            this.lablEndTime.ForeColor = System.Drawing.Color.Maroon;
            this.lablEndTime.Name = "lablEndTime";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btWaiveResult
            // 
            resources.ApplyResources(this.btWaiveResult, "btWaiveResult");
            this.btWaiveResult.Name = "btWaiveResult";
            this.btWaiveResult.Tag = "1";
            this.btWaiveResult.UseVisualStyleBackColor = true;
            this.btWaiveResult.Click += new System.EventHandler(this.btWaiveResult_Click);
            // 
            // labResult
            // 
            resources.ApplyResources(this.labResult, "labResult");
            this.labResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labResult.ForeColor = System.Drawing.Color.Red;
            this.labResult.Name = "labResult";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label16.Name = "label16";
            // 
            // timer1
            // 
            this.timer1.Interval = 700;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // fMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.Panel10);
            this.Name = "fMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fMain_Load);
            this.cmsdefect.ResumeLayout(false);
            this.Panel10.ResumeLayout(false);
            this.Panel10.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.cmsTestType.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemType)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpNormal.ResumeLayout(false);
            this.tpNormal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSampleType)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tpRoHS.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoHSItem)).EndInit();
            this.PanelRoHSResult.ResumeLayout(false);
            this.PanelRoHSResult.PerformLayout();
            this.PanelRoHS.ResumeLayout(false);
            this.PanelRoHS.PerformLayout();
            this.tpSnHistory.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSN)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tpComponentPic.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPart3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPart4)).EndInit();
            this.tabPageSize.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSize1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSize2)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgRT)).EndInit();
            this.panel11.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox combLotNo;
        private System.Windows.Forms.ComboBox combItemSeq;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button sbtnPass;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DateInsp;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip cmsdefect;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsTestType;
        private System.Windows.Forms.ToolStripMenuItem inspectItemToolStripMenuItem;
        private System.Windows.Forms.Button sbtnReject;
        private System.Windows.Forms.Button sbtnWaive;
        private System.Windows.Forms.Button sbtnByPass;
        private System.Windows.Forms.Button sbtnHold;
        private System.Windows.Forms.Button sbtnSorting;
        private System.Windows.Forms.Button sbtnPartialWaive;
        private System.Windows.Forms.ToolStripMenuItem changeSampleTypeToolStripMenuItem;
        private System.Windows.Forms.Button sbtnRollback;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpNormal;
        private System.Windows.Forms.TabPage tpRoHS;
        private System.Windows.Forms.DataGridView dgvRoHSItem;
        private System.Windows.Forms.Panel PanelRoHS;
        private System.Windows.Forms.Panel PanelRoHSResult;
        private System.Windows.Forms.Button btnRoHSNG;
        private System.Windows.Forms.Button btnRoHSOK;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lablRoHSResult;
        private System.Windows.Forms.Button sbtnSpecialWaive;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.DataGridViewTextBoxColumn POSITION;
        private System.Windows.Forms.DataGridViewTextBoxColumn PB;
        private System.Windows.Forms.DataGridViewTextBoxColumn CD;
        private System.Windows.Forms.DataGridViewTextBoxColumn HG;
        private System.Windows.Forms.DataGridViewTextBoxColumn CR;
        private System.Windows.Forms.DataGridViewTextBoxColumn BR;
        private System.Windows.Forms.DataGridViewTextBoxColumn CL;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEMO;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.TextBox editRoHSMemo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnRollbackRoHs;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.ComboBox combItemSeq1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox combLot;
        private System.Windows.Forms.Label l5;
        private System.Windows.Forms.Label l3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label l2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lablPONo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label l4;
        private System.Windows.Forms.Label l;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Button btERP;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Label lablPartNo;
        private System.Windows.Forms.Label lablStartTime;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lablSpec1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lablVendor;
        private System.Windows.Forms.Label lablLotSize;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label labResult;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ToolStripMenuItem importTestResultToolStripMenuItem;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label lablRoHSOn;
        private System.Windows.Forms.Button btnStartRoHS;
        private System.Windows.Forms.Button btnStopRoHS;
        private System.Windows.Forms.DataGridView dgvDefect;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_DESC;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_MEMO;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_ID;
        public System.Windows.Forms.TextBox editTestTypeMemo;
        private System.Windows.Forms.TextBox editItemTypeName;
        private System.Windows.Forms.DataGridView dgvSampleType;
        private System.Windows.Forms.DataGridViewTextBoxColumn TSAMPLE_TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TSAMPLE_LEVEL;
        private System.Windows.Forms.DataGridViewTextBoxColumn TSAMPLE_UNIT;
        private System.Windows.Forms.DataGridViewTextBoxColumn TSAMPLE_SIZE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TCRITICAL;
        private System.Windows.Forms.DataGridViewTextBoxColumn TMAJOR;
        private System.Windows.Forms.DataGridViewTextBoxColumn TMINOR;
        private System.Windows.Forms.DataGridViewTextBoxColumn TSAMPLE_LEVEL_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TSAMPLE_ID;
        private System.Windows.Forms.DataGridView dgvItemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn TYPE_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn RECID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SEQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn TYPE_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAMPLE_TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAMPLE_LEVEL;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAMPLE_SIZE;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASS_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn FAIL_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn QC_RESULT;
        private System.Windows.Forms.DataGridViewTextBoxColumn EXIST;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAMPLING_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAMPLE_LEVEL_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TYPE_ID;
        private System.Windows.Forms.Button btnShowRT;
        private System.Windows.Forms.DataGridView dgRT;
        private System.Windows.Forms.DataGridViewTextBoxColumn RT_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PO;
        private System.Windows.Forms.DataGridViewTextBoxColumn QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn RT_SEQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ACCEPT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn REJECT_QTY;
        private System.Windows.Forms.Button btWaiveResult;
        private System.Windows.Forms.CheckBox ckbUrgent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lablEndTime;
        private System.Windows.Forms.Panel panelImg;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnHistory;
        private System.Windows.Forms.ToolStripButton tsbtnMPN;
        private System.Windows.Forms.ToolStripButton tsbtnTooling;
        private System.Windows.Forms.ToolStripButton tsbtnNote;
        private System.Windows.Forms.ToolStripButton tsbtnPDM;
        private System.Windows.Forms.ToolStripButton tsbtnPhoto;
        private System.Windows.Forms.ToolStripButton tsbtnMCI;
        private System.Windows.Forms.ToolStripButton tsbtnInspSOP;
        private System.Windows.Forms.Label lablWareHouse;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripButton tsbtnQualityReport;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lablSpecA;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lablModelName;
        private System.Windows.Forms.Label lablSpecB;
        private System.Windows.Forms.Label lablCreateEmp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tpSnHistory;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripInsert;
        private System.Windows.Forms.ToolStripButton toolStripModify;
        private System.Windows.Forms.ToolStripButton toolStripDelete;
        private System.Windows.Forms.DataGridView dgvSN;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataGridViewTextBoxColumn SERIAL_NUMBER;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATECODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TEST_VALUE;
        private System.Windows.Forms.DataGridViewTextBoxColumn REMARK;
        private System.Windows.Forms.DataGridViewTextBoxColumn CREATE_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn CREATE_USER;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lablCriticalPartType;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.RadioButton rbtnExceptionNo;
        private System.Windows.Forms.RadioButton rbtnExceptionYes;
        private System.Windows.Forms.TabPage tpComponentPic;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox picPart1;
        private System.Windows.Forms.PictureBox picPart2;
        private System.Windows.Forms.PictureBox picPart3;
        private System.Windows.Forms.PictureBox picPart4;
        private System.Windows.Forms.TabPage tabPageSize;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox picBoxSize1;
        private System.Windows.Forms.PictureBox picBoxSize2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lablUrgentType;
        private System.Windows.Forms.Button btnRDAdmit;
    }
}