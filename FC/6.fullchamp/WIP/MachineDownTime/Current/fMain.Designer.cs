namespace MachineDownTime
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Tlp_Form = new System.Windows.Forms.TableLayoutPanel();
            this.Tlp_Grid = new System.Windows.Forms.TableLayoutPanel();
            this.Gb_RCInfo = new System.Windows.Forms.GroupBox();
            this.Tlp_RCInfo = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Tb_WorkOrder = new System.Windows.Forms.TextBox();
            this.Tb_FormerNo = new System.Windows.Forms.TextBox();
            this.Tb_Spec = new System.Windows.Forms.TextBox();
            this.Tb_PartNo = new System.Windows.Forms.TextBox();
            this.Tb_Qty = new System.Windows.Forms.TextBox();
            this.Tb_Good = new System.Windows.Forms.TextBox();
            this.Tb_Scrap = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Tb_Runcard = new System.Windows.Forms.TextBox();
            this.Gb_Machine_InUse = new System.Windows.Forms.GroupBox();
            this.Dgv_Machine_InUse = new System.Windows.Forms.DataGridView();
            this.Gb_Data = new System.Windows.Forms.GroupBox();
            this.Tlp_Data = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.Lb_Machine = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.Btn_Submit = new System.Windows.Forms.Button();
            this.Cb_DownType = new System.Windows.Forms.ComboBox();
            this.Dtp_End_Date = new System.Windows.Forms.DateTimePicker();
            this.Tb_Memo = new System.Windows.Forms.TextBox();
            this.Cb_DownCode = new System.Windows.Forms.ComboBox();
            this.Dtp_Start_Date = new System.Windows.Forms.DateTimePicker();
            this.Dtp_Start_Time = new System.Windows.Forms.DateTimePicker();
            this.Dtp_End_Time = new System.Windows.Forms.DateTimePicker();
            this.Gb_DownTime_Log = new System.Windows.Forms.GroupBox();
            this.Dgv_DownTime_Log = new System.Windows.Forms.DataGridView();
            this.Tlp_Query = new System.Windows.Forms.TableLayoutPanel();
            this.Btn_OK_1 = new System.Windows.Forms.Button();
            this.Tb_Emp = new System.Windows.Forms.TextBox();
            this.Btn_OK_2 = new System.Windows.Forms.Button();
            this.Tb_RC = new System.Windows.Forms.TextBox();
            this.Lb_Inprocess_Time = new System.Windows.Forms.Label();
            this.Lb_Process = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Btn_Clear = new System.Windows.Forms.Button();
            this.CB_Process = new System.Windows.Forms.ComboBox();
            this.Tlp_Form.SuspendLayout();
            this.Tlp_Grid.SuspendLayout();
            this.Gb_RCInfo.SuspendLayout();
            this.Tlp_RCInfo.SuspendLayout();
            this.Gb_Machine_InUse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_Machine_InUse)).BeginInit();
            this.Gb_Data.SuspendLayout();
            this.Tlp_Data.SuspendLayout();
            this.Gb_DownTime_Log.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_DownTime_Log)).BeginInit();
            this.Tlp_Query.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tlp_Form
            // 
            resources.ApplyResources(this.Tlp_Form, "Tlp_Form");
            this.Tlp_Form.Controls.Add(this.Tlp_Grid, 1, 1);
            this.Tlp_Form.Name = "Tlp_Form";
            // 
            // Tlp_Grid
            // 
            resources.ApplyResources(this.Tlp_Grid, "Tlp_Grid");
            this.Tlp_Grid.Controls.Add(this.Gb_RCInfo, 0, 1);
            this.Tlp_Grid.Controls.Add(this.Gb_Machine_InUse, 1, 1);
            this.Tlp_Grid.Controls.Add(this.Gb_Data, 0, 2);
            this.Tlp_Grid.Controls.Add(this.Gb_DownTime_Log, 0, 3);
            this.Tlp_Grid.Controls.Add(this.Tlp_Query, 0, 0);
            this.Tlp_Grid.Name = "Tlp_Grid";
            // 
            // Gb_RCInfo
            // 
            this.Gb_RCInfo.Controls.Add(this.Tlp_RCInfo);
            resources.ApplyResources(this.Gb_RCInfo, "Gb_RCInfo");
            this.Gb_RCInfo.Name = "Gb_RCInfo";
            this.Gb_RCInfo.TabStop = false;
            // 
            // Tlp_RCInfo
            // 
            resources.ApplyResources(this.Tlp_RCInfo, "Tlp_RCInfo");
            this.Tlp_RCInfo.Controls.Add(this.label1, 0, 0);
            this.Tlp_RCInfo.Controls.Add(this.label5, 0, 1);
            this.Tlp_RCInfo.Controls.Add(this.label6, 0, 2);
            this.Tlp_RCInfo.Controls.Add(this.label7, 0, 3);
            this.Tlp_RCInfo.Controls.Add(this.label9, 2, 1);
            this.Tlp_RCInfo.Controls.Add(this.label3, 2, 2);
            this.Tlp_RCInfo.Controls.Add(this.label4, 2, 3);
            this.Tlp_RCInfo.Controls.Add(this.Tb_WorkOrder, 1, 0);
            this.Tlp_RCInfo.Controls.Add(this.Tb_FormerNo, 1, 1);
            this.Tlp_RCInfo.Controls.Add(this.Tb_Spec, 1, 2);
            this.Tlp_RCInfo.Controls.Add(this.Tb_PartNo, 1, 3);
            this.Tlp_RCInfo.Controls.Add(this.Tb_Qty, 3, 1);
            this.Tlp_RCInfo.Controls.Add(this.Tb_Good, 3, 2);
            this.Tlp_RCInfo.Controls.Add(this.Tb_Scrap, 3, 3);
            this.Tlp_RCInfo.Controls.Add(this.label2, 2, 0);
            this.Tlp_RCInfo.Controls.Add(this.Tb_Runcard, 3, 0);
            this.Tlp_RCInfo.Name = "Tlp_RCInfo";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // Tb_WorkOrder
            // 
            resources.ApplyResources(this.Tb_WorkOrder, "Tb_WorkOrder");
            this.Tb_WorkOrder.Name = "Tb_WorkOrder";
            this.Tb_WorkOrder.ReadOnly = true;
            // 
            // Tb_FormerNo
            // 
            resources.ApplyResources(this.Tb_FormerNo, "Tb_FormerNo");
            this.Tb_FormerNo.Name = "Tb_FormerNo";
            this.Tb_FormerNo.ReadOnly = true;
            // 
            // Tb_Spec
            // 
            resources.ApplyResources(this.Tb_Spec, "Tb_Spec");
            this.Tb_Spec.Name = "Tb_Spec";
            this.Tb_Spec.ReadOnly = true;
            // 
            // Tb_PartNo
            // 
            resources.ApplyResources(this.Tb_PartNo, "Tb_PartNo");
            this.Tb_PartNo.Name = "Tb_PartNo";
            this.Tb_PartNo.ReadOnly = true;
            // 
            // Tb_Qty
            // 
            resources.ApplyResources(this.Tb_Qty, "Tb_Qty");
            this.Tb_Qty.Name = "Tb_Qty";
            this.Tb_Qty.ReadOnly = true;
            // 
            // Tb_Good
            // 
            resources.ApplyResources(this.Tb_Good, "Tb_Good");
            this.Tb_Good.Name = "Tb_Good";
            this.Tb_Good.ReadOnly = true;
            // 
            // Tb_Scrap
            // 
            resources.ApplyResources(this.Tb_Scrap, "Tb_Scrap");
            this.Tb_Scrap.Name = "Tb_Scrap";
            this.Tb_Scrap.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Tb_Runcard
            // 
            resources.ApplyResources(this.Tb_Runcard, "Tb_Runcard");
            this.Tb_Runcard.Name = "Tb_Runcard";
            this.Tb_Runcard.ReadOnly = true;
            // 
            // Gb_Machine_InUse
            // 
            this.Gb_Machine_InUse.Controls.Add(this.Dgv_Machine_InUse);
            resources.ApplyResources(this.Gb_Machine_InUse, "Gb_Machine_InUse");
            this.Gb_Machine_InUse.Name = "Gb_Machine_InUse";
            this.Gb_Machine_InUse.TabStop = false;
            // 
            // Dgv_Machine_InUse
            // 
            this.Dgv_Machine_InUse.AllowUserToAddRows = false;
            this.Dgv_Machine_InUse.AllowUserToDeleteRows = false;
            this.Dgv_Machine_InUse.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Dgv_Machine_InUse.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Dgv_Machine_InUse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.Dgv_Machine_InUse, "Dgv_Machine_InUse");
            this.Dgv_Machine_InUse.MultiSelect = false;
            this.Dgv_Machine_InUse.Name = "Dgv_Machine_InUse";
            this.Dgv_Machine_InUse.ReadOnly = true;
            this.Dgv_Machine_InUse.RowTemplate.Height = 24;
            // 
            // Gb_Data
            // 
            this.Tlp_Grid.SetColumnSpan(this.Gb_Data, 2);
            this.Gb_Data.Controls.Add(this.Tlp_Data);
            resources.ApplyResources(this.Gb_Data, "Gb_Data");
            this.Gb_Data.Name = "Gb_Data";
            this.Gb_Data.TabStop = false;
            // 
            // Tlp_Data
            // 
            resources.ApplyResources(this.Tlp_Data, "Tlp_Data");
            this.Tlp_Data.Controls.Add(this.label11, 0, 0);
            this.Tlp_Data.Controls.Add(this.Lb_Machine, 0, 1);
            this.Tlp_Data.Controls.Add(this.label12, 2, 0);
            this.Tlp_Data.Controls.Add(this.label13, 5, 0);
            this.Tlp_Data.Controls.Add(this.label14, 2, 1);
            this.Tlp_Data.Controls.Add(this.label15, 5, 1);
            this.Tlp_Data.Controls.Add(this.label16, 9, 0);
            this.Tlp_Data.Controls.Add(this.Btn_Submit, 12, 1);
            this.Tlp_Data.Controls.Add(this.Cb_DownType, 3, 0);
            this.Tlp_Data.Controls.Add(this.Dtp_End_Date, 6, 1);
            this.Tlp_Data.Controls.Add(this.Tb_Memo, 10, 0);
            this.Tlp_Data.Controls.Add(this.Cb_DownCode, 3, 1);
            this.Tlp_Data.Controls.Add(this.Dtp_Start_Date, 6, 0);
            this.Tlp_Data.Controls.Add(this.Dtp_Start_Time, 7, 0);
            this.Tlp_Data.Controls.Add(this.Dtp_End_Time, 7, 1);
            this.Tlp_Data.Name = "Tlp_Data";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // Lb_Machine
            // 
            resources.ApplyResources(this.Lb_Machine, "Lb_Machine");
            this.Lb_Machine.Name = "Lb_Machine";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // Btn_Submit
            // 
            resources.ApplyResources(this.Btn_Submit, "Btn_Submit");
            this.Btn_Submit.Name = "Btn_Submit";
            this.Btn_Submit.UseVisualStyleBackColor = true;
            // 
            // Cb_DownType
            // 
            resources.ApplyResources(this.Cb_DownType, "Cb_DownType");
            this.Cb_DownType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cb_DownType.FormattingEnabled = true;
            this.Cb_DownType.Name = "Cb_DownType";
            // 
            // Dtp_End_Date
            // 
            resources.ApplyResources(this.Dtp_End_Date, "Dtp_End_Date");
            this.Dtp_End_Date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.Dtp_End_Date.Name = "Dtp_End_Date";
            // 
            // Tb_Memo
            // 
            resources.ApplyResources(this.Tb_Memo, "Tb_Memo");
            this.Tb_Memo.Name = "Tb_Memo";
            this.Tlp_Data.SetRowSpan(this.Tb_Memo, 2);
            // 
            // Cb_DownCode
            // 
            resources.ApplyResources(this.Cb_DownCode, "Cb_DownCode");
            this.Cb_DownCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cb_DownCode.FormattingEnabled = true;
            this.Cb_DownCode.Name = "Cb_DownCode";
            // 
            // Dtp_Start_Date
            // 
            resources.ApplyResources(this.Dtp_Start_Date, "Dtp_Start_Date");
            this.Dtp_Start_Date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.Dtp_Start_Date.Name = "Dtp_Start_Date";
            // 
            // Dtp_Start_Time
            // 
            resources.ApplyResources(this.Dtp_Start_Time, "Dtp_Start_Time");
            this.Dtp_Start_Time.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_Start_Time.Name = "Dtp_Start_Time";
            this.Dtp_Start_Time.ShowUpDown = true;
            // 
            // Dtp_End_Time
            // 
            resources.ApplyResources(this.Dtp_End_Time, "Dtp_End_Time");
            this.Dtp_End_Time.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_End_Time.Name = "Dtp_End_Time";
            this.Dtp_End_Time.ShowUpDown = true;
            // 
            // Gb_DownTime_Log
            // 
            this.Tlp_Grid.SetColumnSpan(this.Gb_DownTime_Log, 2);
            this.Gb_DownTime_Log.Controls.Add(this.Dgv_DownTime_Log);
            resources.ApplyResources(this.Gb_DownTime_Log, "Gb_DownTime_Log");
            this.Gb_DownTime_Log.Name = "Gb_DownTime_Log";
            this.Gb_DownTime_Log.TabStop = false;
            // 
            // Dgv_DownTime_Log
            // 
            this.Dgv_DownTime_Log.AllowUserToAddRows = false;
            this.Dgv_DownTime_Log.AllowUserToDeleteRows = false;
            this.Dgv_DownTime_Log.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Dgv_DownTime_Log.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.Dgv_DownTime_Log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.Dgv_DownTime_Log, "Dgv_DownTime_Log");
            this.Dgv_DownTime_Log.MultiSelect = false;
            this.Dgv_DownTime_Log.Name = "Dgv_DownTime_Log";
            this.Dgv_DownTime_Log.ReadOnly = true;
            this.Dgv_DownTime_Log.RowTemplate.Height = 24;
            // 
            // Tlp_Query
            // 
            resources.ApplyResources(this.Tlp_Query, "Tlp_Query");
            this.Tlp_Grid.SetColumnSpan(this.Tlp_Query, 2);
            this.Tlp_Query.Controls.Add(this.Btn_OK_1, 2, 0);
            this.Tlp_Query.Controls.Add(this.Tb_Emp, 1, 0);
            this.Tlp_Query.Controls.Add(this.Btn_OK_2, 6, 0);
            this.Tlp_Query.Controls.Add(this.Tb_RC, 5, 0);
            this.Tlp_Query.Controls.Add(this.Lb_Inprocess_Time, 9, 0);
            this.Tlp_Query.Controls.Add(this.Lb_Process, 7, 0);
            this.Tlp_Query.Controls.Add(this.label8, 0, 0);
            this.Tlp_Query.Controls.Add(this.label10, 4, 0);
            this.Tlp_Query.Controls.Add(this.Btn_Clear, 10, 0);
            this.Tlp_Query.Controls.Add(this.CB_Process, 8, 0);
            this.Tlp_Query.Name = "Tlp_Query";
            // 
            // Btn_OK_1
            // 
            resources.ApplyResources(this.Btn_OK_1, "Btn_OK_1");
            this.Btn_OK_1.Name = "Btn_OK_1";
            this.Btn_OK_1.UseVisualStyleBackColor = true;
            // 
            // Tb_Emp
            // 
            resources.ApplyResources(this.Tb_Emp, "Tb_Emp");
            this.Tb_Emp.Name = "Tb_Emp";
            // 
            // Btn_OK_2
            // 
            resources.ApplyResources(this.Btn_OK_2, "Btn_OK_2");
            this.Btn_OK_2.Name = "Btn_OK_2";
            this.Btn_OK_2.UseVisualStyleBackColor = true;
            // 
            // Tb_RC
            // 
            resources.ApplyResources(this.Tb_RC, "Tb_RC");
            this.Tb_RC.Name = "Tb_RC";
            // 
            // Lb_Inprocess_Time
            // 
            resources.ApplyResources(this.Lb_Inprocess_Time, "Lb_Inprocess_Time");
            this.Lb_Inprocess_Time.Name = "Lb_Inprocess_Time";
            // 
            // Lb_Process
            // 
            resources.ApplyResources(this.Lb_Process, "Lb_Process");
            this.Lb_Process.Name = "Lb_Process";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // Btn_Clear
            // 
            resources.ApplyResources(this.Btn_Clear, "Btn_Clear");
            this.Btn_Clear.Name = "Btn_Clear";
            this.Btn_Clear.UseVisualStyleBackColor = true;
            // 
            // CB_Process
            // 
            resources.ApplyResources(this.CB_Process, "CB_Process");
            this.CB_Process.FormattingEnabled = true;
            this.CB_Process.Name = "CB_Process";
            this.CB_Process.SelectedIndexChanged += new System.EventHandler(this.CB_Process_SelectedIndexChanged);
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Tlp_Form);
            this.Name = "fMain";
            this.Tlp_Form.ResumeLayout(false);
            this.Tlp_Grid.ResumeLayout(false);
            this.Gb_RCInfo.ResumeLayout(false);
            this.Tlp_RCInfo.ResumeLayout(false);
            this.Tlp_RCInfo.PerformLayout();
            this.Gb_Machine_InUse.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_Machine_InUse)).EndInit();
            this.Gb_Data.ResumeLayout(false);
            this.Tlp_Data.ResumeLayout(false);
            this.Tlp_Data.PerformLayout();
            this.Gb_DownTime_Log.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_DownTime_Log)).EndInit();
            this.Tlp_Query.ResumeLayout(false);
            this.Tlp_Query.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Tlp_Form;
        private System.Windows.Forms.TableLayoutPanel Tlp_Grid;
        private System.Windows.Forms.TableLayoutPanel Tlp_RCInfo;
        private System.Windows.Forms.GroupBox Gb_RCInfo;
        private System.Windows.Forms.GroupBox Gb_Machine_InUse;
        private System.Windows.Forms.GroupBox Gb_Data;
        private System.Windows.Forms.GroupBox Gb_DownTime_Log;
        private System.Windows.Forms.DataGridView Dgv_Machine_InUse;
        private System.Windows.Forms.DataGridView Dgv_DownTime_Log;
        private System.Windows.Forms.TableLayoutPanel Tlp_Query;
        private System.Windows.Forms.TableLayoutPanel Tlp_Data;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label Lb_Inprocess_Time;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label Lb_Process;
        private System.Windows.Forms.TextBox Tb_WorkOrder;
        private System.Windows.Forms.TextBox Tb_Runcard;
        private System.Windows.Forms.TextBox Tb_FormerNo;
        private System.Windows.Forms.TextBox Tb_Spec;
        private System.Windows.Forms.TextBox Tb_PartNo;
        private System.Windows.Forms.TextBox Tb_Qty;
        private System.Windows.Forms.TextBox Tb_Good;
        private System.Windows.Forms.TextBox Tb_Scrap;
        private System.Windows.Forms.Button Btn_OK_1;
        private System.Windows.Forms.TextBox Tb_Emp;
        private System.Windows.Forms.Button Btn_OK_2;
        private System.Windows.Forms.TextBox Tb_RC;
        private System.Windows.Forms.Button Btn_Clear;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label Lb_Machine;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button Btn_Submit;
        private System.Windows.Forms.ComboBox Cb_DownType;
        private System.Windows.Forms.DateTimePicker Dtp_End_Date;
        private System.Windows.Forms.TextBox Tb_Memo;
        private System.Windows.Forms.ComboBox Cb_DownCode;
        private System.Windows.Forms.DateTimePicker Dtp_Start_Date;
        private System.Windows.Forms.DateTimePicker Dtp_Start_Time;
        private System.Windows.Forms.DateTimePicker Dtp_End_Time;
        private System.Windows.Forms.ComboBox CB_Process;
    }
}