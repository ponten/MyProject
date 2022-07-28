namespace RCOutput_WO
{
    partial class fAddMachine
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnSubmit = new System.Windows.Forms.Button();
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
            this.machineDownModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.DtpStart = new System.Windows.Forms.DateTimePicker();
            this.CbSharedSettings = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDownModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.BtnCancel, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.BtnSubmit, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.DgvMachine, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.DtpStart, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.CbSharedSettings, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 311);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(276, 273);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(100, 30);
            this.BtnCancel.TabIndex = 1;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnSubmit
            // 
            this.BtnSubmit.Location = new System.Drawing.Point(170, 273);
            this.BtnSubmit.Name = "BtnSubmit";
            this.BtnSubmit.Size = new System.Drawing.Size(100, 30);
            this.BtnSubmit.TabIndex = 0;
            this.BtnSubmit.Text = "Submit";
            this.BtnSubmit.UseVisualStyleBackColor = true;
            // 
            // DgvMachine
            // 
            this.DgvMachine.AllowUserToAddRows = false;
            this.DgvMachine.AllowUserToDeleteRows = false;
            this.DgvMachine.AllowUserToResizeColumns = false;
            this.DgvMachine.AllowUserToResizeRows = false;
            this.DgvMachine.AutoGenerateColumns = false;
            this.DgvMachine.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
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
            this.tableLayoutPanel1.SetColumnSpan(this.DgvMachine, 4);
            this.DgvMachine.DataSource = this.machineDownModelBindingSource;
            this.DgvMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvMachine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvMachine.Location = new System.Drawing.Point(7, 7);
            this.DgvMachine.Margin = new System.Windows.Forms.Padding(2);
            this.DgvMachine.Name = "DgvMachine";
            this.DgvMachine.RowTemplate.Height = 24;
            this.DgvMachine.Size = new System.Drawing.Size(370, 216);
            this.DgvMachine.TabIndex = 2;
            // 
            // selectDataGridViewCheckBoxColumn
            // 
            this.selectDataGridViewCheckBoxColumn.DataPropertyName = "Select";
            this.selectDataGridViewCheckBoxColumn.Frozen = true;
            this.selectDataGridViewCheckBoxColumn.HeaderText = "Select";
            this.selectDataGridViewCheckBoxColumn.Name = "selectDataGridViewCheckBoxColumn";
            this.selectDataGridViewCheckBoxColumn.ReadOnly = true;
            this.selectDataGridViewCheckBoxColumn.Width = 51;
            // 
            // mACHINEIDDataGridViewTextBoxColumn
            // 
            this.mACHINEIDDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_ID";
            this.mACHINEIDDataGridViewTextBoxColumn.HeaderText = "MACHINE_ID";
            this.mACHINEIDDataGridViewTextBoxColumn.Name = "mACHINEIDDataGridViewTextBoxColumn";
            this.mACHINEIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINEIDDataGridViewTextBoxColumn.Visible = false;
            this.mACHINEIDDataGridViewTextBoxColumn.Width = 127;
            // 
            // mACHINECODEDataGridViewTextBoxColumn
            // 
            this.mACHINECODEDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_CODE";
            this.mACHINECODEDataGridViewTextBoxColumn.HeaderText = "Machine code";
            this.mACHINECODEDataGridViewTextBoxColumn.Name = "mACHINECODEDataGridViewTextBoxColumn";
            this.mACHINECODEDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINECODEDataGridViewTextBoxColumn.Width = 111;
            // 
            // mACHINEDESCDataGridViewTextBoxColumn
            // 
            this.mACHINEDESCDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_DESC";
            this.mACHINEDESCDataGridViewTextBoxColumn.HeaderText = "Machine";
            this.mACHINEDESCDataGridViewTextBoxColumn.Name = "mACHINEDESCDataGridViewTextBoxColumn";
            this.mACHINEDESCDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINEDESCDataGridViewTextBoxColumn.Width = 87;
            // 
            // sTATUSNAMEDataGridViewTextBoxColumn
            // 
            this.sTATUSNAMEDataGridViewTextBoxColumn.DataPropertyName = "STATUS_NAME";
            this.sTATUSNAMEDataGridViewTextBoxColumn.HeaderText = "STATUS_NAME";
            this.sTATUSNAMEDataGridViewTextBoxColumn.Name = "sTATUSNAMEDataGridViewTextBoxColumn";
            this.sTATUSNAMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTATUSNAMEDataGridViewTextBoxColumn.Visible = false;
            this.sTATUSNAMEDataGridViewTextBoxColumn.Width = 141;
            // 
            // tYPEIDDataGridViewTextBoxColumn
            // 
            this.tYPEIDDataGridViewTextBoxColumn.DataPropertyName = "TYPE_ID";
            this.tYPEIDDataGridViewTextBoxColumn.HeaderText = "TYPE_ID";
            this.tYPEIDDataGridViewTextBoxColumn.Name = "tYPEIDDataGridViewTextBoxColumn";
            this.tYPEIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.tYPEIDDataGridViewTextBoxColumn.Visible = false;
            this.tYPEIDDataGridViewTextBoxColumn.Width = 94;
            // 
            // rEASONIDDataGridViewTextBoxColumn
            // 
            this.rEASONIDDataGridViewTextBoxColumn.DataPropertyName = "REASON_ID";
            this.rEASONIDDataGridViewTextBoxColumn.HeaderText = "REASON_ID";
            this.rEASONIDDataGridViewTextBoxColumn.Name = "rEASONIDDataGridViewTextBoxColumn";
            this.rEASONIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.rEASONIDDataGridViewTextBoxColumn.Visible = false;
            this.rEASONIDDataGridViewTextBoxColumn.Width = 117;
            // 
            // sTARTTIMEDataGridViewTextBoxColumn
            // 
            this.sTARTTIMEDataGridViewTextBoxColumn.DataPropertyName = "START_TIME";
            this.sTARTTIMEDataGridViewTextBoxColumn.HeaderText = "START_TIME";
            this.sTARTTIMEDataGridViewTextBoxColumn.Name = "sTARTTIMEDataGridViewTextBoxColumn";
            this.sTARTTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTARTTIMEDataGridViewTextBoxColumn.Visible = false;
            this.sTARTTIMEDataGridViewTextBoxColumn.Width = 124;
            // 
            // eNDTIMEDataGridViewTextBoxColumn
            // 
            this.eNDTIMEDataGridViewTextBoxColumn.DataPropertyName = "END_TIME";
            this.eNDTIMEDataGridViewTextBoxColumn.HeaderText = "END_TIME";
            this.eNDTIMEDataGridViewTextBoxColumn.Name = "eNDTIMEDataGridViewTextBoxColumn";
            this.eNDTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.eNDTIMEDataGridViewTextBoxColumn.Visible = false;
            this.eNDTIMEDataGridViewTextBoxColumn.Width = 108;
            // 
            // lOADQTYDataGridViewTextBoxColumn
            // 
            this.lOADQTYDataGridViewTextBoxColumn.DataPropertyName = "LOAD_QTY";
            dataGridViewCellStyle3.NullValue = "0";
            this.lOADQTYDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.lOADQTYDataGridViewTextBoxColumn.HeaderText = "Load";
            this.lOADQTYDataGridViewTextBoxColumn.Name = "lOADQTYDataGridViewTextBoxColumn";
            this.lOADQTYDataGridViewTextBoxColumn.Width = 65;
            // 
            // dATECODEDataGridViewTextBoxColumn
            // 
            this.dATECODEDataGridViewTextBoxColumn.DataPropertyName = "DATE_CODE";
            this.dATECODEDataGridViewTextBoxColumn.HeaderText = "DateCode";
            this.dATECODEDataGridViewTextBoxColumn.Name = "dATECODEDataGridViewTextBoxColumn";
            this.dATECODEDataGridViewTextBoxColumn.Visible = false;
            this.dATECODEDataGridViewTextBoxColumn.Width = 95;
            // 
            // rEMARKDataGridViewTextBoxColumn
            // 
            this.rEMARKDataGridViewTextBoxColumn.DataPropertyName = "REMARK";
            this.rEMARKDataGridViewTextBoxColumn.HeaderText = "REMARK";
            this.rEMARKDataGridViewTextBoxColumn.Name = "rEMARKDataGridViewTextBoxColumn";
            this.rEMARKDataGridViewTextBoxColumn.ReadOnly = true;
            this.rEMARKDataGridViewTextBoxColumn.Visible = false;
            this.rEMARKDataGridViewTextBoxColumn.Width = 97;
            // 
            // machineDownModelBindingSource
            // 
            this.machineDownModelBindingSource.DataSource = typeof(RCOutput_WO.Models.MachineDownModel);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start time";
            // 
            // DtpStart
            // 
            this.DtpStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.DtpStart, 3);
            this.DtpStart.CustomFormat = "yyyy/ MM/ dd HH: mm: ss";
            this.DtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStart.Location = new System.Drawing.Point(81, 234);
            this.DtpStart.Name = "DtpStart";
            this.DtpStart.Size = new System.Drawing.Size(200, 27);
            this.DtpStart.TabIndex = 4;
            // 
            // CbSharedSettings
            // 
            this.CbSharedSettings.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CbSharedSettings.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.CbSharedSettings, 2);
            this.CbSharedSettings.Enabled = false;
            this.CbSharedSettings.Location = new System.Drawing.Point(8, 278);
            this.CbSharedSettings.Name = "CbSharedSettings";
            this.CbSharedSettings.Size = new System.Drawing.Size(151, 20);
            this.CbSharedSettings.TabIndex = 5;
            this.CbSharedSettings.Text = "Shared data settings";
            this.CbSharedSettings.UseVisualStyleBackColor = true;
            // 
            // fAddMachine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 311);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fAddMachine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add machine";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDownModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSubmit;
        private System.Windows.Forms.DataGridView DgvMachine;
        private System.Windows.Forms.DataGridViewTextBoxColumn rUNFLAGDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dEFAULTSTATUSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEASONCODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEASONDESCDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DtpStart;
        private System.Windows.Forms.BindingSource machineDownModelBindingSource;
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
        private System.Windows.Forms.CheckBox CbSharedSettings;
    }
}