namespace PackCarton
{
    partial class fPrintNewRC
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
            this.lbRC = new System.Windows.Forms.Label();
            this.lbQTY = new System.Windows.Forms.Label();
            this.tbRC = new System.Windows.Forms.TextBox();
            this.tbQTY = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbRC
            // 
            this.lbRC.AutoSize = true;
            this.lbRC.Location = new System.Drawing.Point(16, 25);
            this.lbRC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRC.Name = "lbRC";
            this.lbRC.Size = new System.Drawing.Size(49, 15);
            this.lbRC.TabIndex = 0;
            this.lbRC.Text = "RC NO";
            // 
            // lbQTY
            // 
            this.lbQTY.AutoSize = true;
            this.lbQTY.Location = new System.Drawing.Point(16, 76);
            this.lbQTY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbQTY.Name = "lbQTY";
            this.lbQTY.Size = new System.Drawing.Size(36, 15);
            this.lbQTY.TabIndex = 1;
            this.lbQTY.Text = "QTY";
            // 
            // tbRC
            // 
            this.tbRC.Location = new System.Drawing.Point(171, 21);
            this.tbRC.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbRC.Name = "tbRC";
            this.tbRC.ReadOnly = true;
            this.tbRC.Size = new System.Drawing.Size(332, 25);
            this.tbRC.TabIndex = 2;
            // 
            // tbQTY
            // 
            this.tbQTY.Location = new System.Drawing.Point(171, 72);
            this.tbQTY.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbQTY.Name = "tbQTY";
            this.tbQTY.ReadOnly = true;
            this.tbQTY.Size = new System.Drawing.Size(332, 25);
            this.tbQTY.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(404, 120);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 29);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(296, 120);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 29);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // fPrintNewRC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(539, 164);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tbQTY);
            this.Controls.Add(this.tbRC);
            this.Controls.Add(this.lbQTY);
            this.Controls.Add(this.lbRC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "fPrintNewRC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrintNewRC";
            this.Load += new System.EventHandler(this.fPrintNewRC_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbRC;
        private System.Windows.Forms.Label lbQTY;
        private System.Windows.Forms.TextBox tbRC;
        private System.Windows.Forms.TextBox tbQTY;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrint;
    }
}