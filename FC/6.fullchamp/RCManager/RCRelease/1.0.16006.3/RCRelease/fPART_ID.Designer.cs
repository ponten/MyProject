namespace RC_Release
{
    partial class fPART_ID
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
            this.fPART_ID_dgv = new System.Windows.Forms.DataGridView();
            this.SELECT = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PART_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PART_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PART_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fPART_ID_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // fPART_ID_dgv
            // 
            this.fPART_ID_dgv.AllowUserToAddRows = false;
            this.fPART_ID_dgv.AllowUserToDeleteRows = false;
            this.fPART_ID_dgv.AllowUserToResizeColumns = false;
            this.fPART_ID_dgv.AllowUserToResizeRows = false;
            this.fPART_ID_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.fPART_ID_dgv.BackgroundColor = System.Drawing.SystemColors.Control;
            this.fPART_ID_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fPART_ID_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SELECT,
            this.PART_ID,
            this.PART_NO,
            this.PART_TYPE});
            this.fPART_ID_dgv.Location = new System.Drawing.Point(11, 13);
            this.fPART_ID_dgv.Name = "fPART_ID_dgv";
            this.fPART_ID_dgv.RowTemplate.Height = 23;
            this.fPART_ID_dgv.Size = new System.Drawing.Size(459, 397);
            this.fPART_ID_dgv.TabIndex = 0;
            // 
            // SELECT
            // 
            this.SELECT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SELECT.HeaderText = "SELECT";
            this.SELECT.Name = "SELECT";
            this.SELECT.ReadOnly = true;
            this.SELECT.Width = 60;
            // 
            // PART_ID
            // 
            this.PART_ID.HeaderText = "PART_ID";
            this.PART_ID.Name = "PART_ID";
            this.PART_ID.ReadOnly = true;
            // 
            // PART_NO
            // 
            this.PART_NO.HeaderText = "PART_NO";
            this.PART_NO.Name = "PART_NO";
            this.PART_NO.ReadOnly = true;
            // 
            // PART_TYPE
            // 
            this.PART_TYPE.HeaderText = "PART_TYPE";
            this.PART_TYPE.Name = "PART_TYPE";
            this.PART_TYPE.ReadOnly = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(283, 422);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(384, 422);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 28);
            this.button2.TabIndex = 2;
            this.button2.Text = "CANCEL";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // fPART_ID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 460);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fPART_ID_dgv);
            this.Name = "fPART_ID";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fPART_ID";
            this.Load += new System.EventHandler(this.fPART_ID_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fPART_ID_dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView fPART_ID_dgv;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SELECT;
        private System.Windows.Forms.DataGridViewTextBoxColumn PART_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PART_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PART_TYPE;
    }
}