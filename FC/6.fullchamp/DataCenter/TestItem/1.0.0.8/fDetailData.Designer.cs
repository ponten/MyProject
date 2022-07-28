namespace CTestItem
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
            this.editInspQty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.editDesc2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gbValueType = new System.Windows.Forms.GroupBox();
            this.rbtnCharacter = new System.Windows.Forms.RadioButton();
            this.rbtnNumber = new System.Windows.Forms.RadioButton();
            this.combHasValue = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LabTypeName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.editCode = new System.Windows.Forms.TextBox();
            this.LabCode = new System.Windows.Forms.Label();
            this.editDesc = new System.Windows.Forms.TextBox();
            this.editName = new System.Windows.Forms.TextBox();
            this.LabName = new System.Windows.Forms.Label();
            this.LabDesc = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.maxItemCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panelControl.SuspendLayout();
            this.gbValueType.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            resources.ApplyResources(this.panelControl, "panelControl");
            this.panelControl.BackColor = System.Drawing.Color.Transparent;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelControl.Controls.Add(this.maxItemCode);
            this.panelControl.Controls.Add(this.label5);
            this.panelControl.Controls.Add(this.editInspQty);
            this.panelControl.Controls.Add(this.label4);
            this.panelControl.Controls.Add(this.editDesc2);
            this.panelControl.Controls.Add(this.label3);
            this.panelControl.Controls.Add(this.gbValueType);
            this.panelControl.Controls.Add(this.combHasValue);
            this.panelControl.Controls.Add(this.label2);
            this.panelControl.Controls.Add(this.LabTypeName);
            this.panelControl.Controls.Add(this.label1);
            this.panelControl.Controls.Add(this.editCode);
            this.panelControl.Controls.Add(this.LabCode);
            this.panelControl.Controls.Add(this.editDesc);
            this.panelControl.Controls.Add(this.editName);
            this.panelControl.Controls.Add(this.LabName);
            this.panelControl.Controls.Add(this.LabDesc);
            this.panelControl.Name = "panelControl";
            // 
            // editInspQty
            // 
            resources.ApplyResources(this.editInspQty, "editInspQty");
            this.editInspQty.Name = "editInspQty";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // editDesc2
            // 
            resources.ApplyResources(this.editDesc2, "editDesc2");
            this.editDesc2.Name = "editDesc2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // gbValueType
            // 
            this.gbValueType.Controls.Add(this.rbtnCharacter);
            this.gbValueType.Controls.Add(this.rbtnNumber);
            resources.ApplyResources(this.gbValueType, "gbValueType");
            this.gbValueType.Name = "gbValueType";
            this.gbValueType.TabStop = false;
            // 
            // rbtnCharacter
            // 
            resources.ApplyResources(this.rbtnCharacter, "rbtnCharacter");
            this.rbtnCharacter.Name = "rbtnCharacter";
            this.rbtnCharacter.TabStop = true;
            this.rbtnCharacter.UseVisualStyleBackColor = true;
            // 
            // rbtnNumber
            // 
            resources.ApplyResources(this.rbtnNumber, "rbtnNumber");
            this.rbtnNumber.Name = "rbtnNumber";
            this.rbtnNumber.TabStop = true;
            this.rbtnNumber.UseVisualStyleBackColor = true;
            // 
            // combHasValue
            // 
            this.combHasValue.BackColor = System.Drawing.Color.White;
            this.combHasValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combHasValue.FormattingEnabled = true;
            this.combHasValue.Items.AddRange(new object[] {
            resources.GetString("combHasValue.Items"),
            resources.GetString("combHasValue.Items1")});
            resources.ApplyResources(this.combHasValue, "combHasValue");
            this.combHasValue.Name = "combHasValue";
            this.combHasValue.SelectedIndexChanged += new System.EventHandler(this.combHasValue_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // LabTypeName
            // 
            resources.ApplyResources(this.LabTypeName, "LabTypeName");
            this.LabTypeName.BackColor = System.Drawing.Color.Transparent;
            this.LabTypeName.ForeColor = System.Drawing.Color.Maroon;
            this.LabTypeName.Name = "LabTypeName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
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
            // editDesc
            // 
            resources.ApplyResources(this.editDesc, "editDesc");
            this.editDesc.Name = "editDesc";
            // 
            // editName
            // 
            this.editName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editName, "editName");
            this.editName.Name = "editName";
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
            // maxItemCode
            // 
            this.maxItemCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.maxItemCode, "maxItemCode");
            this.maxItemCode.Name = "maxItemCode";
            this.maxItemCode.ReadOnly = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Name = "label5";
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
            this.gbValueType.ResumeLayout(false);
            this.gbValueType.PerformLayout();
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
        private System.Windows.Forms.TextBox editDesc;
        private System.Windows.Forms.Label LabDesc;
        private System.Windows.Forms.TextBox editCode;
        private System.Windows.Forms.Label LabCode;
        private System.Windows.Forms.Label LabTypeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox combHasValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbValueType;
        private System.Windows.Forms.RadioButton rbtnCharacter;
        private System.Windows.Forms.RadioButton rbtnNumber;
        private System.Windows.Forms.TextBox editDesc2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox editInspQty;
        private System.Windows.Forms.TextBox maxItemCode;
        private System.Windows.Forms.Label label5;
    }
}