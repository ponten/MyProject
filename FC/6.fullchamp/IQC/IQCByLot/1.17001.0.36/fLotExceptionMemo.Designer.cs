namespace IQCbyLot
{
    partial class fLotExceptionMemo
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
            this.editTypeMemo = new System.Windows.Forms.TextBox();
            this.lablMemo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lablLotNo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.editReceiveQty = new System.Windows.Forms.TextBox();
            this.lablAcceptQty = new System.Windows.Forms.Label();
            this.editLotsize = new System.Windows.Forms.TextBox();
            this.lablLotSize = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.editWaiveNo = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // editTypeMemo
            // 
            this.editTypeMemo.BackColor = System.Drawing.Color.White;
            this.editTypeMemo.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editTypeMemo.Location = new System.Drawing.Point(13, 22);
            this.editTypeMemo.Multiline = true;
            this.editTypeMemo.Name = "editTypeMemo";
            this.editTypeMemo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.editTypeMemo.Size = new System.Drawing.Size(422, 166);
            this.editTypeMemo.TabIndex = 43;
            // 
            // lablMemo
            // 
            this.lablMemo.AutoSize = true;
            this.lablMemo.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablMemo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablMemo.Location = new System.Drawing.Point(8, 3);
            this.lablMemo.Name = "lablMemo";
            this.lablMemo.Size = new System.Drawing.Size(115, 16);
            this.lablMemo.TabIndex = 44;
            this.lablMemo.Text = "Exception Memo";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lablLotNo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(447, 40);
            this.panel1.TabIndex = 45;
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
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 269);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(447, 40);
            this.panel2.TabIndex = 46;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Location = new System.Drawing.Point(241, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            // panel3
            // 
            this.panel3.Controls.Add(this.lablMemo);
            this.panel3.Controls.Add(this.editTypeMemo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 132);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(447, 137);
            this.panel3.TabIndex = 47;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.editWaiveNo);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.editReceiveQty);
            this.panel4.Controls.Add(this.lablAcceptQty);
            this.panel4.Controls.Add(this.editLotsize);
            this.panel4.Controls.Add(this.lablLotSize);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 40);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(447, 92);
            this.panel4.TabIndex = 48;
            // 
            // editReceiveQty
            // 
            this.editReceiveQty.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editReceiveQty.Location = new System.Drawing.Point(115, 30);
            this.editReceiveQty.Margin = new System.Windows.Forms.Padding(4);
            this.editReceiveQty.Name = "editReceiveQty";
            this.editReceiveQty.Size = new System.Drawing.Size(201, 25);
            this.editReceiveQty.TabIndex = 52;
            // 
            // lablAcceptQty
            // 
            this.lablAcceptQty.AutoSize = true;
            this.lablAcceptQty.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablAcceptQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablAcceptQty.Location = new System.Drawing.Point(8, 35);
            this.lablAcceptQty.Name = "lablAcceptQty";
            this.lablAcceptQty.Size = new System.Drawing.Size(76, 15);
            this.lablAcceptQty.TabIndex = 53;
            this.lablAcceptQty.Text = "Receive Qty";
            // 
            // editLotsize
            // 
            this.editLotsize.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editLotsize.Location = new System.Drawing.Point(115, 2);
            this.editLotsize.Margin = new System.Windows.Forms.Padding(4);
            this.editLotsize.Name = "editLotsize";
            this.editLotsize.Size = new System.Drawing.Size(201, 25);
            this.editLotsize.TabIndex = 51;
            this.editLotsize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editLotsize_KeyPress);
            // 
            // lablLotSize
            // 
            this.lablLotSize.AutoSize = true;
            this.lablLotSize.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablLotSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablLotSize.Location = new System.Drawing.Point(8, 7);
            this.lablLotSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.lablLotSize.Name = "lablLotSize";
            this.lablLotSize.Size = new System.Drawing.Size(55, 15);
            this.lablLotSize.TabIndex = 50;
            this.lablLotSize.Text = "Lot Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(8, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 15);
            this.label1.TabIndex = 54;
            this.label1.Text = "Waive No";
            // 
            // editWaiveNo
            // 
            this.editWaiveNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editWaiveNo.Location = new System.Drawing.Point(115, 58);
            this.editWaiveNo.Margin = new System.Windows.Forms.Padding(4);
            this.editWaiveNo.Name = "editWaiveNo";
            this.editWaiveNo.Size = new System.Drawing.Size(201, 25);
            this.editWaiveNo.TabIndex = 55;
            // 
            // fLotExceptionMemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 309);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fLotExceptionMemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Lot Exception Memo";
            this.Load += new System.EventHandler(this.fLotExceptionMemo_Load);
            this.Activated += new System.EventHandler(this.fLotExceptionMemo_Activated);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox editTypeMemo;
        private System.Windows.Forms.Label lablMemo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lablLotNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lablLotSize;
        public System.Windows.Forms.TextBox editLotsize;
        public System.Windows.Forms.TextBox editReceiveQty;
        private System.Windows.Forms.Label lablAcceptQty;
        public System.Windows.Forms.TextBox editWaiveNo;
        private System.Windows.Forms.Label label1;
    }
}