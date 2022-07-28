namespace BCReprintDll
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.btnReprint = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.editInput2 = new System.Windows.Forms.TextBox();
            this.combReprint = new System.Windows.Forms.ComboBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.editInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.combType = new System.Windows.Forms.ComboBox();
            this.SCMaster = new System.Windows.Forms.SplitContainer();
            this.gbAll = new System.Windows.Forms.GroupBox();
            this.LVAll = new System.Windows.Forms.ListView();
            this.Input = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnUnchoose = new System.Windows.Forms.Button();
            this.btnChoose = new System.Windows.Forms.Button();
            this.gbChoose = new System.Windows.Forms.GroupBox();
            this.ListData = new System.Windows.Forms.ListBox();
            this.ListParam = new System.Windows.Forms.ListBox();
            this.LVChoose = new System.Windows.Forms.ListView();
            this.Choose = new System.Windows.Forms.ColumnHeader();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LabDesc = new System.Windows.Forms.Label();
            this.LabLabel = new System.Windows.Forms.Label();
            this.LabPrintMethod = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LabPrintQty = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lablSN = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SCMaster.Panel1.SuspendLayout();
            this.SCMaster.Panel2.SuspendLayout();
            this.SCMaster.SuspendLayout();
            this.gbAll.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gbChoose.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReprint
            // 
            resources.ApplyResources(this.btnReprint, "btnReprint");
            this.btnReprint.Name = "btnReprint";
            this.btnReprint.UseVisualStyleBackColor = true;
            this.btnReprint.Click += new System.EventHandler(this.btnReprint_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this.lablSN);
            this.panel1.Controls.Add(this.editInput2);
            this.panel1.Controls.Add(this.combReprint);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.editInput);
            this.panel1.Controls.Add(this.btnReprint);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.combType);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Name = "panel1";
            // 
            // editInput2
            // 
            resources.ApplyResources(this.editInput2, "editInput2");
            this.editInput2.Name = "editInput2";
            this.editInput2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.editInput2_KeyDown);
            // 
            // combReprint
            // 
            this.combReprint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combReprint, "combReprint");
            this.combReprint.FormattingEnabled = true;
            this.combReprint.Name = "combReprint";
            this.combReprint.SelectedIndexChanged += new System.EventHandler(this.combReprint_SelectedIndexChanged);
            // 
            // btnQuery
            // 
            resources.ApplyResources(this.btnQuery, "btnQuery");
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.button1_Click);
            // 
            // editInput
            // 
            resources.ApplyResources(this.editInput, "editInput");
            this.editInput.Name = "editInput";
            this.editInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.editInput_KeyDown);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // combType
            // 
            this.combType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combType, "combType");
            this.combType.FormattingEnabled = true;
            this.combType.Name = "combType";
            this.combType.SelectedIndexChanged += new System.EventHandler(this.combType_SelectedIndexChanged);
            // 
            // SCMaster
            // 
            resources.ApplyResources(this.SCMaster, "SCMaster");
            this.SCMaster.Name = "SCMaster";
            // 
            // SCMaster.Panel1
            // 
            this.SCMaster.Panel1.Controls.Add(this.gbAll);
            this.SCMaster.Panel1.Controls.Add(this.panel2);
            // 
            // SCMaster.Panel2
            // 
            this.SCMaster.Panel2.Controls.Add(this.gbChoose);
            // 
            // gbAll
            // 
            this.gbAll.Controls.Add(this.LVAll);
            resources.ApplyResources(this.gbAll, "gbAll");
            this.gbAll.Name = "gbAll";
            this.gbAll.TabStop = false;
            // 
            // LVAll
            // 
            this.LVAll.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Input});
            resources.ApplyResources(this.LVAll, "LVAll");
            this.LVAll.Name = "LVAll";
            this.LVAll.SmallImageList = this.imageList1;
            this.LVAll.UseCompatibleStateImageBehavior = false;
            this.LVAll.View = System.Windows.Forms.View.Details;
            this.LVAll.DoubleClick += new System.EventHandler(this.btnChoose_Click);
            // 
            // Input
            // 
            resources.ApplyResources(this.Input, "Input");
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "rule.bmp");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnUnchoose);
            this.panel2.Controls.Add(this.btnChoose);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnUnchoose
            // 
            resources.ApplyResources(this.btnUnchoose, "btnUnchoose");
            this.btnUnchoose.Name = "btnUnchoose";
            this.btnUnchoose.UseVisualStyleBackColor = true;
            this.btnUnchoose.Click += new System.EventHandler(this.btnUnchoose_Click);
            // 
            // btnChoose
            // 
            resources.ApplyResources(this.btnChoose, "btnChoose");
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // gbChoose
            // 
            this.gbChoose.Controls.Add(this.ListData);
            this.gbChoose.Controls.Add(this.ListParam);
            this.gbChoose.Controls.Add(this.LVChoose);
            resources.ApplyResources(this.gbChoose, "gbChoose");
            this.gbChoose.Name = "gbChoose";
            this.gbChoose.TabStop = false;
            // 
            // ListData
            // 
            this.ListData.FormattingEnabled = true;
            resources.ApplyResources(this.ListData, "ListData");
            this.ListData.Name = "ListData";
            // 
            // ListParam
            // 
            this.ListParam.FormattingEnabled = true;
            resources.ApplyResources(this.ListParam, "ListParam");
            this.ListParam.Name = "ListParam";
            // 
            // LVChoose
            // 
            this.LVChoose.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Choose});
            resources.ApplyResources(this.LVChoose, "LVChoose");
            this.LVChoose.Name = "LVChoose";
            this.LVChoose.SmallImageList = this.imageList1;
            this.LVChoose.UseCompatibleStateImageBehavior = false;
            this.LVChoose.View = System.Windows.Forms.View.Details;
            this.LVChoose.DoubleClick += new System.EventHandler(this.btnUnchoose_Click);
            // 
            // Choose
            // 
            resources.ApplyResources(this.Choose, "Choose");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox2.Controls.Add(this.LabDesc);
            this.groupBox2.Controls.Add(this.LabLabel);
            this.groupBox2.Controls.Add(this.LabPrintMethod);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.LabPrintQty);
            this.groupBox2.Controls.Add(this.label3);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // LabDesc
            // 
            resources.ApplyResources(this.LabDesc, "LabDesc");
            this.LabDesc.BackColor = System.Drawing.Color.Transparent;
            this.LabDesc.ForeColor = System.Drawing.Color.Maroon;
            this.LabDesc.Name = "LabDesc";
            // 
            // LabLabel
            // 
            resources.ApplyResources(this.LabLabel, "LabLabel");
            this.LabLabel.BackColor = System.Drawing.Color.Transparent;
            this.LabLabel.Name = "LabLabel";
            // 
            // LabPrintMethod
            // 
            resources.ApplyResources(this.LabPrintMethod, "LabPrintMethod");
            this.LabPrintMethod.BackColor = System.Drawing.Color.Transparent;
            this.LabPrintMethod.ForeColor = System.Drawing.Color.Maroon;
            this.LabPrintMethod.Name = "LabPrintMethod";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // LabPrintQty
            // 
            resources.ApplyResources(this.LabPrintQty, "LabPrintQty");
            this.LabPrintQty.BackColor = System.Drawing.Color.Transparent;
            this.LabPrintQty.ForeColor = System.Drawing.Color.Maroon;
            this.LabPrintQty.Name = "LabPrintQty";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // lablSN
            // 
            resources.ApplyResources(this.lablSN, "lablSN");
            this.lablSN.BackColor = System.Drawing.Color.Transparent;
            this.lablSN.Name = "lablSN";
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.SCMaster);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel1);
            this.Name = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.Shown += new System.EventHandler(this.fMain_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.SCMaster.Panel1.ResumeLayout(false);
            this.SCMaster.Panel2.ResumeLayout(false);
            this.SCMaster.ResumeLayout(false);
            this.gbAll.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.gbChoose.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnReprint;
        private System.Windows.Forms.SplitContainer SCMaster;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox combType;
        private System.Windows.Forms.TextBox editInput;
        private System.Windows.Forms.ListView LVAll;
        private System.Windows.Forms.ColumnHeader Input;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox gbAll;
        private System.Windows.Forms.GroupBox gbChoose;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.Button btnUnchoose;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView LVChoose;
        private System.Windows.Forms.ColumnHeader Choose;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ListBox ListData;
        private System.Windows.Forms.ListBox ListParam;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label LabLabel;
        private System.Windows.Forms.Label LabPrintMethod;
        private System.Windows.Forms.Label LabDesc;
        private System.Windows.Forms.Label LabPrintQty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.ComboBox combReprint;
        private System.Windows.Forms.TextBox editInput2;
        private System.Windows.Forms.Label lablSN;
    }
}
