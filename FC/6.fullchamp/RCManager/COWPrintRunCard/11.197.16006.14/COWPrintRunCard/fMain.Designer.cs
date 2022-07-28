namespace COWPrintRunCard
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.Checked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RC_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sheet_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CURRENT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RCStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WOStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.NonePrinttoolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.RCNonePrinttoolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.WONoneprinttoolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblWOEndTime = new System.Windows.Forms.Label();
            this.lblWOStartTime = new System.Windows.Forms.Label();
            this.lblSpec2 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblSpecialPN = new System.Windows.Forms.Label();
            this.txtRC = new System.Windows.Forms.TextBox();
            this.lblProductGrade = new System.Windows.Forms.Label();
            this.lblRunCardTemplate = new System.Windows.Forms.Label();
            this.lblPartNo = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblWOType = new System.Windows.Forms.Label();
            this.lblModel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sampleBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.blue1radioButton = new System.Windows.Forms.RadioButton();
            this.red1radioButton = new System.Windows.Forms.RadioButton();
            this.greenradioButton = new System.Windows.Forms.RadioButton();
            this.pinkradioButton = new System.Windows.Forms.RadioButton();
            this.yellowradioButton = new System.Windows.Forms.RadioButton();
            this.noneradioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.blueradioButton = new System.Windows.Forms.RadioButton();
            this.blackradioButton = new System.Windows.Forms.RadioButton();
            this.redradioButton = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpDispatchDate = new System.Windows.Forms.DateTimePicker();
            this.dtpInvDate = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtWO = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrintRC = new System.Windows.Forms.Button();
            this.btnPrintWO = new System.Windows.Forms.Button();
            this.lblOption12 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 225);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1014, 245);
            this.panel2.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgv);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1014, 245);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "SelectQty:0";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Checked,
            this.RC_NO,
            this.Sheet_Name,
            this.CURRENT_QTY,
            this.RCStatus,
            this.WOStatus});
            this.dgv.ContextMenuStrip = this.contextMenuStrip1;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(3, 18);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 23;
            this.dgv.Size = new System.Drawing.Size(1008, 224);
            this.dgv.TabIndex = 0;
            this.dgv.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgv_CurrentCellDirtyStateChanged);
            // 
            // Checked
            // 
            this.Checked.HeaderText = "Checked";
            this.Checked.Name = "Checked";
            this.Checked.Width = 52;
            // 
            // RC_NO
            // 
            this.RC_NO.HeaderText = "RC No";
            this.RC_NO.Name = "RC_NO";
            this.RC_NO.ReadOnly = true;
            this.RC_NO.Width = 63;
            // 
            // Sheet_Name
            // 
            this.Sheet_Name.HeaderText = "Sheet Name";
            this.Sheet_Name.Name = "Sheet_Name";
            this.Sheet_Name.ReadOnly = true;
            this.Sheet_Name.Width = 85;
            // 
            // CURRENT_QTY
            // 
            this.CURRENT_QTY.HeaderText = "Current Qty";
            this.CURRENT_QTY.Name = "CURRENT_QTY";
            this.CURRENT_QTY.ReadOnly = true;
            this.CURRENT_QTY.Width = 86;
            // 
            // RCStatus
            // 
            this.RCStatus.HeaderText = "RC_Status";
            this.RCStatus.Name = "RCStatus";
            this.RCStatus.ReadOnly = true;
            this.RCStatus.Width = 79;
            // 
            // WOStatus
            // 
            this.WOStatus.HeaderText = "WO_Status";
            this.WOStatus.Name = "WOStatus";
            this.WOStatus.ReadOnly = true;
            this.WOStatus.Width = 82;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem1,
            this.NonePrinttoolStripMenuItem1,
            this.RCNonePrinttoolStripMenuItem1,
            this.WONoneprinttoolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(157, 92);
            // 
            // selectAllToolStripMenuItem1
            // 
            this.selectAllToolStripMenuItem1.Name = "selectAllToolStripMenuItem1";
            this.selectAllToolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.selectAllToolStripMenuItem1.Text = "SelectAll";
            this.selectAllToolStripMenuItem1.Click += new System.EventHandler(this.selectAllToolStripMenuItem1_Click);
            // 
            // NonePrinttoolStripMenuItem1
            // 
            this.NonePrinttoolStripMenuItem1.Name = "NonePrinttoolStripMenuItem1";
            this.NonePrinttoolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.NonePrinttoolStripMenuItem1.Text = "Noneprint";
            this.NonePrinttoolStripMenuItem1.Click += new System.EventHandler(this.NonePrinttoolStripMenuItem1_Click);
            // 
            // RCNonePrinttoolStripMenuItem1
            // 
            this.RCNonePrinttoolStripMenuItem1.Name = "RCNonePrinttoolStripMenuItem1";
            this.RCNonePrinttoolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.RCNonePrinttoolStripMenuItem1.Text = "RCNonePrint";
            this.RCNonePrinttoolStripMenuItem1.Click += new System.EventHandler(this.RCNonePrinttoolStripMenuItem1_Click);
            // 
            // WONoneprinttoolStripMenuItem1
            // 
            this.WONoneprinttoolStripMenuItem1.Name = "WONoneprinttoolStripMenuItem1";
            this.WONoneprinttoolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.WONoneprinttoolStripMenuItem1.Text = "WONoneprint";
            this.WONoneprinttoolStripMenuItem1.Click += new System.EventHandler(this.WONoneprinttoolStripMenuItem1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1014, 225);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblOption12);
            this.splitContainer1.Panel1.Controls.Add(this.lblWOEndTime);
            this.splitContainer1.Panel1.Controls.Add(this.lblWOStartTime);
            this.splitContainer1.Panel1.Controls.Add(this.lblSpec2);
            this.splitContainer1.Panel1.Controls.Add(this.label14);
            this.splitContainer1.Panel1.Controls.Add(this.lblSpecialPN);
            this.splitContainer1.Panel1.Controls.Add(this.txtRC);
            this.splitContainer1.Panel1.Controls.Add(this.lblProductGrade);
            this.splitContainer1.Panel1.Controls.Add(this.lblRunCardTemplate);
            this.splitContainer1.Panel1.Controls.Add(this.lblPartNo);
            this.splitContainer1.Panel1.Controls.Add(this.label12);
            this.splitContainer1.Panel1.Controls.Add(this.lblWOType);
            this.splitContainer1.Panel1.Controls.Add(this.lblModel);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.lblQty);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.txtWO);
            this.splitContainer1.Panel1.Controls.Add(this.btnSelect);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.btnPrintRC);
            this.splitContainer1.Panel1.Controls.Add(this.btnPrintWO);
            this.splitContainer1.Size = new System.Drawing.Size(1014, 225);
            this.splitContainer1.SplitterDistance = 740;
            this.splitContainer1.TabIndex = 21;
            // 
            // lblWOEndTime
            // 
            this.lblWOEndTime.AutoSize = true;
            this.lblWOEndTime.ForeColor = System.Drawing.Color.DarkRed;
            this.lblWOEndTime.Location = new System.Drawing.Point(160, 199);
            this.lblWOEndTime.Name = "lblWOEndTime";
            this.lblWOEndTime.Size = new System.Drawing.Size(24, 12);
            this.lblWOEndTime.TabIndex = 55;
            this.lblWOEndTime.Text = "N/A";
            this.lblWOEndTime.Visible = false;
            // 
            // lblWOStartTime
            // 
            this.lblWOStartTime.AutoSize = true;
            this.lblWOStartTime.ForeColor = System.Drawing.Color.DarkRed;
            this.lblWOStartTime.Location = new System.Drawing.Point(160, 169);
            this.lblWOStartTime.Name = "lblWOStartTime";
            this.lblWOStartTime.Size = new System.Drawing.Size(24, 12);
            this.lblWOStartTime.TabIndex = 54;
            this.lblWOStartTime.Text = "N/A";
            this.lblWOStartTime.Visible = false;
            // 
            // lblSpec2
            // 
            this.lblSpec2.AutoSize = true;
            this.lblSpec2.ForeColor = System.Drawing.Color.DarkRed;
            this.lblSpec2.Location = new System.Drawing.Point(360, 79);
            this.lblSpec2.Name = "lblSpec2";
            this.lblSpec2.Size = new System.Drawing.Size(24, 12);
            this.lblSpec2.TabIndex = 53;
            this.lblSpec2.Text = "N/A";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(240, 80);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 12);
            this.label14.TabIndex = 52;
            this.label14.Text = "Spec2";
            // 
            // lblSpecialPN
            // 
            this.lblSpecialPN.AutoSize = true;
            this.lblSpecialPN.ForeColor = System.Drawing.Color.DarkRed;
            this.lblSpecialPN.Location = new System.Drawing.Point(360, 229);
            this.lblSpecialPN.Name = "lblSpecialPN";
            this.lblSpecialPN.Size = new System.Drawing.Size(24, 12);
            this.lblSpecialPN.TabIndex = 51;
            this.lblSpecialPN.Text = "N/A";
            this.lblSpecialPN.Visible = false;
            // 
            // txtRC
            // 
            this.txtRC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtRC.Location = new System.Drawing.Point(60, 45);
            this.txtRC.Name = "txtRC";
            this.txtRC.Size = new System.Drawing.Size(170, 22);
            this.txtRC.TabIndex = 23;
            this.txtRC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRC_KeyPress);
            // 
            // lblProductGrade
            // 
            this.lblProductGrade.AutoSize = true;
            this.lblProductGrade.ForeColor = System.Drawing.Color.DarkRed;
            this.lblProductGrade.Location = new System.Drawing.Point(360, 169);
            this.lblProductGrade.Name = "lblProductGrade";
            this.lblProductGrade.Size = new System.Drawing.Size(24, 12);
            this.lblProductGrade.TabIndex = 41;
            this.lblProductGrade.Text = "N/A";
            // 
            // lblRunCardTemplate
            // 
            this.lblRunCardTemplate.AutoSize = true;
            this.lblRunCardTemplate.ForeColor = System.Drawing.Color.DarkRed;
            this.lblRunCardTemplate.Location = new System.Drawing.Point(360, 199);
            this.lblRunCardTemplate.Name = "lblRunCardTemplate";
            this.lblRunCardTemplate.Size = new System.Drawing.Size(24, 12);
            this.lblRunCardTemplate.TabIndex = 39;
            this.lblRunCardTemplate.Text = "N/A";
            // 
            // lblPartNo
            // 
            this.lblPartNo.AutoSize = true;
            this.lblPartNo.ForeColor = System.Drawing.Color.DarkRed;
            this.lblPartNo.Location = new System.Drawing.Point(360, 49);
            this.lblPartNo.Name = "lblPartNo";
            this.lblPartNo.Size = new System.Drawing.Size(24, 12);
            this.lblPartNo.TabIndex = 45;
            this.lblPartNo.Text = "N/A";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(240, 230);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 12);
            this.label12.TabIndex = 50;
            this.label12.Text = "SpecialPN";
            this.label12.Visible = false;
            // 
            // lblWOType
            // 
            this.lblWOType.AutoSize = true;
            this.lblWOType.ForeColor = System.Drawing.Color.DarkRed;
            this.lblWOType.Location = new System.Drawing.Point(360, 19);
            this.lblWOType.Name = "lblWOType";
            this.lblWOType.Size = new System.Drawing.Size(24, 12);
            this.lblWOType.TabIndex = 43;
            this.lblWOType.Text = "N/A";
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.ForeColor = System.Drawing.Color.DarkRed;
            this.lblModel.Location = new System.Drawing.Point(360, 109);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(24, 12);
            this.lblModel.TabIndex = 47;
            this.lblModel.Text = "N/A";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 12);
            this.label11.TabIndex = 22;
            this.label11.Text = "RC";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(240, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 12);
            this.label5.TabIndex = 44;
            this.label5.Text = "Spec1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(240, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 12);
            this.label4.TabIndex = 42;
            this.label4.Text = "WO Type";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(240, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 46;
            this.label6.Text = "Version";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(240, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 12);
            this.label3.TabIndex = 40;
            this.label3.Text = "Product Grade";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(240, 140);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 12);
            this.label7.TabIndex = 48;
            this.label7.Text = "Qty";
            // 
            // lblQty
            // 
            this.lblQty.AutoSize = true;
            this.lblQty.ForeColor = System.Drawing.Color.DarkRed;
            this.lblQty.Location = new System.Drawing.Point(360, 139);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(24, 12);
            this.lblQty.TabIndex = 49;
            this.lblQty.Text = "N/A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 12);
            this.label2.TabIndex = 38;
            this.label2.Text = "RC Template";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sampleBtn);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.dtpDispatchDate);
            this.groupBox1.Controls.Add(this.dtpInvDate);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtTitle);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(450, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 225);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PrintDocSetup";
            this.groupBox1.Visible = false;
            // 
            // sampleBtn
            // 
            this.sampleBtn.Font = new System.Drawing.Font("新細明體", 9F);
            this.sampleBtn.Location = new System.Drawing.Point(188, 150);
            this.sampleBtn.Name = "sampleBtn";
            this.sampleBtn.Size = new System.Drawing.Size(82, 30);
            this.sampleBtn.TabIndex = 29;
            this.sampleBtn.Text = "Sample";
            this.sampleBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.blue1radioButton);
            this.groupBox3.Controls.Add(this.red1radioButton);
            this.groupBox3.Controls.Add(this.greenradioButton);
            this.groupBox3.Controls.Add(this.pinkradioButton);
            this.groupBox3.Controls.Add(this.yellowradioButton);
            this.groupBox3.Controls.Add(this.noneradioButton);
            this.groupBox3.Location = new System.Drawing.Point(120, 50);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(150, 90);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Interiorcolor";
            // 
            // blue1radioButton
            // 
            this.blue1radioButton.AutoSize = true;
            this.blue1radioButton.Location = new System.Drawing.Point(85, 65);
            this.blue1radioButton.Name = "blue1radioButton";
            this.blue1radioButton.Size = new System.Drawing.Size(43, 16);
            this.blue1radioButton.TabIndex = 31;
            this.blue1radioButton.TabStop = true;
            this.blue1radioButton.Text = "blue";
            this.blue1radioButton.UseVisualStyleBackColor = true;
            this.blue1radioButton.CheckedChanged += new System.EventHandler(this.blue1radioButton_CheckedChanged);
            // 
            // red1radioButton
            // 
            this.red1radioButton.AutoSize = true;
            this.red1radioButton.Location = new System.Drawing.Point(85, 40);
            this.red1radioButton.Name = "red1radioButton";
            this.red1radioButton.Size = new System.Drawing.Size(53, 16);
            this.red1radioButton.TabIndex = 30;
            this.red1radioButton.TabStop = true;
            this.red1radioButton.Text = "purple";
            this.red1radioButton.UseVisualStyleBackColor = true;
            this.red1radioButton.CheckedChanged += new System.EventHandler(this.red1radioButton_CheckedChanged);
            // 
            // greenradioButton
            // 
            this.greenradioButton.AutoSize = true;
            this.greenradioButton.Location = new System.Drawing.Point(85, 15);
            this.greenradioButton.Name = "greenradioButton";
            this.greenradioButton.Size = new System.Drawing.Size(49, 16);
            this.greenradioButton.TabIndex = 29;
            this.greenradioButton.TabStop = true;
            this.greenradioButton.Text = "green";
            this.greenradioButton.UseVisualStyleBackColor = true;
            this.greenradioButton.CheckedChanged += new System.EventHandler(this.greenradioButton_CheckedChanged);
            // 
            // pinkradioButton
            // 
            this.pinkradioButton.AutoSize = true;
            this.pinkradioButton.Location = new System.Drawing.Point(15, 65);
            this.pinkradioButton.Name = "pinkradioButton";
            this.pinkradioButton.Size = new System.Drawing.Size(55, 16);
            this.pinkradioButton.TabIndex = 28;
            this.pinkradioButton.TabStop = true;
            this.pinkradioButton.Text = "orange";
            this.pinkradioButton.UseVisualStyleBackColor = true;
            this.pinkradioButton.CheckedChanged += new System.EventHandler(this.pinkradioButton_CheckedChanged);
            // 
            // yellowradioButton
            // 
            this.yellowradioButton.AutoSize = true;
            this.yellowradioButton.Location = new System.Drawing.Point(15, 40);
            this.yellowradioButton.Name = "yellowradioButton";
            this.yellowradioButton.Size = new System.Drawing.Size(54, 16);
            this.yellowradioButton.TabIndex = 27;
            this.yellowradioButton.TabStop = true;
            this.yellowradioButton.Text = "yellow";
            this.yellowradioButton.UseVisualStyleBackColor = true;
            this.yellowradioButton.CheckedChanged += new System.EventHandler(this.yellowradioButton_CheckedChanged);
            // 
            // noneradioButton
            // 
            this.noneradioButton.AutoSize = true;
            this.noneradioButton.Checked = true;
            this.noneradioButton.Location = new System.Drawing.Point(15, 15);
            this.noneradioButton.Name = "noneradioButton";
            this.noneradioButton.Size = new System.Drawing.Size(46, 16);
            this.noneradioButton.TabIndex = 26;
            this.noneradioButton.TabStop = true;
            this.noneradioButton.Text = "none";
            this.noneradioButton.UseVisualStyleBackColor = true;
            this.noneradioButton.CheckedChanged += new System.EventHandler(this.noneradioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.blueradioButton);
            this.groupBox2.Controls.Add(this.blackradioButton);
            this.groupBox2.Controls.Add(this.redradioButton);
            this.groupBox2.Location = new System.Drawing.Point(20, 50);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(90, 90);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "fontcolor";
            // 
            // blueradioButton
            // 
            this.blueradioButton.AutoSize = true;
            this.blueradioButton.Location = new System.Drawing.Point(15, 65);
            this.blueradioButton.Name = "blueradioButton";
            this.blueradioButton.Size = new System.Drawing.Size(43, 16);
            this.blueradioButton.TabIndex = 27;
            this.blueradioButton.TabStop = true;
            this.blueradioButton.Text = "blue";
            this.blueradioButton.UseVisualStyleBackColor = true;
            this.blueradioButton.CheckedChanged += new System.EventHandler(this.blueradioButton_CheckedChanged);
            // 
            // blackradioButton
            // 
            this.blackradioButton.AutoSize = true;
            this.blackradioButton.Checked = true;
            this.blackradioButton.Location = new System.Drawing.Point(15, 15);
            this.blackradioButton.Name = "blackradioButton";
            this.blackradioButton.Size = new System.Drawing.Size(48, 16);
            this.blackradioButton.TabIndex = 25;
            this.blackradioButton.TabStop = true;
            this.blackradioButton.Text = "black";
            this.blackradioButton.UseVisualStyleBackColor = true;
            this.blackradioButton.CheckedChanged += new System.EventHandler(this.blackradioButton_CheckedChanged);
            // 
            // redradioButton
            // 
            this.redradioButton.AutoSize = true;
            this.redradioButton.Location = new System.Drawing.Point(15, 40);
            this.redradioButton.Name = "redradioButton";
            this.redradioButton.Size = new System.Drawing.Size(38, 16);
            this.redradioButton.TabIndex = 26;
            this.redradioButton.TabStop = true;
            this.redradioButton.Text = "red";
            this.redradioButton.UseVisualStyleBackColor = true;
            this.redradioButton.CheckedChanged += new System.EventHandler(this.redradioButton_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 170);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "Dispatch Date";
            this.label8.Visible = false;
            // 
            // dtpDispatchDate
            // 
            this.dtpDispatchDate.CustomFormat = "yyyy/MM/dd";
            this.dtpDispatchDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDispatchDate.Location = new System.Drawing.Point(120, 165);
            this.dtpDispatchDate.Name = "dtpDispatchDate";
            this.dtpDispatchDate.Size = new System.Drawing.Size(119, 22);
            this.dtpDispatchDate.TabIndex = 18;
            this.dtpDispatchDate.Visible = false;
            // 
            // dtpInvDate
            // 
            this.dtpInvDate.CustomFormat = "yyyy/MM/dd";
            this.dtpInvDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpInvDate.Location = new System.Drawing.Point(120, 195);
            this.dtpInvDate.Name = "dtpInvDate";
            this.dtpInvDate.Size = new System.Drawing.Size(119, 22);
            this.dtpInvDate.TabIndex = 18;
            this.dtpInvDate.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 200);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "Schedule Date";
            this.label9.Visible = false;
            // 
            // txtTitle
            // 
            this.txtTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtTitle.Location = new System.Drawing.Point(70, 15);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(170, 22);
            this.txtTitle.TabIndex = 20;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 12);
            this.label10.TabIndex = 19;
            this.label10.Text = "Title";
            // 
            // txtWO
            // 
            this.txtWO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtWO.Location = new System.Drawing.Point(60, 15);
            this.txtWO.Name = "txtWO";
            this.txtWO.Size = new System.Drawing.Size(170, 22);
            this.txtWO.TabIndex = 1;
            this.txtWO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWO_KeyPress);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(243, 14);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(34, 25);
            this.btnSelect.TabIndex = 10;
            this.btnSelect.Text = "...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Visible = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "WO";
            // 
            // btnPrintRC
            // 
            this.btnPrintRC.Location = new System.Drawing.Point(148, 104);
            this.btnPrintRC.Name = "btnPrintRC";
            this.btnPrintRC.Size = new System.Drawing.Size(82, 30);
            this.btnPrintRC.TabIndex = 11;
            this.btnPrintRC.Text = "Print RC";
            this.btnPrintRC.UseVisualStyleBackColor = true;
            this.btnPrintRC.Click += new System.EventHandler(this.btnPrintRC_Click);
            // 
            // btnPrintWO
            // 
            this.btnPrintWO.Location = new System.Drawing.Point(60, 104);
            this.btnPrintWO.Name = "btnPrintWO";
            this.btnPrintWO.Size = new System.Drawing.Size(82, 30);
            this.btnPrintWO.TabIndex = 11;
            this.btnPrintWO.Text = "Print WO";
            this.btnPrintWO.UseVisualStyleBackColor = true;
            this.btnPrintWO.Visible = false;
            this.btnPrintWO.Click += new System.EventHandler(this.btnPrintWO_Click);
            // 
            // lblOption12
            // 
            this.lblOption12.AutoSize = true;
            this.lblOption12.ForeColor = System.Drawing.Color.DarkRed;
            this.lblOption12.Location = new System.Drawing.Point(160, 139);
            this.lblOption12.Name = "lblOption12";
            this.lblOption12.Size = new System.Drawing.Size(24, 12);
            this.lblOption12.TabIndex = 56;
            this.lblOption12.Text = "N/A";
            this.lblOption12.Visible = false;
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 470);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "COW Print RunCard";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.panel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Checked;
        private System.Windows.Forms.DataGridViewTextBoxColumn RC_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sheet_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn CURRENT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn RCStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn WOStatus;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem NonePrinttoolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem RCNonePrinttoolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem WONoneprinttoolStripMenuItem1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblSpecialPN;
        private System.Windows.Forms.TextBox txtRC;
        private System.Windows.Forms.Label lblProductGrade;
        private System.Windows.Forms.Label lblRunCardTemplate;
        private System.Windows.Forms.Label lblPartNo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblWOType;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button sampleBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton blue1radioButton;
        private System.Windows.Forms.RadioButton red1radioButton;
        private System.Windows.Forms.RadioButton greenradioButton;
        private System.Windows.Forms.RadioButton pinkradioButton;
        private System.Windows.Forms.RadioButton yellowradioButton;
        private System.Windows.Forms.RadioButton noneradioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton blueradioButton;
        private System.Windows.Forms.RadioButton blackradioButton;
        private System.Windows.Forms.RadioButton redradioButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpDispatchDate;
        private System.Windows.Forms.DateTimePicker dtpInvDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtWO;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPrintRC;
        private System.Windows.Forms.Button btnPrintWO;
        private System.Windows.Forms.Label lblSpec2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblWOEndTime;
        private System.Windows.Forms.Label lblWOStartTime;
        private System.Windows.Forms.Label lblOption12;
    }
}