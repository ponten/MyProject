namespace CSaleOrderInput
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
            this.panelControl = new System.Windows.Forms.Panel();
            this.dtRealDueDate = new DateTimePickerWithBackColor.BCDateTimePicker();
            this.dtRealDate = new DateTimePickerWithBackColor.BCDateTimePicker();
            this.gbSale = new System.Windows.Forms.GroupBox();
            this.btnSelectSale = new System.Windows.Forms.Button();
            this.lblSaleId = new System.Windows.Forms.Label();
            this.tbSaleId = new System.Windows.Forms.TextBox();
            this.lblSaleName = new System.Windows.Forms.Label();
            this.tbSaleName = new System.Windows.Forms.TextBox();
            this.gbToSend = new System.Windows.Forms.GroupBox();
            this.btnSelectToSend = new System.Windows.Forms.Button();
            this.lblToSendId = new System.Windows.Forms.Label();
            this.tbToSendId = new System.Windows.Forms.TextBox();
            this.lblToSendName = new System.Windows.Forms.Label();
            this.tbToSendName = new System.Windows.Forms.TextBox();
            this.gbAccount = new System.Windows.Forms.GroupBox();
            this.btnSelectAccount = new System.Windows.Forms.Button();
            this.lblAccountId = new System.Windows.Forms.Label();
            this.tbAccountId = new System.Windows.Forms.TextBox();
            this.lblAccountName = new System.Windows.Forms.Label();
            this.tbAccountName = new System.Windows.Forms.TextBox();
            this.gbCustomer = new System.Windows.Forms.GroupBox();
            this.btnSelectCustomer = new System.Windows.Forms.Button();
            this.lblCustomerId = new System.Windows.Forms.Label();
            this.tbCustomerId = new System.Windows.Forms.TextBox();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.tbCustomerName = new System.Windows.Forms.TextBox();
            this.tbNumber2 = new System.Windows.Forms.TextBox();
            this.tbOperationName = new System.Windows.Forms.TextBox();
            this.lblRealDueDate = new System.Windows.Forms.Label();
            this.lblNumber2 = new System.Windows.Forms.Label();
            this.lblOperationName = new System.Windows.Forms.Label();
            this.tbNumber3 = new System.Windows.Forms.TextBox();
            this.tbCustomize = new System.Windows.Forms.TextBox();
            this.lblNumber3 = new System.Windows.Forms.Label();
            this.lblCustomize = new System.Windows.Forms.Label();
            this.lblRealDate = new System.Windows.Forms.Label();
            this.tbOperationId = new System.Windows.Forms.TextBox();
            this.tbNumber1 = new System.Windows.Forms.TextBox();
            this.lblOperationId = new System.Windows.Forms.Label();
            this.lblNumber1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.editRealDate = new System.Windows.Forms.TextBox();
            this.editRealDueDate = new System.Windows.Forms.TextBox();
            this.panelControl.SuspendLayout();
            this.gbSale.SuspendLayout();
            this.gbToSend.SuspendLayout();
            this.gbAccount.SuspendLayout();
            this.gbCustomer.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            resources.ApplyResources(this.panelControl, "panelControl");
            this.panelControl.BackColor = System.Drawing.Color.Transparent;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelControl.Controls.Add(this.editRealDueDate);
            this.panelControl.Controls.Add(this.editRealDate);
            this.panelControl.Controls.Add(this.dtRealDueDate);
            this.panelControl.Controls.Add(this.dtRealDate);
            this.panelControl.Controls.Add(this.gbSale);
            this.panelControl.Controls.Add(this.gbToSend);
            this.panelControl.Controls.Add(this.gbAccount);
            this.panelControl.Controls.Add(this.gbCustomer);
            this.panelControl.Controls.Add(this.tbNumber2);
            this.panelControl.Controls.Add(this.tbOperationName);
            this.panelControl.Controls.Add(this.lblRealDueDate);
            this.panelControl.Controls.Add(this.lblNumber2);
            this.panelControl.Controls.Add(this.lblOperationName);
            this.panelControl.Controls.Add(this.tbNumber3);
            this.panelControl.Controls.Add(this.tbCustomize);
            this.panelControl.Controls.Add(this.lblNumber3);
            this.panelControl.Controls.Add(this.lblCustomize);
            this.panelControl.Controls.Add(this.lblRealDate);
            this.panelControl.Controls.Add(this.tbOperationId);
            this.panelControl.Controls.Add(this.tbNumber1);
            this.panelControl.Controls.Add(this.lblOperationId);
            this.panelControl.Controls.Add(this.lblNumber1);
            this.panelControl.Name = "panelControl";
            // 
            // dtRealDueDate
            // 
            resources.ApplyResources(this.dtRealDueDate, "dtRealDueDate");
            this.dtRealDueDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dtRealDueDate.BackDisabledColor = System.Drawing.SystemColors.Control;
            this.dtRealDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtRealDueDate.Name = "dtRealDueDate";
            this.dtRealDueDate.ValueChanged += new System.EventHandler(this.dtRealDueDate_ValueChanged);
            // 
            // dtRealDate
            // 
            this.dtRealDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dtRealDate.BackDisabledColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.dtRealDate, "dtRealDate");
            this.dtRealDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtRealDate.Name = "dtRealDate";
            this.dtRealDate.ValueChanged += new System.EventHandler(this.dtRealDate_ValueChanged);
            // 
            // gbSale
            // 
            resources.ApplyResources(this.gbSale, "gbSale");
            this.gbSale.Controls.Add(this.btnSelectSale);
            this.gbSale.Controls.Add(this.lblSaleId);
            this.gbSale.Controls.Add(this.tbSaleId);
            this.gbSale.Controls.Add(this.lblSaleName);
            this.gbSale.Controls.Add(this.tbSaleName);
            this.gbSale.Name = "gbSale";
            this.gbSale.TabStop = false;
            // 
            // btnSelectSale
            // 
            resources.ApplyResources(this.btnSelectSale, "btnSelectSale");
            this.btnSelectSale.Name = "btnSelectSale";
            this.btnSelectSale.UseVisualStyleBackColor = true;
            this.btnSelectSale.Click += new System.EventHandler(this.btnSelectSale_Click);
            // 
            // lblSaleId
            // 
            resources.ApplyResources(this.lblSaleId, "lblSaleId");
            this.lblSaleId.Name = "lblSaleId";
            // 
            // tbSaleId
            // 
            resources.ApplyResources(this.tbSaleId, "tbSaleId");
            this.tbSaleId.Name = "tbSaleId";
            this.tbSaleId.ReadOnly = true;
            // 
            // lblSaleName
            // 
            resources.ApplyResources(this.lblSaleName, "lblSaleName");
            this.lblSaleName.Name = "lblSaleName";
            // 
            // tbSaleName
            // 
            resources.ApplyResources(this.tbSaleName, "tbSaleName");
            this.tbSaleName.Name = "tbSaleName";
            this.tbSaleName.ReadOnly = true;
            // 
            // gbToSend
            // 
            resources.ApplyResources(this.gbToSend, "gbToSend");
            this.gbToSend.Controls.Add(this.btnSelectToSend);
            this.gbToSend.Controls.Add(this.lblToSendId);
            this.gbToSend.Controls.Add(this.tbToSendId);
            this.gbToSend.Controls.Add(this.lblToSendName);
            this.gbToSend.Controls.Add(this.tbToSendName);
            this.gbToSend.Name = "gbToSend";
            this.gbToSend.TabStop = false;
            // 
            // btnSelectToSend
            // 
            resources.ApplyResources(this.btnSelectToSend, "btnSelectToSend");
            this.btnSelectToSend.Name = "btnSelectToSend";
            this.btnSelectToSend.UseVisualStyleBackColor = true;
            this.btnSelectToSend.Click += new System.EventHandler(this.btnSelectCustomer_Click);
            // 
            // lblToSendId
            // 
            resources.ApplyResources(this.lblToSendId, "lblToSendId");
            this.lblToSendId.Name = "lblToSendId";
            // 
            // tbToSendId
            // 
            resources.ApplyResources(this.tbToSendId, "tbToSendId");
            this.tbToSendId.Name = "tbToSendId";
            this.tbToSendId.ReadOnly = true;
            // 
            // lblToSendName
            // 
            resources.ApplyResources(this.lblToSendName, "lblToSendName");
            this.lblToSendName.Name = "lblToSendName";
            // 
            // tbToSendName
            // 
            resources.ApplyResources(this.tbToSendName, "tbToSendName");
            this.tbToSendName.Name = "tbToSendName";
            this.tbToSendName.ReadOnly = true;
            // 
            // gbAccount
            // 
            resources.ApplyResources(this.gbAccount, "gbAccount");
            this.gbAccount.Controls.Add(this.btnSelectAccount);
            this.gbAccount.Controls.Add(this.lblAccountId);
            this.gbAccount.Controls.Add(this.tbAccountId);
            this.gbAccount.Controls.Add(this.lblAccountName);
            this.gbAccount.Controls.Add(this.tbAccountName);
            this.gbAccount.Name = "gbAccount";
            this.gbAccount.TabStop = false;
            // 
            // btnSelectAccount
            // 
            resources.ApplyResources(this.btnSelectAccount, "btnSelectAccount");
            this.btnSelectAccount.Name = "btnSelectAccount";
            this.btnSelectAccount.UseVisualStyleBackColor = true;
            this.btnSelectAccount.Click += new System.EventHandler(this.btnSelectCustomer_Click);
            // 
            // lblAccountId
            // 
            resources.ApplyResources(this.lblAccountId, "lblAccountId");
            this.lblAccountId.Name = "lblAccountId";
            // 
            // tbAccountId
            // 
            resources.ApplyResources(this.tbAccountId, "tbAccountId");
            this.tbAccountId.Name = "tbAccountId";
            this.tbAccountId.ReadOnly = true;
            // 
            // lblAccountName
            // 
            resources.ApplyResources(this.lblAccountName, "lblAccountName");
            this.lblAccountName.Name = "lblAccountName";
            // 
            // tbAccountName
            // 
            resources.ApplyResources(this.tbAccountName, "tbAccountName");
            this.tbAccountName.Name = "tbAccountName";
            this.tbAccountName.ReadOnly = true;
            // 
            // gbCustomer
            // 
            resources.ApplyResources(this.gbCustomer, "gbCustomer");
            this.gbCustomer.Controls.Add(this.btnSelectCustomer);
            this.gbCustomer.Controls.Add(this.lblCustomerId);
            this.gbCustomer.Controls.Add(this.tbCustomerId);
            this.gbCustomer.Controls.Add(this.lblCustomerName);
            this.gbCustomer.Controls.Add(this.tbCustomerName);
            this.gbCustomer.Name = "gbCustomer";
            this.gbCustomer.TabStop = false;
            // 
            // btnSelectCustomer
            // 
            resources.ApplyResources(this.btnSelectCustomer, "btnSelectCustomer");
            this.btnSelectCustomer.Name = "btnSelectCustomer";
            this.btnSelectCustomer.UseVisualStyleBackColor = true;
            this.btnSelectCustomer.Click += new System.EventHandler(this.btnSelectCustomer_Click);
            // 
            // lblCustomerId
            // 
            resources.ApplyResources(this.lblCustomerId, "lblCustomerId");
            this.lblCustomerId.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerId.Name = "lblCustomerId";
            // 
            // tbCustomerId
            // 
            resources.ApplyResources(this.tbCustomerId, "tbCustomerId");
            this.tbCustomerId.Name = "tbCustomerId";
            this.tbCustomerId.ReadOnly = true;
            // 
            // lblCustomerName
            // 
            resources.ApplyResources(this.lblCustomerName, "lblCustomerName");
            this.lblCustomerName.Name = "lblCustomerName";
            // 
            // tbCustomerName
            // 
            resources.ApplyResources(this.tbCustomerName, "tbCustomerName");
            this.tbCustomerName.Name = "tbCustomerName";
            this.tbCustomerName.ReadOnly = true;
            // 
            // tbNumber2
            // 
            resources.ApplyResources(this.tbNumber2, "tbNumber2");
            this.tbNumber2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tbNumber2.Name = "tbNumber2";
            // 
            // tbOperationName
            // 
            resources.ApplyResources(this.tbOperationName, "tbOperationName");
            this.tbOperationName.Name = "tbOperationName";
            // 
            // lblRealDueDate
            // 
            resources.ApplyResources(this.lblRealDueDate, "lblRealDueDate");
            this.lblRealDueDate.Name = "lblRealDueDate";
            // 
            // lblNumber2
            // 
            resources.ApplyResources(this.lblNumber2, "lblNumber2");
            this.lblNumber2.Name = "lblNumber2";
            // 
            // lblOperationName
            // 
            resources.ApplyResources(this.lblOperationName, "lblOperationName");
            this.lblOperationName.Name = "lblOperationName";
            // 
            // tbNumber3
            // 
            resources.ApplyResources(this.tbNumber3, "tbNumber3");
            this.tbNumber3.Name = "tbNumber3";
            // 
            // tbCustomize
            // 
            resources.ApplyResources(this.tbCustomize, "tbCustomize");
            this.tbCustomize.Name = "tbCustomize";
            // 
            // lblNumber3
            // 
            resources.ApplyResources(this.lblNumber3, "lblNumber3");
            this.lblNumber3.Name = "lblNumber3";
            // 
            // lblCustomize
            // 
            resources.ApplyResources(this.lblCustomize, "lblCustomize");
            this.lblCustomize.Name = "lblCustomize";
            // 
            // lblRealDate
            // 
            resources.ApplyResources(this.lblRealDate, "lblRealDate");
            this.lblRealDate.Name = "lblRealDate";
            // 
            // tbOperationId
            // 
            resources.ApplyResources(this.tbOperationId, "tbOperationId");
            this.tbOperationId.Name = "tbOperationId";
            // 
            // tbNumber1
            // 
            this.tbNumber1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.tbNumber1, "tbNumber1");
            this.tbNumber1.Name = "tbNumber1";
            // 
            // lblOperationId
            // 
            resources.ApplyResources(this.lblOperationId, "lblOperationId");
            this.lblOperationId.BackColor = System.Drawing.Color.Transparent;
            this.lblOperationId.Name = "lblOperationId";
            // 
            // lblNumber1
            // 
            resources.ApplyResources(this.lblNumber1, "lblNumber1");
            this.lblNumber1.BackColor = System.Drawing.Color.Transparent;
            this.lblNumber1.Name = "lblNumber1";
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
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // editRealDate
            // 
            this.editRealDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editRealDate, "editRealDate");
            this.editRealDate.Name = "editRealDate";
            this.editRealDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editRealDate_KeyPress);
            // 
            // editRealDueDate
            // 
            this.editRealDueDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editRealDueDate, "editRealDueDate");
            this.editRealDueDate.Name = "editRealDueDate";
            this.editRealDueDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editRealDueDate_KeyPress);
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
            this.panelControl.PerformLayout();
            this.gbSale.ResumeLayout(false);
            this.gbSale.PerformLayout();
            this.gbToSend.ResumeLayout(false);
            this.gbToSend.PerformLayout();
            this.gbAccount.ResumeLayout(false);
            this.gbAccount.PerformLayout();
            this.gbCustomer.ResumeLayout(false);
            this.gbCustomer.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblOperationId;
        private System.Windows.Forms.TextBox tbNumber1;
        private System.Windows.Forms.Label lblNumber1;
        private System.Windows.Forms.TextBox tbCustomerId;
        private System.Windows.Forms.Label lblCustomerId;
        private System.Windows.Forms.TextBox tbOperationId;
        private System.Windows.Forms.TextBox tbNumber3;
        private System.Windows.Forms.TextBox tbCustomize;
        private System.Windows.Forms.TextBox tbSaleId;
        private System.Windows.Forms.TextBox tbToSendId;
        private System.Windows.Forms.TextBox tbAccountId;
        private System.Windows.Forms.Label lblNumber3;
        private System.Windows.Forms.Label lblCustomize;
        private System.Windows.Forms.Label lblRealDate;
        private System.Windows.Forms.Label lblSaleId;
        private System.Windows.Forms.Label lblToSendId;
        private System.Windows.Forms.Label lblAccountId;
        private System.Windows.Forms.Label lblOperationName;
        private System.Windows.Forms.TextBox tbSaleName;
        private System.Windows.Forms.TextBox tbToSendName;
        private System.Windows.Forms.TextBox tbAccountName;
        private System.Windows.Forms.TextBox tbCustomerName;
        private System.Windows.Forms.TextBox tbNumber2;
        private System.Windows.Forms.TextBox tbOperationName;
        private System.Windows.Forms.Label lblRealDueDate;
        private System.Windows.Forms.Label lblSaleName;
        private System.Windows.Forms.Label lblToSendName;
        private System.Windows.Forms.Label lblAccountName;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblNumber2;
        private System.Windows.Forms.GroupBox gbCustomer;
        private System.Windows.Forms.Button btnSelectCustomer;
        private System.Windows.Forms.GroupBox gbAccount;
        private System.Windows.Forms.Button btnSelectAccount;
        private System.Windows.Forms.GroupBox gbToSend;
        private System.Windows.Forms.Button btnSelectToSend;
        private System.Windows.Forms.GroupBox gbSale;
        private System.Windows.Forms.Button btnSelectSale;
        private DateTimePickerWithBackColor.BCDateTimePicker dtRealDate;
        private DateTimePickerWithBackColor.BCDateTimePicker dtRealDueDate;
        private System.Windows.Forms.TextBox editRealDueDate;
        private System.Windows.Forms.TextBox editRealDate;
    }
}