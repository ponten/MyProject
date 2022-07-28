namespace RCTravel
{
    partial class showGroupProcess
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvRC = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.oRDDCMCTLOCATORPATHSTMPBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new RCTravel.DataSet1();
            this.oRDDCM_CT_LOCATORPATHS_TMPTableAdapter = new RCTravel.DataSet1TableAdapters.ORDDCM_CT_LOCATORPATHS_TMPTableAdapter();
            this.button1 = new System.Windows.Forms.Button();
            this.bsGrid = new System.Windows.Forms.BindingSource(this.components);
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oRDDCMCTLOCATORPATHSTMPBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRC
            // 
            this.dgvRC.AllowUserToAddRows = false;
            this.dgvRC.AllowUserToDeleteRows = false;
            this.dgvRC.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRC.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRC.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRC.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvRC.Location = new System.Drawing.Point(12, 12);
            this.dgvRC.MultiSelect = false;
            this.dgvRC.Name = "dgvRC";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRC.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRC.RowHeadersVisible = false;
            this.dgvRC.RowTemplate.Height = 23;
            this.dgvRC.Size = new System.Drawing.Size(440, 207);
            this.dgvRC.TabIndex = 0;
            this.dgvRC.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRC_CellClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "choose";
            this.Column1.Name = "Column1";
            this.Column1.TrueValue = "";
            this.Column1.Width = 50;
            // 
            // oRDDCMCTLOCATORPATHSTMPBindingSource
            // 
            this.oRDDCMCTLOCATORPATHSTMPBindingSource.DataMember = "ORDDCM_CT_LOCATORPATHS_TMP";
            this.oRDDCMCTLOCATORPATHSTMPBindingSource.DataSource = this.dataSet1;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "DataSet1";
            this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // oRDDCM_CT_LOCATORPATHS_TMPTableAdapter
            // 
            this.oRDDCM_CT_LOCATORPATHS_TMPTableAdapter.ClearBeforeFill = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(296, 225);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(377, 225);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // showGroupProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 253);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgvRC);
            this.Name = "showGroupProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Process choose";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oRDDCMCTLOCATORPATHSTMPBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRC;
        private DataSet1 dataSet1;
        private System.Windows.Forms.BindingSource oRDDCMCTLOCATORPATHSTMPBindingSource;
        private DataSet1TableAdapters.ORDDCM_CT_LOCATORPATHS_TMPTableAdapter oRDDCM_CT_LOCATORPATHS_TMPTableAdapter;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource bsGrid;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
    }
}