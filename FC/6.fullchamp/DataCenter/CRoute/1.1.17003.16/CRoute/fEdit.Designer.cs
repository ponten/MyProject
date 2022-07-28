namespace CRoute
{
    partial class fEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fEdit));
            this.btnAddEnd = new System.Windows.Forms.Button();
            this.btnAddnode = new System.Windows.Forms.Button();
            this.btnclear = new System.Windows.Forms.Button();
            this.btnSavexml = new System.Windows.Forms.Button();
            this.btnGroup = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboLine = new System.Windows.Forms.ComboBox();
            this.rdLink1 = new System.Windows.Forms.RadioButton();
            this.rdDrag = new System.Windows.Forms.RadioButton();
            this.labRouteName = new System.Windows.Forms.Label();
            this.labRouteID = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddEnd
            // 
            this.btnAddEnd.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddEnd.Image = ((System.Drawing.Image)(resources.GetObject("btnAddEnd.Image")));
            this.btnAddEnd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddEnd.Location = new System.Drawing.Point(305, 3);
            this.btnAddEnd.Name = "btnAddEnd";
            this.btnAddEnd.Size = new System.Drawing.Size(90, 40);
            this.btnAddEnd.TabIndex = 22;
            this.btnAddEnd.Text = "新增終點";
            this.btnAddEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddEnd.UseVisualStyleBackColor = false;
            this.btnAddEnd.Click += new System.EventHandler(this.btnAddEnd_Click);
            // 
            // btnAddnode
            // 
            this.btnAddnode.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddnode.Image = ((System.Drawing.Image)(resources.GetObject("btnAddnode.Image")));
            this.btnAddnode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddnode.Location = new System.Drawing.Point(209, 3);
            this.btnAddnode.Name = "btnAddnode";
            this.btnAddnode.Size = new System.Drawing.Size(90, 40);
            this.btnAddnode.TabIndex = 17;
            this.btnAddnode.Text = "新增節點";
            this.btnAddnode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddnode.UseVisualStyleBackColor = false;
            this.btnAddnode.Click += new System.EventHandler(this.btnAddnode_Click);
            // 
            // btnclear
            // 
            this.btnclear.Image = ((System.Drawing.Image)(resources.GetObject("btnclear.Image")));
            this.btnclear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnclear.Location = new System.Drawing.Point(113, 3);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(90, 40);
            this.btnclear.TabIndex = 18;
            this.btnclear.Text = "清除畫面";
            this.btnclear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnclear.UseVisualStyleBackColor = true;
            this.btnclear.Click += new System.EventHandler(this.btnclear_Click);
            // 
            // btnSavexml
            // 
            this.btnSavexml.AutoSize = true;
            this.btnSavexml.Image = ((System.Drawing.Image)(resources.GetObject("btnSavexml.Image")));
            this.btnSavexml.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSavexml.Location = new System.Drawing.Point(12, 3);
            this.btnSavexml.Name = "btnSavexml";
            this.btnSavexml.Size = new System.Drawing.Size(90, 40);
            this.btnSavexml.TabIndex = 16;
            this.btnSavexml.Text = "儲存檔案";
            this.btnSavexml.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSavexml.UseVisualStyleBackColor = true;
            this.btnSavexml.Click += new System.EventHandler(this.btnSavexml_Click);
            // 
            // btnGroup
            // 
            this.btnGroup.Image = ((System.Drawing.Image)(resources.GetObject("btnGroup.Image")));
            this.btnGroup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGroup.Location = new System.Drawing.Point(401, 3);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Size = new System.Drawing.Size(90, 40);
            this.btnGroup.TabIndex = 14;
            this.btnGroup.Text = "建立群組";
            this.btnGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGroup.UseVisualStyleBackColor = true;
            this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel3.Location = new System.Drawing.Point(0, 49);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1028, 411);
            this.panel3.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.labRouteName);
            this.panel1.Controls.Add(this.labRouteID);
            this.panel1.Controls.Add(this.btnclear);
            this.panel1.Controls.Add(this.btnGroup);
            this.panel1.Controls.Add(this.btnAddnode);
            this.panel1.Controls.Add(this.btnSavexml);
            this.panel1.Controls.Add(this.btnAddEnd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1028, 49);
            this.panel1.TabIndex = 16;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboLine);
            this.groupBox1.Controls.Add(this.rdLink1);
            this.groupBox1.Controls.Add(this.rdDrag);
            this.groupBox1.Location = new System.Drawing.Point(494, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 47);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            // 
            // comboLine
            // 
            this.comboLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLine.Enabled = false;
            this.comboLine.FormattingEnabled = true;
            this.comboLine.Items.AddRange(new object[] {
            "直線",
            "VH",
            "VHV",
            "HV",
            "HVH"});
            this.comboLine.Location = new System.Drawing.Point(131, 16);
            this.comboLine.Name = "comboLine";
            this.comboLine.Size = new System.Drawing.Size(121, 20);
            this.comboLine.TabIndex = 1;
            // 
            // rdLink1
            // 
            this.rdLink1.Image = ((System.Drawing.Image)(resources.GetObject("rdLink1.Image")));
            this.rdLink1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rdLink1.Location = new System.Drawing.Point(77, 12);
            this.rdLink1.Name = "rdLink1";
            this.rdLink1.Size = new System.Drawing.Size(50, 30);
            this.rdLink1.TabIndex = 0;
            this.rdLink1.TabStop = true;
            this.rdLink1.UseVisualStyleBackColor = true;
            this.rdLink1.CheckedChanged += new System.EventHandler(this.rdLink1_CheckedChanged);
            // 
            // rdDrag
            // 
            this.rdDrag.Image = ((System.Drawing.Image)(resources.GetObject("rdDrag.Image")));
            this.rdDrag.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rdDrag.Location = new System.Drawing.Point(11, 12);
            this.rdDrag.Name = "rdDrag";
            this.rdDrag.Size = new System.Drawing.Size(60, 30);
            this.rdDrag.TabIndex = 0;
            this.rdDrag.TabStop = true;
            this.rdDrag.UseVisualStyleBackColor = true;
            this.rdDrag.CheckedChanged += new System.EventHandler(this.rdDrag_CheckedChanged);
            // 
            // labRouteName
            // 
            this.labRouteName.AutoSize = true;
            this.labRouteName.ForeColor = System.Drawing.Color.Red;
            this.labRouteName.Location = new System.Drawing.Point(763, 29);
            this.labRouteName.Name = "labRouteName";
            this.labRouteName.Size = new System.Drawing.Size(0, 12);
            this.labRouteName.TabIndex = 25;
            // 
            // labRouteID
            // 
            this.labRouteID.AutoSize = true;
            this.labRouteID.ForeColor = System.Drawing.Color.Red;
            this.labRouteID.Location = new System.Drawing.Point(763, 7);
            this.labRouteID.Name = "labRouteID";
            this.labRouteID.Size = new System.Drawing.Size(0, 12);
            this.labRouteID.TabIndex = 24;
            // 
            // fEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1028, 460);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "fEdit";
            this.Text = "fEdit";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddEnd;
        private System.Windows.Forms.Button btnAddnode;
        private System.Windows.Forms.Button btnclear;
        private System.Windows.Forms.Button btnSavexml;
        private System.Windows.Forms.Button btnGroup;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labRouteName;
        private System.Windows.Forms.Label labRouteID;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdLink1;
        private System.Windows.Forms.RadioButton rdDrag;
        private System.Windows.Forms.ComboBox comboLine;


    }
}