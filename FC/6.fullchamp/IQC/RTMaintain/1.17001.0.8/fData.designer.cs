namespace RTMaintain
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lablVendorName = new System.Windows.Forms.Label();
            this.editVendor = new System.Windows.Forms.TextBox();
            this.btnFilterVendor = new System.Windows.Forms.Button();
            this.combVendorID = new System.Windows.Forms.ComboBox();
            this.editPONo = new System.Windows.Forms.TextBox();
            this.editRTNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lablVendorName);
            this.panel1.Controls.Add(this.editVendor);
            this.panel1.Controls.Add(this.btnFilterVendor);
            this.panel1.Controls.Add(this.combVendorID);
            this.panel1.Controls.Add(this.editPONo);
            this.panel1.Controls.Add(this.editRTNo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // lablVendorName
            // 
            resources.ApplyResources(this.lablVendorName, "lablVendorName");
            this.lablVendorName.BackColor = System.Drawing.Color.Transparent;
            this.lablVendorName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lablVendorName.Name = "lablVendorName";
            // 
            // editVendor
            // 
            this.editVendor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.editVendor, "editVendor");
            this.editVendor.Name = "editVendor";
            this.editVendor.TextChanged += new System.EventHandler(this.editVendor_TextChanged);
            this.editVendor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editVendor_KeyPress);
            // 
            // btnFilterVendor
            // 
            resources.ApplyResources(this.btnFilterVendor, "btnFilterVendor");
            this.btnFilterVendor.Name = "btnFilterVendor";
            this.btnFilterVendor.TabStop = false;
            this.btnFilterVendor.Tag = "1";
            this.btnFilterVendor.UseVisualStyleBackColor = true;
            this.btnFilterVendor.Click += new System.EventHandler(this.btnFilterVendor_Click);
            // 
            // combVendorID
            // 
            this.combVendorID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.combVendorID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combVendorID, "combVendorID");
            this.combVendorID.FormattingEnabled = true;
            this.combVendorID.Name = "combVendorID";
            // 
            // editPONo
            // 
            resources.ApplyResources(this.editPONo, "editPONo");
            this.editPONo.Name = "editPONo";
            this.editPONo.TextChanged += new System.EventHandler(this.editPONo_TextChanged);
            // 
            // editRTNo
            // 
            this.editRTNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.editRTNo, "editRTNo");
            this.editRTNo.Name = "editRTNo";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Name = "panel2";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // fData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "fData";
            this.Load += new System.EventHandler(this.fData_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox editPONo;
        private System.Windows.Forms.TextBox editRTNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox combVendorID;
        private System.Windows.Forms.TextBox editVendor;
        private System.Windows.Forms.Button btnFilterVendor;
        private System.Windows.Forms.Label lablVendorName;
        private System.Windows.Forms.Label label4;
    }
}