namespace RCProcess
{
    partial class fData
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fData));
            this.panelControl = new System.Windows.Forms.Panel();
            this.editOutCN = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.editInCN = new System.Windows.Forms.TextBox();
            this.LabInCN = new System.Windows.Forms.Label();
            this.editOutName = new System.Windows.Forms.TextBox();
            this.LabOutName = new System.Windows.Forms.Label();
            this.btnOut = new System.Windows.Forms.Button();
            this.editOutDll = new System.Windows.Forms.TextBox();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.editPrcoess = new System.Windows.Forms.TextBox();
            this.editInDll = new System.Windows.Forms.TextBox();
            this.LabType = new System.Windows.Forms.Label();
            this.LabInput = new System.Windows.Forms.Label();
            this.editInName = new System.Windows.Forms.TextBox();
            this.LabInEN = new System.Windows.Forms.Label();
            this.LabOutput = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelControl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            resources.ApplyResources(this.panelControl, "panelControl");
            this.panelControl.BackColor = System.Drawing.Color.Transparent;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelControl.Controls.Add(this.editOutCN);
            this.panelControl.Controls.Add(this.label2);
            this.panelControl.Controls.Add(this.editInCN);
            this.panelControl.Controls.Add(this.LabInCN);
            this.panelControl.Controls.Add(this.editOutName);
            this.panelControl.Controls.Add(this.LabOutName);
            this.panelControl.Controls.Add(this.btnOut);
            this.panelControl.Controls.Add(this.editOutDll);
            this.panelControl.Controls.Add(this.btnIn);
            this.panelControl.Controls.Add(this.btnProcess);
            this.panelControl.Controls.Add(this.editPrcoess);
            this.panelControl.Controls.Add(this.editInDll);
            this.panelControl.Controls.Add(this.LabType);
            this.panelControl.Controls.Add(this.LabInput);
            this.panelControl.Controls.Add(this.editInName);
            this.panelControl.Controls.Add(this.LabInEN);
            this.panelControl.Controls.Add(this.LabOutput);
            this.panelControl.Name = "panelControl";
            // 
            // editOutCN
            // 
            this.editOutCN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editOutCN, "editOutCN");
            this.editOutCN.Name = "editOutCN";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // editInCN
            // 
            this.editInCN.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editInCN, "editInCN");
            this.editInCN.Name = "editInCN";
            // 
            // LabInCN
            // 
            resources.ApplyResources(this.LabInCN, "LabInCN");
            this.LabInCN.BackColor = System.Drawing.Color.Transparent;
            this.LabInCN.Name = "LabInCN";
            // 
            // editOutName
            // 
            this.editOutName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editOutName, "editOutName");
            this.editOutName.Name = "editOutName";
            this.editOutName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editOutName_KeyPress);
            // 
            // LabOutName
            // 
            resources.ApplyResources(this.LabOutName, "LabOutName");
            this.LabOutName.BackColor = System.Drawing.Color.Transparent;
            this.LabOutName.Name = "LabOutName";
            // 
            // btnOut
            // 
            resources.ApplyResources(this.btnOut, "btnOut");
            this.btnOut.Name = "btnOut";
            this.btnOut.UseVisualStyleBackColor = true;
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // editOutDll
            // 
            this.editOutDll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editOutDll, "editOutDll");
            this.editOutDll.Name = "editOutDll";
            this.editOutDll.TextChanged += new System.EventHandler(this.editOutDll_TextChanged);
            // 
            // btnIn
            // 
            resources.ApplyResources(this.btnIn, "btnIn");
            this.btnIn.Name = "btnIn";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnProcess
            // 
            resources.ApplyResources(this.btnProcess, "btnProcess");
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // editPrcoess
            // 
            this.editPrcoess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editPrcoess, "editPrcoess");
            this.editPrcoess.Name = "editPrcoess";
            // 
            // editInDll
            // 
            this.editInDll.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editInDll, "editInDll");
            this.editInDll.Name = "editInDll";
            this.editInDll.TextChanged += new System.EventHandler(this.editInDll_TextChanged);
            this.editInDll.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editInDll_KeyPress);
            // 
            // LabType
            // 
            resources.ApplyResources(this.LabType, "LabType");
            this.LabType.BackColor = System.Drawing.Color.Transparent;
            this.LabType.Name = "LabType";
            // 
            // LabInput
            // 
            resources.ApplyResources(this.LabInput, "LabInput");
            this.LabInput.BackColor = System.Drawing.Color.Transparent;
            this.LabInput.Name = "LabInput";
            // 
            // editInName
            // 
            this.editInName.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.editInName, "editInName");
            this.editInName.Name = "editInName";
            // 
            // LabInEN
            // 
            resources.ApplyResources(this.LabInEN, "LabInEN");
            this.LabInEN.BackColor = System.Drawing.Color.Transparent;
            this.LabInEN.Name = "LabInEN";
            // 
            // LabOutput
            // 
            resources.ApplyResources(this.LabOutput, "LabOutput");
            this.LabOutput.BackColor = System.Drawing.Color.Transparent;
            this.LabOutput.Name = "LabOutput";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // fData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panel2);
            this.Name = "fData";
            this.Load += new System.EventHandler(this.fData_Load);
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox editInName;
        private System.Windows.Forms.Label LabInEN;
        private System.Windows.Forms.Label LabOutput;
        private System.Windows.Forms.TextBox editInDll;
        private System.Windows.Forms.Label LabInput;
        private System.Windows.Forms.Label LabType;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox editPrcoess;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.TextBox editOutDll;
        private System.Windows.Forms.TextBox editOutName;
        private System.Windows.Forms.Label LabOutName;
        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.TextBox editOutCN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox editInCN;
        private System.Windows.Forms.Label LabInCN;
    }
}