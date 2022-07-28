namespace IQCbyLot
{
    partial class fInspHistoryDetail
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lablTypeName = new System.Windows.Forms.Label();
            this.lablLotNo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvItemNoValue = new System.Windows.Forms.DataGridView();
            this.ITEM_CODE1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEM_NAME1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASS_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FAIL_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvItemValue = new System.Windows.Forms.DataGridView();
            this.ITEM_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEM_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvDefect = new System.Windows.Forms.DataGridView();
            this.DEFECT_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEFECT_DESC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEFECT_DESC2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEFECT_LEVEL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEFECT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEFECT_MEMO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemNoValue)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemValue)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lablTypeName);
            this.panel1.Controls.Add(this.lablLotNo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(751, 41);
            this.panel1.TabIndex = 0;
            // 
            // lablTypeName
            // 
            this.lablTypeName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablTypeName.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablTypeName.ForeColor = System.Drawing.Color.Maroon;
            this.lablTypeName.Location = new System.Drawing.Point(437, 9);
            this.lablTypeName.Name = "lablTypeName";
            this.lablTypeName.Size = new System.Drawing.Size(185, 25);
            this.lablTypeName.TabIndex = 48;
            this.lablTypeName.Text = "N/A";
            // 
            // lablLotNo
            // 
            this.lablLotNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablLotNo.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablLotNo.ForeColor = System.Drawing.Color.Maroon;
            this.lablLotNo.Location = new System.Drawing.Point(106, 9);
            this.lablLotNo.Name = "lablLotNo";
            this.lablLotNo.Size = new System.Drawing.Size(199, 25);
            this.lablLotNo.TabIndex = 47;
            this.lablLotNo.Text = "N/A";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(330, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 15);
            this.label1.TabIndex = 46;
            this.label1.Text = "Type Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(13, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 15);
            this.label3.TabIndex = 45;
            this.label3.Text = "Lot No";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(0, 41);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(751, 454);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.splitter1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(743, 425);
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
            this.groupBox2.Size = new System.Drawing.Size(737, 198);
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
            this.FAIL_QTY});
            this.dgvItemNoValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItemNoValue.EnableHeadersVisualStyles = false;
            this.dgvItemNoValue.Location = new System.Drawing.Point(3, 21);
            this.dgvItemNoValue.MultiSelect = false;
            this.dgvItemNoValue.Name = "dgvItemNoValue";
            this.dgvItemNoValue.ReadOnly = true;
            this.dgvItemNoValue.RowHeadersWidth = 25;
            this.dgvItemNoValue.RowTemplate.Height = 24;
            this.dgvItemNoValue.Size = new System.Drawing.Size(731, 174);
            this.dgvItemNoValue.TabIndex = 57;
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
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(3, 201);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(737, 3);
            this.splitter1.TabIndex = 59;
            this.splitter1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvItemValue);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(3, 204);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(737, 218);
            this.groupBox1.TabIndex = 58;
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
            this.dgvItemValue.Size = new System.Drawing.Size(731, 194);
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvDefect);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(743, 425);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Defect Data";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvDefect
            // 
            this.dgvDefect.AllowUserToAddRows = false;
            this.dgvDefect.AllowUserToDeleteRows = false;
            this.dgvDefect.BackgroundColor = System.Drawing.Color.White;
            this.dgvDefect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDefect.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DEFECT_CODE,
            this.DEFECT_DESC,
            this.DEFECT_DESC2,
            this.DEFECT_LEVEL,
            this.DEFECT_QTY,
            this.DEFECT_MEMO});
            this.dgvDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDefect.EnableHeadersVisualStyles = false;
            this.dgvDefect.Location = new System.Drawing.Point(3, 3);
            this.dgvDefect.MultiSelect = false;
            this.dgvDefect.Name = "dgvDefect";
            this.dgvDefect.ReadOnly = true;
            this.dgvDefect.RowHeadersWidth = 25;
            this.dgvDefect.RowTemplate.Height = 24;
            this.dgvDefect.Size = new System.Drawing.Size(737, 419);
            this.dgvDefect.TabIndex = 58;
            // 
            // DEFECT_CODE
            // 
            this.DEFECT_CODE.HeaderText = "Defect Code";
            this.DEFECT_CODE.Name = "DEFECT_CODE";
            this.DEFECT_CODE.ReadOnly = true;
            this.DEFECT_CODE.Width = 120;
            // 
            // DEFECT_DESC
            // 
            this.DEFECT_DESC.HeaderText = "Defect Description";
            this.DEFECT_DESC.Name = "DEFECT_DESC";
            this.DEFECT_DESC.ReadOnly = true;
            this.DEFECT_DESC.Width = 140;
            // 
            // DEFECT_DESC2
            // 
            this.DEFECT_DESC2.HeaderText = "Defect Description2";
            this.DEFECT_DESC2.Name = "DEFECT_DESC2";
            this.DEFECT_DESC2.ReadOnly = true;
            this.DEFECT_DESC2.Width = 145;
            // 
            // DEFECT_LEVEL
            // 
            this.DEFECT_LEVEL.HeaderText = "Level";
            this.DEFECT_LEVEL.Name = "DEFECT_LEVEL";
            this.DEFECT_LEVEL.ReadOnly = true;
            this.DEFECT_LEVEL.Width = 120;
            // 
            // DEFECT_QTY
            // 
            this.DEFECT_QTY.HeaderText = "Defect Qty";
            this.DEFECT_QTY.Name = "DEFECT_QTY";
            this.DEFECT_QTY.ReadOnly = true;
            this.DEFECT_QTY.Width = 95;
            // 
            // DEFECT_MEMO
            // 
            this.DEFECT_MEMO.HeaderText = "Memo";
            this.DEFECT_MEMO.Name = "DEFECT_MEMO";
            this.DEFECT_MEMO.ReadOnly = true;
            this.DEFECT_MEMO.Width = 150;
            // 
            // fInspHistoryDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 495);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "fInspHistoryDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inspect Item";
            this.Load += new System.EventHandler(this.fInspHistoryDetail_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemNoValue)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemValue)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lablTypeName;
        private System.Windows.Forms.Label lablLotNo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvItemValue;
        private System.Windows.Forms.DataGridView dgvItemNoValue;
        private System.Windows.Forms.DataGridView dgvDefect;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_DESC;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_DESC2;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_LEVEL;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_MEMO;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_CODE1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_NAME1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASS_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn FAIL_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_NAME;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}