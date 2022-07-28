namespace RCProcessParam
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.combPhase = new System.Windows.Forms.ComboBox();
            this.LabTerminal = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbAlertType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.nudSeq = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeq)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.label1.Location = new System.Drawing.Point(33, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item Name";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtName.Location = new System.Drawing.Point(130, 59);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(187, 21);
            this.txtName.TabIndex = 1;
            // 
            // combPhase
            // 
            this.combPhase.BackColor = System.Drawing.Color.Khaki;
            this.combPhase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combPhase.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.combPhase.FormattingEnabled = true;
            this.combPhase.Items.AddRange(new object[] {
            "All",
            "WIP IN",
            "WIP OUT"});
            this.combPhase.Location = new System.Drawing.Point(130, 86);
            this.combPhase.Name = "combPhase";
            this.combPhase.Size = new System.Drawing.Size(187, 24);
            this.combPhase.TabIndex = 19;
            // 
            // LabTerminal
            // 
            this.LabTerminal.AutoSize = true;
            this.LabTerminal.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.LabTerminal.ForeColor = System.Drawing.Color.Black;
            this.LabTerminal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabTerminal.Location = new System.Drawing.Point(32, 89);
            this.LabTerminal.Name = "LabTerminal";
            this.LabTerminal.Size = new System.Drawing.Size(76, 16);
            this.LabTerminal.TabIndex = 18;
            this.LabTerminal.Text = "Item Phase";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(32, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "Alert Type";
            // 
            // cbAlertType
            // 
            this.cbAlertType.BackColor = System.Drawing.Color.Khaki;
            this.cbAlertType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAlertType.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.cbAlertType.FormattingEnabled = true;
            this.cbAlertType.Items.AddRange(new object[] {
            "Info",
            "Warn",
            "Error"});
            this.cbAlertType.Location = new System.Drawing.Point(130, 116);
            this.cbAlertType.Name = "cbAlertType";
            this.cbAlertType.Size = new System.Drawing.Size(187, 24);
            this.cbAlertType.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.label3.Location = new System.Drawing.Point(34, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtTitle.Location = new System.Drawing.Point(130, 162);
            this.txtTitle.Multiline = true;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTitle.Size = new System.Drawing.Size(293, 44);
            this.txtTitle.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.label4.Location = new System.Drawing.Point(34, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 16);
            this.label4.TabIndex = 22;
            this.label4.Text = "Content";
            // 
            // txtContent
            // 
            this.txtContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtContent.Location = new System.Drawing.Point(130, 212);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtContent.Size = new System.Drawing.Size(294, 106);
            this.txtContent.TabIndex = 23;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(131, 324);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 24;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(242, 324);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // nudSeq
            // 
            this.nudSeq.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.nudSeq.Location = new System.Drawing.Point(129, 17);
            this.nudSeq.Name = "nudSeq";
            this.nudSeq.Size = new System.Drawing.Size(60, 27);
            this.nudSeq.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("PMingLiU", 12F);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(34, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 16);
            this.label5.TabIndex = 25;
            this.label5.Text = "Item Seq";
            // 
            // fAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(436, 354);
            this.Controls.Add(this.nudSeq);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbAlertType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.combPhase);
            this.Controls.Add(this.LabTerminal);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "fAlert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alert";
            this.Load += new System.EventHandler(this.fAlert_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudSeq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox combPhase;
        private System.Windows.Forms.Label LabTerminal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbAlertType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown nudSeq;
        private System.Windows.Forms.Label label5;
    }
}