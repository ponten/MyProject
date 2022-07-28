namespace RTMaintain
{
    partial class fMain
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgViewRT = new System.Windows.Forms.DataGridView();
            this.RT_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VENDOR_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VENDOR_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RECEIVE_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RT_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.combFieldName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.editValue = new System.Windows.Forms.TextBox();
            this.lablCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnAppend = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkbDate = new System.Windows.Forms.CheckBox();
            this.btnSearchWO = new System.Windows.Forms.Button();
            this.dtDateFrom = new System.Windows.Forms.DateTimePicker();
            this.btnQuery = new System.Windows.Forms.Button();
            this.dtDateTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.editRTNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.editVendor = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgViewDetail = new System.Windows.Forms.DataGridView();
            this.RT_SEQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PO_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PART_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PART_VERSION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATECODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INCOMING_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOT_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INCOMING_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VENDOR_LOTNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VENDOR_PARTNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WAREHOUSE_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.URGENT_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ROWID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RD_Flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.btnModifyItem = new System.Windows.Forms.Button();
            this.btnAppendItem = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bsRT = new System.Windows.Forms.BindingSource(this.components);
            this.bsItem = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewRT)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewDetail)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsRT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.dgViewRT);
            this.groupBox1.Controls.Add(this.panel1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // dgViewRT
            // 
            this.dgViewRT.AllowUserToAddRows = false;
            this.dgViewRT.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgViewRT.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgViewRT.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dgViewRT, "dgViewRT");
            this.dgViewRT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RT_NO,
            this.VENDOR_CODE,
            this.VENDOR_NAME,
            this.RECEIVE_TIME,
            this.RT_ID});
            this.dgViewRT.MultiSelect = false;
            this.dgViewRT.Name = "dgViewRT";
            this.dgViewRT.ReadOnly = true;
            this.dgViewRT.RowTemplate.Height = 24;
            this.dgViewRT.SelectionChanged += new System.EventHandler(this.dgViewRT_SelectionChanged);
            // 
            // RT_NO
            // 
            resources.ApplyResources(this.RT_NO, "RT_NO");
            this.RT_NO.Name = "RT_NO";
            this.RT_NO.ReadOnly = true;
            // 
            // VENDOR_CODE
            // 
            resources.ApplyResources(this.VENDOR_CODE, "VENDOR_CODE");
            this.VENDOR_CODE.Name = "VENDOR_CODE";
            this.VENDOR_CODE.ReadOnly = true;
            // 
            // VENDOR_NAME
            // 
            resources.ApplyResources(this.VENDOR_NAME, "VENDOR_NAME");
            this.VENDOR_NAME.Name = "VENDOR_NAME";
            this.VENDOR_NAME.ReadOnly = true;
            // 
            // RECEIVE_TIME
            // 
            resources.ApplyResources(this.RECEIVE_TIME, "RECEIVE_TIME");
            this.RECEIVE_TIME.Name = "RECEIVE_TIME";
            this.RECEIVE_TIME.ReadOnly = true;
            // 
            // RT_ID
            // 
            resources.ApplyResources(this.RT_ID, "RT_ID");
            this.RT_ID.Name = "RT_ID";
            this.RT_ID.ReadOnly = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Name = "panel1";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.combFieldName);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.editValue);
            this.panel5.Controls.Add(this.lablCount);
            this.panel5.Controls.Add(this.label3);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // combFieldName
            // 
            this.combFieldName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combFieldName.FormattingEnabled = true;
            resources.ApplyResources(this.combFieldName, "combFieldName");
            this.combFieldName.Name = "combFieldName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // editValue
            // 
            resources.ApplyResources(this.editValue, "editValue");
            this.editValue.Name = "editValue";
            this.editValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editValue_KeyPress);
            // 
            // lablCount
            // 
            resources.ApplyResources(this.lablCount, "lablCount");
            this.lablCount.Name = "lablCount";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnModify);
            this.panel4.Controls.Add(this.btnAppend);
            this.panel4.Controls.Add(this.btnDelete);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // btnModify
            // 
            resources.ApplyResources(this.btnModify, "btnModify");
            this.btnModify.Name = "btnModify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnAppend
            // 
            resources.ApplyResources(this.btnAppend, "btnAppend");
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkbDate);
            this.panel3.Controls.Add(this.btnSearchWO);
            this.panel3.Controls.Add(this.dtDateFrom);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.dtDateTo);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.editRTNo);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.editVendor);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // chkbDate
            // 
            resources.ApplyResources(this.chkbDate, "chkbDate");
            this.chkbDate.Checked = true;
            this.chkbDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbDate.Name = "chkbDate";
            this.chkbDate.UseVisualStyleBackColor = true;
            // 
            // btnSearchWO
            // 
            resources.ApplyResources(this.btnSearchWO, "btnSearchWO");
            this.btnSearchWO.Name = "btnSearchWO";
            this.btnSearchWO.TabStop = false;
            this.btnSearchWO.Tag = "1";
            this.btnSearchWO.UseVisualStyleBackColor = true;
            this.btnSearchWO.Click += new System.EventHandler(this.btnSearchWO_Click);
            // 
            // dtDateFrom
            // 
            this.dtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtDateFrom, "dtDateFrom");
            this.dtDateFrom.Name = "dtDateFrom";
            this.dtDateFrom.Value = new System.DateTime(2010, 5, 17, 0, 0, 0, 0);
            // 
            // btnQuery
            // 
            resources.ApplyResources(this.btnQuery, "btnQuery");
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // dtDateTo
            // 
            this.dtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtDateTo, "dtDateTo");
            this.dtDateTo.Name = "dtDateTo";
            this.dtDateTo.Value = new System.DateTime(2010, 5, 17, 0, 0, 0, 0);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // editRTNo
            // 
            resources.ApplyResources(this.editRTNo, "editRTNo");
            this.editRTNo.Name = "editRTNo";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // editVendor
            // 
            resources.ApplyResources(this.editVendor, "editVendor");
            this.editVendor.Name = "editVendor";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.dgViewDetail);
            this.groupBox2.Controls.Add(this.panel2);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // dgViewDetail
            // 
            this.dgViewDetail.AllowUserToAddRows = false;
            this.dgViewDetail.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgViewDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgViewDetail.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dgViewDetail, "dgViewDetail");
            this.dgViewDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RT_SEQ,
            this.PO_NO,
            this.PART_NO,
            this.PART_VERSION,
            this.DATECODE,
            this.INCOMING_QTY,
            this.LOT_NO,
            this.INCOMING_TIME,
            this.VENDOR_LOTNO,
            this.VENDOR_PARTNO,
            this.WAREHOUSE_NAME,
            this.URGENT_TYPE,
            this.ROWID,
            this.RD_Flag});
            this.dgViewDetail.MultiSelect = false;
            this.dgViewDetail.Name = "dgViewDetail";
            this.dgViewDetail.ReadOnly = true;
            this.dgViewDetail.RowTemplate.Height = 24;
            // 
            // RT_SEQ
            // 
            resources.ApplyResources(this.RT_SEQ, "RT_SEQ");
            this.RT_SEQ.Name = "RT_SEQ";
            this.RT_SEQ.ReadOnly = true;
            // 
            // PO_NO
            // 
            resources.ApplyResources(this.PO_NO, "PO_NO");
            this.PO_NO.Name = "PO_NO";
            this.PO_NO.ReadOnly = true;
            // 
            // PART_NO
            // 
            resources.ApplyResources(this.PART_NO, "PART_NO");
            this.PART_NO.Name = "PART_NO";
            this.PART_NO.ReadOnly = true;
            // 
            // PART_VERSION
            // 
            resources.ApplyResources(this.PART_VERSION, "PART_VERSION");
            this.PART_VERSION.Name = "PART_VERSION";
            this.PART_VERSION.ReadOnly = true;
            // 
            // DATECODE
            // 
            resources.ApplyResources(this.DATECODE, "DATECODE");
            this.DATECODE.Name = "DATECODE";
            this.DATECODE.ReadOnly = true;
            // 
            // INCOMING_QTY
            // 
            resources.ApplyResources(this.INCOMING_QTY, "INCOMING_QTY");
            this.INCOMING_QTY.Name = "INCOMING_QTY";
            this.INCOMING_QTY.ReadOnly = true;
            // 
            // LOT_NO
            // 
            resources.ApplyResources(this.LOT_NO, "LOT_NO");
            this.LOT_NO.Name = "LOT_NO";
            this.LOT_NO.ReadOnly = true;
            // 
            // INCOMING_TIME
            // 
            resources.ApplyResources(this.INCOMING_TIME, "INCOMING_TIME");
            this.INCOMING_TIME.Name = "INCOMING_TIME";
            this.INCOMING_TIME.ReadOnly = true;
            // 
            // VENDOR_LOTNO
            // 
            resources.ApplyResources(this.VENDOR_LOTNO, "VENDOR_LOTNO");
            this.VENDOR_LOTNO.Name = "VENDOR_LOTNO";
            this.VENDOR_LOTNO.ReadOnly = true;
            // 
            // VENDOR_PARTNO
            // 
            resources.ApplyResources(this.VENDOR_PARTNO, "VENDOR_PARTNO");
            this.VENDOR_PARTNO.Name = "VENDOR_PARTNO";
            this.VENDOR_PARTNO.ReadOnly = true;
            // 
            // WAREHOUSE_NAME
            // 
            resources.ApplyResources(this.WAREHOUSE_NAME, "WAREHOUSE_NAME");
            this.WAREHOUSE_NAME.Name = "WAREHOUSE_NAME";
            this.WAREHOUSE_NAME.ReadOnly = true;
            // 
            // URGENT_TYPE
            // 
            resources.ApplyResources(this.URGENT_TYPE, "URGENT_TYPE");
            this.URGENT_TYPE.Name = "URGENT_TYPE";
            this.URGENT_TYPE.ReadOnly = true;
            // 
            // ROWID
            // 
            resources.ApplyResources(this.ROWID, "ROWID");
            this.ROWID.Name = "ROWID";
            this.ROWID.ReadOnly = true;
            // 
            // RD_Flag
            // 
            resources.ApplyResources(this.RD_Flag, "RD_Flag");
            this.RD_Flag.Name = "RD_Flag";
            this.RD_Flag.ReadOnly = true;
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.btnDeleteItem);
            this.panel2.Controls.Add(this.btnModifyItem);
            this.panel2.Controls.Add(this.btnAppendItem);
            this.panel2.Name = "panel2";
            // 
            // btnDeleteItem
            // 
            resources.ApplyResources(this.btnDeleteItem, "btnDeleteItem");
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.UseVisualStyleBackColor = true;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // btnModifyItem
            // 
            resources.ApplyResources(this.btnModifyItem, "btnModifyItem");
            this.btnModifyItem.Name = "btnModifyItem";
            this.btnModifyItem.UseVisualStyleBackColor = true;
            this.btnModifyItem.Click += new System.EventHandler(this.btnAppendItem_Click);
            // 
            // btnAppendItem
            // 
            resources.ApplyResources(this.btnAppendItem, "btnAppendItem");
            this.btnAppendItem.Name = "btnAppendItem";
            this.btnAppendItem.UseVisualStyleBackColor = true;
            this.btnAppendItem.Click += new System.EventHandler(this.btnAppendItem_Click);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.Name = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewRT)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewDetail)).EndInit();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsRT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnModifyItem;
        private System.Windows.Forms.Button btnAppendItem;
        private System.Windows.Forms.BindingSource bsRT;
        private System.Windows.Forms.BindingSource bsItem;
        private System.Windows.Forms.TextBox editValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox combFieldName;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgViewRT;
        private System.Windows.Forms.DataGridView dgViewDetail;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.CheckBox chkbDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox editVendor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox editRTNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtDateTo;
        private System.Windows.Forms.DateTimePicker dtDateFrom;
        private System.Windows.Forms.Label lablCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearchWO;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridViewTextBoxColumn RT_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn VENDOR_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn VENDOR_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn RECEIVE_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn RT_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn RT_SEQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn PO_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PART_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PART_VERSION;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATECODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn INCOMING_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOT_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn INCOMING_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn VENDOR_LOTNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn VENDOR_PARTNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn WAREHOUSE_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn URGENT_TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ROWID;
        private System.Windows.Forms.DataGridViewTextBoxColumn RD_Flag;

    }
}
