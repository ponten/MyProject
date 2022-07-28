namespace RCPart
{
    partial class fPKSPEC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fPKSPEC));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.editFilter = new System.Windows.Forms.TextBox();
            this.Lab1 = new System.Windows.Forms.Label();
            this.grdViewData = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewData)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Control;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.editFilter);
            this.panel2.Controls.Add(this.Lab1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // editFilter
            // 
            resources.ApplyResources(this.editFilter, "editFilter");
            this.editFilter.Name = "editFilter";
            this.editFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.editFilter_KeyDown);
            // 
            // Lab1
            // 
            resources.ApplyResources(this.Lab1, "Lab1");
            this.Lab1.BackColor = System.Drawing.SystemColors.Control;
            this.Lab1.Name = "Lab1";
            // 
            // grdViewData
            // 
            this.grdViewData.AllowUserToAddRows = false;
            this.grdViewData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.grdViewData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdViewData.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.grdViewData, "grdViewData");
            this.grdViewData.MultiSelect = false;
            this.grdViewData.Name = "grdViewData";
            this.grdViewData.ReadOnly = true;
            this.grdViewData.RowTemplate.Height = 24;
            this.grdViewData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdViewData.DoubleClick += new System.EventHandler(this.button1_Click);
            // 
            // fPKSPEC
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdViewData);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fPKSPEC";
            this.Load += new System.EventHandler(this.fPKSPEC_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label Lab1;
        public System.Windows.Forms.DataGridView grdViewData;
        private System.Windows.Forms.TextBox editFilter;

    }
}