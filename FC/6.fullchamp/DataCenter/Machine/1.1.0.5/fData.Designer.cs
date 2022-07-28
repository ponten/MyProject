namespace CMachine
{
    partial class fData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fData));
            this.panelControl = new System.Windows.Forms.Panel();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtWarrantyDate = new System.Windows.Forms.DateTimePicker();
            this.dtArrivalDate = new System.Windows.Forms.DateTimePicker();
            this.labWarranty = new System.Windows.Forms.Label();
            this.labArrival = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.editVendor = new System.Windows.Forms.TextBox();
            this.labVendor = new System.Windows.Forms.Label();
            this.chkUtilization = new System.Windows.Forms.CheckBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.labType = new System.Windows.Forms.Label();
            this.cmbLoc = new System.Windows.Forms.ComboBox();
            this.labLine = new System.Windows.Forms.Label();
            this.editDesc = new System.Windows.Forms.TextBox();
            this.editCode = new System.Windows.Forms.TextBox();
            this.LabCode = new System.Windows.Forms.Label();
            this.LabDesc = new System.Windows.Forms.Label();
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
            this.panelControl.Controls.Add(this.cmbStatus);
            this.panelControl.Controls.Add(this.label1);
            this.panelControl.Controls.Add(this.dtWarrantyDate);
            this.panelControl.Controls.Add(this.dtArrivalDate);
            this.panelControl.Controls.Add(this.labWarranty);
            this.panelControl.Controls.Add(this.labArrival);
            this.panelControl.Controls.Add(this.btnSearch);
            this.panelControl.Controls.Add(this.editVendor);
            this.panelControl.Controls.Add(this.labVendor);
            this.panelControl.Controls.Add(this.chkUtilization);
            this.panelControl.Controls.Add(this.cmbType);
            this.panelControl.Controls.Add(this.labType);
            this.panelControl.Controls.Add(this.cmbLoc);
            this.panelControl.Controls.Add(this.labLine);
            this.panelControl.Controls.Add(this.editDesc);
            this.panelControl.Controls.Add(this.editCode);
            this.panelControl.Controls.Add(this.LabCode);
            this.panelControl.Controls.Add(this.LabDesc);
            this.panelControl.Name = "panelControl";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            resources.GetString("cmbStatus.Items"),
            resources.GetString("cmbStatus.Items1")});
            resources.ApplyResources(this.cmbStatus, "cmbStatus");
            this.cmbStatus.Name = "cmbStatus";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // dtWarrantyDate
            // 
            this.dtWarrantyDate.Checked = false;
            resources.ApplyResources(this.dtWarrantyDate, "dtWarrantyDate");
            this.dtWarrantyDate.Name = "dtWarrantyDate";
            this.dtWarrantyDate.ShowCheckBox = true;
            this.dtWarrantyDate.Value = new System.DateTime(2015, 4, 7, 0, 0, 0, 0);
            // 
            // dtArrivalDate
            // 
            this.dtArrivalDate.Checked = false;
            resources.ApplyResources(this.dtArrivalDate, "dtArrivalDate");
            this.dtArrivalDate.Name = "dtArrivalDate";
            this.dtArrivalDate.ShowCheckBox = true;
            this.dtArrivalDate.Value = new System.DateTime(2015, 4, 7, 0, 0, 0, 0);
            // 
            // labWarranty
            // 
            resources.ApplyResources(this.labWarranty, "labWarranty");
            this.labWarranty.Name = "labWarranty";
            // 
            // labArrival
            // 
            resources.ApplyResources(this.labArrival, "labArrival");
            this.labArrival.Name = "labArrival";
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // editVendor
            // 
            this.editVendor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.editVendor, "editVendor");
            this.editVendor.Name = "editVendor";
            this.editVendor.ReadOnly = true;
            // 
            // labVendor
            // 
            resources.ApplyResources(this.labVendor, "labVendor");
            this.labVendor.Name = "labVendor";
            // 
            // chkUtilization
            // 
            resources.ApplyResources(this.chkUtilization, "chkUtilization");
            this.chkUtilization.Name = "chkUtilization";
            this.chkUtilization.UseVisualStyleBackColor = true;
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            // 
            // labType
            // 
            resources.ApplyResources(this.labType, "labType");
            this.labType.Name = "labType";
            // 
            // cmbLoc
            // 
            this.cmbLoc.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLoc, "cmbLoc");
            this.cmbLoc.Name = "cmbLoc";
            // 
            // labLine
            // 
            resources.ApplyResources(this.labLine, "labLine");
            this.labLine.Name = "labLine";
            // 
            // editDesc
            // 
            resources.ApplyResources(this.editDesc, "editDesc");
            this.editDesc.Name = "editDesc";
            // 
            // editCode
            // 
            this.editCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editCode, "editCode");
            this.editCode.Name = "editCode";
            // 
            // LabCode
            // 
            resources.ApplyResources(this.LabCode, "LabCode");
            this.LabCode.BackColor = System.Drawing.Color.Transparent;
            this.LabCode.Name = "LabCode";
            // 
            // LabDesc
            // 
            resources.ApplyResources(this.LabDesc, "LabDesc");
            this.LabDesc.BackColor = System.Drawing.Color.Transparent;
            this.LabDesc.Name = "LabDesc";
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
            // fData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panel2);
            this.Name = "fData";
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
        private System.Windows.Forms.TextBox editCode;
        private System.Windows.Forms.Label LabCode;
        private System.Windows.Forms.TextBox editDesc;
        private System.Windows.Forms.Label LabDesc;
        private System.Windows.Forms.CheckBox chkUtilization;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label labType;
        private System.Windows.Forms.ComboBox cmbLoc;
        private System.Windows.Forms.Label labLine;
        private System.Windows.Forms.DateTimePicker dtWarrantyDate;
        private System.Windows.Forms.DateTimePicker dtArrivalDate;
        private System.Windows.Forms.Label labWarranty;
        private System.Windows.Forms.Label labArrival;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox editVendor;
        private System.Windows.Forms.Label labVendor;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label1;
    }
}