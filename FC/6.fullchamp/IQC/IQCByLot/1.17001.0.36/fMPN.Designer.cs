namespace IQCbyLot
{
    partial class fMPN
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblLotNo_show = new System.Windows.Forms.Label();
            this.lbl_LotNo = new System.Windows.Forms.Label();
            this.editMPNNo = new System.Windows.Forms.TextBox();
            this.lblVN_show = new System.Windows.Forms.Label();
            this.lblPartNo_show = new System.Windows.Forms.Label();
            this.lbl_MPN = new System.Windows.Forms.Label();
            this.lbl_Vendor_Name = new System.Windows.Forms.Label();
            this.lbl_PartNo = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvMPN = new System.Windows.Forms.DataGridView();
            this.VENDOR_PART_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CREATE_USER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CREATE_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMPN)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblLotNo_show);
            this.panel1.Controls.Add(this.lbl_LotNo);
            this.panel1.Controls.Add(this.editMPNNo);
            this.panel1.Controls.Add(this.lblVN_show);
            this.panel1.Controls.Add(this.lblPartNo_show);
            this.panel1.Controls.Add(this.lbl_MPN);
            this.panel1.Controls.Add(this.lbl_Vendor_Name);
            this.panel1.Controls.Add(this.lbl_PartNo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(560, 141);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lblLotNo_show
            // 
            this.lblLotNo_show.AutoSize = true;
            this.lblLotNo_show.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLotNo_show.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblLotNo_show.ForeColor = System.Drawing.Color.Maroon;
            this.lblLotNo_show.Location = new System.Drawing.Point(90, 15);
            this.lblLotNo_show.Name = "lblLotNo_show";
            this.lblLotNo_show.Size = new System.Drawing.Size(2, 17);
            this.lblLotNo_show.TabIndex = 21;
            // 
            // lbl_LotNo
            // 
            this.lbl_LotNo.AutoSize = true;
            this.lbl_LotNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_LotNo.Location = new System.Drawing.Point(4, 15);
            this.lbl_LotNo.Name = "lbl_LotNo";
            this.lbl_LotNo.Size = new System.Drawing.Size(48, 15);
            this.lbl_LotNo.TabIndex = 20;
            this.lbl_LotNo.Text = "Lot No";
            // 
            // editMPNNo
            // 
            this.editMPNNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editMPNNo.Location = new System.Drawing.Point(90, 114);
            this.editMPNNo.Name = "editMPNNo";
            this.editMPNNo.Size = new System.Drawing.Size(167, 25);
            this.editMPNNo.TabIndex = 19;
            this.editMPNNo.TextChanged += new System.EventHandler(this.editMPNNo_TextChanged);
            this.editMPNNo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.editMPNNo_KeyUp);
            this.editMPNNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editMPNNo_KeyPress);
            // 
            // lblVN_show
            // 
            this.lblVN_show.AutoSize = true;
            this.lblVN_show.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVN_show.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblVN_show.ForeColor = System.Drawing.Color.Maroon;
            this.lblVN_show.Location = new System.Drawing.Point(90, 81);
            this.lblVN_show.Name = "lblVN_show";
            this.lblVN_show.Size = new System.Drawing.Size(2, 17);
            this.lblVN_show.TabIndex = 18;
            // 
            // lblPartNo_show
            // 
            this.lblPartNo_show.AutoSize = true;
            this.lblPartNo_show.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPartNo_show.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblPartNo_show.ForeColor = System.Drawing.Color.Maroon;
            this.lblPartNo_show.Location = new System.Drawing.Point(90, 48);
            this.lblPartNo_show.Name = "lblPartNo_show";
            this.lblPartNo_show.Size = new System.Drawing.Size(2, 17);
            this.lblPartNo_show.TabIndex = 17;
            // 
            // lbl_MPN
            // 
            this.lbl_MPN.AutoSize = true;
            this.lbl_MPN.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_MPN.Location = new System.Drawing.Point(4, 114);
            this.lbl_MPN.Name = "lbl_MPN";
            this.lbl_MPN.Size = new System.Drawing.Size(59, 15);
            this.lbl_MPN.TabIndex = 16;
            this.lbl_MPN.Text = "MPN No";
            // 
            // lbl_Vendor_Name
            // 
            this.lbl_Vendor_Name.AutoSize = true;
            this.lbl_Vendor_Name.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Vendor_Name.Location = new System.Drawing.Point(4, 81);
            this.lbl_Vendor_Name.Name = "lbl_Vendor_Name";
            this.lbl_Vendor_Name.Size = new System.Drawing.Size(86, 15);
            this.lbl_Vendor_Name.TabIndex = 15;
            this.lbl_Vendor_Name.Text = "Vendor Name";
            // 
            // lbl_PartNo
            // 
            this.lbl_PartNo.AutoSize = true;
            this.lbl_PartNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_PartNo.Location = new System.Drawing.Point(4, 48);
            this.lbl_PartNo.Name = "lbl_PartNo";
            this.lbl_PartNo.Size = new System.Drawing.Size(51, 15);
            this.lbl_PartNo.TabIndex = 14;
            this.lbl_PartNo.Text = "Part No";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvMPN);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 141);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(560, 214);
            this.panel2.TabIndex = 1;
            // 
            // dgvMPN
            // 
            this.dgvMPN.AllowUserToAddRows = false;
            this.dgvMPN.AllowUserToDeleteRows = false;
            this.dgvMPN.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMPN.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMPN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMPN.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VENDOR_PART_NO,
            this.CREATE_USER,
            this.CREATE_TIME,
            this.STATUS});
            this.dgvMPN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMPN.EnableHeadersVisualStyles = false;
            this.dgvMPN.Location = new System.Drawing.Point(0, 0);
            this.dgvMPN.MultiSelect = false;
            this.dgvMPN.Name = "dgvMPN";
            this.dgvMPN.ReadOnly = true;
            this.dgvMPN.RowHeadersWidth = 25;
            this.dgvMPN.RowTemplate.Height = 24;
            this.dgvMPN.Size = new System.Drawing.Size(560, 214);
            this.dgvMPN.TabIndex = 0;
            // 
            // VENDOR_PART_NO
            // 
            this.VENDOR_PART_NO.HeaderText = "MPN No";
            this.VENDOR_PART_NO.Name = "VENDOR_PART_NO";
            this.VENDOR_PART_NO.ReadOnly = true;
            this.VENDOR_PART_NO.Width = 130;
            // 
            // CREATE_USER
            // 
            this.CREATE_USER.HeaderText = "CREATE_USER";
            this.CREATE_USER.Name = "CREATE_USER";
            this.CREATE_USER.ReadOnly = true;
            this.CREATE_USER.Width = 130;
            // 
            // CREATE_TIME
            // 
            this.CREATE_TIME.HeaderText = "CREATE_TIME";
            this.CREATE_TIME.Name = "CREATE_TIME";
            this.CREATE_TIME.ReadOnly = true;
            this.CREATE_TIME.Width = 130;
            // 
            // STATUS
            // 
            this.STATUS.HeaderText = "Status";
            this.STATUS.Name = "STATUS";
            this.STATUS.ReadOnly = true;
            this.STATUS.Width = 130;
            // 
            // fMPN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 355);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "fMPN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MPN";
            this.Load += new System.EventHandler(this.fMPN_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMPN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbl_PartNo;
        private System.Windows.Forms.Label lbl_Vendor_Name;
        private System.Windows.Forms.Label lbl_MPN;
        public System.Windows.Forms.Label lblPartNo_show;
        public System.Windows.Forms.Label lblVN_show;
        public System.Windows.Forms.TextBox editMPNNo;
        private System.Windows.Forms.DataGridView dgvMPN;
        public System.Windows.Forms.Label lblLotNo_show;
        private System.Windows.Forms.Label lbl_LotNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn VENDOR_PART_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn CREATE_USER;
        private System.Windows.Forms.DataGridViewTextBoxColumn CREATE_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn STATUS;
    }
}