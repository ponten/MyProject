namespace IQCbyLot
{
    partial class fTooling
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_LotNo = new System.Windows.Forms.Label();
            this.lbl_PartNo = new System.Windows.Forms.Label();
            this.lblPartNo_show = new System.Windows.Forms.Label();
            this.lblLotNo_show = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearchLot = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.editToolingNo = new System.Windows.Forms.TextBox();
            this.lbl_ToolingNo = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvTooling = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.editUseCount = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.combToolingNo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.editToolingMemo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TOOLING_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOOLING_SN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAX_USED_COUNT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LIMIT_USED_COUNT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOOLING_USED_COUNT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USED_COUNT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CREATE_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EMP_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEMO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOOLING_SN_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTooling)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbl_LotNo);
            this.panel1.Controls.Add(this.lbl_PartNo);
            this.panel1.Controls.Add(this.lblPartNo_show);
            this.panel1.Controls.Add(this.lblLotNo_show);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(703, 41);
            this.panel1.TabIndex = 10;
            // 
            // lbl_LotNo
            // 
            this.lbl_LotNo.AutoSize = true;
            this.lbl_LotNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_LotNo.Location = new System.Drawing.Point(12, 13);
            this.lbl_LotNo.Name = "lbl_LotNo";
            this.lbl_LotNo.Size = new System.Drawing.Size(48, 15);
            this.lbl_LotNo.TabIndex = 13;
            this.lbl_LotNo.Text = "Lot No";
            // 
            // lbl_PartNo
            // 
            this.lbl_PartNo.AutoSize = true;
            this.lbl_PartNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_PartNo.Location = new System.Drawing.Point(302, 14);
            this.lbl_PartNo.Name = "lbl_PartNo";
            this.lbl_PartNo.Size = new System.Drawing.Size(51, 15);
            this.lbl_PartNo.TabIndex = 10;
            this.lbl_PartNo.Text = "Part No";
            // 
            // lblPartNo_show
            // 
            this.lblPartNo_show.AutoSize = true;
            this.lblPartNo_show.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPartNo_show.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblPartNo_show.ForeColor = System.Drawing.Color.Maroon;
            this.lblPartNo_show.Location = new System.Drawing.Point(380, 12);
            this.lblPartNo_show.Name = "lblPartNo_show";
            this.lblPartNo_show.Size = new System.Drawing.Size(2, 17);
            this.lblPartNo_show.TabIndex = 15;
            // 
            // lblLotNo_show
            // 
            this.lblLotNo_show.AutoSize = true;
            this.lblLotNo_show.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLotNo_show.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblLotNo_show.ForeColor = System.Drawing.Color.Maroon;
            this.lblLotNo_show.Location = new System.Drawing.Point(90, 12);
            this.lblLotNo_show.Name = "lblLotNo_show";
            this.lblLotNo_show.Size = new System.Drawing.Size(2, 17);
            this.lblLotNo_show.TabIndex = 14;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(616, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearchLot
            // 
            this.btnSearchLot.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.btnSearchLot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearchLot.Location = new System.Drawing.Point(262, 35);
            this.btnSearchLot.Name = "btnSearchLot";
            this.btnSearchLot.Size = new System.Drawing.Size(24, 25);
            this.btnSearchLot.TabIndex = 18;
            this.btnSearchLot.Text = "...";
            this.btnSearchLot.UseVisualStyleBackColor = true;
            this.btnSearchLot.Click += new System.EventHandler(this.btnSearchLot_Click);
            // 
            // btnDel
            // 
            this.btnDel.Enabled = false;
            this.btnDel.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnDel.Location = new System.Drawing.Point(615, 37);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 16;
            this.btnDel.Text = "Delete";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Visible = false;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // editToolingNo
            // 
            this.editToolingNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.editToolingNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editToolingNo.Location = new System.Drawing.Point(89, 35);
            this.editToolingNo.Name = "editToolingNo";
            this.editToolingNo.Size = new System.Drawing.Size(167, 25);
            this.editToolingNo.TabIndex = 12;
            this.editToolingNo.TextChanged += new System.EventHandler(this.editToolingNo_TextChanged);
            this.editToolingNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editToolingNo_KeyPress);
            // 
            // lbl_ToolingNo
            // 
            this.lbl_ToolingNo.AutoSize = true;
            this.lbl_ToolingNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_ToolingNo.Location = new System.Drawing.Point(11, 40);
            this.lbl_ToolingNo.Name = "lbl_ToolingNo";
            this.lbl_ToolingNo.Size = new System.Drawing.Size(74, 15);
            this.lbl_ToolingNo.TabIndex = 11;
            this.lbl_ToolingNo.Text = "Tooling SN";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvTooling);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 146);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(703, 257);
            this.panel2.TabIndex = 11;
            // 
            // dgvTooling
            // 
            this.dgvTooling.AllowUserToAddRows = false;
            this.dgvTooling.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.dgvTooling.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTooling.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTooling.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTooling.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTooling.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TOOLING_NO,
            this.TOOLING_SN,
            this.MAX_USED_COUNT,
            this.LIMIT_USED_COUNT,
            this.TOOLING_USED_COUNT,
            this.USED_COUNT,
            this.CREATE_TIME,
            this.EMP_NAME,
            this.MEMO,
            this.TOOLING_SN_ID});
            this.dgvTooling.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTooling.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTooling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTooling.EnableHeadersVisualStyles = false;
            this.dgvTooling.Location = new System.Drawing.Point(0, 0);
            this.dgvTooling.MultiSelect = false;
            this.dgvTooling.Name = "dgvTooling";
            this.dgvTooling.ReadOnly = true;
            this.dgvTooling.RowHeadersWidth = 25;
            this.dgvTooling.RowTemplate.Height = 24;
            this.dgvTooling.Size = new System.Drawing.Size(703, 257);
            this.dgvTooling.TabIndex = 9;
            this.dgvTooling.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTooling_CellContentClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(114, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(11, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "Use Count";
            // 
            // editUseCount
            // 
            this.editUseCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.editUseCount.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editUseCount.Location = new System.Drawing.Point(89, 65);
            this.editUseCount.Name = "editUseCount";
            this.editUseCount.Size = new System.Drawing.Size(167, 25);
            this.editUseCount.TabIndex = 21;
            this.editUseCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editUseCount_KeyPress);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 403);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(703, 39);
            this.panel3.TabIndex = 10;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.combToolingNo);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.btnAdd);
            this.panel5.Controls.Add(this.editToolingMemo);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.editToolingNo);
            this.panel5.Controls.Add(this.lbl_ToolingNo);
            this.panel5.Controls.Add(this.btnDel);
            this.panel5.Controls.Add(this.editUseCount);
            this.panel5.Controls.Add(this.btnSearchLot);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 41);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(703, 105);
            this.panel5.TabIndex = 23;
            // 
            // combToolingNo
            // 
            this.combToolingNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combToolingNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.combToolingNo.FormattingEnabled = true;
            this.combToolingNo.Location = new System.Drawing.Point(89, 6);
            this.combToolingNo.Name = "combToolingNo";
            this.combToolingNo.Size = new System.Drawing.Size(167, 23);
            this.combToolingNo.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(11, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 15);
            this.label3.TabIndex = 25;
            this.label3.Text = "Tooling No";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnAdd.Location = new System.Drawing.Point(614, 70);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 24;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // editToolingMemo
            // 
            this.editToolingMemo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editToolingMemo.Location = new System.Drawing.Point(351, 34);
            this.editToolingMemo.Multiline = true;
            this.editToolingMemo.Name = "editToolingMemo";
            this.editToolingMemo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.editToolingMemo.Size = new System.Drawing.Size(241, 58);
            this.editToolingMemo.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(301, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "Memo";
            // 
            // TOOLING_NO
            // 
            this.TOOLING_NO.HeaderText = "Tooling No";
            this.TOOLING_NO.Name = "TOOLING_NO";
            this.TOOLING_NO.ReadOnly = true;
            // 
            // TOOLING_SN
            // 
            this.TOOLING_SN.HeaderText = "Tooling SN";
            this.TOOLING_SN.Name = "TOOLING_SN";
            this.TOOLING_SN.ReadOnly = true;
            this.TOOLING_SN.Width = 120;
            // 
            // MAX_USED_COUNT
            // 
            this.MAX_USED_COUNT.HeaderText = "Max Used Count";
            this.MAX_USED_COUNT.Name = "MAX_USED_COUNT";
            this.MAX_USED_COUNT.ReadOnly = true;
            this.MAX_USED_COUNT.Width = 120;
            // 
            // LIMIT_USED_COUNT
            // 
            this.LIMIT_USED_COUNT.HeaderText = "Limit Used Count";
            this.LIMIT_USED_COUNT.Name = "LIMIT_USED_COUNT";
            this.LIMIT_USED_COUNT.ReadOnly = true;
            // 
            // TOOLING_USED_COUNT
            // 
            this.TOOLING_USED_COUNT.HeaderText = "Used Count";
            this.TOOLING_USED_COUNT.Name = "TOOLING_USED_COUNT";
            this.TOOLING_USED_COUNT.ReadOnly = true;
            // 
            // USED_COUNT
            // 
            this.USED_COUNT.HeaderText = "Use Count";
            this.USED_COUNT.Name = "USED_COUNT";
            this.USED_COUNT.ReadOnly = true;
            this.USED_COUNT.Width = 110;
            // 
            // CREATE_TIME
            // 
            this.CREATE_TIME.HeaderText = "Create Time";
            this.CREATE_TIME.Name = "CREATE_TIME";
            this.CREATE_TIME.ReadOnly = true;
            this.CREATE_TIME.Width = 160;
            // 
            // EMP_NAME
            // 
            this.EMP_NAME.HeaderText = "Emp Name";
            this.EMP_NAME.Name = "EMP_NAME";
            this.EMP_NAME.ReadOnly = true;
            this.EMP_NAME.Width = 90;
            // 
            // MEMO
            // 
            this.MEMO.HeaderText = "Memo";
            this.MEMO.Name = "MEMO";
            this.MEMO.ReadOnly = true;
            this.MEMO.Width = 200;
            // 
            // TOOLING_SN_ID
            // 
            this.TOOLING_SN_ID.HeaderText = "TOOLING_SN_ID";
            this.TOOLING_SN_ID.Name = "TOOLING_SN_ID";
            this.TOOLING_SN_ID.ReadOnly = true;
            this.TOOLING_SN_ID.Visible = false;
            // 
            // fTooling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 442);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.MaximizeBox = false;
            this.Name = "fTooling";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tooling";
            this.Load += new System.EventHandler(this.fTooling_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTooling)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDel;
        public System.Windows.Forms.Label lblPartNo_show;
        public System.Windows.Forms.Label lblLotNo_show;
        private System.Windows.Forms.Label lbl_LotNo;
        public System.Windows.Forms.TextBox editToolingNo;
        private System.Windows.Forms.Label lbl_ToolingNo;
        private System.Windows.Forms.Label lbl_PartNo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSearchLot;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvTooling;
        public System.Windows.Forms.TextBox editUseCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.TextBox editToolingMemo;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ComboBox combToolingNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOOLING_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOOLING_SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAX_USED_COUNT;
        private System.Windows.Forms.DataGridViewTextBoxColumn LIMIT_USED_COUNT;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOOLING_USED_COUNT;
        private System.Windows.Forms.DataGridViewTextBoxColumn USED_COUNT;
        private System.Windows.Forms.DataGridViewTextBoxColumn CREATE_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMP_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEMO;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOOLING_SN_ID;


    }
}