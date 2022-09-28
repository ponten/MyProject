namespace RCMerge
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
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.btnSearchWo = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.LabWO = new System.Windows.Forms.Label();
            this.editRC = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.LabSpec2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LabPart = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LabQty = new System.Windows.Forms.Label();
            this.LabVersion = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.LabProcess = new System.Windows.Forms.Label();
            this.LabRC = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gvDetail = new System.Windows.Forms.DataGridView();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.gvData = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            this.SuspendLayout();
            // 
            // splitter3
            // 
            this.splitter3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point(0, 161);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(819, 3);
            this.splitter3.TabIndex = 25;
            this.splitter3.TabStop = false;
            // 
            // btnSearchWo
            // 
            this.btnSearchWo.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchWo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearchWo.Location = new System.Drawing.Point(430, 10);
            this.btnSearchWo.Name = "btnSearchWo";
            this.btnSearchWo.Size = new System.Drawing.Size(37, 25);
            this.btnSearchWo.TabIndex = 11;
            this.btnSearchWo.Text = "...";
            this.btnSearchWo.UseVisualStyleBackColor = false;
            this.btnSearchWo.Click += new System.EventHandler(this.btnSearchWo_Click);
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
            // editRC
            // 
            this.editRC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.editRC.Font = new System.Drawing.Font("新細明體", 12F);
            this.editRC.Location = new System.Drawing.Point(120, 9);
            this.editRC.Name = "editRC";
            this.editRC.Size = new System.Drawing.Size(304, 27);
            this.editRC.TabIndex = 0;
            this.editRC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editRC_KeyPress);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label1);
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
            this.panel1.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(320, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 16);
            this.label1.TabIndex = 62;
            this.label1.Text = "Spec2";
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
            this.panel2.TabIndex = 19;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvDetail);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(584, 164);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(235, 334);
            this.panel3.TabIndex = 26;
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
            this.gvDetail.TabIndex = 1;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(581, 164);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 334);
            this.splitter2.TabIndex = 27;
            this.splitter2.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnSelectNone);
            this.panel4.Controls.Add(this.btnSelectAll);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 164);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(581, 38);
            this.panel4.TabIndex = 28;
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Enabled = false;
            this.btnSelectNone.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectNone.Location = new System.Drawing.Point(115, 6);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(93, 25);
            this.btnSelectNone.TabIndex = 70;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Enabled = false;
            this.btnSelectAll.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectAll.Location = new System.Drawing.Point(15, 6);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(94, 25);
            this.btnSelectAll.TabIndex = 69;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnExit);
            this.panel5.Controls.Add(this.btnMerge);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 461);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(581, 37);
            this.panel5.TabIndex = 29;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnExit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExit.Location = new System.Drawing.Point(115, 6);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.BackColor = System.Drawing.Color.Transparent;
            this.btnMerge.Enabled = false;
            this.btnMerge.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnMerge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMerge.Location = new System.Drawing.Point(15, 6);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(75, 25);
            this.btnMerge.TabIndex = 3;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = false;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.gvData);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 202);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(581, 259);
            this.panel6.TabIndex = 30;
            // 
            // gvData
            // 
            this.gvData.AllowUserToAddRows = false;
            this.gvData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Lavender;
            this.gvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvData.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvData.Location = new System.Drawing.Point(0, 0);
            this.gvData.MultiSelect = false;
            this.gvData.Name = "gvData";
            this.gvData.RowTemplate.Height = 24;
            this.gvData.Size = new System.Drawing.Size(581, 259);
            this.gvData.TabIndex = 68;
            this.gvData.SelectionChanged += new System.EventHandler(this.gvData_SelectionChanged);
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 498);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "fMain";
            this.Text = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Button btnSearchWo;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label LabWO;
        public  System.Windows.Forms.TextBox editRC;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LabPart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LabQty;
        private System.Windows.Forms.Label LabVersion;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label LabProcess;
        private System.Windows.Forms.Label LabRC;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView gvDetail;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.DataGridView gvData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabSpec2;
    }
}