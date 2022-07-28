namespace RCHold
{
    partial class fSelect
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
            this.RBtn_Hold = new System.Windows.Forms.RadioButton();
            this.RBtn_WaitTransfer = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCANCEL = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RBtn_Hold
            // 
            this.RBtn_Hold.AutoSize = true;
            this.RBtn_Hold.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RBtn_Hold.Location = new System.Drawing.Point(45, 23);
            this.RBtn_Hold.Name = "RBtn_Hold";
            this.RBtn_Hold.Size = new System.Drawing.Size(57, 20);
            this.RBtn_Hold.TabIndex = 0;
            this.RBtn_Hold.TabStop = true;
            this.RBtn_Hold.Text = "Hold";
            this.RBtn_Hold.UseVisualStyleBackColor = true;
            // 
            // RBtn_WaitTransfer
            // 
            this.RBtn_WaitTransfer.AutoSize = true;
            this.RBtn_WaitTransfer.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RBtn_WaitTransfer.Location = new System.Drawing.Point(45, 56);
            this.RBtn_WaitTransfer.Name = "RBtn_WaitTransfer";
            this.RBtn_WaitTransfer.Size = new System.Drawing.Size(111, 20);
            this.RBtn_WaitTransfer.TabIndex = 1;
            this.RBtn_WaitTransfer.TabStop = true;
            this.RBtn_WaitTransfer.Text = "Wait Transfer";
            this.RBtn_WaitTransfer.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(45, 89);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCANCEL
            // 
            this.btnCANCEL.Location = new System.Drawing.Point(139, 89);
            this.btnCANCEL.Name = "btnCANCEL";
            this.btnCANCEL.Size = new System.Drawing.Size(75, 25);
            this.btnCANCEL.TabIndex = 3;
            this.btnCANCEL.Text = "Cancel";
            this.btnCANCEL.UseVisualStyleBackColor = true;
            this.btnCANCEL.Click += new System.EventHandler(this.btnCANCEL_Click);
            // 
            // fSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 147);
            this.ControlBox = false;
            this.Controls.Add(this.btnCANCEL);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.RBtn_WaitTransfer);
            this.Controls.Add(this.RBtn_Hold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.Load += new System.EventHandler(this.fSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton RBtn_Hold;
        private System.Windows.Forms.RadioButton RBtn_WaitTransfer;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCANCEL;
    }
}