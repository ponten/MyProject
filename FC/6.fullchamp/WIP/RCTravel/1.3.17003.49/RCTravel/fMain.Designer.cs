namespace RCTravel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ScMain = new System.Windows.Forms.SplitContainer();
            this.TlpTop = new System.Windows.Forms.TableLayoutPanel();
            this.GbLeft = new System.Windows.Forms.GroupBox();
            this.TlpExecute = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEmp = new System.Windows.Forms.TextBox();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.BtnEnter = new System.Windows.Forms.Button();
            this.BtnTravel = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnReset = new System.Windows.Forms.Button();
            this.GbLastRuncard = new System.Windows.Forms.GroupBox();
            this.TlpTravel = new System.Windows.Forms.TableLayoutPanel();
            this.DgvTravel = new System.Windows.Forms.DataGridView();
            this.LbRuncard = new System.Windows.Forms.Label();
            this.LbProcess = new System.Windows.Forms.Label();
            this.LbSheet = new System.Windows.Forms.Label();
            this.GbBottom = new System.Windows.Forms.GroupBox();
            this.TlpQuery = new System.Windows.Forms.TableLayoutPanel();
            this.tsType = new System.Windows.Forms.ComboBox();
            this.dgvRC = new System.Windows.Forms.DataGridView();
            this.txtData = new System.Windows.Forms.TextBox();
            this.txtWo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CbByWorkOrder = new System.Windows.Forms.CheckBox();
            this.CbByRoute = new System.Windows.Forms.CheckBox();
            this.BtnQuery = new System.Windows.Forms.Button();
            this.BtnExecute = new System.Windows.Forms.Button();
            this.combStage = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.combProcess = new System.Windows.Forms.ComboBox();
            this.CkbStageProcess = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnDeleteSelectRC = new System.Windows.Forms.ToolStripMenuItem();
            this.bsGrid = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ScMain)).BeginInit();
            this.ScMain.Panel1.SuspendLayout();
            this.ScMain.Panel2.SuspendLayout();
            this.ScMain.SuspendLayout();
            this.TlpTop.SuspendLayout();
            this.GbLeft.SuspendLayout();
            this.TlpExecute.SuspendLayout();
            this.GbLastRuncard.SuspendLayout();
            this.TlpTravel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTravel)).BeginInit();
            this.GbBottom.SuspendLayout();
            this.TlpQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRC)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // ScMain
            // 
            resources.ApplyResources(this.ScMain, "ScMain");
            this.ScMain.Name = "ScMain";
            // 
            // ScMain.Panel1
            // 
            this.ScMain.Panel1.Controls.Add(this.TlpTop);
            resources.ApplyResources(this.ScMain.Panel1, "ScMain.Panel1");
            // 
            // ScMain.Panel2
            // 
            this.ScMain.Panel2.Controls.Add(this.GbBottom);
            resources.ApplyResources(this.ScMain.Panel2, "ScMain.Panel2");
            // 
            // TlpTop
            // 
            resources.ApplyResources(this.TlpTop, "TlpTop");
            this.TlpTop.Controls.Add(this.GbLeft, 0, 0);
            this.TlpTop.Controls.Add(this.GbLastRuncard, 1, 0);
            this.TlpTop.Name = "TlpTop";
            // 
            // GbLeft
            // 
            resources.ApplyResources(this.GbLeft, "GbLeft");
            this.GbLeft.Controls.Add(this.TlpExecute);
            this.GbLeft.Name = "GbLeft";
            this.GbLeft.TabStop = false;
            // 
            // TlpExecute
            // 
            resources.ApplyResources(this.TlpExecute, "TlpExecute");
            this.TlpExecute.Controls.Add(this.label5, 0, 2);
            this.TlpExecute.Controls.Add(this.label1, 0, 0);
            this.TlpExecute.Controls.Add(this.txtEmp, 1, 0);
            this.TlpExecute.Controls.Add(this.txtSN, 1, 2);
            this.TlpExecute.Controls.Add(this.label7, 1, 1);
            this.TlpExecute.Controls.Add(this.BtnEnter, 1, 3);
            this.TlpExecute.Controls.Add(this.BtnTravel, 2, 3);
            this.TlpExecute.Controls.Add(this.BtnClear, 3, 3);
            this.TlpExecute.Controls.Add(this.BtnReset, 2, 1);
            this.TlpExecute.Name = "TlpExecute";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtEmp
            // 
            resources.ApplyResources(this.txtEmp, "txtEmp");
            this.TlpExecute.SetColumnSpan(this.txtEmp, 3);
            this.txtEmp.Name = "txtEmp";
            this.txtEmp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEmp_KeyPress);
            // 
            // txtSN
            // 
            resources.ApplyResources(this.txtSN, "txtSN");
            this.TlpExecute.SetColumnSpan(this.txtSN, 3);
            this.txtSN.Name = "txtSN";
            this.txtSN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSN_KeyPress);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // BtnEnter
            // 
            resources.ApplyResources(this.BtnEnter, "BtnEnter");
            this.BtnEnter.Name = "BtnEnter";
            this.BtnEnter.UseVisualStyleBackColor = true;
            this.BtnEnter.Click += new System.EventHandler(this.BtnEnter_Click);
            // 
            // BtnTravel
            // 
            resources.ApplyResources(this.BtnTravel, "BtnTravel");
            this.BtnTravel.Name = "BtnTravel";
            this.BtnTravel.UseVisualStyleBackColor = true;
            this.BtnTravel.Click += new System.EventHandler(this.BtnTravel_Click);
            // 
            // BtnClear
            // 
            resources.ApplyResources(this.BtnClear, "BtnClear");
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnReset
            // 
            resources.ApplyResources(this.BtnReset, "BtnReset");
            this.TlpExecute.SetColumnSpan(this.BtnReset, 2);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.UseVisualStyleBackColor = true;
            this.BtnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // GbLastRuncard
            // 
            resources.ApplyResources(this.GbLastRuncard, "GbLastRuncard");
            this.GbLastRuncard.Controls.Add(this.TlpTravel);
            this.GbLastRuncard.Name = "GbLastRuncard";
            this.GbLastRuncard.TabStop = false;
            // 
            // TlpTravel
            // 
            resources.ApplyResources(this.TlpTravel, "TlpTravel");
            this.TlpTravel.Controls.Add(this.DgvTravel, 0, 1);
            this.TlpTravel.Controls.Add(this.LbRuncard, 0, 0);
            this.TlpTravel.Controls.Add(this.LbProcess, 1, 0);
            this.TlpTravel.Controls.Add(this.LbSheet, 2, 0);
            this.TlpTravel.Name = "TlpTravel";
            // 
            // DgvTravel
            // 
            this.DgvTravel.AllowUserToAddRows = false;
            this.DgvTravel.AllowUserToDeleteRows = false;
            this.DgvTravel.AllowUserToOrderColumns = true;
            this.DgvTravel.AllowUserToResizeRows = false;
            this.DgvTravel.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DgvTravel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TlpTravel.SetColumnSpan(this.DgvTravel, 3);
            resources.ApplyResources(this.DgvTravel, "DgvTravel");
            this.DgvTravel.MultiSelect = false;
            this.DgvTravel.Name = "DgvTravel";
            this.DgvTravel.ReadOnly = true;
            this.DgvTravel.RowTemplate.Height = 24;
            // 
            // LbRuncard
            // 
            resources.ApplyResources(this.LbRuncard, "LbRuncard");
            this.LbRuncard.Name = "LbRuncard";
            // 
            // LbProcess
            // 
            resources.ApplyResources(this.LbProcess, "LbProcess");
            this.LbProcess.Name = "LbProcess";
            // 
            // LbSheet
            // 
            resources.ApplyResources(this.LbSheet, "LbSheet");
            this.LbSheet.Name = "LbSheet";
            // 
            // GbBottom
            // 
            resources.ApplyResources(this.GbBottom, "GbBottom");
            this.GbBottom.Controls.Add(this.TlpQuery);
            this.GbBottom.Name = "GbBottom";
            this.GbBottom.TabStop = false;
            // 
            // TlpQuery
            // 
            resources.ApplyResources(this.TlpQuery, "TlpQuery");
            this.TlpQuery.Controls.Add(this.tsType, 0, 2);
            this.TlpQuery.Controls.Add(this.dgvRC, 0, 3);
            this.TlpQuery.Controls.Add(this.txtData, 1, 2);
            this.TlpQuery.Controls.Add(this.txtWo, 3, 2);
            this.TlpQuery.Controls.Add(this.label2, 2, 2);
            this.TlpQuery.Controls.Add(this.CbByWorkOrder, 5, 1);
            this.TlpQuery.Controls.Add(this.CbByRoute, 5, 0);
            this.TlpQuery.Controls.Add(this.BtnQuery, 5, 2);
            this.TlpQuery.Controls.Add(this.BtnExecute, 6, 2);
            this.TlpQuery.Controls.Add(this.combStage, 1, 0);
            this.TlpQuery.Controls.Add(this.label4, 2, 0);
            this.TlpQuery.Controls.Add(this.combProcess, 3, 0);
            this.TlpQuery.Controls.Add(this.CkbStageProcess, 0, 0);
            this.TlpQuery.Name = "TlpQuery";
            // 
            // tsType
            // 
            resources.ApplyResources(this.tsType, "tsType");
            this.tsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsType.FormattingEnabled = true;
            this.tsType.Name = "tsType";
            // 
            // dgvRC
            // 
            this.dgvRC.AllowUserToAddRows = false;
            this.dgvRC.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgvRC.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRC.BackgroundColor = System.Drawing.Color.White;
            this.dgvRC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TlpQuery.SetColumnSpan(this.dgvRC, 8);
            resources.ApplyResources(this.dgvRC, "dgvRC");
            this.dgvRC.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvRC.Name = "dgvRC";
            this.dgvRC.ReadOnly = true;
            this.dgvRC.RowTemplate.Height = 24;
            this.dgvRC.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // txtData
            // 
            resources.ApplyResources(this.txtData, "txtData");
            this.txtData.Name = "txtData";
            // 
            // txtWo
            // 
            resources.ApplyResources(this.txtWo, "txtWo");
            this.txtWo.Name = "txtWo";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // CbByWorkOrder
            // 
            resources.ApplyResources(this.CbByWorkOrder, "CbByWorkOrder");
            this.TlpQuery.SetColumnSpan(this.CbByWorkOrder, 2);
            this.CbByWorkOrder.Name = "CbByWorkOrder";
            this.CbByWorkOrder.UseVisualStyleBackColor = true;
            this.CbByWorkOrder.CheckStateChanged += new System.EventHandler(this.CbByWorkOrder_CheckStateChanged);
            // 
            // CbByRoute
            // 
            resources.ApplyResources(this.CbByRoute, "CbByRoute");
            this.TlpQuery.SetColumnSpan(this.CbByRoute, 2);
            this.CbByRoute.Name = "CbByRoute";
            this.CbByRoute.UseVisualStyleBackColor = true;
            this.CbByRoute.CheckStateChanged += new System.EventHandler(this.CbByRoute_CheckStateChanged);
            // 
            // BtnQuery
            // 
            resources.ApplyResources(this.BtnQuery, "BtnQuery");
            this.BtnQuery.Name = "BtnQuery";
            this.BtnQuery.UseVisualStyleBackColor = true;
            this.BtnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // BtnExecute
            // 
            resources.ApplyResources(this.BtnExecute, "BtnExecute");
            this.BtnExecute.Name = "BtnExecute";
            this.BtnExecute.UseVisualStyleBackColor = true;
            this.BtnExecute.Click += new System.EventHandler(this.BtnExecute_Click);
            // 
            // combStage
            // 
            resources.ApplyResources(this.combStage, "combStage");
            this.combStage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combStage.FormattingEnabled = true;
            this.combStage.Name = "combStage";
            this.TlpQuery.SetRowSpan(this.combStage, 2);
            this.combStage.SelectedIndexChanged += new System.EventHandler(this.combStage_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.TlpQuery.SetRowSpan(this.label4, 2);
            // 
            // combProcess
            // 
            resources.ApplyResources(this.combProcess, "combProcess");
            this.combProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combProcess.FormattingEnabled = true;
            this.combProcess.Name = "combProcess";
            this.TlpQuery.SetRowSpan(this.combProcess, 2);
            // 
            // CkbStageProcess
            // 
            resources.ApplyResources(this.CkbStageProcess, "CkbStageProcess");
            this.CkbStageProcess.Name = "CkbStageProcess";
            this.TlpQuery.SetRowSpan(this.CkbStageProcess, 2);
            this.CkbStageProcess.UseVisualStyleBackColor = true;
            this.CkbStageProcess.CheckedChanged += new System.EventHandler(this.CkbStageProcess_CheckedChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDeleteSelectRC});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // btnDeleteSelectRC
            // 
            this.btnDeleteSelectRC.Name = "btnDeleteSelectRC";
            resources.ApplyResources(this.btnDeleteSelectRC, "btnDeleteSelectRC");
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ScMain);
            this.Name = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.Shown += new System.EventHandler(this.fMain_Shown);
            this.ScMain.Panel1.ResumeLayout(false);
            this.ScMain.Panel1.PerformLayout();
            this.ScMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScMain)).EndInit();
            this.ScMain.ResumeLayout(false);
            this.TlpTop.ResumeLayout(false);
            this.GbLeft.ResumeLayout(false);
            this.GbLeft.PerformLayout();
            this.TlpExecute.ResumeLayout(false);
            this.TlpExecute.PerformLayout();
            this.GbLastRuncard.ResumeLayout(false);
            this.TlpTravel.ResumeLayout(false);
            this.TlpTravel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTravel)).EndInit();
            this.GbBottom.ResumeLayout(false);
            this.GbBottom.PerformLayout();
            this.TlpQuery.ResumeLayout(false);
            this.TlpQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRC)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GbBottom;
        private System.Windows.Forms.DataGridView dgvRC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEmp;
        private System.Windows.Forms.TextBox txtSN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWo;
        private System.Windows.Forms.ComboBox combStage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.ComboBox combProcess;
        private System.Windows.Forms.Button BtnQuery;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox GbLeft;
        private System.Windows.Forms.Button BtnExecute;
        private System.Windows.Forms.ComboBox tsType;
        private System.Windows.Forms.BindingSource bsGrid;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btnDeleteSelectRC;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox CbByWorkOrder;
        private System.Windows.Forms.TableLayoutPanel TlpQuery;
        private System.Windows.Forms.TableLayoutPanel TlpExecute;
        private System.Windows.Forms.TableLayoutPanel TlpTop;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.CheckBox CbByRoute;
        private System.Windows.Forms.CheckBox CkbStageProcess;
        private System.Windows.Forms.Button BtnEnter;
        private System.Windows.Forms.GroupBox GbLastRuncard;
        private System.Windows.Forms.DataGridView DgvTravel;
        private System.Windows.Forms.TableLayoutPanel TlpTravel;
        private System.Windows.Forms.Label LbRuncard;
        private System.Windows.Forms.Label LbProcess;
        private System.Windows.Forms.Label LbSheet;
        private System.Windows.Forms.Button BtnTravel;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.SplitContainer ScMain;
    }
}
