namespace RC_ChangeRoute
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
            this.txtRC_NO = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGR = new System.Windows.Forms.Button();
            this.btnGP = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Release_richTxt = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCR = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGP = new System.Windows.Forms.TextBox();
            this.txtCP = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtWO = new System.Windows.Forms.TextBox();
            this.combGRN = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPART_NO = new System.Windows.Forms.TextBox();
            this.labPART_NO = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSpec2 = new System.Windows.Forms.TextBox();
            this.labSpec2 = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.labVersion = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRC_NO
            // 
            this.txtRC_NO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtRC_NO.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRC_NO.Location = new System.Drawing.Point(140, 16);
            this.txtRC_NO.Name = "txtRC_NO";
            this.txtRC_NO.Size = new System.Drawing.Size(174, 27);
            this.txtRC_NO.TabIndex = 0;
            this.txtRC_NO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRC_NO_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(30, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "RC";
            // 
            // btnGR
            // 
            this.btnGR.Location = new System.Drawing.Point(735, 56);
            this.btnGR.Name = "btnGR";
            this.btnGR.Size = new System.Drawing.Size(120, 25);
            this.btnGR.TabIndex = 3;
            this.btnGR.Text = "Transfer Route";
            this.btnGR.UseVisualStyleBackColor = true;
            this.btnGR.Visible = false;
            this.btnGR.Click += new System.EventHandler(this.btnGR_Click);
            // 
            // btnGP
            // 
            this.btnGP.Location = new System.Drawing.Point(735, 86);
            this.btnGP.Name = "btnGP";
            this.btnGP.Size = new System.Drawing.Size(120, 25);
            this.btnGP.TabIndex = 4;
            this.btnGP.Text = "Transfer Process";
            this.btnGP.UseVisualStyleBackColor = true;
            this.btnGP.Visible = false;
            this.btnGP.Click += new System.EventHandler(this.btnGP_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(31, 517);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "DESC";
            // 
            // Release_richTxt
            // 
            this.Release_richTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Release_richTxt.Location = new System.Drawing.Point(24, 546);
            this.Release_richTxt.Name = "Release_richTxt";
            this.Release_richTxt.Size = new System.Drawing.Size(819, 59);
            this.Release_richTxt.TabIndex = 7;
            this.Release_richTxt.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtVersion);
            this.groupBox1.Controls.Add(this.labVersion);
            this.groupBox1.Controls.Add(this.txtSpec2);
            this.groupBox1.Controls.Add(this.labSpec2);
            this.groupBox1.Controls.Add(this.txtCR);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtGP);
            this.groupBox1.Controls.Add(this.txtCP);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnGR);
            this.groupBox1.Controls.Add(this.txtWO);
            this.groupBox1.Controls.Add(this.btnGP);
            this.groupBox1.Controls.Add(this.combGRN);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtPART_NO);
            this.groupBox1.Controls.Add(this.labPART_NO);
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(25, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 150);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RC Information";
            // 
            // txtCR
            // 
            this.txtCR.BackColor = System.Drawing.SystemColors.Window;
            this.txtCR.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtCR.Location = new System.Drawing.Point(115, 56);
            this.txtCR.Name = "txtCR";
            this.txtCR.ReadOnly = true;
            this.txtCR.Size = new System.Drawing.Size(174, 27);
            this.txtCR.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Route Name";
            // 
            // txtGP
            // 
            this.txtGP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtGP.Location = new System.Drawing.Point(115, 116);
            this.txtGP.Name = "txtGP";
            this.txtGP.Size = new System.Drawing.Size(174, 27);
            this.txtGP.TabIndex = 12;
            // 
            // txtCP
            // 
            this.txtCP.BackColor = System.Drawing.SystemColors.Window;
            this.txtCP.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtCP.Location = new System.Drawing.Point(115, 86);
            this.txtCP.Name = "txtCP";
            this.txtCP.ReadOnly = true;
            this.txtCP.Size = new System.Drawing.Size(174, 27);
            this.txtCP.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 16);
            this.label7.TabIndex = 11;
            this.label7.Text = "Process Name ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Process Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(625, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 16);
            this.label6.TabIndex = 10;
            this.label6.Text = "Route Name";
            this.label6.Visible = false;
            // 
            // txtWO
            // 
            this.txtWO.BackColor = System.Drawing.SystemColors.Window;
            this.txtWO.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtWO.Location = new System.Drawing.Point(115, 26);
            this.txtWO.Name = "txtWO";
            this.txtWO.ReadOnly = true;
            this.txtWO.Size = new System.Drawing.Size(174, 27);
            this.txtWO.TabIndex = 3;
            // 
            // combGRN
            // 
            this.combGRN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combGRN.FormattingEnabled = true;
            this.combGRN.Location = new System.Drawing.Point(735, 26);
            this.combGRN.Name = "combGRN";
            this.combGRN.Size = new System.Drawing.Size(174, 24);
            this.combGRN.TabIndex = 9;
            this.combGRN.Visible = false;
            this.combGRN.SelectedValueChanged += new System.EventHandler(this.combGRN_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Work Order";
            // 
            // txtPART_NO
            // 
            this.txtPART_NO.BackColor = System.Drawing.SystemColors.Window;
            this.txtPART_NO.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtPART_NO.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtPART_NO.Location = new System.Drawing.Point(425, 26);
            this.txtPART_NO.Name = "txtPART_NO";
            this.txtPART_NO.ReadOnly = true;
            this.txtPART_NO.Size = new System.Drawing.Size(174, 27);
            this.txtPART_NO.TabIndex = 1;
            // 
            // labPART_NO
            // 
            this.labPART_NO.AutoSize = true;
            this.labPART_NO.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labPART_NO.Location = new System.Drawing.Point(315, 30);
            this.labPART_NO.Name = "labPART_NO";
            this.labPART_NO.Size = new System.Drawing.Size(46, 16);
            this.labPART_NO.TabIndex = 0;
            this.labPART_NO.Text = "Spec1";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(24, 218);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 286);
            this.panel1.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(864, 545);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 59);
            this.button1.TabIndex = 14;
            this.button1.Text = "EXCUTE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSpec2
            // 
            this.txtSpec2.BackColor = System.Drawing.SystemColors.Window;
            this.txtSpec2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtSpec2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtSpec2.Location = new System.Drawing.Point(425, 56);
            this.txtSpec2.Name = "txtSpec2";
            this.txtSpec2.ReadOnly = true;
            this.txtSpec2.Size = new System.Drawing.Size(174, 27);
            this.txtSpec2.TabIndex = 14;
            // 
            // labSpec2
            // 
            this.labSpec2.AutoSize = true;
            this.labSpec2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labSpec2.Location = new System.Drawing.Point(315, 60);
            this.labSpec2.Name = "labSpec2";
            this.labSpec2.Size = new System.Drawing.Size(46, 16);
            this.labSpec2.TabIndex = 13;
            this.labSpec2.Text = "Spec2";
            // 
            // txtVersion
            // 
            this.txtVersion.BackColor = System.Drawing.SystemColors.Window;
            this.txtVersion.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtVersion.Location = new System.Drawing.Point(425, 86);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.ReadOnly = true;
            this.txtVersion.Size = new System.Drawing.Size(174, 27);
            this.txtVersion.TabIndex = 16;
            // 
            // labVersion
            // 
            this.labVersion.AutoSize = true;
            this.labVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labVersion.Location = new System.Drawing.Point(315, 90);
            this.labVersion.Name = "labVersion";
            this.labVersion.Size = new System.Drawing.Size(57, 16);
            this.labVersion.TabIndex = 15;
            this.labVersion.Text = "Version";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 623);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Release_richTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRC_NO);
            this.Name = "fMain";
            this.Text = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRC_NO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGR;
        private System.Windows.Forms.Button btnGP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox Release_richTxt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtCP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtWO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPART_NO;
        private System.Windows.Forms.Label labPART_NO;
        private System.Windows.Forms.ComboBox combGRN;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtGP;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtCR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label labVersion;
        private System.Windows.Forms.TextBox txtSpec2;
        private System.Windows.Forms.Label labSpec2;
    }
}