namespace IQCbyLot
{
    partial class fAQL
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
            this.lablTypeName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lablLotSize = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.combAQLID = new System.Windows.Forms.ComboBox();
            this.combLevel = new System.Windows.Forms.ComboBox();
            this.combAQL = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lablLotNo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.MIN_LOT_SIZE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAX_LOT_SIZE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAMPLE_SIZE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UNIT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CRITICAL_REJECT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAJOR_REJECT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MINOR_REJECT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAMPLING_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAMPLING_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lablTypeName);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lablLotSize);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.combAQLID);
            this.panel1.Controls.Add(this.combLevel);
            this.panel1.Controls.Add(this.combAQL);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lablLotNo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(559, 159);
            this.panel1.TabIndex = 0;
            // 
            // lablTypeName
            // 
            this.lablTypeName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablTypeName.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablTypeName.ForeColor = System.Drawing.Color.Maroon;
            this.lablTypeName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablTypeName.Location = new System.Drawing.Point(118, 65);
            this.lablTypeName.Name = "lablTypeName";
            this.lablTypeName.Size = new System.Drawing.Size(266, 25);
            this.lablTypeName.TabIndex = 26;
            this.lablTypeName.Text = "N/A";
            this.lablTypeName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(3, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 15);
            this.label5.TabIndex = 25;
            this.label5.Text = "Type Name";
            // 
            // lablLotSize
            // 
            this.lablLotSize.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablLotSize.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablLotSize.ForeColor = System.Drawing.Color.Maroon;
            this.lablLotSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablLotSize.Location = new System.Drawing.Point(118, 38);
            this.lablLotSize.Name = "lablLotSize";
            this.lablLotSize.Size = new System.Drawing.Size(266, 24);
            this.lablLotSize.TabIndex = 24;
            this.lablLotSize.Text = "N/A";
            this.lablLotSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(3, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 23;
            this.label2.Text = "Lot Size";
            // 
            // combAQLID
            // 
            this.combAQLID.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.combAQLID.FormattingEnabled = true;
            this.combAQLID.Location = new System.Drawing.Point(410, 39);
            this.combAQLID.Name = "combAQLID";
            this.combAQLID.Size = new System.Drawing.Size(121, 24);
            this.combAQLID.TabIndex = 22;
            this.combAQLID.Visible = false;
            // 
            // combLevel
            // 
            this.combLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combLevel.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.combLevel.FormattingEnabled = true;
            this.combLevel.Location = new System.Drawing.Point(118, 123);
            this.combLevel.Name = "combLevel";
            this.combLevel.Size = new System.Drawing.Size(266, 23);
            this.combLevel.TabIndex = 21;
            this.combLevel.SelectedIndexChanged += new System.EventHandler(this.cmbAQL_SelectedIndexChanged);
            // 
            // combAQL
            // 
            this.combAQL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combAQL.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.combAQL.FormattingEnabled = true;
            this.combAQL.Location = new System.Drawing.Point(118, 93);
            this.combAQL.Name = "combAQL";
            this.combAQL.Size = new System.Drawing.Size(266, 23);
            this.combAQL.TabIndex = 20;
            this.combAQL.SelectedIndexChanged += new System.EventHandler(this.cmbAQL_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 16;
            this.label1.Text = "Lot No";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(3, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 15);
            this.label4.TabIndex = 19;
            this.label4.Text = "Level";
            // 
            // lablLotNo
            // 
            this.lablLotNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablLotNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablLotNo.ForeColor = System.Drawing.Color.Maroon;
            this.lablLotNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablLotNo.Location = new System.Drawing.Point(118, 9);
            this.lablLotNo.Name = "lablLotNo";
            this.lablLotNo.Size = new System.Drawing.Size(266, 25);
            this.lablLotNo.TabIndex = 17;
            this.lablLotNo.Text = "N/A";
            this.lablLotNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(3, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 18;
            this.label3.Text = "Sample Type";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 384);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(559, 38);
            this.panel2.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Location = new System.Drawing.Point(297, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSave.Location = new System.Drawing.Point(157, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MIN_LOT_SIZE,
            this.MAX_LOT_SIZE,
            this.SAMPLE_SIZE,
            this.UNIT,
            this.CRITICAL_REJECT_QTY,
            this.MAJOR_REJECT_QTY,
            this.MINOR_REJECT_QTY,
            this.SAMPLING_TYPE,
            this.SAMPLING_ID});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 159);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 10;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(559, 225);
            this.dataGridView1.TabIndex = 20;
            // 
            // MIN_LOT_SIZE
            // 
            this.MIN_LOT_SIZE.HeaderText = "Min Size";
            this.MIN_LOT_SIZE.Name = "MIN_LOT_SIZE";
            this.MIN_LOT_SIZE.ReadOnly = true;
            // 
            // MAX_LOT_SIZE
            // 
            this.MAX_LOT_SIZE.HeaderText = "Max Size";
            this.MAX_LOT_SIZE.Name = "MAX_LOT_SIZE";
            this.MAX_LOT_SIZE.ReadOnly = true;
            // 
            // SAMPLE_SIZE
            // 
            this.SAMPLE_SIZE.HeaderText = "Sample Size";
            this.SAMPLE_SIZE.Name = "SAMPLE_SIZE";
            this.SAMPLE_SIZE.ReadOnly = true;
            this.SAMPLE_SIZE.Width = 110;
            // 
            // UNIT
            // 
            this.UNIT.HeaderText = "Unit";
            this.UNIT.Name = "UNIT";
            this.UNIT.ReadOnly = true;
            this.UNIT.Width = 70;
            // 
            // CRITICAL_REJECT_QTY
            // 
            this.CRITICAL_REJECT_QTY.HeaderText = "Critical Qty";
            this.CRITICAL_REJECT_QTY.Name = "CRITICAL_REJECT_QTY";
            this.CRITICAL_REJECT_QTY.ReadOnly = true;
            this.CRITICAL_REJECT_QTY.Width = 105;
            // 
            // MAJOR_REJECT_QTY
            // 
            this.MAJOR_REJECT_QTY.HeaderText = "Major Qty";
            this.MAJOR_REJECT_QTY.Name = "MAJOR_REJECT_QTY";
            this.MAJOR_REJECT_QTY.ReadOnly = true;
            // 
            // MINOR_REJECT_QTY
            // 
            this.MINOR_REJECT_QTY.HeaderText = "Minor Qty";
            this.MINOR_REJECT_QTY.Name = "MINOR_REJECT_QTY";
            this.MINOR_REJECT_QTY.ReadOnly = true;
            // 
            // SAMPLING_TYPE
            // 
            this.SAMPLING_TYPE.HeaderText = "Sampling Type";
            this.SAMPLING_TYPE.Name = "SAMPLING_TYPE";
            this.SAMPLING_TYPE.ReadOnly = true;
            this.SAMPLING_TYPE.Visible = false;
            // 
            // SAMPLING_ID
            // 
            this.SAMPLING_ID.HeaderText = "SAMPLING_ID";
            this.SAMPLING_ID.Name = "SAMPLING_ID";
            this.SAMPLING_ID.ReadOnly = true;
            this.SAMPLING_ID.Visible = false;
            // 
            // fAQL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 422);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fAQL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sampling Plan";
            this.Load += new System.EventHandler(this.fAQL_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox combLevel;
        private System.Windows.Forms.ComboBox combAQL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lablLotNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox combAQLID;
        private System.Windows.Forms.Label lablLotSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lablTypeName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn MIN_LOT_SIZE;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAX_LOT_SIZE;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAMPLE_SIZE;
        private System.Windows.Forms.DataGridViewTextBoxColumn UNIT;
        private System.Windows.Forms.DataGridViewTextBoxColumn CRITICAL_REJECT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAJOR_REJECT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn MINOR_REJECT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAMPLING_TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAMPLING_ID;
    }
}