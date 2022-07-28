namespace IQCbyLot
{
    partial class fHold
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lablStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lablLotNo = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.editLotMemo = new System.Windows.Forms.TextBox();
            this.lablMemo = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.editMemoHistory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lablStatus);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lablLotNo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(515, 77);
            this.panel1.TabIndex = 0;
            // 
            // lablStatus
            // 
            this.lablStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablStatus.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablStatus.ForeColor = System.Drawing.Color.Maroon;
            this.lablStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablStatus.Location = new System.Drawing.Point(113, 44);
            this.lablStatus.Name = "lablStatus";
            this.lablStatus.Size = new System.Drawing.Size(266, 25);
            this.lablStatus.TabIndex = 31;
            this.lablStatus.Text = "N/A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 30;
            this.label2.Text = "Status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 28;
            this.label1.Text = "Lot No";
            // 
            // lablLotNo
            // 
            this.lablLotNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablLotNo.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablLotNo.ForeColor = System.Drawing.Color.Maroon;
            this.lablLotNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablLotNo.Location = new System.Drawing.Point(113, 14);
            this.lablLotNo.Name = "lablLotNo";
            this.lablLotNo.Size = new System.Drawing.Size(266, 25);
            this.lablLotNo.TabIndex = 29;
            this.lablLotNo.Text = "N/A";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel2.Location = new System.Drawing.Point(0, 347);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(515, 41);
            this.panel2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Location = new System.Drawing.Point(281, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSave.Location = new System.Drawing.Point(161, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // editLotMemo
            // 
            this.editLotMemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.editLotMemo.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editLotMemo.Location = new System.Drawing.Point(113, 259);
            this.editLotMemo.Multiline = true;
            this.editLotMemo.Name = "editLotMemo";
            this.editLotMemo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.editLotMemo.Size = new System.Drawing.Size(391, 82);
            this.editLotMemo.TabIndex = 39;
            // 
            // lablMemo
            // 
            this.lablMemo.AutoSize = true;
            this.lablMemo.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablMemo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablMemo.Location = new System.Drawing.Point(12, 259);
            this.lablMemo.Name = "lablMemo";
            this.lablMemo.Size = new System.Drawing.Size(48, 16);
            this.lablMemo.TabIndex = 40;
            this.lablMemo.Text = "Memo";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.editMemoHistory);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 77);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(515, 176);
            this.panel3.TabIndex = 41;
            // 
            // editMemoHistory
            // 
            this.editMemoHistory.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editMemoHistory.Location = new System.Drawing.Point(113, 9);
            this.editMemoHistory.Multiline = true;
            this.editMemoHistory.Name = "editMemoHistory";
            this.editMemoHistory.ReadOnly = true;
            this.editMemoHistory.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.editMemoHistory.Size = new System.Drawing.Size(390, 164);
            this.editMemoHistory.TabIndex = 40;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 16);
            this.label3.TabIndex = 41;
            this.label3.Text = "History";
            // 
            // fHold
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 388);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.editLotMemo);
            this.Controls.Add(this.lablMemo);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fHold";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "fHold";
            this.Load += new System.EventHandler(this.fHold_Load);
            this.Shown += new System.EventHandler(this.fHold_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.TextBox editLotMemo;
        private System.Windows.Forms.Label lablMemo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lablLotNo;
        private System.Windows.Forms.Label lablStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox editMemoHistory;
    }
}