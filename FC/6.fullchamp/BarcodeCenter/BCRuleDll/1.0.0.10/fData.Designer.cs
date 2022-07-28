namespace BCRuleDll
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.editUSeqCode = new System.Windows.Forms.TextBox();
            this.editUSeq = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSeq = new System.Windows.Forms.TabPage();
            this.tabPageFun = new System.Windows.Forms.TabPage();
            this.combField = new System.Windows.Forms.ComboBox();
            this.combFunction = new System.Windows.Forms.ComboBox();
            this.editFunCode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPageSeq.SuspendLayout();
            this.tabPageFun.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
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
            // editUSeqCode
            // 
            resources.ApplyResources(this.editUSeqCode, "editUSeqCode");
            this.editUSeqCode.Name = "editUSeqCode";
            // 
            // editUSeq
            // 
            resources.ApplyResources(this.editUSeq, "editUSeq");
            this.editUSeq.Name = "editUSeq";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSeq);
            this.tabControl1.Controls.Add(this.tabPageFun);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPageSeq
            // 
            this.tabPageSeq.BackColor = System.Drawing.Color.Transparent;
            this.tabPageSeq.Controls.Add(this.editUSeqCode);
            this.tabPageSeq.Controls.Add(this.editUSeq);
            this.tabPageSeq.Controls.Add(this.label1);
            this.tabPageSeq.Controls.Add(this.label2);
            resources.ApplyResources(this.tabPageSeq, "tabPageSeq");
            this.tabPageSeq.Name = "tabPageSeq";
            // 
            // tabPageFun
            // 
            this.tabPageFun.BackColor = System.Drawing.Color.Transparent;
            this.tabPageFun.Controls.Add(this.combField);
            this.tabPageFun.Controls.Add(this.combFunction);
            this.tabPageFun.Controls.Add(this.editFunCode);
            this.tabPageFun.Controls.Add(this.label8);
            this.tabPageFun.Controls.Add(this.label7);
            this.tabPageFun.Controls.Add(this.label6);
            resources.ApplyResources(this.tabPageFun, "tabPageFun");
            this.tabPageFun.Name = "tabPageFun";
            // 
            // combField
            // 
            this.combField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combField.FormattingEnabled = true;
            resources.ApplyResources(this.combField, "combField");
            this.combField.Name = "combField";
            this.combField.SelectedIndexChanged += new System.EventHandler(this.combField_SelectedIndexChanged);
            // 
            // combFunction
            // 
            this.combFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combFunction.FormattingEnabled = true;
            resources.ApplyResources(this.combFunction, "combFunction");
            this.combFunction.Name = "combFunction";
            // 
            // editFunCode
            // 
            resources.ApplyResources(this.editFunCode, "editFunCode");
            this.editFunCode.Name = "editFunCode";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // fData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "fData";
            this.Load += new System.EventHandler(this.fData_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSeq.ResumeLayout(false);
            this.tabPageSeq.PerformLayout();
            this.tabPageFun.ResumeLayout(false);
            this.tabPageFun.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox editUSeqCode;
        public System.Windows.Forms.TextBox editUSeq;
        private System.Windows.Forms.TabControl tabControl1;        
        private System.Windows.Forms.Panel panel1;        
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox editFunCode;
        public System.Windows.Forms.ComboBox combFunction;
        public System.Windows.Forms.ComboBox combField;
        private System.Windows.Forms.TabPage tabPageSeq;
        private System.Windows.Forms.TabPage tabPageFun;
    }
}