namespace BCRuleDll
{
    partial class fWOData
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
            this.dgvSNList = new System.Windows.Forms.DataGridView();
            this.CHECKED = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.WORK_ORDER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PART_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WO_RULE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TARGET_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INPUT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OUTPUT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lablType = new System.Windows.Forms.Label();
            this.lablRuleName = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSNList)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSNList
            // 
            this.dgvSNList.AllowUserToAddRows = false;
            this.dgvSNList.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.dgvSNList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSNList.BackgroundColor = System.Drawing.Color.White;
            this.dgvSNList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSNList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CHECKED,
            this.WORK_ORDER,
            this.PART_NO,
            this.WO_RULE,
            this.TARGET_QTY,
            this.INPUT_QTY,
            this.OUTPUT_QTY});
            this.dgvSNList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSNList.Location = new System.Drawing.Point(3, 56);
            this.dgvSNList.Name = "dgvSNList";
            this.dgvSNList.RowHeadersWidth = 10;
            this.dgvSNList.RowTemplate.Height = 24;
            this.dgvSNList.Size = new System.Drawing.Size(614, 343);
            this.dgvSNList.TabIndex = 1;
            // 
            // CHECKED
            // 
            this.CHECKED.FalseValue = "N";
            this.CHECKED.HeaderText = "";
            this.CHECKED.Name = "CHECKED";
            this.CHECKED.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CHECKED.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CHECKED.TrueValue = "Y";
            this.CHECKED.Width = 20;
            // 
            // WORK_ORDER
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.WORK_ORDER.DefaultCellStyle = dataGridViewCellStyle2;
            this.WORK_ORDER.HeaderText = "Work Order";
            this.WORK_ORDER.Name = "WORK_ORDER";
            this.WORK_ORDER.Width = 150;
            // 
            // PART_NO
            // 
            this.PART_NO.HeaderText = "Part No";
            this.PART_NO.Name = "PART_NO";
            this.PART_NO.Width = 120;
            // 
            // WO_RULE
            // 
            this.WO_RULE.HeaderText = "WO Rule";
            this.WO_RULE.Name = "WO_RULE";
            // 
            // TARGET_QTY
            // 
            this.TARGET_QTY.HeaderText = "Target Qty";
            this.TARGET_QTY.Name = "TARGET_QTY";
            // 
            // INPUT_QTY
            // 
            this.INPUT_QTY.HeaderText = "Input Qty";
            this.INPUT_QTY.Name = "INPUT_QTY";
            // 
            // OUTPUT_QTY
            // 
            this.OUTPUT_QTY.HeaderText = "Output Qty";
            this.OUTPUT_QTY.Name = "OUTPUT_QTY";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(336, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOK.Location = new System.Drawing.Point(255, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lablType);
            this.panel2.Controls.Add(this.lablRuleName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(620, 39);
            this.panel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Rule Name";
            // 
            // lablType
            // 
            this.lablType.AutoSize = true;
            this.lablType.Location = new System.Drawing.Point(325, 9);
            this.lablType.Name = "lablType";
            this.lablType.Size = new System.Drawing.Size(31, 15);
            this.lablType.TabIndex = 1;
            this.lablType.Text = "N/A";
            this.lablType.Visible = false;
            // 
            // lablRuleName
            // 
            this.lablRuleName.AutoSize = true;
            this.lablRuleName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lablRuleName.Location = new System.Drawing.Point(96, 9);
            this.lablRuleName.Name = "lablRuleName";
            this.lablRuleName.Size = new System.Drawing.Size(31, 15);
            this.lablRuleName.TabIndex = 0;
            this.lablRuleName.Text = "N/A";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.btnSelectNone);
            this.panel3.Controls.Add(this.btnOK);
            this.panel3.Controls.Add(this.btnSelectAll);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 21);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(614, 35);
            this.panel3.TabIndex = 4;
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectNone.Location = new System.Drawing.Point(96, 6);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(90, 23);
            this.btnSelectNone.TabIndex = 1;
            this.btnSelectNone.Tag = "2";
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectAll.Location = new System.Drawing.Point(3, 6);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(90, 23);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Tag = "1";
            this.btnSelectAll.Text = "Select ALL";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvSNList);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(0, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(620, 402);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Work Order List";
            // 
            // fWOData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 441);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Name = "fWOData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Work Order List";
            this.Load += new System.EventHandler(this.fWOData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSNList)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Label lablRuleName;
        public System.Windows.Forms.Label lablType;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.DataGridView dgvSNList;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CHECKED;
        private System.Windows.Forms.DataGridViewTextBoxColumn WORK_ORDER;
        private System.Windows.Forms.DataGridViewTextBoxColumn PART_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn WO_RULE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TARGET_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn INPUT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn OUTPUT_QTY;
    }
}