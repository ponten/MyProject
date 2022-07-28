namespace CPartIQCItem
{
    partial class fTypeData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fTypeData));
            this.panelControl = new System.Windows.Forms.Panel();
            this.LabTypeName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.combSamplingPlan = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.editName = new System.Windows.Forms.TextBox();
            this.LabName = new System.Windows.Forms.Label();
            this.LabDesc = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.combSamplingLevel = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelControl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            resources.ApplyResources(this.panelControl, "panelControl");
            this.panelControl.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelControl.Controls.Add(this.combSamplingLevel);
            this.panelControl.Controls.Add(this.label2);
            this.panelControl.Controls.Add(this.LabTypeName);
            this.panelControl.Controls.Add(this.label1);
            this.panelControl.Controls.Add(this.combSamplingPlan);
            this.panelControl.Controls.Add(this.btnSearch);
            this.panelControl.Controls.Add(this.editName);
            this.panelControl.Controls.Add(this.LabName);
            this.panelControl.Controls.Add(this.LabDesc);
            this.panelControl.Name = "panelControl";
            // 
            // LabTypeName
            // 
            resources.ApplyResources(this.LabTypeName, "LabTypeName");
            this.LabTypeName.BackColor = System.Drawing.Color.Transparent;
            this.LabTypeName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabTypeName.Name = "LabTypeName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // combSamplingPlan
            // 
            this.combSamplingPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combSamplingPlan.FormattingEnabled = true;
            resources.ApplyResources(this.combSamplingPlan, "combSamplingPlan");
            this.combSamplingPlan.Name = "combSamplingPlan";
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // editName
            // 
            this.editName.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.editName, "editName");
            this.editName.Name = "editName";
            this.editName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editName_KeyPress);
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
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // combSamplingLevel
            // 
            this.combSamplingLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combSamplingLevel.FormattingEnabled = true;
            resources.ApplyResources(this.combSamplingLevel, "combSamplingLevel");
            this.combSamplingLevel.Name = "combSamplingLevel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // fTypeData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panel2);
            this.Name = "fTypeData";
            this.Load += new System.EventHandler(this.fTypeData_Load);
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
        private System.Windows.Forms.TextBox editName;
        private System.Windows.Forms.Label LabName;
        private System.Windows.Forms.Label LabDesc;
        private System.Windows.Forms.ComboBox combSamplingPlan;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label LabTypeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox combSamplingLevel;
        private System.Windows.Forms.Label label2;
    }
}