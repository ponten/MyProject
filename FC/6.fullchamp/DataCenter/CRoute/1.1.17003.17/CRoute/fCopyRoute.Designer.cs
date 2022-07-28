namespace CRoute
{
    partial class fCopyRoute
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
            this.label1 = new System.Windows.Forms.Label();
            this.textNewRouteName = new System.Windows.Forms.TextBox();
            this.btnsave = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.textOldRouteName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textRouteDesc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "New RouteName";
            // 
            // textNewRouteName
            // 
            this.textNewRouteName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textNewRouteName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textNewRouteName.Location = new System.Drawing.Point(95, 48);
            this.textNewRouteName.Name = "textNewRouteName";
            this.textNewRouteName.Size = new System.Drawing.Size(255, 22);
            this.textNewRouteName.TabIndex = 2;
            // 
            // btnsave
            // 
            this.btnsave.Location = new System.Drawing.Point(95, 195);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 23);
            this.btnsave.TabIndex = 4;
            this.btnsave.Text = "OK";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(275, 195);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 5;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // textOldRouteName
            // 
            this.textOldRouteName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textOldRouteName.Enabled = false;
            this.textOldRouteName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textOldRouteName.Location = new System.Drawing.Point(95, 21);
            this.textOldRouteName.Name = "textOldRouteName";
            this.textOldRouteName.ReadOnly = true;
            this.textOldRouteName.Size = new System.Drawing.Size(255, 22);
            this.textOldRouteName.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Old RouteName";
            // 
            // textRouteDesc
            // 
            this.textRouteDesc.Location = new System.Drawing.Point(95, 75);
            this.textRouteDesc.Multiline = true;
            this.textRouteDesc.Name = "textRouteDesc";
            this.textRouteDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textRouteDesc.Size = new System.Drawing.Size(257, 97);
            this.textRouteDesc.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Route Desc";
            // 
            // fCopyRoute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 230);
            this.Controls.Add(this.textRouteDesc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textOldRouteName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.textNewRouteName);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fCopyRoute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Save";
            this.Load += new System.EventHandler(this.fCopyRoute_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textNewRouteName;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.TextBox textOldRouteName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textRouteDesc;
        private System.Windows.Forms.Label label2;
    }
}