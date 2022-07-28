using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Collections.Specialized;
using System.Data.OracleClient;

namespace CRole
{
    public partial class fModule : Form
    {
        public fModule()
        {
            InitializeComponent();
        }

        public string g_sRoleID;
        public string g_sRoleName;
        public string sSQL;
        StringCollection sListAuthEng = new StringCollection();
        StringCollection sListAuthCht = new StringCollection();
        public string g_sField;

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            //Delete
            sSQL = " Delete SAJET.SYS_ROLE_PRIVILEGE "
                 + " Where ROLE_ID = '" + g_sRoleID + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            //Insert 
            for (int i = 0; i <= TreeViewSelect.Nodes.Count - 1; i++)
            {
                string sProgram = TreeViewSelect.Nodes[i].Name;
                for (int j = 0; j <= TreeViewSelect.Nodes[i].Nodes.Count - 1; j++)
                {
                    string sFun = TreeViewSelect.Nodes[i].Nodes[j].Name;
                    string sAuth = TreeViewSelect.Nodes[i].Nodes[j].Tag.ToString();

                    sSQL = " Insert Into SAJET.SYS_ROLE_PRIVILEGE "
                         + " (ROLE_ID,PROGRAM,FUNCTION,AUTHORITYS,UPDATE_USERID) "
                         + " Values "
                         + " ('" + g_sRoleID + "','" + sProgram + "','" + sFun + "','" + sAuth + "','" + fMain.g_sUserID + "') ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
            }

            MessageBox.Show(SajetCommon.SetLanguage("Privilege Apply OK", 1));
            DialogResult = DialogResult.OK;
        }

        public void Show_Module_List()
        {
            TreeViewAll.Nodes.Clear();
            LVData.Items.Clear();

            string sFUN_Field = "FUN_ENG";
            string sFUN_DESC_Field = "FUN_DESC_ENG";
            string sProgram_Field = "PROGRAM";
            string sFUN_SQL = "";
            if (!string.IsNullOrEmpty(g_sField))
            {
                sFUN_Field = "FUN_" + g_sField;
                sFUN_DESC_Field = "FUN_DESC_" + g_sField;
                sProgram_Field = "PROGRAM_" + g_sField;
                sFUN_SQL = " ,b." + sProgram_Field + " ,a." + sFUN_Field + ",a." + sFUN_DESC_Field + " ";
            }

            string sPreProgram = "";
            string sPreFunction = "";
            string sSQL = "SELECT PARAM_VALUE FROM SAJET.SYS_BASE_PARAM "
                + "WHERE PROGRAM = 'Data Center' AND PARAM_NAME = 'Role' AND PARAM_TYPE = 'List' AND ROWNUM = 1";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sSQL = dsTemp.Tables[0].Rows[0][0].ToString().Replace("@", sFUN_SQL);
            }
            else
            {
                sSQL = string.Format(@"SELECT DISTINCT A.PROGRAM, a.FUNCTION, c.AUTH_SEQ, c.AUTHORITYS,
                    B.FUN_TYPE_IDX, A.FUN_TYPE_IDX, A.FUN_IDX, FUN_DESC_ENG {0}
                    from SAJET.SYS_PROGRAM_FUN_NAME a, SAJET.SYS_PROGRAM_FUN_AUTHORITY C, sajet.sys_program_name b 
                    where a.program = b.program 
                    and a.ENABLED = 'Y' 
                    and b.ENABLED = 'Y' 
                    AND A.PROGRAM = C.PROGRAM AND A.FUNCTION = C.FUNCTION 
                    ORDER BY 5, 9, 1, 6, 7, 10, 3, 4", sFUN_SQL);

            }
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sProgram = dsTemp.Tables[0].Rows[i]["PROGRAM"].ToString();
                string sFunction = dsTemp.Tables[0].Rows[i]["FUNCTION"].ToString();
                string sAuth = dsTemp.Tables[0].Rows[i]["AUTHORITYS"].ToString();
                string sAuth_Seq = dsTemp.Tables[0].Rows[i]["AUTH_SEQ"].ToString();

                string sProgramDisplay = dsTemp.Tables[0].Rows[i][sProgram_Field].ToString();
                if (string.IsNullOrEmpty(sProgramDisplay.Trim()))
                    sProgramDisplay = dsTemp.Tables[0].Rows[i]["PROGRAM"].ToString();
                string sFunDisplay = dsTemp.Tables[0].Rows[i][sFUN_Field].ToString();
                if (string.IsNullOrEmpty(sFunDisplay.Trim()))
                    sFunDisplay = dsTemp.Tables[0].Rows[i]["FUNCTION"].ToString();
                string sFunctionDesc = dsTemp.Tables[0].Rows[i][sFUN_DESC_Field].ToString();
                if (string.IsNullOrEmpty(sFunctionDesc.Trim()))
                    sFunctionDesc = dsTemp.Tables[0].Rows[i]["FUN_DESC_ENG"].ToString();

                //根節點(PROGRAM)
                if (sPreProgram != sProgram)
                {
                    TreeNode tNode = new TreeNode();
                    tNode.Text = sProgramDisplay;
                    tNode.Name = sProgram;
                    tNode.ImageIndex = 0;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    TreeViewAll.Nodes.Add(tNode);
                    sPreFunction = "";
                }

                //子節點(FUNCTION)
                if (sPreFunction != sFunction)
                {
                    TreeNode tNode = new TreeNode();
                    tNode.Text = sFunDisplay;
                    tNode.Name = sFunction;
                    tNode.ImageIndex = 1;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    tNode.ToolTipText = sFunctionDesc;
                    TreeViewAll.Nodes[TreeViewAll.Nodes.Count - 1].Nodes.Add(tNode);
                }

                sPreProgram = sProgram;
                sPreFunction = sFunction;

                //ListView
                LVData.Items.Add(sProgram);
                LVData.Items[LVData.Items.Count - 1].SubItems.Add(sFunction);
                LVData.Items[LVData.Items.Count - 1].SubItems.Add(sAuth_Seq);
                LVData.Items[LVData.Items.Count - 1].SubItems.Add(SajetCommon.SetLanguage(sAuth, 1));
                LVData.Items[LVData.Items.Count - 1].SubItems.Add(sAuth);
                LVData.Items[LVData.Items.Count - 1].SubItems.Add(sFunctionDesc);
            }
        }

        public void Show_Module_Pri()
        {
            TreeViewSelect.Nodes.Clear();

            string sFUN_Field = "FUN_ENG";
            string sFUN_DESC_Field = "FUN_DESC_ENG";
            string sProgram_Field = "PROGRAM";
            string sFUN_SQL = "";
            if (!string.IsNullOrEmpty(g_sField))
            {
                sFUN_Field = "FUN_" + g_sField;
                sFUN_DESC_Field = "FUN_DESC_" + g_sField;
                sProgram_Field = "PROGRAM_" + g_sField;
                sFUN_SQL = " ,c." + sProgram_Field + " ,b." + sFUN_Field + ",b." + sFUN_DESC_Field + " ";
            }

            string sPreProgram = "";
            string sPreFunction = "";
            string sSQL = "SELECT PARAM_VALUE FROM SAJET.SYS_BASE_PARAM "
                + "WHERE PROGRAM = 'Data Center' AND PARAM_NAME = 'Role' AND PARAM_TYPE = 'Role' AND ROWNUM = 1";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            object[][] Params = new object[1][]; ;
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROLE_ID", g_sRoleID };
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sSQL = dsTemp.Tables[0].Rows[0][0].ToString().Replace("@", sFUN_SQL);
            }
            else
            {
                sSQL = string.Format(@"Select distinct a.PROGRAM,A.AUTHORITYS,a.FUNCTION,b.FUN_ENG,b.FUN_DESC_ENG,A.AUTHORITYS ROLE_AUTHORITYS{0},C.FUN_TYPE_IDX,B.FUN_TYPE_IDX,B.FUN_IDX
                    FROM SAJET.SYS_ROLE_PRIVILEGE a, sajet.sys_program_fun_name b, sajet.sys_program_name c 
                    Where a.Role_id = :ROLE_ID 
                    and a.program = b.program 
                    and a.function = b.function 
                    and a.program = c.program 
                    and b.ENABLED = 'Y' 
                    and c.ENABLED = 'Y'
                    ORDER BY C.FUN_TYPE_IDX, PROGRAM, B.FUN_TYPE_IDX, B.FUN_IDX, FUNCTION, A.AUTHORITYS ", sFUN_SQL);
                /*sSQL = " Select distinct a.PROGRAM,A.AUTHORITYS,a.FUNCTION,b.FUN_ENG,b.FUN_DESC_ENG,A.AUTHORITYS ROLE_AUTHORITYS " + sFUN_SQL
                    + " ,C.FUN_TYPE_IDX,B.FUN_TYPE_IDX,B.FUN_IDX "
                    + " from SAJET.SYS_ROLE_PRIVILEGE a, sajet.sys_program_fun_name b "
                    + " ,sajet.sys_program_name c "
                    + " Where a.Role_id = :ROLE_ID "
                    + " and a.program = b.program "
                    + " and a.function = b.function "
                    + " and a.program = c.program "
                    + " and b.ENABLED = 'Y' "
                    + " and c.ENABLED = 'Y' "
                    + " Order by  C.FUN_TYPE_IDX,a.PROGRAM, B.FUN_TYPE_IDX,B.FUN_IDX,A.AUTHORITYS ";*/
                //  + " Order by a.PROGRAM,a.FUNCTION,A.AUTHORITYS ";
            }
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sProgram = dsTemp.Tables[0].Rows[i]["PROGRAM"].ToString();
                string sFunction = dsTemp.Tables[0].Rows[i]["FUNCTION"].ToString();
                string sAuth = dsTemp.Tables[0].Rows[i]["AUTHORITYS"].ToString();

                string sProgramDisplay = dsTemp.Tables[0].Rows[i][sProgram_Field].ToString();
                if (string.IsNullOrEmpty(sProgramDisplay.Trim()))
                    sProgramDisplay = dsTemp.Tables[0].Rows[i]["PROGRAM"].ToString();
                string sFunDisplay = dsTemp.Tables[0].Rows[i][sFUN_Field].ToString();
                if (string.IsNullOrEmpty(sFunDisplay.Trim()))
                    sFunDisplay = dsTemp.Tables[0].Rows[i]["FUNCTION"].ToString();
                string sFunctionDesc = dsTemp.Tables[0].Rows[i][sFUN_DESC_Field].ToString();
                if (string.IsNullOrEmpty(sFunctionDesc.Trim()))
                    sFunctionDesc = dsTemp.Tables[0].Rows[i]["FUN_DESC_ENG"].ToString();

                //根節點(PROGRAM)
                if (sPreProgram != sProgram)
                {
                    TreeNode tNode = new TreeNode();
                    tNode.Text = sProgramDisplay;
                    tNode.Name = sProgram;
                    tNode.ImageIndex = 0;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    TreeViewSelect.Nodes.Add(tNode);
                    sPreFunction = "";
                }

                //子節點(FUNCTION)
                if (sPreFunction != sFunction)
                {
                    TreeNode tNode = new TreeNode();
                    tNode.Text = sFunDisplay;
                    tNode.Name = sFunction;
                    tNode.ImageIndex = 1;
                    tNode.ToolTipText = sFunctionDesc;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    tNode.Tag = dsTemp.Tables[0].Rows[i]["ROLE_AUTHORITYS"].ToString();
                    TreeViewSelect.Nodes[TreeViewSelect.Nodes.Count - 1].Nodes.Add(tNode);
                }

                //子節點(權限)
                TreeNode tNode1 = new TreeNode();
                tNode1.Tag = sAuth;
                tNode1.Text = SajetCommon.SetLanguage(sAuth, 1);
                tNode1.Name = tNode1.Text;
                tNode1.ImageIndex = 2;
                tNode1.SelectedImageIndex = tNode1.ImageIndex;
                TreeViewSelect.Nodes[TreeViewSelect.Nodes.Count - 1].LastNode.Nodes.Add(tNode1);

                sPreProgram = sProgram;
                sPreFunction = sFunction;
            }
        }

        private void fModule_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            sSQL = "Select distinct authoritys "
                 + "from sajet.sys_program_fun_authority "
                 + "where authoritys is not null";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sAuthority = dsTemp.Tables[0].Rows[i]["authoritys"].ToString();
                sListAuthEng.Add(sAuthority);
                sListAuthCht.Add(SajetCommon.SetLanguage(sAuthority, 1));
            }
            LabRoleName.Text = g_sRoleName;
            Show_Module_List();
            Show_Module_Pri();
        }

        private void TreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void TreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void TreeViewSelect_DragDrop(object sender, DragEventArgs e)
        {
            //TreeNode mNode;
            TreeNode SrcNode;
            SrcNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode"); //來源Node           
            //目的Node    
            //Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            //mNode = ((TreeView)sender).GetNodeAt(pt);                                            

            TreeViewSelect.Focus();
            //移動整個Program
            if (SrcNode.Level == 0)
            {
                //若右方已有此Program,不可移動整個program
                TreeNode[] tProgramNode = TreeViewSelect.Nodes.Find(SrcNode.Name, false);
                if (tProgramNode.Length > 0)
                {
                    TreeViewSelect.SelectedNode = tProgramNode[0];
                    return;
                }

                TreeViewSelect.Nodes.Add((TreeNode)SrcNode.Clone());
                for (int i = 0; i <= TreeViewSelect.Nodes[TreeViewSelect.Nodes.Count - 1].Nodes.Count - 1; i++)
                {
                    //加入預設的Auth節點
                    string sProgram = SrcNode.Name;
                    string sFunction = SrcNode.Nodes[i].Name;
                    string sAuthData = Get_Default_Auth(sProgram, sFunction);
                    TreeNode tNode = new TreeNode();
                    tNode.Text = SajetCommon.SetLanguage(sAuthData, 1);
                    tNode.Tag = sAuthData;
                    tNode.Name = tNode.Text;
                    tNode.ImageIndex = 2;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    TreeViewSelect.Nodes[TreeViewSelect.Nodes.Count - 1].Nodes[i].Tag = sAuthData;
                    TreeViewSelect.Nodes[TreeViewSelect.Nodes.Count - 1].Nodes[i].Nodes.Add(tNode);
                }
                TreeViewSelect.SelectedNode = TreeViewSelect.Nodes[TreeViewSelect.Nodes.Count - 1];
            }

            //只移動Function
            if (SrcNode.Level == 1)
            {
                string sProgram = SrcNode.Parent.Name;
                string sProgramDisplay = SrcNode.Parent.Text;
                string sFunction = SrcNode.Name;
                string sFunctionDisplay = SrcNode.Text;
                //找是否已有父節點(Program),若無則先加入
                TreeNode tProgramNode;
                TreeNode[] tFindNode = TreeViewSelect.Nodes.Find(sProgram, false);
                if (tFindNode.Length == 0)
                {
                    TreeNode tParentNode = new TreeNode();
                    tParentNode.Text = sProgramDisplay;
                    tParentNode.Name = sProgram;
                    tParentNode.ImageIndex = 0;
                    tParentNode.SelectedImageIndex = tParentNode.ImageIndex;
                    TreeViewSelect.Nodes.Add(tParentNode);
                    tProgramNode = TreeViewSelect.Nodes[TreeViewSelect.Nodes.Count - 1];
                }
                else
                    tProgramNode = tFindNode[0];

                TreeViewSelect.SelectedNode = tProgramNode;

                //找子節點(Function)是否已存在
                if (tProgramNode.Nodes.Find(sFunction, false).Length > 0)
                    return;

                //加入選擇的Function節點
                TreeNode tNewNode = new TreeNode();
                tNewNode.Text = sFunctionDisplay;
                tNewNode.Name = sFunction;
                tNewNode.ImageIndex = 1;
                tNewNode.SelectedImageIndex = tNewNode.ImageIndex;
                tProgramNode.Nodes.Add(tNewNode);

                //加入預設的Auth節點                
                string sAuthData = Get_Default_Auth(sProgram, sFunction);
                tNewNode.Tag = sAuthData;
                TreeNode tNode = new TreeNode();
                tNode.Text = SajetCommon.SetLanguage(sAuthData, 1);
                tNode.Tag = sAuthData;
                tNode.Name = tNode.Text;
                tNode.ImageIndex = 2;
                tNode.SelectedImageIndex = tNode.ImageIndex;
                tProgramNode.LastNode.Nodes.Add(tNode);
            }
        }

        public string Get_Default_Auth(string sPrg, string sFun)
        {
            for (int i = 0; i <= LVData.Items.Count - 1; i++)
            {
                if (LVData.Items[i].Text == sPrg && LVData.Items[i].SubItems[1].Text == sFun)
                {
                    return LVData.Items[i].SubItems[4].Text;
                }
            }
            return "";
        }

        private void TreeViewSelect_AfterSelect(object sender, TreeViewEventArgs e)
        {
            combAuth.Items.Clear();
            combAuth.Text = "";
            string sPrg = "";
            string sFun = "";
            string sAuth = "";

            switch (TreeViewSelect.SelectedNode.Level)
            {
                case 0:
                    return;
                case 1:
                    sPrg = TreeViewSelect.SelectedNode.Parent.Name;
                    sFun = TreeViewSelect.SelectedNode.Name;
                    if (TreeViewSelect.SelectedNode.Tag == null)
                        sAuth = TreeViewSelect.SelectedNode.Nodes[0].Text;
                    else
                        sAuth = SajetCommon.SetLanguage(TreeViewSelect.SelectedNode.Tag.ToString(), 1);
                    break;
                case 2:
                    sPrg = TreeViewSelect.SelectedNode.Parent.Parent.Name;
                    sFun = TreeViewSelect.SelectedNode.Parent.Name;
                    sAuth = TreeViewSelect.SelectedNode.Text;
                    break;
                default:
                    return;
            }

            menuitemCombAuth.Items.Clear();
            menuitemCombAuth.Text = "";
            for (int i = 0; i <= LVData.Items.Count - 1; i++)
            {
                if (LVData.Items[i].Text == sPrg && LVData.Items[i].SubItems[1].Text == sFun)
                {
                    combAuth.Items.Add(LVData.Items[i].SubItems[3].Text);
                    menuitemCombAuth.Items.Add(LVData.Items[i].SubItems[3].Text);
                    if (LVData.Items[i].Text != sPrg || LVData.Items[i].SubItems[1].Text != sFun)
                        return;
                }
            }
            combAuth.SelectedIndex = combAuth.FindString(sAuth);
            menuitemCombAuth.SelectedIndex = menuitemCombAuth.FindString(sAuth);
        }

        private void combAuth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TreeViewSelect.SelectedNode == null)
                return;

            switch (TreeViewSelect.SelectedNode.Level)
            {
                case 0:
                    return;
                case 1:
                    TreeViewSelect.SelectedNode.Tag = sListAuthEng[sListAuthCht.IndexOf(combAuth.Text)].ToString();
                    TreeViewSelect.SelectedNode.Nodes[0].Text = combAuth.Text;
                    break;
                case 2:
                    TreeViewSelect.SelectedNode.Parent.Tag = sListAuthEng[sListAuthCht.IndexOf(combAuth.Text)].ToString();
                    TreeViewSelect.SelectedNode.Text = combAuth.Text;
                    break;
            }
        }

        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeViewSelect.CollapseAll();
        }

        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeViewSelect.ExpandAll();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (TreeViewSelect.SelectedNode.Level)
            {
                case 0:
                    TreeViewSelect.SelectedNode.Remove();
                    break;
                case 1:
                    if (TreeViewSelect.SelectedNode.Parent.Nodes.Count <= 1)
                        TreeViewSelect.SelectedNode.Parent.Remove();
                    else
                        TreeViewSelect.SelectedNode.Remove();
                    break;
                case 2:
                    if (TreeViewSelect.SelectedNode.Parent.Parent.Nodes.Count <= 1)
                        TreeViewSelect.SelectedNode.Parent.Parent.Remove();
                    else
                        TreeViewSelect.SelectedNode.Parent.Remove();
                    break;
            }

        }

        private void menuitemCombAuth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TreeViewSelect.SelectedNode == null)
                return;

            switch (TreeViewSelect.SelectedNode.Level)
            {
                case 0:
                    return;
                case 1:
                    TreeViewSelect.SelectedNode.Tag = sListAuthEng[sListAuthCht.IndexOf(menuitemCombAuth.Text)].ToString();
                    break;
                case 2:
                    TreeViewSelect.SelectedNode.Parent.Tag = sListAuthEng[sListAuthCht.IndexOf(menuitemCombAuth.Text)].ToString();
                    break;
            }
            combAuth.SelectedIndex = combAuth.FindString(menuitemCombAuth.Text);
            popMenu2.Close();
        }

        private void popMenu2_Opening(object sender, CancelEventArgs e)
        {
            if (TreeViewSelect.SelectedNode == null)
                return;
            if (TreeViewSelect.SelectedNode.Level == 0)
            {
                menuitemCombAuth.Visible = false;
                AlltoolStripMenuItem.Visible = true;
            }
            else
            {
                menuitemCombAuth.Visible = true;
                AlltoolStripMenuItem.Visible = false;
            }
        }

        private void minToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //全部設為最小的權限
            for (int j = 0; j <= TreeViewSelect.SelectedNode.GetNodeCount(false) - 1; j++)
            {
                string sPrg = TreeViewSelect.SelectedNode.Name;
                string sFun = TreeViewSelect.SelectedNode.Nodes[j].Name;
                string sAuth = TreeViewSelect.SelectedNode.Nodes[j].Nodes[0].Text;

                for (int i = 0; i <= LVData.Items.Count - 1; i++)
                {
                    if (LVData.Items[i].Text == sPrg && LVData.Items[i].SubItems[1].Text == sFun)
                    {
                        TreeViewSelect.SelectedNode.Nodes[j].Tag = LVData.Items[i].SubItems[4].Text;
                        TreeViewSelect.SelectedNode.Nodes[j].Nodes[0].Text = sListAuthCht[sListAuthEng.IndexOf(LVData.Items[i].SubItems[4].Text)];
                        break;
                    }
                }
            }
        }

        private void maxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //全部設為最大的權限
            for (int j = 0; j <= TreeViewSelect.SelectedNode.GetNodeCount(false) - 1; j++)
            {
                string sPrg = TreeViewSelect.SelectedNode.Name;
                string sFun = TreeViewSelect.SelectedNode.Nodes[j].Name;
                string sAuth = TreeViewSelect.SelectedNode.Nodes[j].Nodes[0].Text;

                for (int i = LVData.Items.Count - 1; i >= 0; i--)
                {
                    if (LVData.Items[i].Text == sPrg && LVData.Items[i].SubItems[1].Text == sFun)
                    {
                        TreeViewSelect.SelectedNode.Nodes[j].Tag = LVData.Items[i].SubItems[4].Text;
                        TreeViewSelect.SelectedNode.Nodes[j].Nodes[0].Text = sListAuthCht[sListAuthEng.IndexOf(LVData.Items[i].SubItems[4].Text)];
                        break;
                    }
                }
            }
        }

        private void TreeViewSelect_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeViewHitTestInfo hitTestInfo = TreeViewSelect.HitTest(e.X, e.Y);
                if (hitTestInfo != null)
                {
                    TreeViewSelect.SelectedNode = hitTestInfo.Node;
                }
            }
        }
    }
}