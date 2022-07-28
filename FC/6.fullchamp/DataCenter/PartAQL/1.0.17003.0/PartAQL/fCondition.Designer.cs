namespace PartAQL
{
    partial class fCondition
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fCondition));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LabTerminal = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.editName = new System.Windows.Forms.ComboBox();
            this.combUnit = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.editRow = new System.Windows.Forms.TextBox();
            this.lblColumn = new System.Windows.Forms.Label();
            this.editColumn = new System.Windows.Forms.TextBox();
            this.lblRow = new System.Windows.Forms.Label();
            this.combPrint = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.editMax = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.editMin = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.combNecessary = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.combConvert = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.editDefault = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.combInputType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.combValueType = new System.Windows.Forms.ComboBox();
            this.combPhase = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudSeq = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeq)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // LabTerminal
            // 
            resources.ApplyResources(this.LabTerminal, "LabTerminal");
            this.LabTerminal.ForeColor = System.Drawing.Color.Black;
            this.LabTerminal.Name = "LabTerminal";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.editName);
            this.panel1.Controls.Add(this.combUnit);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.editRow);
            this.panel1.Controls.Add(this.lblColumn);
            this.panel1.Controls.Add(this.editColumn);
            this.panel1.Controls.Add(this.lblRow);
            this.panel1.Controls.Add(this.combPrint);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.editMax);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.editMin);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.combNecessary);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.combConvert);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.editDefault);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.combInputType);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.combValueType);
            this.panel1.Controls.Add(this.combPhase);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.nudSeq);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.LabTerminal);
            this.panel1.Name = "panel1";
            // 
            // editName
            // 
            this.editName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editName, "editName");
            this.editName.FormattingEnabled = true;
            this.editName.Name = "editName";
            // 
            // combUnit
            // 
            this.combUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combUnit, "combUnit");
            this.combUnit.FormattingEnabled = true;
            this.combUnit.Name = "combUnit";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Name = "label12";
            // 
            // editRow
            // 
            this.editRow.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editRow, "editRow");
            this.editRow.Name = "editRow";
            // 
            // lblColumn
            // 
            resources.ApplyResources(this.lblColumn, "lblColumn");
            this.lblColumn.ForeColor = System.Drawing.Color.Black;
            this.lblColumn.Name = "lblColumn";
            // 
            // editColumn
            // 
            this.editColumn.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editColumn, "editColumn");
            this.editColumn.Name = "editColumn";
            // 
            // lblRow
            // 
            resources.ApplyResources(this.lblRow, "lblRow");
            this.lblRow.ForeColor = System.Drawing.Color.Black;
            this.lblRow.Name = "lblRow";
            // 
            // combPrint
            // 
            this.combPrint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combPrint, "combPrint");
            this.combPrint.FormattingEnabled = true;
            this.combPrint.Items.AddRange(new object[] {
            resources.GetString("combPrint.Items"),
            resources.GetString("combPrint.Items1")});
            this.combPrint.Name = "combPrint";
            this.combPrint.Tag = "1";
            this.combPrint.SelectedIndexChanged += new System.EventHandler(this.combPrint_SelectedIndexChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Name = "label10";
            // 
            // editMax
            // 
            this.editMax.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editMax, "editMax");
            this.editMax.Name = "editMax";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Name = "label9";
            // 
            // editMin
            // 
            this.editMin.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editMin, "editMin");
            this.editMin.Name = "editMin";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Name = "label8";
            // 
            // combNecessary
            // 
            this.combNecessary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combNecessary, "combNecessary");
            this.combNecessary.FormattingEnabled = true;
            this.combNecessary.Items.AddRange(new object[] {
            resources.GetString("combNecessary.Items"),
            resources.GetString("combNecessary.Items1")});
            this.combNecessary.Name = "combNecessary";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Name = "label7";
            // 
            // combConvert
            // 
            this.combConvert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combConvert, "combConvert");
            this.combConvert.FormattingEnabled = true;
            this.combConvert.Items.AddRange(new object[] {
            resources.GetString("combConvert.Items"),
            resources.GetString("combConvert.Items1"),
            resources.GetString("combConvert.Items2")});
            this.combConvert.Name = "combConvert";
            this.combConvert.Tag = "1";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Name = "label6";
            // 
            // editDefault
            // 
            this.editDefault.BackColor = System.Drawing.Color.White;
            this.editDefault.DropDownWidth = 180;
            resources.ApplyResources(this.editDefault, "editDefault");
            this.editDefault.Name = "editDefault";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // combInputType
            // 
            this.combInputType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combInputType, "combInputType");
            this.combInputType.FormattingEnabled = true;
            this.combInputType.Items.AddRange(new object[] {
            resources.GetString("combInputType.Items"),
            resources.GetString("combInputType.Items1"),
            resources.GetString("combInputType.Items2")});
            this.combInputType.Name = "combInputType";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // combValueType
            // 
            this.combValueType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combValueType, "combValueType");
            this.combValueType.FormattingEnabled = true;
            this.combValueType.Items.AddRange(new object[] {
            resources.GetString("combValueType.Items"),
            resources.GetString("combValueType.Items1")});
            this.combValueType.Name = "combValueType";
            // 
            // combPhase
            // 
            this.combPhase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combPhase, "combPhase");
            this.combPhase.FormattingEnabled = true;
            this.combPhase.Items.AddRange(new object[] {
            resources.GetString("combPhase.Items"),
            resources.GetString("combPhase.Items1"),
            resources.GetString("combPhase.Items2")});
            this.combPhase.Name = "combPhase";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // nudSeq
            // 
            resources.ApplyResources(this.nudSeq, "nudSeq");
            this.nudSeq.Name = "nudSeq";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // fCondition
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "fCondition";
            this.Load += new System.EventHandler(this.fCondition_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeq)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label LabTerminal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown nudSeq;
        private System.Windows.Forms.ComboBox combPhase;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox combValueType;
        private System.Windows.Forms.ComboBox combInputType;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox editDefault;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox combConvert;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox combNecessary;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox editMax;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox editMin;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox combPrint;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox editRow;
        private System.Windows.Forms.Label lblColumn;
        public System.Windows.Forms.TextBox editColumn;
        private System.Windows.Forms.Label lblRow;
        private System.Windows.Forms.ComboBox combUnit;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox editName;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}