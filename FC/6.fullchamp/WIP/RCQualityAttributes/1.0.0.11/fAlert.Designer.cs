namespace RCQualityAttributes
{
    partial class fAlert
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
            this.btnOK = new System.Windows.Forms.Button();
            this.RBtn_Scrap = new System.Windows.Forms.RadioButton();
            this.RBtn_Release = new System.Windows.Forms.RadioButton();
            this.btnCANCEL = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(20, 76);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // RBtn_Scrap
            // 
            this.RBtn_Scrap.AutoSize = true;
            this.RBtn_Scrap.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RBtn_Scrap.Location = new System.Drawing.Point(21, 42);
            this.RBtn_Scrap.Name = "RBtn_Scrap";
            this.RBtn_Scrap.Size = new System.Drawing.Size(61, 20);
            this.RBtn_Scrap.TabIndex = 19;
            this.RBtn_Scrap.TabStop = true;
            this.RBtn_Scrap.Text = "Scrap";
            this.RBtn_Scrap.UseVisualStyleBackColor = true;
            // 
            // RBtn_Release
            // 
            this.RBtn_Release.AutoSize = true;
            this.RBtn_Release.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RBtn_Release.Location = new System.Drawing.Point(21, 12);
            this.RBtn_Release.Name = "RBtn_Release";
            this.RBtn_Release.Size = new System.Drawing.Size(74, 20);
            this.RBtn_Release.TabIndex = 18;
            this.RBtn_Release.TabStop = true;
            this.RBtn_Release.Text = "Release";
            this.RBtn_Release.UseVisualStyleBackColor = true;
            // 
            // btnCANCEL
            // 
            this.btnCANCEL.Location = new System.Drawing.Point(114, 76);
            this.btnCANCEL.Name = "btnCANCEL";
            this.btnCANCEL.Size = new System.Drawing.Size(75, 25);
            this.btnCANCEL.TabIndex = 17;
            this.btnCANCEL.Text = "Cancel";
            this.btnCANCEL.UseVisualStyleBackColor = true;
            this.btnCANCEL.Click += new System.EventHandler(this.btnCANCEL_Click);
            // 
            // fAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 118);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.RBtn_Scrap);
            this.Controls.Add(this.RBtn_Release);
            this.Controls.Add(this.btnCANCEL);
            this.Name = "fAlert";
            this.Text = "fAlert";
            this.Load += new System.EventHandler(this.fAlert_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton RBtn_Scrap;
        private System.Windows.Forms.RadioButton RBtn_Release;
        private System.Windows.Forms.Button btnCANCEL;
    }
}