namespace RCPart
{
    partial class fData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fData));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tbToPallet = new System.Windows.Forms.TextBox();
            this.lblToPallet = new System.Windows.Forms.Label();
            this.tbOldCode = new System.Windows.Forms.TextBox();
            this.lblOldCode = new System.Windows.Forms.Label();
            this.tbMaterial = new System.Windows.Forms.TextBox();
            this.tbForgingWeight = new System.Windows.Forms.TextBox();
            this.tbForgingNo = new System.Windows.Forms.TextBox();
            this.tbProductWeight = new System.Windows.Forms.TextBox();
            this.tbBluePrint = new System.Windows.Forms.TextBox();
            this.lblMaterial = new System.Windows.Forms.Label();
            this.lblForgingWeight = new System.Windows.Forms.Label();
            this.lblForgingNo = new System.Windows.Forms.Label();
            this.lblCustomerCode = new System.Windows.Forms.Label();
            this.lblProductWeight = new System.Windows.Forms.Label();
            this.tbT6Time = new System.Windows.Forms.TextBox();
            this.tbT4T6TimeInt = new System.Windows.Forms.TextBox();
            this.tbT4Time = new System.Windows.Forms.TextBox();
            this.tbUnitCount = new System.Windows.Forms.TextBox();
            this.lblT6Time = new System.Windows.Forms.Label();
            this.lblT4T6TimeInt = new System.Windows.Forms.Label();
            this.lblT4Time = new System.Windows.Forms.Label();
            this.lblUnitCount = new System.Windows.Forms.Label();
            this.lblBluePrint = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvRoute = new System.Windows.Forms.DataGridView();
            this.ROUTE_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ROUTE_DESC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EMP_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UPDATE_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ROUTE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextRouteMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuAppendRoute = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDeleteRoute = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiRouteDetail = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.LVPKSpec = new System.Windows.Forms.ListView();
            this.columnSpec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnBox = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCarton = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPallet = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuAppendPKSpec = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDeletePKSpec = new System.Windows.Forms.ToolStripMenuItem();
            this.combRuleSet = new System.Windows.Forms.ComboBox();
            this.combRoute = new System.Windows.Forms.ComboBox();
            this.editSpec2 = new System.Windows.Forms.TextBox();
            this.LabSpec2 = new System.Windows.Forms.Label();
            this.editVendorPart = new System.Windows.Forms.TextBox();
            this.LabVendorPart = new System.Windows.Forms.Label();
            this.editVersion = new System.Windows.Forms.TextBox();
            this.LabVersion = new System.Windows.Forms.Label();
            this.LabRoute = new System.Windows.Forms.Label();
            this.LabRuleSet = new System.Windows.Forms.Label();
            this.s = new System.Windows.Forms.TextBox();
            this.LabUnit = new System.Windows.Forms.Label();
            this.editSpec1 = new System.Windows.Forms.TextBox();
            this.LabSpec1 = new System.Windows.Forms.Label();
            this.editCustPart = new System.Windows.Forms.TextBox();
            this.editPart = new System.Windows.Forms.TextBox();
            this.LabCustPart = new System.Windows.Forms.Label();
            this.LabPart = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TbCustomerCode = new System.Windows.Forms.TextBox();
            this.BtnCustomer = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.LbChildPart = new System.Windows.Forms.Label();
            this.TbChildPart = new System.Windows.Forms.TextBox();
            this.TbNewChildPartNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnChildPart = new System.Windows.Forms.Button();
            this.LbEndProcess = new System.Windows.Forms.Label();
            this.TbEndProcess = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).BeginInit();
            this.contextRouteMenu.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbToPallet
            // 
            resources.ApplyResources(this.tbToPallet, "tbToPallet");
            this.tbToPallet.BackColor = System.Drawing.Color.White;
            this.tbToPallet.Name = "tbToPallet";
            this.tbToPallet.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbToPallet_KeyPress);
            // 
            // lblToPallet
            // 
            resources.ApplyResources(this.lblToPallet, "lblToPallet");
            this.lblToPallet.Name = "lblToPallet";
            // 
            // tbOldCode
            // 
            resources.ApplyResources(this.tbOldCode, "tbOldCode");
            this.tbOldCode.BackColor = System.Drawing.Color.Yellow;
            this.tbOldCode.Name = "tbOldCode";
            // 
            // lblOldCode
            // 
            resources.ApplyResources(this.lblOldCode, "lblOldCode");
            this.lblOldCode.Name = "lblOldCode";
            // 
            // tbMaterial
            // 
            resources.ApplyResources(this.tbMaterial, "tbMaterial");
            this.tbMaterial.Name = "tbMaterial";
            // 
            // tbForgingWeight
            // 
            resources.ApplyResources(this.tbForgingWeight, "tbForgingWeight");
            this.tbForgingWeight.Name = "tbForgingWeight";
            // 
            // tbForgingNo
            // 
            resources.ApplyResources(this.tbForgingNo, "tbForgingNo");
            this.tbForgingNo.Name = "tbForgingNo";
            // 
            // tbProductWeight
            // 
            resources.ApplyResources(this.tbProductWeight, "tbProductWeight");
            this.tbProductWeight.Name = "tbProductWeight";
            // 
            // tbBluePrint
            // 
            resources.ApplyResources(this.tbBluePrint, "tbBluePrint");
            this.tbBluePrint.BackColor = System.Drawing.Color.Yellow;
            this.tbBluePrint.Name = "tbBluePrint";
            // 
            // lblMaterial
            // 
            resources.ApplyResources(this.lblMaterial, "lblMaterial");
            this.lblMaterial.Name = "lblMaterial";
            // 
            // lblForgingWeight
            // 
            resources.ApplyResources(this.lblForgingWeight, "lblForgingWeight");
            this.lblForgingWeight.Name = "lblForgingWeight";
            // 
            // lblForgingNo
            // 
            resources.ApplyResources(this.lblForgingNo, "lblForgingNo");
            this.lblForgingNo.Name = "lblForgingNo";
            // 
            // lblCustomerCode
            // 
            resources.ApplyResources(this.lblCustomerCode, "lblCustomerCode");
            this.lblCustomerCode.Name = "lblCustomerCode";
            // 
            // lblProductWeight
            // 
            resources.ApplyResources(this.lblProductWeight, "lblProductWeight");
            this.lblProductWeight.Name = "lblProductWeight";
            // 
            // tbT6Time
            // 
            resources.ApplyResources(this.tbT6Time, "tbT6Time");
            this.tbT6Time.Name = "tbT6Time";
            // 
            // tbT4T6TimeInt
            // 
            resources.ApplyResources(this.tbT4T6TimeInt, "tbT4T6TimeInt");
            this.tbT4T6TimeInt.Name = "tbT4T6TimeInt";
            // 
            // tbT4Time
            // 
            resources.ApplyResources(this.tbT4Time, "tbT4Time");
            this.tbT4Time.Name = "tbT4Time";
            // 
            // tbUnitCount
            // 
            resources.ApplyResources(this.tbUnitCount, "tbUnitCount");
            this.tbUnitCount.BackColor = System.Drawing.Color.White;
            this.tbUnitCount.Name = "tbUnitCount";
            this.tbUnitCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbUnitCount_KeyPress);
            // 
            // lblT6Time
            // 
            resources.ApplyResources(this.lblT6Time, "lblT6Time");
            this.lblT6Time.Name = "lblT6Time";
            // 
            // lblT4T6TimeInt
            // 
            resources.ApplyResources(this.lblT4T6TimeInt, "lblT4T6TimeInt");
            this.lblT4T6TimeInt.Name = "lblT4T6TimeInt";
            // 
            // lblT4Time
            // 
            resources.ApplyResources(this.lblT4Time, "lblT4Time");
            this.lblT4Time.Name = "lblT4Time";
            // 
            // lblUnitCount
            // 
            resources.ApplyResources(this.lblUnitCount, "lblUnitCount");
            this.lblUnitCount.Name = "lblUnitCount";
            // 
            // lblBluePrint
            // 
            resources.ApplyResources(this.lblBluePrint, "lblBluePrint");
            this.lblBluePrint.Name = "lblBluePrint";
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 8);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvRoute);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvRoute
            // 
            this.dgvRoute.AllowUserToAddRows = false;
            this.dgvRoute.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgvRoute.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRoute.BackgroundColor = System.Drawing.Color.Lavender;
            resources.ApplyResources(this.dgvRoute, "dgvRoute");
            this.dgvRoute.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ROUTE_NAME,
            this.ROUTE_DESC,
            this.EMP_NO,
            this.UPDATE_TIME,
            this.ROUTE_ID});
            this.dgvRoute.ContextMenuStrip = this.contextRouteMenu;
            this.dgvRoute.MultiSelect = false;
            this.dgvRoute.Name = "dgvRoute";
            this.dgvRoute.ReadOnly = true;
            this.dgvRoute.RowTemplate.Height = 24;
            this.dgvRoute.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvRoute.TabStop = false;
            // 
            // ROUTE_NAME
            // 
            resources.ApplyResources(this.ROUTE_NAME, "ROUTE_NAME");
            this.ROUTE_NAME.Name = "ROUTE_NAME";
            this.ROUTE_NAME.ReadOnly = true;
            // 
            // ROUTE_DESC
            // 
            resources.ApplyResources(this.ROUTE_DESC, "ROUTE_DESC");
            this.ROUTE_DESC.Name = "ROUTE_DESC";
            this.ROUTE_DESC.ReadOnly = true;
            // 
            // EMP_NO
            // 
            resources.ApplyResources(this.EMP_NO, "EMP_NO");
            this.EMP_NO.Name = "EMP_NO";
            this.EMP_NO.ReadOnly = true;
            // 
            // UPDATE_TIME
            // 
            dataGridViewCellStyle2.Format = "yyyy/ MM/ dd HH: mm: ss";
            this.UPDATE_TIME.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.UPDATE_TIME, "UPDATE_TIME");
            this.UPDATE_TIME.Name = "UPDATE_TIME";
            this.UPDATE_TIME.ReadOnly = true;
            // 
            // ROUTE_ID
            // 
            resources.ApplyResources(this.ROUTE_ID, "ROUTE_ID");
            this.ROUTE_ID.Name = "ROUTE_ID";
            this.ROUTE_ID.ReadOnly = true;
            // 
            // contextRouteMenu
            // 
            this.contextRouteMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextRouteMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuAppendRoute,
            this.MenuDeleteRoute,
            this.TsmiRouteDetail});
            this.contextRouteMenu.Name = "contextMenuStrip2";
            resources.ApplyResources(this.contextRouteMenu, "contextRouteMenu");
            // 
            // MenuAppendRoute
            // 
            this.MenuAppendRoute.Name = "MenuAppendRoute";
            resources.ApplyResources(this.MenuAppendRoute, "MenuAppendRoute");
            this.MenuAppendRoute.Click += new System.EventHandler(this.MenuAppendRoute_Click);
            // 
            // MenuDeleteRoute
            // 
            this.MenuDeleteRoute.Name = "MenuDeleteRoute";
            resources.ApplyResources(this.MenuDeleteRoute, "MenuDeleteRoute");
            this.MenuDeleteRoute.Click += new System.EventHandler(this.MenuDeleteRoute_Click);
            // 
            // TsmiRouteDetail
            // 
            this.TsmiRouteDetail.Name = "TsmiRouteDetail";
            resources.ApplyResources(this.TsmiRouteDetail, "TsmiRouteDetail");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.LVPKSpec);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LVPKSpec
            // 
            this.LVPKSpec.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnSpec,
            this.columnBox,
            this.columnCarton,
            this.columnPallet});
            this.LVPKSpec.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.LVPKSpec, "LVPKSpec");
            this.LVPKSpec.FullRowSelect = true;
            this.LVPKSpec.HideSelection = false;
            this.LVPKSpec.Name = "LVPKSpec";
            this.LVPKSpec.UseCompatibleStateImageBehavior = false;
            this.LVPKSpec.View = System.Windows.Forms.View.Details;
            // 
            // columnSpec
            // 
            resources.ApplyResources(this.columnSpec, "columnSpec");
            // 
            // columnBox
            // 
            resources.ApplyResources(this.columnBox, "columnBox");
            // 
            // columnCarton
            // 
            resources.ApplyResources(this.columnCarton, "columnCarton");
            // 
            // columnPallet
            // 
            resources.ApplyResources(this.columnPallet, "columnPallet");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuAppendPKSpec,
            this.MenuDeletePKSpec});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // MenuAppendPKSpec
            // 
            this.MenuAppendPKSpec.Name = "MenuAppendPKSpec";
            resources.ApplyResources(this.MenuAppendPKSpec, "MenuAppendPKSpec");
            this.MenuAppendPKSpec.Click += new System.EventHandler(this.MenuAppendPKSpec_Click);
            // 
            // MenuDeletePKSpec
            // 
            this.MenuDeletePKSpec.Name = "MenuDeletePKSpec";
            resources.ApplyResources(this.MenuDeletePKSpec, "MenuDeletePKSpec");
            this.MenuDeletePKSpec.Click += new System.EventHandler(this.MenuDeletePKSpec_Click);
            // 
            // combRuleSet
            // 
            resources.ApplyResources(this.combRuleSet, "combRuleSet");
            this.combRuleSet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.combRuleSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combRuleSet.FormattingEnabled = true;
            this.combRuleSet.Name = "combRuleSet";
            // 
            // combRoute
            // 
            resources.ApplyResources(this.combRoute, "combRoute");
            this.combRoute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.combRoute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combRoute.FormattingEnabled = true;
            this.combRoute.Name = "combRoute";
            // 
            // editSpec2
            // 
            resources.ApplyResources(this.editSpec2, "editSpec2");
            this.editSpec2.Name = "editSpec2";
            // 
            // LabSpec2
            // 
            resources.ApplyResources(this.LabSpec2, "LabSpec2");
            this.LabSpec2.BackColor = System.Drawing.Color.Transparent;
            this.LabSpec2.Name = "LabSpec2";
            // 
            // editVendorPart
            // 
            resources.ApplyResources(this.editVendorPart, "editVendorPart");
            this.editVendorPart.Name = "editVendorPart";
            // 
            // LabVendorPart
            // 
            resources.ApplyResources(this.LabVendorPart, "LabVendorPart");
            this.LabVendorPart.BackColor = System.Drawing.Color.Transparent;
            this.LabVendorPart.Name = "LabVendorPart";
            // 
            // editVersion
            // 
            resources.ApplyResources(this.editVersion, "editVersion");
            this.editVersion.BackColor = System.Drawing.Color.White;
            this.editVersion.Name = "editVersion";
            // 
            // LabVersion
            // 
            resources.ApplyResources(this.LabVersion, "LabVersion");
            this.LabVersion.BackColor = System.Drawing.Color.Transparent;
            this.LabVersion.Name = "LabVersion";
            // 
            // LabRoute
            // 
            resources.ApplyResources(this.LabRoute, "LabRoute");
            this.LabRoute.BackColor = System.Drawing.Color.Transparent;
            this.LabRoute.Name = "LabRoute";
            // 
            // LabRuleSet
            // 
            resources.ApplyResources(this.LabRuleSet, "LabRuleSet");
            this.LabRuleSet.BackColor = System.Drawing.Color.Transparent;
            this.LabRuleSet.Name = "LabRuleSet";
            // 
            // s
            // 
            resources.ApplyResources(this.s, "s");
            this.s.Name = "s";
            // 
            // LabUnit
            // 
            resources.ApplyResources(this.LabUnit, "LabUnit");
            this.LabUnit.BackColor = System.Drawing.Color.Transparent;
            this.LabUnit.Name = "LabUnit";
            // 
            // editSpec1
            // 
            resources.ApplyResources(this.editSpec1, "editSpec1");
            this.editSpec1.Name = "editSpec1";
            // 
            // LabSpec1
            // 
            resources.ApplyResources(this.LabSpec1, "LabSpec1");
            this.LabSpec1.BackColor = System.Drawing.Color.Transparent;
            this.LabSpec1.Name = "LabSpec1";
            // 
            // editCustPart
            // 
            resources.ApplyResources(this.editCustPart, "editCustPart");
            this.editCustPart.Name = "editCustPart";
            // 
            // editPart
            // 
            resources.ApplyResources(this.editPart, "editPart");
            this.editPart.BackColor = System.Drawing.Color.Yellow;
            this.editPart.Name = "editPart";
            // 
            // LabCustPart
            // 
            resources.ApplyResources(this.LabCustPart, "LabCustPart");
            this.LabCustPart.BackColor = System.Drawing.Color.Transparent;
            this.LabCustPart.Name = "LabCustPart";
            // 
            // LabPart
            // 
            resources.ApplyResources(this.LabPart, "LabPart");
            this.LabPart.BackColor = System.Drawing.Color.Transparent;
            this.LabPart.Name = "LabPart";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.editVendorPart, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.LabVendorPart, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.editPart, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.LabPart, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.editSpec2, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.LabSpec2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbOldCode, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.editVersion, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbUnitCount, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabVersion, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblOldCode, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblUnitCount, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbBluePrint, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbProductWeight, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.LabRoute, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.combRoute, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.editCustPart, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblBluePrint, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.LabCustPart, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblProductWeight, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.LabRuleSet, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.combRuleSet, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblCustomerCode, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.editSpec1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.LabSpec1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.LabUnit, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.s, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.TbCustomerCode, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.BtnCustomer, 4, 5);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.LbChildPart, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.TbChildPart, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblT4Time, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbT4Time, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblT4T6TimeInt, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbT4T6TimeInt, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblT6Time, 5, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbT6Time, 6, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblMaterial, 5, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbMaterial, 6, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblForgingWeight, 5, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbForgingWeight, 6, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblForgingNo, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbForgingNo, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblToPallet, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbToPallet, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.TbNewChildPartNo, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.BtnChildPart, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.LbEndProcess, 5, 6);
            this.tableLayoutPanel1.Controls.Add(this.TbEndProcess, 6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // TbCustomerCode
            // 
            resources.ApplyResources(this.TbCustomerCode, "TbCustomerCode");
            this.TbCustomerCode.Name = "TbCustomerCode";
            this.TbCustomerCode.ReadOnly = true;
            // 
            // BtnCustomer
            // 
            resources.ApplyResources(this.BtnCustomer, "BtnCustomer");
            this.BtnCustomer.Name = "BtnCustomer";
            this.BtnCustomer.UseVisualStyleBackColor = true;
            this.BtnCustomer.Click += new System.EventHandler(this.BtnCustomer_Click);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 8);
            this.tableLayoutPanel2.Controls.Add(this.btnOK, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // LbChildPart
            // 
            resources.ApplyResources(this.LbChildPart, "LbChildPart");
            this.LbChildPart.Name = "LbChildPart";
            // 
            // TbChildPart
            // 
            resources.ApplyResources(this.TbChildPart, "TbChildPart");
            this.TbChildPart.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TbChildPart.Name = "TbChildPart";
            this.TbChildPart.ReadOnly = true;
            // 
            // TbNewChildPartNo
            // 
            resources.ApplyResources(this.TbNewChildPartNo, "TbNewChildPartNo");
            this.TbNewChildPartNo.Name = "TbNewChildPartNo";
            this.TbNewChildPartNo.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // BtnChildPart
            // 
            resources.ApplyResources(this.BtnChildPart, "BtnChildPart");
            this.BtnChildPart.Name = "BtnChildPart";
            this.BtnChildPart.UseVisualStyleBackColor = true;
            this.BtnChildPart.Click += new System.EventHandler(this.BtnChildPart_Click);
            // 
            // LbEndProcess
            // 
            resources.ApplyResources(this.LbEndProcess, "LbEndProcess");
            this.LbEndProcess.Name = "LbEndProcess";
            // 
            // TbEndProcess
            // 
            resources.ApplyResources(this.TbEndProcess, "TbEndProcess");
            this.TbEndProcess.BackColor = System.Drawing.Color.Yellow;
            this.TbEndProcess.Name = "TbEndProcess";
            this.TbEndProcess.ReadOnly = true;
            // 
            // fData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "fData";
            this.Load += new System.EventHandler(this.fData_Load);
            this.Shown += new System.EventHandler(this.fData_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).EndInit();
            this.contextRouteMenu.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox editCustPart;
        private System.Windows.Forms.TextBox editPart;
        private System.Windows.Forms.Label LabCustPart;
        private System.Windows.Forms.Label LabPart;
        private System.Windows.Forms.TextBox editSpec2;
        private System.Windows.Forms.Label LabSpec2;
        private System.Windows.Forms.TextBox editVendorPart;
        private System.Windows.Forms.Label LabVendorPart;
        private System.Windows.Forms.TextBox editVersion;
        private System.Windows.Forms.Label LabVersion;
        private System.Windows.Forms.Label LabRoute;
        private System.Windows.Forms.Label LabRuleSet;
        private System.Windows.Forms.TextBox s;
        private System.Windows.Forms.Label LabUnit;
        private System.Windows.Forms.TextBox editSpec1;
        private System.Windows.Forms.Label LabSpec1;
        private System.Windows.Forms.ComboBox combRoute;
        private System.Windows.Forms.ComboBox combRuleSet;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuAppendPKSpec;
        private System.Windows.Forms.ToolStripMenuItem MenuDeletePKSpec;
        private System.Windows.Forms.ContextMenuStrip contextRouteMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuAppendRoute;
        private System.Windows.Forms.ToolStripMenuItem MenuDeleteRoute;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView LVPKSpec;
        private System.Windows.Forms.ColumnHeader columnSpec;
        private System.Windows.Forms.ColumnHeader columnBox;
        private System.Windows.Forms.ColumnHeader columnCarton;
        private System.Windows.Forms.ColumnHeader columnPallet;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvRoute;
        private System.Windows.Forms.Label lblBluePrint;
        private System.Windows.Forms.TextBox tbT6Time;
        private System.Windows.Forms.TextBox tbT4T6TimeInt;
        private System.Windows.Forms.TextBox tbT4Time;
        private System.Windows.Forms.TextBox tbUnitCount;
        private System.Windows.Forms.Label lblT6Time;
        private System.Windows.Forms.Label lblT4T6TimeInt;
        private System.Windows.Forms.Label lblT4Time;
        private System.Windows.Forms.Label lblUnitCount;
        private System.Windows.Forms.TextBox tbMaterial;
        private System.Windows.Forms.TextBox tbForgingWeight;
        private System.Windows.Forms.TextBox tbForgingNo;
        private System.Windows.Forms.TextBox tbProductWeight;
        private System.Windows.Forms.TextBox tbBluePrint;
        private System.Windows.Forms.Label lblMaterial;
        private System.Windows.Forms.Label lblForgingWeight;
        private System.Windows.Forms.Label lblForgingNo;
        private System.Windows.Forms.Label lblCustomerCode;
        private System.Windows.Forms.Label lblProductWeight;
        private System.Windows.Forms.TextBox tbOldCode;
        private System.Windows.Forms.Label lblOldCode;
        private System.Windows.Forms.TextBox tbToPallet;
        private System.Windows.Forms.Label lblToPallet;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox TbCustomerCode;
        private System.Windows.Forms.Button BtnCustomer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label LbChildPart;
        private System.Windows.Forms.TextBox TbChildPart;
        private System.Windows.Forms.Button BtnChildPart;
        private System.Windows.Forms.TextBox TbNewChildPartNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem TsmiRouteDetail;
        private System.Windows.Forms.Label LbEndProcess;
        private System.Windows.Forms.TextBox TbEndProcess;
        private System.Windows.Forms.DataGridViewTextBoxColumn ROUTE_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn ROUTE_DESC;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMP_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn UPDATE_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn ROUTE_ID;
    }
}