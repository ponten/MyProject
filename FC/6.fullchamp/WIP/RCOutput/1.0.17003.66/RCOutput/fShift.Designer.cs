namespace RCOutput
{
    partial class fShift
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.DtpEnd = new System.Windows.Forms.DateTimePicker();
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
            this.sTOVESEQDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rEMARKDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machineDownModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDownModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.BtnCancel, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.DtpEnd, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.BtnSubmit, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.DgvMachine, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(459, 211);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnCancel.Location = new System.Drawing.Point(350, 177);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(100, 30);
            this.BtnCancel.TabIndex = 1;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 147);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "End time";
            // 
            // DtpEnd
            // 
            this.DtpEnd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.DtpEnd, 2);
            this.DtpEnd.CustomFormat = "yyyy/ MM/ dd HH: mm: ss";
            this.DtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEnd.Location = new System.Drawing.Point(79, 142);
            this.DtpEnd.Margin = new System.Windows.Forms.Padding(2);
            this.DtpEnd.Name = "DtpEnd";
            this.DtpEnd.Size = new System.Drawing.Size(248, 27);
            this.DtpEnd.TabIndex = 9;
            // 
            // BtnSubmit
            // 
            this.BtnSubmit.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnSubmit.Location = new System.Drawing.Point(242, 177);
            this.BtnSubmit.Margin = new System.Windows.Forms.Padding(4);
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
            this.DgvMachine.AutoGenerateColumns = false;
            this.DgvMachine.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvMachine.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
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
            this.sTOVESEQDataGridViewTextBoxColumn,
            this.rEMARKDataGridViewTextBoxColumn});
            this.tableLayoutPanel1.SetColumnSpan(this.DgvMachine, 3);
            this.DgvMachine.DataSource = this.machineDownModelBindingSource;
            this.DgvMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvMachine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvMachine.Location = new System.Drawing.Point(8, 3);
            this.DgvMachine.MultiSelect = false;
            this.DgvMachine.Name = "DgvMachine";
            this.DgvMachine.RowTemplate.Height = 24;
            this.DgvMachine.Size = new System.Drawing.Size(443, 132);
            this.DgvMachine.TabIndex = 10;
            // 
            // selectDataGridViewCheckBoxColumn
            // 
            this.selectDataGridViewCheckBoxColumn.DataPropertyName = "Select";
            this.selectDataGridViewCheckBoxColumn.HeaderText = "Select";
            this.selectDataGridViewCheckBoxColumn.Name = "selectDataGridViewCheckBoxColumn";
            this.selectDataGridViewCheckBoxColumn.ReadOnly = true;
            this.selectDataGridViewCheckBoxColumn.Visible = false;
            // 
            // mACHINEIDDataGridViewTextBoxColumn
            // 
            this.mACHINEIDDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_ID";
            this.mACHINEIDDataGridViewTextBoxColumn.HeaderText = "MACHINE_ID";
            this.mACHINEIDDataGridViewTextBoxColumn.Name = "mACHINEIDDataGridViewTextBoxColumn";
            this.mACHINEIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINEIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // mACHINECODEDataGridViewTextBoxColumn
            // 
            this.mACHINECODEDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_CODE";
            this.mACHINECODEDataGridViewTextBoxColumn.HeaderText = "Machine code";
            this.mACHINECODEDataGridViewTextBoxColumn.Name = "mACHINECODEDataGridViewTextBoxColumn";
            this.mACHINECODEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // mACHINEDESCDataGridViewTextBoxColumn
            // 
            this.mACHINEDESCDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_DESC";
            this.mACHINEDESCDataGridViewTextBoxColumn.HeaderText = "Machine";
            this.mACHINEDESCDataGridViewTextBoxColumn.Name = "mACHINEDESCDataGridViewTextBoxColumn";
            this.mACHINEDESCDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sTATUSNAMEDataGridViewTextBoxColumn
            // 
            this.sTATUSNAMEDataGridViewTextBoxColumn.DataPropertyName = "STATUS_NAME";
            this.sTATUSNAMEDataGridViewTextBoxColumn.HeaderText = "STATUS_NAME";
            this.sTATUSNAMEDataGridViewTextBoxColumn.Name = "sTATUSNAMEDataGridViewTextBoxColumn";
            this.sTATUSNAMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTATUSNAMEDataGridViewTextBoxColumn.Visible = false;
            // 
            // tYPEIDDataGridViewTextBoxColumn
            // 
            this.tYPEIDDataGridViewTextBoxColumn.DataPropertyName = "TYPE_ID";
            this.tYPEIDDataGridViewTextBoxColumn.HeaderText = "TYPE_ID";
            this.tYPEIDDataGridViewTextBoxColumn.Name = "tYPEIDDataGridViewTextBoxColumn";
            this.tYPEIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.tYPEIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // rEASONIDDataGridViewTextBoxColumn
            // 
            this.rEASONIDDataGridViewTextBoxColumn.DataPropertyName = "REASON_ID";
            this.rEASONIDDataGridViewTextBoxColumn.HeaderText = "REASON_ID";
            this.rEASONIDDataGridViewTextBoxColumn.Name = "rEASONIDDataGridViewTextBoxColumn";
            this.rEASONIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.rEASONIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // sTARTTIMEDataGridViewTextBoxColumn
            // 
            this.sTARTTIMEDataGridViewTextBoxColumn.DataPropertyName = "START_TIME";
            this.sTARTTIMEDataGridViewTextBoxColumn.HeaderText = "START_TIME";
            this.sTARTTIMEDataGridViewTextBoxColumn.Name = "sTARTTIMEDataGridViewTextBoxColumn";
            this.sTARTTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTARTTIMEDataGridViewTextBoxColumn.Visible = false;
            // 
            // eNDTIMEDataGridViewTextBoxColumn
            // 
            this.eNDTIMEDataGridViewTextBoxColumn.DataPropertyName = "END_TIME";
            this.eNDTIMEDataGridViewTextBoxColumn.HeaderText = "END_TIME";
            this.eNDTIMEDataGridViewTextBoxColumn.Name = "eNDTIMEDataGridViewTextBoxColumn";
            this.eNDTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.eNDTIMEDataGridViewTextBoxColumn.Visible = false;
            // 
            // lOADQTYDataGridViewTextBoxColumn
            // 
            this.lOADQTYDataGridViewTextBoxColumn.DataPropertyName = "LOAD_QTY";
            this.lOADQTYDataGridViewTextBoxColumn.HeaderText = "Load";
            this.lOADQTYDataGridViewTextBoxColumn.Name = "lOADQTYDataGridViewTextBoxColumn";
            // 
            // dATECODEDataGridViewTextBoxColumn
            // 
            this.dATECODEDataGridViewTextBoxColumn.DataPropertyName = "DATE_CODE";
            this.dATECODEDataGridViewTextBoxColumn.HeaderText = "DATE_CODE";
            this.dATECODEDataGridViewTextBoxColumn.Name = "dATECODEDataGridViewTextBoxColumn";
            this.dATECODEDataGridViewTextBoxColumn.ReadOnly = true;
            this.dATECODEDataGridViewTextBoxColumn.Visible = false;
            // 
            // sTOVESEQDataGridViewTextBoxColumn
            // 
            this.sTOVESEQDataGridViewTextBoxColumn.DataPropertyName = "STOVE_SEQ";
            this.sTOVESEQDataGridViewTextBoxColumn.HeaderText = "STOVE_SEQ";
            this.sTOVESEQDataGridViewTextBoxColumn.Name = "sTOVESEQDataGridViewTextBoxColumn";
            this.sTOVESEQDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTOVESEQDataGridViewTextBoxColumn.Visible = false;
            // 
            // rEMARKDataGridViewTextBoxColumn
            // 
            this.rEMARKDataGridViewTextBoxColumn.DataPropertyName = "REMARK";
            this.rEMARKDataGridViewTextBoxColumn.HeaderText = "REMARK";
            this.rEMARKDataGridViewTextBoxColumn.Name = "rEMARKDataGridViewTextBoxColumn";
            this.rEMARKDataGridViewTextBoxColumn.ReadOnly = true;
            this.rEMARKDataGridViewTextBoxColumn.Visible = false;
            // 
            // machineDownModelBindingSource
            // 
            this.machineDownModelBindingSource.DataSource = typeof(RCOutput.Models.MachineDownModel);
            // 
            // fShift
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 211);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fShift";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shift";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDownModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button BtnSubmit;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpEnd;
        private System.Windows.Forms.DataGridView DgvMachine;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn sTOVESEQDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEMARKDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource machineDownModelBindingSource;
    }
}