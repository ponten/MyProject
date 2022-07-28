namespace RCCreate
{
    partial class fData
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LabWO = new System.Windows.Forms.Label();
            this.LabPartNo = new System.Windows.Forms.Label();
            this.LabCustomer = new System.Windows.Forms.Label();
            this.LabRoute = new System.Windows.Forms.Label();
            this.LabPDLine = new System.Windows.Forms.Label();
            this.LabVersion = new System.Windows.Forms.Label();
            this.LabWOType = new System.Windows.Forms.Label();
            this.LabDueDate = new System.Windows.Forms.Label();
            this.LabAvailableQty = new System.Windows.Forms.Label();
            this.editWO = new System.Windows.Forms.TextBox();
            this.editPart = new System.Windows.Forms.TextBox();
            this.editVersion = new System.Windows.Forms.TextBox();
            this.editDue = new System.Windows.Forms.TextBox();
            this.editAvailableQty = new System.Windows.Forms.TextBox();
            this.btnSearchWO = new System.Windows.Forms.Button();
            this.editType = new System.Windows.Forms.TextBox();
            this.editPDLine = new System.Windows.Forms.TextBox();
            this.editRoute = new System.Windows.Forms.TextBox();
            this.editCustomer = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.LabRCNo = new System.Windows.Forms.Label();
            this.btnSelectQty = new System.Windows.Forms.Button();
            this.LabInputQty = new System.Windows.Forms.Label();
            this.combPriority = new System.Windows.Forms.ComboBox();
            this.LabPriority = new System.Windows.Forms.Label();
            this.editRCNo = new System.Windows.Forms.TextBox();
            this.btnSearchRC = new System.Windows.Forms.Button();
            this.editInputQty = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gvComponent = new System.Windows.Forms.DataGridView();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvComponent)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(427, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 2;
            this.btnOK.TabStop = false;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(514, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(5, 432);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(632, 44);
            this.panel2.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(0, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(632, 176);
            this.groupBox1.TabIndex = 63;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "W/O Information";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.Controls.Add(this.LabWO, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LabPartNo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.LabCustomer, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.LabRoute, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.LabPDLine, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabVersion, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabWOType, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.LabDueDate, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.LabAvailableQty, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.editWO, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.editPart, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.editVersion, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.editDue, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.editAvailableQty, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnSearchWO, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.editType, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.editPDLine, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.editRoute, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.editCustomer, 4, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(626, 154);
            this.tableLayoutPanel1.TabIndex = 49;
            // 
            // LabWO
            // 
            this.LabWO.AutoSize = true;
            this.LabWO.BackColor = System.Drawing.Color.Transparent;
            this.LabWO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabWO.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabWO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabWO.Location = new System.Drawing.Point(8, 0);
            this.LabWO.Name = "LabWO";
            this.LabWO.Size = new System.Drawing.Size(91, 30);
            this.LabWO.TabIndex = 45;
            this.LabWO.Text = "Work Order";
            this.LabWO.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabPartNo
            // 
            this.LabPartNo.AutoSize = true;
            this.LabPartNo.BackColor = System.Drawing.Color.Transparent;
            this.LabPartNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabPartNo.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabPartNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabPartNo.Location = new System.Drawing.Point(8, 30);
            this.LabPartNo.Name = "LabPartNo";
            this.LabPartNo.Size = new System.Drawing.Size(91, 30);
            this.LabPartNo.TabIndex = 0;
            this.LabPartNo.Text = "Spec1";
            this.LabPartNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabCustomer
            // 
            this.LabCustomer.AutoSize = true;
            this.LabCustomer.BackColor = System.Drawing.Color.Transparent;
            this.LabCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabCustomer.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabCustomer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabCustomer.Location = new System.Drawing.Point(311, 120);
            this.LabCustomer.Name = "LabCustomer";
            this.LabCustomer.Size = new System.Drawing.Size(99, 34);
            this.LabCustomer.TabIndex = 24;
            this.LabCustomer.Text = "Customer";
            this.LabCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabRoute
            // 
            this.LabRoute.AutoSize = true;
            this.LabRoute.BackColor = System.Drawing.Color.Transparent;
            this.LabRoute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabRoute.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabRoute.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabRoute.Location = new System.Drawing.Point(311, 90);
            this.LabRoute.Name = "LabRoute";
            this.LabRoute.Size = new System.Drawing.Size(99, 30);
            this.LabRoute.TabIndex = 22;
            this.LabRoute.Text = "Route Name";
            this.LabRoute.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabPDLine
            // 
            this.LabPDLine.AutoSize = true;
            this.LabPDLine.BackColor = System.Drawing.Color.Transparent;
            this.LabPDLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabPDLine.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabPDLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabPDLine.Location = new System.Drawing.Point(311, 60);
            this.LabPDLine.Name = "LabPDLine";
            this.LabPDLine.Size = new System.Drawing.Size(99, 30);
            this.LabPDLine.TabIndex = 20;
            this.LabPDLine.Text = "Production Line";
            this.LabPDLine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabVersion
            // 
            this.LabVersion.AutoSize = true;
            this.LabVersion.BackColor = System.Drawing.Color.Transparent;
            this.LabVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabVersion.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabVersion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabVersion.Location = new System.Drawing.Point(8, 60);
            this.LabVersion.Name = "LabVersion";
            this.LabVersion.Size = new System.Drawing.Size(91, 30);
            this.LabVersion.TabIndex = 1;
            this.LabVersion.Text = "Version";
            this.LabVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabWOType
            // 
            this.LabWOType.AutoSize = true;
            this.LabWOType.BackColor = System.Drawing.Color.Transparent;
            this.LabWOType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabWOType.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabWOType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabWOType.Location = new System.Drawing.Point(311, 30);
            this.LabWOType.Name = "LabWOType";
            this.LabWOType.Size = new System.Drawing.Size(99, 30);
            this.LabWOType.TabIndex = 18;
            this.LabWOType.Text = "W/O Type";
            this.LabWOType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabDueDate
            // 
            this.LabDueDate.AutoSize = true;
            this.LabDueDate.BackColor = System.Drawing.Color.Transparent;
            this.LabDueDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabDueDate.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabDueDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabDueDate.Location = new System.Drawing.Point(8, 90);
            this.LabDueDate.Name = "LabDueDate";
            this.LabDueDate.Size = new System.Drawing.Size(91, 30);
            this.LabDueDate.TabIndex = 4;
            this.LabDueDate.Text = "W/O Due Date";
            this.LabDueDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabAvailableQty
            // 
            this.LabAvailableQty.AutoSize = true;
            this.LabAvailableQty.BackColor = System.Drawing.Color.Transparent;
            this.LabAvailableQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabAvailableQty.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabAvailableQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabAvailableQty.Location = new System.Drawing.Point(8, 120);
            this.LabAvailableQty.Name = "LabAvailableQty";
            this.LabAvailableQty.Size = new System.Drawing.Size(91, 34);
            this.LabAvailableQty.TabIndex = 6;
            this.LabAvailableQty.Text = "Available Qty";
            this.LabAvailableQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // editWO
            // 
            this.editWO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.editWO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editWO.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.editWO.Location = new System.Drawing.Point(105, 3);
            this.editWO.Name = "editWO";
            this.editWO.Size = new System.Drawing.Size(150, 25);
            this.editWO.TabIndex = 1;
            this.editWO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editWO_KeyPress);
            // 
            // editPart
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.editPart, 2);
            this.editPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editPart.Location = new System.Drawing.Point(105, 33);
            this.editPart.Name = "editPart";
            this.editPart.ReadOnly = true;
            this.editPart.Size = new System.Drawing.Size(200, 25);
            this.editPart.TabIndex = 47;
            // 
            // editVersion
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.editVersion, 2);
            this.editVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editVersion.Location = new System.Drawing.Point(105, 63);
            this.editVersion.Name = "editVersion";
            this.editVersion.ReadOnly = true;
            this.editVersion.Size = new System.Drawing.Size(200, 25);
            this.editVersion.TabIndex = 48;
            // 
            // editDue
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.editDue, 2);
            this.editDue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editDue.Location = new System.Drawing.Point(105, 93);
            this.editDue.Name = "editDue";
            this.editDue.ReadOnly = true;
            this.editDue.Size = new System.Drawing.Size(200, 25);
            this.editDue.TabIndex = 49;
            // 
            // editAvailableQty
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.editAvailableQty, 2);
            this.editAvailableQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editAvailableQty.Location = new System.Drawing.Point(105, 123);
            this.editAvailableQty.Name = "editAvailableQty";
            this.editAvailableQty.ReadOnly = true;
            this.editAvailableQty.Size = new System.Drawing.Size(200, 25);
            this.editAvailableQty.TabIndex = 50;
            // 
            // btnSearchWO
            // 
            this.btnSearchWO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearchWO.Location = new System.Drawing.Point(261, 3);
            this.btnSearchWO.Name = "btnSearchWO";
            this.btnSearchWO.Size = new System.Drawing.Size(44, 24);
            this.btnSearchWO.TabIndex = 53;
            this.btnSearchWO.Text = "...";
            this.btnSearchWO.UseVisualStyleBackColor = true;
            this.btnSearchWO.Click += new System.EventHandler(this.btnSearchWO_Click);
            // 
            // editType
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.editType, 2);
            this.editType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editType.Location = new System.Drawing.Point(416, 33);
            this.editType.Name = "editType";
            this.editType.ReadOnly = true;
            this.editType.Size = new System.Drawing.Size(202, 25);
            this.editType.TabIndex = 56;
            // 
            // editPDLine
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.editPDLine, 2);
            this.editPDLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editPDLine.Location = new System.Drawing.Point(416, 63);
            this.editPDLine.Name = "editPDLine";
            this.editPDLine.ReadOnly = true;
            this.editPDLine.Size = new System.Drawing.Size(202, 25);
            this.editPDLine.TabIndex = 57;
            // 
            // editRoute
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.editRoute, 2);
            this.editRoute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editRoute.Location = new System.Drawing.Point(416, 93);
            this.editRoute.Name = "editRoute";
            this.editRoute.ReadOnly = true;
            this.editRoute.Size = new System.Drawing.Size(202, 25);
            this.editRoute.TabIndex = 58;
            // 
            // editCustomer
            // 
            this.editCustomer.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.editCustomer, 2);
            this.editCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editCustomer.Location = new System.Drawing.Point(416, 123);
            this.editCustomer.Name = "editCustomer";
            this.editCustomer.ReadOnly = true;
            this.editCustomer.Size = new System.Drawing.Size(202, 25);
            this.editCustomer.TabIndex = 59;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panel1.Size = new System.Drawing.Size(632, 248);
            this.panel1.TabIndex = 66;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.8F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.2F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 243F));
            this.tableLayoutPanel2.Controls.Add(this.LabRCNo, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSelectQty, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.LabInputQty, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.combPriority, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.LabPriority, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.editRCNo, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSearchRC, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.editInputQty, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 181);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(632, 67);
            this.tableLayoutPanel2.TabIndex = 65;
            // 
            // LabRCNo
            // 
            this.LabRCNo.AutoSize = true;
            this.LabRCNo.BackColor = System.Drawing.Color.Transparent;
            this.LabRCNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabRCNo.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabRCNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabRCNo.Location = new System.Drawing.Point(11, 0);
            this.LabRCNo.Name = "LabRCNo";
            this.LabRCNo.Size = new System.Drawing.Size(80, 33);
            this.LabRCNo.TabIndex = 56;
            this.LabRCNo.Text = "Run Card No";
            this.LabRCNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSelectQty
            // 
            this.btnSelectQty.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectQty.Location = new System.Drawing.Point(234, 36);
            this.btnSelectQty.Name = "btnSelectQty";
            this.btnSelectQty.Size = new System.Drawing.Size(62, 28);
            this.btnSelectQty.TabIndex = 1;
            this.btnSelectQty.Text = "Select";
            this.btnSelectQty.UseVisualStyleBackColor = true;
            this.btnSelectQty.Click += new System.EventHandler(this.btnSelectQty_Click);
            // 
            // LabInputQty
            // 
            this.LabInputQty.AutoSize = true;
            this.LabInputQty.BackColor = System.Drawing.Color.Transparent;
            this.LabInputQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabInputQty.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabInputQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabInputQty.Location = new System.Drawing.Point(11, 33);
            this.LabInputQty.Name = "LabInputQty";
            this.LabInputQty.Size = new System.Drawing.Size(80, 34);
            this.LabInputQty.TabIndex = 57;
            this.LabInputQty.Text = "Input Qty";
            this.LabInputQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // combPriority
            // 
            this.combPriority.Dock = System.Windows.Forms.DockStyle.Fill;
            this.combPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combPriority.FormattingEnabled = true;
            this.combPriority.Location = new System.Drawing.Point(383, 36);
            this.combPriority.Name = "combPriority";
            this.combPriority.Size = new System.Drawing.Size(238, 20);
            this.combPriority.TabIndex = 62;
            // 
            // LabPriority
            // 
            this.LabPriority.AutoSize = true;
            this.LabPriority.BackColor = System.Drawing.Color.Transparent;
            this.LabPriority.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabPriority.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabPriority.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabPriority.Location = new System.Drawing.Point(302, 33);
            this.LabPriority.Name = "LabPriority";
            this.LabPriority.Size = new System.Drawing.Size(75, 34);
            this.LabPriority.TabIndex = 58;
            this.LabPriority.Text = "Priority";
            this.LabPriority.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // editRCNo
            // 
            this.editRCNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editRCNo.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editRCNo.Location = new System.Drawing.Point(97, 3);
            this.editRCNo.Name = "editRCNo";
            this.editRCNo.Size = new System.Drawing.Size(131, 23);
            this.editRCNo.TabIndex = 59;
            // 
            // btnSearchRC
            // 
            this.btnSearchRC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearchRC.Location = new System.Drawing.Point(234, 3);
            this.btnSearchRC.Name = "btnSearchRC";
            this.btnSearchRC.Size = new System.Drawing.Size(62, 27);
            this.btnSearchRC.TabIndex = 61;
            this.btnSearchRC.Text = "...";
            this.btnSearchRC.UseVisualStyleBackColor = true;
            this.btnSearchRC.Click += new System.EventHandler(this.btnSearchRC_Click);
            // 
            // editInputQty
            // 
            this.editInputQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editInputQty.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editInputQty.Location = new System.Drawing.Point(97, 36);
            this.editInputQty.Name = "editInputQty";
            this.editInputQty.Size = new System.Drawing.Size(131, 23);
            this.editInputQty.TabIndex = 60;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSelectNone);
            this.panel3.Controls.Add(this.btnSelectAll);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(5, 248);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(91, 184);
            this.panel3.TabIndex = 68;
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectNone.Location = new System.Drawing.Point(0, 33);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(89, 27);
            this.btnSelectNone.TabIndex = 2;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectAll.Location = new System.Drawing.Point(0, 0);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(89, 27);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(96, 248);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(541, 184);
            this.tabControl1.TabIndex = 69;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gvComponent);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(533, 154);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Pick Component";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gvComponent
            // 
            this.gvComponent.AllowUserToAddRows = false;
            this.gvComponent.AllowUserToDeleteRows = false;
            this.gvComponent.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gvComponent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvComponent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvComponent.Location = new System.Drawing.Point(3, 3);
            this.gvComponent.Name = "gvComponent";
            this.gvComponent.RowTemplate.Height = 24;
            this.gvComponent.Size = new System.Drawing.Size(527, 148);
            this.gvComponent.TabIndex = 0;
            // 
            // fData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 476);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "fData";
            this.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Append";
            this.Load += new System.EventHandler(this.fData_Load);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvComponent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label LabWO;
        private System.Windows.Forms.Label LabPartNo;
        private System.Windows.Forms.Label LabCustomer;
        private System.Windows.Forms.Label LabRoute;
        private System.Windows.Forms.Label LabPDLine;
        private System.Windows.Forms.Label LabVersion;
        private System.Windows.Forms.Label LabWOType;
        private System.Windows.Forms.Label LabDueDate;
        private System.Windows.Forms.Label LabAvailableQty;
        private System.Windows.Forms.TextBox editWO;
        private System.Windows.Forms.TextBox editPart;
        private System.Windows.Forms.TextBox editVersion;
        private System.Windows.Forms.TextBox editDue;
        private System.Windows.Forms.TextBox editAvailableQty;
        private System.Windows.Forms.Button btnSearchWO;
        private System.Windows.Forms.TextBox editType;
        private System.Windows.Forms.TextBox editPDLine;
        private System.Windows.Forms.TextBox editRoute;
        private System.Windows.Forms.TextBox editCustomer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label LabRCNo;
        private System.Windows.Forms.Label LabInputQty;
        private System.Windows.Forms.ComboBox combPriority;
        private System.Windows.Forms.Label LabPriority;
        private System.Windows.Forms.TextBox editRCNo;
        private System.Windows.Forms.Button btnSearchRC;
        private System.Windows.Forms.TextBox editInputQty;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnSelectQty;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView gvComponent;

    }
}