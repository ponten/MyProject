namespace CPartQCItem
{
    partial class TFDetail
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
            this.btnOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.LabDesc = new System.Windows.Forms.Label();
            this.LabName = new System.Windows.Forms.Label();
            this.editName = new System.Windows.Forms.TextBox();
            this.editDesc = new System.Windows.Forms.TextBox();
            this.LabCode = new System.Windows.Forms.Label();
            this.editCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LabTypeName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.combHasValue = new System.Windows.Forms.ComboBox();
            this.gbValueType = new System.Windows.Forms.GroupBox();
            this.rbtnCharacter = new System.Windows.Forms.RadioButton();
            this.rbtnNumber = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.editDesc2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.editInspQty = new System.Windows.Forms.TextBox();
            this.panelControl = new System.Windows.Forms.Panel();
            this.maxItemCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.gbValueType.SuspendLayout();
            this.panelControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(227, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 297);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(392, 33);
            this.panel2.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(308, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // LabDesc
            // 
            this.LabDesc.AutoSize = true;
            this.LabDesc.BackColor = System.Drawing.Color.Transparent;
            this.LabDesc.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.LabDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabDesc.Location = new System.Drawing.Point(42, 120);
            this.LabDesc.Name = "LabDesc";
            this.LabDesc.Size = new System.Drawing.Size(62, 13);
            this.LabDesc.TabIndex = 4;
            this.LabDesc.Text = "Description";
            // 
            // LabName
            // 
            this.LabName.AutoSize = true;
            this.LabName.BackColor = System.Drawing.Color.Transparent;
            this.LabName.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.LabName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabName.Location = new System.Drawing.Point(42, 94);
            this.LabName.Name = "LabName";
            this.LabName.Size = new System.Drawing.Size(61, 13);
            this.LabName.TabIndex = 1;
            this.LabName.Text = "Item Name";
            // 
            // editName
            // 
            this.editName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.editName.Location = new System.Drawing.Point(177, 91);
            this.editName.Name = "editName";
            this.editName.Size = new System.Drawing.Size(166, 22);
            this.editName.TabIndex = 1;
            // 
            // editDesc
            // 
            this.editDesc.Location = new System.Drawing.Point(177, 117);
            this.editDesc.Name = "editDesc";
            this.editDesc.Size = new System.Drawing.Size(166, 22);
            this.editDesc.TabIndex = 2;
            // 
            // LabCode
            // 
            this.LabCode.AutoSize = true;
            this.LabCode.BackColor = System.Drawing.Color.Transparent;
            this.LabCode.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.LabCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabCode.Location = new System.Drawing.Point(42, 42);
            this.LabCode.Name = "LabCode";
            this.LabCode.Size = new System.Drawing.Size(57, 13);
            this.LabCode.TabIndex = 5;
            this.LabCode.Text = "Item Code";
            // 
            // editCode
            // 
            this.editCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.editCode.Location = new System.Drawing.Point(177, 39);
            this.editCode.Name = "editCode";
            this.editCode.Size = new System.Drawing.Size(166, 22);
            this.editCode.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(42, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Item Type Name";
            // 
            // LabTypeName
            // 
            this.LabTypeName.AutoSize = true;
            this.LabTypeName.BackColor = System.Drawing.Color.Transparent;
            this.LabTypeName.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.LabTypeName.ForeColor = System.Drawing.Color.Maroon;
            this.LabTypeName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabTypeName.Location = new System.Drawing.Point(174, 18);
            this.LabTypeName.Name = "LabTypeName";
            this.LabTypeName.Size = new System.Drawing.Size(28, 13);
            this.LabTypeName.TabIndex = 10;
            this.LabTypeName.Text = "N/A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(42, 199);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Has Value";
            // 
            // combHasValue
            // 
            this.combHasValue.BackColor = System.Drawing.Color.White;
            this.combHasValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combHasValue.FormattingEnabled = true;
            this.combHasValue.Items.AddRange(new object[] {
            "Y",
            "N"});
            this.combHasValue.Location = new System.Drawing.Point(177, 196);
            this.combHasValue.Name = "combHasValue";
            this.combHasValue.Size = new System.Drawing.Size(117, 20);
            this.combHasValue.TabIndex = 4;
            this.combHasValue.SelectedIndexChanged += new System.EventHandler(this.combHasValue_SelectedIndexChanged);
            // 
            // gbValueType
            // 
            this.gbValueType.Controls.Add(this.rbtnCharacter);
            this.gbValueType.Controls.Add(this.rbtnNumber);
            this.gbValueType.Location = new System.Drawing.Point(49, 224);
            this.gbValueType.Name = "gbValueType";
            this.gbValueType.Size = new System.Drawing.Size(224, 46);
            this.gbValueType.TabIndex = 5;
            this.gbValueType.TabStop = false;
            this.gbValueType.Text = "Value Type";
            // 
            // rbtnCharacter
            // 
            this.rbtnCharacter.AutoSize = true;
            this.rbtnCharacter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbtnCharacter.Location = new System.Drawing.Point(107, 21);
            this.rbtnCharacter.Name = "rbtnCharacter";
            this.rbtnCharacter.Size = new System.Drawing.Size(68, 16);
            this.rbtnCharacter.TabIndex = 1;
            this.rbtnCharacter.TabStop = true;
            this.rbtnCharacter.Text = "Character";
            this.rbtnCharacter.UseVisualStyleBackColor = true;
            // 
            // rbtnNumber
            // 
            this.rbtnNumber.AutoSize = true;
            this.rbtnNumber.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbtnNumber.Location = new System.Drawing.Point(6, 21);
            this.rbtnNumber.Name = "rbtnNumber";
            this.rbtnNumber.Size = new System.Drawing.Size(61, 16);
            this.rbtnNumber.TabIndex = 0;
            this.rbtnNumber.TabStop = true;
            this.rbtnNumber.Text = "Number";
            this.rbtnNumber.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(42, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Description2";
            // 
            // editDesc2
            // 
            this.editDesc2.Location = new System.Drawing.Point(177, 143);
            this.editDesc2.Name = "editDesc2";
            this.editDesc2.Size = new System.Drawing.Size(166, 22);
            this.editDesc2.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(42, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Min Inspect Qty";
            // 
            // editInspQty
            // 
            this.editInspQty.Location = new System.Drawing.Point(177, 169);
            this.editInspQty.Name = "editInspQty";
            this.editInspQty.Size = new System.Drawing.Size(166, 22);
            this.editInspQty.TabIndex = 17;
            // 
            // panelControl
            // 
            this.panelControl.AutoScroll = true;
            this.panelControl.BackColor = System.Drawing.Color.Transparent;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelControl.Controls.Add(this.maxItemCode);
            this.panelControl.Controls.Add(this.label5);
            this.panelControl.Controls.Add(this.editInspQty);
            this.panelControl.Controls.Add(this.label4);
            this.panelControl.Controls.Add(this.editDesc2);
            this.panelControl.Controls.Add(this.label3);
            this.panelControl.Controls.Add(this.gbValueType);
            this.panelControl.Controls.Add(this.combHasValue);
            this.panelControl.Controls.Add(this.label2);
            this.panelControl.Controls.Add(this.LabTypeName);
            this.panelControl.Controls.Add(this.label1);
            this.panelControl.Controls.Add(this.editCode);
            this.panelControl.Controls.Add(this.LabCode);
            this.panelControl.Controls.Add(this.editDesc);
            this.panelControl.Controls.Add(this.editName);
            this.panelControl.Controls.Add(this.LabName);
            this.panelControl.Controls.Add(this.LabDesc);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl.Location = new System.Drawing.Point(0, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(392, 330);
            this.panelControl.TabIndex = 2;
            // 
            // maxItemCode
            // 
            this.maxItemCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.maxItemCode.Location = new System.Drawing.Point(177, 65);
            this.maxItemCode.Name = "maxItemCode";
            this.maxItemCode.ReadOnly = true;
            this.maxItemCode.Size = new System.Drawing.Size(166, 22);
            this.maxItemCode.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(42, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Item Code (Max)";
            // 
            // TFDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 330);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelControl);
            this.Name = "TFDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TFDetail";
            this.Load += new System.EventHandler(this.TFDetail_Load);
            this.panel2.ResumeLayout(false);
            this.gbValueType.ResumeLayout(false);
            this.gbValueType.PerformLayout();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label LabDesc;
        private System.Windows.Forms.Label LabName;
        private System.Windows.Forms.TextBox editName;
        private System.Windows.Forms.TextBox editDesc;
        private System.Windows.Forms.Label LabCode;
        private System.Windows.Forms.TextBox editCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabTypeName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox combHasValue;
        private System.Windows.Forms.GroupBox gbValueType;
        private System.Windows.Forms.RadioButton rbtnCharacter;
        private System.Windows.Forms.RadioButton rbtnNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editDesc2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox editInspQty;
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.TextBox maxItemCode;
        private System.Windows.Forms.Label label5;
    }
}