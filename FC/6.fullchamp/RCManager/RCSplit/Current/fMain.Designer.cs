namespace RCSplit
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSearchWo = new System.Windows.Forms.Button();
            this.editRC = new System.Windows.Forms.TextBox();
            this.LabRC = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LabWO = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LabPart = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LabQty = new System.Windows.Forms.Label();
            this.LabVersion = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.LabProcess = new System.Windows.Forms.Label();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gvDetail = new System.Windows.Forms.DataGridView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnPick = new System.Windows.Forms.Button();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSplit = new System.Windows.Forms.Button();
            this.rbQtyEach = new System.Windows.Forms.RadioButton();
            this.rbPartQty = new System.Windows.Forms.RadioButton();
            this.editQty = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gvData = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.LabSpec2 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightBlue;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnSearchWo);
            this.panel2.Controls.Add(this.editRC);
            this.panel2.Controls.Add(this.LabRC);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(819, 50);
            this.panel2.TabIndex = 1;
            // 
            // btnSearchWo
            // 
            this.btnSearchWo.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchWo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearchWo.Location = new System.Drawing.Point(323, 8);
            this.btnSearchWo.Name = "btnSearchWo";
            this.btnSearchWo.Size = new System.Drawing.Size(37, 25);
            this.btnSearchWo.TabIndex = 11;
            this.btnSearchWo.Text = "...";
            this.btnSearchWo.UseVisualStyleBackColor = false;
            this.btnSearchWo.Click += new System.EventHandler(this.btnSearchWo_Click);
            // 
            // editRC
            // 
            this.editRC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.editRC.Font = new System.Drawing.Font("新細明體", 12F);
            this.editRC.Location = new System.Drawing.Point(120, 9);
            this.editRC.Name = "editRC";
            this.editRC.Size = new System.Drawing.Size(174, 27);
            this.editRC.TabIndex = 0;
            this.editRC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editRC_KeyPress);
            // 
            // LabRC
            // 
            this.LabRC.AutoSize = true;
            this.LabRC.BackColor = System.Drawing.Color.Transparent;
            this.LabRC.Font = new System.Drawing.Font("新細明體", 12F);
            this.LabRC.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabRC.Location = new System.Drawing.Point(10, 13);
            this.LabRC.Name = "LabRC";
            this.LabRC.Size = new System.Drawing.Size(71, 16);
            this.LabRC.TabIndex = 9;
            this.LabRC.Text = "Parent RC";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.LabSpec2);
            this.panel1.Controls.Add(this.LabWO);
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.LabPart);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.LabQty);
            this.panel1.Controls.Add(this.LabVersion);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.LabProcess);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(819, 111);
            this.panel1.TabIndex = 6;
            // 
            // LabWO
            // 
            this.LabWO.BackColor = System.Drawing.Color.Transparent;
            this.LabWO.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabWO.Font = new System.Drawing.Font("新細明體", 12F);
            this.LabWO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LabWO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabWO.Location = new System.Drawing.Point(120, 12);
            this.LabWO.Name = "LabWO";
            this.LabWO.Size = new System.Drawing.Size(174, 24);
            this.LabWO.TabIndex = 61;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("新細明體", 12F);
            this.label20.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label20.Location = new System.Drawing.Point(10, 13);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(83, 16);
            this.label20.TabIndex = 60;
            this.label20.Text = "Work Order";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(320, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 16);
            this.label3.TabIndex = 52;
            this.label3.Text = "Spec1";
            // 
            // LabPart
            // 
            this.LabPart.BackColor = System.Drawing.Color.Transparent;
            this.LabPart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabPart.Font = new System.Drawing.Font("新細明體", 12F);
            this.LabPart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LabPart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabPart.Location = new System.Drawing.Point(430, 12);
            this.LabPart.Name = "LabPart";
            this.LabPart.Size = new System.Drawing.Size(174, 24);
            this.LabPart.TabIndex = 53;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F);
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(10, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 16);
            this.label5.TabIndex = 54;
            this.label5.Text = "Current Qty";
            // 
            // LabQty
            // 
            this.LabQty.BackColor = System.Drawing.Color.Transparent;
            this.LabQty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabQty.Font = new System.Drawing.Font("新細明體", 12F);
            this.LabQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LabQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabQty.Location = new System.Drawing.Point(120, 42);
            this.LabQty.Name = "LabQty";
            this.LabQty.Size = new System.Drawing.Size(174, 24);
            this.LabQty.TabIndex = 55;
            // 
            // LabVersion
            // 
            this.LabVersion.BackColor = System.Drawing.Color.Transparent;
            this.LabVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabVersion.Font = new System.Drawing.Font("新細明體", 12F);
            this.LabVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LabVersion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabVersion.Location = new System.Drawing.Point(430, 72);
            this.LabVersion.Name = "LabVersion";
            this.LabVersion.Size = new System.Drawing.Size(174, 24);
            this.LabVersion.TabIndex = 59;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("新細明體", 12F);
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(320, 73);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 16);
            this.label14.TabIndex = 58;
            this.label14.Text = "Version";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("新細明體", 12F);
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(10, 73);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 16);
            this.label13.TabIndex = 57;
            this.label13.Text = "Process Name";
            // 
            // LabProcess
            // 
            this.LabProcess.BackColor = System.Drawing.Color.Transparent;
            this.LabProcess.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabProcess.Font = new System.Drawing.Font("新細明體", 12F);
            this.LabProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LabProcess.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabProcess.Location = new System.Drawing.Point(120, 72);
            this.LabProcess.Name = "LabProcess";
            this.LabProcess.Size = new System.Drawing.Size(174, 24);
            this.LabProcess.TabIndex = 56;
            // 
            // splitter2
            // 
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 161);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(819, 3);
            this.splitter2.TabIndex = 7;
            this.splitter2.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvDetail);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(584, 164);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(235, 334);
            this.panel3.TabIndex = 9;
            // 
            // gvDetail
            // 
            this.gvDetail.AllowUserToAddRows = false;
            this.gvDetail.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.gvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gvDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvDetail.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.gvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvDetail.Location = new System.Drawing.Point(0, 0);
            this.gvDetail.MultiSelect = false;
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.ReadOnly = true;
            this.gvDetail.RowTemplate.Height = 24;
            this.gvDetail.Size = new System.Drawing.Size(235, 334);
            this.gvDetail.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(581, 164);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 334);
            this.splitter1.TabIndex = 10;
            this.splitter1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnPick);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 230);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(581, 40);
            this.panel5.TabIndex = 15;
            // 
            // btnPick
            // 
            this.btnPick.Enabled = false;
            this.btnPick.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPick.Location = new System.Drawing.Point(15, 11);
            this.btnPick.Name = "btnPick";
            this.btnPick.Size = new System.Drawing.Size(135, 25);
            this.btnPick.TabIndex = 68;
            this.btnPick.Text = "Pick Component";
            this.btnPick.UseVisualStyleBackColor = true;
            this.btnPick.Visible = false;
            this.btnPick.Click += new System.EventHandler(this.btnPick_Click);
            // 
            // splitter3
            // 
            this.splitter3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point(0, 227);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(581, 3);
            this.splitter3.TabIndex = 14;
            this.splitter3.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.btnSplit);
            this.panel4.Controls.Add(this.rbQtyEach);
            this.panel4.Controls.Add(this.rbPartQty);
            this.panel4.Controls.Add(this.editQty);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 164);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(581, 63);
            this.panel4.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(15, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 66;
            this.label1.Text = "Split Filter";
            // 
            // btnSplit
            // 
            this.btnSplit.Enabled = false;
            this.btnSplit.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSplit.Location = new System.Drawing.Point(531, 18);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(75, 25);
            this.btnSplit.TabIndex = 62;
            this.btnSplit.Text = "Split";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Visible = false;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // rbQtyEach
            // 
            this.rbQtyEach.AutoSize = true;
            this.rbQtyEach.Checked = true;
            this.rbQtyEach.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbQtyEach.Location = new System.Drawing.Point(325, 19);
            this.rbQtyEach.Name = "rbQtyEach";
            this.rbQtyEach.Size = new System.Drawing.Size(129, 20);
            this.rbQtyEach.TabIndex = 65;
            this.rbQtyEach.TabStop = true;
            this.rbQtyEach.Text = "Qty of Each Part";
            this.rbQtyEach.UseVisualStyleBackColor = true;
            // 
            // rbPartQty
            // 
            this.rbPartQty.AutoSize = true;
            this.rbPartQty.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbPartQty.Location = new System.Drawing.Point(325, 43);
            this.rbPartQty.Name = "rbPartQty";
            this.rbPartQty.Size = new System.Drawing.Size(77, 20);
            this.rbPartQty.TabIndex = 64;
            this.rbPartQty.Text = "Part Qty";
            this.rbPartQty.UseVisualStyleBackColor = true;
            this.rbPartQty.Visible = false;
            // 
            // editQty
            // 
            this.editQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.editQty.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editQty.Location = new System.Drawing.Point(122, 19);
            this.editQty.Name = "editQty";
            this.editQty.Size = new System.Drawing.Size(174, 27);
            this.editQty.TabIndex = 63;
            this.editQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editQty_KeyPress);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnExit);
            this.panel6.Controls.Add(this.btnSave);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 461);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(581, 37);
            this.panel6.TabIndex = 19;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnExit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExit.Location = new System.Drawing.Point(115, 6);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Enabled = false;
            this.btnSave.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(15, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Execute";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gvData);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 270);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(581, 191);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Child RC Information";
            // 
            // gvData
            // 
            this.gvData.AllowUserToAddRows = false;
            this.gvData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Lavender;
            this.gvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gvData.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvData.Location = new System.Drawing.Point(3, 18);
            this.gvData.MultiSelect = false;
            this.gvData.Name = "gvData";
            this.gvData.RowTemplate.Height = 24;
            this.gvData.Size = new System.Drawing.Size(575, 170);
            this.gvData.TabIndex = 67;
            this.gvData.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gvData_CellValidating);
            this.gvData.SelectionChanged += new System.EventHandler(this.gvData_SelectionChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(320, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 16);
            this.label2.TabIndex = 62;
            this.label2.Text = "Spec2";
            // 
            // LabSpec2
            // 
            this.LabSpec2.BackColor = System.Drawing.Color.Transparent;
            this.LabSpec2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabSpec2.Font = new System.Drawing.Font("新細明體", 12F);
            this.LabSpec2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LabSpec2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabSpec2.Location = new System.Drawing.Point(430, 42);
            this.LabSpec2.Name = "LabSpec2";
            this.LabSpec2.Size = new System.Drawing.Size(174, 24);
            this.LabSpec2.TabIndex = 63;
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 498);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "fMain";
            this.Text = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSearchWo;
        public  System.Windows.Forms.TextBox editRC;
        private System.Windows.Forms.Label LabRC;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LabWO;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LabPart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LabQty;
        private System.Windows.Forms.Label LabVersion;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label LabProcess;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView gvDetail;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnPick;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.RadioButton rbQtyEach;
        private System.Windows.Forms.RadioButton rbPartQty;
        private System.Windows.Forms.TextBox editQty;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView gvData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LabSpec2;
    }
}