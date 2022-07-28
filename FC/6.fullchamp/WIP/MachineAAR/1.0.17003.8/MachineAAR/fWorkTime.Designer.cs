namespace MachineAAR
{
    partial class fWorkTime
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.DtpStart = new System.Windows.Forms.DateTimePicker();
            this.DtpEnd = new System.Windows.Forms.DateTimePicker();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.CbStart = new System.Windows.Forms.CheckBox();
            this.CbEnd = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.DtpStart, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.DtpEnd, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.BtnOK, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.BtnCancel, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.CbStart, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.CbEnd, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(334, 161);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // DtpStart
            // 
            this.DtpStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.DtpStart, 2);
            this.DtpStart.CustomFormat = "yyyy/ MM/ dd HH: mm: ss";
            this.DtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStart.Location = new System.Drawing.Point(96, 41);
            this.DtpStart.Margin = new System.Windows.Forms.Padding(0);
            this.DtpStart.Name = "DtpStart";
            this.DtpStart.Size = new System.Drawing.Size(234, 27);
            this.DtpStart.TabIndex = 2;
            // 
            // DtpEnd
            // 
            this.DtpEnd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.DtpEnd, 2);
            this.DtpEnd.CustomFormat = "yyyy/ MM/ dd HH: mm: ss";
            this.DtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEnd.Location = new System.Drawing.Point(96, 91);
            this.DtpEnd.Margin = new System.Windows.Forms.Padding(0);
            this.DtpEnd.Name = "DtpEnd";
            this.DtpEnd.Size = new System.Drawing.Size(234, 27);
            this.DtpEnd.TabIndex = 3;
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnOK.AutoSize = true;
            this.BtnOK.Location = new System.Drawing.Point(175, 127);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 27);
            this.BtnOK.TabIndex = 4;
            this.BtnOK.Text = "Confirm";
            this.BtnOK.UseVisualStyleBackColor = true;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnCancel.Location = new System.Drawing.Point(256, 127);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 27);
            this.BtnCancel.TabIndex = 5;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // CbStart
            // 
            this.CbStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CbStart.AutoSize = true;
            this.CbStart.Location = new System.Drawing.Point(5, 45);
            this.CbStart.Margin = new System.Windows.Forms.Padding(5);
            this.CbStart.Name = "CbStart";
            this.CbStart.Size = new System.Drawing.Size(86, 20);
            this.CbStart.TabIndex = 6;
            this.CbStart.Text = "Start time";
            this.CbStart.UseVisualStyleBackColor = true;
            // 
            // CbEnd
            // 
            this.CbEnd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CbEnd.AutoSize = true;
            this.CbEnd.Location = new System.Drawing.Point(5, 95);
            this.CbEnd.Margin = new System.Windows.Forms.Padding(5);
            this.CbEnd.Name = "CbEnd";
            this.CbEnd.Size = new System.Drawing.Size(83, 20);
            this.CbEnd.TabIndex = 7;
            this.CbEnd.Text = "End time";
            this.CbEnd.UseVisualStyleBackColor = true;
            // 
            // fWorkTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(334, 161);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fWorkTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set work time";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DateTimePicker DtpStart;
        private System.Windows.Forms.DateTimePicker DtpEnd;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.CheckBox CbStart;
        private System.Windows.Forms.CheckBox CbEnd;
    }
}