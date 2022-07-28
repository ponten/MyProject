namespace RCOutput
{
    partial class fMachineChange
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.DgvMachine = new System.Windows.Forms.DataGridView();
            this.machineDownModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BtnRemove = new System.Windows.Forms.Button();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDownModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.DgvMachine, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.BtnRemove, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.BtnOK, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.BtnAdd, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 361);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.sTOVESEQDataGridViewTextBoxColumn,
            this.rEMARKDataGridViewTextBoxColumn});
            this.tableLayoutPanel1.SetColumnSpan(this.DgvMachine, 2);
            this.DgvMachine.DataSource = this.machineDownModelBindingSource;
            this.DgvMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvMachine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvMachine.Location = new System.Drawing.Point(9, 43);
            this.DgvMachine.Margin = new System.Windows.Forms.Padding(4);
            this.DgvMachine.MultiSelect = false;
            this.DgvMachine.Name = "DgvMachine";
            this.DgvMachine.ReadOnly = true;
            this.DgvMachine.RowTemplate.Height = 24;
            this.DgvMachine.Size = new System.Drawing.Size(466, 271);
            this.DgvMachine.TabIndex = 0;
            // 
            // machineDownModelBindingSource
            // 
            this.machineDownModelBindingSource.DataSource = typeof(RCOutput.Models.MachineDownModel);
            // 
            // BtnRemove
            // 
            this.BtnRemove.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnRemove.Location = new System.Drawing.Point(42, 7);
            this.BtnRemove.Margin = new System.Windows.Forms.Padding(2, 2, 50, 2);
            this.BtnRemove.Name = "BtnRemove";
            this.BtnRemove.Size = new System.Drawing.Size(150, 30);
            this.BtnRemove.TabIndex = 0;
            this.BtnRemove.Text = "Remove machine";
            this.BtnRemove.UseVisualStyleBackColor = true;
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnOK.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnOK.Location = new System.Drawing.Point(375, 322);
            this.BtnOK.Margin = new System.Windows.Forms.Padding(4);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(100, 30);
            this.BtnOK.TabIndex = 1;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BtnAdd.Location = new System.Drawing.Point(292, 7);
            this.BtnAdd.Margin = new System.Windows.Forms.Padding(50, 2, 2, 2);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(150, 30);
            this.BtnAdd.TabIndex = 1;
            this.BtnAdd.Text = "Add machine";
            this.BtnAdd.UseVisualStyleBackColor = true;
            // 
            // selectDataGridViewCheckBoxColumn
            // 
            this.selectDataGridViewCheckBoxColumn.DataPropertyName = "Select";
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
            this.mACHINEIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.mACHINEIDDataGridViewTextBoxColumn.Visible = false;
            this.mACHINEIDDataGridViewTextBoxColumn.Width = 108;
            // 
            // mACHINECODEDataGridViewTextBoxColumn
            // 
            this.mACHINECODEDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_CODE";
            this.mACHINECODEDataGridViewTextBoxColumn.HeaderText = "Machine code";
            this.mACHINECODEDataGridViewTextBoxColumn.Name = "mACHINECODEDataGridViewTextBoxColumn";
            this.mACHINECODEDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINECODEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.mACHINECODEDataGridViewTextBoxColumn.Width = 92;
            // 
            // mACHINEDESCDataGridViewTextBoxColumn
            // 
            this.mACHINEDESCDataGridViewTextBoxColumn.DataPropertyName = "MACHINE_DESC";
            this.mACHINEDESCDataGridViewTextBoxColumn.HeaderText = "Machine";
            this.mACHINEDESCDataGridViewTextBoxColumn.Name = "mACHINEDESCDataGridViewTextBoxColumn";
            this.mACHINEDESCDataGridViewTextBoxColumn.ReadOnly = true;
            this.mACHINEDESCDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.mACHINEDESCDataGridViewTextBoxColumn.Width = 68;
            // 
            // sTATUSNAMEDataGridViewTextBoxColumn
            // 
            this.sTATUSNAMEDataGridViewTextBoxColumn.DataPropertyName = "STATUS_NAME";
            this.sTATUSNAMEDataGridViewTextBoxColumn.HeaderText = "STATUS_NAME";
            this.sTATUSNAMEDataGridViewTextBoxColumn.Name = "sTATUSNAMEDataGridViewTextBoxColumn";
            this.sTATUSNAMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTATUSNAMEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sTATUSNAMEDataGridViewTextBoxColumn.Visible = false;
            this.sTATUSNAMEDataGridViewTextBoxColumn.Width = 122;
            // 
            // tYPEIDDataGridViewTextBoxColumn
            // 
            this.tYPEIDDataGridViewTextBoxColumn.DataPropertyName = "TYPE_ID";
            this.tYPEIDDataGridViewTextBoxColumn.HeaderText = "TYPE_ID";
            this.tYPEIDDataGridViewTextBoxColumn.Name = "tYPEIDDataGridViewTextBoxColumn";
            this.tYPEIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.tYPEIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tYPEIDDataGridViewTextBoxColumn.Visible = false;
            this.tYPEIDDataGridViewTextBoxColumn.Width = 75;
            // 
            // rEASONIDDataGridViewTextBoxColumn
            // 
            this.rEASONIDDataGridViewTextBoxColumn.DataPropertyName = "REASON_ID";
            this.rEASONIDDataGridViewTextBoxColumn.HeaderText = "REASON_ID";
            this.rEASONIDDataGridViewTextBoxColumn.Name = "rEASONIDDataGridViewTextBoxColumn";
            this.rEASONIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.rEASONIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rEASONIDDataGridViewTextBoxColumn.Visible = false;
            this.rEASONIDDataGridViewTextBoxColumn.Width = 98;
            // 
            // sTARTTIMEDataGridViewTextBoxColumn
            // 
            this.sTARTTIMEDataGridViewTextBoxColumn.DataPropertyName = "START_TIME";
            this.sTARTTIMEDataGridViewTextBoxColumn.HeaderText = "START_TIME";
            this.sTARTTIMEDataGridViewTextBoxColumn.Name = "sTARTTIMEDataGridViewTextBoxColumn";
            this.sTARTTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTARTTIMEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sTARTTIMEDataGridViewTextBoxColumn.Visible = false;
            this.sTARTTIMEDataGridViewTextBoxColumn.Width = 105;
            // 
            // eNDTIMEDataGridViewTextBoxColumn
            // 
            this.eNDTIMEDataGridViewTextBoxColumn.DataPropertyName = "END_TIME";
            this.eNDTIMEDataGridViewTextBoxColumn.HeaderText = "END_TIME";
            this.eNDTIMEDataGridViewTextBoxColumn.Name = "eNDTIMEDataGridViewTextBoxColumn";
            this.eNDTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.eNDTIMEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.eNDTIMEDataGridViewTextBoxColumn.Visible = false;
            this.eNDTIMEDataGridViewTextBoxColumn.Width = 89;
            // 
            // lOADQTYDataGridViewTextBoxColumn
            // 
            this.lOADQTYDataGridViewTextBoxColumn.DataPropertyName = "LOAD_QTY";
            this.lOADQTYDataGridViewTextBoxColumn.HeaderText = "Load";
            this.lOADQTYDataGridViewTextBoxColumn.Name = "lOADQTYDataGridViewTextBoxColumn";
            this.lOADQTYDataGridViewTextBoxColumn.ReadOnly = true;
            this.lOADQTYDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.lOADQTYDataGridViewTextBoxColumn.Width = 46;
            // 
            // dATECODEDataGridViewTextBoxColumn
            // 
            this.dATECODEDataGridViewTextBoxColumn.DataPropertyName = "DATE_CODE";
            this.dATECODEDataGridViewTextBoxColumn.HeaderText = "DateCode";
            this.dATECODEDataGridViewTextBoxColumn.Name = "dATECODEDataGridViewTextBoxColumn";
            this.dATECODEDataGridViewTextBoxColumn.ReadOnly = true;
            this.dATECODEDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dATECODEDataGridViewTextBoxColumn.Visible = false;
            this.dATECODEDataGridViewTextBoxColumn.Width = 76;
            // 
            // sTOVESEQDataGridViewTextBoxColumn
            // 
            this.sTOVESEQDataGridViewTextBoxColumn.DataPropertyName = "STOVE_SEQ";
            this.sTOVESEQDataGridViewTextBoxColumn.HeaderText = "Stove sequence";
            this.sTOVESEQDataGridViewTextBoxColumn.Name = "sTOVESEQDataGridViewTextBoxColumn";
            this.sTOVESEQDataGridViewTextBoxColumn.ReadOnly = true;
            this.sTOVESEQDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sTOVESEQDataGridViewTextBoxColumn.Visible = false;
            // 
            // rEMARKDataGridViewTextBoxColumn
            // 
            this.rEMARKDataGridViewTextBoxColumn.DataPropertyName = "REMARK";
            this.rEMARKDataGridViewTextBoxColumn.HeaderText = "REMARK";
            this.rEMARKDataGridViewTextBoxColumn.Name = "rEMARKDataGridViewTextBoxColumn";
            this.rEMARKDataGridViewTextBoxColumn.ReadOnly = true;
            this.rEMARKDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rEMARKDataGridViewTextBoxColumn.Visible = false;
            this.rEMARKDataGridViewTextBoxColumn.Width = 78;
            // 
            // fChangeMachine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fChangeMachine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Machine in production";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineDownModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button BtnRemove;
        private System.Windows.Forms.DataGridView DgvMachine;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEASONCODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEASONDESCDataGridViewTextBoxColumn;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn sTOVESEQDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEMARKDataGridViewTextBoxColumn;
    }
}