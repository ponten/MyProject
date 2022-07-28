namespace BCRuleDll
{
    partial class fDayCode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fDayCode));
            this.gvValue = new System.Windows.Forms.DataGridView();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bbtnCancel = new System.Windows.Forms.Button();
            this.bbtnOK = new System.Windows.Forms.Button();
            this.editCode = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvValue)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvValue
            // 
            this.gvValue.AllowUserToAddRows = false;
            this.gvValue.AllowUserToDeleteRows = false;
            this.gvValue.BackgroundColor = System.Drawing.Color.White;
            this.gvValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvValue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Key,
            this.Value});
            resources.ApplyResources(this.gvValue, "gvValue");
            this.gvValue.Name = "gvValue";
            this.gvValue.RowTemplate.Height = 24;
            // 
            // Key
            // 
            resources.ApplyResources(this.Key, "Key");
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            this.Key.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Value
            // 
            resources.ApplyResources(this.Value, "Value");
            this.Value.Name = "Value";
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bbtnCancel);
            this.panel1.Controls.Add(this.bbtnOK);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // bbtnCancel
            // 
            this.bbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.bbtnCancel, "bbtnCancel");
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.UseVisualStyleBackColor = true;
            // 
            // bbtnOK
            // 
            resources.ApplyResources(this.bbtnOK, "bbtnOK");
            this.bbtnOK.Name = "bbtnOK";
            this.bbtnOK.UseVisualStyleBackColor = true;
            this.bbtnOK.Click += new System.EventHandler(this.bbtnOK_Click);
            // 
            // editCode
            // 
            resources.ApplyResources(this.editCode, "editCode");
            this.editCode.Name = "editCode";
            this.editCode.ReadOnly = true;
            // 
            // fDayCode
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gvValue);
            this.Controls.Add(this.editCode);
            this.Controls.Add(this.panel1);
            this.Name = "fDayCode";
            this.Load += new System.EventHandler(this.fDefineCode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvValue)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bbtnCancel;
        private System.Windows.Forms.Button bbtnOK;
        public System.Windows.Forms.TextBox editCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridView gvValue;


    }
}