namespace BCConfig
{
    partial class fMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSave = new System.Windows.Forms.Button();
            this.dvGrid = new System.Windows.Forms.DataGridView();
            this.combCodeSoft = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.LabelType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintLabel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PrintQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintMethod = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.PrintPort = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LabelQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dvGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dvGrid
            // 
            this.dvGrid.AllowUserToAddRows = false;
            this.dvGrid.AllowUserToDeleteRows = false;
            this.dvGrid.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dvGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dvGrid, "dvGrid");
            this.dvGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LabelType,
            this.PrintLabel,
            this.PrintQty,
            this.PrintMethod,
            this.PrintPort,
            this.LabelQty});
            this.dvGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dvGrid.MultiSelect = false;
            this.dvGrid.Name = "dvGrid";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dvGrid.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dvGrid.RowTemplate.Height = 24;
            this.dvGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGrid_CellValueChanged);
            // 
            // combCodeSoft
            // 
            this.combCodeSoft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combCodeSoft.FormattingEnabled = true;
            this.combCodeSoft.Items.AddRange(new object[] {
            resources.GetString("combCodeSoft.Items"),
            resources.GetString("combCodeSoft.Items1"),
            resources.GetString("combCodeSoft.Items2")});
            resources.ApplyResources(this.combCodeSoft, "combCodeSoft");
            this.combCodeSoft.Name = "combCodeSoft";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Name = "label14";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.combCodeSoft);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // LabelType
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabelType.DefaultCellStyle = dataGridViewCellStyle2;
            this.LabelType.Frozen = true;
            resources.ApplyResources(this.LabelType, "LabelType");
            this.LabelType.Name = "LabelType";
            this.LabelType.ReadOnly = true;
            // 
            // PrintLabel
            // 
            this.PrintLabel.FalseValue = "0";
            resources.ApplyResources(this.PrintLabel, "PrintLabel");
            this.PrintLabel.Name = "PrintLabel";
            this.PrintLabel.TrueValue = "1";
            // 
            // PrintQty
            // 
            resources.ApplyResources(this.PrintQty, "PrintQty");
            this.PrintQty.Name = "PrintQty";
            // 
            // PrintMethod
            // 
            resources.ApplyResources(this.PrintMethod, "PrintMethod");
            this.PrintMethod.Items.AddRange(new object[] {
            "CodeSoft",
            "DLL"});
            this.PrintMethod.Name = "PrintMethod";
            // 
            // PrintPort
            // 
            resources.ApplyResources(this.PrintPort, "PrintPort");
            this.PrintPort.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "LPT1"});
            this.PrintPort.Name = "PrintPort";
            // 
            // LabelQty
            // 
            resources.ApplyResources(this.LabelQty, "LabelQty");
            this.LabelQty.Name = "LabelQty";
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dvGrid);
            this.Controls.Add(this.panel1);
            this.Name = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox combCodeSoft;
        private System.Windows.Forms.DataGridView dvGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn LabelType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PrintLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrintQty;
        private System.Windows.Forms.DataGridViewComboBoxColumn PrintMethod;
        private System.Windows.Forms.DataGridViewComboBoxColumn PrintPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn LabelQty;        
    }
}
