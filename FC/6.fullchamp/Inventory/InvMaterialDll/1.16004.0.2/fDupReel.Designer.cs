namespace InvMaterialdll
{
    partial class fDupReel
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
            this.dgvSelectSN = new System.Windows.Forms.DataGridView();
            this.REEL_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PART_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VENDOR_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATECODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REEL_MEMO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.lablReelNo = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectSN)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSelectSN
            // 
            this.dgvSelectSN.AllowUserToAddRows = false;
            this.dgvSelectSN.AllowUserToDeleteRows = false;
            this.dgvSelectSN.BackgroundColor = System.Drawing.Color.White;
            this.dgvSelectSN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelectSN.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.REEL_NO,
            this.PART_NO,
            this.VENDOR_NAME,
            this.DATECODE,
            this.LOT,
            this.REEL_MEMO});
            this.dgvSelectSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelectSN.Location = new System.Drawing.Point(3, 23);
            this.dgvSelectSN.Name = "dgvSelectSN";
            this.dgvSelectSN.RowHeadersWidth = 20;
            this.dgvSelectSN.RowTemplate.Height = 24;
            this.dgvSelectSN.Size = new System.Drawing.Size(630, 238);
            this.dgvSelectSN.TabIndex = 2;
            // 
            // REEL_NO
            // 
            this.REEL_NO.HeaderText = "Reel No";
            this.REEL_NO.Name = "REEL_NO";
            this.REEL_NO.Width = 120;
            // 
            // PART_NO
            // 
            this.PART_NO.HeaderText = "Part No";
            this.PART_NO.Name = "PART_NO";
            // 
            // VENDOR_NAME
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.VENDOR_NAME.DefaultCellStyle = dataGridViewCellStyle1;
            this.VENDOR_NAME.HeaderText = "Vendor Name";
            this.VENDOR_NAME.Name = "VENDOR_NAME";
            this.VENDOR_NAME.Width = 150;
            // 
            // DATECODE
            // 
            this.DATECODE.HeaderText = "Date Code";
            this.DATECODE.Name = "DATECODE";
            // 
            // LOT
            // 
            this.LOT.HeaderText = "Lot";
            this.LOT.Name = "LOT";
            // 
            // REEL_MEMO
            // 
            this.REEL_MEMO.HeaderText = "Memo";
            this.REEL_MEMO.Name = "REEL_MEMO";
            this.REEL_MEMO.Width = 200;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 303);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(636, 46);
            this.panel1.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Location = new System.Drawing.Point(336, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOK.Location = new System.Drawing.Point(230, 11);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(79, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Stock";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.lablReelNo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(636, 39);
            this.panel2.TabIndex = 4;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("新細明體", 12F);
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(3, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(103, 16);
            this.label12.TabIndex = 57;
            this.label12.Text = "Reel No Range";
            // 
            // lablReelNo
            // 
            this.lablReelNo.AutoSize = true;
            this.lablReelNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablReelNo.Font = new System.Drawing.Font("新細明體", 12F);
            this.lablReelNo.ForeColor = System.Drawing.Color.Maroon;
            this.lablReelNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablReelNo.Location = new System.Drawing.Point(136, 9);
            this.lablReelNo.Name = "lablReelNo";
            this.lablReelNo.Size = new System.Drawing.Size(36, 18);
            this.lablReelNo.TabIndex = 56;
            this.lablReelNo.Text = "N/A";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvSelectSN);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(0, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(636, 264);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Duplicate Reel No";
            // 
            // fDupReel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 349);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fDupReel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Duplicate Reel No";
            this.Load += new System.EventHandler(this.fDupReel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectSN)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgvSelectSN;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.Label lablReelNo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn REEL_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PART_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn VENDOR_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATECODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOT;
        private System.Windows.Forms.DataGridViewTextBoxColumn REEL_MEMO;
        public System.Windows.Forms.Button btnOK;
    }
}