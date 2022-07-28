namespace PackCarton
{
    partial class fWeightCarton
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStandardWeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCartonNo = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTestValue = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMaxWeight = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMinWeight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPartNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCOM = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtStandardWeight);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtCartonNo);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.txtMsg);
            this.panel1.Controls.Add(this.txtResult);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.txtTestValue);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtMaxWeight);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtMinWeight);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtPartNo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtCOM);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(492, 560);
            this.panel1.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(431, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 19);
            this.label4.TabIndex = 26;
            this.label4.Text = "KG";
            // 
            // txtStandardWeight
            // 
            this.txtStandardWeight.ForeColor = System.Drawing.Color.Red;
            this.txtStandardWeight.Location = new System.Drawing.Point(139, 170);
            this.txtStandardWeight.Name = "txtStandardWeight";
            this.txtStandardWeight.ReadOnly = true;
            this.txtStandardWeight.Size = new System.Drawing.Size(286, 29);
            this.txtStandardWeight.TabIndex = 25;
            this.txtStandardWeight.Text = "N/A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 19);
            this.label2.TabIndex = 24;
            this.label2.Text = "Standard Weight";
            // 
            // txtCartonNo
            // 
            this.txtCartonNo.ForeColor = System.Drawing.Color.Red;
            this.txtCartonNo.Location = new System.Drawing.Point(140, 71);
            this.txtCartonNo.Name = "txtCartonNo";
            this.txtCartonNo.ReadOnly = true;
            this.txtCartonNo.Size = new System.Drawing.Size(285, 29);
            this.txtCartonNo.TabIndex = 23;
            this.txtCartonNo.Text = "N/A";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(35, 74);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(99, 19);
            this.label14.TabIndex = 22;
            this.label14.Text = "Carton No";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(319, 519);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(117, 32);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(186, 519);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(102, 32);
            this.btnOK.TabIndex = 21;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMsg.Location = new System.Drawing.Point(7, 450);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(485, 49);
            this.txtMsg.TabIndex = 20;
            this.txtMsg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtResult.ForeColor = System.Drawing.Color.Red;
            this.txtResult.Location = new System.Drawing.Point(133, 392);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(292, 29);
            this.txtResult.TabIndex = 19;
            this.txtResult.Text = "N/A";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(35, 395);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 19);
            this.label13.TabIndex = 18;
            this.label13.Text = "Result";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(431, 339);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 19);
            this.label12.TabIndex = 17;
            this.label12.Text = "KG";
            // 
            // txtTestValue
            // 
            this.txtTestValue.BackColor = System.Drawing.SystemColors.Control;
            this.txtTestValue.ForeColor = System.Drawing.Color.Red;
            this.txtTestValue.Location = new System.Drawing.Point(138, 336);
            this.txtTestValue.Name = "txtTestValue";
            this.txtTestValue.Size = new System.Drawing.Size(287, 29);
            this.txtTestValue.TabIndex = 16;
            this.txtTestValue.Text = "N/A";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(35, 338);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(109, 19);
            this.label11.TabIndex = 15;
            this.label11.Text = "Test Value";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(431, 268);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 19);
            this.label10.TabIndex = 14;
            this.label10.Text = "KG";
            // 
            // txtMaxWeight
            // 
            this.txtMaxWeight.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaxWeight.ForeColor = System.Drawing.Color.Red;
            this.txtMaxWeight.Location = new System.Drawing.Point(138, 265);
            this.txtMaxWeight.Name = "txtMaxWeight";
            this.txtMaxWeight.ReadOnly = true;
            this.txtMaxWeight.Size = new System.Drawing.Size(287, 29);
            this.txtMaxWeight.TabIndex = 13;
            this.txtMaxWeight.Text = "N/A";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(35, 268);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 19);
            this.label9.TabIndex = 12;
            this.label9.Text = "Max Weight";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(431, 221);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 19);
            this.label8.TabIndex = 11;
            this.label8.Text = "KG";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(303, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 19);
            this.label6.TabIndex = 10;
            // 
            // txtMinWeight
            // 
            this.txtMinWeight.BackColor = System.Drawing.SystemColors.Control;
            this.txtMinWeight.ForeColor = System.Drawing.Color.Red;
            this.txtMinWeight.Location = new System.Drawing.Point(138, 218);
            this.txtMinWeight.Name = "txtMinWeight";
            this.txtMinWeight.ReadOnly = true;
            this.txtMinWeight.Size = new System.Drawing.Size(287, 29);
            this.txtMinWeight.TabIndex = 9;
            this.txtMinWeight.Text = "N/A";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 19);
            this.label5.TabIndex = 8;
            this.label5.Text = "Min Weight";
            // 
            // txtPartNo
            // 
            this.txtPartNo.BackColor = System.Drawing.SystemColors.Control;
            this.txtPartNo.ForeColor = System.Drawing.Color.Red;
            this.txtPartNo.Location = new System.Drawing.Point(139, 118);
            this.txtPartNo.Name = "txtPartNo";
            this.txtPartNo.ReadOnly = true;
            this.txtPartNo.Size = new System.Drawing.Size(286, 29);
            this.txtPartNo.TabIndex = 5;
            this.txtPartNo.Text = "N/A";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "Part No";
            // 
            // txtCOM
            // 
            this.txtCOM.BackColor = System.Drawing.SystemColors.Control;
            this.txtCOM.ForeColor = System.Drawing.Color.Red;
            this.txtCOM.Location = new System.Drawing.Point(138, 28);
            this.txtCOM.Name = "txtCOM";
            this.txtCOM.ReadOnly = true;
            this.txtCOM.Size = new System.Drawing.Size(287, 29);
            this.txtCOM.TabIndex = 1;
            this.txtCOM.Text = "N/A";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "COM";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // fWeightCarton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 560);
            this.Controls.Add(this.panel1);
            this.Name = "fWeightCarton";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Weight Carton";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fWeightCarton_FormClosing);
            this.Load += new System.EventHandler(this.fWeightCarton_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtCartonNo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtTestValue;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMaxWeight;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMinWeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPartNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCOM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStandardWeight;
    }
}