namespace QualityInspectionAAR
{
    partial class FData
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TlpForm = new System.Windows.Forms.TableLayoutPanel();
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.Lb_7 = new System.Windows.Forms.Label();
            this.LbRCQty = new System.Windows.Forms.Label();
            this.Lb_5 = new System.Windows.Forms.Label();
            this.Lb_8 = new System.Windows.Forms.Label();
            this.LbEmp = new System.Windows.Forms.Label();
            this.LbRCInspectQty = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.DgvData = new System.Windows.Forms.DataGridView();
            this.DgvLog = new System.Windows.Forms.DataGridView();
            this.Sc_1 = new System.Windows.Forms.SplitContainer();
            this.TlpForm.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Sc_1)).BeginInit();
            this.Sc_1.Panel1.SuspendLayout();
            this.Sc_1.Panel2.SuspendLayout();
            this.Sc_1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TlpForm
            // 
            this.TlpForm.ColumnCount = 2;
            this.TlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpForm.Controls.Add(this.Sc_1, 0, 1);
            this.TlpForm.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.TlpForm.Controls.Add(this.BtnSave, 0, 2);
            this.TlpForm.Controls.Add(this.BtnClose, 1, 2);
            this.TlpForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpForm.Location = new System.Drawing.Point(0, 0);
            this.TlpForm.Margin = new System.Windows.Forms.Padding(4);
            this.TlpForm.Name = "TlpForm";
            this.TlpForm.RowCount = 3;
            this.TlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.TlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.TlpForm.Size = new System.Drawing.Size(784, 561);
            this.TlpForm.TabIndex = 0;
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnClose.Location = new System.Drawing.Point(680, 526);
            this.BtnClose.Margin = new System.Windows.Forms.Padding(4);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(100, 30);
            this.BtnClose.TabIndex = 11;
            this.BtnClose.Text = "CLOSE";
            this.BtnClose.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnSave.Location = new System.Drawing.Point(572, 526);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(100, 30);
            this.BtnSave.TabIndex = 10;
            this.BtnSave.Text = "SAVE";
            this.BtnSave.UseVisualStyleBackColor = true;
            // 
            // Lb_7
            // 
            this.Lb_7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Lb_7.AutoSize = true;
            this.Lb_7.Location = new System.Drawing.Point(203, 9);
            this.Lb_7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_7.Name = "Lb_7";
            this.Lb_7.Size = new System.Drawing.Size(126, 16);
            this.Lb_7.TabIndex = 15;
            this.Lb_7.Text = "RC REMAIN QTY";
            this.Lb_7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbRCQty
            // 
            this.LbRCQty.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LbRCQty.AutoSize = true;
            this.LbRCQty.Location = new System.Drawing.Point(337, 9);
            this.LbRCQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbRCQty.Name = "LbRCQty";
            this.LbRCQty.Size = new System.Drawing.Size(16, 16);
            this.LbRCQty.TabIndex = 16;
            this.LbRCQty.Text = "0";
            this.LbRCQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Lb_5
            // 
            this.Lb_5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Lb_5.AutoSize = true;
            this.Lb_5.Location = new System.Drawing.Point(4, 9);
            this.Lb_5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_5.Name = "Lb_5";
            this.Lb_5.Size = new System.Drawing.Size(38, 16);
            this.Lb_5.TabIndex = 19;
            this.Lb_5.Text = "EMP";
            this.Lb_5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Lb_8
            // 
            this.Lb_8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Lb_8.AutoSize = true;
            this.Lb_8.Location = new System.Drawing.Point(490, 9);
            this.Lb_8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lb_8.Name = "Lb_8";
            this.Lb_8.Size = new System.Drawing.Size(136, 16);
            this.Lb_8.TabIndex = 17;
            this.Lb_8.Text = "RC SAMPLED QTY";
            this.Lb_8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbEmp
            // 
            this.LbEmp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LbEmp.AutoSize = true;
            this.LbEmp.Location = new System.Drawing.Point(50, 9);
            this.LbEmp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbEmp.Name = "LbEmp";
            this.LbEmp.Size = new System.Drawing.Size(18, 16);
            this.LbEmp.TabIndex = 20;
            this.LbEmp.Text = "--";
            this.LbEmp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LbRCInspectQty
            // 
            this.LbRCInspectQty.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LbRCInspectQty.AutoSize = true;
            this.LbRCInspectQty.Location = new System.Drawing.Point(634, 9);
            this.LbRCInspectQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbRCInspectQty.Name = "LbRCInspectQty";
            this.LbRCInspectQty.Size = new System.Drawing.Size(16, 16);
            this.LbRCInspectQty.TabIndex = 18;
            this.LbRCInspectQty.Text = "0";
            this.LbRCInspectQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.TlpForm.SetColumnSpan(this.tableLayoutPanel1, 2);
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Controls.Add(this.LbRCInspectQty, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.LbEmp, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Lb_8, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.Lb_5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LbRCQty, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.Lb_7, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 35);
            this.tableLayoutPanel1.TabIndex = 22;
            // 
            // DgvData
            // 
            this.DgvData.AllowUserToAddRows = false;
            this.DgvData.AllowUserToDeleteRows = false;
            this.DgvData.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.DgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvData.Location = new System.Drawing.Point(0, 0);
            this.DgvData.Margin = new System.Windows.Forms.Padding(2);
            this.DgvData.Name = "DgvData";
            this.DgvData.RowTemplate.Height = 24;
            this.DgvData.Size = new System.Drawing.Size(778, 153);
            this.DgvData.TabIndex = 3;
            // 
            // DgvLog
            // 
            this.DgvLog.AllowUserToAddRows = false;
            this.DgvLog.AllowUserToDeleteRows = false;
            this.DgvLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DgvLog.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.DgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvLog.Location = new System.Drawing.Point(0, 0);
            this.DgvLog.Margin = new System.Windows.Forms.Padding(2);
            this.DgvLog.Name = "DgvLog";
            this.DgvLog.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.DgvLog.RowTemplate.Height = 24;
            this.DgvLog.Size = new System.Drawing.Size(778, 323);
            this.DgvLog.TabIndex = 2;
            // 
            // Sc_1
            // 
            this.TlpForm.SetColumnSpan(this.Sc_1, 2);
            this.Sc_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Sc_1.Location = new System.Drawing.Point(3, 38);
            this.Sc_1.Name = "Sc_1";
            this.Sc_1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Sc_1.Panel1
            // 
            this.Sc_1.Panel1.Controls.Add(this.DgvLog);
            // 
            // Sc_1.Panel2
            // 
            this.Sc_1.Panel2.Controls.Add(this.DgvData);
            this.Sc_1.Size = new System.Drawing.Size(778, 480);
            this.Sc_1.SplitterDistance = 323;
            this.Sc_1.TabIndex = 21;
            // 
            // FData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.TlpForm);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RC AQL";
            this.TlpForm.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvLog)).EndInit();
            this.Sc_1.Panel1.ResumeLayout(false);
            this.Sc_1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Sc_1)).EndInit();
            this.Sc_1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TlpForm;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.SplitContainer Sc_1;
        private System.Windows.Forms.DataGridView DgvLog;
        private System.Windows.Forms.DataGridView DgvData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label LbRCInspectQty;
        private System.Windows.Forms.Label LbEmp;
        private System.Windows.Forms.Label Lb_8;
        private System.Windows.Forms.Label Lb_5;
        private System.Windows.Forms.Label LbRCQty;
        private System.Windows.Forms.Label Lb_7;
    }
}

