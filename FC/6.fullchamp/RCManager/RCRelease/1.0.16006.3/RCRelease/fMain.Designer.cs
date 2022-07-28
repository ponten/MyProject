namespace RC_Release
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
            this.Release_dgv = new System.Windows.Forms.DataGridView();
            this.SELECT = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.HOLD_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEFECT_TYPE_DESC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HOLD_USERID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HOLD_INTERVAL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HOLD_DESC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Release_richTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RC_dgv = new System.Windows.Forms.DataGridView();
            this.RC_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERIAL_NUMBER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROCESS_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CURRENT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Hold_NOlabel = new System.Windows.Forms.Label();
            this.QUERY_btn = new System.Windows.Forms.Button();
            this.RELEASE_btn = new System.Windows.Forms.Button();
            this.txt_RCSN = new System.Windows.Forms.TextBox();
            this.WO_Label = new System.Windows.Forms.Label();
            this.PN_TBOX = new System.Windows.Forms.TextBox();
            this.PN_Label = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtWO = new System.Windows.Forms.TextBox();
            this.txtHold_no = new System.Windows.Forms.TextBox();
            this.combRCSN = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.Release_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RC_dgv)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Release_dgv
            // 
            this.Release_dgv.AllowUserToAddRows = false;
            this.Release_dgv.AllowUserToDeleteRows = false;
            this.Release_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Release_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.Release_dgv.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Release_dgv.ColumnHeadersHeight = 25;
            this.Release_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SELECT,
            this.HOLD_NO,
            this.DEFECT_TYPE_DESC,
            this.HOLD_USERID,
            this.HOLD_INTERVAL,
            this.HOLD_DESC});
            this.Release_dgv.Location = new System.Drawing.Point(10, 153);
            this.Release_dgv.Name = "Release_dgv";
            this.Release_dgv.RowTemplate.Height = 23;
            this.Release_dgv.Size = new System.Drawing.Size(1337, 200);
            this.Release_dgv.TabIndex = 1;
            this.Release_dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Release_dgv_CellClick);
            this.Release_dgv.SelectionChanged += new System.EventHandler(this.Release_dgv_SelectionChanged);
            // 
            // SELECT
            // 
            this.SELECT.HeaderText = "SELECT";
            this.SELECT.Name = "SELECT";
            this.SELECT.Width = 53;
            // 
            // HOLD_NO
            // 
            this.HOLD_NO.FillWeight = 25.77319F;
            this.HOLD_NO.HeaderText = "HOLD_NO";
            this.HOLD_NO.Name = "HOLD_NO";
            this.HOLD_NO.ReadOnly = true;
            this.HOLD_NO.Width = 83;
            // 
            // DEFECT_TYPE_DESC
            // 
            this.DEFECT_TYPE_DESC.FillWeight = 25.77319F;
            this.DEFECT_TYPE_DESC.HeaderText = "DEFECT_TYPE_DESC";
            this.DEFECT_TYPE_DESC.Name = "DEFECT_TYPE_DESC";
            this.DEFECT_TYPE_DESC.ReadOnly = true;
            this.DEFECT_TYPE_DESC.Width = 142;
            // 
            // HOLD_USERID
            // 
            this.HOLD_USERID.FillWeight = 25.77319F;
            this.HOLD_USERID.HeaderText = "HOLD_USERID";
            this.HOLD_USERID.Name = "HOLD_USERID";
            this.HOLD_USERID.ReadOnly = true;
            this.HOLD_USERID.Width = 108;
            // 
            // HOLD_INTERVAL
            // 
            this.HOLD_INTERVAL.FillWeight = 25.77319F;
            this.HOLD_INTERVAL.HeaderText = "HOLD_INTERVAL";
            this.HOLD_INTERVAL.Name = "HOLD_INTERVAL";
            this.HOLD_INTERVAL.ReadOnly = true;
            this.HOLD_INTERVAL.Width = 124;
            // 
            // HOLD_DESC
            // 
            this.HOLD_DESC.FillWeight = 396.9072F;
            this.HOLD_DESC.HeaderText = "HOLD_DESC";
            this.HOLD_DESC.Name = "HOLD_DESC";
            this.HOLD_DESC.ReadOnly = true;
            this.HOLD_DESC.Width = 96;
            // 
            // Release_richTextBox
            // 
            this.Release_richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Release_richTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Release_richTextBox.Location = new System.Drawing.Point(10, 543);
            this.Release_richTextBox.Name = "Release_richTextBox";
            this.Release_richTextBox.Size = new System.Drawing.Size(1168, 59);
            this.Release_richTextBox.TabIndex = 2;
            this.Release_richTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 516);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "DESC";
            // 
            // RC_dgv
            // 
            this.RC_dgv.AllowUserToAddRows = false;
            this.RC_dgv.AllowUserToDeleteRows = false;
            this.RC_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RC_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.RC_dgv.BackgroundColor = System.Drawing.SystemColors.Control;
            this.RC_dgv.ColumnHeadersHeight = 25;
            this.RC_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RC_NO,
            this.SERIAL_NUMBER,
            this.PROCESS_NAME,
            this.CURRENT_QTY});
            this.RC_dgv.Cursor = System.Windows.Forms.Cursors.Default;
            this.RC_dgv.Location = new System.Drawing.Point(10, 359);
            this.RC_dgv.Name = "RC_dgv";
            this.RC_dgv.ReadOnly = true;
            this.RC_dgv.RowTemplate.Height = 23;
            this.RC_dgv.Size = new System.Drawing.Size(1337, 139);
            this.RC_dgv.TabIndex = 4;
            // 
            // RC_NO
            // 
            this.RC_NO.HeaderText = "RC_NO";
            this.RC_NO.Name = "RC_NO";
            this.RC_NO.ReadOnly = true;
            this.RC_NO.Width = 68;
            // 
            // SERIAL_NUMBER
            // 
            this.SERIAL_NUMBER.HeaderText = "SERIAL_NUMBER";
            this.SERIAL_NUMBER.Name = "SERIAL_NUMBER";
            this.SERIAL_NUMBER.ReadOnly = true;
            this.SERIAL_NUMBER.Width = 125;
            // 
            // PROCESS_NAME
            // 
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
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button1.Location = new System.Drawing.Point(297, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(34, 22);
            this.button1.TabIndex = 11;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoEllipsis = true;
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(297, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "label2";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Hold_NOlabel
            // 
            this.Hold_NOlabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Hold_NOlabel.AutoSize = true;
            this.Hold_NOlabel.Location = new System.Drawing.Point(3, 71);
            this.Hold_NOlabel.Name = "Hold_NOlabel";
            this.Hold_NOlabel.Size = new System.Drawing.Size(58, 12);
            this.Hold_NOlabel.TabIndex = 8;
            this.Hold_NOlabel.Text = "HOLD_NO";
            // 
            // QUERY_btn
            // 
            this.QUERY_btn.Location = new System.Drawing.Point(1203, 73);
            this.QUERY_btn.Name = "QUERY_btn";
            this.QUERY_btn.Size = new System.Drawing.Size(144, 59);
            this.QUERY_btn.TabIndex = 6;
            this.QUERY_btn.Text = "QUERY";
            this.QUERY_btn.UseVisualStyleBackColor = true;
            this.QUERY_btn.Click += new System.EventHandler(this.QUERY_btn_Click);
            // 
            // RELEASE_btn
            // 
            this.RELEASE_btn.Location = new System.Drawing.Point(1203, 543);
            this.RELEASE_btn.Name = "RELEASE_btn";
            this.RELEASE_btn.Size = new System.Drawing.Size(144, 59);
            this.RELEASE_btn.TabIndex = 7;
            this.RELEASE_btn.Text = "RELEASE";
            this.RELEASE_btn.UseVisualStyleBackColor = true;
            this.RELEASE_btn.Click += new System.EventHandler(this.RELEASE_btn_Click);
            // 
            // txt_RCSN
            // 
            this.txt_RCSN.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_RCSN.Location = new System.Drawing.Point(120, 98);
            this.txt_RCSN.Name = "txt_RCSN";
            this.txt_RCSN.Size = new System.Drawing.Size(161, 22);
            this.txt_RCSN.TabIndex = 5;
            // 
            // WO_Label
            // 
            this.WO_Label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.WO_Label.AutoSize = true;
            this.WO_Label.Location = new System.Drawing.Point(3, 38);
            this.WO_Label.Name = "WO_Label";
            this.WO_Label.Size = new System.Drawing.Size(85, 12);
            this.WO_Label.TabIndex = 2;
            this.WO_Label.Text = "WORK_ORDER";
            // 
            // PN_TBOX
            // 
            this.PN_TBOX.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PN_TBOX.Location = new System.Drawing.Point(120, 3);
            this.PN_TBOX.Name = "PN_TBOX";
            this.PN_TBOX.ReadOnly = true;
            this.PN_TBOX.Size = new System.Drawing.Size(161, 22);
            this.PN_TBOX.TabIndex = 1;
            // 
            // PN_Label
            // 
            this.PN_Label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PN_Label.AutoSize = true;
            this.PN_Label.Location = new System.Drawing.Point(3, 8);
            this.PN_Label.Name = "PN_Label";
            this.PN_Label.Size = new System.Drawing.Size(56, 12);
            this.PN_Label.TabIndex = 0;
            this.PN_Label.Text = "PART_NO";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.86486F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.13514F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 494F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 226F));
            this.tableLayoutPanel1.Controls.Add(this.PN_Label, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.PN_TBOX, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.WO_Label, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtWO, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.Hold_NOlabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txt_RCSN, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtHold_no, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.combRCSN, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.03175F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.96825F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1015, 126);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtWO
            // 
            this.txtWO.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtWO.Location = new System.Drawing.Point(120, 33);
            this.txtWO.Name = "txtWO";
            this.txtWO.Size = new System.Drawing.Size(160, 22);
            this.txtWO.TabIndex = 16;
            // 
            // txtHold_no
            // 
            this.txtHold_no.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtHold_no.Location = new System.Drawing.Point(120, 66);
            this.txtHold_no.Name = "txtHold_no";
            this.txtHold_no.Size = new System.Drawing.Size(161, 22);
            this.txtHold_no.TabIndex = 15;
            // 
            // combRCSN
            // 
            this.combRCSN.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.combRCSN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combRCSN.FormattingEnabled = true;
            this.combRCSN.Location = new System.Drawing.Point(3, 99);
            this.combRCSN.Name = "combRCSN";
            this.combRCSN.Size = new System.Drawing.Size(111, 20);
            this.combRCSN.TabIndex = 17;
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1359, 639);
            this.Controls.Add(this.RC_dgv);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Release_richTextBox);
            this.Controls.Add(this.Release_dgv);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.RELEASE_btn);
            this.Controls.Add(this.QUERY_btn);
            this.Name = "fMain";
            this.Text = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Release_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RC_dgv)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Release_dgv;
        private System.Windows.Forms.RichTextBox Release_richTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView RC_dgv;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Hold_NOlabel;
        private System.Windows.Forms.Button QUERY_btn;
        private System.Windows.Forms.Button RELEASE_btn;
        private System.Windows.Forms.TextBox txt_RCSN;
        private System.Windows.Forms.Label WO_Label;
        private System.Windows.Forms.TextBox PN_TBOX;
        private System.Windows.Forms.Label PN_Label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtHold_no;
        private System.Windows.Forms.TextBox txtWO;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SELECT;
        private System.Windows.Forms.DataGridViewTextBoxColumn HOLD_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEFECT_TYPE_DESC;
        private System.Windows.Forms.DataGridViewTextBoxColumn HOLD_USERID;
        private System.Windows.Forms.DataGridViewTextBoxColumn HOLD_INTERVAL;
        private System.Windows.Forms.DataGridViewTextBoxColumn HOLD_DESC;
        private System.Windows.Forms.DataGridViewTextBoxColumn RC_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn SERIAL_NUMBER;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROCESS_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn CURRENT_QTY;
        private System.Windows.Forms.ComboBox combRCSN;
    }
}