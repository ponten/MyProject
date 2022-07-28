namespace RCOutput_Route
{
    partial class fMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LayoutInfo = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.LbProductName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LbWorkOrder = new System.Windows.Forms.TextBox();
            this.LbPart = new System.Windows.Forms.TextBox();
            this.LbVersion = new System.Windows.Forms.TextBox();
            this.LbRemark = new System.Windows.Forms.TextBox();
            this.LbRouteName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.LbSpecification = new System.Windows.Forms.TextBox();
            this.tsDetial = new System.Windows.Forms.ToolStripMenuItem();
            this.tsDll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cmbParam = new System.Windows.Forms.ToolStripLabel();
            this.txtInput = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tsEmp = new System.Windows.Forms.ToolStripLabel();
            this.cmsTabpage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanelRemark = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tsMemo = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.statusStripMessage = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvRC = new System.Windows.Forms.DataGridView();
            this.GridLayout = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.groupBoxRC = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnSelectAll = new System.Windows.Forms.Button();
            this.BtnResetAll = new System.Windows.Forms.Button();
            this.groupBoxProcess = new System.Windows.Forms.GroupBox();
            this.dgvProcess = new System.Windows.Forms.DataGridView();
            this.CHECK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PROCESS_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROCESS_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NODE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NEXT_NODE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPERATE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPTION1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPTION2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPTION3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LbProcessName = new System.Windows.Forms.Label();
            this.LbFormerNO = new System.Windows.Forms.Label();
            this.LbBluePrint = new System.Windows.Forms.Label();
            this.SelectRuncards = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RC_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CURRENT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1.SuspendLayout();
            this.LayoutInfo.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.cmsTabpage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.tableLayoutPanelRemark.SuspendLayout();
            this.statusStripMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRC)).BeginInit();
            this.GridLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.groupBoxRC.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBoxProcess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcess)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "mouse.gif");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(144, 48);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            // 
            // LayoutInfo
            // 
            this.LayoutInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LayoutInfo.BackColor = System.Drawing.Color.Transparent;
            this.LayoutInfo.ColumnCount = 4;
            this.LayoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.LayoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.LayoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.LayoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.LayoutInfo.Controls.Add(this.label2, 0, 0);
            this.LayoutInfo.Controls.Add(this.label3, 0, 1);
            this.LayoutInfo.Controls.Add(this.label4, 0, 2);
            this.LayoutInfo.Controls.Add(this.label14, 2, 2);
            this.LayoutInfo.Controls.Add(this.LbProductName, 3, 1);
            this.LayoutInfo.Controls.Add(this.label12, 2, 0);
            this.LayoutInfo.Controls.Add(this.label5, 0, 3);
            this.LayoutInfo.Controls.Add(this.LbWorkOrder, 1, 0);
            this.LayoutInfo.Controls.Add(this.LbPart, 1, 1);
            this.LayoutInfo.Controls.Add(this.LbVersion, 1, 2);
            this.LayoutInfo.Controls.Add(this.LbRemark, 1, 3);
            this.LayoutInfo.Controls.Add(this.LbRouteName, 3, 0);
            this.LayoutInfo.Controls.Add(this.label13, 2, 1);
            this.LayoutInfo.Controls.Add(this.LbSpecification, 3, 2);
            this.LayoutInfo.Location = new System.Drawing.Point(0, 100);
            this.LayoutInfo.Margin = new System.Windows.Forms.Padding(0);
            this.LayoutInfo.Name = "LayoutInfo";
            this.LayoutInfo.RowCount = 4;
            this.LayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.LayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.LayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.LayoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.LayoutInfo.Size = new System.Drawing.Size(884, 112);
            this.LayoutInfo.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(4, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "WorkOrder";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(4, 34);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Part No";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(4, 62);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Version";
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.Location = new System.Drawing.Point(438, 62);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(89, 16);
            this.label14.TabIndex = 12;
            this.label14.Text = "Specification";
            // 
            // LbProductName
            // 
            this.LbProductName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LbProductName.BackColor = System.Drawing.SystemColors.Control;
            this.LbProductName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbProductName.Location = new System.Drawing.Point(539, 28);
            this.LbProductName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.LbProductName.Name = "LbProductName";
            this.LbProductName.ReadOnly = true;
            this.LbProductName.Size = new System.Drawing.Size(342, 27);
            this.LbProductName.TabIndex = 5;
            this.LbProductName.Text = "--";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(438, 6);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 16);
            this.label12.TabIndex = 10;
            this.label12.Text = "Route name";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(4, 90);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "Remark";
            // 
            // LbWorkOrder
            // 
            this.LbWorkOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LbWorkOrder.BackColor = System.Drawing.SystemColors.Control;
            this.LbWorkOrder.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbWorkOrder.Location = new System.Drawing.Point(90, 0);
            this.LbWorkOrder.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.LbWorkOrder.Name = "LbWorkOrder";
            this.LbWorkOrder.ReadOnly = true;
            this.LbWorkOrder.Size = new System.Drawing.Size(341, 27);
            this.LbWorkOrder.TabIndex = 7;
            this.LbWorkOrder.Text = "--";
            // 
            // LbPart
            // 
            this.LbPart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LbPart.BackColor = System.Drawing.SystemColors.Control;
            this.LbPart.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbPart.Location = new System.Drawing.Point(90, 28);
            this.LbPart.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.LbPart.Name = "LbPart";
            this.LbPart.ReadOnly = true;
            this.LbPart.Size = new System.Drawing.Size(341, 27);
            this.LbPart.TabIndex = 8;
            this.LbPart.Text = "--";
            // 
            // LbVersion
            // 
            this.LbVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LbVersion.BackColor = System.Drawing.SystemColors.Control;
            this.LbVersion.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbVersion.Location = new System.Drawing.Point(90, 56);
            this.LbVersion.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.LbVersion.Name = "LbVersion";
            this.LbVersion.ReadOnly = true;
            this.LbVersion.Size = new System.Drawing.Size(341, 27);
            this.LbVersion.TabIndex = 9;
            this.LbVersion.Text = "--";
            // 
            // LbRemark
            // 
            this.LbRemark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LbRemark.BackColor = System.Drawing.SystemColors.Control;
            this.LayoutInfo.SetColumnSpan(this.LbRemark, 3);
            this.LbRemark.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbRemark.Location = new System.Drawing.Point(90, 84);
            this.LbRemark.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.LbRemark.Name = "LbRemark";
            this.LbRemark.ReadOnly = true;
            this.LbRemark.Size = new System.Drawing.Size(791, 27);
            this.LbRemark.TabIndex = 13;
            this.LbRemark.Text = "--";
            // 
            // LbRouteName
            // 
            this.LbRouteName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LbRouteName.BackColor = System.Drawing.SystemColors.Control;
            this.LbRouteName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbRouteName.Location = new System.Drawing.Point(539, 0);
            this.LbRouteName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.LbRouteName.Name = "LbRouteName";
            this.LbRouteName.ReadOnly = true;
            this.LbRouteName.Size = new System.Drawing.Size(342, 27);
            this.LbRouteName.TabIndex = 4;
            this.LbRouteName.Text = "--";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.Location = new System.Drawing.Point(438, 34);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(94, 16);
            this.label13.TabIndex = 11;
            this.label13.Text = "Product name";
            // 
            // LbSpecification
            // 
            this.LbSpecification.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LbSpecification.BackColor = System.Drawing.SystemColors.Control;
            this.LbSpecification.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbSpecification.Location = new System.Drawing.Point(539, 56);
            this.LbSpecification.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.LbSpecification.Name = "LbSpecification";
            this.LbSpecification.ReadOnly = true;
            this.LbSpecification.Size = new System.Drawing.Size(342, 27);
            this.LbSpecification.TabIndex = 6;
            this.LbSpecification.Text = "--";
            // 
            // tsDetial
            // 
            this.tsDetial.Name = "tsDetial";
            this.tsDetial.Size = new System.Drawing.Size(116, 22);
            this.tsDetial.Text = "tsDetail";
            // 
            // tsDll
            // 
            this.tsDll.Name = "tsDll";
            this.tsDll.Size = new System.Drawing.Size(116, 22);
            this.tsDll.Text = "tsDll";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmbParam,
            this.txtInput,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.tsEmp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(884, 25);
            this.toolStrip1.TabIndex = 37;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cmbParam
            // 
            this.cmbParam.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cmbParam.Name = "cmbParam";
            this.cmbParam.Size = new System.Drawing.Size(46, 22);
            this.cmbParam.Text = "RC No";
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(200, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(63, 22);
            this.toolStripLabel2.Text = "Employee";
            // 
            // tsEmp
            // 
            this.tsEmp.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tsEmp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.tsEmp.Name = "tsEmp";
            this.tsEmp.Size = new System.Drawing.Size(0, 22);
            // 
            // cmsTabpage
            // 
            this.cmsTabpage.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsTabpage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsDll,
            this.tsDetial});
            this.cmsTabpage.Name = "cmsTabpage";
            this.cmsTabpage.Size = new System.Drawing.Size(117, 48);
            // 
            // tableLayoutPanelRemark
            // 
            this.tableLayoutPanelRemark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelRemark.ColumnCount = 4;
            this.tableLayoutPanelRemark.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRemark.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRemark.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRemark.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRemark.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelRemark.Controls.Add(this.tsMemo, 1, 0);
            this.tableLayoutPanelRemark.Controls.Add(this.buttonOK, 2, 0);
            this.tableLayoutPanelRemark.Controls.Add(this.buttonCancel, 3, 0);
            this.tableLayoutPanelRemark.Location = new System.Drawing.Point(0, 504);
            this.tableLayoutPanelRemark.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelRemark.Name = "tableLayoutPanelRemark";
            this.tableLayoutPanelRemark.RowCount = 1;
            this.tableLayoutPanelRemark.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelRemark.Size = new System.Drawing.Size(884, 35);
            this.tableLayoutPanelRemark.TabIndex = 44;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Remark";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsMemo
            // 
            this.tsMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tsMemo.Location = new System.Drawing.Point(69, 4);
            this.tsMemo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tsMemo.Name = "tsMemo";
            this.tsMemo.Size = new System.Drawing.Size(595, 27);
            this.tsMemo.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonOK.Location = new System.Drawing.Point(672, 4);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 27);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonCancel.Location = new System.Drawing.Point(780, 4);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 27);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // statusStripMessage
            // 
            this.statusStripMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.statusStripMessage.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStripMessage.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsMsg});
            this.statusStripMessage.Location = new System.Drawing.Point(0, 539);
            this.statusStripMessage.Name = "statusStripMessage";
            this.statusStripMessage.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStripMessage.Size = new System.Drawing.Size(884, 22);
            this.statusStripMessage.TabIndex = 45;
            this.statusStripMessage.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(58, 17);
            this.toolStripStatusLabel1.Text = "Message";
            // 
            // tsMsg
            // 
            this.tsMsg.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tsMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.tsMsg.Name = "tsMsg";
            this.tsMsg.Size = new System.Drawing.Size(0, 17);
            // 
            // dgvRC
            // 
            this.dgvRC.AllowUserToAddRows = false;
            this.dgvRC.AllowUserToDeleteRows = false;
            this.dgvRC.AllowUserToResizeRows = false;
            this.dgvRC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRC.BackgroundColor = System.Drawing.Color.White;
            this.dgvRC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectRuncards,
            this.RC_NO,
            this.CURRENT_QTY});
            this.tableLayoutPanel2.SetColumnSpan(this.dgvRC, 2);
            this.dgvRC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRC.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvRC.Location = new System.Drawing.Point(3, 33);
            this.dgvRC.Name = "dgvRC";
            this.dgvRC.RowHeadersWidth = 51;
            this.dgvRC.RowTemplate.Height = 27;
            this.dgvRC.Size = new System.Drawing.Size(341, 224);
            this.dgvRC.TabIndex = 0;
            // 
            // GridLayout
            // 
            this.GridLayout.ColumnCount = 1;
            this.GridLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.GridLayout.Controls.Add(this.LayoutInfo, 0, 2);
            this.GridLayout.Controls.Add(this.toolStrip1, 0, 0);
            this.GridLayout.Controls.Add(this.tableLayoutPanelRemark, 0, 4);
            this.GridLayout.Controls.Add(this.statusStripMessage, 0, 5);
            this.GridLayout.Controls.Add(this.splitContainerMain, 0, 3);
            this.GridLayout.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.GridLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridLayout.Location = new System.Drawing.Point(0, 0);
            this.GridLayout.Name = "GridLayout";
            this.GridLayout.RowCount = 6;
            this.GridLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.GridLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.GridLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.GridLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.GridLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.GridLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.GridLayout.Size = new System.Drawing.Size(884, 561);
            this.GridLayout.TabIndex = 47;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(3, 215);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.groupBoxRC);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.groupBoxProcess);
            this.splitContainerMain.Size = new System.Drawing.Size(878, 286);
            this.splitContainerMain.SplitterDistance = 353;
            this.splitContainerMain.TabIndex = 47;
            // 
            // groupBoxRC
            // 
            this.groupBoxRC.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxRC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRC.Location = new System.Drawing.Point(0, 0);
            this.groupBoxRC.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxRC.Name = "groupBoxRC";
            this.groupBoxRC.Size = new System.Drawing.Size(353, 286);
            this.groupBoxRC.TabIndex = 44;
            this.groupBoxRC.TabStop = false;
            this.groupBoxRC.Text = "RunCards";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.dgvRC, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.BtnSelectAll, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.BtnResetAll, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(347, 260);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // BtnSelectAll
            // 
            this.BtnSelectAll.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnSelectAll.Location = new System.Drawing.Point(53, 1);
            this.BtnSelectAll.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.BtnSelectAll.Name = "BtnSelectAll";
            this.BtnSelectAll.Size = new System.Drawing.Size(100, 27);
            this.BtnSelectAll.TabIndex = 1;
            this.BtnSelectAll.Text = "Select all";
            this.BtnSelectAll.UseVisualStyleBackColor = true;
            // 
            // BtnResetAll
            // 
            this.BtnResetAll.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BtnResetAll.Location = new System.Drawing.Point(193, 1);
            this.BtnResetAll.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.BtnResetAll.Name = "BtnResetAll";
            this.BtnResetAll.Size = new System.Drawing.Size(100, 27);
            this.BtnResetAll.TabIndex = 2;
            this.BtnResetAll.Text = "Reset all";
            this.BtnResetAll.UseVisualStyleBackColor = true;
            // 
            // groupBoxProcess
            // 
            this.groupBoxProcess.Controls.Add(this.dgvProcess);
            this.groupBoxProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProcess.Location = new System.Drawing.Point(0, 0);
            this.groupBoxProcess.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxProcess.Name = "groupBoxProcess";
            this.groupBoxProcess.Size = new System.Drawing.Size(521, 286);
            this.groupBoxProcess.TabIndex = 1;
            this.groupBoxProcess.TabStop = false;
            this.groupBoxProcess.Text = "Process";
            // 
            // dgvProcess
            // 
            this.dgvProcess.AllowUserToAddRows = false;
            this.dgvProcess.AllowUserToDeleteRows = false;
            this.dgvProcess.AllowUserToResizeRows = false;
            this.dgvProcess.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvProcess.BackgroundColor = System.Drawing.Color.White;
            this.dgvProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProcess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CHECK,
            this.PROCESS_ID,
            this.PROCESS_NAME,
            this.NODE_ID,
            this.NEXT_NODE_ID,
            this.OPERATE_ID,
            this.OPTION1,
            this.OPTION2,
            this.OPTION3});
            this.dgvProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProcess.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvProcess.Location = new System.Drawing.Point(3, 23);
            this.dgvProcess.Name = "dgvProcess";
            this.dgvProcess.RowHeadersWidth = 51;
            this.dgvProcess.RowTemplate.Height = 27;
            this.dgvProcess.Size = new System.Drawing.Size(515, 260);
            this.dgvProcess.TabIndex = 0;
            this.dgvProcess.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProcess_CellValueChanged);
            this.dgvProcess.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvProcess_CurrentCellDirtyStateChanged);
            // 
            // CHECK
            // 
            this.CHECK.DataPropertyName = "CHECK";
            this.CHECK.HeaderText = "CHECK";
            this.CHECK.MinimumWidth = 6;
            this.CHECK.Name = "CHECK";
            this.CHECK.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CHECK.Width = 65;
            // 
            // PROCESS_ID
            // 
            this.PROCESS_ID.DataPropertyName = "PROCESS_ID";
            this.PROCESS_ID.HeaderText = "PROCESS_ID";
            this.PROCESS_ID.MinimumWidth = 6;
            this.PROCESS_ID.Name = "PROCESS_ID";
            this.PROCESS_ID.ReadOnly = true;
            this.PROCESS_ID.Visible = false;
            this.PROCESS_ID.Width = 121;
            // 
            // PROCESS_NAME
            // 
            this.PROCESS_NAME.DataPropertyName = "PROCESS_NAME";
            this.PROCESS_NAME.HeaderText = "Process name";
            this.PROCESS_NAME.MinimumWidth = 6;
            this.PROCESS_NAME.Name = "PROCESS_NAME";
            this.PROCESS_NAME.ReadOnly = true;
            this.PROCESS_NAME.Width = 108;
            // 
            // NODE_ID
            // 
            this.NODE_ID.DataPropertyName = "NODE_ID";
            this.NODE_ID.HeaderText = "NODE_ID";
            this.NODE_ID.MinimumWidth = 6;
            this.NODE_ID.Name = "NODE_ID";
            this.NODE_ID.ReadOnly = true;
            this.NODE_ID.Visible = false;
            this.NODE_ID.Width = 99;
            // 
            // NEXT_NODE_ID
            // 
            this.NEXT_NODE_ID.DataPropertyName = "NEXT_NODE_ID";
            this.NEXT_NODE_ID.HeaderText = "NEXT_NODE_ID";
            this.NEXT_NODE_ID.MinimumWidth = 6;
            this.NEXT_NODE_ID.Name = "NEXT_NODE_ID";
            this.NEXT_NODE_ID.ReadOnly = true;
            this.NEXT_NODE_ID.Visible = false;
            this.NEXT_NODE_ID.Width = 147;
            // 
            // OPERATE_ID
            // 
            this.OPERATE_ID.DataPropertyName = "OPERATE_ID";
            this.OPERATE_ID.HeaderText = "OPERATE_ID";
            this.OPERATE_ID.MinimumWidth = 6;
            this.OPERATE_ID.Name = "OPERATE_ID";
            this.OPERATE_ID.ReadOnly = true;
            this.OPERATE_ID.Visible = false;
            this.OPERATE_ID.Width = 124;
            // 
            // OPTION1
            // 
            this.OPTION1.DataPropertyName = "OPTION1";
            this.OPTION1.HeaderText = "Print WP label";
            this.OPTION1.Name = "OPTION1";
            this.OPTION1.ReadOnly = true;
            this.OPTION1.Width = 112;
            // 
            // OPTION2
            // 
            this.OPTION2.DataPropertyName = "OPTION2";
            this.OPTION2.HeaderText = "Is T4 stove";
            this.OPTION2.Name = "OPTION2";
            this.OPTION2.ReadOnly = true;
            this.OPTION2.Width = 94;
            // 
            // OPTION3
            // 
            this.OPTION3.DataPropertyName = "OPTION3";
            this.OPTION3.HeaderText = "Is T6 stove";
            this.OPTION3.Name = "OPTION3";
            this.OPTION3.ReadOnly = true;
            this.OPTION3.Width = 94;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.LbProcessName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LbFormerNO, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.LbBluePrint, 2, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(884, 75);
            this.tableLayoutPanel1.TabIndex = 48;
            // 
            // LbProcessName
            // 
            this.LbProcessName.AutoSize = true;
            this.LbProcessName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LbProcessName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbProcessName.Font = new System.Drawing.Font("新細明體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbProcessName.ForeColor = System.Drawing.Color.Red;
            this.LbProcessName.Location = new System.Drawing.Point(8, 7);
            this.LbProcessName.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.LbProcessName.Name = "LbProcessName";
            this.LbProcessName.Size = new System.Drawing.Size(228, 61);
            this.LbProcessName.TabIndex = 48;
            this.LbProcessName.Text = "Process name";
            this.LbProcessName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LbFormerNO
            // 
            this.LbFormerNO.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LbFormerNO.AutoSize = true;
            this.LbFormerNO.ForeColor = System.Drawing.Color.Red;
            this.LbFormerNO.Location = new System.Drawing.Point(247, 24);
            this.LbFormerNO.Name = "LbFormerNO";
            this.LbFormerNO.Size = new System.Drawing.Size(129, 27);
            this.LbFormerNO.TabIndex = 49;
            this.LbFormerNO.Text = "Former NO";
            // 
            // LbBluePrint
            // 
            this.LbBluePrint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LbBluePrint.AutoSize = true;
            this.LbBluePrint.ForeColor = System.Drawing.Color.Red;
            this.LbBluePrint.Location = new System.Drawing.Point(382, 24);
            this.LbBluePrint.Name = "LbBluePrint";
            this.LbBluePrint.Size = new System.Drawing.Size(108, 27);
            this.LbBluePrint.TabIndex = 50;
            this.LbBluePrint.Text = "Blueprint";
            // 
            // SelectRuncards
            // 
            this.SelectRuncards.DataPropertyName = "SELECT";
            this.SelectRuncards.FalseValue = "0";
            this.SelectRuncards.HeaderText = "Select";
            this.SelectRuncards.Name = "SelectRuncards";
            this.SelectRuncards.ReadOnly = true;
            this.SelectRuncards.TrueValue = "1";
            this.SelectRuncards.Width = 51;
            // 
            // RC_NO
            // 
            this.RC_NO.DataPropertyName = "RC_NO";
            this.RC_NO.HeaderText = "RC_NO";
            this.RC_NO.MinimumWidth = 6;
            this.RC_NO.Name = "RC_NO";
            this.RC_NO.ReadOnly = true;
            this.RC_NO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RC_NO.Width = 64;
            // 
            // CURRENT_QTY
            // 
            this.CURRENT_QTY.DataPropertyName = "CURRENT_QTY";
            this.CURRENT_QTY.HeaderText = "CURRENT_QTY";
            this.CURRENT_QTY.MinimumWidth = 6;
            this.CURRENT_QTY.Name = "CURRENT_QTY";
            this.CURRENT_QTY.ReadOnly = true;
            this.CURRENT_QTY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CURRENT_QTY.Width = 123;
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.GridLayout);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RC Output WO";
            this.Load += new System.EventHandler(this.FMain_Load);
            this.Shown += new System.EventHandler(this.FMain_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.LayoutInfo.ResumeLayout(false);
            this.LayoutInfo.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cmsTabpage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.tableLayoutPanelRemark.ResumeLayout(false);
            this.tableLayoutPanelRemark.PerformLayout();
            this.statusStripMessage.ResumeLayout(false);
            this.statusStripMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRC)).EndInit();
            this.GridLayout.ResumeLayout(false);
            this.GridLayout.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.groupBoxRC.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBoxProcess.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcess)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel LayoutInfo;
        private System.Windows.Forms.ToolStripMenuItem tsDetial;
        private System.Windows.Forms.ToolStripMenuItem tsDll;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox txtInput;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip cmsTabpage;
        private System.Windows.Forms.ToolStripLabel cmbParam;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel tsEmp;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRemark;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tsMemo;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.StatusStrip statusStripMessage;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsMsg;
        private System.Windows.Forms.DataGridView dgvRC;
        private System.Windows.Forms.TableLayoutPanel GridLayout;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.GroupBox groupBoxRC;
        private System.Windows.Forms.Label LbProcessName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label LbFormerNO;
        private System.Windows.Forms.Label LbBluePrint;
        private System.Windows.Forms.GroupBox groupBoxProcess;
        private System.Windows.Forms.DataGridView dgvProcess;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox LbRouteName;
        private System.Windows.Forms.TextBox LbProductName;
        private System.Windows.Forms.TextBox LbSpecification;
        private System.Windows.Forms.TextBox LbWorkOrder;
        private System.Windows.Forms.TextBox LbPart;
        private System.Windows.Forms.TextBox LbVersion;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox LbRemark;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button BtnSelectAll;
        private System.Windows.Forms.Button BtnResetAll;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CHECK;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROCESS_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROCESS_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn NODE_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NEXT_NODE_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPERATE_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPTION1;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPTION2;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPTION3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectRuncards;
        private System.Windows.Forms.DataGridViewTextBoxColumn RC_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn CURRENT_QTY;
    }
}