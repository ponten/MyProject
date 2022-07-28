using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace CWoManager
{
    public partial class fWoBom : Form
    {
        public fWoBom()
        {
            InitializeComponent();
        }
      
        public string g_sPartID;

        private void editPartFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13)
                return;

            string sSQL = "Select Part_NO,Spec1 from Sajet.SYS_Part "
                        + "Where Part_No Like '" + editPartFilter.Text + "'";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            LVPart.Items.Clear();
            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                LVPart.Items.Add(DS.Tables[0].Rows[i]["Part_NO"].ToString());
                LVPart.Items[i].SubItems.Add(DS.Tables[0].Rows[i]["Spec1"].ToString());
                LVPart.Items[i].ImageIndex = 2;
            }
        }        

        public void ShowBom(string sPartID, string sVer)
        {
            DataSet DS;
            string sSQL;                   

            LVData.Items.Clear();                    
            //根節點==================================================
            TreeBomData.Nodes.Clear();
            TreeBomData.Nodes.Add(LabPartNo.Text);
            TreeBomData.Nodes[0].ImageIndex = 0;
            TreeBomData.Nodes[0].SelectedImageIndex = 0;
            TreeBomData.Nodes[0].Tag = sVer;

            //子節點===================================================
           
            //先找是否有WO_BOM
            string sBomType = "(WO_BOM)";
            string sUpdateFlag = ""; 
            sSQL = "Select D.PART_NO ITEM_PART_NO,B.ITEM_PART_ID "
                  + ",F.PROCESS_NAME,B.ITEM_COUNT,NVL(B.VERSION,'N/A') VERSION,B.ITEM_GROUP "
                  + ",D.PART_TYPE, D.SPEC1, b.rowid bomrowid, NVL(b.PROCESS_ID,0) PROCESS_ID "
                  + "FROM SAJET.G_WO_BOM B, "
                  + "SAJET.SYS_PART D, "
                  + "SAJET.SYS_PROCESS F "
                  + "Where B.WORK_ORDER = '" + LabWO.Text + "' "
                  + "and B.PART_ID = '" + sPartID + "' "
                  + "and B.ITEM_PART_ID = D.PART_ID(+) "
                  + "and B.PROCESS_ID = F.PROCESS_ID(+) "
                  + "Order By F.PROCESS_NAME, B.ITEM_GROUP,ITEM_PART_NO ";
            DS = ClientUtils.ExecuteSQL(sSQL);         
            if (DS.Tables[0].Rows.Count == 0)
            {                
                sBomType = "(Default)";
                sUpdateFlag = "Y";
                sSQL = "Select D.PART_NO ITEM_PART_NO,B.ITEM_PART_ID, A.BOM_ID "
                 + "      ,F.PROCESS_NAME,B.ITEM_COUNT,NVL(B.VERSION,'N/A') VERSION,B.ITEM_GROUP "
                 + "      ,D.PART_TYPE, D.SPEC1, '' bomrowid, NVL(b.PROCESS_ID,0) PROCESS_ID "
                 + "      ,A.BOM_ID "
                 + " From SAJET.SYS_BOM_INFO A "
                 + "     ,SAJET.SYS_BOM B "
                 + "     ,SAJET.SYS_PART D "
                 + "     ,SAJET.SYS_PROCESS F "
                 + " Where A.PART_ID = '" + sPartID + "' and A.VERSION = '" + sVer + "' "
                 + "   and A.BOM_ID = B.BOM_ID "
                 + "   and B.ITEM_PART_ID = D.PART_ID(+) "
                 + "   and B.PROCESS_ID = F.PROCESS_ID(+) "
                 + " Order By F.PROCESS_NAME, B.ITEM_GROUP,ITEM_PART_NO ";
                DS = ClientUtils.ExecuteSQL(sSQL);
                if (DS.Tables[0].Rows.Count > 0)
                {
                    //若無WO_BOM,則顯示SYS_BOM,且無法修改
                    //須先Save工單將BOM Copy到WO BOM後,才可修改,否則在刪除或移動時會有問題
                    TreeBomData.AllowDrop = false;
                    LVPart.AllowDrop = false;
                    MenuItemDelete.Visible = false;
                }
            }
            LabBomType.Text = SajetCommon.SetLanguage(sBomType);
            string sPreProcess = "";
            string sProcess = "";
            string sPreRelation = "";            

            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                sProcess = DS.Tables[0].Rows[i]["PROCESS_NAME"].ToString();
                string sItemPartNo = DS.Tables[0].Rows[i]["ITEM_PART_NO"].ToString();
                string sItemCount = DS.Tables[0].Rows[i]["ITEM_COUNT"].ToString();
                string sItemGroup = DS.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                string sSubVersion = DS.Tables[0].Rows[i]["VERSION"].ToString();
                string sPartType = DS.Tables[0].Rows[i]["PART_TYPE"].ToString();
                string sSpec1 = DS.Tables[0].Rows[i]["SPEC1"].ToString();
                string sRowID = DS.Tables[0].Rows[i]["bomrowid"].ToString();
                string sProcessID = DS.Tables[0].Rows[i]["PROCESS_ID"].ToString();
                string sItemPartID = DS.Tables[0].Rows[i]["ITEM_PART_ID"].ToString();

                if (string.IsNullOrEmpty(sProcess))
                    sProcess = "N/A";
                LVData.Items.Add(sItemPartNo);             //Item0-Part
                LVData.Items[i].SubItems.Add(sProcess);    //Item1-Process
                LVData.Items[i].SubItems.Add(sItemCount);  //Item2-Qty
                LVData.Items[i].SubItems.Add(sItemGroup);  //Item3-Relation
                LVData.Items[i].SubItems.Add(sSubVersion); //Item4-Version
                LVData.Items[i].SubItems.Add(sPartType);   //Item5-Part_Type
                LVData.Items[i].SubItems.Add(sSpec1);      //Item6-Spec
                //Location ==============================                               
                string sLocation = "";
                DataSet DSLoc;
                if (sBomType == "(Default)")
                {
                    string sBOMID = DS.Tables[0].Rows[i]["BOM_ID"].ToString();
                    string sSQL1 = " Select Location "
                                 + " From SAJET.SYS_BOM_LOCATION "
                                 + " Where BOM_ID = '" + sBOMID + "' "
                                 + " And Item_Part_ID = '" + sItemPartID + "' "
                                 + " ORDER BY LOCATION ";
                    DSLoc = ClientUtils.ExecuteSQL(sSQL1);
                }
                else
                {
                    string sSQL1 = " Select Location "
                                 + " From SAJET.G_WO_BOM_LOCATION "
                                 + " Where WORK_ORDER = '" + LabWO.Text + "' "
                                 + " And Item_Part_ID = '" + sItemPartID + "' "
                                 + " ORDER BY LOCATION ";
                    DSLoc = ClientUtils.ExecuteSQL(sSQL1);
                }
                for (int j = 0; j <= DSLoc.Tables[0].Rows.Count - 1; j++)
                    sLocation = sLocation + DSLoc.Tables[0].Rows[j]["Location"].ToString() + ',';              
                String delim = ",";
                sLocation = sLocation.TrimEnd(delim.ToCharArray());
                LVData.Items[i].SubItems.Add(sLocation); //Item7 -Location
                //===========================================

                LVData.Items[i].SubItems.Add(sRowID);      //Item8 -Rowid
                LVData.Items[i].SubItems.Add(sProcessID);  //Item9 -Process_ID
                LVData.Items[i].SubItems.Add(sItemPartID); //Item10 -Item_Part_ID           
                LVData.Items[i].SubItems.Add(sUpdateFlag); //Item11 -UpdateFlag     
                LVData.Items[i].ImageIndex = 2;

                //畫TreeView================================================
                //Tree-Process
                if (sPreProcess != sProcess)
                {
                    TreeBomData.Nodes[0].Nodes.Add(sProcess);
                    TreeBomData.Nodes[0].LastNode.ImageIndex = 1;
                    TreeBomData.Nodes[0].LastNode.SelectedImageIndex = 1;
                    TreeBomData.Nodes[0].LastNode.Name = sProcess;
                    sPreRelation = "";
                }
                //Tree-Part
                TreeNode tNode = new TreeNode
                {
                    Text = sItemPartNo,
                    Tag = i.ToString()  //為了與LVData對應(Tag值是LVData的Row)
                };

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
                mNode = TreeBomData.TopNode; 
            TreeBomData.Select();
            TreeBomData.Focus();

            if (SrcNode == null)  //來源是LVPart,加入新的子階料號
            {
                string sPart = TreeBomData.Nodes[0].Text;
                string sVer = TreeBomData.Nodes[0].Tag.ToString();
                string sAddPart = LVPart.SelectedItems[0].Text; //欲加入的子料

                if (F_AppandBomData(sPart, sVer, sAddPart, mNode))
                {
                    //無Rowid的表示新增,需Insert
                    for (int i = 0; i <= LVData.Items.Count - 1; i++)
                    {
                        if (LVData.Items[i].SubItems[11].Text == "Y")
                            Update_BOM(i);
                    }
                    TreeBomData.ExpandAll();
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

        private bool F_AppandBomData(string sPart, string sVer, string sAddPart, TreeNode tNode)
        {
            string sProcess = "";
            string sCount = "";
            string sRelation = "";
            string sPartVersion = "";
            string sLocation = "";
            bool bChangeGroup = false;
            int iNodeLevel = tNode.Level;

            if (iNodeLevel == 0) //新的process
            {
                sProcess = "";
                sCount = "1";
                sRelation = "0";
                sPartVersion = "";
                sLocation = "";
            }
            else if (iNodeLevel == 1) //在同個的process下加主料
            {
                sProcess = tNode.Text;
                sCount = "1";
                sRelation = "0";
                sPartVersion = "";
                sLocation = "";

                //料號
                string sProcessID =  getProcessID(sProcess);
                if (!CheckDup(sProcessID, sAddPart, 0))
                {
                    string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate");
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
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate");
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + sAddPart, 0);
                        return false;     
                    }
                }

                int iIndex = System.Convert.ToInt32(tNode.Tag.ToString());
                sCount = LVData.Items[iIndex].SubItems[2].Text;
                sRelation = LVData.Items[iIndex].SubItems[3].Text;
                sPartVersion = LVData.Items[iIndex].SubItems[4].Text;
                sLocation = LVData.Items[iIndex].SubItems[7].Text;

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
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate");
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
            }
            else
            {
                return false;
            }

            fBomData fBomData = new fBomData();
            fBomData.LabWorkOrder.Text = LabWO.Text;
            fBomData.LabPart.Text = sPart;
            fBomData.LabVer.Text = sVer;
            fBomData.g_sSelectProcess = sProcess;
            fBomData.editSubPart.Text = sAddPart;           
            fBomData.editQty.Text = sCount;
            fBomData.editSubPartVer.Text = "";//sPartVersion;
            fBomData.editGroup.Text = sRelation;
            fBomData.g_sChangeGroup = bChangeGroup;            
            string[] split = sLocation.Split(new Char[] { ',' });
            fBomData.editLocation.Lines = split;

            if (iNodeLevel >= 2)
            {
                fBomData.editSubPart.Enabled = false;
                fBomData.combProcess.Enabled = false;
                fBomData.editQty.Enabled = false;
                fBomData.editGroup.Enabled = bChangeGroup;
            }
            else if (iNodeLevel == 1)
            {
                fBomData.editSubPart.Enabled = false;
                fBomData.combProcess.Enabled = false;
                fBomData.editGroup.Enabled = false;
            }
            else if (iNodeLevel == 0)
            {
                fBomData.editGroup.Enabled = false;
            }

            // =======Show Form==========================================================
            if (fBomData.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            sAddPart = fBomData.editSubPart.Text;
            //若變更為有替代料關係,需同時更改GROUP
            if (bChangeGroup)
            {
                int iTag = System.Convert.ToInt32(tNode.Tag.ToString());
                LVData.Items[iTag].SubItems[3].Text = fBomData.editGroup.Text;
                LVData.Items[iTag].SubItems[11].Text = "Y"; //Y1表示此筆資料有更動,需Update DB
            }

            sProcess = fBomData.combProcess.Text.Trim();
            if (sProcess == "")
                sProcess = "N/A";
            //加入新節點===================================================
            if (iNodeLevel == 0) //需先建立Process的節點
            {
                //找是否已有此process的node  
                TreeNode[] tFindProcessNodes = TreeBomData.Nodes[0].Nodes.Find(sProcess, false);

                if (tFindProcessNodes.Length == 0)
                {
                    TreeNode tProcessNode = new TreeNode
                    {
                        Text = sProcess,
                        ImageIndex = iNodeLevel + 1
                    };
                    tProcessNode.SelectedImageIndex = tProcessNode.ImageIndex;
                    tProcessNode.Name = sProcess;
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
            TreeNode t1 = new TreeNode
            {
                Text = sAddPart,
                Tag = iRwoCount.ToString(),
                ImageIndex = iNodeLevel + 1
            };
            t1.SelectedImageIndex = t1.ImageIndex;
            tNode.Nodes.Add(t1);

            LVData.Items.Add(sAddPart);  //Item0-Part
            LVData.Items[iRwoCount].SubItems.Add(sProcess);    //Item1-Process
            LVData.Items[iRwoCount].SubItems.Add(fBomData.editQty.Text);        //Item2-Qty
            LVData.Items[iRwoCount].SubItems.Add(fBomData.editGroup.Text);      //Item3-Relation
            LVData.Items[iRwoCount].SubItems.Add(fBomData.editSubPartVer.Text); //Item4-Version
            LVData.Items[iRwoCount].SubItems.Add(fBomData.g_sItemPartType);     //Item5-Part_Type 
            LVData.Items[iRwoCount].SubItems.Add(fBomData.g_sItemSpec1);        //Item6-Spec
            //Location==
            sLocation = "";
            for (int j = 0; j <= fBomData.editLocation.Lines.Length - 1; j++)
            {
                sLocation = sLocation + fBomData.editLocation.Lines[j].ToString() + ',';
            }
            String delim = ",";
            sLocation = sLocation.TrimEnd(delim.ToCharArray());
            LVData.Items[iRwoCount].SubItems.Add(sLocation);  //Item7 -Location
            //==            
            LVData.Items[iRwoCount].SubItems.Add("");  //Item8 -Rowid
            LVData.Items[iRwoCount].SubItems.Add(fBomData.g_sProcessID);  //Item9 -Process_ID
            LVData.Items[iRwoCount].SubItems.Add(fBomData.g_sItemPartID);  //Item10 -Item_Part_ID
            LVData.Items[iRwoCount].SubItems.Add("Y"); //Item11 -Update Flag
            LVData.Items[iRwoCount].ImageIndex = 2;
            LVData.Items[iRwoCount].StateImageIndex = LVData.Items[iRwoCount].ImageIndex;
            fBomData.Dispose();
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

            if (sVERSION == "")
                sVERSION = "N/A";
            if (sRowID == "")
            {
                sSQL = " Insert Into SAJET.G_WO_BOM "
                     + " (WORK_ORDER,PART_ID,ITEM_PART_ID,ITEM_GROUP,ITEM_COUNT "
                     + "  ,PROCESS_ID,VERSION,UPDATE_USERID) "
                     + " Values "
                     + " ('" + LabWO.Text + "','" + g_sPartID + "','" + sITEM_PART_ID + "','" + sITEM_GROUP + "','" + sITEM_COUNT + "' "
                     + " ,'" + sPROCESS_ID + "','" + sVERSION + "','" + ClientUtils.UserPara1 + "') ";
                ClientUtils.ExecuteSQL(sSQL);

                //Insert Bom Location====
                sSQL = " Delete SAJET.G_WO_BOM_LOCATION "
                     + " Where WORK_ORDER = '" + LabWO.Text + "' "
                     + " and Item_Part_Id = '" + sITEM_PART_ID + "' ";
                ClientUtils.ExecuteSQL(sSQL);

                string[] split = sLocation.Split(new Char[] { ',' });
                for (int i = 0; i <= split.Length - 1; i++)
                {
                    sSQL = " Insert Into SAJET.G_WO_BOM_LOCATION "
                         + " (WORK_ORDER,PART_ID,ITEM_PART_ID,ITEM_GROUP,LOCATION,UPDATE_USERID) "
                         + " Values "
                         + " ('" + LabWO.Text + "','" + g_sPartID + "','" + sITEM_PART_ID + "','" + sITEM_GROUP + "','" + split.GetValue(i).ToString() + "' "
                         + " ,'" + ClientUtils.UserPara1 + "') ";
                    ClientUtils.ExecuteSQL(sSQL);
                }

                //找此筆RowID
                sSQL = " Select Rowid from SAJET.G_WO_BOM "
                     + " Where WORK_ORDER = '" + LabWO.Text + "' "
                     + " and Item_Part_Id = '" + sITEM_PART_ID + "' "
                     + " and NVL(Process_ID,0) = '" + sPROCESS_ID + "' ";
                DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                LVData.Items[iRow].SubItems[8].Text = DS.Tables[0].Rows[0]["RowID"].ToString();
            }
            else
            {
                sSQL = " Update SAJET.G_WO_BOM "
                     + " Set ITEM_GROUP = '" + sITEM_GROUP + "' "
                     + "   ,ITEM_COUNT = '" + sITEM_COUNT + "' "
                     + "   ,PROCESS_ID = '" + sPROCESS_ID + "' "
                     + "   ,VERSION = '" + sVERSION + "' "
                     + "   ,UPDATE_USERID = '" + ClientUtils.UserPara1 + "' "
                     + "   ,UPDATE_TIME = SYSDATE "
                     + " Where Rowid = '" + sRowID + "'";
                ClientUtils.ExecuteSQL(sSQL);
            }
            LVData.Items[iRow].SubItems[11].Text = ""; ;
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
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate");
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
                                string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate");
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
                                string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate");
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
                    sTargetRelation = F_GETMAXGROUP(LabWO.Text);
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
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate");
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);                        
                        return false;
                    }
                }
                else
                {
                    if (!CheckDup(sTProcessID, tSrcNode.Text, 1))
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate");
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

            TreeNode tNewNode = new TreeNode
            {
                Text = tSrcNode.Text,
                Tag = tSrcNode.Tag,
                ImageIndex = tTargetNode.Level + 1
            };
            tNewNode.SelectedImageIndex = tNewNode.ImageIndex;
            tTargetNode.Nodes.Add(tNewNode);
            tSrcNode.Remove();
            tTargetNode.Expand();
            return true;
        }

        private string getProcessID(string sProcessName)
        {
            string sSQL = "Select Process_ID from sajet.sys_process "
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
                        + " from sajet.g_wo_bom a, sajet.sys_part b "
                        + " where a.work_order = '" + LabWO.Text + "' "
                        + " and NVL(a.Process_ID,'0') = '" + sProcessID + "' "
                        + " and a.Item_Part_ID = b.part_id "
                        + " and b.Part_No= '" + sItemPart + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (iCount.ToString() == DS.Tables[0].Rows[0]["sCount"].ToString())
                return true;
            else
                return false;
        }

        private string F_GETMAXGROUP(string sWO)
        {
            string sSQL = "Select MAX(ITEM_GROUP)+1 ITEM_GROUP from sajet.g_wo_bom "
                        + "where work_order = '" + sWO + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            string sItemGroup = DS.Tables[0].Rows[0]["ITEM_GROUP"].ToString();
            return sItemGroup;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeBomData.CollapseAll();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            TreeBomData.ExpandAll();
        }

        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            string sSQL = "";
            int iNodeLevel = TreeBomData.SelectedNode.Level;
            if (iNodeLevel == 0) //刪除整個BOM
            {
                string sMsg = SajetCommon.SetLanguage("Delete this WO BOM");
                if (SajetCommon.Show_Message(sMsg + " ?", 2) != DialogResult.Yes)
                    return;

                sSQL = " DELETE SAJET.G_WO_BOM "
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.G_WO_BOM_LOCATION "
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            else if (iNodeLevel == 1) //刪除此process下所有料
            {
                string sMsg = SajetCommon.SetLanguage("Delete all Part of this Process");
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
                    sSQL = " SELECT ITEM_PART_ID FROM SAJET.G_WO_BOM "
                         + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                         + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                         + " AND ROWID <> '" + sRowID + "' "
                         + " AND ROWNUM=1 ";
                    DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                    if (DS.Tables[0].Rows.Count == 0)
                    {
                        sSQL = " DELETE SAJET.G_WO_BOM_LOCATION "
                             + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                             + " AND ITEM_PART_ID = '" + sItemPartID + "' ";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }

                sSQL = " DELETE SAJET.G_WO_BOM "
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                     + " AND NVL(PROCESS_ID,'0') = '" + sProcessID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
            }
            else  //刪除某一個料
            {
                string sMsg = SajetCommon.SetLanguage("Delete this Part");
                if (SajetCommon.Show_Message(sMsg + " ?", 2) != DialogResult.Yes)
                    return;

                int iRow = System.Convert.ToInt32(TreeBomData.SelectedNode.Tag.ToString());
                string sProcessID = LVData.Items[iRow].SubItems[9].Text;
                string sItemPartID = LVData.Items[iRow].SubItems[10].Text;
                string sItemGroup = LVData.Items[iRow].SubItems[3].Text;

                sSQL = " DELETE SAJET.G_WO_BOM "
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                     + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                     + " AND PROCESS_ID = '" + sProcessID + "' ";
                ClientUtils.ExecuteSQL(sSQL);

                //刪除後無替代料,將ITEM GROUP改為0
                if (sItemGroup != "0")
                {
                    sSQL = " SELECT COUNT(*) CNT FROM SAJET.G_WO_BOM "
                         + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                         + " AND ITEM_GROUP = '" + sItemGroup + "' ";
                    DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                    if (DS.Tables[0].Rows[0]["CNT"].ToString() == "1")
                    {
                        sSQL = " UPDATE SAJET.G_WO_BOM "
                             + " SET ITEM_GROUP = '0' "
                             + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                             + " AND ITEM_GROUP = '" + sItemGroup + "' ";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }

                //此BOM中已經沒有相同的Part時,刪除Location
                sSQL = " SELECT ITEM_PART_ID FROM SAJET.G_WO_BOM "
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                     + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                     + " AND ROWNUM=1 ";
                DataSet DS1 = ClientUtils.ExecuteSQL(sSQL);
                if (DS1.Tables[0].Rows.Count == 0)
                {
                    sSQL = " DELETE SAJET.G_WO_BOM_LOCATION "
                         + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                         + " AND ITEM_PART_ID = '" + sItemPartID + "' ";
                    ClientUtils.ExecuteSQL(sSQL);
                }
            }
            ShowBom(g_sPartID, LabVer.Text);
        }        

        private void TreeBomData_DragOver(object sender, DragEventArgs e)
        {
            TreeNode DropNode = new TreeNode();
            Point Position = TreeBomData.PointToClient(new Point(e.X, e.Y));
            DropNode = TreeBomData.GetNodeAt(Position);
            if (DropNode != null)
            {
                TreeBomData.Focus();
                TreeBomData.SelectedNode = DropNode;
            }  
        }

        private void fWoBom_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
        }        
    }
}