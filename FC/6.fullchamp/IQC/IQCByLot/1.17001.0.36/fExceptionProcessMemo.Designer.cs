namespace IQCbyLot
{
    partial class fExceptionProcessMemo
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.lablStatus = new System.Windows.Forms.Label();
            this.lablMemo = new System.Windows.Forms.Label();
            this.editTypeMemo = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lablLotNo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lablStatus);
            this.panel3.Controls.Add(this.lablMemo);
            this.panel3.Controls.Add(this.editTypeMemo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(451, 210);
            this.panel3.TabIndex = 50;
            // 
            // lablStatus
            // 
            this.lablStatus.AutoSize = true;
            this.lablStatus.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablStatus.Location = new System.Drawing.Point(8, 7);
            this.lablStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.lablStatus.Name = "lablStatus";
            this.lablStatus.Size = new System.Drawing.Size(39, 15);
            this.lablStatus.TabIndex = 50;
            this.lablStatus.Text = "Start";
            // 
            // lablMemo
            // 
            this.lablMemo.AutoSize = true;
            this.lablMemo.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablMemo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablMemo.Location = new System.Drawing.Point(8, 28);
            this.lablMemo.Name = "lablMemo";
            this.lablMemo.Size = new System.Drawing.Size(48, 16);
            this.lablMemo.TabIndex = 44;
            this.lablMemo.Text = "Memo";
            // 
            // editTypeMemo
            // 
            this.editTypeMemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.editTypeMemo.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editTypeMemo.Location = new System.Drawing.Point(13, 49);
            this.editTypeMemo.Multiline = true;
            this.editTypeMemo.Name = "editTypeMemo";
            this.editTypeMemo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.editTypeMemo.Size = new System.Drawing.Size(422, 139);
            this.editTypeMemo.TabIndex = 43;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 247);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(451, 40);
            this.panel2.TabIndex = 49;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Location = new System.Drawing.Point(241, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSave.Location = new System.Drawing.Point(133, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lablLotNo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(451, 37);
            this.panel1.TabIndex = 48;
            // 
            // lablLotNo
            // 
            this.lablLotNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablLotNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablLotNo.ForeColor = System.Drawing.Color.Maroon;
            this.lablLotNo.Location = new System.Drawing.Point(117, 9);
            this.lablLotNo.Name = "lablLotNo";
            this.lablLotNo.Size = new System.Drawing.Size(199, 25);
            this.lablLotNo.TabIndex = 51;
            this.lablLotNo.Text = "N/A";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(8, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 15);
            this.label3.TabIndex = 49;
            this.label3.Text = "Lot No";
            // 
            // fExceptionProcessMemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 287);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fExceptionProcessMemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Exception Memo";
            this.Load += new System.EventHandler(this.fExceptionProcessMemo_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lablMemo;
        public System.Windows.Forms.TextBox editTypeMemo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lablLotNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lablStatus;
    }
}