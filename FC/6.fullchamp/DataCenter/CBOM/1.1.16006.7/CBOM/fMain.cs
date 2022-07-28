using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SajetClass;
using SajetFilter;

namespace CBOM
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        public string g_sBOMID;

        public string g_sUserID;
        public String g_sProgram, g_sFunction;
        string sSQL;
        DataSet dsTemp;

        public void check_privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            TreeBomData.AllowDrop = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            ModifyToolStripMenuItem.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
            LVPart.AllowDrop = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            deleteToolStripMenuItem.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;

            check_privilege();
        }

        private void collapseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeBom.CollapseAll();
        }

        private void expandToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeBom.ExpandAll();
        }

        /// <summary>
        /// 找料號的所有BOM版本
        /// </summary>
        /// <param name="sPart"></param>
        public void Get_Bom_Ver(string sPart)
        {
            //找料號的所有BOM版本
            combVer.Items.Clear();
            string sSQL = "Select PART_ID, VERSION "
                       + "From SAJET.SYS_PART "
                       + "Where PART_NO = '" + sPart + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (DS.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Part No Error", 0);
                editPartNo.Focus();
                editPartNo.SelectAll();
                return;
            }
            string sPartID = DS.Tables[0].Rows[0]["PART_ID"].ToString();
            string sVer = DS.Tables[0].Rows[0]["VERSION"].ToString();

            sSQL = "Select VERSION From SAJET.SYS_BOM_INFO "
                 + "Where PART_ID = '" + sPartID + "' and enabled = 'Y' "
                 + "order by update_time desc ";
            DS = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
                combVer.Items.Add(DS.Tables[0].Rows[i]["VERSION"].ToString());

            if (combVer.FindString(sVer) == -1)
                combVer.Items.Insert(0, sVer);

            combVer.SelectedIndex = 0;
        }

        private void ShowBomDetail(string sPartNo, string sVer)
        {
            //顯示根節點
            TreeBom.Nodes.Clear();
            TreeBom.Nodes.Add(editPartNo.Text);
            TreeBom.Nodes[0].Tag = combVer.Text;
            TreeBom.Nodes[0].ImageIndex = 0;
            TreeBom.Nodes[0].SelectedImageIndex = TreeBom.Nodes[0].ImageIndex;
            TreeNode tNode = TreeBom.Nodes[0];

            //顯示所有子階料號(TreeBom)
            string sSQL = "Select PART_ID "
                        + "From SAJET.SYS_PART "
                        + "Where PART_NO = '" + sPartNo + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            string sPartID = DS.Tables[0].Rows[0]["PART_ID"].ToString();

            sSQL = "select AA.ALEVEL,BB.Part_NO,CC.PART_NO ITEM_PART_NO from "
                 + "( "
                 + "   select a.*,level ALEVEL from "
                 + "    ( "
                 + "     select b.part_id,a.item_part_id,a.bom_id,a.process_id "
                 + "     from sajet.sys_bom a,sajet.sys_bom_info b "
                 + "     where a.bom_id = b.bom_id "
                 + "     ) a "
                 + "   start with part_id = '" + sPartID + "' "
                 + "   connect by prior item_part_id = part_id "
                 + ") AA "
                 + ",sajet.sys_part BB,sajet.sys_part CC "
                 + "where AA.part_id = BB.part_id "
                 + "and AA.item_part_id = CC.part_id "
                 + "group by AA.ALEVEL,BB.Part_NO,CC.PART_NO "
                 + "order by ALEVEL,BB.part_no,CC.part_no ";
            DS = ClientUtils.ExecuteSQL(sSQL);

            TreeNode tParentNode = null;
            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                string sMainPart = DS.Tables[0].Rows[i]["Part_NO"].ToString();
                string sItemPart = DS.Tables[0].Rows[i]["ITEM_PART_NO"].ToString();
                int iLevel = Convert.ToInt32(DS.Tables[0].Rows[i]["ALEVEL"].ToString());

                TreeNode NewNode = new TreeNode();
                NewNode.Text = sItemPart;
                NewNode.ImageIndex = 2;
                NewNode.SelectedImageIndex = NewNode.ImageIndex;
                NewNode.Name = sItemPart;

                tParentNode = tNode; //第一層 
                TreeNode tParentNode1 = tNode; //第一層 
                if (iLevel == 1)
                {
                    tParentNode.Nodes.Add(NewNode);
                }
                else
                {
                    for (int j = 2; j <= iLevel - 1; j++)
                    {
                        tParentNode = tParentNode.LastNode;
                    }
                    TreeNode[] tFindNodes = tParentNode.Nodes.Find(sMainPart, true);
                    if (tFindNodes.Length > 0)
                    {
                        tParentNode = tFindNodes[0];
                        tParentNode.Nodes.Add(NewNode);
                    }
                }
            }
            tNode.Expand();
        }

        /// <summary>
        /// 顯示子階詳細資訊(右方TreeBomData&LVData)
        /// </summary>
        /// <param name="sPartNo"></param>
        /// <param name="sVer"></param>
        private void ShowPartDetail(string sPartNo, string sVer)
        {
            LVData.Items.Clear();
            g_sBOMID = "";

            //顯示子階詳細資訊(右方TreeBomData&LVData)
            string sSQL = "Select PART_ID "
                       + "From SAJET.SYS_PART "
                       + "Where PART_NO = '" + sPartNo + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            string sPartID = DS.Tables[0].Rows[0]["PART_ID"].ToString();
            //根節點===
            TreeBomData.Nodes.Clear();
            TreeBomData.Nodes.Add(sPartNo);
            TreeBomData.Nodes[0].ImageIndex = 0;
            TreeBomData.Nodes[0].SelectedImageIndex = 0;
            TreeBomData.Nodes[0].Tag = sVer;

            sSQL = " Select BOM_ID "
                 + " From SAJET.SYS_BOM_INFO "
                 + " Where PART_ID = '" + sPartID + "' and VERSION = '" + sVer + "' ";
            DS = ClientUtils.ExecuteSQL(sSQL);
            if (DS.Tables[0].Rows.Count == 0)
            {
                return;
            }
            g_sBOMID = DS.Tables[0].Rows[0]["BOM_ID"].ToString();

            //子節點===            
            string sPreProcess = "";
            string sProcess = "";
            string sPreRelation = "";
            sSQL = "Select D.PART_NO ITEM_PART_NO,B.ITEM_PART_ID, A.BOM_ID "
                 + "      ,F.PROCESS_NAME,B.ITEM_COUNT,B.VERSION,B.ITEM_GROUP "
                 + "      ,D.PART_TYPE, D.SPEC1, b.rowid, NVL(b.PROCESS_ID,0) PROCESS_ID,B.PURCHASE "
                 + " From SAJET.SYS_BOM_INFO A "
                 + "     ,SAJET.SYS_BOM B "
                 + "     ,SAJET.SYS_PART D "
                 + "     ,SAJET.SYS_PROCESS F "
                 + " Where A.PART_ID = '" + sPartID + "' and A.VERSION = '" + sVer + "' "
                 + "   and A.BOM_ID = B.BOM_ID "
                 + "   and B.ITEM_PART_ID = D.PART_ID(+) "
                 + "   and B.PROCESS_ID = F.PROCESS_ID(+) "
                 + " Order By PROCESS_NAME, B.ITEM_GROUP,ITEM_PART_NO,VERSION ";
            DS = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                sProcess = DS.Tables[0].Rows[i]["PROCESS_NAME"].ToString();
                string sItemPartNo = DS.Tables[0].Rows[i]["ITEM_PART_NO"].ToString();
                string sItemCount = DS.Tables[0].Rows[i]["ITEM_COUNT"].ToString();
                string sItemGroup = DS.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                string sSubVersion = DS.Tables[0].Rows[i]["VERSION"].ToString();
                string sPartType = DS.Tables[0].Rows[i]["PART_TYPE"].ToString();
                string sSpec1 = DS.Tables[0].Rows[i]["SPEC1"].ToString();
                string sRowID = DS.Tables[0].Rows[i]["ROWID"].ToString();
                string sProcessID = DS.Tables[0].Rows[i]["PROCESS_ID"].ToString();
                string sItemPartID = DS.Tables[0].Rows[i]["ITEM_PART_ID"].ToString();
                string sPurchase = DS.Tables[0].Rows[i]["PURCHASE"].ToString();

                if (string.IsNullOrEmpty(sProcess))
                    sProcess = "N/A";
                LVData.Items.Add(sItemPartNo);             //Item0-Part
                LVData.Items[i].SubItems.Add(sProcess);    //Item1-Process
                LVData.Items[i].SubItems.Add(sItemCount);  //Item2-Qty
                LVData.Items[i].SubItems.Add(sItemGroup);  //Item3-Relation
                LVData.Items[i].SubItems.Add(sSubVersion); //Item4-Version
                LVData.Items[i].SubItems.Add(sPartType);   //Item5-Part_Type
                LVData.Items[i].SubItems.Add(sSpec1);      //Item6-Spec
                //Location ====               
                string sLocation = "";
                string sSQL1 = " Select Location "
                             + " From SAJET.SYS_BOM_LOCATION "
                             + " Where BOM_ID = '" + g_sBOMID + "' "
                             + " And Item_Part_ID = '" + sItemPartID + "' "
                             + " ORDER BY LOCATION ";
                DataSet DS1 = ClientUtils.ExecuteSQL(sSQL1);
                for (int j = 0; j <= DS1.Tables[0].Rows.Count - 1; j++)
                {
                    sLocation = sLocation + DS1.Tables[0].Rows[j]["Location"].ToString() + ',';
                }
                String delim = ",";
                sLocation = sLocation.TrimEnd(delim.ToCharArray());
                LVData.Items[i].SubItems.Add(sLocation); //Item7 -Location
                //
                LVData.Items[i].SubItems.Add(sRowID); //Item8 -Rowid
                LVData.Items[i].SubItems.Add(sProcessID); //Item9 -Process_ID
                LVData.Items[i].SubItems.Add(sItemPartID); //Item10 -Item_Part_ID
                LVData.Items[i].SubItems.Add(""); //Item11 -UpdateFlag
                LVData.Items[i].SubItems.Add(sPurchase);  //Item12 -Purchase
                LVData.Items[i].ImageIndex = 2;

                //畫TreeView==================
                //Tree-Process
                if (sPreProcess != sProcess)
                {
                    TreeBomData.Nodes[0].Nodes.Add(sProcess);
                    TreeBomData.Nodes[0].LastNode.ImageIndex = 1;
                    TreeBomData.Nodes[0].LastNode.SelectedImageIndex = 1;
                    TreeBomData.Nodes[0].LastNode.Name = sProcess; //為了Find使用
                    sPreRelation = "";
                }
                //Tree-Part
                TreeNode tNode = new TreeNode();
                tNode.Text = sItemPartNo;
                tNode.Tag = i.ToString();  //為了與LVData對應(Tag值是LVData的Row)

                if (sItemGroup == "0" || sPreRelation != sItemGroup)
                {
                    tNode.ImageIndex = 2;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    TreeBomData.Nodes[0].LastNode.Nodes.Add(tNode);
                }
                else  //Tree-替代料
                {
                    tNode.ImageIndex = 3;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    TreeBomData.Nodes[0].LastNode.LastNode.Nodes.Add(tNode);
                }
                sPreProcess = sProcess;
                sPreRelation = sItemGroup;
            }
            TreeBomData.ExpandAll();
        }

        private void combVer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowPartDetail(editPartNo.Text, combVer.Text); //料號和Process         
            //ShowBomDetail(editPartNo.Text, combVer.Text);  //顯示所有子階料號

            editPartFilter.Focus();
        }

        private void editPartFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13)
                return;

            string sSQL = "Select Part_NO,Spec1 from Sajet.SYS_Part "
                        + "Where Part_No Like '" + editPartFilter.Text + "%'"
                        + "and enabled = 'Y' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            LVPart.Items.Clear();
            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                LVPart.Items.Add(DS.Tables[0].Rows[i]["Part_NO"].ToString());
                LVPart.Items[i].SubItems.Add(DS.Tables[0].Rows[i]["Spec1"].ToString());
                LVPart.Items[i].ImageIndex = 2;
            }
        }

        private void TreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void TreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void TreeBomData_DragDrop(object sender, DragEventArgs e)
        {
            //來源Node           
            TreeNode SrcNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
            //目的Node   
            TreeNode mNode;
            Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            mNode = ((TreeView)sender).GetNodeAt(pt);
            if (mNode == null)
            { mNode = TreeBomData.TopNode; }
            TreeBomData.Select();
            TreeBomData.Focus();

            if (SrcNode == null)  //來源是LVPart,加入新的子階料號
            {
                string sPart = TreeBomData.Nodes[0].Text;
                string sVer = TreeBomData.Nodes[0].Tag.ToString();
                string sAddPart = LVPart.SelectedItems[0].Text; //欲加入的子料
                //主件與子件相同                
                if (sPart == sAddPart)
                {
                    SajetCommon.Show_Message("Sub Part No = Main Part No", 0);
                    return;
                }

                //檢查是否會造成無窮迴圈
                if (Check_SubPartRecu(sPart, sAddPart))
                {
                    return;
                }

                if (F_AppandBomData(sPart, sVer, sAddPart, mNode))
                {
                    //無Rowid的表示新增,需Insert
                    for (int i = 0; i <= LVData.Items.Count - 1; i++)
                    {
                        if (LVData.Items[i].SubItems[11].Text == "Y")
                            Update_BOM(i);
                    }
                    TreeBomData.ExpandAll();
                    //ShowBomDetail(editPartNo.Text, combVer.Text); 
                }
            }
            else  //移動原本已有的料號
            {
                if (SrcNode.Level <= 1)
                    return;
                if (mNode.Level == 0 | mNode.Level > 2)
                    return;

                if (MoveBomData(SrcNode, mNode))
                {
                    //無Rowid的表示新增,需Insert
                    for (int i = 0; i <= LVData.Items.Count - 1; i++)
                    {
                        if (LVData.Items[i].SubItems[11].Text == "Y")
                            Update_BOM(i);
                    }
                }
            }
        }

        private bool MoveBomData(TreeNode tSrcNode, TreeNode tTargetNode)
        {
            string sTProcess = ""; //目標Process
            string sProcess = "";
            string sTProcessID = ""; //目標Process ID

            int iSrcInx = System.Convert.ToInt32(tSrcNode.Tag);
            //加入成替代料========================================================
            if (tTargetNode.Level == 2)
            {
                int iTargetInx = System.Convert.ToInt32(tTargetNode.Tag);
                sTProcess = tTargetNode.Parent.Text;
                sTProcessID = getProcessID(sTProcess);
                if (tSrcNode.Level == 2)
                    sProcess = tSrcNode.Parent.Text;
                else
                    sProcess = tSrcNode.Parent.Parent.Text;

                //Process不同
                if (sTProcess != sProcess)
                {
                    //此Process下已有此料
                    if (!CheckDup(sTProcessID, tSrcNode.Text, 0))
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                        return false;
                    }
                    //
                    if (tSrcNode.Text != tTargetNode.Text)
                    {
                        bool bResult = false;
                        for (int i = 0; i <= tTargetNode.Nodes.Count - 1; i++)
                        {
                            if (tSrcNode.Text == tTargetNode.Nodes[i].Text)
                            {
                                bResult = true;
                                break;
                            }
                        }
                        if (!bResult)
                        {
                            if (!CheckDup(sTProcessID, tSrcNode.Text, 0))
                            {
                                string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                                SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                                return false;
                            }
                        }
                    }
                }
                else   //Process相同
                {
                    if (tSrcNode.Level == 3)
                    {
                        for (int i = 0; i <= tTargetNode.Nodes.Count - 1; i++)
                        {
                            if (tSrcNode.Text == tTargetNode.Nodes[i].Text)
                            {
                                string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                                SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                                return false;
                            }
                        }
                    }
                }

                //目標原本無替代料,需改變成新的Relation號碼
                string sTargetRelation = LVData.Items[iTargetInx].SubItems[3].Text;
                if (sTargetRelation == "0")
                {
                    sTargetRelation = F_GETMAXGROUP(g_sBOMID);
                    LVData.Items[iTargetInx].SubItems[3].Text = sTargetRelation;
                    LVData.Items[iTargetInx].SubItems[11].Text = "Y";
                }
                LVData.Items[iSrcInx].SubItems[3].Text = sTargetRelation;
                LVData.Items[iSrcInx].SubItems[11].Text = "Y";
                LVData.Items[iSrcInx].SubItems[1].Text = sTProcess;
                LVData.Items[iSrcInx].SubItems[9].Text = sTProcessID;
            }
            else
            //加入成主料============================================================
            {
                sTProcess = tTargetNode.Text;
                sTProcessID = getProcessID(sTProcess);
                if (tSrcNode.Level == 2)
                {
                    sProcess = tSrcNode.Parent.Text;
                    if (sProcess == sTProcess)
                        return false;
                }
                else
                    sProcess = tSrcNode.Parent.Parent.Text;
                if (sProcess != sTProcess)
                {
                    if (!CheckDup(sTProcessID, tSrcNode.Text, 0))
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                        return false;
                    }
                }
                else
                {
                    if (!CheckDup(sTProcessID, tSrcNode.Text, 1))
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                        return false;
                    }
                }
                LVData.Items[iSrcInx].SubItems[3].Text = "0";
                LVData.Items[iSrcInx].SubItems[11].Text = "Y";
                LVData.Items[iSrcInx].SubItems[1].Text = sTProcess;
                LVData.Items[iSrcInx].SubItems[9].Text = sTProcessID;
            }

            //原本的料若無替代料,ITEM GROUP需改成0
            if (tSrcNode.Level == 3)
            {
                if (tSrcNode.Parent.Nodes.Count == 1)
                {
                    int iParantInx = System.Convert.ToInt32(tSrcNode.Parent.Tag);
                    LVData.Items[iParantInx].SubItems[3].Text = "0";
                    LVData.Items[iParantInx].SubItems[11].Text = "Y";
                }
            }

            TreeNode tNewNode = new TreeNode();
            tNewNode.Text = tSrcNode.Text;
            tNewNode.Tag = tSrcNode.Tag;
            tNewNode.ImageIndex = tTargetNode.Level + 1;
            tNewNode.SelectedImageIndex = tNewNode.ImageIndex;
            tTargetNode.Nodes.Add(tNewNode);
            tSrcNode.Remove();
            tTargetNode.Expand();
            return true;
        }

        private bool Check_SubPartRecu(string sPartNo, string sSubPartNo)
        {
            string sPartID = GET_FIELD_ID("SAJET.SYS_PART", "PART_NO", "PART_ID", sPartNo);
            string sSubPartID = GET_FIELD_ID("SAJET.SYS_PART", "PART_NO", "PART_ID", sSubPartNo);

            //檢查是否會造成遞迴
            //Ex:A->B->C->D , D下不可再加入A,B,C,否則會造成無窮迴圈  
            sSQL = "select AA.LV,BB.Part_NO,CC.PART_NO ITEMPART from "
                 + "( "
                 + "   select Level LV,part_id,item_part_id from "
                 + "   ( "
                 + "     select b.part_id,a.item_part_id "
                 + "     from sajet.sys_bom a,sajet.sys_bom_info b "
                 + "     where a.bom_id = b.bom_id "
                 + "   ) "
                 + "   start with item_part_id = '" + sPartID + "' "
                 + "   connect by prior part_id = item_part_id " //由下向上找
                 + " ) AA "
                 + "  ,sajet.sys_part BB,sajet.sys_part CC "
                 + "where AA.part_id = '" + sSubPartID + "' "
                 + "and AA.part_id = BB.part_id "
                 + "and AA.item_part_id = CC.part_id "
                 + "and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sMsg1 = SajetCommon.SetLanguage("Sub Part No Recursive", 1);
                string sMsg2 = SajetCommon.SetLanguage("Bottom Up Level", 1);
                string sMsg3 = SajetCommon.SetLanguage("Part No", 1);
                string sMsg4 = SajetCommon.SetLanguage("Sub Part No", 1);
                SajetCommon.Show_Message(sMsg1 + " !!" + Environment.NewLine
                          + sMsg2 + " : " + dsTemp.Tables[0].Rows[0]["LV"].ToString() + Environment.NewLine
                          + sMsg3 + " : " + dsTemp.Tables[0].Rows[0]["PART_NO"].ToString() + Environment.NewLine
                          + sMsg4 + " : " + dsTemp.Tables[0].Rows[0]["ITEMPART"].ToString(), 0);
                return true;
            }
            return false;
        }

        private string F_GETMAXGROUP(string sBOMID)
        {
            string sItemGroup = "0";
            int j = 0;
            sSQL = " Select distinct item_group from sajet.sys_bom "
                 + " where BOM_ID = '" + sBOMID + "' "
                 + " order by item_group desc ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            //因為Item Group有可能有非數字的,所以只找出最大的數字+1
            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                sItemGroup = DS.Tables[0].Rows[i]["item_group"].ToString();
                if (int.TryParse(sItemGroup, out j))
                    break;

            }
            sItemGroup = Convert.ToString(j + 1);
            return sItemGroup;
        }

        private string getProcessID(string sProcessName)
        {
            sSQL = "Select Process_ID from sajet.sys_process "
                 + "where process_name = '" + sProcessName + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (DS.Tables[0].Rows.Count > 0)
                return DS.Tables[0].Rows[0]["Process_ID"].ToString();
            else
                return "0";
        }

        private bool CheckDup(string sProcessID, string sItemPart, int iCount)
        {
            string sSQL = " Select count(*) sCount "
                        + " from sajet.sys_bom a, sajet.sys_part b "
                        + " where a.BOM_ID = '" + g_sBOMID + "' "
                        + " and NVL(a.Process_ID,'0') = '" + sProcessID + "' "
                        + " and a.Item_Part_ID = b.part_id "
                        + " and b.Part_No= '" + sItemPart + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (iCount.ToString() == DS.Tables[0].Rows[0]["sCount"].ToString())
                return true;
            else
                return false;
        }

        private bool F_AppandBomData(string sPart, string sVer, string sAddPart, TreeNode tNode)
        {
            string sProcess = "";
            string sCount = "";
            string sRelation = "";
            string sPartVersion = "";
            string sLocation = "";
            string sPurchase = "";
            bool bChangeGroup = false;
            int iNodeLevel = tNode.Level;

            if (iNodeLevel == 0) //新的process
            {
                sProcess = "";
                sCount = "1";
                sRelation = "0";
                sPartVersion = "";
                sLocation = "";
                sPurchase = "N";
            }
            else if (iNodeLevel == 1) //在同個的process下加主料
            {
                sProcess = tNode.Text;
                sCount = "1";
                sRelation = "0";
                sPartVersion = "";
                sLocation = "";
                sPurchase = "N";

                //料號
                string sProcessID = getProcessID(sProcess);
                if (!CheckDup(sProcessID, sAddPart, 0))
                {
                    string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                    SajetCommon.Show_Message(sMsg + Environment.NewLine + sAddPart, 0);
                    return false;
                }
            }
            else if (iNodeLevel == 2) //在Part中增加一替代料
            {
                sProcess = tNode.Parent.Text;

                if (sAddPart == tNode.Text)
                    return false;
                else
                {
                    string sProcessID = getProcessID(sProcess);
                    if (!CheckDup(sProcessID, sAddPart, 0))
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + sAddPart, 0);
                        return false;
                    }
                }

                int iIndex = System.Convert.ToInt32(tNode.Tag.ToString());
                sCount = LVData.Items[iIndex].SubItems[2].Text;
                sRelation = LVData.Items[iIndex].SubItems[3].Text;
                sPartVersion = LVData.Items[iIndex].SubItems[4].Text;
                sLocation = LVData.Items[iIndex].SubItems[7].Text;
                sPurchase = LVData.Items[iIndex].SubItems[12].Text;

                //若原本無替代料,需將group改為非0的值 
                if (sRelation == "0")
                {
                    sRelation = "";
                    bChangeGroup = true;
                }
            }
            else if (iNodeLevel == 3) //在替代料中增加一替代料
            {
                //Part重複
                for (int i = 0; i <= tNode.Parent.Nodes.Count - 1; i++)
                {
                    string sData = tNode.Parent.Nodes[i].Text;
                    if (sData == sAddPart)
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + sAddPart, 0);
                        return false;
                    }
                }

                int iIndex = System.Convert.ToInt32(tNode.Tag.ToString());
                sProcess = tNode.Parent.Parent.Text;
                sCount = LVData.Items[iIndex].SubItems[2].Text;
                sRelation = LVData.Items[iIndex].SubItems[3].Text;
                sPartVersion = LVData.Items[iIndex].SubItems[4].Text;
                sLocation = LVData.Items[iIndex].SubItems[7].Text;
                sPurchase = LVData.Items[iIndex].SubItems[12].Text;
            }
            else
            {
                return false;
            }

            fData fData = new fData();
            fData.g_sUpdateType = "Append";
            fData.g_sPartNo = sPart;
            fData.g_sVer = sVer;
            fData.g_sProcess = sProcess;
            fData.editSubPartNo.Text = sAddPart;
            fData.editQty.Text = sCount;
            fData.editSubPartVer.Text = "";//sPartVersion;
            fData.editGroup.Text = sRelation;
            fData.g_sPurchase = sPurchase;
            fData.g_sChangeGroup = bChangeGroup;
            fData.g_sBOM_ID = g_sBOMID;
            string[] split = sLocation.Split(new Char[] { ',' });
            fData.editLocation.Lines = split;

            if (iNodeLevel >= 2)
            {
                fData.editSubPartNo.Enabled = false;
                fData.combProcess.Enabled = false;
                //fData.combPurchase.Enabled = false;
                fData.editQty.Enabled = false;
                fData.editGroup.Enabled = bChangeGroup;
            }
            else if (iNodeLevel == 1)
            {
                fData.editSubPartNo.Enabled = false;
                fData.combProcess.Enabled = false;
                //fData.combPurchase.Enabled = false;
                fData.editGroup.Enabled = false;
            }
            else if (iNodeLevel == 0)
            {
                fData.editGroup.Enabled = false;
            }

            // =======Show Form==========================================================
            if (fData.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            g_sBOMID = fData.g_sBOM_ID;
            sAddPart = fData.editSubPartNo.Text;
            //若變更為有替代料關係,需同時更改GROUP
            if (bChangeGroup)
            {
                int iTag = System.Convert.ToInt32(tNode.Tag.ToString());
                LVData.Items[iTag].SubItems[3].Text = fData.editGroup.Text;
                LVData.Items[iTag].SubItems[11].Text = "Y"; //Y1表示此筆資料有更動,需Update DB
            }

            sProcess = fData.combProcess.Text.Trim();
            if (sProcess == "")
                sProcess = "N/A";
            //加入新節點===================================================
            if (iNodeLevel == 0) //需先建立Process的節點
            {
                //找是否已有此process的node  
                TreeNode[] tFindProcessNodes = TreeBomData.Nodes[0].Nodes.Find(sProcess, true);

                if (tFindProcessNodes.Length == 0)
                {
                    TreeNode tProcessNode = new TreeNode();
                    tProcessNode.Text = sProcess;
                    tProcessNode.ImageIndex = iNodeLevel + 1;
                    tProcessNode.Name = sProcess;
                    tProcessNode.SelectedImageIndex = tProcessNode.ImageIndex;
                    tNode.Nodes.Add(tProcessNode);
                    tNode = tNode.LastNode;
                }
                else
                {
                    tNode = tFindProcessNodes[0];
                }
                iNodeLevel = 1;
            }

            if (iNodeLevel == 3) //若是拖曳到替代料節點上,Tree Node建在同一層
            {
                iNodeLevel = iNodeLevel - 1;
                tNode = tNode.Parent;
            }
            int iRwoCount = LVData.Items.Count;
            TreeNode t1 = new TreeNode();
            t1.Text = sAddPart;
            t1.Tag = iRwoCount.ToString();
            t1.ImageIndex = iNodeLevel + 1;
            t1.SelectedImageIndex = t1.ImageIndex;
            tNode.Nodes.Add(t1);

            LVData.Items.Add(sAddPart);  //Item0-Part
            LVData.Items[iRwoCount].SubItems.Add(sProcess);    //Item1-Process
            LVData.Items[iRwoCount].SubItems.Add(fData.editQty.Text);        //Item2-Qty
            LVData.Items[iRwoCount].SubItems.Add(fData.editGroup.Text);      //Item3-Relation
            LVData.Items[iRwoCount].SubItems.Add(fData.editSubPartVer.Text); //Item4-Version
            LVData.Items[iRwoCount].SubItems.Add(fData.g_sItemPartType);     //Item5-Part_Type 
            LVData.Items[iRwoCount].SubItems.Add(fData.g_sItemSpec1);        //Item6-Spec
            //Location==
            sLocation = "";
            for (int j = 0; j <= fData.editLocation.Lines.Length - 1; j++)
            {
                sLocation = sLocation + fData.editLocation.Lines[j].ToString() + ',';
            }
            String delim = ",";
            sLocation = sLocation.TrimEnd(delim.ToCharArray());
            LVData.Items[iRwoCount].SubItems.Add(sLocation);  //Item7 -Location
            //==            
            LVData.Items[iRwoCount].SubItems.Add("");  //Item8 -Rowid
            LVData.Items[iRwoCount].SubItems.Add(fData.g_sProcessID);  //Item9 -Process_ID
            LVData.Items[iRwoCount].SubItems.Add(fData.g_sItemPartID);  //Item10 -Item_Part_ID
            LVData.Items[iRwoCount].SubItems.Add("Y"); //Item11 -Update Flag
            LVData.Items[iRwoCount].SubItems.Add(fData.g_sPurchase);  //Item12 -Purchase
            LVData.Items[iRwoCount].ImageIndex = 2;
            LVData.Items[iRwoCount].StateImageIndex = LVData.Items[iRwoCount].ImageIndex;
            fData.Dispose();
            return true;
        }

        private void Update_BOM(int iRow)
        {
            string sSQL = "";
            string sITEM_COUNT = LVData.Items[iRow].SubItems[2].Text;
            string sITEM_GROUP = LVData.Items[iRow].SubItems[3].Text;
            string sVERSION = LVData.Items[iRow].SubItems[4].Text;
            string sRowID = LVData.Items[iRow].SubItems[8].Text;
            string sPROCESS_ID = LVData.Items[iRow].SubItems[9].Text;
            string sITEM_PART_ID = LVData.Items[iRow].SubItems[10].Text;
            string sLocation = LVData.Items[iRow].SubItems[7].Text;
            string sPurchase = LVData.Items[iRow].SubItems[12].Text;

            if (sVERSION == "")
                sVERSION = "N/A";
            if (sRowID == "")
            {
                sSQL = " Insert Into SAJET.SYS_BOM "
                     + " (BOM_ID,ITEM_PART_ID,ITEM_GROUP,ITEM_COUNT "
                     + "  ,PROCESS_ID,VERSION,UPDATE_USERID,PURCHASE) "
                     + " Values "
                     + " ('" + g_sBOMID + "','" + sITEM_PART_ID + "','" + sITEM_GROUP + "','" + sITEM_COUNT + "' "
                     + " ,'" + sPROCESS_ID + "','" + sVERSION + "','" + g_sUserID + "','" + sPurchase + "') ";
                ClientUtils.ExecuteSQL(sSQL);

                //Insert Bom Location====
                sSQL = " Delete SAJET.SYS_BOM_LOCATION "
                     + " Where BOM_ID = '" + g_sBOMID + "' "
                     + " and Item_Part_Id = '" + sITEM_PART_ID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                if (!string.IsNullOrEmpty(sLocation))
                {
                    string[] split = sLocation.Split(new Char[] { ',' });
                    for (int i = 0; i <= split.Length - 1; i++)
                    {
                        sSQL = " Insert Into SAJET.SYS_BOM_LOCATION "
                             + " (BOM_ID,ITEM_PART_ID,ITEM_GROUP,LOCATION,UPDATE_USERID) "
                             + " Values "
                             + " ('" + g_sBOMID + "','" + sITEM_PART_ID + "','" + sITEM_GROUP + "','" + split.GetValue(i).ToString() + "' "
                             + " ,'" + g_sUserID + "') ";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }
                //找此筆RowID
                sSQL = " Select Rowid from SAJET.SYS_BOM "
                     + " Where BOM_ID = '" + g_sBOMID + "' "
                     + " and Item_Part_Id = '" + sITEM_PART_ID + "' "
                     + " and NVL(Process_ID,0) = '" + sPROCESS_ID + "' ";
                DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                LVData.Items[iRow].SubItems[8].Text = DS.Tables[0].Rows[0]["RowID"].ToString();
                sRowID = DS.Tables[0].Rows[0]["RowID"].ToString();
            }
            else
            {
                sSQL = " Update SAJET.SYS_BOM "
                     + " Set ITEM_GROUP = '" + sITEM_GROUP + "' "
                     + "   ,ITEM_COUNT = '" + sITEM_COUNT + "' "
                     + "   ,PROCESS_ID = '" + sPROCESS_ID + "' "
                     + "   ,VERSION = '" + sVERSION + "' "
                     + "   ,UPDATE_USERID = '" + g_sUserID + "' "
                     + "   ,UPDATE_TIME = SYSDATE "
                     + "   ,PURCHASE = '" + sPurchase + "' "
                     + " Where Rowid = '" + sRowID + "'";
                ClientUtils.ExecuteSQL(sSQL);
            }
            LVData.Items[iRow].SubItems[11].Text = "";
            CopyToHistory(sRowID);
        }
        private void CopyToHistory(string sRowid)
        {
            sSQL = "Insert Into SAJET.SYS_HT_BOM "
                 + "Select * from SAJET.SYS_BOM "
                 + "Where ROWID = '" + sRowid + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeBomData.CollapseAll();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            TreeBomData.ExpandAll();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeBomData.Nodes.Count == 0)
                return;
            if (TreeBomData.SelectedNode == null)
                return;

            string sSQL = "";
            int iNodeLevel = TreeBomData.SelectedNode.Level;
            if (iNodeLevel == 0) //刪除整個BOM
            {
                string sMsg = SajetCommon.SetLanguage("Delete this BOM", 1);
                if (SajetCommon.Show_Message(sMsg + " ?", 2) != DialogResult.Yes)
                    return;
                // Copy To History ==
                sSQL = " UPDATE SAJET.SYS_BOM "
                     + " SET ENABLED = 'Drop' "
                     + "    ,UPDATE_USERID = '" + g_sUserID + "' "
                     + "    ,UPDATE_TIME = SYSDATE "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                sSQL = " Insert Into SAJET.SYS_HT_BOM "
                     + " Select * from SAJET.SYS_BOM "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                //==================

                sSQL = " DELETE SAJET.SYS_BOM "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' ";
                ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.SYS_BOM_LOCATION "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' ";
                ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.SYS_BOM_INFO "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' ";
                ClientUtils.ExecuteSQL(sSQL);

            }
            else if (iNodeLevel == 1) //刪除此process下所有料
            {
                string sMsg = SajetCommon.SetLanguage("Delete all Part of this Process", 1);
                if (SajetCommon.Show_Message(sMsg + " ?", 2) != DialogResult.Yes)
                    return;

                int iRow = System.Convert.ToInt32(TreeBomData.SelectedNode.Nodes[0].Tag.ToString());
                string sProcessID = LVData.Items[iRow].SubItems[9].Text;

                for (int i = 0; i <= TreeBomData.SelectedNode.Nodes.Count - 1; i++)
                {
                    int iIndex = System.Convert.ToInt32(TreeBomData.SelectedNode.Nodes[i].Tag.ToString());
                    string sItemPartID = LVData.Items[iIndex].SubItems[10].Text;
                    string sRowID = LVData.Items[iIndex].SubItems[8].Text;
                    //此BOM中已經沒有相同的Part時,刪除Location
                    sSQL = " SELECT ITEM_PART_ID FROM SAJET.SYS_BOM "
                         + " WHERE BOM_ID = '" + g_sBOMID + "' "
                         + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                         + " AND ROWID <> '" + sRowID + "' "
                         + " AND ROWNUM=1 ";
                    DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                    if (DS.Tables[0].Rows.Count == 0)
                    {
                        sSQL = " DELETE SAJET.SYS_BOM_LOCATION "
                             + " WHERE BOM_ID = '" + g_sBOMID + "' "
                             + " AND ITEM_PART_ID = '" + sItemPartID + "' ";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }

                // Copy To History ==
                sSQL = " UPDATE SAJET.SYS_BOM "
                     + " SET ENABLED = 'Drop' "
                     + "    ,UPDATE_USERID = '" + g_sUserID + "' "
                     + "    ,UPDATE_TIME = SYSDATE "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' "
                     + " AND NVL(PROCESS_ID,'0') = '" + sProcessID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                sSQL = " Insert Into SAJET.SYS_HT_BOM "
                     + " Select * from SAJET.SYS_BOM "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' "
                     + " AND NVL(PROCESS_ID,'0') = '" + sProcessID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                //==================

                sSQL = " DELETE SAJET.SYS_BOM "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' "
                     + " AND NVL(PROCESS_ID,'0') = '" + sProcessID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
            }
            else  //刪除某一個料
            {
                string sMsg = SajetCommon.SetLanguage("Delete this Part", 1);
                if (SajetCommon.Show_Message(sMsg + " ?", 2) != DialogResult.Yes)
                    return;

                int iRow = System.Convert.ToInt32(TreeBomData.SelectedNode.Tag.ToString());
                string sProcessID = LVData.Items[iRow].SubItems[9].Text;
                string sItemPartID = LVData.Items[iRow].SubItems[10].Text;
                string sItemGroup = LVData.Items[iRow].SubItems[3].Text;

                // Copy To History ==
                sSQL = " UPDATE SAJET.SYS_BOM "
                     + " SET ENABLED = 'Drop' "
                     + "    ,UPDATE_USERID = '" + g_sUserID + "' "
                     + "    ,UPDATE_TIME = SYSDATE "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' "
                     + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                     + " AND PROCESS_ID = '" + sProcessID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                sSQL = " Insert Into SAJET.SYS_HT_BOM "
                     + " Select * from SAJET.SYS_BOM "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' "
                     + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                     + " AND PROCESS_ID = '" + sProcessID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                //==================

                sSQL = " DELETE SAJET.SYS_BOM "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' "
                     + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                     + " AND PROCESS_ID = '" + sProcessID + "' ";
                ClientUtils.ExecuteSQL(sSQL);

                //刪除後無替代料,將ITEM GROUP改為0
                if (sItemGroup != "0")
                {
                    sSQL = " SELECT COUNT(*) CNT FROM SAJET.SYS_BOM "
                         + " WHERE BOM_ID = '" + g_sBOMID + "' "
                         + " AND ITEM_GROUP = '" + sItemGroup + "' ";
                    DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                    if (DS.Tables[0].Rows[0]["CNT"].ToString() == "1")
                    {
                        sSQL = " UPDATE SAJET.SYS_BOM "
                             + " SET ITEM_GROUP = '0' "
                             + " WHERE BOM_ID = '" + g_sBOMID + "' "
                             + " AND ITEM_GROUP = '" + sItemGroup + "' ";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }

                //此BOM中已經沒有相同的Part時,刪除Location
                sSQL = " SELECT ITEM_PART_ID FROM SAJET.SYS_BOM "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' "
                     + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                     + " AND ROWNUM=1 ";
                DataSet DS1 = ClientUtils.ExecuteSQL(sSQL);
                if (DS1.Tables[0].Rows.Count == 0)
                {
                    sSQL = " DELETE SAJET.SYS_BOM_LOCATION "
                         + " WHERE BOM_ID = '" + g_sBOMID + "' "
                         + " AND ITEM_PART_ID = '" + sItemPartID + "' ";
                    ClientUtils.ExecuteSQL(sSQL);
                }
            }
            ShowPartDetail(editPartNo.Text, combVer.Text);
            //ShowBomDetail(editPartNo.Text, combVer.Text); 
        }

        private void TreeBomData_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //顯示選擇的料號細項資料
            LV1.Items.Clear();
            if (TreeBomData.SelectedNode.Level > 1)
            {
                int iIndex = Convert.ToInt32(TreeBomData.SelectedNode.Tag.ToString());
                LV1.Items.Add(LVData.Items[iIndex].Text);
                for (int i = 1; i <= 7; i++)
                {
                    LV1.Items[0].SubItems.Add(LVData.Items[iIndex].SubItems[i].Text);
                }
                LV1.Items[0].ImageIndex = 2;
                LV1.Items[0].StateImageIndex = LV1.Items[0].ImageIndex;
            }
        }

        private void TreeBomData_DragOver(object sender, DragEventArgs e)
        {
            //當移動到節點上,該節點會Focus並成藍色
            TreeNode DropNode = new TreeNode();
            Point Position = TreeBomData.PointToClient(new Point(e.X, e.Y));
            DropNode = TreeBomData.GetNodeAt(Position);
            if (DropNode != null)
            {
                TreeBomData.Focus();
                TreeBomData.SelectedNode = DropNode;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sSQL = " select part_no,spec1,spec2 "
                 + " from sajet.sys_part "
                 + " where enabled = 'Y' "
                 + " and part_no Like '" + editPartNo.Text + "%' "
                 + " Order By part_no ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            f.sPartNoMain = editPartNo.Text;
            f.sFlag = "N";
            if (f.ShowDialog() == DialogResult.OK)
            {
                editPartNo.Text = f.dgvData.CurrentRow.Cells["part_no"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editPartNo_KeyPress(sender, Key);
            }
        }

        private void editPartNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            TreeBomData.Nodes.Clear();
            LVData.Items.Clear();
            LV1.Items.Clear();
            TreeBom.Nodes.Clear();
            if (e.KeyChar == (char)Keys.Return)
            {
                Get_Bom_Ver(editPartNo.Text);
            }
        }

        public static string GET_FIELD_ID(string sTable, string sFieldName, string sFieldID, string sFieldValue)
        {
            string sSQL = " Select " + sFieldID + " FIELD_ID from " + sTable
                        + " Where " + sFieldName + " = '" + sFieldValue + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0]["FIELD_ID"].ToString();
            else
                return "0";
        }

        private void ModifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeBomData.Nodes.Count == 0)
                return;
            if (TreeBomData.SelectedNode == null)
                return;
            if (TreeBomData.SelectedNode.Level < 2)
                return;

            int iSelectRow = Convert.ToInt32(TreeBomData.SelectedNode.Tag.ToString());

            string sPart = TreeBomData.Nodes[0].Text;
            string sVer = TreeBomData.Nodes[0].Tag.ToString();
            string sProcess = LVData.Items[iSelectRow].SubItems[1].Text;
            string sAddPart = LVData.Items[iSelectRow].SubItems[0].Text;
            string sCount = LVData.Items[iSelectRow].SubItems[2].Text;
            string sPartVersion = LVData.Items[iSelectRow].SubItems[4].Text;
            string sRelation = LVData.Items[iSelectRow].SubItems[3].Text;
            string sLocation = LVData.Items[iSelectRow].SubItems[7].Text;
            string sRowID = LVData.Items[iSelectRow].SubItems[8].Text;
            string sPURCHASE = LVData.Items[iSelectRow].SubItems[12].Text;

            fData fData = new fData();
            fData.g_sPartNo = sPart;
            fData.g_sVer = sVer;
            fData.g_sProcess = sProcess;
            fData.g_sPurchase = sPURCHASE;
            fData.editSubPartNo.Text = sAddPart;
            fData.editQty.Text = sCount;
            fData.editSubPartVer.Text = sPartVersion;
            fData.editGroup.Text = sRelation;
            fData.g_sChangeGroup = false;
            fData.g_sBOM_ID = g_sBOMID;
            string[] split = sLocation.Split(new Char[] { ',' });
            fData.editLocation.Lines = split;

            fData.editSubPartNo.Enabled = false;

            fData.g_sUpdateType = "Modify";
            fData.g_sRowid = sRowID;
            // =======Show Form==========================
            if (fData.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                string sITEM_COUNT = fData.editQty.Text;
                string sITEM_GROUP = fData.editGroup.Text;
                string sVERSION = fData.editSubPartVer.Text;
                string sPROCESS_ID = fData.g_sProcessID;
                string sITEM_PART_ID = fData.g_sItemPartID;
                string sPurchase = fData.g_sPurchase;
                string[] sLocations = fData.editLocation.Lines;

                sSQL = " Update SAJET.SYS_BOM "
                     + " Set ITEM_GROUP = '" + sITEM_GROUP + "' "
                     + "    ,ITEM_COUNT = '" + sITEM_COUNT + "' "
                     + "    ,PROCESS_ID = '" + sPROCESS_ID + "' "
                     + "    ,VERSION = '" + sVERSION + "' "
                     + "    ,UPDATE_USERID = '" + g_sUserID + "' "
                     + "    ,UPDATE_TIME = SYSDATE "
                     + "    ,PURCHASE = '" + sPurchase + "' "
                     + " Where Rowid = '" + sRowID + "'";
                ClientUtils.ExecuteSQL(sSQL);
                CopyToHistory(sRowID);

                //Insert Bom Location====
                sSQL = " Delete SAJET.SYS_BOM_LOCATION "
                     + " Where BOM_ID = '" + g_sBOMID + "' "
                     + " and Item_Part_Id = '" + sITEM_PART_ID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                for (int i = 0; i <= sLocations.Length - 1; i++)
                {
                    sSQL = " Insert Into SAJET.SYS_BOM_LOCATION "
                         + " (BOM_ID,ITEM_PART_ID,ITEM_GROUP,LOCATION,UPDATE_USERID) "
                         + " Values "
                         + " ('" + g_sBOMID + "','" + sITEM_PART_ID + "','" + sITEM_GROUP + "','" + sLocations[i].ToString() + "' "
                         + " ,'" + g_sUserID + "') ";
                    ClientUtils.ExecuteSQL(sSQL);
                }

                ShowPartDetail(editPartNo.Text, combVer.Text);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception - " + ex.Message, 0);
            }
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            editPartNo.Focus();
        }
    }
}

