namespace BCLabelDll
{
    partial class fList
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
            this.LVList = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // LVList
            // 
            this.LVList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LVList.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LVList.FullRowSelect = true;
            this.LVList.GridLines = true;
            this.LVList.Location = new System.Drawing.Point(0, 0);
            this.LVList.Name = "LVList";
            this.LVList.Size = new System.Drawing.Size(859, 433);
            this.LVList.TabIndex = 1;
            this.LVList.UseCompatibleStateImageBehavior = false;
            this.LVList.View = System.Windows.Forms.View.Details;
            // 
            // fList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 433);
            this.Controls.Add(this.LVList);
            this.Name = "fList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "List";
            this.Load += new System.EventHandler(this.fList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView LVList;
    }
}