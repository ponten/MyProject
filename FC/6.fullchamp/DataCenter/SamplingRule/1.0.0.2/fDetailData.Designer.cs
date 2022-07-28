namespace CSamplingRule
{
    partial class fDetailData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fDetailData));
            this.panelControl = new System.Windows.Forms.Panel();
            this.cmbNextLevel = new System.Windows.Forms.ComboBox();
            this.labNextLevel = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtReject = new System.Windows.Forms.TextBox();
            this.txtContinuous = new System.Windows.Forms.TextBox();
            this.cmbLevel = new System.Windows.Forms.ComboBox();
            this.labSamplingName = new System.Windows.Forms.Label();
            this.labContinuous = new System.Windows.Forms.Label();
            this.labReject = new System.Windows.Forms.Label();
            this.labPass = new System.Windows.Forms.Label();
            this.labLevel = new System.Windows.Forms.Label();
            this.LabStageName = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panelControl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            resources.ApplyResources(this.panelControl, "panelControl");
            this.panelControl.BackColor = System.Drawing.Color.Transparent;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelControl.Controls.Add(this.cmbNextLevel);
            this.panelControl.Controls.Add(this.labNextLevel);
            this.panelControl.Controls.Add(this.txtPass);
            this.panelControl.Controls.Add(this.txtReject);
            this.panelControl.Controls.Add(this.txtContinuous);
            this.panelControl.Controls.Add(this.cmbLevel);
            this.panelControl.Controls.Add(this.labSamplingName);
            this.panelControl.Controls.Add(this.labContinuous);
            this.panelControl.Controls.Add(this.labReject);
            this.panelControl.Controls.Add(this.labPass);
            this.panelControl.Controls.Add(this.labLevel);
            this.panelControl.Controls.Add(this.LabStageName);
            this.panelControl.Name = "panelControl";
            // 
            // cmbNextLevel
            // 
            this.cmbNextLevel.BackColor = System.Drawing.Color.White;
            this.cmbNextLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNextLevel.FormattingEnabled = true;
            this.cmbNextLevel.Items.AddRange(new object[] {
            resources.GetString("cmbNextLevel.Items"),
            resources.GetString("cmbNextLevel.Items1"),
            resources.GetString("cmbNextLevel.Items2"),
            resources.GetString("cmbNextLevel.Items3")});
            resources.ApplyResources(this.cmbNextLevel, "cmbNextLevel");
            this.cmbNextLevel.Name = "cmbNextLevel";
            // 
            // labNextLevel
            // 
            resources.ApplyResources(this.labNextLevel, "labNextLevel");
            this.labNextLevel.Name = "labNextLevel";
            // 
            // txtPass
            // 
            this.txtPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.txtPass, "txtPass");
            this.txtPass.Name = "txtPass";
            this.txtPass.TextChanged += new System.EventHandler(this.txtPass_TextChanged);
            this.txtPass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // txtReject
            // 
            this.txtReject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.txtReject, "txtReject");
            this.txtReject.Name = "txtReject";
            this.txtReject.TextChanged += new System.EventHandler(this.txtReject_TextChanged);
            this.txtReject.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // txtContinuous
            // 
            this.txtContinuous.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.txtContinuous, "txtContinuous");
            this.txtContinuous.Name = "txtContinuous";
            this.txtContinuous.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // cmbLevel
            // 
            this.cmbLevel.BackColor = System.Drawing.Color.White;
            this.cmbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Items.AddRange(new object[] {
            resources.GetString("cmbLevel.Items"),
            resources.GetString("cmbLevel.Items1"),
            resources.GetString("cmbLevel.Items2"),
            resources.GetString("cmbLevel.Items3")});
            resources.ApplyResources(this.cmbLevel, "cmbLevel");
            this.cmbLevel.Name = "cmbLevel";
            // 
            // labSamplingName
            // 
            resources.ApplyResources(this.labSamplingName, "labSamplingName");
            this.labSamplingName.Name = "labSamplingName";
            // 
            // labContinuous
            // 
            resources.ApplyResources(this.labContinuous, "labContinuous");
            this.labContinuous.Name = "labContinuous";
            // 
            // labReject
            // 
            resources.ApplyResources(this.labReject, "labReject");
            this.labReject.Name = "labReject";
            // 
            // labPass
            // 
            resources.ApplyResources(this.labPass, "labPass");
            this.labPass.Name = "labPass";
            // 
            // labLevel
            // 
            resources.ApplyResources(this.labLevel, "labLevel");
            this.labLevel.Name = "labLevel";
            // 
            // LabStageName
            // 
            resources.ApplyResources(this.LabStageName, "LabStageName");
            this.LabStageName.BackColor = System.Drawing.Color.Transparent;
            this.LabStageName.ForeColor = System.Drawing.Color.Maroon;
            this.LabStageName.Name = "LabStageName";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // fDetailData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panel2);
            this.Name = "fDetailData";
            this.Load += new System.EventHandler(this.fData_Load);
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label LabStageName;
        private System.Windows.Forms.Label labSamplingName;
        private System.Windows.Forms.Label labContinuous;
        private System.Windows.Forms.Label labReject;
        private System.Windows.Forms.Label labPass;
        private System.Windows.Forms.Label labLevel;
        private System.Windows.Forms.ComboBox cmbLevel;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtReject;
        private System.Windows.Forms.TextBox txtContinuous;
        private System.Windows.Forms.ComboBox cmbNextLevel;
        private System.Windows.Forms.Label labNextLevel;
    }
}