namespace MachineAAR
{
    partial class fMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.TlpForm = new System.Windows.Forms.TableLayoutPanel();
            this.TlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.LbStart = new System.Windows.Forms.Label();
            this.DtpStart = new System.Windows.Forms.DateTimePicker();
            this.LbEnd = new System.Windows.Forms.Label();
            this.DtpEnd = new System.Windows.Forms.DateTimePicker();
            this.BtnOK = new System.Windows.Forms.Button();
            this.DgvMachine = new System.Windows.Forms.DataGridView();
            this.selectDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.mACHINEIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mACHINECODEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mACHINEDESCDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTATUSNAMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tYPEIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rEASONIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTARTTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eNDTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lOADQTYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dATECODEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rEMARKDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machineModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TlpTop = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnClear = new System.Windows.Forms.Button();
            this.TbRC = new System.Windows.Forms.TextBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.CbProcess = new System.Windows.Forms.ComboBox();
            this.CbStage = new System.Windows.Forms.ComboBox();
            this.TbWO = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LbT4T6 = new System.Windows.Forms.Label();
            this.DgvRuncard = new System.Windows.Forms.DataGridView();
            this.runcardModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.wORKORDERDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pARTNODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sPEC1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oPTION2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oPTION4DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rCNODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pROCESSIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nODEIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cURRENTQTYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wIPOUTTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TlpForm.SuspendLayout();
            this.TlpBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineModelBindingSource)).BeginInit();
            this.TlpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvRuncard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.runcardModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // TlpForm
            // 
            resources.ApplyResources(this.TlpForm, "TlpForm");
            this.TlpForm.Controls.Add(this.TlpBottom, 0, 3);
            this.TlpForm.Controls.Add(this.DgvMachine, 0, 2);
            this.TlpForm.Controls.Add(this.TlpTop, 0, 0);
            this.TlpForm.Controls.Add(this.DgvRuncard, 0, 1);
            this.TlpForm.Name = "TlpForm";
            // 
            // TlpBottom
            // 
            resources.ApplyResources(this.TlpBottom, "TlpBottom");
            this.TlpBottom.Controls.Add(this.label1, 0, 0);
            this.TlpBottom.Controls.Add(this.LbStart, 1, 0);
            this.TlpBottom.Controls.Add(this.DtpStart, 2, 0);
            this.TlpBottom.Controls.Add(this.LbEnd, 3, 0);
            this.TlpBottom.Controls.Add(this.DtpEnd, 4, 0);
            this.TlpBottom.Controls.Add(this.BtnOK, 5, 0);
            this.TlpBottom.Name = "TlpBottom";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // LbStart
            // 
            resources.ApplyResources(this.LbStart, "LbStart");
            this.LbStart.Name = "LbStart";
            // 
            // DtpStart
            // 
            resources.ApplyResources(this.DtpStart, "DtpStart");
            this.DtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStart.Name = "DtpStart";
            // 
            // LbEnd
            // 
            resources.ApplyResources(this.LbEnd, "LbEnd");
            this.LbEnd.Name = "LbEnd";
            // 
            // DtpEnd
            // 
            resources.ApplyResources(this.DtpEnd, "DtpEnd");
            this.DtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEnd.Name = "DtpEnd";
            // 
            // BtnOK
            // 
            resources.ApplyResources(this.BtnOK, "BtnOK");
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.UseVisualStyleBackColor = true;
            // 
            // DgvMachine
            // 
            this.DgvMachine.AllowUserToAddRows = false;
            this.DgvMachine.AllowUserToDeleteRows = false;
            this.DgvMachine.AllowUserToResizeRows = false;
            this.DgvMachine.AutoGenerateColumns = false;
            this.DgvMachine.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DgvMachine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMachine.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectDataGridViewCheckBoxColumn,
            this.mACHINEIDDataGridViewTextBoxColumn,
            this.mACHINECODEDataGridViewTextBoxColumn,
            this.mACHINEDESCDataGridViewTextBoxColumn,
            this.sTATUSNAMEDataGridViewTextBoxColumn,
            this.tYPEIDDataGridViewTextBoxColumn,
            this.rEASONIDDataGridViewTextBoxColumn,
            this.sTARTTIMEDataGridViewTextBoxColumn,
            this.eNDTIMEDataGridViewTextBoxColumn,
            this.lOADQTYDataGridViewTextBoxColumn,
            this.dATECODEDataGridViewTextBoxColumn,
            this.rEMARKDataGridViewTextBoxColumn});
            this.DgvMachine.DataSource = this.machineModelBindingSource;
            resources.ApplyResources(this.DgvMachine, "DgvMachine");
            this.DgvMachine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvMachine.MultiSelect = false;
            this.DgvMachine.Name = "DgvMachine";
            this.DgvMachine.RowTemplate.Height = 24;
            // 
            // selectDataGridViewCheckBoxColumn
            // 
            this.selectDataGridViewCheckBoxColumn.DataPropertyName = "Select";
            resources.ApplyResources(this.selectDataGridViewCheckBoxColumn, "selectDataGridViewCheckBoxColumn");
            this.selectDataGridViewCheckBoxColumn.Name = "selectDataGridViewCheckBoxColumn";
            this.selectDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // mACHINEIDDataGridViewTextBoxColumn
            // 
            this.mACHINEIDDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_ID";
            resources.ApplyResources(this.mACHINEIDDataGridViewTextBoxColumn, "mACHINEIDDataGridViewTextBoxColumn");
            this.mACHINEIDDataGridViewTextBoxColumn.Name = "mACHINEIDDataGridViewTextBoxColumn";
            this.mACHINEIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // mACHINECODEDataGridViewTextBoxColumn
            // 
            this.mACHINECODEDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_CODE";
            resources.ApplyResources(this.mACHINECODEDataGridViewTextBoxColumn, "mACHINECODEDataGridViewTextBoxColumn");
            this.mACHINECODEDataGridViewTextBoxColumn.Name = "mACHINECODEDataGridViewTextBoxColumn";
            this.mACHINECODEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // mACHINEDESCDataGridViewTextBoxColumn
            // 
            this.mACHINEDESCDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_DESC";
            resources.ApplyResources(this.mACHINEDESCDataGridViewTextBoxColumn, "mACHINEDESCDataGridViewTextBoxColumn");
            this.mACHINEDESCDataGridViewTextBoxColumn.Name = "mACHINEDESCDataGridViewTextBoxColumn";
            this.mACHINEDESCDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sTATUSNAMEDataGridViewTextBoxColumn
            // 
            this.sTATUSNAMEDataGridViewTextBoxColumn.DataPropertyName = "STATUS_NAME";
            resources.ApplyResources(this.sTATUSNAMEDataGridViewTextBoxColumn, "sTATUSNAMEDataGridViewTextBoxColumn");
            this.sTATUSNAMEDataGridViewTextBoxColumn.Name = "sTATUSNAMEDataGridViewTextBoxColumn";
            this.sTATUSNAMEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tYPEIDDataGridViewTextBoxColumn
            // 
            this.tYPEIDDataGridViewTextBoxColumn.DataPropertyName = "TYPE_ID";
            resources.ApplyResources(this.tYPEIDDataGridViewTextBoxColumn, "tYPEIDDataGridViewTextBoxColumn");
            this.tYPEIDDataGridViewTextBoxColumn.Name = "tYPEIDDataGridViewTextBoxColumn";
            this.tYPEIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rEASONIDDataGridViewTextBoxColumn
            // 
            this.rEASONIDDataGridViewTextBoxColumn.DataPropertyName = "REASON_ID";
            resources.ApplyResources(this.rEASONIDDataGridViewTextBoxColumn, "rEASONIDDataGridViewTextBoxColumn");
            this.rEASONIDDataGridViewTextBoxColumn.Name = "rEASONIDDataGridViewTextBoxColumn";
            this.rEASONIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sTARTTIMEDataGridViewTextBoxColumn
            // 
            this.sTARTTIMEDataGridViewTextBoxColumn.DataPropertyName = "START_TIME";
            resources.ApplyResources(this.sTARTTIMEDataGridViewTextBoxColumn, "sTARTTIMEDataGridViewTextBoxColumn");
            this.sTARTTIMEDataGridViewTextBoxColumn.Name = "sTARTTIMEDataGridViewTextBoxColumn";
            this.sTARTTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // eNDTIMEDataGridViewTextBoxColumn
            // 
            this.eNDTIMEDataGridViewTextBoxColumn.DataPropertyName = "END_TIME";
            resources.ApplyResources(this.eNDTIMEDataGridViewTextBoxColumn, "eNDTIMEDataGridViewTextBoxColumn");
            this.eNDTIMEDataGridViewTextBoxColumn.Name = "eNDTIMEDataGridViewTextBoxColumn";
            this.eNDTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lOADQTYDataGridViewTextBoxColumn
            // 
            this.lOADQTYDataGridViewTextBoxColumn.DataPropertyName = "LOAD_QTY";
            resources.ApplyResources(this.lOADQTYDataGridViewTextBoxColumn, "lOADQTYDataGridViewTextBoxColumn");
            this.lOADQTYDataGridViewTextBoxColumn.Name = "lOADQTYDataGridViewTextBoxColumn";
            // 
            // dATECODEDataGridViewTextBoxColumn
            // 
            this.dATECODEDataGridViewTextBoxColumn.DataPropertyName = "DATE_CODE";
            resources.ApplyResources(this.dATECODEDataGridViewTextBoxColumn, "dATECODEDataGridViewTextBoxColumn");
            this.dATECODEDataGridViewTextBoxColumn.Name = "dATECODEDataGridViewTextBoxColumn";
            // 
            // rEMARKDataGridViewTextBoxColumn
            // 
            this.rEMARKDataGridViewTextBoxColumn.DataPropertyName = "REMARK";
            resources.ApplyResources(this.rEMARKDataGridViewTextBoxColumn, "rEMARKDataGridViewTextBoxColumn");
            this.rEMARKDataGridViewTextBoxColumn.Name = "rEMARKDataGridViewTextBoxColumn";
            this.rEMARKDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // machineModelBindingSource
            // 
            this.machineModelBindingSource.DataSource = typeof(MachineAAR.MachineModel);
            // 
            // TlpTop
            // 
            resources.ApplyResources(this.TlpTop, "TlpTop");
            this.TlpTop.Controls.Add(this.label2, 0, 0);
            this.TlpTop.Controls.Add(this.label3, 2, 0);
            this.TlpTop.Controls.Add(this.BtnClear, 5, 1);
            this.TlpTop.Controls.Add(this.TbRC, 3, 1);
            this.TlpTop.Controls.Add(this.BtnSearch, 4, 1);
            this.TlpTop.Controls.Add(this.CbProcess, 3, 0);
            this.TlpTop.Controls.Add(this.CbStage, 1, 0);
            this.TlpTop.Controls.Add(this.TbWO, 1, 1);
            this.TlpTop.Controls.Add(this.label4, 0, 1);
            this.TlpTop.Controls.Add(this.label5, 2, 1);
            this.TlpTop.Controls.Add(this.LbT4T6, 4, 0);
            this.TlpTop.Name = "TlpTop";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // BtnClear
            // 
            resources.ApplyResources(this.BtnClear, "BtnClear");
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.UseVisualStyleBackColor = true;
            // 
            // TbRC
            // 
            resources.ApplyResources(this.TbRC, "TbRC");
            this.TbRC.Name = "TbRC";
            // 
            // BtnSearch
            // 
            resources.ApplyResources(this.BtnSearch, "BtnSearch");
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.UseVisualStyleBackColor = true;
            // 
            // CbProcess
            // 
            resources.ApplyResources(this.CbProcess, "CbProcess");
            this.CbProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbProcess.FormattingEnabled = true;
            this.CbProcess.Name = "CbProcess";
            // 
            // CbStage
            // 
            resources.ApplyResources(this.CbStage, "CbStage");
            this.CbStage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbStage.FormattingEnabled = true;
            this.CbStage.Name = "CbStage";
            // 
            // TbWO
            // 
            resources.ApplyResources(this.TbWO, "TbWO");
            this.TbWO.Name = "TbWO";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // LbT4T6
            // 
            resources.ApplyResources(this.LbT4T6, "LbT4T6");
            this.LbT4T6.Name = "LbT4T6";
            // 
            // DgvRuncard
            // 
            this.DgvRuncard.AllowUserToAddRows = false;
            this.DgvRuncard.AllowUserToDeleteRows = false;
            this.DgvRuncard.AllowUserToResizeRows = false;
            this.DgvRuncard.AutoGenerateColumns = false;
            this.DgvRuncard.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DgvRuncard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvRuncard.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.wORKORDERDataGridViewTextBoxColumn,
            this.pARTNODataGridViewTextBoxColumn,
            this.sPEC1DataGridViewTextBoxColumn,
            this.oPTION2DataGridViewTextBoxColumn,
            this.oPTION4DataGridViewTextBoxColumn,
            this.rCNODataGridViewTextBoxColumn,
            this.pROCESSIDDataGridViewTextBoxColumn,
            this.nODEIDDataGridViewTextBoxColumn,
            this.cURRENTQTYDataGridViewTextBoxColumn,
            this.wIPOUTTIMEDataGridViewTextBoxColumn});
            this.DgvRuncard.DataSource = this.runcardModelBindingSource;
            resources.ApplyResources(this.DgvRuncard, "DgvRuncard");
            this.DgvRuncard.MultiSelect = false;
            this.DgvRuncard.Name = "DgvRuncard";
            this.DgvRuncard.ReadOnly = true;
            this.DgvRuncard.RowTemplate.Height = 24;
            // 
            // runcardModelBindingSource
            // 
            this.runcardModelBindingSource.DataSource = typeof(MachineAAR.RuncardModel);
            // 
            // wORKORDERDataGridViewTextBoxColumn
            // 
            this.wORKORDERDataGridViewTextBoxColumn.DataPropertyName = "WORK_ORDER";
            resources.ApplyResources(this.wORKORDERDataGridViewTextBoxColumn, "wORKORDERDataGridViewTextBoxColumn");
            this.wORKORDERDataGridViewTextBoxColumn.Name = "wORKORDERDataGridViewTextBoxColumn";
            this.wORKORDERDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pARTNODataGridViewTextBoxColumn
            // 
            this.pARTNODataGridViewTextBoxColumn.DataPropertyName = "PART_NO";
            resources.ApplyResources(this.pARTNODataGridViewTextBoxColumn, "pARTNODataGridViewTextBoxColumn");
            this.pARTNODataGridViewTextBoxColumn.Name = "pARTNODataGridViewTextBoxColumn";
            this.pARTNODataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sPEC1DataGridViewTextBoxColumn
            // 
            this.sPEC1DataGridViewTextBoxColumn.DataPropertyName = "SPEC1";
            resources.ApplyResources(this.sPEC1DataGridViewTextBoxColumn, "sPEC1DataGridViewTextBoxColumn");
            this.sPEC1DataGridViewTextBoxColumn.Name = "sPEC1DataGridViewTextBoxColumn";
            this.sPEC1DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // oPTION2DataGridViewTextBoxColumn
            // 
            this.oPTION2DataGridViewTextBoxColumn.DataPropertyName = "OPTION2";
            resources.ApplyResources(this.oPTION2DataGridViewTextBoxColumn, "oPTION2DataGridViewTextBoxColumn");
            this.oPTION2DataGridViewTextBoxColumn.Name = "oPTION2DataGridViewTextBoxColumn";
            this.oPTION2DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // oPTION4DataGridViewTextBoxColumn
            // 
            this.oPTION4DataGridViewTextBoxColumn.DataPropertyName = "OPTION4";
            resources.ApplyResources(this.oPTION4DataGridViewTextBoxColumn, "oPTION4DataGridViewTextBoxColumn");
            this.oPTION4DataGridViewTextBoxColumn.Name = "oPTION4DataGridViewTextBoxColumn";
            this.oPTION4DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rCNODataGridViewTextBoxColumn
            // 
            this.rCNODataGridViewTextBoxColumn.DataPropertyName = "RC_NO";
            resources.ApplyResources(this.rCNODataGridViewTextBoxColumn, "rCNODataGridViewTextBoxColumn");
            this.rCNODataGridViewTextBoxColumn.Name = "rCNODataGridViewTextBoxColumn";
            this.rCNODataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pROCESSIDDataGridViewTextBoxColumn
            // 
            this.pROCESSIDDataGridViewTextBoxColumn.DataPropertyName = "PROCESS_ID";
            resources.ApplyResources(this.pROCESSIDDataGridViewTextBoxColumn, "pROCESSIDDataGridViewTextBoxColumn");
            this.pROCESSIDDataGridViewTextBoxColumn.Name = "pROCESSIDDataGridViewTextBoxColumn";
            this.pROCESSIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nODEIDDataGridViewTextBoxColumn
            // 
            this.nODEIDDataGridViewTextBoxColumn.DataPropertyName = "NODE_ID";
            resources.ApplyResources(this.nODEIDDataGridViewTextBoxColumn, "nODEIDDataGridViewTextBoxColumn");
            this.nODEIDDataGridViewTextBoxColumn.Name = "nODEIDDataGridViewTextBoxColumn";
            this.nODEIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cURRENTQTYDataGridViewTextBoxColumn
            // 
            this.cURRENTQTYDataGridViewTextBoxColumn.DataPropertyName = "CURRENT_QTY";
            resources.ApplyResources(this.cURRENTQTYDataGridViewTextBoxColumn, "cURRENTQTYDataGridViewTextBoxColumn");
            this.cURRENTQTYDataGridViewTextBoxColumn.Name = "cURRENTQTYDataGridViewTextBoxColumn";
            this.cURRENTQTYDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // wIPOUTTIMEDataGridViewTextBoxColumn
            // 
            this.wIPOUTTIMEDataGridViewTextBoxColumn.DataPropertyName = "WIP_OUT_TIME";
            resources.ApplyResources(this.wIPOUTTIMEDataGridViewTextBoxColumn, "wIPOUTTIMEDataGridViewTextBoxColumn");
            this.wIPOUTTIMEDataGridViewTextBoxColumn.Name = "wIPOUTTIMEDataGridViewTextBoxColumn";
            this.wIPOUTTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TlpForm);
            this.Name = "fMain";
            this.TlpForm.ResumeLayout(false);
            this.TlpBottom.ResumeLayout(false);
            this.TlpBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineModelBindingSource)).EndInit();
            this.TlpTop.ResumeLayout(false);
            this.TlpTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvRuncard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.runcardModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TlpForm;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.DateTimePicker DtpEnd;
        private System.Windows.Forms.DateTimePicker DtpStart;
        private System.Windows.Forms.Label LbStart;
        private System.Windows.Forms.Label LbEnd;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.DataGridView DgvMachine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CbStage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CbProcess;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TbWO;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TbRC;
        private System.Windows.Forms.DataGridView DgvRuncard;
        private System.Windows.Forms.TableLayoutPanel TlpBottom;
        private System.Windows.Forms.TableLayoutPanel TlpTop;
        private System.Windows.Forms.BindingSource machineModelBindingSource;
        private System.Windows.Forms.BindingSource runcardModelBindingSource;
        private System.Windows.Forms.Label LbT4T6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mACHINEIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mACHINECODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mACHINEDESCDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sTATUSNAMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tYPEIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEASONIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sTARTTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eNDTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lOADQTYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dATECODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEMARKDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn wORKORDERDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pARTNODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sPEC1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oPTION2DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oPTION4DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rCNODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pROCESSIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nODEIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cURRENTQTYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn wIPOUTTIMEDataGridViewTextBoxColumn;
    }
}