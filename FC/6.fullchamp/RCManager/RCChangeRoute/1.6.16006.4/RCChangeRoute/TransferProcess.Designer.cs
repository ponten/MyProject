namespace RC_ChangeRoute
{
    partial class TransferProcess
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
            this.FLP_ButtonZone = new System.Windows.Forms.FlowLayoutPanel();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_OK = new System.Windows.Forms.Button();
            this.DGV_Process = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_Process = new System.Windows.Forms.TextBox();
            this.TB_Exit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_Return = new System.Windows.Forms.Button();
            this.NID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROCESS_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NEXT_PROCESS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Process)).BeginInit();
            this.SuspendLayout();
            // 
            // FLP_ButtonZone
            // 
            this.FLP_ButtonZone.AutoScroll = true;
            this.FLP_ButtonZone.BackColor = System.Drawing.SystemColors.Control;
            this.FLP_ButtonZone.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FLP_ButtonZone.Location = new System.Drawing.Point(10, 40);
            this.FLP_ButtonZone.Name = "FLP_ButtonZone";
            this.FLP_ButtonZone.Size = new System.Drawing.Size(294, 200);
            this.FLP_ButtonZone.TabIndex = 0;
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Location = new System.Drawing.Point(160, 250);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(144, 59);
            this.Btn_Cancel.TabIndex = 1;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // Btn_OK
            // 
            this.Btn_OK.Location = new System.Drawing.Point(10, 250);
            this.Btn_OK.Name = "Btn_OK";
            this.Btn_OK.Size = new System.Drawing.Size(144, 59);
            this.Btn_OK.TabIndex = 2;
            this.Btn_OK.Text = "OK";
            this.Btn_OK.UseVisualStyleBackColor = true;
            this.Btn_OK.Visible = false;
            this.Btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // DGV_Process
            // 
            this.DGV_Process.AllowUserToAddRows = false;
            this.DGV_Process.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.DGV_Process.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV_Process.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DGV_Process.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGV_Process.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Process.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NID,
            this.PROCESS_ID,
            this.NEXT_PROCESS,
            this.STATUS});
            this.DGV_Process.Location = new System.Drawing.Point(360, 40);
            this.DGV_Process.Name = "DGV_Process";
            this.DGV_Process.RowTemplate.Height = 24;
            this.DGV_Process.Size = new System.Drawing.Size(282, 269);
            this.DGV_Process.TabIndex = 4;
            this.DGV_Process.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_Process_CellMouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Process Name ";
            // 
            // TB_Process
            // 
            this.TB_Process.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.TB_Process.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TB_Process.Location = new System.Drawing.Point(120, 6);
            this.TB_Process.Name = "TB_Process";
            this.TB_Process.Size = new System.Drawing.Size(174, 27);
            this.TB_Process.TabIndex = 13;
            // 
            // TB_Exit
            // 
            this.TB_Exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.TB_Exit.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TB_Exit.Location = new System.Drawing.Point(470, 6);
            this.TB_Exit.Name = "TB_Exit";
            this.TB_Exit.Size = new System.Drawing.Size(174, 27);
            this.TB_Exit.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(360, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "Exit";
            // 
            // Btn_Return
            // 
            this.Btn_Return.Enabled = false;
            this.Btn_Return.Location = new System.Drawing.Point(10, 250);
            this.Btn_Return.Name = "Btn_Return";
            this.Btn_Return.Size = new System.Drawing.Size(144, 59);
            this.Btn_Return.TabIndex = 16;
            this.Btn_Return.Text = "OK";
            this.Btn_Return.UseVisualStyleBackColor = true;
            this.Btn_Return.Click += new System.EventHandler(this.Btn_Return_Click);
            // 
            // NID
            // 
            this.NID.HeaderText = "NID";
            this.NID.Name = "NID";
            this.NID.Visible = false;
            this.NID.Width = 50;
            // 
            // PROCESS_ID
            // 
            this.PROCESS_ID.HeaderText = "ID";
            this.PROCESS_ID.Name = "PROCESS_ID";
            this.PROCESS_ID.ReadOnly = true;
            this.PROCESS_ID.Visible = false;
            this.PROCESS_ID.Width = 42;
            // 
            // NEXT_PROCESS
            // 
            this.NEXT_PROCESS.HeaderText = "Next Process";
            this.NEXT_PROCESS.Name = "NEXT_PROCESS";
            this.NEXT_PROCESS.ReadOnly = true;
            this.NEXT_PROCESS.Width = 89;
            // 
            // STATUS
            // 
            this.STATUS.HeaderText = "Status";
            this.STATUS.Name = "STATUS";
            this.STATUS.ReadOnly = true;
            this.STATUS.Width = 57;
            // 
            // TransferProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 322);
            this.ControlBox = false;
            this.Controls.Add(this.Btn_Return);
            this.Controls.Add(this.Btn_OK);
            this.Controls.Add(this.TB_Exit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TB_Process);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DGV_Process);
            this.Controls.Add(this.Btn_Cancel);
            this.Controls.Add(this.FLP_ButtonZone);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "TransferProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transfer Process";
            this.Load += new System.EventHandler(this.TransferProcess_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Process)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel FLP_ButtonZone;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_OK;
        private System.Windows.Forms.DataGridView DGV_Process;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_Process;
        private System.Windows.Forms.TextBox TB_Exit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Btn_Return;
        private System.Windows.Forms.DataGridViewTextBoxColumn NID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROCESS_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NEXT_PROCESS;
        private System.Windows.Forms.DataGridViewTextBoxColumn STATUS;
    }
}