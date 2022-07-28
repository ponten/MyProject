using System.Drawing.Printing;
using System.Windows.Forms;

namespace CWoManager
{
    public partial class fData
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.columnPKSpec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labClose = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labClientOrder = new System.Windows.Forms.Label();
            this.txtUni = new System.Windows.Forms.TextBox();
            this.labDate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comProduceNo = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listViewOrderDetail = new System.Windows.Forms.ListView();
            this.colOrderNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCustomerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDueDateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOrderCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colProduceCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAccumulation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dgvRoute = new System.Windows.Forms.DataGridView();
            this.labProd10B = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LVPkSPec = new System.Windows.Forms.ListView();
            this.PKSPEC_NAME = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BOX_QTY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CARTON_QTY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PALLET_QTY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuAppend = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.listView2 = new System.Windows.Forms.ListView();
            this.editTargetQty = new System.Windows.Forms.TextBox();
            this.lblFactory = new System.Windows.Forms.Label();
            this.editRemark = new System.Windows.Forms.TextBox();
            this.combLine = new System.Windows.Forms.ComboBox();
            this.LabRemark = new System.Windows.Forms.Label();
            this.LabWO = new System.Windows.Forms.Label();
            this.tbWO = new System.Windows.Forms.TextBox();
            this.combRoute = new System.Windows.Forms.ComboBox();
            this.LabRoute = new System.Windows.Forms.Label();
            this.LabStatus = new System.Windows.Forms.Label();
            this.LabLine = new System.Windows.Forms.Label();
            this.LabWoStatus = new System.Windows.Forms.Label();
            this.LabPart = new System.Windows.Forms.Label();
            this.editPart = new System.Windows.Forms.TextBox();
            this.LabVersion = new System.Windows.Forms.Label();
            this.LabWoRule = new System.Windows.Forms.Label();
            this.combWoRule = new System.Windows.Forms.ComboBox();
            this.LabWoType = new System.Windows.Forms.Label();
            this.combWoType = new System.Windows.Forms.ComboBox();
            this.LabTargetQty = new System.Windows.Forms.Label();
            this.dtScheduleDate = new System.Windows.Forms.DateTimePicker();
            this.dtDueDate = new System.Windows.Forms.DateTimePicker();
            this.LabDueDate = new System.Windows.Forms.Label();
            this.labSales = new System.Windows.Forms.Label();
            this.txtClient2 = new System.Windows.Forms.TextBox();
            this.labOutSource = new System.Windows.Forms.Label();
            this.txtSales = new System.Windows.Forms.TextBox();
            this.labBlueprint = new System.Windows.Forms.Label();
            this.txtOutsource = new System.Windows.Forms.TextBox();
            this.labPallets = new System.Windows.Forms.Label();
            this.txtPallets = new System.Windows.Forms.TextBox();
            this.labOldNumber = new System.Windows.Forms.Label();
            this.labUnit = new System.Windows.Forms.Label();
            this.txtOldNum = new System.Windows.Forms.TextBox();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.labFinished = new System.Windows.Forms.Label();
            this.txtFinish = new System.Windows.Forms.TextBox();
            this.labBad = new System.Windows.Forms.Label();
            this.txtBad = new System.Windows.Forms.TextBox();
            this.labNotPaid = new System.Windows.Forms.Label();
            this.txtNotPaid = new System.Windows.Forms.TextBox();
            this.txtRule1 = new System.Windows.Forms.TextBox();
            this.combMade = new System.Windows.Forms.ComboBox();
            this.labMadeCategory = new System.Windows.Forms.Label();
            this.cbNumber2 = new System.Windows.Forms.ComboBox();
            this.txtClient = new System.Windows.Forms.TextBox();
            this.labProductRate = new System.Windows.Forms.Label();
            this.txtProductRate = new System.Windows.Forms.TextBox();
            this.labNumber1 = new System.Windows.Forms.Label();
            this.txtCustomize = new System.Windows.Forms.TextBox();
            this.txtBlueprint = new System.Windows.Forms.TextBox();
            this.labCustomize = new System.Windows.Forms.Label();
            this.labClient = new System.Windows.Forms.Label();
            this.combOperation = new System.Windows.Forms.ComboBox();
            this.labOperation = new System.Windows.Forms.Label();
            this.labRule = new System.Windows.Forms.Label();
            this.txtNum = new System.Windows.Forms.TextBox();
            this.lblProd10B = new System.Windows.Forms.Label();
            this.cbProd10B = new System.Windows.Forms.ComboBox();
            this.cbNumber1 = new System.Windows.Forms.ComboBox();
            this.labCustomerDate = new System.Windows.Forms.Label();
            this.labCustomerDueDate = new System.Windows.Forms.Label();
            this.dtCustDate = new System.Windows.Forms.DateTimePicker();
            this.dtCustDueDate = new System.Windows.Forms.DateTimePicker();
            this.cbProd10A = new System.Windows.Forms.ComboBox();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.txtSequence = new System.Windows.Forms.TextBox();
            this.gbBOMRelation = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblProd10C = new System.Windows.Forms.Label();
            this.lblProd10A = new System.Windows.Forms.Label();
            this.cbProd10C = new System.Windows.Forms.ComboBox();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.cbOrderSequence = new System.Windows.Forms.ComboBox();
            this.lblOrderSequence = new System.Windows.Forms.Label();
            this.lblSequence = new System.Windows.Forms.Label();
            this.lblNumber2 = new System.Windows.Forms.Label();
            this.lblNTFC = new System.Windows.Forms.Label();
            this.LabScheduleDate = new System.Windows.Forms.Label();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.TbBookNo = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).BeginInit();
            this.labProd10B.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.gbBOMRelation.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.btnCancel.Location = new System.Drawing.Point(1101, 631);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnOK.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.btnOK.Location = new System.Drawing.Point(1001, 631);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.TabStop = false;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // columnPKSpec
            // 
            this.columnPKSpec.Text = "Packing Spec";
            this.columnPKSpec.Width = 125;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(56, 15);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(114, 20);
            this.comboBox1.TabIndex = 229;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(345, 16);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(74, 20);
            this.comboBox2.TabIndex = 233;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(226, 15);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(116, 22);
            this.dateTimePicker1.TabIndex = 232;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.textBox1.Location = new System.Drawing.Point(433, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(61, 22);
            this.textBox1.TabIndex = 253;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(7, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 12);
            this.label5.TabIndex = 228;
            this.label5.Text = "Client";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(7, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 12);
            this.label2.TabIndex = 230;
            this.label2.Text = "Operation";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(7, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 238;
            this.label9.Text = "客訂";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(207, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 231;
            this.label3.Text = "號碼";
            // 
            // labClose
            // 
            this.labClose.AutoSize = true;
            this.labClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labClose.Location = new System.Drawing.Point(235, 105);
            this.labClose.Name = "labClose";
            this.labClose.Size = new System.Drawing.Size(37, 12);
            this.labClose.TabIndex = 244;
            this.labClose.Text = "Closed";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(404, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 12);
            this.label7.TabIndex = 197;
            this.label7.Text = "Sales";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(404, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 12);
            this.label8.TabIndex = 199;
            this.label8.Text = "Out Source";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(404, 105);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(28, 12);
            this.label13.TabIndex = 246;
            this.label13.Text = "Void";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(593, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 195;
            this.label4.Text = "Made Category";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label23.Location = new System.Drawing.Point(315, 260);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(29, 12);
            this.label23.TabIndex = 218;
            this.label23.Text = "結案";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(7, 144);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(27, 12);
            this.label15.TabIndex = 251;
            this.label15.Text = "Rule";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(8, 231);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(63, 12);
            this.label17.TabIndex = 211;
            this.label17.Text = "Old Number";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label21.Location = new System.Drawing.Point(8, 263);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(70, 12);
            this.label21.TabIndex = 214;
            this.label21.Text = "Total Number";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label26.Location = new System.Drawing.Point(8, 292);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(24, 12);
            this.label26.TabIndex = 224;
            this.label26.Text = "Bad";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(197, 228);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(25, 12);
            this.label18.TabIndex = 212;
            this.label18.Text = "Unit";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label22.Location = new System.Drawing.Point(197, 260);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(44, 12);
            this.label22.TabIndex = 216;
            this.label22.Text = "Finished";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label27.Location = new System.Drawing.Point(197, 295);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(45, 12);
            this.label27.TabIndex = 226;
            this.label27.Text = "Not Paid";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(337, 231);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 12);
            this.label14.TabIndex = 201;
            this.label14.Text = "Blueprint";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(533, 231);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(34, 12);
            this.label16.TabIndex = 203;
            this.label16.Text = "Pallets";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label24.Location = new System.Drawing.Point(513, 257);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(54, 12);
            this.label24.TabIndex = 220;
            this.label24.Text = "Difference";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label25.Location = new System.Drawing.Point(654, 260);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(51, 12);
            this.label25.TabIndex = 222;
            this.label25.Text = "Inventory";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label20.Location = new System.Drawing.Point(581, 144);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(77, 12);
            this.label20.TabIndex = 206;
            this.label20.Text = "Real Date Time";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label19.Location = new System.Drawing.Point(580, 174);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(99, 12);
            this.label19.TabIndex = 205;
            this.label19.Text = "Real Due Date Time";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(9, 237);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 12);
            this.label12.TabIndex = 254;
            this.label12.Text = "Produce No";
            // 
            // labClientOrder
            // 
            this.labClientOrder.AutoSize = true;
            this.labClientOrder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labClientOrder.Location = new System.Drawing.Point(7, 77);
            this.labClientOrder.Name = "labClientOrder";
            this.labClientOrder.Size = new System.Drawing.Size(54, 12);
            this.labClientOrder.TabIndex = 238;
            this.labClientOrder.Text = "Customize";
            // 
            // txtUni
            // 
            this.txtUni.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtUni.Location = new System.Drawing.Point(245, 269);
            this.txtUni.Name = "txtUni";
            this.txtUni.Size = new System.Drawing.Size(100, 22);
            this.txtUni.TabIndex = 215;
            // 
            // labDate
            // 
            this.labDate.AutoSize = true;
            this.labDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labDate.Location = new System.Drawing.Point(576, 141);
            this.labDate.Name = "labDate";
            this.labDate.Size = new System.Drawing.Size(74, 12);
            this.labDate.TabIndex = 258;
            this.labDate.Text = "Customer Date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(577, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 12);
            this.label6.TabIndex = 259;
            this.label6.Text = "Due Date";
            // 
            // comProduceNo
            // 
            this.comProduceNo.Location = new System.Drawing.Point(0, 0);
            this.comProduceNo.Name = "comProduceNo";
            this.comProduceNo.Size = new System.Drawing.Size(121, 20);
            this.comProduceNo.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 9);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.labProd10B);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 370);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1178, 253);
            this.tabControl1.TabIndex = 51;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listViewOrderDetail);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1170, 223);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Order Detail";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listViewOrderDetail
            // 
            this.listViewOrderDetail.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colOrderNumber,
            this.colCustomerName,
            this.colDueDateTime,
            this.colOrderCount,
            this.colProduceCount,
            this.colAccumulation});
            this.listViewOrderDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewOrderDetail.FullRowSelect = true;
            this.listViewOrderDetail.GridLines = true;
            this.listViewOrderDetail.HideSelection = false;
            this.listViewOrderDetail.Location = new System.Drawing.Point(3, 3);
            this.listViewOrderDetail.Name = "listViewOrderDetail";
            this.listViewOrderDetail.Size = new System.Drawing.Size(1164, 217);
            this.listViewOrderDetail.TabIndex = 0;
            this.listViewOrderDetail.UseCompatibleStateImageBehavior = false;
            this.listViewOrderDetail.View = System.Windows.Forms.View.Details;
            // 
            // colOrderNumber
            // 
            this.colOrderNumber.Text = "Order Number";
            this.colOrderNumber.Width = 320;
            // 
            // colCustomerName
            // 
            this.colCustomerName.Text = "Customer Name";
            this.colCustomerName.Width = 136;
            // 
            // colDueDateTime
            // 
            this.colDueDateTime.Text = "Due Date Time";
            this.colDueDateTime.Width = 145;
            // 
            // colOrderCount
            // 
            this.colOrderCount.Text = "Order Count";
            this.colOrderCount.Width = 138;
            // 
            // colProduceCount
            // 
            this.colProduceCount.Text = "Produce Count";
            this.colProduceCount.Width = 143;
            // 
            // colAccumulation
            // 
            this.colAccumulation.Text = "Accumulation";
            this.colAccumulation.Width = 151;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dgvRoute);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1170, 223);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Route Detail";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgvRoute
            // 
            this.dgvRoute.AllowUserToAddRows = false;
            this.dgvRoute.AllowUserToDeleteRows = false;
            this.dgvRoute.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRoute.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvRoute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRoute.Location = new System.Drawing.Point(0, 0);
            this.dgvRoute.Name = "dgvRoute";
            this.dgvRoute.ReadOnly = true;
            this.dgvRoute.RowHeadersWidth = 51;
            this.dgvRoute.RowTemplate.Height = 24;
            this.dgvRoute.Size = new System.Drawing.Size(1170, 223);
            this.dgvRoute.TabIndex = 0;
            // 
            // labProd10B
            // 
            this.labProd10B.Controls.Add(this.groupBox1);
            this.labProd10B.Location = new System.Drawing.Point(4, 26);
            this.labProd10B.Name = "labProd10B";
            this.labProd10B.Padding = new System.Windows.Forms.Padding(3);
            this.labProd10B.Size = new System.Drawing.Size(1170, 223);
            this.labProd10B.TabIndex = 0;
            this.labProd10B.Text = "Packing Specification";
            this.labProd10B.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.LVPkSPec);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 12F);
            this.groupBox1.Location = new System.Drawing.Point(4, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1266, 150);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Packing Specification";
            // 
            // LVPkSPec
            // 
            this.LVPkSPec.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PKSPEC_NAME,
            this.BOX_QTY,
            this.CARTON_QTY,
            this.PALLET_QTY});
            this.LVPkSPec.ContextMenuStrip = this.contextMenuStrip1;
            this.LVPkSPec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LVPkSPec.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LVPkSPec.FullRowSelect = true;
            this.LVPkSPec.GridLines = true;
            this.LVPkSPec.HideSelection = false;
            this.LVPkSPec.Location = new System.Drawing.Point(28, 23);
            this.LVPkSPec.Name = "LVPkSPec";
            this.LVPkSPec.Size = new System.Drawing.Size(1235, 124);
            this.LVPkSPec.TabIndex = 0;
            this.LVPkSPec.TabStop = false;
            this.LVPkSPec.UseCompatibleStateImageBehavior = false;
            this.LVPkSPec.View = System.Windows.Forms.View.Details;
            // 
            // PKSPEC_NAME
            // 
            this.PKSPEC_NAME.Text = "Packing Spec";
            this.PKSPEC_NAME.Width = 125;
            // 
            // BOX_QTY
            // 
            this.BOX_QTY.Text = "Box";
            this.BOX_QTY.Width = 90;
            // 
            // CARTON_QTY
            // 
            this.CARTON_QTY.Text = "Carton";
            this.CARTON_QTY.Width = 90;
            // 
            // PALLET_QTY
            // 
            this.PALLET_QTY.Text = "Pallet";
            this.PALLET_QTY.Width = 90;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuAppend,
            this.MenuRemove});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(122, 48);
            // 
            // MenuAppend
            // 
            this.MenuAppend.Name = "MenuAppend";
            this.MenuAppend.Size = new System.Drawing.Size(121, 22);
            this.MenuAppend.Text = "Append";
            this.MenuAppend.Click += new System.EventHandler(this.MenuAppend_Click);
            // 
            // MenuRemove
            // 
            this.MenuRemove.Name = "MenuRemove";
            this.MenuRemove.Size = new System.Drawing.Size(121, 22);
            this.MenuRemove.Text = "Remove";
            this.MenuRemove.Click += new System.EventHandler(this.MenuRemove_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(3, 23);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(25, 124);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(22, 24);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.MenuAppend_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(22, 24);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.MenuRemove_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(22, 24);
            this.toolStripButton3.Text = "toolStripButton3";
            // 
            // listView1
            // 
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(4, 4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1246, 204);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // listView2
            // 
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(1, 4);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(1247, 207);
            this.listView2.TabIndex = 0;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // editTargetQty
            // 
            this.editTargetQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.editTargetQty.BackColor = System.Drawing.Color.Yellow;
            this.tableLayoutPanel1.SetColumnSpan(this.editTargetQty, 2);
            this.editTargetQty.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.editTargetQty.Location = new System.Drawing.Point(1001, 213);
            this.editTargetQty.Name = "editTargetQty";
            this.editTargetQty.Size = new System.Drawing.Size(180, 25);
            this.editTargetQty.TabIndex = 48;
            this.editTargetQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editTargetQty_KeyPress);
            // 
            // lblFactory
            // 
            this.lblFactory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFactory.AutoSize = true;
            this.lblFactory.BackColor = System.Drawing.Color.Transparent;
            this.lblFactory.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.lblFactory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblFactory.Location = new System.Drawing.Point(891, 7);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(50, 15);
            this.lblFactory.TabIndex = 45;
            this.lblFactory.Text = "Factory";
            this.lblFactory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // editRemark
            // 
            this.editRemark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.editRemark, 2);
            this.editRemark.Location = new System.Drawing.Point(1001, 303);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(180, 24);
            this.editRemark.TabIndex = 52;
            // 
            // combLine
            // 
            this.combLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.combLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tableLayoutPanel1.SetColumnSpan(this.combLine, 2);
            this.combLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combLine.Location = new System.Drawing.Point(1001, 243);
            this.combLine.Name = "combLine";
            this.combLine.Size = new System.Drawing.Size(180, 24);
            this.combLine.TabIndex = 52;
            // 
            // LabRemark
            // 
            this.LabRemark.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabRemark.AutoSize = true;
            this.LabRemark.BackColor = System.Drawing.Color.Transparent;
            this.LabRemark.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabRemark.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabRemark.Location = new System.Drawing.Point(891, 307);
            this.LabRemark.Name = "LabRemark";
            this.LabRemark.Size = new System.Drawing.Size(51, 15);
            this.LabRemark.TabIndex = 35;
            this.LabRemark.Text = "Remark";
            this.LabRemark.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabWO
            // 
            this.LabWO.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabWO.AutoSize = true;
            this.LabWO.BackColor = System.Drawing.Color.Transparent;
            this.LabWO.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabWO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabWO.Location = new System.Drawing.Point(891, 37);
            this.LabWO.Name = "LabWO";
            this.LabWO.Size = new System.Drawing.Size(94, 15);
            this.LabWO.TabIndex = 0;
            this.LabWO.Text = "W/O (Preview)";
            this.LabWO.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbWO
            // 
            this.tbWO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWO.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.tbWO, 2);
            this.tbWO.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.tbWO.Location = new System.Drawing.Point(1001, 33);
            this.tbWO.Name = "tbWO";
            this.tbWO.ReadOnly = true;
            this.tbWO.Size = new System.Drawing.Size(180, 25);
            this.tbWO.TabIndex = 0;
            // 
            // combRoute
            // 
            this.combRoute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.combRoute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tableLayoutPanel1.SetColumnSpan(this.combRoute, 2);
            this.combRoute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combRoute.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.combRoute.FormattingEnabled = true;
            this.combRoute.Location = new System.Drawing.Point(1001, 273);
            this.combRoute.Name = "combRoute";
            this.combRoute.Size = new System.Drawing.Size(180, 23);
            this.combRoute.TabIndex = 7;
            this.combRoute.SelectedIndexChanged += new System.EventHandler(this.combRoute_SelectedIndexChanged);
            // 
            // LabRoute
            // 
            this.LabRoute.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabRoute.AutoSize = true;
            this.LabRoute.BackColor = System.Drawing.Color.Transparent;
            this.LabRoute.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabRoute.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabRoute.Location = new System.Drawing.Point(891, 277);
            this.LabRoute.Name = "LabRoute";
            this.LabRoute.Size = new System.Drawing.Size(77, 15);
            this.LabRoute.TabIndex = 20;
            this.LabRoute.Text = "Route Name";
            this.LabRoute.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabStatus
            // 
            this.LabStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabStatus.AutoSize = true;
            this.LabStatus.BackColor = System.Drawing.Color.Transparent;
            this.LabStatus.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabStatus.Location = new System.Drawing.Point(891, 67);
            this.LabStatus.Name = "LabStatus";
            this.LabStatus.Size = new System.Drawing.Size(72, 15);
            this.LabStatus.TabIndex = 1;
            this.LabStatus.Text = "W/O Status";
            this.LabStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabLine
            // 
            this.LabLine.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabLine.AutoSize = true;
            this.LabLine.BackColor = System.Drawing.Color.Transparent;
            this.LabLine.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabLine.Location = new System.Drawing.Point(891, 247);
            this.LabLine.Name = "LabLine";
            this.LabLine.Size = new System.Drawing.Size(88, 15);
            this.LabLine.TabIndex = 18;
            this.LabLine.Text = "PDLine Name";
            this.LabLine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabWoStatus
            // 
            this.LabWoStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabWoStatus.AutoSize = true;
            this.LabWoStatus.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.LabWoStatus, 2);
            this.LabWoStatus.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Bold);
            this.LabWoStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabWoStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabWoStatus.Location = new System.Drawing.Point(1001, 67);
            this.LabWoStatus.Name = "LabWoStatus";
            this.LabWoStatus.Size = new System.Drawing.Size(34, 15);
            this.LabWoStatus.TabIndex = 43;
            this.LabWoStatus.Text = "N/A";
            this.LabWoStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabPart
            // 
            this.LabPart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabPart.AutoSize = true;
            this.LabPart.BackColor = System.Drawing.Color.Transparent;
            this.LabPart.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabPart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabPart.Location = new System.Drawing.Point(891, 97);
            this.LabPart.Name = "LabPart";
            this.LabPart.Size = new System.Drawing.Size(51, 15);
            this.LabPart.TabIndex = 4;
            this.LabPart.Text = "Part No";
            this.LabPart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // editPart
            // 
            this.editPart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.editPart.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.editPart, 2);
            this.editPart.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.editPart.Location = new System.Drawing.Point(1001, 93);
            this.editPart.Name = "editPart";
            this.editPart.Size = new System.Drawing.Size(180, 25);
            this.editPart.TabIndex = 1;
            this.editPart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editPart_KeyPress);
            // 
            // LabVersion
            // 
            this.LabVersion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabVersion.AutoSize = true;
            this.LabVersion.BackColor = System.Drawing.Color.Transparent;
            this.LabVersion.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabVersion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabVersion.Location = new System.Drawing.Point(891, 127);
            this.LabVersion.Name = "LabVersion";
            this.LabVersion.Size = new System.Drawing.Size(51, 15);
            this.LabVersion.TabIndex = 6;
            this.LabVersion.Text = "Version";
            this.LabVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabWoRule
            // 
            this.LabWoRule.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabWoRule.AutoSize = true;
            this.LabWoRule.BackColor = System.Drawing.Color.Transparent;
            this.LabWoRule.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabWoRule.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabWoRule.Location = new System.Drawing.Point(891, 157);
            this.LabWoRule.Name = "LabWoRule";
            this.LabWoRule.Size = new System.Drawing.Size(64, 15);
            this.LabWoRule.TabIndex = 8;
            this.LabWoRule.Text = "W/O Rule";
            this.LabWoRule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // combWoRule
            // 
            this.combWoRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.combWoRule.BackColor = System.Drawing.Color.Yellow;
            this.tableLayoutPanel1.SetColumnSpan(this.combWoRule, 2);
            this.combWoRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combWoRule.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.combWoRule.FormattingEnabled = true;
            this.combWoRule.Location = new System.Drawing.Point(1001, 153);
            this.combWoRule.Name = "combWoRule";
            this.combWoRule.Size = new System.Drawing.Size(180, 23);
            this.combWoRule.TabIndex = 3;
            this.combWoRule.SelectedIndexChanged += new System.EventHandler(this.combWoRule_SelectedIndexChanged);
            // 
            // LabWoType
            // 
            this.LabWoType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabWoType.AutoSize = true;
            this.LabWoType.BackColor = System.Drawing.Color.Transparent;
            this.LabWoType.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabWoType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabWoType.Location = new System.Drawing.Point(891, 187);
            this.LabWoType.Name = "LabWoType";
            this.LabWoType.Size = new System.Drawing.Size(67, 15);
            this.LabWoType.TabIndex = 10;
            this.LabWoType.Text = "W/O Type";
            this.LabWoType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // combWoType
            // 
            this.combWoType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.combWoType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tableLayoutPanel1.SetColumnSpan(this.combWoType, 2);
            this.combWoType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combWoType.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.combWoType.FormattingEnabled = true;
            this.combWoType.Location = new System.Drawing.Point(1001, 183);
            this.combWoType.Name = "combWoType";
            this.combWoType.Size = new System.Drawing.Size(180, 23);
            this.combWoType.TabIndex = 4;
            // 
            // LabTargetQty
            // 
            this.LabTargetQty.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabTargetQty.AutoSize = true;
            this.LabTargetQty.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabTargetQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabTargetQty.Location = new System.Drawing.Point(891, 217);
            this.LabTargetQty.Name = "LabTargetQty";
            this.LabTargetQty.Size = new System.Drawing.Size(69, 15);
            this.LabTargetQty.TabIndex = 47;
            this.LabTargetQty.Text = "Target Qty";
            this.LabTargetQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtScheduleDate
            // 
            this.dtScheduleDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtScheduleDate.CustomFormat = "yyyy/MM/dd";
            this.dtScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtScheduleDate.Location = new System.Drawing.Point(113, 153);
            this.dtScheduleDate.Name = "dtScheduleDate";
            this.dtScheduleDate.Size = new System.Drawing.Size(180, 27);
            this.dtScheduleDate.TabIndex = 0;
            this.dtScheduleDate.Value = new System.DateTime(2018, 10, 7, 0, 0, 0, 0);
            // 
            // dtDueDate
            // 
            this.dtDueDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtDueDate.CustomFormat = "yyyy/MM/dd";
            this.dtDueDate.Font = new System.Drawing.Font("新細明體", 11.2F);
            this.dtDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDueDate.Location = new System.Drawing.Point(409, 153);
            this.dtDueDate.Name = "dtDueDate";
            this.dtDueDate.Size = new System.Drawing.Size(180, 25);
            this.dtDueDate.TabIndex = 46;
            // 
            // LabDueDate
            // 
            this.LabDueDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabDueDate.AutoSize = true;
            this.LabDueDate.BackColor = System.Drawing.Color.Transparent;
            this.LabDueDate.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabDueDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabDueDate.Location = new System.Drawing.Point(299, 157);
            this.LabDueDate.Name = "LabDueDate";
            this.LabDueDate.Size = new System.Drawing.Size(60, 15);
            this.LabDueDate.TabIndex = 16;
            this.LabDueDate.Text = "Due Date";
            this.LabDueDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labSales
            // 
            this.labSales.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labSales.AutoSize = true;
            this.labSales.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labSales.Location = new System.Drawing.Point(595, 67);
            this.labSales.Name = "labSales";
            this.labSales.Size = new System.Drawing.Size(40, 16);
            this.labSales.TabIndex = 197;
            this.labSales.Text = "Sales";
            // 
            // txtClient2
            // 
            this.txtClient2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClient2.BackColor = System.Drawing.Color.White;
            this.txtClient2.Location = new System.Drawing.Point(409, 63);
            this.txtClient2.Name = "txtClient2";
            this.txtClient2.ReadOnly = true;
            this.txtClient2.Size = new System.Drawing.Size(180, 27);
            this.txtClient2.TabIndex = 198;
            // 
            // labOutSource
            // 
            this.labOutSource.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labOutSource.AutoSize = true;
            this.labOutSource.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labOutSource.Location = new System.Drawing.Point(595, 97);
            this.labOutSource.Name = "labOutSource";
            this.labOutSource.Size = new System.Drawing.Size(78, 16);
            this.labOutSource.TabIndex = 199;
            this.labOutSource.Text = "Out Source";
            // 
            // txtSales
            // 
            this.txtSales.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSales.BackColor = System.Drawing.Color.White;
            this.txtSales.Location = new System.Drawing.Point(705, 63);
            this.txtSales.Name = "txtSales";
            this.txtSales.ReadOnly = true;
            this.txtSales.Size = new System.Drawing.Size(180, 27);
            this.txtSales.TabIndex = 200;
            // 
            // labBlueprint
            // 
            this.labBlueprint.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labBlueprint.AutoSize = true;
            this.labBlueprint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labBlueprint.Location = new System.Drawing.Point(595, 307);
            this.labBlueprint.Name = "labBlueprint";
            this.labBlueprint.Size = new System.Drawing.Size(66, 16);
            this.labBlueprint.TabIndex = 201;
            this.labBlueprint.Text = "Blueprint";
            // 
            // txtOutsource
            // 
            this.txtOutsource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutsource.BackColor = System.Drawing.Color.White;
            this.txtOutsource.Location = new System.Drawing.Point(705, 93);
            this.txtOutsource.Name = "txtOutsource";
            this.txtOutsource.Size = new System.Drawing.Size(180, 27);
            this.txtOutsource.TabIndex = 202;
            // 
            // labPallets
            // 
            this.labPallets.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labPallets.AutoSize = true;
            this.labPallets.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labPallets.Location = new System.Drawing.Point(595, 127);
            this.labPallets.Name = "labPallets";
            this.labPallets.Size = new System.Drawing.Size(48, 16);
            this.labPallets.TabIndex = 203;
            this.labPallets.Text = "Pallets";
            // 
            // txtPallets
            // 
            this.txtPallets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPallets.BackColor = System.Drawing.Color.Yellow;
            this.txtPallets.Location = new System.Drawing.Point(705, 123);
            this.txtPallets.Name = "txtPallets";
            this.txtPallets.Size = new System.Drawing.Size(180, 27);
            this.txtPallets.TabIndex = 204;
            this.txtPallets.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPallets_KeyPress);
            // 
            // labOldNumber
            // 
            this.labOldNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labOldNumber.AutoSize = true;
            this.labOldNumber.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labOldNumber.Location = new System.Drawing.Point(3, 307);
            this.labOldNumber.Name = "labOldNumber";
            this.labOldNumber.Size = new System.Drawing.Size(86, 16);
            this.labOldNumber.TabIndex = 211;
            this.labOldNumber.Text = "Old Number";
            // 
            // labUnit
            // 
            this.labUnit.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labUnit.AutoSize = true;
            this.labUnit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labUnit.Location = new System.Drawing.Point(299, 307);
            this.labUnit.Name = "labUnit";
            this.labUnit.Size = new System.Drawing.Size(35, 16);
            this.labUnit.TabIndex = 212;
            this.labUnit.Text = "Unit";
            // 
            // txtOldNum
            // 
            this.txtOldNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOldNum.BackColor = System.Drawing.Color.White;
            this.txtOldNum.Location = new System.Drawing.Point(113, 303);
            this.txtOldNum.Name = "txtOldNum";
            this.txtOldNum.Size = new System.Drawing.Size(180, 27);
            this.txtOldNum.TabIndex = 213;
            // 
            // txtUnit
            // 
            this.txtUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUnit.BackColor = System.Drawing.Color.White;
            this.txtUnit.Location = new System.Drawing.Point(409, 303);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(180, 27);
            this.txtUnit.TabIndex = 215;
            // 
            // labFinished
            // 
            this.labFinished.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labFinished.AutoSize = true;
            this.labFinished.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labFinished.Location = new System.Drawing.Point(3, 337);
            this.labFinished.Name = "labFinished";
            this.labFinished.Size = new System.Drawing.Size(61, 16);
            this.labFinished.TabIndex = 216;
            this.labFinished.Text = "Finished";
            this.labFinished.Visible = false;
            // 
            // txtFinish
            // 
            this.txtFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFinish.BackColor = System.Drawing.SystemColors.Control;
            this.txtFinish.Location = new System.Drawing.Point(113, 333);
            this.txtFinish.Name = "txtFinish";
            this.txtFinish.ReadOnly = true;
            this.txtFinish.Size = new System.Drawing.Size(180, 27);
            this.txtFinish.TabIndex = 219;
            this.txtFinish.Visible = false;
            // 
            // labBad
            // 
            this.labBad.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labBad.AutoSize = true;
            this.labBad.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labBad.Location = new System.Drawing.Point(299, 337);
            this.labBad.Name = "labBad";
            this.labBad.Size = new System.Drawing.Size(33, 16);
            this.labBad.TabIndex = 224;
            this.labBad.Text = "Bad";
            this.labBad.Visible = false;
            // 
            // txtBad
            // 
            this.txtBad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBad.BackColor = System.Drawing.SystemColors.Control;
            this.txtBad.Location = new System.Drawing.Point(409, 333);
            this.txtBad.Name = "txtBad";
            this.txtBad.ReadOnly = true;
            this.txtBad.Size = new System.Drawing.Size(180, 27);
            this.txtBad.TabIndex = 225;
            this.txtBad.Visible = false;
            // 
            // labNotPaid
            // 
            this.labNotPaid.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labNotPaid.AutoSize = true;
            this.labNotPaid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labNotPaid.Location = new System.Drawing.Point(595, 337);
            this.labNotPaid.Name = "labNotPaid";
            this.labNotPaid.Size = new System.Drawing.Size(62, 16);
            this.labNotPaid.TabIndex = 226;
            this.labNotPaid.Text = "Not Paid";
            this.labNotPaid.Visible = false;
            // 
            // txtNotPaid
            // 
            this.txtNotPaid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotPaid.BackColor = System.Drawing.SystemColors.Control;
            this.txtNotPaid.Location = new System.Drawing.Point(705, 333);
            this.txtNotPaid.Name = "txtNotPaid";
            this.txtNotPaid.ReadOnly = true;
            this.txtNotPaid.Size = new System.Drawing.Size(180, 27);
            this.txtNotPaid.TabIndex = 227;
            this.txtNotPaid.Visible = false;
            // 
            // txtRule1
            // 
            this.txtRule1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.txtRule1, 3);
            this.txtRule1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRule1.Location = new System.Drawing.Point(113, 183);
            this.txtRule1.Multiline = true;
            this.txtRule1.Name = "txtRule1";
            this.tableLayoutPanel1.SetRowSpan(this.txtRule1, 2);
            this.txtRule1.Size = new System.Drawing.Size(476, 54);
            this.txtRule1.TabIndex = 248;
            // 
            // combMade
            // 
            this.combMade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.combMade.BackColor = System.Drawing.Color.Yellow;
            this.combMade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combMade.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.combMade.FormattingEnabled = true;
            this.combMade.Location = new System.Drawing.Point(705, 33);
            this.combMade.Name = "combMade";
            this.combMade.Size = new System.Drawing.Size(180, 24);
            this.combMade.TabIndex = 196;
            // 
            // labMadeCategory
            // 
            this.labMadeCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labMadeCategory.AutoSize = true;
            this.labMadeCategory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labMadeCategory.Location = new System.Drawing.Point(595, 37);
            this.labMadeCategory.Name = "labMadeCategory";
            this.labMadeCategory.Size = new System.Drawing.Size(72, 16);
            this.labMadeCategory.TabIndex = 195;
            this.labMadeCategory.Text = "Made Cat.";
            // 
            // cbNumber2
            // 
            this.cbNumber2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbNumber2.BackColor = System.Drawing.Color.Yellow;
            this.cbNumber2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNumber2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbNumber2.FormattingEnabled = true;
            this.cbNumber2.Location = new System.Drawing.Point(705, 3);
            this.cbNumber2.Name = "cbNumber2";
            this.cbNumber2.Size = new System.Drawing.Size(180, 24);
            this.cbNumber2.TabIndex = 233;
            this.cbNumber2.SelectedIndexChanged += new System.EventHandler(this.combNum1_SelectedIndexChanged);
            // 
            // txtClient
            // 
            this.txtClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClient.BackColor = System.Drawing.Color.White;
            this.txtClient.Location = new System.Drawing.Point(113, 63);
            this.txtClient.Name = "txtClient";
            this.txtClient.ReadOnly = true;
            this.txtClient.Size = new System.Drawing.Size(180, 27);
            this.txtClient.TabIndex = 237;
            // 
            // labProductRate
            // 
            this.labProductRate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labProductRate.AutoSize = true;
            this.labProductRate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labProductRate.Location = new System.Drawing.Point(299, 97);
            this.labProductRate.Name = "labProductRate";
            this.labProductRate.Size = new System.Drawing.Size(88, 16);
            this.labProductRate.TabIndex = 240;
            this.labProductRate.Text = "Product Rate";
            // 
            // txtProductRate
            // 
            this.txtProductRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProductRate.BackColor = System.Drawing.Color.White;
            this.txtProductRate.Location = new System.Drawing.Point(409, 93);
            this.txtProductRate.Name = "txtProductRate";
            this.txtProductRate.Size = new System.Drawing.Size(180, 27);
            this.txtProductRate.TabIndex = 243;
            this.txtProductRate.Text = "1";
            // 
            // labNumber1
            // 
            this.labNumber1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labNumber1.AutoSize = true;
            this.labNumber1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labNumber1.Location = new System.Drawing.Point(299, 7);
            this.labNumber1.Name = "labNumber1";
            this.labNumber1.Size = new System.Drawing.Size(67, 16);
            this.labNumber1.TabIndex = 231;
            this.labNumber1.Text = "Number1";
            // 
            // txtCustomize
            // 
            this.txtCustomize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomize.BackColor = System.Drawing.Color.White;
            this.txtCustomize.Location = new System.Drawing.Point(113, 93);
            this.txtCustomize.Name = "txtCustomize";
            this.txtCustomize.ReadOnly = true;
            this.txtCustomize.Size = new System.Drawing.Size(180, 27);
            this.txtCustomize.TabIndex = 239;
            // 
            // txtBlueprint
            // 
            this.txtBlueprint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBlueprint.BackColor = System.Drawing.Color.White;
            this.txtBlueprint.Location = new System.Drawing.Point(705, 303);
            this.txtBlueprint.Name = "txtBlueprint";
            this.txtBlueprint.Size = new System.Drawing.Size(180, 27);
            this.txtBlueprint.TabIndex = 235;
            // 
            // labCustomize
            // 
            this.labCustomize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labCustomize.AutoSize = true;
            this.labCustomize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labCustomize.Location = new System.Drawing.Point(3, 97);
            this.labCustomize.Name = "labCustomize";
            this.labCustomize.Size = new System.Drawing.Size(74, 16);
            this.labCustomize.TabIndex = 238;
            this.labCustomize.Text = "Customize";
            // 
            // labClient
            // 
            this.labClient.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labClient.AutoSize = true;
            this.labClient.BackColor = System.Drawing.Color.Transparent;
            this.labClient.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labClient.Location = new System.Drawing.Point(3, 67);
            this.labClient.Name = "labClient";
            this.labClient.Size = new System.Drawing.Size(45, 16);
            this.labClient.TabIndex = 228;
            this.labClient.Text = "Client";
            // 
            // combOperation
            // 
            this.combOperation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.combOperation.BackColor = System.Drawing.Color.Yellow;
            this.combOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combOperation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.combOperation.FormattingEnabled = true;
            this.combOperation.Location = new System.Drawing.Point(113, 3);
            this.combOperation.Name = "combOperation";
            this.combOperation.Size = new System.Drawing.Size(180, 24);
            this.combOperation.TabIndex = 229;
            this.combOperation.SelectedIndexChanged += new System.EventHandler(this.combOperation_SelectedIndexChanged);
            // 
            // labOperation
            // 
            this.labOperation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labOperation.AutoSize = true;
            this.labOperation.BackColor = System.Drawing.Color.Transparent;
            this.labOperation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labOperation.Location = new System.Drawing.Point(3, 7);
            this.labOperation.Name = "labOperation";
            this.labOperation.Size = new System.Drawing.Size(70, 16);
            this.labOperation.TabIndex = 230;
            this.labOperation.Text = "Operation";
            this.labOperation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labRule
            // 
            this.labRule.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labRule.AutoSize = true;
            this.labRule.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labRule.Location = new System.Drawing.Point(3, 187);
            this.labRule.Name = "labRule";
            this.labRule.Size = new System.Drawing.Size(37, 16);
            this.labRule.TabIndex = 251;
            this.labRule.Text = "Rule";
            // 
            // txtNum
            // 
            this.txtNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNum.BackColor = System.Drawing.Color.Yellow;
            this.txtNum.Location = new System.Drawing.Point(705, 183);
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(180, 27);
            this.txtNum.TabIndex = 253;
            this.txtNum.Visible = false;
            this.txtNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNum_KeyPress);
            // 
            // lblProd10B
            // 
            this.lblProd10B.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProd10B.AutoSize = true;
            this.lblProd10B.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblProd10B.Location = new System.Drawing.Point(298, 12);
            this.lblProd10B.Name = "lblProd10B";
            this.lblProd10B.Size = new System.Drawing.Size(71, 16);
            this.lblProd10B.TabIndex = 254;
            this.lblProd10B.Text = "Prod. 10B";
            // 
            // cbProd10B
            // 
            this.cbProd10B.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbProd10B.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProd10B.FormattingEnabled = true;
            this.cbProd10B.Location = new System.Drawing.Point(375, 5);
            this.cbProd10B.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cbProd10B.Name = "cbProd10B";
            this.cbProd10B.Size = new System.Drawing.Size(212, 24);
            this.cbProd10B.TabIndex = 255;
            this.cbProd10B.SelectedIndexChanged += new System.EventHandler(this.cbProd10B_SelectedIndexChanged);
            // 
            // cbNumber1
            // 
            this.cbNumber1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbNumber1.BackColor = System.Drawing.Color.Yellow;
            this.cbNumber1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNumber1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbNumber1.FormattingEnabled = true;
            this.cbNumber1.Location = new System.Drawing.Point(409, 3);
            this.cbNumber1.Name = "cbNumber1";
            this.cbNumber1.Size = new System.Drawing.Size(180, 24);
            this.cbNumber1.TabIndex = 256;
            this.cbNumber1.SelectedIndexChanged += new System.EventHandler(this.combDateTimeNum_SelectedIndexChanged);
            // 
            // labCustomerDate
            // 
            this.labCustomerDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labCustomerDate.AutoSize = true;
            this.labCustomerDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labCustomerDate.Location = new System.Drawing.Point(3, 127);
            this.labCustomerDate.Name = "labCustomerDate";
            this.labCustomerDate.Size = new System.Drawing.Size(73, 16);
            this.labCustomerDate.TabIndex = 258;
            this.labCustomerDate.Text = "Cust. Date";
            // 
            // labCustomerDueDate
            // 
            this.labCustomerDueDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labCustomerDueDate.AutoSize = true;
            this.labCustomerDueDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labCustomerDueDate.Location = new System.Drawing.Point(299, 127);
            this.labCustomerDueDate.Name = "labCustomerDueDate";
            this.labCustomerDueDate.Size = new System.Drawing.Size(103, 16);
            this.labCustomerDueDate.TabIndex = 259;
            this.labCustomerDueDate.Text = "Cust. Due Date";
            // 
            // dtCustDate
            // 
            this.dtCustDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtCustDate.CustomFormat = "yyyy/MM/dd";
            this.dtCustDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtCustDate.Location = new System.Drawing.Point(113, 123);
            this.dtCustDate.Name = "dtCustDate";
            this.dtCustDate.Size = new System.Drawing.Size(180, 27);
            this.dtCustDate.TabIndex = 260;
            this.dtCustDate.Value = new System.DateTime(2018, 10, 7, 15, 34, 0, 0);
            // 
            // dtCustDueDate
            // 
            this.dtCustDueDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtCustDueDate.CustomFormat = "yyyy/MM/dd";
            this.dtCustDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtCustDueDate.Location = new System.Drawing.Point(409, 123);
            this.dtCustDueDate.Name = "dtCustDueDate";
            this.dtCustDueDate.Size = new System.Drawing.Size(180, 27);
            this.dtCustDueDate.TabIndex = 262;
            // 
            // cbProd10A
            // 
            this.cbProd10A.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbProd10A.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProd10A.FormattingEnabled = true;
            this.cbProd10A.Location = new System.Drawing.Point(671, 5);
            this.cbProd10A.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cbProd10A.Name = "cbProd10A";
            this.cbProd10A.Size = new System.Drawing.Size(214, 24);
            this.cbProd10A.TabIndex = 264;
            this.cbProd10A.SelectedIndexChanged += new System.EventHandler(this.cbProd10A_SelectedIndexChanged);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtVersion, 2);
            this.txtVersion.Location = new System.Drawing.Point(1001, 123);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(180, 27);
            this.txtVersion.TabIndex = 278;
            // 
            // txtSequence
            // 
            this.txtSequence.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSequence.BackColor = System.Drawing.Color.Yellow;
            this.txtSequence.Location = new System.Drawing.Point(409, 33);
            this.txtSequence.Name = "txtSequence";
            this.txtSequence.Size = new System.Drawing.Size(180, 27);
            this.txtSequence.TabIndex = 277;
            this.txtSequence.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSequence_KeyPress);
            // 
            // gbBOMRelation
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.gbBOMRelation, 6);
            this.gbBOMRelation.Controls.Add(this.tableLayoutPanel2);
            this.gbBOMRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbBOMRelation.Location = new System.Drawing.Point(0, 240);
            this.gbBOMRelation.Margin = new System.Windows.Forms.Padding(0);
            this.gbBOMRelation.Name = "gbBOMRelation";
            this.gbBOMRelation.Padding = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.SetRowSpan(this.gbBOMRelation, 2);
            this.gbBOMRelation.Size = new System.Drawing.Size(888, 60);
            this.gbBOMRelation.TabIndex = 276;
            this.gbBOMRelation.TabStop = false;
            this.gbBOMRelation.Text = "BOM Relation";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.cbProd10A, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblProd10C, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblProd10A, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbProd10C, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbProd10B, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblProd10B, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 20);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(888, 40);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // lblProd10C
            // 
            this.lblProd10C.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProd10C.AutoSize = true;
            this.lblProd10C.Location = new System.Drawing.Point(3, 12);
            this.lblProd10C.Name = "lblProd10C";
            this.lblProd10C.Size = new System.Drawing.Size(71, 16);
            this.lblProd10C.TabIndex = 274;
            this.lblProd10C.Text = "Prod. 10C";
            // 
            // lblProd10A
            // 
            this.lblProd10A.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProd10A.AutoSize = true;
            this.lblProd10A.Location = new System.Drawing.Point(593, 12);
            this.lblProd10A.Name = "lblProd10A";
            this.lblProd10A.Size = new System.Drawing.Size(72, 16);
            this.lblProd10A.TabIndex = 273;
            this.lblProd10A.Text = "Prod. 10A";
            // 
            // cbProd10C
            // 
            this.cbProd10C.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbProd10C.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProd10C.FormattingEnabled = true;
            this.cbProd10C.Location = new System.Drawing.Point(80, 5);
            this.cbProd10C.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cbProd10C.Name = "cbProd10C";
            this.cbProd10C.Size = new System.Drawing.Size(212, 24);
            this.cbProd10C.TabIndex = 275;
            this.cbProd10C.SelectedIndexChanged += new System.EventHandler(this.cbProd10C_SelectedIndexChanged);
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Location = new System.Drawing.Point(299, 67);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(81, 16);
            this.lblCustomerName.TabIndex = 271;
            this.lblCustomerName.Text = "Cust. Name";
            // 
            // cbOrderSequence
            // 
            this.cbOrderSequence.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOrderSequence.BackColor = System.Drawing.Color.Yellow;
            this.cbOrderSequence.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrderSequence.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbOrderSequence.FormattingEnabled = true;
            this.cbOrderSequence.Location = new System.Drawing.Point(113, 33);
            this.cbOrderSequence.Name = "cbOrderSequence";
            this.cbOrderSequence.Size = new System.Drawing.Size(180, 24);
            this.cbOrderSequence.TabIndex = 270;
            this.cbOrderSequence.SelectedIndexChanged += new System.EventHandler(this.cbOrderSequence_SelectedIndexChanged);
            // 
            // lblOrderSequence
            // 
            this.lblOrderSequence.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOrderSequence.AutoSize = true;
            this.lblOrderSequence.Location = new System.Drawing.Point(3, 37);
            this.lblOrderSequence.Name = "lblOrderSequence";
            this.lblOrderSequence.Size = new System.Drawing.Size(75, 16);
            this.lblOrderSequence.TabIndex = 269;
            this.lblOrderSequence.Text = "Order Seq.";
            // 
            // lblSequence
            // 
            this.lblSequence.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSequence.AutoSize = true;
            this.lblSequence.Location = new System.Drawing.Point(299, 37);
            this.lblSequence.Name = "lblSequence";
            this.lblSequence.Size = new System.Drawing.Size(68, 16);
            this.lblSequence.TabIndex = 267;
            this.lblSequence.Text = "Sequence";
            // 
            // lblNumber2
            // 
            this.lblNumber2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNumber2.AutoSize = true;
            this.lblNumber2.Location = new System.Drawing.Point(595, 7);
            this.lblNumber2.Name = "lblNumber2";
            this.lblNumber2.Size = new System.Drawing.Size(67, 16);
            this.lblNumber2.TabIndex = 266;
            this.lblNumber2.Text = "Number2";
            // 
            // lblNTFC
            // 
            this.lblNTFC.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNTFC.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblNTFC, 2);
            this.lblNTFC.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblNTFC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblNTFC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblNTFC.Location = new System.Drawing.Point(1001, 7);
            this.lblNTFC.Name = "lblNTFC";
            this.lblNTFC.Size = new System.Drawing.Size(47, 15);
            this.lblNTFC.TabIndex = 265;
            this.lblNTFC.Text = "NTFC";
            // 
            // LabScheduleDate
            // 
            this.LabScheduleDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabScheduleDate.AutoSize = true;
            this.LabScheduleDate.BackColor = System.Drawing.Color.Transparent;
            this.LabScheduleDate.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.LabScheduleDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabScheduleDate.Location = new System.Drawing.Point(3, 157);
            this.LabScheduleDate.Name = "LabScheduleDate";
            this.LabScheduleDate.Size = new System.Drawing.Size(88, 15);
            this.LabScheduleDate.TabIndex = 14;
            this.LabScheduleDate.Text = "Schedule Date";
            this.LabScheduleDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(7, 105);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 242;
            this.label11.Text = "復核";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(228, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 240;
            this.label10.Text = "Product Rate";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Controls.Add(this.lblFactory, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtVersion, 7, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblNTFC, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtNotPaid, 5, 11);
            this.tableLayoutPanel1.Controls.Add(this.txtBlueprint, 5, 10);
            this.tableLayoutPanel1.Controls.Add(this.labNotPaid, 4, 11);
            this.tableLayoutPanel1.Controls.Add(this.gbBOMRelation, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtBad, 3, 11);
            this.tableLayoutPanel1.Controls.Add(this.txtSequence, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.labBad, 2, 11);
            this.tableLayoutPanel1.Controls.Add(this.labRule, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtFinish, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.txtRule1, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.labFinished, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.dtDueDate, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.dtScheduleDate, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.labBlueprint, 4, 10);
            this.tableLayoutPanel1.Controls.Add(this.txtUnit, 3, 10);
            this.tableLayoutPanel1.Controls.Add(this.lblCustomerName, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.labUnit, 2, 10);
            this.tableLayoutPanel1.Controls.Add(this.txtOldNum, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.dtCustDueDate, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.LabWO, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.labOldNumber, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.LabDueDate, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.labCustomerDueDate, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.dtCustDate, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbWO, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.LabScheduleDate, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.LabStatus, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.labCustomerDate, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblSequence, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbOrderSequence, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.LabWoStatus, 7, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblOrderSequence, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labCustomize, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtProductRate, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtCustomize, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.LabPart, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.labProductRate, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.editPart, 7, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtPallets, 5, 4);
            this.tableLayoutPanel1.Controls.Add(this.labClient, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labPallets, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblNumber2, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.LabVersion, 6, 4);
            this.tableLayoutPanel1.Controls.Add(this.editRemark, 7, 10);
            this.tableLayoutPanel1.Controls.Add(this.LabWoRule, 6, 5);
            this.tableLayoutPanel1.Controls.Add(this.combLine, 7, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtClient, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabRemark, 6, 10);
            this.tableLayoutPanel1.Controls.Add(this.combRoute, 7, 9);
            this.tableLayoutPanel1.Controls.Add(this.cbNumber1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtOutsource, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.combWoRule, 7, 5);
            this.tableLayoutPanel1.Controls.Add(this.combMade, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.labOutSource, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.labMadeCategory, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.editTargetQty, 7, 7);
            this.tableLayoutPanel1.Controls.Add(this.LabWoType, 6, 6);
            this.tableLayoutPanel1.Controls.Add(this.combWoType, 7, 6);
            this.tableLayoutPanel1.Controls.Add(this.combOperation, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labOperation, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LabTargetQty, 6, 7);
            this.tableLayoutPanel1.Controls.Add(this.LabLine, 6, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtSales, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.labNumber1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbNumber2, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.labSales, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtClient2, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabRoute, 6, 9);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 7, 13);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 8, 13);
            this.tableLayoutPanel1.Controls.Add(this.txtNum, 5, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 4, 5);
            this.tableLayoutPanel1.Controls.Add(this.TbBookNo, 5, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 14;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1184, 661);
            this.tableLayoutPanel1.TabIndex = 51;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(595, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 16);
            this.label1.TabIndex = 279;
            this.label1.Text = "Book number";
            // 
            // TbBookNo
            // 
            this.TbBookNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TbBookNo.BackColor = System.Drawing.Color.Yellow;
            this.TbBookNo.Location = new System.Drawing.Point(705, 153);
            this.TbBookNo.Name = "TbBookNo";
            this.TbBookNo.Size = new System.Drawing.Size(180, 27);
            this.TbBookNo.TabIndex = 280;
            // 
            // fData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "fData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.Load += new System.EventHandler(this.fData_Load);
            this.Shown += new System.EventHandler(this.fData_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).EndInit();
            this.labProd10B.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbBOMRelation.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }




        #endregion
        private Button btnCancel;
        private Button btnOK;
        private ColumnHeader columnPKSpec;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private DateTimePicker dateTimePicker1;
        private TextBox textBox1;
        private Label label5;
        private Label label2;
        private Label label9;
        private Label label3;
        private Label labClose;
        private Label label7;
        private Label label8;
        private Label label13;
        private Label label4;
        private Label label23;
        private Label label15;
        private Label label17;
        private Label label21;
        private Label label26;
        private Label label18;
        private Label label22;
        private Label label27;
        private Label label14;
        private Label label16;
        private Label label24;
        private Label label25;
        private Label label20;
        private Label label19;
        private Label label12;
        private Label labClientOrder;
        private TextBox txtUni;
        private Label labDate;
        private Label label6;
        private ComboBox comProduceNo;
        private TabControl tabControl1;
        private TabPage labProd10B;
        private TabPage tabPage2;
        private GroupBox groupBox1;
        private ListView LVPkSPec;
        private ColumnHeader PKSPEC_NAME;
        private ColumnHeader BOX_QTY;
        private ColumnHeader CARTON_QTY;
        private ColumnHeader PALLET_QTY;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ListView listViewOrderDetail;
        private ColumnHeader colOrderNumber;
        private ColumnHeader colCustomerName;
        private ColumnHeader colDueDateTime;
        private ColumnHeader colOrderCount;
        private ColumnHeader colProduceCount;
        private ColumnHeader colAccumulation;
        private ListView listView1;
        private ListView listView2;
        private ToolStripButton toolStripButton3;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem MenuAppend;
        private ToolStripMenuItem MenuRemove;
        private DateTimePicker dtScheduleDate;
        private TextBox editTargetQty;
        private Label lblFactory;
        private TextBox editRemark;
        private ComboBox combLine;
        private Label LabRemark;
        private DateTimePicker dtDueDate;
        private Label LabWO;
        private TextBox tbWO;
        private ComboBox combRoute;
        private Label LabRoute;
        private Label LabStatus;
        private Label LabLine;
        private Label LabWoStatus;
        private Label LabPart;
        private TextBox editPart;
        private Label LabVersion;
        private Label LabDueDate;
        private Label LabWoRule;
        private ComboBox combWoRule;
        private Label LabWoType;
        private ComboBox combWoType;
        private Label LabTargetQty;
        private Label labSales;
        private TextBox txtClient2;
        private Label labOutSource;
        private TextBox txtSales;
        private Label labBlueprint;
        private TextBox txtOutsource;
        private Label labPallets;
        private TextBox txtPallets;
        private Label labOldNumber;
        private Label labUnit;
        private TextBox txtOldNum;
        private TextBox txtUnit;
        private Label labFinished;
        private TextBox txtFinish;
        private Label labBad;
        private TextBox txtBad;
        private Label labNotPaid;
        private TextBox txtNotPaid;
        private TextBox txtRule1;
        private ComboBox combMade;
        private Label labMadeCategory;
        private ComboBox cbNumber2;
        private TextBox txtClient;
        private Label labProductRate;
        private TextBox txtProductRate;
        private Label labNumber1;
        private TextBox txtCustomize;
        private TextBox txtBlueprint;
        private Label labCustomize;
        private Label labClient;
        private ComboBox combOperation;
        private Label labOperation;
        private Label labRule;
        private TextBox txtNum;
        private Label lblProd10B;
        private ComboBox cbProd10B;
        private ComboBox cbNumber1;
        private Label labCustomerDate;
        private Label labCustomerDueDate;
        private DateTimePicker dtCustDate;
        private DateTimePicker dtCustDueDate;
        private ComboBox cbProd10A;
        private TabPage tabPage4;
        private DataGridView dgvRoute;
        private PrintPreviewDialog printPreviewDialog1;
        private PrintDocument printDocument1;
        private Label LabScheduleDate;
        private Label lblNTFC;
        private Label lblNumber2;
        private ComboBox cbOrderSequence;
        private Label lblOrderSequence;
        private Label lblSequence;
        private Label lblCustomerName;
        private Label lblProd10A;
        private ComboBox cbProd10C;
        private Label lblProd10C;
        private GroupBox gbBOMRelation;
        private TextBox txtSequence;
        private TextBox txtVersion;
        private Label label11;
        private Label label10;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private TextBox TbBookNo;
    }
}
