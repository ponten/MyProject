namespace BCRuleSetDll
{
    partial class fRuleMainTain
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.combRule = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.editRuleDesc = new System.Windows.Forms.TextBox();
            this.editRuleName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(382, 44);
            this.panel1.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(214, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(295, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // combRule
            // 
            this.combRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combRule.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.combRule.FormattingEnabled = true;
            this.combRule.Location = new System.Drawing.Point(110, 78);
            this.combRule.Name = "combRule";
            this.combRule.Size = new System.Drawing.Size(240, 23);
            this.combRule.TabIndex = 15;
            this.combRule.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "Copy From";
            this.label3.Visible = false;
            // 
            // editRuleDesc
            // 
            this.editRuleDesc.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.editRuleDesc.Location = new System.Drawing.Point(110, 48);
            this.editRuleDesc.Name = "editRuleDesc";
            this.editRuleDesc.Size = new System.Drawing.Size(239, 25);
            this.editRuleDesc.TabIndex = 14;
            this.editRuleDesc.Visible = false;
            // 
            // editRuleName
            // 
            this.editRuleName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.editRuleName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editRuleName.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.editRuleName.Location = new System.Drawing.Point(110, 18);
            this.editRuleName.Name = "editRuleName";
            this.editRuleName.Size = new System.Drawing.Size(239, 25);
            this.editRuleName.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Description";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Rule Name";
            // 
            // fRuleMainTain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 95);
            this.Controls.Add(this.combRule);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.editRuleDesc);
            this.Controls.Add(this.editRuleName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "fRuleMainTain";
            this.Text = "fRuleMainTain";
            this.Load += new System.EventHandler(this.fRuleMainTain_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ComboBox combRule;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox editRuleDesc;
        public System.Windows.Forms.TextBox editRuleName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}