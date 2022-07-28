namespace BCRuleDll
{
    partial class fModifyRule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fModifyRule));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.editRuleName = new System.Windows.Forms.TextBox();
            this.editRuleDesc = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.combRule = new System.Windows.Forms.ComboBox();
            this.editGroupQty = new System.Windows.Forms.TextBox();
            this.labGroupQty = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.editSafetyStock = new System.Windows.Forms.TextBox();
            this.LabSafetyStock = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // editRuleName
            // 
            this.editRuleName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.editRuleName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.editRuleName, "editRuleName");
            this.editRuleName.Name = "editRuleName";
            // 
            // editRuleDesc
            // 
            resources.ApplyResources(this.editRuleDesc, "editRuleDesc");
            this.editRuleDesc.Name = "editRuleDesc";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // combRule
            // 
            this.combRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combRule, "combRule");
            this.combRule.FormattingEnabled = true;
            this.combRule.Name = "combRule";
            // 
            // editGroupQty
            // 
            resources.ApplyResources(this.editGroupQty, "editGroupQty");
            this.editGroupQty.Name = "editGroupQty";
            this.editGroupQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editGroupQty_KeyPress);
            // 
            // labGroupQty
            // 
            resources.ApplyResources(this.labGroupQty, "labGroupQty");
            this.labGroupQty.Name = "labGroupQty";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // editSafetyStock
            // 
            resources.ApplyResources(this.editSafetyStock, "editSafetyStock");
            this.editSafetyStock.Name = "editSafetyStock";
            this.editSafetyStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editGroupQty_KeyPress);
            // 
            // LabSafetyStock
            // 
            resources.ApplyResources(this.LabSafetyStock, "LabSafetyStock");
            this.LabSafetyStock.Name = "LabSafetyStock";
            // 
            // fModifyRule
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.editSafetyStock);
            this.Controls.Add(this.LabSafetyStock);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.editGroupQty);
            this.Controls.Add(this.labGroupQty);
            this.Controls.Add(this.combRule);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.editRuleDesc);
            this.Controls.Add(this.editRuleName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "fModifyRule";
            this.Load += new System.EventHandler(this.fModifyRule_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox editRuleName;
        public System.Windows.Forms.TextBox editRuleDesc;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox combRule;
        public System.Windows.Forms.TextBox editGroupQty;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TextBox editSafetyStock;
        public System.Windows.Forms.Label labGroupQty;
        public System.Windows.Forms.Label LabSafetyStock;
    }
}