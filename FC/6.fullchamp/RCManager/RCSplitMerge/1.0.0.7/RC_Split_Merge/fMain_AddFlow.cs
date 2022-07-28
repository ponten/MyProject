using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using Lassalle.Flow;
using Lassalle.Flow.Layout.Hierarchic;
using SajetClass;

namespace RC_Split_Merge
{
    partial class fMain
    {
        Lassalle.Flow.AddFlow addflowPanel;
        Lassalle.Flow.Layout.Hierarchic.HFlow hflow = null;
        AddflowZoneParameter azp;
        NodeParameter np;
        DataTable dt;
        

        private void InitializeAddFlow()
        {
            if (addflowPanel == null)
                addflowPanel = new AddFlow();
            else
            addflowPanel.Nodes.Clear();

            azp = new AddflowZoneParameter();
            np = new NodeParameter();
            panel2.Controls.Add(addflowPanel);
            addflowPanel.Dock = DockStyle.Fill;
            addflowPanel.DefLinkProp.Jump = Jump.Arc;
            addflowPanel.AutoScroll = true;
            addflowPanel.BackColor = Color.White;
            addflowPanel.CanDragScroll = true;
            addflowPanel.CanMoveNode = true;
            addflowPanel.CanLabelEdit = false;
            addflowPanel.CanDrawLink = false;
            addflowPanel.CanDrawNode = false;
            addflowPanel.CanSizeNode = false;
            addflowPanel.CanStretchLink = false;
            addflowPanel.Zoom = new Zoom(1, 1);
        }

        private void CreateDiagram(DataTable flowTable)
        {
            //flowTable.WriteXml("F:\\addflowText.xml");
            Node fromN;
            Node toN;
            Link linkI;
            
            PointF LastLocation = azp.InitializeLocation;
            List<string> ls = new List<string>();
            
            string Action = "", ActionType = "", FromRC = "", FromRCQty = "", ToRC = "", ToRCQty = "";
            CreateDt();

            for (int i = 0; i < flowTable.Rows.Count; i++)
            {
                string sTravelId = flowTable.Rows[i][SajetCommon.SetLanguage("Travel ID")].ToString();
                string sActionType = flowTable.Rows[i][SajetCommon.SetLanguage("ActionType")].ToString();
                string stfRC;
                int s = 0;
                if ("M".Equals(sActionType))
                {
                    stfRC = flowTable.Rows[i][SajetCommon.SetLanguage("To R/C")].ToString();
                }
                else
                {
                    stfRC = flowTable.Rows[i][SajetCommon.SetLanguage("From R/C")].ToString();
                }
                if (!ls.Contains(sTravelId) || !ls.Contains(stfRC))
                {
                    ls.Add(sTravelId);
                    ls.Add(stfRC);
                }
            }
            //string [] ss=new string [100];
            int iLength = ls.Count;
            int sCN = 0;
            for (int i = 0; i < iLength; i+=2)
            {
                string sLs = ls[i].ToString();
                string sLs1 = ls[i + 1].ToString();
                string str = "";
                int cnn = 0;
                
                
                for (int j = sCN; j < flowTable.Rows.Count; j++)
                {
                    string sActionType = flowTable.Rows[j][SajetCommon.SetLanguage("ActionType")].ToString();
                    string sTravelId = flowTable.Rows[j][SajetCommon.SetLanguage("Travel ID")].ToString();
                    string stfRC;
                    if ("M".Equals(sActionType))
                    {
                        stfRC = flowTable.Rows[j][SajetCommon.SetLanguage("To R/C")].ToString();
                    }
                    else
                    {
                        stfRC = flowTable.Rows[j][SajetCommon.SetLanguage("From R/C")].ToString();
                    }
                    //if (sSEQ != i.ToString())
                    if (sTravelId != sLs || stfRC !=sLs1)
                    {                        
                        str = "Next";
                        break;
                    }
                    sCN++;
                    Action = flowTable.Rows[j][SajetCommon.SetLanguage("Action")].ToString();
                    ActionType = flowTable.Rows[j][SajetCommon.SetLanguage("ActionType")].ToString();

                    FromRC = flowTable.Rows[j][SajetCommon.SetLanguage("From R/C")].ToString();
                    FromRCQty = flowTable.Rows[j][SajetCommon.SetLanguage("From Qty")].ToString();
                    ToRC = flowTable.Rows[j][SajetCommon.SetLanguage("To R/C")].ToString();
                    ToRCQty = flowTable.Rows[j][SajetCommon.SetLanguage("To Qty")].ToString();


                    dt.Rows.Add(new string[]{
                                            FromRC,
                                            FromRCQty,
                                            ToRC,
                                            ToRCQty
                    });

                }

                
                int iCount = dt.Rows.Count;
                int n = -1;
                string m_sRC;
                if ("M".Equals(ActionType))
                {
                    if (!azp.d_LastNode.ContainsKey(dt.Rows[0]["To R/C"].ToString())) //1.先創右边的NODE
                    {
                        if (dt.Rows[0]["To R/C"].ToString() == sRC)
                        {
                            toN = CreateNode(dt.Rows[0]["To R/C"].ToString(), dt.Rows[0]["To Qty"].ToString());
                            toN.DrawColor = toN.FillColor = Color.LightGreen;
                            toN.TextColor = Color.Black;
                        }
                        else
                        {
                            toN = CreateNode(dt.Rows[0]["To R/C"].ToString(), dt.Rows[0]["To Qty"].ToString());
                        }
                    }
                    else
                    {
                        if (dt.Rows[0]["To R/C"].ToString() == sRC)
                        {
                            toN = CreateNode(dt.Rows[0]["To R/C"].ToString(), dt.Rows[0]["To Qty"].ToString());
                            toN.DrawColor = toN.FillColor = Color.LightGreen;
                            toN.TextColor = Color.Black;
                        }
                        else
                        {
                            toN = CreateNode(dt.Rows[0]["To R/C"].ToString(), dt.Rows[0]["To Qty"].ToString());
                        }
                    }

                    for (int j = 0; j < iCount; j++)
                    { 
                        m_sRC = dt.Rows[j]["From R/C"].ToString();
                        if (n != -1)
                            break;
                        if (m_sRC == dt.Rows[0]["To R/C"].ToString())
                        {
                            n = j;

                            if (!azp.d_LastNode.ContainsKey(dt.Rows[n]["From R/C"].ToString())) //第二項來源
                            {
                                if (dt.Rows[n]["From R/C"].ToString() == sRC)
                                {
                                    fromN = CreateNode(dt.Rows[n]["From R/C"].ToString(), dt.Rows[n]["From Qty"].ToString());//建立合併的另一個RC單
                                    fromN.DrawColor = fromN.FillColor = Color.LightGreen;
                                    fromN.TextColor = Color.Black;
                                }
                                else
                                {
                                    fromN = CreateNode(dt.Rows[n]["From R/C"].ToString(), dt.Rows[n]["From Qty"].ToString());
                                }
                            }
                            else
                            {
                                if (dt.Rows[n]["From R/C"].ToString() == sRC)
                                {
                                    fromN = azp.d_LastNode[dt.Rows[n]["From R/C"].ToString()];
                                    fromN.DrawColor = fromN.FillColor = Color.LightGreen;
                                    fromN.TextColor = Color.Black;
                                }
                                else
                                {
                                    fromN = azp.d_LastNode[dt.Rows[n]["From R/C"].ToString()];
                                }
                            } 
                            AddNode(dt.Rows[n]["From R/C"].ToString(), dt.Rows[n]["From Qty"].ToString(), fromN);
                            addflowPanel.Nodes.Add(fromN);
                            linkI = drawLink(Action);
                            //linkII = drawLink(Action);
                            addflowPanel.AddLink(linkI, fromN, toN);
                            //addflowPanel.AddLink(linkII, fromN_II, toN);
                            break;
                        }
                    }
                        if (n == -1)
                        {
                        for (int k = 0; k < iCount; k++)
                        {
                       
                        if (!azp.d_LastNode.ContainsKey(dt.Rows[k]["From R/C"].ToString())) //第二項來源
                        {
                            if (dt.Rows[k]["From R/C"].ToString() == sRC)
                            {
                                fromN = CreateNode(dt.Rows[k]["From R/C"].ToString(), dt.Rows[k]["From Qty"].ToString());//建立合併的另一個RC單
                                fromN.DrawColor = fromN.FillColor = Color.LightGreen;
                                fromN.TextColor = Color.Black;
                            }
                            else
                            {
                                fromN = CreateNode(dt.Rows[k]["From R/C"].ToString(), dt.Rows[k]["From Qty"].ToString());
                            }
                        }
                        else
                        {
                            if (dt.Rows[k]["From R/C"].ToString() == sRC)
                            {
                                fromN = azp.d_LastNode[dt.Rows[k]["From R/C"].ToString()];
                                fromN.DrawColor = fromN.FillColor = Color.LightGreen;
                                fromN.TextColor = Color.Black;
                            }
                            else
                            {
                                fromN = azp.d_LastNode[dt.Rows[k]["From R/C"].ToString()];
                            }
                        }
                        if (k > 0)
                        {
                            AddNode(dt.Rows[0]["To R/C"].ToString(), dt.Rows[0]["To Qty"].ToString(), toN);
                        }
                        AddNode(dt.Rows[k]["From R/C"].ToString(), dt.Rows[k]["From Qty"].ToString(), fromN);
                        addflowPanel.Nodes.Add(fromN);
                        linkI = drawLink(Action);
                        //linkII = drawLink(Action);
                        addflowPanel.AddLink(linkI, fromN, toN);
                        //addflowPanel.AddLink(linkII, fromN_II, toN);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < iCount; k++)
                        {
                            if (k == n)
                                continue;
                            if (!azp.d_LastNode.ContainsKey(dt.Rows[k]["From R/C"].ToString())) //第二項來源
                            {
                                if (dt.Rows[k]["From R/C"].ToString() == sRC)
                                {
                                    fromN = CreateNode(dt.Rows[k]["From R/C"].ToString(), dt.Rows[k]["From Qty"].ToString());//建立合併的另一個RC單
                                    fromN.DrawColor = fromN.FillColor = Color.LightGreen;
                                    fromN.TextColor = Color.Black;
                                }
                                else
                                {
                                    fromN = CreateNode(dt.Rows[k]["From R/C"].ToString(), dt.Rows[k]["From Qty"].ToString());
                                }
                            }
                            else
                            {
                                if (dt.Rows[k]["From R/C"].ToString() == sRC)
                                {
                                    fromN = azp.d_LastNode[dt.Rows[k]["From R/C"].ToString()];
                                    fromN.DrawColor = fromN.FillColor = Color.LightGreen;
                                    fromN.TextColor = Color.Black;
                                }
                                else
                                {
                                    fromN = azp.d_LastNode[dt.Rows[k]["From R/C"].ToString()];
                                }
                            }
                            //if (k > 0)
                            //{
                                AddNode(dt.Rows[0]["To R/C"].ToString(), dt.Rows[0]["To Qty"].ToString(), toN);
                            //}
                            AddNode(dt.Rows[k]["From R/C"].ToString(), dt.Rows[k]["From Qty"].ToString(), fromN);
                            addflowPanel.Nodes.Add(fromN);
                            linkI = drawLink(Action);
                            //linkII = drawLink(Action);
                            addflowPanel.AddLink(linkI, fromN, toN);
                            //addflowPanel.AddLink(linkII, fromN_II, toN);
                        }
                    } 
               } 
                else  //拆批
                {
                    if (!azp.d_LastNode.ContainsKey(dt.Rows[0]["From R/C"].ToString())) //1.先創左邊的NODE
                    {
                        if (dt.Rows[0]["From R/C"].ToString() == sRC)
                        {
                            fromN = CreateNode(dt.Rows[0]["From R/C"].ToString(), dt.Rows[0]["From Qty"].ToString());
                            fromN.DrawColor = fromN.FillColor = Color.LightGreen;
                            fromN.TextColor = Color.Black;
                        }
                        else
                        {
                            fromN = CreateNode(dt.Rows[0]["From R/C"].ToString(), dt.Rows[0]["From Qty"].ToString());
                        }
                    }
                    else
                    {
                        if (dt.Rows[0]["From R/C"].ToString() == sRC)
                        {
                            fromN = azp.d_LastNode[dt.Rows[0]["From R/C"].ToString()];
                            fromN.DrawColor = fromN.FillColor = Color.LightGreen;
                            fromN.TextColor = Color.Black;
                        }
                        else
                        {
                            fromN = azp.d_LastNode[dt.Rows[0]["From R/C"].ToString()];
                        }
                    }
                    if (cnn < 1)
                    {
                        AddNode(dt.Rows[0]["From R/C"].ToString(), dt.Rows[0]["From Qty"].ToString(), fromN);
                    }

                    for (int j = 0; j < iCount; j++)
                    {
                        m_sRC = dt.Rows[j]["To R/C"].ToString();
                        if (n != -1)
                            break;
                        if (m_sRC == dt.Rows[0]["From R/C"].ToString())
                        {
                            n = j;

                            if (dt.Rows[n]["To R/C"].ToString() == sRC)
                            {
                                toN = CreateNode(dt.Rows[n]["To R/C"].ToString(), dt.Rows[n]["To Qty"].ToString());//2.創右下角的NODE
                                toN.DrawColor = toN.FillColor = Color.LightGreen;
                                toN.TextColor = Color.Black;
                            }
                            else
                            {
                                toN = CreateNode(dt.Rows[n]["To R/C"].ToString(), dt.Rows[n]["To Qty"].ToString());//2.創右下角的NODE
                            }

                            AddNode(dt.Rows[n]["To R/C"].ToString(), dt.Rows[n]["To Qty"].ToString(), toN);

                            linkI = drawLink(Action);

                            fromN.OutLinks.Add(linkI, toN);
                            break;
                        }
                    }

                    if (n == -1)
                    {
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (dt.Rows[k]["To R/C"].ToString() == sRC)
                            {
                                toN = CreateNode(dt.Rows[k]["To R/C"].ToString(), dt.Rows[k]["To Qty"].ToString());//2.創右下角的NODE
                                toN.DrawColor = toN.FillColor = Color.LightGreen;
                                toN.TextColor = Color.Black;
                            }
                            else
                            {
                                toN = CreateNode(dt.Rows[k]["To R/C"].ToString(), dt.Rows[k]["To Qty"].ToString());//2.創右下角的NODE
                            }

                            AddNode(dt.Rows[k]["To R/C"].ToString(), dt.Rows[k]["To Qty"].ToString(), toN);

                            linkI = drawLink(Action);

                            fromN.OutLinks.Add(linkI, toN);

                        }
                        cnn = 1;
                    }
                    else
                    {
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (k == n)
                                continue;
                            if (dt.Rows[k]["To R/C"].ToString() == sRC)
                            {
                                toN = CreateNode(dt.Rows[k]["To R/C"].ToString(), dt.Rows[k]["To Qty"].ToString());//2.創右下角的NODE
                                toN.DrawColor = toN.FillColor = Color.LightGreen;
                                toN.TextColor = Color.Black;
                            }
                            else
                            {
                                toN = CreateNode(dt.Rows[k]["To R/C"].ToString(), dt.Rows[k]["To Qty"].ToString());//2.創右下角的NODE
                            }

                            AddNode(dt.Rows[k]["To R/C"].ToString(), dt.Rows[k]["To Qty"].ToString(), toN);

                            linkI = drawLink(Action);

                            fromN.OutLinks.Add(linkI, toN);

                        }
                        cnn = 1;
                    }
                    
                }
                dt.Clear();
            }
               
            SetJump();
            HFlow();
        }

        private void SetJump()
        {
            addflowPanel.BeginAction(1009);

            // Change the jump style of each existing link
            foreach (Node node in addflowPanel.Nodes)
                foreach (Link link in node.OutLinks)
                    link.Jump = addflowPanel.DefLinkProp.Jump;

            // Terminate the action for the undo processor
            addflowPanel.EndAction();
        }

        private void HFlow()
        {
            hflow = new HFlow();
            // So that the user will be able to undo the layout in one time
            addflowPanel.BeginAction(1004);

            // Updates the properties
            hflow.MarginSize = new SizeF(40, 40);
            hflow.Orientation = Lassalle.Flow.Layout.Hierarchic.Orientation.West;
            hflow.VertexDistance = 70;
            hflow.LayerDistance = 70;
            hflow.LayerWidth = 0;

            // Execute the layout
            hflow.Layout(addflowPanel);

            // Terminate the action for the undo processor
            addflowPanel.EndAction();
        }

        private Node CreateNode(string RC, string RCQty)
        {
            Node nd = new Node(0, 0, np.Node_DrawWidth, np.Node_DrawHigh,
                                RC + Environment.NewLine + RCQty);
            return setNote(nd);
        }
        private Link drawLink(string Action)
        {
            Link lk = new Link(Action);
            lk.Font = np.Node_Font;
            lk.DrawWidth = np.LINK_DrawWidth;
            return lk;
        }

        private Node setNote(Node nd)
        {
            nd.DrawColor = nd.FillColor = np.Node_DrawColor;
            nd.Shape.Style = np.Node_ShapeStyle;
            nd.TextColor = np.Node_FontColor;
            nd.Font = np.Node_Font;
            return nd;
        }

        private void AddNode(string RCName, string RCQTY, Node nd)
        {
            addflowPanel.Nodes.Add(nd);
            if (azp.d_LastNode.ContainsKey(RCName))
                azp.d_LastNode[RCName] = nd;
            else
                azp.d_LastNode.Add(RCName, nd);
        }

        private void CreateDt()
        {
            dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]{
                                                   new DataColumn("From R/C"),
                                                   new DataColumn("From Qty"),
                                                   new DataColumn("To R/C"),
                                                   new DataColumn("To Qty"),
                    });
        }
    }
}
