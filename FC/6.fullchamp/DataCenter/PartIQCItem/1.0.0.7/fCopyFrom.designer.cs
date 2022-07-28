namespace CPartIQCItem
{
    partial class fCopyFrom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fCopyFrom));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearchProcess = new System.Windows.Forms.Button();
            this.editCopyProcess = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearchPart = new System.Windows.Forms.Button();
            this.editCopyPart = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCopyCancel = new System.Windows.Forms.Button();
            this.btnCopyOK = new System.Windows.Forms.Button();
            this.datagridMaster = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.datagridDetail = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridMaster)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSearchProcess);
            this.panel1.Controls.Add(this.editCopyProcess);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnSearchPart);
            this.panel1.Controls.Add(this.editCopyPart);
            this.panel1.Controls.Add(this.label1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnSearchProcess
            // 
            resources.ApplyResources(this.btnSearchProcess, "btnSearchProcess");
            this.btnSearchProcess.Name = "btnSearchProcess";
            this.btnSearchProcess.UseVisualStyleBackColor = true;
            this.btnSearchProcess.Click += new System.EventHandler(this.btnSearchProcess_Click);
            // 
            // editCopyProcess
            // 
            resources.ApplyResources(this.editCopyProcess, "editCopyProcess");
            this.editCopyProcess.Name = "editCopyProcess";
            this.editCopyProcess.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editCopyProcess_KeyPress);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnSearchPart
            // 
            resources.ApplyResources(this.btnSearchPart, "btnSearchPart");
            this.btnSearchPart.Name = "btnSearchPart";
            this.btnSearchPart.UseVisualStyleBackColor = true;
            this.btnSearchPart.Click += new System.EventHandler(this.btnSearchPart_Click);
            // 
            // editCopyPart
            // 
            resources.ApplyResources(this.editCopyPart, "editCopyPart");
            this.editCopyPart.Name = "editCopyPart";
            this.editCopyPart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editCopyPart_KeyPress);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCopyCancel);
            this.panel2.Controls.Add(this.btnCopyOK);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnCopyCancel
            // 
            this.btnCopyCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCopyCancel, "btnCopyCancel");
            this.btnCopyCancel.Name = "btnCopyCancel";
            this.btnCopyCancel.UseVisualStyleBackColor = true;
            // 
            // btnCopyOK
            // 
            resources.ApplyResources(this.btnCopyOK, "btnCopyOK");
            this.btnCopyOK.Name = "btnCopyOK";
            this.btnCopyOK.UseVisualStyleBackColor = true;
            this.btnCopyOK.Click += new System.EventHandler(this.btnCopyOK_Click);
            // 
            // datagridMaster
            // 
            this.datagridMaster.AllowUserToAddRows = false;
            this.datagridMaster.AllowUserToDeleteRows = false;
            this.datagridMaster.BackgroundColor = System.Drawing.Color.White;
            this.datagridMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.datagridMaster, "datagridMaster");
            this.datagridMaster.MultiSelect = false;
            this.datagridMaster.Name = "datagridMaster";
            this.datagridMaster.ReadOnly = true;
            this.datagridMaster.RowTemplate.Height = 24;
            this.datagridMaster.SelectionChanged += new System.EventHandler(this.datagridMaster_SelectionChanged);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.datagridMaster);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.datagridDetail);
            // 
            // datagridDetail
            // 
            this.datagridDetail.AllowUserToAddRows = false;
            this.datagridDetail.AllowUserToDeleteRows = false;
            this.datagridDetail.BackgroundColor = System.Drawing.Color.White;
            this.datagridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.datagridDetail, "datagridDetail");
            this.datagridDetail.MultiSelect = false;
            this.datagridDetail.Name = "datagridDetail";
            this.datagridDetail.ReadOnly = true;
            this.datagridDetail.RowTemplate.Height = 24;
            // 
            // fCopyFrom
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fCopyFrom";
            this.Load += new System.EventHandler(this.fCopyFrom_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagridMaster)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagridDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearchPart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearchProcess;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView datagridMaster;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView datagridDetail;
        private System.Windows.Forms.Button btnCopyCancel;
        private System.Windows.Forms.Button btnCopyOK;
        public System.Windows.Forms.TextBox editCopyPart;
        public System.Windows.Forms.TextBox editCopyProcess;        
    }
}