namespace CMachine
{
    partial class fDetailData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.combYear = new System.Windows.Forms.ComboBox();
            this.editYear = new System.Windows.Forms.TextBox();
            this.lablUnit = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.lablValueTitle = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.editValue = new System.Windows.Forms.TextBox();
            this.labMachineCode = new System.Windows.Forms.Label();
            this.LabName = new System.Windows.Forms.Label();
            this.btnPreview = new System.Windows.Forms.Button();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.PanelMonth = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.YEAR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MONTH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DAY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VALUE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.combYear);
            this.panel2.Controls.Add(this.editYear);
            this.panel2.Controls.Add(this.lablUnit);
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.lablValueTitle);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.dateTimePicker4);
            this.panel2.Controls.Add(this.editValue);
            this.panel2.Controls.Add(this.labMachineCode);
            this.panel2.Controls.Add(this.LabName);
            this.panel2.Controls.Add(this.btnPreview);
            this.panel2.Controls.Add(this.dateTimePicker3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnReset);
            this.panel2.Controls.Add(this.btnQuery);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(839, 60);
            this.panel2.TabIndex = 3;
            // 
            // combYear
            // 
            this.combYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combYear.FormattingEnabled = true;
            this.combYear.Items.AddRange(new object[] {
            "2009",
            "2010",
            "2011"});
            this.combYear.Location = new System.Drawing.Point(253, 5);
            this.combYear.Name = "combYear";
            this.combYear.Size = new System.Drawing.Size(121, 20);
            this.combYear.TabIndex = 52;
            this.combYear.SelectedIndexChanged += new System.EventHandler(this.combYear_SelectedIndexChanged);
            // 
            // editYear
            // 
            this.editYear.Location = new System.Drawing.Point(247, 6);
            this.editYear.Name = "editYear";
            this.editYear.Size = new System.Drawing.Size(117, 22);
            this.editYear.TabIndex = 51;
            this.editYear.Visible = false;
            // 
            // lablUnit
            // 
            this.lablUnit.AutoSize = true;
            this.lablUnit.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablUnit.Location = new System.Drawing.Point(637, 37);
            this.lablUnit.Name = "lablUnit";
            this.lablUnit.Size = new System.Drawing.Size(38, 13);
            this.lablUnit.TabIndex = 50;
            this.lablUnit.Text = "(Min.)";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(393, 34);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(52, 23);
            this.btnClear.TabIndex = 49;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lablValueTitle
            // 
            this.lablValueTitle.AutoSize = true;
            this.lablValueTitle.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablValueTitle.Location = new System.Drawing.Point(447, 37);
            this.lablValueTitle.Name = "lablValueTitle";
            this.lablValueTitle.Size = new System.Drawing.Size(78, 13);
            this.lablValueTitle.TabIndex = 48;
            this.lablValueTitle.Text = "Working Time";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(226, 32);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 13);
            this.label11.TabIndex = 47;
            this.label11.Text = "~";
            // 
            // dateTimePicker4
            // 
            this.dateTimePicker4.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker4.Location = new System.Drawing.Point(253, 32);
            this.dateTimePicker4.Name = "dateTimePicker4";
            this.dateTimePicker4.Size = new System.Drawing.Size(138, 23);
            this.dateTimePicker4.TabIndex = 46;
            // 
            // editValue
            // 
            this.editValue.Location = new System.Drawing.Point(531, 34);
            this.editValue.Name = "editValue";
            this.editValue.Size = new System.Drawing.Size(100, 22);
            this.editValue.TabIndex = 45;
            // 
            // labMachineCode
            // 
            this.labMachineCode.AutoEllipsis = true;
            this.labMachineCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labMachineCode.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labMachineCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labMachineCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labMachineCode.Location = new System.Drawing.Point(81, 2);
            this.labMachineCode.Name = "labMachineCode";
            this.labMachineCode.Size = new System.Drawing.Size(138, 25);
            this.labMachineCode.TabIndex = 43;
            this.labMachineCode.Text = "N/A";
            this.labMachineCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabName
            // 
            this.LabName.AutoSize = true;
            this.LabName.BackColor = System.Drawing.Color.Transparent;
            this.LabName.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.LabName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabName.Location = new System.Drawing.Point(3, 9);
            this.LabName.Name = "LabName";
            this.LabName.Size = new System.Drawing.Size(76, 13);
            this.LabName.TabIndex = 12;
            this.LabName.Text = "Machine Code";
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(739, 34);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(52, 23);
            this.btnPreview.TabIndex = 6;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Visible = false;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker3.Location = new System.Drawing.Point(82, 32);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(138, 23);
            this.dateTimePicker3.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(4, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Base Date";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(681, 34);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(52, 23);
            this.btnReset.TabIndex = 11;
            this.btnReset.Text = "Save";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(393, 6);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(52, 23);
            this.btnQuery.TabIndex = 10;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Visible = false;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(217, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Year";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 577);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(839, 32);
            this.panel1.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(745, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(664, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Save";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // PanelMonth
            // 
            this.PanelMonth.AutoScroll = true;
            this.PanelMonth.BackColor = System.Drawing.Color.Transparent;
            this.PanelMonth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelMonth.Location = new System.Drawing.Point(3, 3);
            this.PanelMonth.Name = "PanelMonth";
            this.PanelMonth.Size = new System.Drawing.Size(825, 486);
            this.PanelMonth.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 60);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(839, 517);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.PanelMonth);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(831, 492);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Calendar";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvData);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(831, 492);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Data";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.YEAR,
            this.MONTH,
            this.DAY,
            this.VALUE});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(3, 3);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersWidth = 10;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.Size = new System.Drawing.Size(825, 486);
            this.dgvData.TabIndex = 1;
            this.dgvData.TabStop = false;
            this.dgvData.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellDoubleClick_1);
            // 
            // YEAR
            // 
            this.YEAR.HeaderText = "Year";
            this.YEAR.Name = "YEAR";
            this.YEAR.ReadOnly = true;
            this.YEAR.Width = 140;
            // 
            // MONTH
            // 
            this.MONTH.HeaderText = "Month";
            this.MONTH.Name = "MONTH";
            this.MONTH.ReadOnly = true;
            // 
            // DAY
            // 
            this.DAY.HeaderText = "Day";
            this.DAY.Name = "DAY";
            this.DAY.ReadOnly = true;
            // 
            // VALUE
            // 
            this.VALUE.HeaderText = "Value";
            this.VALUE.Name = "VALUE";
            this.VALUE.ReadOnly = true;
            this.VALUE.Width = 140;
            // 
            // fDetailData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 609);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "fDetailData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Maintain Plan";
            this.Load += new System.EventHandler(this.fDetailData_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel PanelMonth;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label LabName;
        private System.Windows.Forms.Label labMachineCode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dateTimePicker4;
        private System.Windows.Forms.TextBox editValue;
        private System.Windows.Forms.Label lablValueTitle;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.DataGridViewTextBoxColumn YEAR;
        private System.Windows.Forms.DataGridViewTextBoxColumn MONTH;
        private System.Windows.Forms.DataGridViewTextBoxColumn DAY;
        private System.Windows.Forms.DataGridViewTextBoxColumn VALUE;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lablUnit;
        private System.Windows.Forms.TextBox editYear;
        private System.Windows.Forms.ComboBox combYear;
    }
}