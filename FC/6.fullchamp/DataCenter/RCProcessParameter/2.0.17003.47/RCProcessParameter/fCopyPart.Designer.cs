namespace RCProcessParam
{
    partial class fCopyPart
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ckbAll = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.editPart = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnUnchoose = new System.Windows.Forms.Button();
            this.btnChoose = new System.Windows.Forms.Button();
            this.gbAll = new System.Windows.Forms.GroupBox();
            this.LVAll = new System.Windows.Forms.ListView();
            this.gbChoose = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.LVChoose = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.gbAll.SuspendLayout();
            this.gbChoose.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblVersion);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ckbAll);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.editPart);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(759, 56);
            this.panel1.TabIndex = 16;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.ForeColor = System.Drawing.Color.DarkRed;
            this.lblVersion.Location = new System.Drawing.Point(452, 21);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(31, 15);
            this.lblVersion.TabIndex = 23;
            this.lblVersion.Text = "N/A";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(381, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 15);
            this.label1.TabIndex = 22;
            this.label1.Text = "Version";
            // 
            // ckbAll
            // 
            this.ckbAll.AutoSize = true;
            this.ckbAll.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ckbAll.Location = new System.Drawing.Point(613, 18);
            this.ckbAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckbAll.Name = "ckbAll";
            this.ckbAll.Size = new System.Drawing.Size(104, 24);
            this.ckbAll.TabIndex = 21;
            this.ckbAll.Text = "Select All";
            this.ckbAll.UseVisualStyleBackColor = true;
            this.ckbAll.CheckedChanged += new System.EventHandler(this.ckbAll_CheckedChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearch.Location = new System.Drawing.Point(316, 12);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(39, 34);
            this.btnSearch.TabIndex = 20;
            this.btnSearch.Text = "..";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // editPart
            // 
            this.editPart.Font = new System.Drawing.Font("新細明體", 12F);
            this.editPart.Location = new System.Drawing.Point(87, 14);
            this.editPart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editPart.Name = "editPart";
            this.editPart.Size = new System.Drawing.Size(221, 31);
            this.editPart.TabIndex = 19;
            this.editPart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editPart_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(13, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "Part No";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnOK);
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 488);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(759, 50);
            this.panel3.TabIndex = 40;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(490, 8);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 31);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(620, 8);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 31);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnUnchoose);
            this.panel6.Controls.Add(this.btnChoose);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(350, 56);
            this.panel6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(59, 432);
            this.panel6.TabIndex = 43;
            // 
            // btnUnchoose
            // 
            this.btnUnchoose.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnUnchoose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnUnchoose.Location = new System.Drawing.Point(5, 250);
            this.btnUnchoose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUnchoose.Name = "btnUnchoose";
            this.btnUnchoose.Size = new System.Drawing.Size(48, 38);
            this.btnUnchoose.TabIndex = 19;
            this.btnUnchoose.Text = "<<";
            this.btnUnchoose.UseVisualStyleBackColor = true;
            this.btnUnchoose.Click += new System.EventHandler(this.btnUnchoose_Click);
            // 
            // btnChoose
            // 
            this.btnChoose.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnChoose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChoose.Location = new System.Drawing.Point(5, 125);
            this.btnChoose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(48, 38);
            this.btnChoose.TabIndex = 18;
            this.btnChoose.Text = ">>";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // gbAll
            // 
            this.gbAll.Controls.Add(this.LVAll);
            this.gbAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbAll.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.gbAll.Location = new System.Drawing.Point(0, 56);
            this.gbAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbAll.Name = "gbAll";
            this.gbAll.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbAll.Size = new System.Drawing.Size(350, 432);
            this.gbAll.TabIndex = 42;
            this.gbAll.TabStop = false;
            this.gbAll.Text = "Origin";
            // 
            // LVAll
            // 
            this.LVAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LVAll.HideSelection = false;
            this.LVAll.Location = new System.Drawing.Point(4, 27);
            this.LVAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LVAll.Name = "LVAll";
            this.LVAll.Size = new System.Drawing.Size(342, 401);
            this.LVAll.TabIndex = 0;
            this.LVAll.UseCompatibleStateImageBehavior = false;
            this.LVAll.View = System.Windows.Forms.View.Details;
            this.LVAll.SelectedIndexChanged += new System.EventHandler(this.LVAll_SelectedIndexChanged);
            this.LVAll.DoubleClick += new System.EventHandler(this.LVAll_DoubleClick);
            // 
            // gbChoose
            // 
            this.gbChoose.Controls.Add(this.listBox1);
            this.gbChoose.Controls.Add(this.listBox2);
            this.gbChoose.Controls.Add(this.LVChoose);
            this.gbChoose.Dock = System.Windows.Forms.DockStyle.Right;
            this.gbChoose.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.gbChoose.Location = new System.Drawing.Point(409, 56);
            this.gbChoose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbChoose.Name = "gbChoose";
            this.gbChoose.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbChoose.Size = new System.Drawing.Size(350, 432);
            this.gbChoose.TabIndex = 42;
            this.gbChoose.TabStop = false;
            this.gbChoose.Text = "Target";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 19;
            this.listBox1.Location = new System.Drawing.Point(88, 85);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(135, 99);
            this.listBox1.TabIndex = 26;
            this.listBox1.Visible = false;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 19;
            this.listBox2.Location = new System.Drawing.Point(20, 188);
            this.listBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(135, 99);
            this.listBox2.TabIndex = 25;
            this.listBox2.Visible = false;
            // 
            // LVChoose
            // 
            this.LVChoose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LVChoose.HideSelection = false;
            this.LVChoose.Location = new System.Drawing.Point(4, 27);
            this.LVChoose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LVChoose.Name = "LVChoose";
            this.LVChoose.Size = new System.Drawing.Size(342, 401);
            this.LVChoose.TabIndex = 1;
            this.LVChoose.UseCompatibleStateImageBehavior = false;
            this.LVChoose.View = System.Windows.Forms.View.Details;
            this.LVChoose.SelectedIndexChanged += new System.EventHandler(this.LVChoose_SelectedIndexChanged);
            // 
            // fCopyPart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 538);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.gbAll);
            this.Controls.Add(this.gbChoose);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(777, 585);
            this.Name = "fCopyPart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Copy Part Parameter";
            this.Load += new System.EventHandler(this.fCopyPart_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.gbAll.ResumeLayout(false);
            this.gbChoose.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox editPart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnUnchoose;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.GroupBox gbAll;
        private System.Windows.Forms.ListView LVAll;
        private System.Windows.Forms.GroupBox gbChoose;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListView LVChoose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox ckbAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblVersion;
    }
}