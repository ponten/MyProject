namespace RCPart
{
    partial class fSQLMultiList
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
            this.lstAll = new System.Windows.Forms.ListBox();
            this.lstSelect = new System.Windows.Forms.ListBox();
            this.btnAddOne = new System.Windows.Forms.Button();
            this.btnRemoveOne = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.LabPart = new System.Windows.Forms.Label();
            this.lablPartNo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstAll
            // 
            this.lstAll.AllowDrop = true;
            this.lstAll.FormattingEnabled = true;
            this.lstAll.ItemHeight = 12;
            this.lstAll.Location = new System.Drawing.Point(12, 24);
            this.lstAll.Name = "lstAll";
            this.lstAll.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAll.Size = new System.Drawing.Size(188, 280);
            this.lstAll.TabIndex = 0;
            // 
            // lstSelect
            // 
            this.lstSelect.AllowDrop = true;
            this.lstSelect.FormattingEnabled = true;
            this.lstSelect.ItemHeight = 12;
            this.lstSelect.Location = new System.Drawing.Point(254, 24);
            this.lstSelect.Name = "lstSelect";
            this.lstSelect.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelect.Size = new System.Drawing.Size(184, 280);
            this.lstSelect.TabIndex = 1;
            // 
            // btnAddOne
            // 
            this.btnAddOne.Location = new System.Drawing.Point(206, 56);
            this.btnAddOne.Name = "btnAddOne";
            this.btnAddOne.Size = new System.Drawing.Size(36, 23);
            this.btnAddOne.TabIndex = 2;
            this.btnAddOne.Text = ">";
            this.btnAddOne.UseVisualStyleBackColor = true;
            this.btnAddOne.Click += new System.EventHandler(this.btnAddOne_Click);
            // 
            // btnRemoveOne
            // 
            this.btnRemoveOne.Location = new System.Drawing.Point(206, 141);
            this.btnRemoveOne.Name = "btnRemoveOne";
            this.btnRemoveOne.Size = new System.Drawing.Size(36, 23);
            this.btnRemoveOne.TabIndex = 3;
            this.btnRemoveOne.Text = "<";
            this.btnRemoveOne.UseVisualStyleBackColor = true;
            this.btnRemoveOne.Click += new System.EventHandler(this.btnRemoveOne_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(206, 85);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(36, 23);
            this.btnAddAll.TabIndex = 4;
            this.btnAddAll.Text = ">>";
            this.btnAddAll.UseVisualStyleBackColor = true;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(206, 170);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(36, 23);
            this.btnRemoveAll.TabIndex = 5;
            this.btnRemoveAll.Text = "<<";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.Location = new System.Drawing.Point(125, 316);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(254, 316);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // LabPart
            // 
            this.LabPart.AutoSize = true;
            this.LabPart.BackColor = System.Drawing.Color.Transparent;
            this.LabPart.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.LabPart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabPart.Location = new System.Drawing.Point(13, 5);
            this.LabPart.Name = "LabPart";
            this.LabPart.Size = new System.Drawing.Size(44, 13);
            this.LabPart.TabIndex = 8;
            this.LabPart.Text = "Part No";
            // 
            // lablPartNo
            // 
            this.lablPartNo.AutoSize = true;
            this.lablPartNo.BackColor = System.Drawing.Color.Transparent;
            this.lablPartNo.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.lablPartNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablPartNo.Location = new System.Drawing.Point(75, 5);
            this.lablPartNo.Name = "lablPartNo";
            this.lablPartNo.Size = new System.Drawing.Size(28, 13);
            this.lablPartNo.TabIndex = 9;
            this.lablPartNo.Text = "N/A";
            // 
            // fSQLMultiList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 345);
            this.Controls.Add(this.lablPartNo);
            this.Controls.Add(this.LabPart);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRemoveAll);
            this.Controls.Add(this.btnAddAll);
            this.Controls.Add(this.btnRemoveOne);
            this.Controls.Add(this.btnAddOne);
            this.Controls.Add(this.lstSelect);
            this.Controls.Add(this.lstAll);
            this.MinimumSize = new System.Drawing.Size(469, 383);
            this.Name = "fSQLMultiList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List";
            this.Load += new System.EventHandler(this.fMultiList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddOne;
        private System.Windows.Forms.Button btnRemoveOne;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Button btnRemoveAll;
        public System.Windows.Forms.ListBox lstAll;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ListBox lstSelect;
        private System.Windows.Forms.Label LabPart;
        private System.Windows.Forms.Label lablPartNo;
    }
}