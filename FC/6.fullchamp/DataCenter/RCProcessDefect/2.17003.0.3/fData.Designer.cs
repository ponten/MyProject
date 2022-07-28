namespace RCProcessDefect
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelControl = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvDefect = new System.Windows.Forms.DataGridView();
            this.CLICK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DEFECT_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEFECT_DESC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckbSelectAll = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.editProcess = new System.Windows.Forms.TextBox();
            this.LabCode = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panelControl.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            resources.ApplyResources(this.panelControl, "panelControl");
            this.panelControl.BackColor = System.Drawing.Color.Transparent;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelControl.Controls.Add(this.groupBox1);
            this.panelControl.Controls.Add(this.panel1);
            this.panelControl.Name = "panelControl";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvDefect);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // dgvDefect
            // 
            this.dgvDefect.AllowUserToAddRows = false;
            this.dgvDefect.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.dgvDefect.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDefect.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dgvDefect, "dgvDefect");
            this.dgvDefect.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CLICK,
            this.DEFECT_CODE,
            this.DEFECT_DESC});
            this.dgvDefect.GridColor = System.Drawing.Color.White;
            this.dgvDefect.MultiSelect = false;
            this.dgvDefect.Name = "dgvDefect";
            this.dgvDefect.RowTemplate.Height = 24;
            this.dgvDefect.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDefect.TabStop = false;
            this.dgvDefect.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDefect_CellContentClick);
            // 
            // CLICK
            // 
            resources.ApplyResources(this.CLICK, "CLICK");
            this.CLICK.Name = "CLICK";
            this.CLICK.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CLICK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // DEFECT_CODE
            // 
            resources.ApplyResources(this.DEFECT_CODE, "DEFECT_CODE");
            this.DEFECT_CODE.Name = "DEFECT_CODE";
            // 
            // DEFECT_DESC
            // 
            resources.ApplyResources(this.DEFECT_DESC, "DEFECT_DESC");
            this.DEFECT_DESC.Name = "DEFECT_DESC";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.ckbSelectAll);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.editProcess);
            this.panel1.Controls.Add(this.LabCode);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // ckbSelectAll
            // 
            resources.ApplyResources(this.ckbSelectAll, "ckbSelectAll");
            this.ckbSelectAll.Name = "ckbSelectAll";
            this.ckbSelectAll.UseVisualStyleBackColor = true;
            this.ckbSelectAll.CheckedChanged += new System.EventHandler(this.ckbSelectAll_CheckedChanged);
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // editProcess
            // 
            this.editProcess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editProcess, "editProcess");
            this.editProcess.Name = "editProcess";
            this.editProcess.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editProcess_KeyPress);
            // 
            // LabCode
            // 
            resources.ApplyResources(this.LabCode, "LabCode");
            this.LabCode.BackColor = System.Drawing.Color.Transparent;
            this.LabCode.Name = "LabCode";
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
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox editProcess;
        private System.Windows.Forms.Label LabCode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvDefect;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CLICK;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_DESC;
        private System.Windows.Forms.CheckBox ckbSelectAll;
    }
}