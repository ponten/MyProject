namespace CSaleOrderInput
{
    partial class fFilter_Part
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.combField = new System.Windows.Forms.ComboBox();
            this.editValue = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.gvData = new System.Windows.Forms.DataGridView();
            this.PART_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SPEC1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPTION5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CUSTOMER_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CUSTOMER_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gvData, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(540, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.combField);
            this.flowLayoutPanel1.Controls.Add(this.editValue);
            this.flowLayoutPanel1.Controls.Add(this.buttonSearch);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(534, 44);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // combField
            // 
            this.combField.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.combField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combField.FormattingEnabled = true;
            this.combField.Location = new System.Drawing.Point(3, 6);
            this.combField.Name = "combField";
            this.combField.Size = new System.Drawing.Size(150, 23);
            this.combField.TabIndex = 0;
            // 
            // editValue
            // 
            this.editValue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.editValue.Location = new System.Drawing.Point(159, 5);
            this.editValue.Name = "editValue";
            this.editValue.Size = new System.Drawing.Size(150, 25);
            this.editValue.TabIndex = 1;
            this.editValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editValue_KeyPress);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonSearch.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.buttonSearch.Location = new System.Drawing.Point(315, 3);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(100, 30);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // gvData
            // 
            this.gvData.AllowUserToAddRows = false;
            this.gvData.AllowUserToDeleteRows = false;
            this.gvData.BackgroundColor = System.Drawing.Color.White;
            this.gvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PART_NO,
            this.SPEC1,
            this.OPTION5,
            this.CUSTOMER_ID,
            this.CUSTOMER_NAME});
            this.gvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvData.Location = new System.Drawing.Point(3, 53);
            this.gvData.Name = "gvData";
            this.gvData.ReadOnly = true;
            this.gvData.RowHeadersWidth = 51;
            this.gvData.RowTemplate.Height = 27;
            this.gvData.Size = new System.Drawing.Size(534, 394);
            this.gvData.TabIndex = 1;
            this.gvData.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvData_CellDoubleClick);
            // 
            // PART_NO
            // 
            this.PART_NO.DataPropertyName = "PART_NO";
            this.PART_NO.HeaderText = "PART_NO";
            this.PART_NO.MinimumWidth = 6;
            this.PART_NO.Name = "PART_NO";
            this.PART_NO.ReadOnly = true;
            this.PART_NO.Width = 125;
            // 
            // SPEC1
            // 
            this.SPEC1.DataPropertyName = "SPEC1";
            this.SPEC1.HeaderText = "SPEC1";
            this.SPEC1.MinimumWidth = 6;
            this.SPEC1.Name = "SPEC1";
            this.SPEC1.ReadOnly = true;
            this.SPEC1.Width = 125;
            // 
            // OPTION5
            // 
            this.OPTION5.DataPropertyName = "OPTION5";
            this.OPTION5.HeaderText = "OPTION5";
            this.OPTION5.MinimumWidth = 6;
            this.OPTION5.Name = "OPTION5";
            this.OPTION5.ReadOnly = true;
            this.OPTION5.Width = 125;
            // 
            // CUSTOMER_ID
            // 
            this.CUSTOMER_ID.DataPropertyName = "CUSTOMER_ID";
            this.CUSTOMER_ID.HeaderText = "CUSTOMER_ID";
            this.CUSTOMER_ID.MinimumWidth = 6;
            this.CUSTOMER_ID.Name = "CUSTOMER_ID";
            this.CUSTOMER_ID.ReadOnly = true;
            this.CUSTOMER_ID.Width = 125;
            // 
            // CUSTOMER_NAME
            // 
            this.CUSTOMER_NAME.DataPropertyName = "CUSTOMER_NAME";
            this.CUSTOMER_NAME.HeaderText = "CUSTOMER_NAME";
            this.CUSTOMER_NAME.MinimumWidth = 6;
            this.CUSTOMER_NAME.Name = "CUSTOMER_NAME";
            this.CUSTOMER_NAME.ReadOnly = true;
            this.CUSTOMER_NAME.Width = 125;
            // 
            // fFilter_Part
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "fFilter_Part";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fFilter_Part";
            this.Load += new System.EventHandler(this.fFilter_Part_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox combField;
        private System.Windows.Forms.TextBox editValue;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.DataGridView gvData;
        private System.Windows.Forms.DataGridViewTextBoxColumn PART_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn SPEC1;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPTION5;
        private System.Windows.Forms.DataGridViewTextBoxColumn CUSTOMER_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CUSTOMER_NAME;
    }
}