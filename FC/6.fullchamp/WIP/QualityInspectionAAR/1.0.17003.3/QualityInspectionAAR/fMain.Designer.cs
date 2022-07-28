namespace QualityInspectionAAR
{
    partial class fMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.TlpForm = new System.Windows.Forms.TableLayoutPanel();
            this.TlpTop = new System.Windows.Forms.TableLayoutPanel();
            this.Lb_1 = new System.Windows.Forms.Label();
            this.Lb_2 = new System.Windows.Forms.Label();
            this.BtnClear = new System.Windows.Forms.Button();
            this.TbRC = new System.Windows.Forms.TextBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.CbProcess = new System.Windows.Forms.ComboBox();
            this.CbStage = new System.Windows.Forms.ComboBox();
            this.TbWO = new System.Windows.Forms.TextBox();
            this.Lb_3 = new System.Windows.Forms.Label();
            this.Lb_4 = new System.Windows.Forms.Label();
            this.Sc_1 = new System.Windows.Forms.SplitContainer();
            this.DgvRC = new System.Windows.Forms.DataGridView();
            this.DgvData = new System.Windows.Forms.DataGridView();
            this.TlpForm.SuspendLayout();
            this.TlpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Sc_1)).BeginInit();
            this.Sc_1.Panel1.SuspendLayout();
            this.Sc_1.Panel2.SuspendLayout();
            this.Sc_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvRC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // TlpForm
            // 
            resources.ApplyResources(this.TlpForm, "TlpForm");
            this.TlpForm.Controls.Add(this.TlpTop, 0, 0);
            this.TlpForm.Controls.Add(this.Sc_1, 0, 1);
            this.TlpForm.Name = "TlpForm";
            // 
            // TlpTop
            // 
            resources.ApplyResources(this.TlpTop, "TlpTop");
            this.TlpTop.Controls.Add(this.Lb_1, 0, 0);
            this.TlpTop.Controls.Add(this.Lb_2, 2, 0);
            this.TlpTop.Controls.Add(this.BtnClear, 5, 1);
            this.TlpTop.Controls.Add(this.TbRC, 3, 1);
            this.TlpTop.Controls.Add(this.BtnSearch, 4, 1);
            this.TlpTop.Controls.Add(this.CbProcess, 3, 0);
            this.TlpTop.Controls.Add(this.CbStage, 1, 0);
            this.TlpTop.Controls.Add(this.TbWO, 1, 1);
            this.TlpTop.Controls.Add(this.Lb_3, 0, 1);
            this.TlpTop.Controls.Add(this.Lb_4, 2, 1);
            this.TlpTop.Name = "TlpTop";
            // 
            // Lb_1
            // 
            resources.ApplyResources(this.Lb_1, "Lb_1");
            this.Lb_1.Name = "Lb_1";
            // 
            // Lb_2
            // 
            resources.ApplyResources(this.Lb_2, "Lb_2");
            this.Lb_2.Name = "Lb_2";
            // 
            // BtnClear
            // 
            resources.ApplyResources(this.BtnClear, "BtnClear");
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.UseVisualStyleBackColor = true;
            // 
            // TbRC
            // 
            resources.ApplyResources(this.TbRC, "TbRC");
            this.TbRC.Name = "TbRC";
            // 
            // BtnSearch
            // 
            resources.ApplyResources(this.BtnSearch, "BtnSearch");
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.UseVisualStyleBackColor = true;
            // 
            // CbProcess
            // 
            resources.ApplyResources(this.CbProcess, "CbProcess");
            this.CbProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbProcess.FormattingEnabled = true;
            this.CbProcess.Name = "CbProcess";
            // 
            // CbStage
            // 
            resources.ApplyResources(this.CbStage, "CbStage");
            this.CbStage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbStage.FormattingEnabled = true;
            this.CbStage.Name = "CbStage";
            // 
            // TbWO
            // 
            resources.ApplyResources(this.TbWO, "TbWO");
            this.TbWO.Name = "TbWO";
            // 
            // Lb_3
            // 
            resources.ApplyResources(this.Lb_3, "Lb_3");
            this.Lb_3.Name = "Lb_3";
            // 
            // Lb_4
            // 
            resources.ApplyResources(this.Lb_4, "Lb_4");
            this.Lb_4.Name = "Lb_4";
            // 
            // Sc_1
            // 
            resources.ApplyResources(this.Sc_1, "Sc_1");
            this.Sc_1.Name = "Sc_1";
            // 
            // Sc_1.Panel1
            // 
            this.Sc_1.Panel1.Controls.Add(this.DgvRC);
            // 
            // Sc_1.Panel2
            // 
            this.Sc_1.Panel2.Controls.Add(this.DgvData);
            // 
            // DgvRC
            // 
            this.DgvRC.AllowUserToAddRows = false;
            this.DgvRC.AllowUserToDeleteRows = false;
            this.DgvRC.AllowUserToResizeRows = false;
            this.DgvRC.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DgvRC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.DgvRC, "DgvRC");
            this.DgvRC.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvRC.MultiSelect = false;
            this.DgvRC.Name = "DgvRC";
            this.DgvRC.ReadOnly = true;
            this.DgvRC.RowTemplate.Height = 24;
            // 
            // DgvData
            // 
            this.DgvData.AllowUserToAddRows = false;
            this.DgvData.AllowUserToDeleteRows = false;
            this.DgvData.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.DgvData, "DgvData");
            this.DgvData.MultiSelect = false;
            this.DgvData.Name = "DgvData";
            this.DgvData.ReadOnly = true;
            this.DgvData.RowTemplate.Height = 24;
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TlpForm);
            this.Name = "fMain";
            this.TlpForm.ResumeLayout(false);
            this.TlpTop.ResumeLayout(false);
            this.TlpTop.PerformLayout();
            this.Sc_1.Panel1.ResumeLayout(false);
            this.Sc_1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Sc_1)).EndInit();
            this.Sc_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvRC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TlpForm;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Label Lb_1;
        private System.Windows.Forms.ComboBox CbStage;
        private System.Windows.Forms.Label Lb_2;
        private System.Windows.Forms.ComboBox CbProcess;
        private System.Windows.Forms.Label Lb_3;
        private System.Windows.Forms.TextBox TbWO;
        private System.Windows.Forms.Label Lb_4;
        private System.Windows.Forms.TextBox TbRC;
        private System.Windows.Forms.DataGridView DgvRC;
        private System.Windows.Forms.TableLayoutPanel TlpTop;
        private System.Windows.Forms.DataGridView DgvData;
        private System.Windows.Forms.SplitContainer Sc_1;
    }
}