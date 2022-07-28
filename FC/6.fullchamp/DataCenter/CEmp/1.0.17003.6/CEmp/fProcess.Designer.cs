namespace CEmp
{
    partial class fProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fProcess));
            this.tlpFormGrid = new System.Windows.Forms.TableLayoutPanel();
            this.lbEMP = new System.Windows.Forms.Label();
            this.dgvProcess = new System.Windows.Forms.DataGridView();
            this.CHECK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PROCESS_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROCESS_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDeSelect = new System.Windows.Forms.Button();
            this.tlpFormGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcess)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFormGrid
            // 
            this.tlpFormGrid.ColumnCount = 1;
            this.tlpFormGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFormGrid.Controls.Add(this.lbEMP, 0, 0);
            this.tlpFormGrid.Controls.Add(this.dgvProcess, 0, 1);
            this.tlpFormGrid.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tlpFormGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFormGrid.Location = new System.Drawing.Point(0, 0);
            this.tlpFormGrid.Name = "tlpFormGrid";
            this.tlpFormGrid.RowCount = 3;
            this.tlpFormGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFormGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFormGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFormGrid.Size = new System.Drawing.Size(432, 453);
            this.tlpFormGrid.TabIndex = 0;
            // 
            // lbEMP
            // 
            this.lbEMP.AutoSize = true;
            this.lbEMP.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbEMP.Location = new System.Drawing.Point(3, 0);
            this.lbEMP.Name = "lbEMP";
            this.lbEMP.Size = new System.Drawing.Size(86, 15);
            this.lbEMP.TabIndex = 1;
            this.lbEMP.Text = "EMP_NAME";
            this.lbEMP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvProcess
            // 
            this.dgvProcess.AllowUserToAddRows = false;
            this.dgvProcess.AllowUserToDeleteRows = false;
            this.dgvProcess.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvProcess.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProcess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CHECK,
            this.PROCESS_ID,
            this.PROCESS_NAME});
            this.dgvProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProcess.Location = new System.Drawing.Point(3, 18);
            this.dgvProcess.MultiSelect = false;
            this.dgvProcess.Name = "dgvProcess";
            this.dgvProcess.RowHeadersWidth = 40;
            this.dgvProcess.RowTemplate.Height = 27;
            this.dgvProcess.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvProcess.Size = new System.Drawing.Size(426, 372);
            this.dgvProcess.TabIndex = 0;
            // 
            // CHECK
            // 
            this.CHECK.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CHECK.DataPropertyName = "CHECK";
            this.CHECK.FalseValue = "0";
            this.CHECK.HeaderText = "SELECT";
            this.CHECK.MinimumWidth = 6;
            this.CHECK.Name = "CHECK";
            this.CHECK.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CHECK.TrueValue = "1";
            this.CHECK.Width = 66;
            // 
            // PROCESS_ID
            // 
            this.PROCESS_ID.DataPropertyName = "PROCESS_ID";
            this.PROCESS_ID.HeaderText = "PROCESS_ID";
            this.PROCESS_ID.MinimumWidth = 6;
            this.PROCESS_ID.Name = "PROCESS_ID";
            this.PROCESS_ID.ReadOnly = true;
            this.PROCESS_ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PROCESS_ID.Visible = false;
            this.PROCESS_ID.Width = 119;
            // 
            // PROCESS_NAME
            // 
            this.PROCESS_NAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PROCESS_NAME.DataPropertyName = "PROCESS_NAME";
            this.PROCESS_NAME.HeaderText = "Process";
            this.PROCESS_NAME.MinimumWidth = 6;
            this.PROCESS_NAME.Name = "PROCESS_NAME";
            this.PROCESS_NAME.ReadOnly = true;
            this.PROCESS_NAME.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 396);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(426, 54);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnOK);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(209, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(214, 48);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.Location = new System.Drawing.Point(111, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOK.Location = new System.Drawing.Point(5, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnDeSelect);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(200, 48);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // btnDeSelect
            // 
            this.btnDeSelect.Location = new System.Drawing.Point(3, 3);
            this.btnDeSelect.Name = "btnDeSelect";
            this.btnDeSelect.Size = new System.Drawing.Size(100, 30);
            this.btnDeSelect.TabIndex = 3;
            this.btnDeSelect.Text = "Deselect All";
            this.btnDeSelect.UseVisualStyleBackColor = true;
            this.btnDeSelect.Click += new System.EventHandler(this.BtnDeSelect_Click);
            // 
            // fProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 453);
            this.Controls.Add(this.tlpFormGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(375, 500);
            this.Name = "fProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Production-Reporting Authority";
            this.Load += new System.EventHandler(this.FProcess_Load);
            this.tlpFormGrid.ResumeLayout(false);
            this.tlpFormGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcess)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFormGrid;
        private System.Windows.Forms.Label lbEMP;
        private System.Windows.Forms.DataGridView dgvProcess;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CHECK;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROCESS_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROCESS_NAME;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnDeSelect;
    }
}