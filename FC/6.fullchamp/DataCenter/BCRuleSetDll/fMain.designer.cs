namespace BCRuleSetDll
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.editRuleName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SCMaster = new System.Windows.Forms.SplitContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.LVRule = new System.Windows.Forms.ListView();
            this.RuleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.popMenuRule = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemDeleteRule = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAppend = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.editRuleFilter = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dvGrid = new System.Windows.Forms.DataGridView();
            this.LABEL_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LabelRule = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gb1 = new System.Windows.Forms.GroupBox();
            this.chkbLine = new System.Windows.Forms.CheckBox();
            this.chkbRemark = new System.Windows.Forms.CheckBox();
            this.chkbSo = new System.Windows.Forms.CheckBox();
            this.chkbPo = new System.Windows.Forms.CheckBox();
            this.chkbMasterWo = new System.Windows.Forms.CheckBox();
            this.chkbWoType = new System.Windows.Forms.CheckBox();
            this.chkbCust = new System.Windows.Forms.CheckBox();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemModifyRule = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAppendRule = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.SCMaster.Panel1.SuspendLayout();
            this.SCMaster.Panel2.SuspendLayout();
            this.SCMaster.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.popMenuRule.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvGrid)).BeginInit();
            this.gb1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.editRuleName);
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Name = "panel1";
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // editRuleName
            // 
            resources.ApplyResources(this.editRuleName, "editRuleName");
            this.editRuleName.Name = "editRuleName";
            this.editRuleName.ReadOnly = true;
            this.editRuleName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editRuleName_KeyPress);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // SCMaster
            // 
            resources.ApplyResources(this.SCMaster, "SCMaster");
            this.SCMaster.Name = "SCMaster";
            // 
            // SCMaster.Panel1
            // 
            this.SCMaster.Panel1.Controls.Add(this.panel4);
            // 
            // SCMaster.Panel2
            // 
            this.SCMaster.Panel2.Controls.Add(this.panel3);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel2);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.LVRule);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // LVRule
            // 
            this.LVRule.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RuleName});
            this.LVRule.ContextMenuStrip = this.popMenuRule;
            resources.ApplyResources(this.LVRule, "LVRule");
            this.LVRule.MultiSelect = false;
            this.LVRule.Name = "LVRule";
            this.LVRule.SmallImageList = this.imageList1;
            this.LVRule.UseCompatibleStateImageBehavior = false;
            this.LVRule.View = System.Windows.Forms.View.Details;
            this.LVRule.Click += new System.EventHandler(this.LVRule_Click);
            // 
            // RuleName
            // 
            resources.ApplyResources(this.RuleName, "RuleName");
            // 
            // popMenuRule
            // 
            this.popMenuRule.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemAppendRule,
            this.MenuItemModifyRule,
            this.MenuItemDeleteRule});
            this.popMenuRule.Name = "contextMenuStrip1";
            resources.ApplyResources(this.popMenuRule, "popMenuRule");
            // 
            // MenuItemDeleteRule
            // 
            this.MenuItemDeleteRule.Name = "MenuItemDeleteRule";
            resources.ApplyResources(this.MenuItemDeleteRule, "MenuItemDeleteRule");
            this.MenuItemDeleteRule.Click += new System.EventHandler(this.MenuItemDeleteRule_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "rule.bmp");
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnDelete);
            this.panel5.Controls.Add(this.btnAppend);
            this.panel5.Controls.Add(this.btnModify);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAppend
            // 
            resources.ApplyResources(this.btnAppend, "btnAppend");
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnModify
            // 
            resources.ApplyResources(this.btnModify, "btnModify");
            this.btnModify.Name = "btnModify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.editRuleFilter);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Name = "panel2";
            // 
            // editRuleFilter
            // 
            resources.ApplyResources(this.editRuleFilter, "editRuleFilter");
            this.editRuleFilter.Name = "editRuleFilter";
            this.editRuleFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.editRuleFilter_KeyDown);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.dvGrid);
            this.panel3.Controls.Add(this.gb1);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Name = "panel3";
            // 
            // dvGrid
            // 
            this.dvGrid.AllowUserToAddRows = false;
            this.dvGrid.AllowUserToDeleteRows = false;
            this.dvGrid.BackgroundColor = System.Drawing.Color.White;
            this.dvGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LABEL_NAME,
            this.LabelRule,
            this.Code});
            resources.ApplyResources(this.dvGrid, "dvGrid");
            this.dvGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dvGrid.MultiSelect = false;
            this.dvGrid.Name = "dvGrid";
            this.dvGrid.RowTemplate.Height = 24;
            this.dvGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvGrid_CellValueChanged);
            this.dvGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dvGrid_DataError);
            // 
            // LABEL_NAME
            // 
            resources.ApplyResources(this.LABEL_NAME, "LABEL_NAME");
            this.LABEL_NAME.Name = "LABEL_NAME";
            this.LABEL_NAME.ReadOnly = true;
            // 
            // LabelRule
            // 
            resources.ApplyResources(this.LabelRule, "LabelRule");
            this.LabelRule.Name = "LabelRule";
            this.LabelRule.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Code
            // 
            resources.ApplyResources(this.Code, "Code");
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            // 
            // gb1
            // 
            this.gb1.Controls.Add(this.chkbLine);
            this.gb1.Controls.Add(this.chkbRemark);
            this.gb1.Controls.Add(this.chkbSo);
            this.gb1.Controls.Add(this.chkbPo);
            this.gb1.Controls.Add(this.chkbMasterWo);
            this.gb1.Controls.Add(this.chkbWoType);
            this.gb1.Controls.Add(this.chkbCust);
            resources.ApplyResources(this.gb1, "gb1");
            this.gb1.Name = "gb1";
            this.gb1.TabStop = false;
            // 
            // chkbLine
            // 
            resources.ApplyResources(this.chkbLine, "chkbLine");
            this.chkbLine.Name = "chkbLine";
            this.chkbLine.UseVisualStyleBackColor = true;
            // 
            // chkbRemark
            // 
            resources.ApplyResources(this.chkbRemark, "chkbRemark");
            this.chkbRemark.Name = "chkbRemark";
            this.chkbRemark.UseVisualStyleBackColor = true;
            // 
            // chkbSo
            // 
            resources.ApplyResources(this.chkbSo, "chkbSo");
            this.chkbSo.Name = "chkbSo";
            this.chkbSo.UseVisualStyleBackColor = true;
            // 
            // chkbPo
            // 
            resources.ApplyResources(this.chkbPo, "chkbPo");
            this.chkbPo.Name = "chkbPo";
            this.chkbPo.UseVisualStyleBackColor = true;
            // 
            // chkbMasterWo
            // 
            resources.ApplyResources(this.chkbMasterWo, "chkbMasterWo");
            this.chkbMasterWo.Name = "chkbMasterWo";
            this.chkbMasterWo.UseVisualStyleBackColor = true;
            // 
            // chkbWoType
            // 
            resources.ApplyResources(this.chkbWoType, "chkbWoType");
            this.chkbWoType.Name = "chkbWoType";
            this.chkbWoType.UseVisualStyleBackColor = true;
            // 
            // chkbCust
            // 
            resources.ApplyResources(this.chkbCust, "chkbCust");
            this.chkbCust.Name = "chkbCust";
            this.chkbCust.UseVisualStyleBackColor = true;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // MenuItemModifyRule
            // 
            this.MenuItemModifyRule.Name = "MenuItemModifyRule";
            resources.ApplyResources(this.MenuItemModifyRule, "MenuItemModifyRule");
            this.MenuItemModifyRule.Click += new System.EventHandler(this.MenuItemModifyRule_Click);
            // 
            // MenuItemAppendRule
            // 
            this.MenuItemAppendRule.Name = "MenuItemAppendRule";
            resources.ApplyResources(this.MenuItemAppendRule, "MenuItemAppendRule");
            this.MenuItemAppendRule.Click += new System.EventHandler(this.MenuItemAppendRule_Click);
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.SCMaster);
            this.Name = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.SCMaster.Panel1.ResumeLayout(false);
            this.SCMaster.Panel2.ResumeLayout(false);
            this.SCMaster.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.popMenuRule.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dvGrid)).EndInit();
            this.gb1.ResumeLayout(false);
            this.gb1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SplitContainer SCMaster;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox editRuleFilter;
        private System.Windows.Forms.ListView LVRule;
        private System.Windows.Forms.ColumnHeader RuleName;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox editRuleName;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ContextMenuStrip popMenuRule;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDeleteRule;
        private System.Windows.Forms.GroupBox gb1;
        private System.Windows.Forms.CheckBox chkbRemark;
        private System.Windows.Forms.CheckBox chkbSo;
        private System.Windows.Forms.CheckBox chkbPo;
        private System.Windows.Forms.CheckBox chkbMasterWo;
        private System.Windows.Forms.CheckBox chkbWoType;
        private System.Windows.Forms.CheckBox chkbCust;
        private System.Windows.Forms.DataGridView dvGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn LABEL_NAME;
        private System.Windows.Forms.DataGridViewComboBoxColumn LabelRule;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.CheckBox chkbLine;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAppendRule;
        private System.Windows.Forms.ToolStripMenuItem MenuItemModifyRule;
    }
}
