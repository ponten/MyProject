namespace DefectTypeDll
{
    partial class fDetailData
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
            this.editName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.editEmpId = new System.Windows.Forms.TextBox();
            this.labelEmpID = new System.Windows.Forms.Label();
            this.editDesc = new System.Windows.Forms.TextBox();
            this.labelDesc = new System.Windows.Forms.Label();
            this.editCode = new System.Windows.Forms.TextBox();
            this.labelCode = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // editName
            // 
            this.editName.Enabled = false;
            this.editName.Location = new System.Drawing.Point(119, 136);
            this.editName.Name = "editName";
            this.editName.Size = new System.Drawing.Size(166, 21);
            this.editName.TabIndex = 22;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(32, 140);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(53, 12);
            this.labelName.TabIndex = 21;
            this.labelName.Text = "Emp Name";
            // 
            // editEmpId
            // 
            this.editEmpId.BackColor = System.Drawing.Color.Yellow;
            this.editEmpId.Location = new System.Drawing.Point(120, 98);
            this.editEmpId.Name = "editEmpId";
            this.editEmpId.Size = new System.Drawing.Size(166, 21);
            this.editEmpId.TabIndex = 20;
            this.editEmpId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editEmpId_KeyPress);
            // 
            // labelEmpID
            // 
            this.labelEmpID.AutoSize = true;
            this.labelEmpID.Location = new System.Drawing.Point(32, 101);
            this.labelEmpID.Name = "labelEmpID";
            this.labelEmpID.Size = new System.Drawing.Size(41, 12);
            this.labelEmpID.TabIndex = 19;
            this.labelEmpID.Text = "Emp No";
            // 
            // editDesc
            // 
            this.editDesc.BackColor = System.Drawing.SystemColors.Window;
            this.editDesc.Enabled = false;
            this.editDesc.Location = new System.Drawing.Point(120, 56);
            this.editDesc.Name = "editDesc";
            this.editDesc.Size = new System.Drawing.Size(166, 21);
            this.editDesc.TabIndex = 18;
            // 
            // labelDesc
            // 
            this.labelDesc.AutoSize = true;
            this.labelDesc.Location = new System.Drawing.Point(32, 62);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(101, 12);
            this.labelDesc.TabIndex = 17;
            this.labelDesc.Text = "Defect Type Desc";
            // 
            // editCode
            // 
            this.editCode.BackColor = System.Drawing.SystemColors.Window;
            this.editCode.Enabled = false;
            this.editCode.Location = new System.Drawing.Point(120, 14);
            this.editCode.Name = "editCode";
            this.editCode.Size = new System.Drawing.Size(167, 21);
            this.editCode.TabIndex = 16;
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Location = new System.Drawing.Point(32, 20);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(101, 12);
            this.labelCode.TabIndex = 15;
            this.labelCode.Text = "Defect Type Code";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Location = new System.Drawing.Point(0, 170);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(332, 46);
            this.panel1.TabIndex = 23;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("PMingLiU", 9.75F);
            this.btnCancel.Location = new System.Drawing.Point(210, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("PMingLiU", 9.75F);
            this.btnOK.Location = new System.Drawing.Point(123, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // fDetailData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 215);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.editName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.editEmpId);
            this.Controls.Add(this.labelEmpID);
            this.Controls.Add(this.editDesc);
            this.Controls.Add(this.labelDesc);
            this.Controls.Add(this.editCode);
            this.Controls.Add(this.labelCode);
            this.Name = "fDetailData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fDetailData";
            this.Load += new System.EventHandler(this.fDetailData_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox editName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox editEmpId;
        private System.Windows.Forms.Label labelEmpID;
        private System.Windows.Forms.TextBox editDesc;
        private System.Windows.Forms.Label labelDesc;
        private System.Windows.Forms.TextBox editCode;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}