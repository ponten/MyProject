namespace RCTransfer
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txt_RCSN = new System.Windows.Forms.TextBox();
            this.WO_label = new System.Windows.Forms.Label();
            this.PN_label = new System.Windows.Forms.Label();
            this.WO_textBox = new System.Windows.Forms.TextBox();
            this.combRCSN = new System.Windows.Forms.ComboBox();
            this.Spec_textBox = new System.Windows.Forms.TextBox();
            this.btnPN = new System.Windows.Forms.Button();
            this.PN_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_WorkOrder = new System.Windows.Forms.TextBox();
            this.Btn_WorkOrder = new System.Windows.Forms.Button();
            this.Excute_button = new System.Windows.Forms.Button();
            this.QUERY_button = new System.Windows.Forms.Button();
            this.RC_dgv = new System.Windows.Forms.DataGridView();
            this.SELECT = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RC_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROCESS_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CURRENT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CURRENT_STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PRIORITY_LEVEL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HD_richTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnALLSELECT = new System.Windows.Forms.Button();
            this.btnALLCANCEL = new System.Windows.Forms.Button();
            this.Label_TWO = new System.Windows.Forms.Label();
            this.Label_Spec1 = new System.Windows.Forms.Label();
            this.Label_RouteName = new System.Windows.Forms.Label();
            this.TB_Spec1 = new System.Windows.Forms.TextBox();
            this.TB_RouteName = new System.Windows.Forms.TextBox();
            this.Label_AvailableQty = new System.Windows.Forms.Label();
            this.TB_AvailableQty = new System.Windows.Forms.TextBox();
            this.TB_Spec2 = new System.Windows.Forms.TextBox();
            this.Label_Spec2 = new System.Windows.Forms.Label();
            this.TB_Version = new System.Windows.Forms.TextBox();
            this.Label_Version = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RC_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.11F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.89F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 418F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.Controls.Add(this.txt_RCSN, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.WO_label, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.PN_label, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.WO_textBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.combRCSN, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Spec_textBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnPN, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.PN_textBox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.35065F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.64935F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 98);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txt_RCSN
            // 
            this.txt_RCSN.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_RCSN.Location = new System.Drawing.Point(106, 69);
            this.txt_RCSN.Name = "txt_RCSN";
            this.txt_RCSN.Size = new System.Drawing.Size(169, 22);
            this.txt_RCSN.TabIndex = 5;
            // 
            // WO_label
            // 
            this.WO_label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.WO_label.AutoSize = true;
            this.WO_label.Location = new System.Drawing.Point(3, 41);
            this.WO_label.Name = "WO_label";
            this.WO_label.Size = new System.Drawing.Size(62, 12);
            this.WO_label.TabIndex = 0;
            this.WO_label.Text = "Work Order";
            // 
            // PN_label
            // 
            this.PN_label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PN_label.AutoSize = true;
            this.PN_label.Location = new System.Drawing.Point(3, 9);
            this.PN_label.Name = "PN_label";
            this.PN_label.Size = new System.Drawing.Size(56, 12);
            this.PN_label.TabIndex = 2;
            this.PN_label.Text = "Line Name";
            // 
            // WO_textBox
            // 
            this.WO_textBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.WO_textBox.Location = new System.Drawing.Point(106, 36);
            this.WO_textBox.Name = "WO_textBox";
            this.WO_textBox.Size = new System.Drawing.Size(169, 22);
            this.WO_textBox.TabIndex = 1;
            // 
            // combRCSN
            // 
            this.combRCSN.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.combRCSN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combRCSN.FormattingEnabled = true;
            this.combRCSN.Location = new System.Drawing.Point(3, 70);
            this.combRCSN.Name = "combRCSN";
            this.combRCSN.Size = new System.Drawing.Size(97, 20);
            this.combRCSN.TabIndex = 11;
            // 
            // Spec_textBox
            // 
            this.Spec_textBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Spec_textBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Spec_textBox.Location = new System.Drawing.Point(106, 4);
            this.Spec_textBox.Name = "Spec_textBox";
            this.Spec_textBox.ReadOnly = true;
            this.Spec_textBox.Size = new System.Drawing.Size(169, 22);
            this.Spec_textBox.TabIndex = 13;
            // 
            // btnPN
            // 
            this.btnPN.Location = new System.Drawing.Point(281, 3);
            this.btnPN.Name = "btnPN";
            this.btnPN.Size = new System.Drawing.Size(34, 25);
            this.btnPN.TabIndex = 10;
            this.btnPN.Text = "...";
            this.btnPN.UseVisualStyleBackColor = true;
            this.btnPN.Click += new System.EventHandler(this.btnPN_Click);
            // 
            // PN_textBox
            // 
            this.PN_textBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PN_textBox.Location = new System.Drawing.Point(281, 36);
            this.PN_textBox.Name = "PN_textBox";
            this.PN_textBox.ReadOnly = true;
            this.PN_textBox.Size = new System.Drawing.Size(174, 22);
            this.PN_textBox.TabIndex = 3;
            this.PN_textBox.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(281, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "label2";
            // 
            // TB_WorkOrder
            // 
            this.TB_WorkOrder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TB_WorkOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.TB_WorkOrder.Location = new System.Drawing.Point(250, 536);
            this.TB_WorkOrder.Name = "TB_WorkOrder";
            this.TB_WorkOrder.ReadOnly = true;
            this.TB_WorkOrder.Size = new System.Drawing.Size(174, 22);
            this.TB_WorkOrder.TabIndex = 19;
            // 
            // Btn_WorkOrder
            // 
            this.Btn_WorkOrder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Btn_WorkOrder.Location = new System.Drawing.Point(431, 535);
            this.Btn_WorkOrder.Name = "Btn_WorkOrder";
            this.Btn_WorkOrder.Size = new System.Drawing.Size(34, 25);
            this.Btn_WorkOrder.TabIndex = 20;
            this.Btn_WorkOrder.Text = "...";
            this.Btn_WorkOrder.UseVisualStyleBackColor = true;
            this.Btn_WorkOrder.Click += new System.EventHandler(this.Btn_WorkOrder_Click);
            // 
            // Excute_button
            // 
            this.Excute_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Excute_button.Location = new System.Drawing.Point(827, 565);
            this.Excute_button.Name = "Excute_button";
            this.Excute_button.Size = new System.Drawing.Size(144, 59);
            this.Excute_button.TabIndex = 2;
            this.Excute_button.Text = "EXCUTE";
            this.Excute_button.UseVisualStyleBackColor = true;
            this.Excute_button.Click += new System.EventHandler(this.Excute_button_Click);
            // 
            // QUERY_button
            // 
            this.QUERY_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.QUERY_button.Location = new System.Drawing.Point(827, 65);
            this.QUERY_button.Name = "QUERY_button";
            this.QUERY_button.Size = new System.Drawing.Size(144, 59);
            this.QUERY_button.TabIndex = 6;
            this.QUERY_button.Text = "QUERY";
            this.QUERY_button.UseVisualStyleBackColor = true;
            this.QUERY_button.Click += new System.EventHandler(this.QUERY_button_Click);
            // 
            // RC_dgv
            // 
            this.RC_dgv.AllowUserToAddRows = false;
            this.RC_dgv.AllowUserToDeleteRows = false;
            this.RC_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RC_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.RC_dgv.BackgroundColor = System.Drawing.Color.White;
            this.RC_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RC_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SELECT,
            this.RC_NO,
            this.PROCESS_NAME,
            this.CURRENT_QTY,
            this.CURRENT_STATUS,
            this.PRIORITY_LEVEL});
            this.RC_dgv.Location = new System.Drawing.Point(16, 133);
            this.RC_dgv.Name = "RC_dgv";
            this.RC_dgv.RowTemplate.Height = 23;
            this.RC_dgv.Size = new System.Drawing.Size(957, 337);
            this.RC_dgv.TabIndex = 3;
            // 
            // SELECT
            // 
            this.SELECT.FillWeight = 162.4366F;
            this.SELECT.HeaderText = "SELECT";
            this.SELECT.Name = "SELECT";
            this.SELECT.Width = 53;
            // 
            // RC_NO
            // 
            this.RC_NO.FillWeight = 79.18781F;
            this.RC_NO.HeaderText = "RC_NO";
            this.RC_NO.Name = "RC_NO";
            this.RC_NO.ReadOnly = true;
            this.RC_NO.Width = 68;
            // 
            // PROCESS_NAME
            // 
            this.PROCESS_NAME.FillWeight = 79.18781F;
            this.PROCESS_NAME.HeaderText = "PROCESS_NAME";
            this.PROCESS_NAME.Name = "PROCESS_NAME";
            this.PROCESS_NAME.ReadOnly = true;
            this.PROCESS_NAME.Width = 118;
            // 
            // CURRENT_QTY
            // 
            this.CURRENT_QTY.HeaderText = "CURRENT_QTY";
            this.CURRENT_QTY.Name = "CURRENT_QTY";
            this.CURRENT_QTY.ReadOnly = true;
            this.CURRENT_QTY.Width = 113;
            // 
            // CURRENT_STATUS
            // 
            this.CURRENT_STATUS.HeaderText = "CURRENT_STATUS";
            this.CURRENT_STATUS.Name = "CURRENT_STATUS";
            this.CURRENT_STATUS.ReadOnly = true;
            this.CURRENT_STATUS.Width = 132;
            // 
            // PRIORITY_LEVEL
            // 
            this.PRIORITY_LEVEL.HeaderText = "PRIORITY_LEVEL";
            this.PRIORITY_LEVEL.Name = "PRIORITY_LEVEL";
            this.PRIORITY_LEVEL.ReadOnly = true;
            this.PRIORITY_LEVEL.Visible = false;
            this.PRIORITY_LEVEL.Width = 125;
            // 
            // HD_richTextBox
            // 
            this.HD_richTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.HD_richTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.HD_richTextBox.Location = new System.Drawing.Point(15, 566);
            this.HD_richTextBox.Name = "HD_richTextBox";
            this.HD_richTextBox.Size = new System.Drawing.Size(62, 59);
            this.HD_richTextBox.TabIndex = 4;
            this.HD_richTextBox.Text = "";
            this.HD_richTextBox.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 540);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Transfer Desc";
            this.label1.Visible = false;
            // 
            // btnALLSELECT
            // 
            this.btnALLSELECT.Location = new System.Drawing.Point(15, 476);
            this.btnALLSELECT.Name = "btnALLSELECT";
            this.btnALLSELECT.Size = new System.Drawing.Size(88, 30);
            this.btnALLSELECT.TabIndex = 9;
            this.btnALLSELECT.Text = "ALLSELECT";
            this.btnALLSELECT.UseVisualStyleBackColor = true;
            this.btnALLSELECT.Click += new System.EventHandler(this.btnALLSELECT_Click);
            // 
            // btnALLCANCEL
            // 
            this.btnALLCANCEL.Location = new System.Drawing.Point(115, 476);
            this.btnALLCANCEL.Name = "btnALLCANCEL";
            this.btnALLCANCEL.Size = new System.Drawing.Size(89, 30);
            this.btnALLCANCEL.TabIndex = 10;
            this.btnALLCANCEL.Text = "ALLCANCEL";
            this.btnALLCANCEL.UseVisualStyleBackColor = true;
            this.btnALLCANCEL.Click += new System.EventHandler(this.btnALLCANCEL_Click);
            // 
            // Label_TWO
            // 
            this.Label_TWO.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_TWO.AutoSize = true;
            this.Label_TWO.Location = new System.Drawing.Point(150, 540);
            this.Label_TWO.Name = "Label_TWO";
            this.Label_TWO.Size = new System.Drawing.Size(65, 12);
            this.Label_TWO.TabIndex = 19;
            this.Label_TWO.Text = "Work Order ";
            // 
            // Label_Spec1
            // 
            this.Label_Spec1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_Spec1.AutoSize = true;
            this.Label_Spec1.Location = new System.Drawing.Point(500, 540);
            this.Label_Spec1.Name = "Label_Spec1";
            this.Label_Spec1.Size = new System.Drawing.Size(33, 12);
            this.Label_Spec1.TabIndex = 21;
            this.Label_Spec1.Text = "Spec1";
            // 
            // Label_RouteName
            // 
            this.Label_RouteName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_RouteName.AutoSize = true;
            this.Label_RouteName.Location = new System.Drawing.Point(150, 600);
            this.Label_RouteName.Name = "Label_RouteName";
            this.Label_RouteName.Size = new System.Drawing.Size(63, 12);
            this.Label_RouteName.TabIndex = 22;
            this.Label_RouteName.Text = "Route Name";
            // 
            // TB_Spec1
            // 
            this.TB_Spec1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TB_Spec1.BackColor = System.Drawing.SystemColors.Window;
            this.TB_Spec1.Location = new System.Drawing.Point(600, 536);
            this.TB_Spec1.Name = "TB_Spec1";
            this.TB_Spec1.ReadOnly = true;
            this.TB_Spec1.Size = new System.Drawing.Size(174, 22);
            this.TB_Spec1.TabIndex = 23;
            // 
            // TB_RouteName
            // 
            this.TB_RouteName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TB_RouteName.BackColor = System.Drawing.SystemColors.Window;
            this.TB_RouteName.Location = new System.Drawing.Point(250, 596);
            this.TB_RouteName.Name = "TB_RouteName";
            this.TB_RouteName.ReadOnly = true;
            this.TB_RouteName.Size = new System.Drawing.Size(174, 22);
            this.TB_RouteName.TabIndex = 24;
            // 
            // Label_AvailableQty
            // 
            this.Label_AvailableQty.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_AvailableQty.AutoSize = true;
            this.Label_AvailableQty.Location = new System.Drawing.Point(150, 570);
            this.Label_AvailableQty.Name = "Label_AvailableQty";
            this.Label_AvailableQty.Size = new System.Drawing.Size(69, 12);
            this.Label_AvailableQty.TabIndex = 25;
            this.Label_AvailableQty.Text = "Available Qty";
            // 
            // TB_AvailableQty
            // 
            this.TB_AvailableQty.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TB_AvailableQty.BackColor = System.Drawing.SystemColors.Window;
            this.TB_AvailableQty.Location = new System.Drawing.Point(250, 566);
            this.TB_AvailableQty.Name = "TB_AvailableQty";
            this.TB_AvailableQty.ReadOnly = true;
            this.TB_AvailableQty.Size = new System.Drawing.Size(174, 22);
            this.TB_AvailableQty.TabIndex = 26;
            // 
            // TB_Spec2
            // 
            this.TB_Spec2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TB_Spec2.BackColor = System.Drawing.SystemColors.Window;
            this.TB_Spec2.Location = new System.Drawing.Point(600, 566);
            this.TB_Spec2.Name = "TB_Spec2";
            this.TB_Spec2.ReadOnly = true;
            this.TB_Spec2.Size = new System.Drawing.Size(174, 22);
            this.TB_Spec2.TabIndex = 28;
            // 
            // Label_Spec2
            // 
            this.Label_Spec2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_Spec2.AutoSize = true;
            this.Label_Spec2.Location = new System.Drawing.Point(500, 570);
            this.Label_Spec2.Name = "Label_Spec2";
            this.Label_Spec2.Size = new System.Drawing.Size(33, 12);
            this.Label_Spec2.TabIndex = 27;
            this.Label_Spec2.Text = "Spec2";
            // 
            // TB_Version
            // 
            this.TB_Version.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TB_Version.BackColor = System.Drawing.SystemColors.Window;
            this.TB_Version.Location = new System.Drawing.Point(600, 596);
            this.TB_Version.Name = "TB_Version";
            this.TB_Version.ReadOnly = true;
            this.TB_Version.Size = new System.Drawing.Size(174, 22);
            this.TB_Version.TabIndex = 30;
            // 
            // Label_Version
            // 
            this.Label_Version.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label_Version.AutoSize = true;
            this.Label_Version.Location = new System.Drawing.Point(500, 600);
            this.Label_Version.Name = "Label_Version";
            this.Label_Version.Size = new System.Drawing.Size(41, 12);
            this.Label_Version.TabIndex = 29;
            this.Label_Version.Text = "Version";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 662);
            this.Controls.Add(this.TB_Version);
            this.Controls.Add(this.Label_Version);
            this.Controls.Add(this.TB_Spec2);
            this.Controls.Add(this.Label_Spec2);
            this.Controls.Add(this.TB_AvailableQty);
            this.Controls.Add(this.Label_AvailableQty);
            this.Controls.Add(this.TB_RouteName);
            this.Controls.Add(this.TB_Spec1);
            this.Controls.Add(this.Label_RouteName);
            this.Controls.Add(this.Label_Spec1);
            this.Controls.Add(this.Label_TWO);
            this.Controls.Add(this.btnALLCANCEL);
            this.Controls.Add(this.btnALLSELECT);
            this.Controls.Add(this.Btn_WorkOrder);
            this.Controls.Add(this.TB_WorkOrder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HD_richTextBox);
            this.Controls.Add(this.RC_dgv);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.Excute_button);
            this.Controls.Add(this.QUERY_button);
            this.Name = "fMain";
            this.Text = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RC_dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label WO_label;
        private System.Windows.Forms.TextBox WO_textBox;
        private System.Windows.Forms.Label PN_label;
        private System.Windows.Forms.TextBox txt_RCSN;
        private System.Windows.Forms.Button QUERY_button;
        private System.Windows.Forms.Button Excute_button;
        private System.Windows.Forms.DataGridView RC_dgv;
        private System.Windows.Forms.RichTextBox HD_richTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPN;
        private System.Windows.Forms.ComboBox combRCSN;
        private System.Windows.Forms.Button btnALLSELECT;
        private System.Windows.Forms.Button btnALLCANCEL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Spec_textBox;
        private System.Windows.Forms.TextBox TB_WorkOrder;
        private System.Windows.Forms.Button Btn_WorkOrder;
        private System.Windows.Forms.TextBox PN_textBox;
        private System.Windows.Forms.Label Label_TWO;
        private System.Windows.Forms.Label Label_Spec1;
        private System.Windows.Forms.Label Label_RouteName;
        private System.Windows.Forms.TextBox TB_Spec1;
        private System.Windows.Forms.TextBox TB_RouteName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SELECT;
        private System.Windows.Forms.DataGridViewTextBoxColumn RC_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROCESS_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn CURRENT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn CURRENT_STATUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRIORITY_LEVEL;
        private System.Windows.Forms.Label Label_AvailableQty;
        private System.Windows.Forms.TextBox TB_AvailableQty;
        private System.Windows.Forms.TextBox TB_Spec2;
        private System.Windows.Forms.Label Label_Spec2;
        private System.Windows.Forms.TextBox TB_Version;
        private System.Windows.Forms.Label Label_Version;
    }
}