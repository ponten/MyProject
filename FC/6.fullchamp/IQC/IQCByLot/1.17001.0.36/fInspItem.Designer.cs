namespace IQCbyLot
{
    partial class fInspItem
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCopyResult = new System.Windows.Forms.Button();
            this.lablTypeName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lablMinInspQty = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvTestItemSpec = new System.Windows.Forms.DataGridView();
            this.LSL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LCL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UCL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UNIT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lablSampleSize = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lablResult = new System.Windows.Forms.Label();
            this.combItemID = new System.Windows.Forms.ComboBox();
            this.combItemName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.PanelMain = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.PanelNoValue = new System.Windows.Forms.Panel();
            this.editMemo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.editFailQty = new System.Windows.Forms.TextBox();
            this.editPassQty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestItemSpec)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.PanelNoValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCopyResult);
            this.panel1.Controls.Add(this.lablTypeName);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lablMinInspQty);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.dgvTestItemSpec);
            this.panel1.Controls.Add(this.lablSampleSize);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lablResult);
            this.panel1.Controls.Add(this.combItemID);
            this.panel1.Controls.Add(this.combItemName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(471, 180);
            this.panel1.TabIndex = 0;
            // 
            // btnCopyResult
            // 
            this.btnCopyResult.Location = new System.Drawing.Point(4, 93);
            this.btnCopyResult.Name = "btnCopyResult";
            this.btnCopyResult.Size = new System.Drawing.Size(106, 25);
            this.btnCopyResult.TabIndex = 68;
            this.btnCopyResult.Text = "Copy Result";
            this.btnCopyResult.UseVisualStyleBackColor = true;
            this.btnCopyResult.Click += new System.EventHandler(this.btnCopyResult_Click);
            // 
            // lablTypeName
            // 
            this.lablTypeName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablTypeName.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablTypeName.ForeColor = System.Drawing.Color.Maroon;
            this.lablTypeName.Location = new System.Drawing.Point(117, 41);
            this.lablTypeName.Name = "lablTypeName";
            this.lablTypeName.Size = new System.Drawing.Size(321, 25);
            this.lablTypeName.TabIndex = 67;
            this.lablTypeName.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(4, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 15);
            this.label6.TabIndex = 66;
            this.label6.Text = "Type Name";
            // 
            // lablMinInspQty
            // 
            this.lablMinInspQty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablMinInspQty.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablMinInspQty.ForeColor = System.Drawing.Color.Maroon;
            this.lablMinInspQty.Location = new System.Drawing.Point(335, 9);
            this.lablMinInspQty.Name = "lablMinInspQty";
            this.lablMinInspQty.Size = new System.Drawing.Size(103, 25);
            this.lablMinInspQty.TabIndex = 65;
            this.lablMinInspQty.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(229, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 15);
            this.label5.TabIndex = 64;
            this.label5.Text = "Min Insp Qty";
            // 
            // dgvTestItemSpec
            // 
            this.dgvTestItemSpec.AllowUserToAddRows = false;
            this.dgvTestItemSpec.AllowUserToDeleteRows = false;
            this.dgvTestItemSpec.BackgroundColor = System.Drawing.Color.White;
            this.dgvTestItemSpec.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvTestItemSpec.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvTestItemSpec.ColumnHeadersHeight = 25;
            this.dgvTestItemSpec.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LSL,
            this.LCL,
            this.CL,
            this.UCL,
            this.USL,
            this.UNIT});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTestItemSpec.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTestItemSpec.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvTestItemSpec.EnableHeadersVisualStyles = false;
            this.dgvTestItemSpec.Location = new System.Drawing.Point(0, 122);
            this.dgvTestItemSpec.MultiSelect = false;
            this.dgvTestItemSpec.Name = "dgvTestItemSpec";
            this.dgvTestItemSpec.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTestItemSpec.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTestItemSpec.RowHeadersVisible = false;
            this.dgvTestItemSpec.RowTemplate.Height = 24;
            this.dgvTestItemSpec.RowTemplate.ReadOnly = true;
            this.dgvTestItemSpec.Size = new System.Drawing.Size(471, 58);
            this.dgvTestItemSpec.TabIndex = 63;
            // 
            // LSL
            // 
            this.LSL.HeaderText = "LSL";
            this.LSL.Name = "LSL";
            this.LSL.ReadOnly = true;
            this.LSL.Width = 75;
            // 
            // LCL
            // 
            this.LCL.HeaderText = "LCL";
            this.LCL.Name = "LCL";
            this.LCL.ReadOnly = true;
            this.LCL.Width = 75;
            // 
            // CL
            // 
            this.CL.HeaderText = "CL";
            this.CL.Name = "CL";
            this.CL.ReadOnly = true;
            this.CL.Width = 75;
            // 
            // UCL
            // 
            this.UCL.HeaderText = "UCL";
            this.UCL.Name = "UCL";
            this.UCL.ReadOnly = true;
            this.UCL.Width = 75;
            // 
            // USL
            // 
            this.USL.HeaderText = "USL";
            this.USL.Name = "USL";
            this.USL.ReadOnly = true;
            this.USL.Width = 75;
            // 
            // UNIT
            // 
            this.UNIT.HeaderText = "Unit";
            this.UNIT.Name = "UNIT";
            this.UNIT.ReadOnly = true;
            this.UNIT.Width = 50;
            // 
            // lablSampleSize
            // 
            this.lablSampleSize.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablSampleSize.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablSampleSize.ForeColor = System.Drawing.Color.Maroon;
            this.lablSampleSize.Location = new System.Drawing.Point(116, 10);
            this.lablSampleSize.Name = "lablSampleSize";
            this.lablSampleSize.Size = new System.Drawing.Size(103, 25);
            this.lablSampleSize.TabIndex = 27;
            this.lablSampleSize.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(4, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 15);
            this.label2.TabIndex = 26;
            this.label2.Text = "Sample Size";
            // 
            // lablResult
            // 
            this.lablResult.AutoSize = true;
            this.lablResult.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablResult.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablResult.Location = new System.Drawing.Point(291, 4);
            this.lablResult.Name = "lablResult";
            this.lablResult.Size = new System.Drawing.Size(47, 16);
            this.lablResult.TabIndex = 25;
            this.lablResult.Text = "Result";
            this.lablResult.Visible = false;
            // 
            // combItemID
            // 
            this.combItemID.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.combItemID.FormattingEnabled = true;
            this.combItemID.Location = new System.Drawing.Point(260, 4);
            this.combItemID.Name = "combItemID";
            this.combItemID.Size = new System.Drawing.Size(158, 23);
            this.combItemID.TabIndex = 2;
            this.combItemID.Visible = false;
            // 
            // combItemName
            // 
            this.combItemName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combItemName.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.combItemName.FormattingEnabled = true;
            this.combItemName.Location = new System.Drawing.Point(116, 69);
            this.combItemName.Name = "combItemName";
            this.combItemName.Size = new System.Drawing.Size(322, 23);
            this.combItemName.TabIndex = 1;
            this.combItemName.SelectedIndexChanged += new System.EventHandler(this.combItemName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(4, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item Name";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 388);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(471, 40);
            this.panel2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Location = new System.Drawing.Point(225, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSave.Location = new System.Drawing.Point(117, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // PanelMain
            // 
            this.PanelMain.AutoScroll = true;
            this.PanelMain.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.PanelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelMain.Location = new System.Drawing.Point(0, 0);
            this.PanelMain.Name = "PanelMain";
            this.PanelMain.Size = new System.Drawing.Size(471, 208);
            this.PanelMain.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.PanelNoValue);
            this.panel3.Controls.Add(this.PanelMain);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 180);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(471, 208);
            this.panel3.TabIndex = 3;
            // 
            // PanelNoValue
            // 
            this.PanelNoValue.AutoScroll = true;
            this.PanelNoValue.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.PanelNoValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelNoValue.Controls.Add(this.editMemo);
            this.PanelNoValue.Controls.Add(this.label7);
            this.PanelNoValue.Controls.Add(this.editFailQty);
            this.PanelNoValue.Controls.Add(this.editPassQty);
            this.PanelNoValue.Controls.Add(this.label4);
            this.PanelNoValue.Controls.Add(this.label3);
            this.PanelNoValue.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelNoValue.Location = new System.Drawing.Point(0, 90);
            this.PanelNoValue.Name = "PanelNoValue";
            this.PanelNoValue.Size = new System.Drawing.Size(471, 118);
            this.PanelNoValue.TabIndex = 3;
            this.PanelNoValue.Visible = false;
            // 
            // editMemo
            // 
            this.editMemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.editMemo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editMemo.Location = new System.Drawing.Point(25, 78);
            this.editMemo.Name = "editMemo";
            this.editMemo.Size = new System.Drawing.Size(244, 25);
            this.editMemo.TabIndex = 5;
            this.editMemo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editMemo_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label7.Location = new System.Drawing.Point(22, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "Memo";
            // 
            // editFailQty
            // 
            this.editFailQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.editFailQty.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editFailQty.Location = new System.Drawing.Point(169, 29);
            this.editFailQty.Name = "editFailQty";
            this.editFailQty.Size = new System.Drawing.Size(100, 25);
            this.editFailQty.TabIndex = 3;
            this.editFailQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editFailQty_KeyPress);
            // 
            // editPassQty
            // 
            this.editPassQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.editPassQty.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editPassQty.Location = new System.Drawing.Point(25, 29);
            this.editPassQty.Name = "editPassQty";
            this.editPassQty.Size = new System.Drawing.Size(100, 25);
            this.editPassQty.TabIndex = 2;
            this.editPassQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editPassQty_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label4.Location = new System.Drawing.Point(166, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Fail Qty";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label3.Location = new System.Drawing.Point(22, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Pass Qty";
            // 
            // fInspItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 428);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fInspItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inspect Item";
            this.Load += new System.EventHandler(this.fInspItem_Load);
            this.Shown += new System.EventHandler(this.fInspItem_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestItemSpec)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.PanelNoValue.ResumeLayout(false);
            this.PanelNoValue.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox combItemName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox combItemID;
        private System.Windows.Forms.Panel PanelMain;
        public System.Windows.Forms.Label lablResult;
        private System.Windows.Forms.Label lablSampleSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel PanelNoValue;
        private System.Windows.Forms.TextBox editFailQty;
        private System.Windows.Forms.TextBox editPassQty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvTestItemSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn LSL;
        private System.Windows.Forms.DataGridViewTextBoxColumn LCL;
        private System.Windows.Forms.DataGridViewTextBoxColumn CL;
        private System.Windows.Forms.DataGridViewTextBoxColumn UCL;
        private System.Windows.Forms.DataGridViewTextBoxColumn USL;
        private System.Windows.Forms.DataGridViewTextBoxColumn UNIT;
        private System.Windows.Forms.Label lablMinInspQty;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lablTypeName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox editMemo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCopyResult;
    }
}