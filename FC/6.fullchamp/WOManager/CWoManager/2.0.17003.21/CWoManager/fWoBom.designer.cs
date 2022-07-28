namespace CWoManager
{
    partial class fWoBom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fWoBom));
            this.panel1 = new System.Windows.Forms.Panel();
            this.LabVer = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LabPartNo = new System.Windows.Forms.Label();
            this.LabWO = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LVPart = new System.Windows.Forms.ListView();
            this.PartNo = new System.Windows.Forms.ColumnHeader();
            this.Spec1 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.editPartFilter = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.LVData = new System.Windows.Forms.ListView();
            this.Part = new System.Windows.Forms.ColumnHeader();
            this.Process = new System.Windows.Forms.ColumnHeader();
            this.Qty = new System.Windows.Forms.ColumnHeader();
            this.Relation = new System.Windows.Forms.ColumnHeader();
            this.Version = new System.Windows.Forms.ColumnHeader();
            this.Type = new System.Windows.Forms.ColumnHeader();
            this.Spec = new System.Windows.Forms.ColumnHeader();
            this.Location1 = new System.Windows.Forms.ColumnHeader();
            this.Rowid = new System.Windows.Forms.ColumnHeader();
            this.ProcessID = new System.Windows.Forms.ColumnHeader();
            this.ItemPartID = new System.Windows.Forms.ColumnHeader();
            this.Flag = new System.Windows.Forms.ColumnHeader();
            this.TreeBomData = new System.Windows.Forms.TreeView();
            this.PopMenu2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.LabType = new System.Windows.Forms.Label();
            this.LabBomType = new System.Windows.Forms.Label();
            this.LV1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.PopMenu2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.LabVer);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.LabPartNo);
            this.panel1.Controls.Add(this.LabWO);
            this.panel1.Controls.Add(this.label1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // LabVer
            // 
            resources.ApplyResources(this.LabVer, "LabVer");
            this.LabVer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabVer.Name = "LabVer";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // LabPartNo
            // 
            resources.ApplyResources(this.LabPartNo, "LabPartNo");
            this.LabPartNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabPartNo.Name = "LabPartNo";
            // 
            // LabWO
            // 
            resources.ApplyResources(this.LabWO, "LabWO");
            this.LabWO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabWO.Name = "LabWO";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LVPart);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            // 
            // LVPart
            // 
            this.LVPart.AllowDrop = true;
            this.LVPart.BackColor = System.Drawing.Color.White;
            this.LVPart.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PartNo,
            this.Spec1});
            resources.ApplyResources(this.LVPart, "LVPart");
            this.LVPart.FullRowSelect = true;
            this.LVPart.MultiSelect = false;
            this.LVPart.Name = "LVPart";
            this.LVPart.SmallImageList = this.imageList1;
            this.LVPart.UseCompatibleStateImageBehavior = false;
            this.LVPart.View = System.Windows.Forms.View.Details;
            this.LVPart.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeView_ItemDrag);
            // 
            // PartNo
            // 
            resources.ApplyResources(this.PartNo, "PartNo");
            // 
            // Spec1
            // 
            resources.ApplyResources(this.Spec1, "Spec1");
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1.bmp");
            this.imageList1.Images.SetKeyName(1, "3.bmp");
            this.imageList1.Images.SetKeyName(2, "2.bmp");
            this.imageList1.Images.SetKeyName(3, "0.bmp");
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.editPartFilter);
            this.panel2.Name = "panel2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // editPartFilter
            // 
            resources.ApplyResources(this.editPartFilter, "editPartFilter");
            this.editPartFilter.Name = "editPartFilter";
            this.editPartFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.editPartFilter_KeyDown);
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.LVData);
            this.splitContainer2.Panel1.Controls.Add(this.TreeBomData);
            this.splitContainer2.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.LV1);
            // 
            // LVData
            // 
            this.LVData.BackColor = System.Drawing.Color.White;
            this.LVData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Part,
            this.Process,
            this.Qty,
            this.Relation,
            this.Version,
            this.Type,
            this.Spec,
            this.Location1,
            this.Rowid,
            this.ProcessID,
            this.ItemPartID,
            this.Flag});
            this.LVData.FullRowSelect = true;
            resources.ApplyResources(this.LVData, "LVData");
            this.LVData.Name = "LVData";
            this.LVData.UseCompatibleStateImageBehavior = false;
            this.LVData.View = System.Windows.Forms.View.Details;
            // 
            // Part
            // 
            resources.ApplyResources(this.Part, "Part");
            // 
            // Process
            // 
            resources.ApplyResources(this.Process, "Process");
            // 
            // Qty
            // 
            resources.ApplyResources(this.Qty, "Qty");
            // 
            // Relation
            // 
            resources.ApplyResources(this.Relation, "Relation");
            // 
            // Version
            // 
            resources.ApplyResources(this.Version, "Version");
            // 
            // Type
            // 
            resources.ApplyResources(this.Type, "Type");
            // 
            // Spec
            // 
            resources.ApplyResources(this.Spec, "Spec");
            // 
            // Location1
            // 
            resources.ApplyResources(this.Location1, "Location1");
            // 
            // Rowid
            // 
            resources.ApplyResources(this.Rowid, "Rowid");
            // 
            // ProcessID
            // 
            resources.ApplyResources(this.ProcessID, "ProcessID");
            // 
            // ItemPartID
            // 
            resources.ApplyResources(this.ItemPartID, "ItemPartID");
            // 
            // Flag
            // 
            resources.ApplyResources(this.Flag, "Flag");
            // 
            // TreeBomData
            // 
            this.TreeBomData.AllowDrop = true;
            this.TreeBomData.BackColor = System.Drawing.Color.White;
            this.TreeBomData.ContextMenuStrip = this.PopMenu2;
            resources.ApplyResources(this.TreeBomData, "TreeBomData");
            this.TreeBomData.ImageList = this.imageList1;
            this.TreeBomData.Name = "TreeBomData";
            this.TreeBomData.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeBomData_DragDrop);
            this.TreeBomData.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeBomData_AfterSelect);
            this.TreeBomData.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeView_DragEnter);
            this.TreeBomData.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeView_ItemDrag);
            this.TreeBomData.DragOver += new System.Windows.Forms.DragEventHandler(this.TreeBomData_DragOver);
            // 
            // PopMenu2
            // 
            this.PopMenu2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripSeparator1,
            this.MenuItemDelete});
            this.PopMenu2.Name = "PopMenu2";
            resources.ApplyResources(this.PopMenu2, "PopMenu2");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // MenuItemDelete
            // 
            this.MenuItemDelete.Name = "MenuItemDelete";
            resources.ApplyResources(this.MenuItemDelete, "MenuItemDelete");
            this.MenuItemDelete.Click += new System.EventHandler(this.MenuItemDelete_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel3.Controls.Add(this.LabType);
            this.panel3.Controls.Add(this.LabBomType);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // LabType
            // 
            resources.ApplyResources(this.LabType, "LabType");
            this.LabType.ForeColor = System.Drawing.Color.Red;
            this.LabType.Name = "LabType";
            // 
            // LabBomType
            // 
            resources.ApplyResources(this.LabBomType, "LabBomType");
            this.LabBomType.Name = "LabBomType";
            // 
            // LV1
            // 
            this.LV1.BackColor = System.Drawing.Color.White;
            this.LV1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            resources.ApplyResources(this.LV1, "LV1");
            this.LV1.FullRowSelect = true;
            this.LV1.Name = "LV1";
            this.LV1.SmallImageList = this.imageList1;
            this.LV1.UseCompatibleStateImageBehavior = false;
            this.LV1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // columnHeader7
            // 
            resources.ApplyResources(this.columnHeader7, "columnHeader7");
            // 
            // columnHeader8
            // 
            resources.ApplyResources(this.columnHeader8, "columnHeader8");
            // 
            // fWoBom
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "fWoBom";
            this.Load += new System.EventHandler(this.fWoBom_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.PopMenu2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editPartFilter;
        private System.Windows.Forms.ColumnHeader PartNo;
        private System.Windows.Forms.ColumnHeader Spec1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView LV1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip PopMenu2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.Label LabWO;
        public System.Windows.Forms.Label LabPartNo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label LabBomType;
        public System.Windows.Forms.Label LabVer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TreeView TreeBomData;
        public System.Windows.Forms.ListView LVPart;
        public System.Windows.Forms.ToolStripMenuItem MenuItemDelete;
        public System.Windows.Forms.Label LabType;
        private System.Windows.Forms.ListView LVData;
        private System.Windows.Forms.ColumnHeader Part;
        private System.Windows.Forms.ColumnHeader Process;
        private System.Windows.Forms.ColumnHeader Qty;
        private System.Windows.Forms.ColumnHeader Relation;
        private System.Windows.Forms.ColumnHeader Version;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ColumnHeader Spec;
        private System.Windows.Forms.ColumnHeader Location1;
        private System.Windows.Forms.ColumnHeader Rowid;
        private System.Windows.Forms.ColumnHeader ProcessID;
        private System.Windows.Forms.ColumnHeader ItemPartID;
        private System.Windows.Forms.ColumnHeader Flag;        
    }
}