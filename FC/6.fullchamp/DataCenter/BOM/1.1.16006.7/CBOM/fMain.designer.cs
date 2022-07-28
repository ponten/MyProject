namespace CBOM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.SC1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.LVPart = new System.Windows.Forms.ListView();
            this.PartNo = new System.Windows.Forms.ColumnHeader();
            this.Spec1 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.editPartFilter = new System.Windows.Forms.TextBox();
            this.TreeBom = new System.Windows.Forms.TreeView();
            this.PopMenu1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.collapseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
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
            this.ModifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LV1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.editPartNo = new System.Windows.Forms.TextBox();
            this.combVer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SC1.Panel1.SuspendLayout();
            this.SC1.Panel2.SuspendLayout();
            this.SC1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.PopMenu1.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.PopMenu2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SC1
            // 
            resources.ApplyResources(this.SC1, "SC1");
            this.SC1.Name = "SC1";
            // 
            // SC1.Panel1
            // 
            this.SC1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // SC1.Panel2
            // 
            this.SC1.Panel2.Controls.Add(this.splitContainer4);
            // 
            // splitContainer3
            // 
            resources.ApplyResources(this.splitContainer3, "splitContainer3");
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.LVPart);
            this.splitContainer3.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.TreeBom);
            this.splitContainer3.Panel2Collapsed = true;
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
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.editPartFilter);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // editPartFilter
            // 
            resources.ApplyResources(this.editPartFilter, "editPartFilter");
            this.editPartFilter.Name = "editPartFilter";
            this.editPartFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.editPartFilter_KeyDown);
            // 
            // TreeBom
            // 
            this.TreeBom.AllowDrop = true;
            this.TreeBom.BackColor = System.Drawing.Color.White;
            this.TreeBom.ContextMenuStrip = this.PopMenu1;
            resources.ApplyResources(this.TreeBom, "TreeBom");
            this.TreeBom.ImageList = this.imageList1;
            this.TreeBom.Name = "TreeBom";
            // 
            // PopMenu1
            // 
            this.PopMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseToolStripMenuItem1,
            this.expandToolStripMenuItem1});
            this.PopMenu1.Name = "PopMenu2";
            resources.ApplyResources(this.PopMenu1, "PopMenu1");
            // 
            // collapseToolStripMenuItem1
            // 
            this.collapseToolStripMenuItem1.Name = "collapseToolStripMenuItem1";
            resources.ApplyResources(this.collapseToolStripMenuItem1, "collapseToolStripMenuItem1");
            this.collapseToolStripMenuItem1.Click += new System.EventHandler(this.collapseToolStripMenuItem1_Click);
            // 
            // expandToolStripMenuItem1
            // 
            this.expandToolStripMenuItem1.Name = "expandToolStripMenuItem1";
            resources.ApplyResources(this.expandToolStripMenuItem1, "expandToolStripMenuItem1");
            this.expandToolStripMenuItem1.Click += new System.EventHandler(this.expandToolStripMenuItem1_Click);
            // 
            // splitContainer4
            // 
            resources.ApplyResources(this.splitContainer4, "splitContainer4");
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.LVData);
            this.splitContainer4.Panel1.Controls.Add(this.TreeBomData);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.LV1);
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
            this.LVData.SmallImageList = this.imageList1;
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
            this.TreeBomData.HideSelection = false;
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
            this.ModifyToolStripMenuItem,
            this.deleteToolStripMenuItem});
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
            // ModifyToolStripMenuItem
            // 
            this.ModifyToolStripMenuItem.Name = "ModifyToolStripMenuItem";
            resources.ApplyResources(this.ModifyToolStripMenuItem, "ModifyToolStripMenuItem");
            this.ModifyToolStripMenuItem.Click += new System.EventHandler(this.ModifyToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
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
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.editPartNo);
            this.panel1.Controls.Add(this.combVer);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // editPartNo
            // 
            resources.ApplyResources(this.editPartNo, "editPartNo");
            this.editPartNo.Name = "editPartNo";
            this.editPartNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editPartNo_KeyPress);
            // 
            // combVer
            // 
            this.combVer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combVer.FormattingEnabled = true;
            resources.ApplyResources(this.combVer, "combVer");
            this.combVer.Name = "combVer";
            this.combVer.SelectedIndexChanged += new System.EventHandler(this.combVer_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // fMain
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.SC1);
            this.Controls.Add(this.panel1);
            this.Name = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.Shown += new System.EventHandler(this.fMain_Shown);
            this.SC1.Panel1.ResumeLayout(false);
            this.SC1.Panel2.ResumeLayout(false);
            this.SC1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.PopMenu1.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.PopMenu2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip PopMenu1;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ComboBox combVer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip PopMenu2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.SplitContainer SC1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ListView LVPart;
        private System.Windows.Forms.ColumnHeader PartNo;
        private System.Windows.Forms.ColumnHeader Spec1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editPartFilter;
        private System.Windows.Forms.TreeView TreeBomData;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TreeView TreeBom;
        private System.Windows.Forms.ListView LV1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
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
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox editPartNo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ModifyToolStripMenuItem;        
    }
}
