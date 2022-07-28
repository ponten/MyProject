namespace CSamplingPlan
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
            this.LabLevel = new System.Windows.Forms.Label();
            this.txtMinor = new System.Windows.Forms.TextBox();
            this.txtMajor = new System.Windows.Forms.TextBox();
            this.txtCritical = new System.Windows.Forms.TextBox();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.cmbLevel = new System.Windows.Forms.ComboBox();
            this.labSamplingName = new System.Windows.Forms.Label();
            this.MIN_LOT_SIZELabel = new System.Windows.Forms.Label();
            this.labLotSize = new System.Windows.Forms.Label();
            this.labSampleSize = new System.Windows.Forms.Label();
            this.labCritical = new System.Windows.Forms.Label();
            this.labMajor = new System.Windows.Forms.Label();
            this.labMinor = new System.Windows.Forms.Label();
            this.labSamplingLevel = new System.Windows.Forms.Label();
            this.cmbUnit = new System.Windows.Forms.ComboBox();
            this.txtMinSize = new System.Windows.Forms.TextBox();
            this.txtMaxSize = new System.Windows.Forms.TextBox();
            this.LabPlanName = new System.Windows.Forms.Label();
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
            this.panelControl.Controls.Add(this.LabLevel);
            this.panelControl.Controls.Add(this.txtMinor);
            this.panelControl.Controls.Add(this.txtMajor);
            this.panelControl.Controls.Add(this.txtCritical);
            this.panelControl.Controls.Add(this.txtSize);
            this.panelControl.Controls.Add(this.cmbLevel);
            this.panelControl.Controls.Add(this.labSamplingName);
            this.panelControl.Controls.Add(this.MIN_LOT_SIZELabel);
            this.panelControl.Controls.Add(this.labLotSize);
            this.panelControl.Controls.Add(this.labSampleSize);
            this.panelControl.Controls.Add(this.labCritical);
            this.panelControl.Controls.Add(this.labMajor);
            this.panelControl.Controls.Add(this.labMinor);
            this.panelControl.Controls.Add(this.labSamplingLevel);
            this.panelControl.Controls.Add(this.cmbUnit);
            this.panelControl.Controls.Add(this.txtMinSize);
            this.panelControl.Controls.Add(this.txtMaxSize);
            this.panelControl.Controls.Add(this.LabPlanName);
            this.panelControl.Name = "panelControl";
            // 
            // LabLevel
            // 
            resources.ApplyResources(this.LabLevel, "LabLevel");
            this.LabLevel.BackColor = System.Drawing.Color.Transparent;
            this.LabLevel.ForeColor = System.Drawing.Color.Maroon;
            this.LabLevel.Name = "LabLevel";
            // 
            // txtMinor
            // 
            this.txtMinor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.txtMinor, "txtMinor");
            this.txtMinor.Name = "txtMinor";
            this.txtMinor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // txtMajor
            // 
            this.txtMajor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.txtMajor, "txtMajor");
            this.txtMajor.Name = "txtMajor";
            this.txtMajor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // txtCritical
            // 
            this.txtCritical.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.txtCritical, "txtCritical");
            this.txtCritical.Name = "txtCritical";
            this.txtCritical.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // txtSize
            // 
            this.txtSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.txtSize, "txtSize");
            this.txtSize.Name = "txtSize";
            this.txtSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // cmbLevel
            // 
            this.cmbLevel.BackColor = System.Drawing.Color.White;
            this.cmbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbLevel, "cmbLevel");
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Items.AddRange(new object[] {
            resources.GetString("cmbLevel.Items"),
            resources.GetString("cmbLevel.Items1"),
            resources.GetString("cmbLevel.Items2"),
            resources.GetString("cmbLevel.Items3")});
            this.cmbLevel.Name = "cmbLevel";
            // 
            // labSamplingName
            // 
            resources.ApplyResources(this.labSamplingName, "labSamplingName");
            this.labSamplingName.Name = "labSamplingName";
            // 
            // MIN_LOT_SIZELabel
            // 
            resources.ApplyResources(this.MIN_LOT_SIZELabel, "MIN_LOT_SIZELabel");
            this.MIN_LOT_SIZELabel.Name = "MIN_LOT_SIZELabel";
            // 
            // labLotSize
            // 
            resources.ApplyResources(this.labLotSize, "labLotSize");
            this.labLotSize.Name = "labLotSize";
            // 
            // labSampleSize
            // 
            resources.ApplyResources(this.labSampleSize, "labSampleSize");
            this.labSampleSize.Name = "labSampleSize";
            // 
            // labCritical
            // 
            resources.ApplyResources(this.labCritical, "labCritical");
            this.labCritical.Name = "labCritical";
            // 
            // labMajor
            // 
            resources.ApplyResources(this.labMajor, "labMajor");
            this.labMajor.Name = "labMajor";
            // 
            // labMinor
            // 
            resources.ApplyResources(this.labMinor, "labMinor");
            this.labMinor.Name = "labMinor";
            // 
            // labSamplingLevel
            // 
            resources.ApplyResources(this.labSamplingLevel, "labSamplingLevel");
            this.labSamplingLevel.Name = "labSamplingLevel";
            // 
            // cmbUnit
            // 
            this.cmbUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cmbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnit.Items.AddRange(new object[] {
            resources.GetString("cmbUnit.Items"),
            resources.GetString("cmbUnit.Items1")});
            resources.ApplyResources(this.cmbUnit, "cmbUnit");
            this.cmbUnit.Name = "cmbUnit";
            // 
            // txtMinSize
            // 
            this.txtMinSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.txtMinSize, "txtMinSize");
            this.txtMinSize.Name = "txtMinSize";
            this.txtMinSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // txtMaxSize
            // 
            this.txtMaxSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.txtMaxSize, "txtMaxSize");
            this.txtMaxSize.Name = "txtMaxSize";
            this.txtMaxSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // LabPlanName
            // 
            resources.ApplyResources(this.LabPlanName, "LabPlanName");
            this.LabPlanName.BackColor = System.Drawing.Color.Transparent;
            this.LabPlanName.ForeColor = System.Drawing.Color.Maroon;
            this.LabPlanName.Name = "LabPlanName";
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
        private System.Windows.Forms.Label LabPlanName;
        private System.Windows.Forms.Label labSamplingName;
        private System.Windows.Forms.Label MIN_LOT_SIZELabel;
        private System.Windows.Forms.Label labLotSize;
        private System.Windows.Forms.Label labSampleSize;
        private System.Windows.Forms.Label labCritical;
        private System.Windows.Forms.Label labMajor;
        private System.Windows.Forms.Label labMinor;
        private System.Windows.Forms.Label labSamplingLevel;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.TextBox txtMinSize;
        private System.Windows.Forms.TextBox txtMaxSize;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.ComboBox cmbLevel;
        private System.Windows.Forms.TextBox txtMinor;
        private System.Windows.Forms.TextBox txtMajor;
        private System.Windows.Forms.TextBox txtCritical;
        private System.Windows.Forms.Label LabLevel;
    }
}