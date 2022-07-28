namespace CMachineDown
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
            this.LabType = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.editDesc2 = new System.Windows.Forms.TextBox();
            this.LabDesc2 = new System.Windows.Forms.Label();
            this.editDesc1 = new System.Windows.Forms.TextBox();
            this.editCode = new System.Windows.Forms.TextBox();
            this.LabName = new System.Windows.Forms.Label();
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
            this.panelControl.Controls.Add(this.LabType);
            this.panelControl.Controls.Add(this.label1);
            this.panelControl.Controls.Add(this.editDesc2);
            this.panelControl.Controls.Add(this.LabDesc2);
            this.panelControl.Controls.Add(this.editDesc1);
            this.panelControl.Controls.Add(this.editCode);
            this.panelControl.Controls.Add(this.LabName);
            this.panelControl.Controls.Add(this.LabDesc);
            this.panelControl.Name = "panelControl";
            // 
            // LabType
            // 
            resources.ApplyResources(this.LabType, "LabType");
            this.LabType.BackColor = System.Drawing.Color.Transparent;
            this.LabType.ForeColor = System.Drawing.Color.Maroon;
            this.LabType.Name = "LabType";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // editDesc2
            // 
            resources.ApplyResources(this.editDesc2, "editDesc2");
            this.editDesc2.Name = "editDesc2";
            // 
            // LabDesc2
            // 
            resources.ApplyResources(this.LabDesc2, "LabDesc2");
            this.LabDesc2.BackColor = System.Drawing.Color.Transparent;
            this.LabDesc2.Name = "LabDesc2";
            // 
            // editDesc1
            // 
            resources.ApplyResources(this.editDesc1, "editDesc1");
            this.editDesc1.Name = "editDesc1";
            // 
            // editCode
            // 
            this.editCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editCode, "editCode");
            this.editCode.Name = "editCode";
            // 
            // LabName
            // 
            resources.ApplyResources(this.LabName, "LabName");
            this.LabName.BackColor = System.Drawing.Color.Transparent;
            this.LabName.Name = "LabName";
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
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // fDetailData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panel2);
            this.Name = "fDetailData";
            this.Load += new System.EventHandler(this.FData_Load);
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
        private System.Windows.Forms.Label LabName;
        private System.Windows.Forms.TextBox editDesc1;
        private System.Windows.Forms.Label LabDesc;
        private System.Windows.Forms.TextBox editDesc2;
        private System.Windows.Forms.Label LabDesc2;
        private System.Windows.Forms.Label LabType;
        private System.Windows.Forms.Label label1;
    }
}