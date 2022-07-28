namespace SajetFilter
{
    partial class FFilter
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
            this.TlpForm = new System.Windows.Forms.TableLayoutPanel();
            this.LbSearch = new System.Windows.Forms.Label();
            this.CbField = new System.Windows.Forms.ComboBox();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.DgvData = new System.Windows.Forms.DataGridView();
            this.TbField = new System.Windows.Forms.TextBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.TlpMulti = new System.Windows.Forms.TableLayoutPanel();
            this.BtnAll = new System.Windows.Forms.Button();
            this.BtnNone = new System.Windows.Forms.Button();
            this.TlpForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvData)).BeginInit();
            this.TlpMulti.SuspendLayout();
            this.SuspendLayout();
            // 
            // TlpForm
            // 
            this.TlpForm.ColumnCount = 5;
            this.TlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpForm.Controls.Add(this.LbSearch, 0, 0);
            this.TlpForm.Controls.Add(this.CbField, 1, 0);
            this.TlpForm.Controls.Add(this.BtnCancel, 4, 2);
            this.TlpForm.Controls.Add(this.DgvData, 0, 1);
            this.TlpForm.Controls.Add(this.TbField, 2, 0);
            this.TlpForm.Controls.Add(this.BtnSearch, 3, 0);
            this.TlpForm.Controls.Add(this.TlpMulti, 0, 2);
            this.TlpForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpForm.Location = new System.Drawing.Point(0, 0);
            this.TlpForm.Name = "TlpForm";
            this.TlpForm.RowCount = 3;
            this.TlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.TlpForm.Size = new System.Drawing.Size(504, 361);
            this.TlpForm.TabIndex = 0;
            // 
            // LbSearch
            // 
            this.LbSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LbSearch.AutoSize = true;
            this.LbSearch.Location = new System.Drawing.Point(3, 8);
            this.LbSearch.Margin = new System.Windows.Forms.Padding(3);
            this.LbSearch.Name = "LbSearch";
            this.LbSearch.Size = new System.Drawing.Size(50, 16);
            this.LbSearch.TabIndex = 0;
            this.LbSearch.Text = "Search";
            // 
            // CbField
            // 
            this.CbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbField.FormattingEnabled = true;
            this.CbField.Location = new System.Drawing.Point(60, 4);
            this.CbField.Margin = new System.Windows.Forms.Padding(4);
            this.CbField.Name = "CbField";
            this.CbField.Size = new System.Drawing.Size(150, 24);
            this.CbField.TabIndex = 1;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnCancel.Location = new System.Drawing.Point(426, 330);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 27);
            this.BtnCancel.TabIndex = 4;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // DgvData
            // 
            this.DgvData.AllowUserToAddRows = false;
            this.DgvData.AllowUserToDeleteRows = false;
            this.DgvData.AllowUserToResizeRows = false;
            this.DgvData.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TlpForm.SetColumnSpan(this.DgvData, 5);
            this.DgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvData.Location = new System.Drawing.Point(3, 36);
            this.DgvData.MultiSelect = false;
            this.DgvData.Name = "DgvData";
            this.DgvData.ReadOnly = true;
            this.DgvData.RowTemplate.Height = 24;
            this.DgvData.Size = new System.Drawing.Size(498, 287);
            this.DgvData.TabIndex = 5;
            // 
            // TbField
            // 
            this.TbField.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TbField.Location = new System.Drawing.Point(217, 3);
            this.TbField.Name = "TbField";
            this.TbField.Size = new System.Drawing.Size(150, 27);
            this.TbField.TabIndex = 6;
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnOk.Location = new System.Drawing.Point(345, 4);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 27);
            this.BtnOk.TabIndex = 3;
            this.BtnOk.Text = "Confirm";
            this.BtnOk.UseVisualStyleBackColor = true;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TlpForm.SetColumnSpan(this.BtnSearch, 2);
            this.BtnSearch.Location = new System.Drawing.Point(373, 3);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 27);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "Query";
            this.BtnSearch.UseVisualStyleBackColor = true;
            // 
            // TlpMulti
            // 
            this.TlpMulti.ColumnCount = 3;
            this.TlpForm.SetColumnSpan(this.TlpMulti, 4);
            this.TlpMulti.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TlpMulti.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpMulti.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpMulti.Controls.Add(this.BtnAll, 0, 0);
            this.TlpMulti.Controls.Add(this.BtnNone, 1, 0);
            this.TlpMulti.Controls.Add(this.BtnOk, 2, 0);
            this.TlpMulti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpMulti.Location = new System.Drawing.Point(0, 326);
            this.TlpMulti.Margin = new System.Windows.Forms.Padding(0);
            this.TlpMulti.Name = "TlpMulti";
            this.TlpMulti.RowCount = 1;
            this.TlpMulti.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TlpMulti.Size = new System.Drawing.Size(423, 35);
            this.TlpMulti.TabIndex = 7;
            // 
            // BtnAll
            // 
            this.BtnAll.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BtnAll.Location = new System.Drawing.Point(3, 4);
            this.BtnAll.Name = "BtnAll";
            this.BtnAll.Size = new System.Drawing.Size(100, 27);
            this.BtnAll.TabIndex = 0;
            this.BtnAll.Text = "Select All";
            this.BtnAll.UseVisualStyleBackColor = true;
            // 
            // BtnNone
            // 
            this.BtnNone.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BtnNone.Location = new System.Drawing.Point(109, 4);
            this.BtnNone.Name = "BtnNone";
            this.BtnNone.Size = new System.Drawing.Size(100, 27);
            this.BtnNone.TabIndex = 1;
            this.BtnNone.Text = "Select None";
            this.BtnNone.UseVisualStyleBackColor = true;
            // 
            // FFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 361);
            this.Controls.Add(this.TlpForm);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Query window";
            this.TlpForm.ResumeLayout(false);
            this.TlpForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvData)).EndInit();
            this.TlpMulti.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TlpForm;
        private System.Windows.Forms.Label LbSearch;
        private System.Windows.Forms.ComboBox CbField;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.DataGridView DgvData;
        private System.Windows.Forms.TextBox TbField;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.TableLayoutPanel TlpMulti;
        private System.Windows.Forms.Button BtnAll;
        private System.Windows.Forms.Button BtnNone;
    }
}