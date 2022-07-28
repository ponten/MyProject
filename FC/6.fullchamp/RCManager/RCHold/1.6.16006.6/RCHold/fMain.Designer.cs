namespace RCHold
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnPN = new System.Windows.Forms.Button();
            this.PN_textBox = new System.Windows.Forms.TextBox();
            this.Spec_textBox = new System.Windows.Forms.TextBox();
            this.DTD_lab = new System.Windows.Forms.Label();
            this.DTD_comb = new System.Windows.Forms.ComboBox();
            this.Excute_button = new System.Windows.Forms.Button();
            this.QUERY_button = new System.Windows.Forms.Button();
            this.RC_dgv = new System.Windows.Forms.DataGridView();
            this.SELECT = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RC_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROCESS_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CURRENT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CURRENT_STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HD_richTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnALLSELECT = new System.Windows.Forms.Button();
            this.btnALLCANCEL = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_OPTION1 = new System.Windows.Forms.TextBox();
            this.TB_OPTION2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TB_OPTION3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TB_OPTION4 = new System.Windows.Forms.TextBox();
            this.TB_OPTION5 = new System.Windows.Forms.TextBox();
            this.TB_OPTION6 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RC_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.1134F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.8866F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 418F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.Controls.Add(this.txt_RCSN, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.WO_label, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.PN_label, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.WO_textBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.combRCSN, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnPN, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.PN_textBox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.Spec_textBox, 1, 0);
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
            this.txt_RCSN.Location = new System.Drawing.Point(109, 69);
            this.txt_RCSN.Name = "txt_RCSN";
            this.txt_RCSN.Size = new System.Drawing.Size(174, 22);
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
            this.WO_textBox.Location = new System.Drawing.Point(109, 36);
            this.WO_textBox.Name = "WO_textBox";
            this.WO_textBox.Size = new System.Drawing.Size(174, 22);
            this.WO_textBox.TabIndex = 1;
            // 
            // combRCSN
            // 
            this.combRCSN.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.combRCSN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combRCSN.FormattingEnabled = true;
            this.combRCSN.Location = new System.Drawing.Point(3, 70);
            this.combRCSN.Name = "combRCSN";
            this.combRCSN.Size = new System.Drawing.Size(100, 20);
            this.combRCSN.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(289, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "label2";
            // 
            // btnPN
            // 
            this.btnPN.Location = new System.Drawing.Point(289, 3);
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
            this.PN_textBox.Location = new System.Drawing.Point(289, 36);
            this.PN_textBox.Name = "PN_textBox";
            this.PN_textBox.ReadOnly = true;
            this.PN_textBox.Size = new System.Drawing.Size(180, 22);
            this.PN_textBox.TabIndex = 3;
            this.PN_textBox.Visible = false;
            // 
            // Spec_textBox
            // 
            this.Spec_textBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Spec_textBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Spec_textBox.Location = new System.Drawing.Point(109, 4);
            this.Spec_textBox.Name = "Spec_textBox";
            this.Spec_textBox.ReadOnly = true;
            this.Spec_textBox.Size = new System.Drawing.Size(174, 22);
            this.Spec_textBox.TabIndex = 13;
            // 
            // DTD_lab
            // 
            this.DTD_lab.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DTD_lab.AutoSize = true;
            this.DTD_lab.Location = new System.Drawing.Point(15, 536);
            this.DTD_lab.Name = "DTD_lab";
            this.DTD_lab.Size = new System.Drawing.Size(117, 12);
            this.DTD_lab.TabIndex = 7;
            this.DTD_lab.Text = "DEFECT_TYPE_DESC";
            // 
            // DTD_comb
            // 
            this.DTD_comb.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DTD_comb.BackColor = System.Drawing.Color.White;
            this.DTD_comb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DTD_comb.FormattingEnabled = true;
            this.DTD_comb.Location = new System.Drawing.Point(15, 570);
            this.DTD_comb.Name = "DTD_comb";
            this.DTD_comb.Size = new System.Drawing.Size(300, 20);
            this.DTD_comb.TabIndex = 8;
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
            this.CURRENT_STATUS});
            this.RC_dgv.Location = new System.Drawing.Point(16, 133);
            this.RC_dgv.Name = "RC_dgv";
            this.RC_dgv.RowTemplate.Height = 23;
            this.RC_dgv.Size = new System.Drawing.Size(957, 277);
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
            // HD_richTextBox
            // 
            this.HD_richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.HD_richTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.HD_richTextBox.Location = new System.Drawing.Point(337, 566);
            this.HD_richTextBox.Name = "HD_richTextBox";
            this.HD_richTextBox.Size = new System.Drawing.Size(462, 59);
            this.HD_richTextBox.TabIndex = 4;
            this.HD_richTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(335, 536);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "HOLD_DESC";
            // 
            // btnALLSELECT
            // 
            this.btnALLSELECT.Location = new System.Drawing.Point(15, 416);
            this.btnALLSELECT.Name = "btnALLSELECT";
            this.btnALLSELECT.Size = new System.Drawing.Size(88, 30);
            this.btnALLSELECT.TabIndex = 9;
            this.btnALLSELECT.Text = "ALLSELECT";
            this.btnALLSELECT.UseVisualStyleBackColor = true;
            this.btnALLSELECT.Click += new System.EventHandler(this.btnALLSELECT_Click);
            // 
            // btnALLCANCEL
            // 
            this.btnALLCANCEL.Location = new System.Drawing.Point(115, 416);
            this.btnALLCANCEL.Name = "btnALLCANCEL";
            this.btnALLCANCEL.Size = new System.Drawing.Size(89, 30);
            this.btnALLCANCEL.TabIndex = 10;
            this.btnALLCANCEL.Text = "ALLCANCEL";
            this.btnALLCANCEL.UseVisualStyleBackColor = true;
            this.btnALLCANCEL.Click += new System.EventHandler(this.btnALLCANCEL_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(335, 446);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "OPTION1";
            // 
            // TB_OPTION1
            // 
            this.TB_OPTION1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TB_OPTION1.Location = new System.Drawing.Point(415, 442);
            this.TB_OPTION1.Name = "TB_OPTION1";
            this.TB_OPTION1.Size = new System.Drawing.Size(207, 22);
            this.TB_OPTION1.TabIndex = 12;
            // 
            // TB_OPTION2
            // 
            this.TB_OPTION2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TB_OPTION2.Location = new System.Drawing.Point(715, 442);
            this.TB_OPTION2.Name = "TB_OPTION2";
            this.TB_OPTION2.Size = new System.Drawing.Size(207, 22);
            this.TB_OPTION2.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(645, 446);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "OPTION2";
            // 
            // TB_OPTION3
            // 
            this.TB_OPTION3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TB_OPTION3.Location = new System.Drawing.Point(415, 472);
            this.TB_OPTION3.Name = "TB_OPTION3";
            this.TB_OPTION3.Size = new System.Drawing.Size(207, 22);
            this.TB_OPTION3.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(335, 476);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "OPTION3";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(645, 476);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "OPTION4";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(335, 506);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 12);
            this.label7.TabIndex = 18;
            this.label7.Text = "OPTION5";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(645, 506);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 12);
            this.label8.TabIndex = 19;
            this.label8.Text = "OPTION6";
            // 
            // TB_OPTION4
            // 
            this.TB_OPTION4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TB_OPTION4.Location = new System.Drawing.Point(715, 472);
            this.TB_OPTION4.Name = "TB_OPTION4";
            this.TB_OPTION4.Size = new System.Drawing.Size(207, 22);
            this.TB_OPTION4.TabIndex = 20;
            // 
            // TB_OPTION5
            // 
            this.TB_OPTION5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TB_OPTION5.Location = new System.Drawing.Point(415, 502);
            this.TB_OPTION5.Name = "TB_OPTION5";
            this.TB_OPTION5.Size = new System.Drawing.Size(207, 22);
            this.TB_OPTION5.TabIndex = 21;
            // 
            // TB_OPTION6
            // 
            this.TB_OPTION6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TB_OPTION6.Location = new System.Drawing.Point(715, 502);
            this.TB_OPTION6.Name = "TB_OPTION6";
            this.TB_OPTION6.Size = new System.Drawing.Size(207, 22);
            this.TB_OPTION6.TabIndex = 22;
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 662);
            this.Controls.Add(this.TB_OPTION6);
            this.Controls.Add(this.TB_OPTION5);
            this.Controls.Add(this.TB_OPTION4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TB_OPTION3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TB_OPTION2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TB_OPTION1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnALLCANCEL);
            this.Controls.Add(this.btnALLSELECT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DTD_lab);
            this.Controls.Add(this.HD_richTextBox);
            this.Controls.Add(this.DTD_comb);
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
        private System.Windows.Forms.TextBox PN_textBox;
        private System.Windows.Forms.TextBox txt_RCSN;
        private System.Windows.Forms.Button QUERY_button;
        private System.Windows.Forms.Button Excute_button;
        private System.Windows.Forms.DataGridView RC_dgv;
        private System.Windows.Forms.Label DTD_lab;
        private System.Windows.Forms.ComboBox DTD_comb;
        private System.Windows.Forms.RichTextBox HD_richTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPN;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SELECT;
        private System.Windows.Forms.DataGridViewTextBoxColumn RC_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROCESS_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn CURRENT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn CURRENT_STATUS;
        private System.Windows.Forms.ComboBox combRCSN;
        private System.Windows.Forms.Button btnALLSELECT;
        private System.Windows.Forms.Button btnALLCANCEL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Spec_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_OPTION1;
        private System.Windows.Forms.TextBox TB_OPTION2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TB_OPTION3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TB_OPTION4;
        private System.Windows.Forms.TextBox TB_OPTION5;
        private System.Windows.Forms.TextBox TB_OPTION6;
    }
}