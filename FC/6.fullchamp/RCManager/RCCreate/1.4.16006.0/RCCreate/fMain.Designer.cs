namespace RCCreate
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.btnAppend = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.gvData = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.LabFilter = new System.Windows.Forms.ToolStripLabel();
            this.combFilter = new System.Windows.Forms.ToolStripComboBox();
            this.editFilter = new System.Windows.Forms.ToolStripTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.editSpec2 = new System.Windows.Forms.TextBox();
            this.LabSpec2 = new System.Windows.Forms.Label();
            this.editSpec1 = new System.Windows.Forms.TextBox();
            this.LabSpec1 = new System.Windows.Forms.Label();
            this.editType = new System.Windows.Forms.TextBox();
            this.editPDLine = new System.Windows.Forms.TextBox();
            this.editRoute = new System.Windows.Forms.TextBox();
            this.editCustomer = new System.Windows.Forms.TextBox();
            this.editPriority = new System.Windows.Forms.TextBox();
            this.LabPriority = new System.Windows.Forms.Label();
            this.LabRouteName = new System.Windows.Forms.Label();
            this.LabCustomer = new System.Windows.Forms.Label();
            this.LabWOType = new System.Windows.Forms.Label();
            this.LabPDLine = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gvComponent = new System.Windows.Forms.DataGridView();
            this.editQty = new System.Windows.Forms.TextBox();
            this.editPart = new System.Windows.Forms.TextBox();
            this.editVersion = new System.Windows.Forms.TextBox();
            this.editDue = new System.Windows.Forms.TextBox();
            this.editRCNo = new System.Windows.Forms.TextBox();
            this.editWO = new System.Windows.Forms.TextBox();
            this.LabWO = new System.Windows.Forms.Label();
            this.LabPartNo = new System.Windows.Forms.Label();
            this.LabVersion = new System.Windows.Forms.Label();
            this.LabDueDate = new System.Windows.Forms.Label();
            this.LabRCNo = new System.Windows.Forms.Label();
            this.LabQty = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvComponent)).BeginInit();
            this.SuspendLayout();
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAppend,
            this.btnDelete,
            this.bindingNavigatorSeparator2,
            this.btnExport,
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(879, 36);
            this.bindingNavigator1.TabIndex = 11;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // btnAppend
            // 
            this.btnAppend.Image = ((System.Drawing.Image)(resources.GetObject("btnAppend.Image")));
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.RightToLeftAutoMirrorImage = true;
            this.btnAppend.Size = new System.Drawing.Size(49, 33);
            this.btnAppend.Text = "Append";
            this.btnAppend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.RightToLeftAutoMirrorImage = true;
            this.btnDelete.Size = new System.Drawing.Size(41, 33);
            this.btnDelete.Tag = "";
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 36);
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(43, 33);
            this.btnExport.Text = "Export";
            this.btnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 33);
            this.bindingNavigatorMoveFirstItem.Text = "移到最前面";
            this.bindingNavigatorMoveFirstItem.Visible = false;
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 33);
            this.bindingNavigatorMovePreviousItem.Text = "移到上一個";
            this.bindingNavigatorMovePreviousItem.Visible = false;
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 36);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "位置";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(40, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "目前的位置";
            this.bindingNavigatorPositionItem.Visible = false;
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(28, 33);
            this.bindingNavigatorCountItem.Text = "/{0}";
            this.bindingNavigatorCountItem.ToolTipText = "項目總數";
            this.bindingNavigatorCountItem.Visible = false;
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 36);
            this.bindingNavigatorSeparator1.Visible = false;
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 33);
            this.bindingNavigatorMoveNextItem.Text = "移到下一個";
            this.bindingNavigatorMoveNextItem.Visible = false;
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 33);
            this.bindingNavigatorMoveLastItem.Text = "移到最後面";
            this.bindingNavigatorMoveLastItem.Visible = false;
            // 
            // gvData
            // 
            this.gvData.AllowUserToAddRows = false;
            this.gvData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.gvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvData.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gvData.ColumnHeadersHeight = 24;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvData.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvData.Location = new System.Drawing.Point(0, 0);
            this.gvData.Name = "gvData";
            this.gvData.ReadOnly = true;
            this.gvData.RowHeadersWidth = 25;
            this.gvData.RowTemplate.Height = 24;
            this.gvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvData.Size = new System.Drawing.Size(287, 421);
            this.gvData.TabIndex = 2;
            this.gvData.VirtualMode = true;
            this.gvData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvData_CellClick);
            this.gvData.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gvData_CellValueNeeded);
            this.gvData.SelectionChanged += new System.EventHandler(this.gvData_SelectionChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LabFilter,
            this.combFilter,
            this.editFilter});
            this.toolStrip1.Location = new System.Drawing.Point(0, 36);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(879, 25);
            this.toolStrip1.TabIndex = 17;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // LabFilter
            // 
            this.LabFilter.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabFilter.Name = "LabFilter";
            this.LabFilter.Size = new System.Drawing.Size(38, 22);
            this.LabFilter.Text = "Filter";
            // 
            // combFilter
            // 
            this.combFilter.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.combFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combFilter.Name = "combFilter";
            this.combFilter.Size = new System.Drawing.Size(121, 25);
            // 
            // editFilter
            // 
            this.editFilter.Name = "editFilter";
            this.editFilter.Size = new System.Drawing.Size(140, 25);
            this.editFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editFilter_KeyPress);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.gvData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 61);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(291, 425);
            this.panel1.TabIndex = 18;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.splitter1.Location = new System.Drawing.Point(291, 61);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 425);
            this.splitter1.TabIndex = 19;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.editSpec2);
            this.panel2.Controls.Add(this.LabSpec2);
            this.panel2.Controls.Add(this.editSpec1);
            this.panel2.Controls.Add(this.LabSpec1);
            this.panel2.Controls.Add(this.editType);
            this.panel2.Controls.Add(this.editPDLine);
            this.panel2.Controls.Add(this.editRoute);
            this.panel2.Controls.Add(this.editCustomer);
            this.panel2.Controls.Add(this.editPriority);
            this.panel2.Controls.Add(this.LabPriority);
            this.panel2.Controls.Add(this.LabRouteName);
            this.panel2.Controls.Add(this.LabCustomer);
            this.panel2.Controls.Add(this.LabWOType);
            this.panel2.Controls.Add(this.LabPDLine);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.editQty);
            this.panel2.Controls.Add(this.editPart);
            this.panel2.Controls.Add(this.editVersion);
            this.panel2.Controls.Add(this.editDue);
            this.panel2.Controls.Add(this.editRCNo);
            this.panel2.Controls.Add(this.editWO);
            this.panel2.Controls.Add(this.LabWO);
            this.panel2.Controls.Add(this.LabPartNo);
            this.panel2.Controls.Add(this.LabVersion);
            this.panel2.Controls.Add(this.LabDueDate);
            this.panel2.Controls.Add(this.LabRCNo);
            this.panel2.Controls.Add(this.LabQty);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(294, 61);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(585, 425);
            this.panel2.TabIndex = 20;
            // 
            // editSpec2
            // 
            this.editSpec2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editSpec2.ForeColor = System.Drawing.Color.Maroon;
            this.editSpec2.Location = new System.Drawing.Point(130, 64);
            this.editSpec2.Name = "editSpec2";
            this.editSpec2.ReadOnly = true;
            this.editSpec2.Size = new System.Drawing.Size(154, 22);
            this.editSpec2.TabIndex = 97;
            // 
            // LabSpec2
            // 
            this.LabSpec2.AutoSize = true;
            this.LabSpec2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabSpec2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabSpec2.Location = new System.Drawing.Point(6, 68);
            this.LabSpec2.Name = "LabSpec2";
            this.LabSpec2.Size = new System.Drawing.Size(44, 17);
            this.LabSpec2.TabIndex = 96;
            this.LabSpec2.Text = "Spec2";
            // 
            // editSpec1
            // 
            this.editSpec1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editSpec1.ForeColor = System.Drawing.Color.Maroon;
            this.editSpec1.Location = new System.Drawing.Point(130, 37);
            this.editSpec1.Name = "editSpec1";
            this.editSpec1.ReadOnly = true;
            this.editSpec1.Size = new System.Drawing.Size(154, 22);
            this.editSpec1.TabIndex = 95;
            // 
            // LabSpec1
            // 
            this.LabSpec1.AutoSize = true;
            this.LabSpec1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabSpec1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabSpec1.Location = new System.Drawing.Point(6, 41);
            this.LabSpec1.Name = "LabSpec1";
            this.LabSpec1.Size = new System.Drawing.Size(44, 17);
            this.LabSpec1.TabIndex = 94;
            this.LabSpec1.Text = "Spec1";
            // 
            // editType
            // 
            this.editType.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editType.ForeColor = System.Drawing.Color.Maroon;
            this.editType.Location = new System.Drawing.Point(424, 10);
            this.editType.Name = "editType";
            this.editType.ReadOnly = true;
            this.editType.Size = new System.Drawing.Size(154, 22);
            this.editType.TabIndex = 93;
            // 
            // editPDLine
            // 
            this.editPDLine.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editPDLine.ForeColor = System.Drawing.Color.Maroon;
            this.editPDLine.Location = new System.Drawing.Point(424, 37);
            this.editPDLine.Name = "editPDLine";
            this.editPDLine.ReadOnly = true;
            this.editPDLine.Size = new System.Drawing.Size(154, 22);
            this.editPDLine.TabIndex = 92;
            // 
            // editRoute
            // 
            this.editRoute.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editRoute.ForeColor = System.Drawing.Color.Maroon;
            this.editRoute.Location = new System.Drawing.Point(424, 64);
            this.editRoute.Name = "editRoute";
            this.editRoute.ReadOnly = true;
            this.editRoute.Size = new System.Drawing.Size(154, 22);
            this.editRoute.TabIndex = 91;
            // 
            // editCustomer
            // 
            this.editCustomer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editCustomer.ForeColor = System.Drawing.Color.Maroon;
            this.editCustomer.Location = new System.Drawing.Point(424, 91);
            this.editCustomer.Name = "editCustomer";
            this.editCustomer.ReadOnly = true;
            this.editCustomer.Size = new System.Drawing.Size(154, 22);
            this.editCustomer.TabIndex = 90;
            // 
            // editPriority
            // 
            this.editPriority.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editPriority.ForeColor = System.Drawing.Color.Maroon;
            this.editPriority.Location = new System.Drawing.Point(424, 172);
            this.editPriority.Name = "editPriority";
            this.editPriority.ReadOnly = true;
            this.editPriority.Size = new System.Drawing.Size(154, 22);
            this.editPriority.TabIndex = 89;
            // 
            // LabPriority
            // 
            this.LabPriority.AutoSize = true;
            this.LabPriority.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabPriority.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabPriority.Location = new System.Drawing.Point(300, 176);
            this.LabPriority.Name = "LabPriority";
            this.LabPriority.Size = new System.Drawing.Size(52, 17);
            this.LabPriority.TabIndex = 84;
            this.LabPriority.Text = "Priority";
            // 
            // LabRouteName
            // 
            this.LabRouteName.AutoSize = true;
            this.LabRouteName.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabRouteName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabRouteName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabRouteName.Location = new System.Drawing.Point(300, 68);
            this.LabRouteName.Name = "LabRouteName";
            this.LabRouteName.Size = new System.Drawing.Size(83, 17);
            this.LabRouteName.TabIndex = 87;
            this.LabRouteName.Text = "Route Name";
            // 
            // LabCustomer
            // 
            this.LabCustomer.AutoSize = true;
            this.LabCustomer.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabCustomer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabCustomer.Location = new System.Drawing.Point(300, 95);
            this.LabCustomer.Name = "LabCustomer";
            this.LabCustomer.Size = new System.Drawing.Size(68, 17);
            this.LabCustomer.TabIndex = 88;
            this.LabCustomer.Text = "Customer";
            // 
            // LabWOType
            // 
            this.LabWOType.AutoSize = true;
            this.LabWOType.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabWOType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabWOType.Location = new System.Drawing.Point(300, 14);
            this.LabWOType.Name = "LabWOType";
            this.LabWOType.Size = new System.Drawing.Size(69, 17);
            this.LabWOType.TabIndex = 86;
            this.LabWOType.Text = "W/O Type";
            // 
            // LabPDLine
            // 
            this.LabPDLine.AutoSize = true;
            this.LabPDLine.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabPDLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabPDLine.Location = new System.Drawing.Point(300, 41);
            this.LabPDLine.Name = "LabPDLine";
            this.LabPDLine.Size = new System.Drawing.Size(103, 17);
            this.LabPDLine.TabIndex = 85;
            this.LabPDLine.Text = "Production Line";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gvComponent);
            this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(5, 258);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 3, 6, 6);
            this.groupBox1.Size = new System.Drawing.Size(570, 163);
            this.groupBox1.TabIndex = 83;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Component";
            this.groupBox1.Visible = false;
            // 
            // gvComponent
            // 
            this.gvComponent.AllowUserToAddRows = false;
            this.gvComponent.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Lavender;
            this.gvComponent.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gvComponent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvComponent.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.gvComponent.ColumnHeadersHeight = 24;
            this.gvComponent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvComponent.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.gvComponent.Location = new System.Drawing.Point(6, 21);
            this.gvComponent.Name = "gvComponent";
            this.gvComponent.ReadOnly = true;
            this.gvComponent.RowTemplate.Height = 24;
            this.gvComponent.Size = new System.Drawing.Size(558, 136);
            this.gvComponent.TabIndex = 0;
            // 
            // editQty
            // 
            this.editQty.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editQty.ForeColor = System.Drawing.Color.Maroon;
            this.editQty.Location = new System.Drawing.Point(130, 199);
            this.editQty.Name = "editQty";
            this.editQty.ReadOnly = true;
            this.editQty.Size = new System.Drawing.Size(154, 22);
            this.editQty.TabIndex = 82;
            // 
            // editPart
            // 
            this.editPart.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editPart.ForeColor = System.Drawing.Color.Maroon;
            this.editPart.Location = new System.Drawing.Point(424, 118);
            this.editPart.Name = "editPart";
            this.editPart.ReadOnly = true;
            this.editPart.Size = new System.Drawing.Size(154, 22);
            this.editPart.TabIndex = 81;
            this.editPart.Visible = false;
            // 
            // editVersion
            // 
            this.editVersion.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editVersion.ForeColor = System.Drawing.Color.Maroon;
            this.editVersion.Location = new System.Drawing.Point(130, 91);
            this.editVersion.Name = "editVersion";
            this.editVersion.ReadOnly = true;
            this.editVersion.Size = new System.Drawing.Size(154, 22);
            this.editVersion.TabIndex = 80;
            // 
            // editDue
            // 
            this.editDue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editDue.ForeColor = System.Drawing.Color.Maroon;
            this.editDue.Location = new System.Drawing.Point(130, 118);
            this.editDue.Name = "editDue";
            this.editDue.ReadOnly = true;
            this.editDue.Size = new System.Drawing.Size(154, 22);
            this.editDue.TabIndex = 79;
            // 
            // editRCNo
            // 
            this.editRCNo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editRCNo.ForeColor = System.Drawing.Color.Maroon;
            this.editRCNo.Location = new System.Drawing.Point(130, 172);
            this.editRCNo.Name = "editRCNo";
            this.editRCNo.ReadOnly = true;
            this.editRCNo.Size = new System.Drawing.Size(154, 22);
            this.editRCNo.TabIndex = 78;
            // 
            // editWO
            // 
            this.editWO.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editWO.ForeColor = System.Drawing.Color.Maroon;
            this.editWO.Location = new System.Drawing.Point(130, 10);
            this.editWO.Name = "editWO";
            this.editWO.ReadOnly = true;
            this.editWO.Size = new System.Drawing.Size(154, 22);
            this.editWO.TabIndex = 77;
            // 
            // LabWO
            // 
            this.LabWO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.LabWO.AutoSize = true;
            this.LabWO.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabWO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabWO.Location = new System.Drawing.Point(6, 14);
            this.LabWO.Name = "LabWO";
            this.LabWO.Size = new System.Drawing.Size(80, 17);
            this.LabWO.TabIndex = 71;
            this.LabWO.Text = "Work Order";
            // 
            // LabPartNo
            // 
            this.LabPartNo.AutoSize = true;
            this.LabPartNo.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabPartNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabPartNo.Location = new System.Drawing.Point(300, 122);
            this.LabPartNo.Name = "LabPartNo";
            this.LabPartNo.Size = new System.Drawing.Size(54, 17);
            this.LabPartNo.TabIndex = 76;
            this.LabPartNo.Text = "Part No";
            this.LabPartNo.Visible = false;
            // 
            // LabVersion
            // 
            this.LabVersion.AutoSize = true;
            this.LabVersion.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabVersion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabVersion.Location = new System.Drawing.Point(6, 95);
            this.LabVersion.Name = "LabVersion";
            this.LabVersion.Size = new System.Drawing.Size(54, 17);
            this.LabVersion.TabIndex = 72;
            this.LabVersion.Text = "Version";
            // 
            // LabDueDate
            // 
            this.LabDueDate.AutoSize = true;
            this.LabDueDate.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabDueDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabDueDate.Location = new System.Drawing.Point(6, 122);
            this.LabDueDate.Name = "LabDueDate";
            this.LabDueDate.Size = new System.Drawing.Size(97, 17);
            this.LabDueDate.TabIndex = 73;
            this.LabDueDate.Text = "W/O Due Date";
            // 
            // LabRCNo
            // 
            this.LabRCNo.AutoSize = true;
            this.LabRCNo.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabRCNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabRCNo.Location = new System.Drawing.Point(6, 176);
            this.LabRCNo.Name = "LabRCNo";
            this.LabRCNo.Size = new System.Drawing.Size(85, 17);
            this.LabRCNo.TabIndex = 75;
            this.LabRCNo.Text = "Run Card No";
            // 
            // LabQty
            // 
            this.LabQty.AutoSize = true;
            this.LabQty.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabQty.Location = new System.Drawing.Point(6, 203);
            this.LabQty.Name = "LabQty";
            this.LabQty.Size = new System.Drawing.Size(81, 17);
            this.LabQty.TabIndex = 74;
            this.LabQty.Text = "Current Qty";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 486);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.bindingNavigator1);
            this.Name = "fMain";
            this.Text = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvComponent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton btnAppend;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.DataGridView gvData;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel LabFilter;
        private System.Windows.Forms.ToolStripComboBox combFilter;
        private System.Windows.Forms.ToolStripTextBox editFilter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox editType;
        private System.Windows.Forms.TextBox editPDLine;
        private System.Windows.Forms.TextBox editRoute;
        private System.Windows.Forms.TextBox editCustomer;
        private System.Windows.Forms.TextBox editPriority;
        private System.Windows.Forms.Label LabPriority;
        private System.Windows.Forms.Label LabRouteName;
        private System.Windows.Forms.Label LabCustomer;
        private System.Windows.Forms.Label LabWOType;
        private System.Windows.Forms.Label LabPDLine;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView gvComponent;
        private System.Windows.Forms.TextBox editQty;
        private System.Windows.Forms.TextBox editPart;
        private System.Windows.Forms.TextBox editVersion;
        private System.Windows.Forms.TextBox editDue;
        private System.Windows.Forms.TextBox editRCNo;
        private System.Windows.Forms.TextBox editWO;
        private System.Windows.Forms.Label LabWO;
        private System.Windows.Forms.Label LabPartNo;
        private System.Windows.Forms.Label LabVersion;
        private System.Windows.Forms.Label LabDueDate;
        private System.Windows.Forms.Label LabRCNo;
        private System.Windows.Forms.Label LabQty;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.Label LabSpec1;
        private System.Windows.Forms.TextBox editSpec1;
        private System.Windows.Forms.TextBox editSpec2;
        private System.Windows.Forms.Label LabSpec2;
    }
}