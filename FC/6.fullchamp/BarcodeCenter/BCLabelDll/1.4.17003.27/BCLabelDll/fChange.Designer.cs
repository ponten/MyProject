namespace BCLabelDll
{
    partial class fChange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fChange));
            this.button1 = new System.Windows.Forms.Button();
            this.LabDefValue = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.editDefValue = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // LabDefValue
            // 
            resources.ApplyResources(this.LabDefValue, "LabDefValue");
            this.LabDefValue.Name = "LabDefValue";
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // editDefValue
            // 
            resources.ApplyResources(this.editDefValue, "editDefValue");
            this.editDefValue.Name = "editDefValue";
            // 
            // fChange
            // 
            this.AcceptButton = this.button1;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.editDefValue);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.LabDefValue);
            this.Controls.Add(this.button1);
            this.Name = "fChange";
            this.Load += new System.EventHandler(this.fChange_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.MaskedTextBox editDefValue;
        private System.Windows.Forms.Label LabDefValue;
    }
}