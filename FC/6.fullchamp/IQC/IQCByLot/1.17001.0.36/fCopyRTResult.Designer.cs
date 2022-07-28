namespace IQCbyLot
{
    partial class fCopyRTResult
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvItemValue = new System.Windows.Forms.DataGridView();
            this.ITEM_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEM_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvItemNoValue = new System.Windows.Forms.DataGridView();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.label3 = new System.Windows.Forms.Label();
            this.editRTNo = new System.Windows.Forms.TextBox();
            this.btnFilterRT = new System.Windows.Forms.Button();
            this.combRTSeq = new System.Windows.Forms.ComboBox();
            this.lablTypeName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ITEM_CODE1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEM_NAME1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASS_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FAIL_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEM_MEMO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.lablPartNo = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemValue)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemNoValue)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lablPartNo);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lablTypeName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.combRTSeq);
            this.panel1.Controls.Add(this.btnFilterRT);
            this.panel1.Controls.Add(this.editRTNo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(655, 76);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 424);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(655, 38);
            this.panel2.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(569, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(473, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvItemValue);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(0, 275);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(655, 149);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Test Item for Value";
            // 
            // dgvItemValue
            // 
            this.dgvItemValue.AllowUserToAddRows = false;
            this.dgvItemValue.AllowUserToDeleteRows = false;
            this.dgvItemValue.BackgroundColor = System.Drawing.Color.White;
            this.dgvItemValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemValue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ITEM_CODE,
            this.ITEM_NAME});
            this.dgvItemValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItemValue.EnableHeadersVisualStyles = false;
            this.dgvItemValue.Location = new System.Drawing.Point(3, 21);
            this.dgvItemValue.MultiSelect = false;
            this.dgvItemValue.Name = "dgvItemValue";
            this.dgvItemValue.ReadOnly = true;
            this.dgvItemValue.RowHeadersWidth = 25;
            this.dgvItemValue.RowTemplate.Height = 24;
            this.dgvItemValue.Size = new System.Drawing.Size(649, 125);
            this.dgvItemValue.TabIndex = 55;
            // 
            // ITEM_CODE
            // 
            this.ITEM_CODE.HeaderText = "Item Code";
            this.ITEM_CODE.Name = "ITEM_CODE";
            this.ITEM_CODE.ReadOnly = true;
            this.ITEM_CODE.Width = 130;
            // 
            // ITEM_NAME
            // 
            this.ITEM_NAME.HeaderText = "Item Name";
            this.ITEM_NAME.Name = "ITEM_NAME";
            this.ITEM_NAME.ReadOnly = true;
            this.ITEM_NAME.Width = 150;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 272);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(655, 3);
            this.splitter1.TabIndex = 60;
            this.splitter1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(0, 76);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(655, 196);
            this.tabControl1.TabIndex = 62;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.splitter2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(647, 167);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Inspect Item";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvItemNoValue);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(641, 158);
            this.groupBox2.TabIndex = 60;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "All Test Item";
            // 
            // dgvItemNoValue
            // 
            this.dgvItemNoValue.AllowUserToAddRows = false;
            this.dgvItemNoValue.AllowUserToDeleteRows = false;
            this.dgvItemNoValue.BackgroundColor = System.Drawing.Color.White;
            this.dgvItemNoValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemNoValue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ITEM_CODE1,
            this.ITEM_NAME1,
            this.PASS_QTY,
            this.FAIL_QTY,
            this.ITEM_MEMO});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItemNoValue.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvItemNoValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItemNoValue.EnableHeadersVisualStyles = false;
            this.dgvItemNoValue.Location = new System.Drawing.Point(3, 21);
            this.dgvItemNoValue.MultiSelect = false;
            this.dgvItemNoValue.Name = "dgvItemNoValue";
            this.dgvItemNoValue.ReadOnly = true;
            this.dgvItemNoValue.RowHeadersWidth = 25;
            this.dgvItemNoValue.RowTemplate.Height = 24;
            this.dgvItemNoValue.Size = new System.Drawing.Size(635, 134);
            this.dgvItemNoValue.TabIndex = 57;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(3, 161);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(641, 3);
            this.splitter2.TabIndex = 59;
            this.splitter2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(9, 42);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 15);
            this.label3.TabIndex = 46;
            this.label3.Text = "Copy From RT No";
            // 
            // editRTNo
            // 
            this.editRTNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editRTNo.Location = new System.Drawing.Point(148, 37);
            this.editRTNo.Name = "editRTNo";
            this.editRTNo.Size = new System.Drawing.Size(184, 25);
            this.editRTNo.TabIndex = 47;
            this.editRTNo.Tag = "2";
            this.editRTNo.TextChanged += new System.EventHandler(this.editRTNo_TextChanged);
            this.editRTNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editRTNo_KeyPress);
            // 
            // btnFilterRT
            // 
            this.btnFilterRT.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFilterRT.Location = new System.Drawing.Point(338, 38);
            this.btnFilterRT.Name = "btnFilterRT";
            this.btnFilterRT.Size = new System.Drawing.Size(30, 23);
            this.btnFilterRT.TabIndex = 55;
            this.btnFilterRT.Text = "...";
            this.btnFilterRT.UseVisualStyleBackColor = true;
            this.btnFilterRT.Click += new System.EventHandler(this.btnFilterRT_Click);
            // 
            // combRTSeq
            // 
            this.combRTSeq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combRTSeq.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.combRTSeq.FormattingEnabled = true;
            this.combRTSeq.Location = new System.Drawing.Point(464, 38);
            this.combRTSeq.Name = "combRTSeq";
            this.combRTSeq.Size = new System.Drawing.Size(67, 23);
            this.combRTSeq.TabIndex = 56;
            this.combRTSeq.SelectedIndexChanged += new System.EventHandler(this.combRTSeq_SelectedIndexChanged);
            // 
            // lablTypeName
            // 
            this.lablTypeName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablTypeName.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablTypeName.ForeColor = System.Drawing.Color.Maroon;
            this.lablTypeName.Location = new System.Drawing.Point(148, 9);
            this.lablTypeName.Name = "lablTypeName";
            this.lablTypeName.Size = new System.Drawing.Size(185, 25);
            this.lablTypeName.TabIndex = 58;
            this.lablTypeName.Text = "N/A";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 15);
            this.label1.TabIndex = 57;
            this.label1.Text = "Type Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(375, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 15);
            this.label2.TabIndex = 59;
            this.label2.Text = "RT Seq";
            // 
            // ITEM_CODE1
            // 
            this.ITEM_CODE1.HeaderText = "Item Code";
            this.ITEM_CODE1.Name = "ITEM_CODE1";
            this.ITEM_CODE1.ReadOnly = true;
            this.ITEM_CODE1.Width = 130;
            // 
            // ITEM_NAME1
            // 
            this.ITEM_NAME1.HeaderText = "Item Name";
            this.ITEM_NAME1.Name = "ITEM_NAME1";
            this.ITEM_NAME1.ReadOnly = true;
            this.ITEM_NAME1.Width = 150;
            // 
            // PASS_QTY
            // 
            this.PASS_QTY.HeaderText = "Pass Qty";
            this.PASS_QTY.Name = "PASS_QTY";
            this.PASS_QTY.ReadOnly = true;
            // 
            // FAIL_QTY
            // 
            this.FAIL_QTY.HeaderText = "Fail Qty";
            this.FAIL_QTY.Name = "FAIL_QTY";
            this.FAIL_QTY.ReadOnly = true;
            // 
            // ITEM_MEMO
            // 
            this.ITEM_MEMO.HeaderText = "Memo";
            this.ITEM_MEMO.Name = "ITEM_MEMO";
            this.ITEM_MEMO.ReadOnly = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(375, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 15);
            this.label4.TabIndex = 60;
            this.label4.Text = "Part No";
            // 
            // lablPartNo
            // 
            this.lablPartNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablPartNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablPartNo.ForeColor = System.Drawing.Color.Maroon;
            this.lablPartNo.Location = new System.Drawing.Point(466, 9);
            this.lablPartNo.Name = "lablPartNo";
            this.lablPartNo.Size = new System.Drawing.Size(185, 25);
            this.lablPartNo.TabIndex = 61;
            this.lablPartNo.Text = "N/A";
            // 
            // fCopyRTResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 462);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fCopyRTResult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copy Result";
            this.Load += new System.EventHandler(this.fCopyRTResult_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemValue)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemNoValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvItemValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_NAME;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvItemNoValue;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editRTNo;
        private System.Windows.Forms.Button btnFilterRT;
        private System.Windows.Forms.ComboBox combRTSeq;
        private System.Windows.Forms.Label lablTypeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_CODE1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_NAME1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASS_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn FAIL_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_MEMO;
        private System.Windows.Forms.Label lablPartNo;
        private System.Windows.Forms.Label label4;
    }
}