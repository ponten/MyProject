namespace InvPrintReeldll
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.editReel = new System.Windows.Forms.TextBox();
            this.editPrintQty = new System.Windows.Forms.NumericUpDown();
            this.btnPrint = new System.Windows.Forms.Button();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.gbChange = new System.Windows.Forms.GroupBox();
            this.dgvRT = new System.Windows.Forms.DataGridView();
            this.REEL_NO1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PART_NO1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REEL_QTY1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATECODE1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOT1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VENDOR_CODE1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VENDOR_NAME1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RT_NO1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REEL_MEMO1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.combLabelTypeFile = new System.Windows.Forms.ComboBox();
            this.combLabelType = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.grpbResult = new System.Windows.Forms.GroupBox();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.CHECKED = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.REEL_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PART_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REEL_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATECODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VENDOR_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VENDOR_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RT_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REEL_MEMO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.editPartNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.editVendorCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.editRTNo = new System.Windows.Forms.TextBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.editReelNo = new System.Windows.Forms.TextBox();
            this.btnFilterVendor = new System.Windows.Forms.Button();
            this.btnPartNo = new System.Windows.Forms.Button();
            this.btnReelNo = new System.Windows.Forms.Button();
            this.btnRTNo = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkbDate = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.editLot = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.editDatecode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editPrintQty)).BeginInit();
            this.gbChange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRT)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.grpbResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            resources.ApplyResources(this.removeToolStripMenuItem, "removeToolStripMenuItem");
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // editReel
            // 
            this.editReel.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editReel, "editReel");
            this.editReel.Name = "editReel";
            this.editReel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editReel_KeyPress);
            // 
            // editPrintQty
            // 
            this.editPrintQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.editPrintQty, "editPrintQty");
            this.editPrintQty.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.editPrintQty.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.editPrintQty.Name = "editPrintQty";
            this.editPrintQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Green;
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
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
            // gbChange
            // 
            this.gbChange.BackColor = System.Drawing.Color.Transparent;
            this.gbChange.Controls.Add(this.dgvRT);
            this.gbChange.Controls.Add(this.panel4);
            resources.ApplyResources(this.gbChange, "gbChange");
            this.gbChange.Name = "gbChange";
            this.gbChange.TabStop = false;
            // 
            // dgvRT
            // 
            this.dgvRT.AllowUserToAddRows = false;
            this.dgvRT.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgvRT.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRT.BackgroundColor = System.Drawing.Color.Lavender;
            resources.ApplyResources(this.dgvRT, "dgvRT");
            this.dgvRT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.REEL_NO1,
            this.PART_NO1,
            this.REEL_QTY1,
            this.DATECODE1,
            this.LOT1,
            this.VENDOR_CODE1,
            this.VENDOR_NAME1,
            this.RT_NO1,
            this.REEL_MEMO1});
            this.dgvRT.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvRT.MultiSelect = false;
            this.dgvRT.Name = "dgvRT";
            this.dgvRT.RowTemplate.Height = 24;
            this.dgvRT.TabStop = false;
            // 
            // REEL_NO1
            // 
            resources.ApplyResources(this.REEL_NO1, "REEL_NO1");
            this.REEL_NO1.Name = "REEL_NO1";
            this.REEL_NO1.ReadOnly = true;
            // 
            // PART_NO1
            // 
            resources.ApplyResources(this.PART_NO1, "PART_NO1");
            this.PART_NO1.Name = "PART_NO1";
            this.PART_NO1.ReadOnly = true;
            // 
            // REEL_QTY1
            // 
            resources.ApplyResources(this.REEL_QTY1, "REEL_QTY1");
            this.REEL_QTY1.Name = "REEL_QTY1";
            this.REEL_QTY1.ReadOnly = true;
            // 
            // DATECODE1
            // 
            resources.ApplyResources(this.DATECODE1, "DATECODE1");
            this.DATECODE1.Name = "DATECODE1";
            this.DATECODE1.ReadOnly = true;
            // 
            // LOT1
            // 
            resources.ApplyResources(this.LOT1, "LOT1");
            this.LOT1.Name = "LOT1";
            this.LOT1.ReadOnly = true;
            // 
            // VENDOR_CODE1
            // 
            resources.ApplyResources(this.VENDOR_CODE1, "VENDOR_CODE1");
            this.VENDOR_CODE1.Name = "VENDOR_CODE1";
            this.VENDOR_CODE1.ReadOnly = true;
            // 
            // VENDOR_NAME1
            // 
            resources.ApplyResources(this.VENDOR_NAME1, "VENDOR_NAME1");
            this.VENDOR_NAME1.Name = "VENDOR_NAME1";
            this.VENDOR_NAME1.ReadOnly = true;
            // 
            // RT_NO1
            // 
            resources.ApplyResources(this.RT_NO1, "RT_NO1");
            this.RT_NO1.Name = "RT_NO1";
            this.RT_NO1.ReadOnly = true;
            // 
            // REEL_MEMO1
            // 
            resources.ApplyResources(this.REEL_MEMO1, "REEL_MEMO1");
            this.REEL_MEMO1.Name = "REEL_MEMO1";
            this.REEL_MEMO1.ReadOnly = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.editReel);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.btnClear);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.combLabelTypeFile);
            this.panel2.Controls.Add(this.combLabelType);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.editPrintQty);
            this.panel2.Controls.Add(this.btnPrint);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // combLabelTypeFile
            // 
            this.combLabelTypeFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combLabelTypeFile, "combLabelTypeFile");
            this.combLabelTypeFile.FormattingEnabled = true;
            this.combLabelTypeFile.Name = "combLabelTypeFile";
            // 
            // combLabelType
            // 
            this.combLabelType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.combLabelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combLabelType, "combLabelType");
            this.combLabelType.FormattingEnabled = true;
            this.combLabelType.Name = "combLabelType";
            this.combLabelType.SelectedIndexChanged += new System.EventHandler(this.combLabelType_SelectedIndexChanged);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Name = "label16";
            // 
            // grpbResult
            // 
            this.grpbResult.Controls.Add(this.dgvData);
            this.grpbResult.Controls.Add(this.panel3);
            resources.ApplyResources(this.grpbResult, "grpbResult");
            this.grpbResult.Name = "grpbResult";
            this.grpbResult.TabStop = false;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvData.BackgroundColor = System.Drawing.Color.Lavender;
            resources.ApplyResources(this.dgvData, "dgvData");
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CHECKED,
            this.REEL_NO,
            this.PART_NO,
            this.REEL_QTY,
            this.DATECODE,
            this.LOT,
            this.VENDOR_CODE,
            this.VENDOR_NAME,
            this.RT_NO,
            this.REEL_MEMO});
            this.dgvData.ContextMenuStrip = this.contextMenuStrip2;
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.TabStop = false;
            // 
            // CHECKED
            // 
            this.CHECKED.FalseValue = "N";
            resources.ApplyResources(this.CHECKED, "CHECKED");
            this.CHECKED.Name = "CHECKED";
            this.CHECKED.TrueValue = "Y";
            // 
            // REEL_NO
            // 
            resources.ApplyResources(this.REEL_NO, "REEL_NO");
            this.REEL_NO.Name = "REEL_NO";
            this.REEL_NO.ReadOnly = true;
            // 
            // PART_NO
            // 
            resources.ApplyResources(this.PART_NO, "PART_NO");
            this.PART_NO.Name = "PART_NO";
            this.PART_NO.ReadOnly = true;
            // 
            // REEL_QTY
            // 
            resources.ApplyResources(this.REEL_QTY, "REEL_QTY");
            this.REEL_QTY.Name = "REEL_QTY";
            this.REEL_QTY.ReadOnly = true;
            // 
            // DATECODE
            // 
            resources.ApplyResources(this.DATECODE, "DATECODE");
            this.DATECODE.Name = "DATECODE";
            this.DATECODE.ReadOnly = true;
            // 
            // LOT
            // 
            resources.ApplyResources(this.LOT, "LOT");
            this.LOT.Name = "LOT";
            this.LOT.ReadOnly = true;
            // 
            // VENDOR_CODE
            // 
            resources.ApplyResources(this.VENDOR_CODE, "VENDOR_CODE");
            this.VENDOR_CODE.Name = "VENDOR_CODE";
            this.VENDOR_CODE.ReadOnly = true;
            // 
            // VENDOR_NAME
            // 
            resources.ApplyResources(this.VENDOR_NAME, "VENDOR_NAME");
            this.VENDOR_NAME.Name = "VENDOR_NAME";
            this.VENDOR_NAME.ReadOnly = true;
            // 
            // RT_NO
            // 
            resources.ApplyResources(this.RT_NO, "RT_NO");
            this.RT_NO.Name = "RT_NO";
            this.RT_NO.ReadOnly = true;
            // 
            // REEL_MEMO
            // 
            resources.ApplyResources(this.REEL_MEMO, "REEL_MEMO");
            this.REEL_MEMO.Name = "REEL_MEMO";
            this.REEL_MEMO.ReadOnly = true;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            resources.ApplyResources(this.contextMenuStrip2, "contextMenuStrip2");
            // 
            // modifyToolStripMenuItem
            // 
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            resources.ApplyResources(this.modifyToolStripMenuItem, "modifyToolStripMenuItem");
            this.modifyToolStripMenuItem.Click += new System.EventHandler(this.modifyToolStripMenuItem_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.btnSelectAll);
            this.panel3.Controls.Add(this.btnAdd);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.Tag = "1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectAll
            // 
            resources.ApplyResources(this.btnSelectAll, "btnSelectAll");
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Tag = "0";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnAdd
            // 
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // editPartNo
            // 
            this.editPartNo.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editPartNo, "editPartNo");
            this.editPartNo.Name = "editPartNo";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // editVendorCode
            // 
            this.editVendorCode.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editVendorCode, "editVendorCode");
            this.editVendorCode.Name = "editVendorCode";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // editRTNo
            // 
            this.editRTNo.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editRTNo, "editRTNo");
            this.editRTNo.Name = "editRTNo";
            // 
            // btnQuery
            // 
            resources.ApplyResources(this.btnQuery, "btnQuery");
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Name = "label6";
            // 
            // editReelNo
            // 
            this.editReelNo.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editReelNo, "editReelNo");
            this.editReelNo.Name = "editReelNo";
            // 
            // btnFilterVendor
            // 
            resources.ApplyResources(this.btnFilterVendor, "btnFilterVendor");
            this.btnFilterVendor.Name = "btnFilterVendor";
            this.btnFilterVendor.UseVisualStyleBackColor = true;
            this.btnFilterVendor.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnPartNo
            // 
            resources.ApplyResources(this.btnPartNo, "btnPartNo");
            this.btnPartNo.Name = "btnPartNo";
            this.btnPartNo.UseVisualStyleBackColor = true;
            this.btnPartNo.Click += new System.EventHandler(this.btnPartNo_Click);
            // 
            // btnReelNo
            // 
            resources.ApplyResources(this.btnReelNo, "btnReelNo");
            this.btnReelNo.Name = "btnReelNo";
            this.btnReelNo.UseVisualStyleBackColor = true;
            this.btnReelNo.Click += new System.EventHandler(this.btnReelNo_Click);
            // 
            // btnRTNo
            // 
            resources.ApplyResources(this.btnRTNo, "btnRTNo");
            this.btnRTNo.Name = "btnRTNo";
            this.btnRTNo.UseVisualStyleBackColor = true;
            this.btnRTNo.Click += new System.EventHandler(this.btnRTNo_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.chkbDate);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.dtEnd);
            this.panel1.Controls.Add(this.dtStart);
            this.panel1.Controls.Add(this.editLot);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.editDatecode);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnRTNo);
            this.panel1.Controls.Add(this.btnReelNo);
            this.panel1.Controls.Add(this.btnPartNo);
            this.panel1.Controls.Add(this.btnFilterVendor);
            this.panel1.Controls.Add(this.editReelNo);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.editRTNo);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.editVendorCode);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.editPartNo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Name = "panel1";
            // 
            // chkbDate
            // 
            resources.ApplyResources(this.chkbDate, "chkbDate");
            this.chkbDate.BackColor = System.Drawing.Color.Transparent;
            this.chkbDate.Checked = true;
            this.chkbDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbDate.Name = "chkbDate";
            this.chkbDate.UseVisualStyleBackColor = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Name = "label9";
            // 
            // dtEnd
            // 
            resources.ApplyResources(this.dtEnd, "dtEnd");
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtEnd.Name = "dtEnd";
            // 
            // dtStart
            // 
            resources.ApplyResources(this.dtStart, "dtStart");
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtStart.Name = "dtStart";
            // 
            // editLot
            // 
            this.editLot.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editLot, "editLot");
            this.editLot.Name = "editLot";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Name = "label8";
            // 
            // editDatecode
            // 
            this.editDatecode.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editDatecode, "editDatecode");
            this.editDatecode.Name = "editDatecode";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Name = "label7";
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.gbChange);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.grpbResult);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fMain_Load);
            this.Shown += new System.EventHandler(this.fMain_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editPrintQty)).EndInit();
            this.gbChange.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRT)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.grpbResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TextBox editReel;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.NumericUpDown editPrintQty;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbChange;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox grpbResult;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox editPartNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox editVendorCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox editRTNo;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox editReelNo;
        private System.Windows.Forms.Button btnFilterVendor;
        private System.Windows.Forms.Button btnPartNo;
        private System.Windows.Forms.Button btnReelNo;
        private System.Windows.Forms.Button btnRTNo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TextBox editLot;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox editDatecode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox combLabelTypeFile;
        private System.Windows.Forms.ComboBox combLabelType;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.CheckBox chkbDate;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.DataGridView dgvRT;
        private System.Windows.Forms.DataGridViewTextBoxColumn REEL_NO1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PART_NO1;
        private System.Windows.Forms.DataGridViewTextBoxColumn REEL_QTY1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATECODE1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOT1;
        private System.Windows.Forms.DataGridViewTextBoxColumn VENDOR_CODE1;
        private System.Windows.Forms.DataGridViewTextBoxColumn VENDOR_NAME1;
        private System.Windows.Forms.DataGridViewTextBoxColumn RT_NO1;
        private System.Windows.Forms.DataGridViewTextBoxColumn REEL_MEMO1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CHECKED;
        private System.Windows.Forms.DataGridViewTextBoxColumn REEL_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PART_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn REEL_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATECODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOT;
        private System.Windows.Forms.DataGridViewTextBoxColumn VENDOR_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn VENDOR_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn RT_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn REEL_MEMO;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
    }
}
