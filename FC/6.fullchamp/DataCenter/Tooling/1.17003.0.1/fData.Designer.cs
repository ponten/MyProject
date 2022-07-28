namespace CTooling
{
    partial class fData
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
            this.panelControl = new System.Windows.Forms.Panel();
            this.comboCompany = new System.Windows.Forms.ComboBox();
            this.labCompany = new System.Windows.Forms.Label();
            this.labEmail = new System.Windows.Forms.Label();
            this.editEmail = new System.Windows.Forms.TextBox();
            this.rbHalfYear = new System.Windows.Forms.RadioButton();
            this.rbQuarterly = new System.Windows.Forms.RadioButton();
            this.rbMonthly = new System.Windows.Forms.RadioButton();
            this.labRemind = new System.Windows.Forms.Label();
            this.combToolingType = new System.Windows.Forms.ComboBox();
            this.labToolingType = new System.Windows.Forms.Label();
            this.labToolingDesc = new System.Windows.Forms.Label();
            this.editToolingDesc = new System.Windows.Forms.TextBox();
            this.labToolingName = new System.Windows.Forms.Label();
            this.editToolingName = new System.Windows.Forms.TextBox();
            this.labelToolingNo = new System.Windows.Forms.Label();
            this.editToolingNo = new System.Windows.Forms.TextBox();
            this.labMaxUsedCnt = new System.Windows.Forms.Label();
            this.editMaxUseCnt = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.panelControl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.AutoScroll = true;
            this.panelControl.BackColor = System.Drawing.SystemColors.Control;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelControl.Controls.Add(this.comboCompany);
            this.panelControl.Controls.Add(this.labCompany);
            this.panelControl.Controls.Add(this.labEmail);
            this.panelControl.Controls.Add(this.editEmail);
            this.panelControl.Controls.Add(this.rbHalfYear);
            this.panelControl.Controls.Add(this.rbQuarterly);
            this.panelControl.Controls.Add(this.rbMonthly);
            this.panelControl.Controls.Add(this.labRemind);
            this.panelControl.Controls.Add(this.combToolingType);
            this.panelControl.Controls.Add(this.labToolingType);
            this.panelControl.Controls.Add(this.labToolingDesc);
            this.panelControl.Controls.Add(this.editToolingDesc);
            this.panelControl.Controls.Add(this.labToolingName);
            this.panelControl.Controls.Add(this.editToolingName);
            this.panelControl.Controls.Add(this.labelToolingNo);
            this.panelControl.Controls.Add(this.editToolingNo);
            this.panelControl.Controls.Add(this.labMaxUsedCnt);
            this.panelControl.Controls.Add(this.editMaxUseCnt);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl.Location = new System.Drawing.Point(0, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(430, 299);
            this.panelControl.TabIndex = 22;
            // 
            // comboCompany
            // 
            this.comboCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCompany.FormattingEnabled = true;
            this.comboCompany.Items.AddRange(new object[] {
            "事欣",
            "富士亨"});
            this.comboCompany.Location = new System.Drawing.Point(157, 200);
            this.comboCompany.Name = "comboCompany";
            this.comboCompany.Size = new System.Drawing.Size(172, 20);
            this.comboCompany.TabIndex = 36;
            this.comboCompany.Visible = false;
            this.comboCompany.SelectedIndexChanged += new System.EventHandler(this.comboCompany_SelectedIndexChanged);
            // 
            // labCompany
            // 
            this.labCompany.AutoSize = true;
            this.labCompany.Location = new System.Drawing.Point(62, 203);
            this.labCompany.Name = "labCompany";
            this.labCompany.Size = new System.Drawing.Size(51, 12);
            this.labCompany.TabIndex = 35;
            this.labCompany.Text = "Company";
            this.labCompany.Visible = false;
            // 
            // labEmail
            // 
            this.labEmail.AutoSize = true;
            this.labEmail.Location = new System.Drawing.Point(62, 175);
            this.labEmail.Name = "labEmail";
            this.labEmail.Size = new System.Drawing.Size(37, 12);
            this.labEmail.TabIndex = 34;
            this.labEmail.Text = "E-Mail";
            this.labEmail.Visible = false;
            // 
            // editEmail
            // 
            this.editEmail.BackColor = System.Drawing.Color.White;
            this.editEmail.Location = new System.Drawing.Point(157, 172);
            this.editEmail.Name = "editEmail";
            this.editEmail.Size = new System.Drawing.Size(172, 22);
            this.editEmail.TabIndex = 33;
            this.editEmail.Visible = false;
            this.editEmail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editEmail_KeyPress);
            // 
            // rbHalfYear
            // 
            this.rbHalfYear.AutoSize = true;
            this.rbHalfYear.Location = new System.Drawing.Point(291, 233);
            this.rbHalfYear.Name = "rbHalfYear";
            this.rbHalfYear.Size = new System.Drawing.Size(68, 16);
            this.rbHalfYear.TabIndex = 32;
            this.rbHalfYear.Text = "Half Year";
            this.rbHalfYear.UseVisualStyleBackColor = true;
            this.rbHalfYear.Visible = false;
            this.rbHalfYear.CheckedChanged += new System.EventHandler(this.rbMonthly_CheckedChanged);
            // 
            // rbQuarterly
            // 
            this.rbQuarterly.AutoSize = true;
            this.rbQuarterly.Location = new System.Drawing.Point(220, 233);
            this.rbQuarterly.Name = "rbQuarterly";
            this.rbQuarterly.Size = new System.Drawing.Size(67, 16);
            this.rbQuarterly.TabIndex = 31;
            this.rbQuarterly.Text = "Quarterly";
            this.rbQuarterly.UseVisualStyleBackColor = true;
            this.rbQuarterly.Visible = false;
            this.rbQuarterly.CheckedChanged += new System.EventHandler(this.rbMonthly_CheckedChanged);
            // 
            // rbMonthly
            // 
            this.rbMonthly.AutoSize = true;
            this.rbMonthly.Checked = true;
            this.rbMonthly.Location = new System.Drawing.Point(157, 233);
            this.rbMonthly.Name = "rbMonthly";
            this.rbMonthly.Size = new System.Drawing.Size(63, 16);
            this.rbMonthly.TabIndex = 30;
            this.rbMonthly.TabStop = true;
            this.rbMonthly.Text = "Monthly";
            this.rbMonthly.UseVisualStyleBackColor = true;
            this.rbMonthly.Visible = false;
            this.rbMonthly.CheckedChanged += new System.EventHandler(this.rbMonthly_CheckedChanged);
            // 
            // labRemind
            // 
            this.labRemind.AutoSize = true;
            this.labRemind.Location = new System.Drawing.Point(62, 235);
            this.labRemind.Name = "labRemind";
            this.labRemind.Size = new System.Drawing.Size(62, 12);
            this.labRemind.TabIndex = 28;
            this.labRemind.Text = "MT Remind";
            this.labRemind.Visible = false;
            // 
            // combToolingType
            // 
            this.combToolingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combToolingType.FormattingEnabled = true;
            this.combToolingType.Items.AddRange(new object[] {
            "Knife",
            "Tooling"});
            this.combToolingType.Location = new System.Drawing.Point(157, 30);
            this.combToolingType.Name = "combToolingType";
            this.combToolingType.Size = new System.Drawing.Size(172, 20);
            this.combToolingType.TabIndex = 26;
            // 
            // labToolingType
            // 
            this.labToolingType.AutoSize = true;
            this.labToolingType.Location = new System.Drawing.Point(62, 33);
            this.labToolingType.Name = "labToolingType";
            this.labToolingType.Size = new System.Drawing.Size(69, 12);
            this.labToolingType.TabIndex = 25;
            this.labToolingType.Text = "Tooling Type";
            // 
            // labToolingDesc
            // 
            this.labToolingDesc.AutoSize = true;
            this.labToolingDesc.Location = new System.Drawing.Point(62, 119);
            this.labToolingDesc.Name = "labToolingDesc";
            this.labToolingDesc.Size = new System.Drawing.Size(58, 12);
            this.labToolingDesc.TabIndex = 23;
            this.labToolingDesc.Text = "Description";
            // 
            // editToolingDesc
            // 
            this.editToolingDesc.BackColor = System.Drawing.Color.White;
            this.editToolingDesc.Location = new System.Drawing.Point(157, 116);
            this.editToolingDesc.Name = "editToolingDesc";
            this.editToolingDesc.Size = new System.Drawing.Size(172, 22);
            this.editToolingDesc.TabIndex = 24;
            // 
            // labToolingName
            // 
            this.labToolingName.AutoSize = true;
            this.labToolingName.Location = new System.Drawing.Point(62, 91);
            this.labToolingName.Name = "labToolingName";
            this.labToolingName.Size = new System.Drawing.Size(72, 12);
            this.labToolingName.TabIndex = 21;
            this.labToolingName.Text = "Tooling Name";
            // 
            // editToolingName
            // 
            this.editToolingName.BackColor = System.Drawing.Color.White;
            this.editToolingName.Location = new System.Drawing.Point(157, 88);
            this.editToolingName.Name = "editToolingName";
            this.editToolingName.Size = new System.Drawing.Size(172, 22);
            this.editToolingName.TabIndex = 22;
            // 
            // labelToolingNo
            // 
            this.labelToolingNo.AutoSize = true;
            this.labelToolingNo.Location = new System.Drawing.Point(62, 63);
            this.labelToolingNo.Name = "labelToolingNo";
            this.labelToolingNo.Size = new System.Drawing.Size(59, 12);
            this.labelToolingNo.TabIndex = 0;
            this.labelToolingNo.Text = "Tooling No";
            // 
            // editToolingNo
            // 
            this.editToolingNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.editToolingNo.Location = new System.Drawing.Point(157, 60);
            this.editToolingNo.Name = "editToolingNo";
            this.editToolingNo.Size = new System.Drawing.Size(172, 22);
            this.editToolingNo.TabIndex = 6;
            // 
            // labMaxUsedCnt
            // 
            this.labMaxUsedCnt.AutoSize = true;
            this.labMaxUsedCnt.Location = new System.Drawing.Point(62, 147);
            this.labMaxUsedCnt.Name = "labMaxUsedCnt";
            this.labMaxUsedCnt.Size = new System.Drawing.Size(84, 12);
            this.labMaxUsedCnt.TabIndex = 18;
            this.labMaxUsedCnt.Text = "Max Used Count";
            // 
            // editMaxUseCnt
            // 
            this.editMaxUseCnt.BackColor = System.Drawing.Color.White;
            this.editMaxUseCnt.Location = new System.Drawing.Point(157, 144);
            this.editMaxUseCnt.Name = "editMaxUseCnt";
            this.editMaxUseCnt.Size = new System.Drawing.Size(172, 22);
            this.editMaxUseCnt.TabIndex = 17;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 299);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(430, 45);
            this.panel2.TabIndex = 21;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(218, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(100, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // fData
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 344);
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panel2);
            this.Name = "fData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fData";
            this.Load += new System.EventHandler(this.fData_Load);
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Label labelToolingNo;
        private System.Windows.Forms.TextBox editToolingNo;
        private System.Windows.Forms.Label labMaxUsedCnt;
        private System.Windows.Forms.TextBox editMaxUseCnt;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;
        private System.Windows.Forms.Label labToolingName;
        private System.Windows.Forms.TextBox editToolingName;
        private System.Windows.Forms.Label labToolingDesc;
        private System.Windows.Forms.TextBox editToolingDesc;
        private System.Windows.Forms.ComboBox combToolingType;
        private System.Windows.Forms.Label labToolingType;
        private System.Windows.Forms.RadioButton rbMonthly;
        private System.Windows.Forms.Label labRemind;
        private System.Windows.Forms.Label labEmail;
        private System.Windows.Forms.TextBox editEmail;
        private System.Windows.Forms.RadioButton rbHalfYear;
        private System.Windows.Forms.RadioButton rbQuarterly;
        private System.Windows.Forms.ComboBox comboCompany;
        private System.Windows.Forms.Label labCompany;

    }
}