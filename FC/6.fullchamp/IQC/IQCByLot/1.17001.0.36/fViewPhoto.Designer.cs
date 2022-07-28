namespace IQCbyLot
{
    partial class fViewPhoto
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
            this.PanelImage = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lablLotNo = new System.Windows.Forms.Label();
            this.lbl_LotNo = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelImage
            // 
            this.PanelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelImage.Location = new System.Drawing.Point(0, 55);
            this.PanelImage.Name = "PanelImage";
            this.PanelImage.Size = new System.Drawing.Size(377, 317);
            this.PanelImage.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.lablLotNo);
            this.panel1.Controls.Add(this.lbl_LotNo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(377, 55);
            this.panel1.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(297, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lablLotNo
            // 
            this.lablLotNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablLotNo.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.lablLotNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lablLotNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablLotNo.Location = new System.Drawing.Point(82, 11);
            this.lablLotNo.Name = "lablLotNo";
            this.lablLotNo.Size = new System.Drawing.Size(184, 20);
            this.lablLotNo.TabIndex = 29;
            this.lablLotNo.Text = "N/A";
            // 
            // lbl_LotNo
            // 
            this.lbl_LotNo.AutoSize = true;
            this.lbl_LotNo.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_LotNo.Location = new System.Drawing.Point(12, 15);
            this.lbl_LotNo.Name = "lbl_LotNo";
            this.lbl_LotNo.Size = new System.Drawing.Size(41, 13);
            this.lbl_LotNo.TabIndex = 28;
            this.lbl_LotNo.Text = "Lot No";
            // 
            // fViewPhoto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 372);
            this.Controls.Add(this.PanelImage);
            this.Controls.Add(this.panel1);
            this.Name = "fViewPhoto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View Photo";
            this.Load += new System.EventHandler(this.fViewPhoto_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lablLotNo;
        private System.Windows.Forms.Label lbl_LotNo;
    }
}