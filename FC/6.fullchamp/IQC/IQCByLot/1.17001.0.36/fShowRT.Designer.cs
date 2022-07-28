namespace IQCbyLot
{
    partial class fShowRT
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lablLotNo = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btCancel = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgRT = new System.Windows.Forms.DataGridView();
            this.REJECT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RECEIVE_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RT_SEQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RT_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRT)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lablLotNo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 63);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 29;
            this.label1.Text = "Lot No";
            // 
            // lablLotNo
            // 
            this.lablLotNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablLotNo.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lablLotNo.ForeColor = System.Drawing.Color.Maroon;
            this.lablLotNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablLotNo.Location = new System.Drawing.Point(97, 16);
            this.lablLotNo.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lablLotNo.Name = "lablLotNo";
            this.lablLotNo.Size = new System.Drawing.Size(201, 25);
            this.lablLotNo.TabIndex = 30;
            this.lablLotNo.Text = "N/A";
            this.lablLotNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 270);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(514, 39);
            this.panel2.TabIndex = 66;
            // 
            // btCancel
            // 
            this.btCancel.Font = new System.Drawing.Font("新細明體", 12F);
            this.btCancel.ForeColor = System.Drawing.Color.Black;
            this.btCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btCancel.Location = new System.Drawing.Point(427, 7);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 25);
            this.btCancel.TabIndex = 62;
            this.btCancel.Tag = "0";
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgRT);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 63);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(514, 207);
            this.panel3.TabIndex = 67;
            // 
            // dgRT
            // 
            this.dgRT.AllowUserToAddRows = false;
            this.dgRT.AllowUserToDeleteRows = false;
            this.dgRT.BackgroundColor = System.Drawing.Color.White;
            this.dgRT.ColumnHeadersHeight = 25;
            this.dgRT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RT_NO,
            this.PO,
            this.QTY,
            this.RT_SEQ,
            this.RECEIVE_QTY,
            this.REJECT_QTY});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgRT.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgRT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgRT.EnableHeadersVisualStyles = false;
            this.dgRT.Location = new System.Drawing.Point(0, 0);
            this.dgRT.Margin = new System.Windows.Forms.Padding(4);
            this.dgRT.MultiSelect = false;
            this.dgRT.Name = "dgRT";
            this.dgRT.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgRT.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgRT.RowHeadersVisible = false;
            this.dgRT.RowTemplate.Height = 24;
            this.dgRT.RowTemplate.ReadOnly = true;
            this.dgRT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgRT.Size = new System.Drawing.Size(514, 207);
            this.dgRT.TabIndex = 66;
            // 
            // REJECT_QTY
            // 
            this.REJECT_QTY.HeaderText = "Reject Qty";
            this.REJECT_QTY.Name = "REJECT_QTY";
            this.REJECT_QTY.ReadOnly = true;
            // 
            // RECEIVE_QTY
            // 
            this.RECEIVE_QTY.HeaderText = "Receive Qty";
            this.RECEIVE_QTY.Name = "RECEIVE_QTY";
            this.RECEIVE_QTY.ReadOnly = true;
            // 
            // RT_SEQ
            // 
            this.RT_SEQ.HeaderText = "RT_SEQ";
            this.RT_SEQ.Name = "RT_SEQ";
            this.RT_SEQ.ReadOnly = true;
            this.RT_SEQ.Visible = false;
            // 
            // QTY
            // 
            this.QTY.HeaderText = "QTY";
            this.QTY.Name = "QTY";
            this.QTY.ReadOnly = true;
            // 
            // PO
            // 
            this.PO.HeaderText = "PO Number";
            this.PO.Name = "PO";
            this.PO.ReadOnly = true;
            // 
            // RT_NO
            // 
            this.RT_NO.HeaderText = "RT No";
            this.RT_NO.Name = "RT_NO";
            this.RT_NO.ReadOnly = true;
            // 
            // fShowRT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 309);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("新細明體", 12F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fShowRT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RT No";
            this.Load += new System.EventHandler(this.fShowRT_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgRT)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lablLotNo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgRT;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn RT_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PO;
        private System.Windows.Forms.DataGridViewTextBoxColumn QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn RT_SEQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn RECEIVE_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn REJECT_QTY;
    }
}