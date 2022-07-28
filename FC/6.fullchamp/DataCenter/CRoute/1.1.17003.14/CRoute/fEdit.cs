using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Lassalle.Flow;
using System.Data.OracleClient;
using System.IO;
using SajetClass;
using SajetTable;


namespace CRoute
{
    public partial class fEdit : Form
    {
        public string sSQL;
        public DataSet dstemp;
        public AddFlow addflow = new AddFlow();
        public static String g_sUserID;
        public static String g_sXmlfile;
        public string g_sformText;

        private string processValue;
        public string ProcessValue
        {
            set
            {
                processValue = value;
            }
        }

        private string route_name;
        public string Route_name
        {
            set
            {
                route_name = value;
            }
        }

        private string route_id;
        public string Route_ID
        {
            set
            {
                route_id = value;
            }
        }

        private string route_desc = string.Empty;
        public string Route_Desc
        {
            set
            {
                route_desc = value;
            }
        }

        private XmlReader xmlreader;
        public XmlReader XmlReader
        {
            set
            {
                xmlreader = value;
            }
        }

        private string group_text;
        public string Group_Text
        {
            set
            {
                group_text = value;
            }
        }

        private string link_name;
        public string Link_Name
        {
            set
            {
                link_name = value;
            }
        }

        public fEdit()
        {
            InitializeComponent();
        }

        DataSet dsProcess = new DataSet();
        DataSet dsRoute = new DataSet();
        private void Initial_Form()
        {
            SajetCommon.SetLanguageControl(this);
            g_sUserID = ClientUtils.UserPara1;
            this.Text = g_sformText;
            g_sXmlfile = System.Windows.Forms.Application.StartupPath + "\\" + ClientUtils.fCurrentProject.ToString() + "\\";
            comboLine.SelectedIndex = 0;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            Initial_Form();

            #region AddFlow 屬性、事件
            addflow.Parent = this.panel3;
            addflow.Dock = DockStyle.Fill;
            addflow.AutoScroll = true;
            addflow.BackColor = SystemColors.Window;
            addflow.CanDrawNode = false;
            addflow.CanSizeNode = false;
            addflow.CanDrawLink = false;

            addflow.AfterAddNode += new AddFlow.AfterAddNodeEventHandler(after_addnode);
            addflow.BeforeAddNode += new AddFlow.BeforeAddNodeEventHandler(before_addnode);
            addflow.AfterAddLink += new AddFlow.AfterAddLinkEventHandler(After_addlink);
            addflow.BeforeAddLink += new AddFlow.BeforeAddLinkEventHandler(Before_addlink);
            addflow.MouseDown += new MouseEventHandler(mouse_down);
            addflow.KeyDown += new KeyEventHandler(key_down);

            #endregion

            rdDrag.Checked = true;
            Check_DragMode();
            if (xmlreader != null)
            {
                addflow.ReadXml(xmlreader);
                // 2015/11/05, polly
                update_process_name();
                labRouteID.Text = "ROUTE ID : " + route_id;
                labRouteName.Text = "ROUTE NAME : " + route_name;
                xmlreader.Close();
            }
            else
            {
                addflow.Nodes.Clear();
                labRouteID.Text = "";
                labRouteName.Text = "";
                add_node("START", 50, "");
                add_node("END", 100, "");
            }
        }

        #region 新增SubRoute
        public void add_subroute(string node_name, float location, string route_id)
        {
            Node node = new Node();
            node.Shape.Style = ShapeStyle.PredefinedProcess;
            node.Size = new SizeF(90, 60);
            node.Location = new PointF(0, location);
            node.Text = route_id + "\r\n" + node_name;
            node.Gradient = true;
            node.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            node.DrawColor = ColorTranslator.FromHtml("#646464");
            node.FillColor = ColorTranslator.FromHtml("#F1F1F1");
            node.GradientColor = ColorTranslator.FromHtml("#7E7E7E");
            node.TextColor = ColorTranslator.FromHtml("#000000");
            node.DrawWidth = 3;
            node.LabelEdit = false;
            addflow.Nodes.Add(node);

        }
        #endregion

        #region 新增Node
        public void add_node(string node_name, float location, string node_id)
        {
            Node node = new Node();
            if (node_name == "START")
            {
                node.Shape.Style = ShapeStyle.Connector;
                node.Size = new SizeF(50, 50);
                node.Location = new PointF(80, location);
                node.Text = node_name;
                node.Gradient = true;
                node.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                node.DrawColor = ColorTranslator.FromHtml("#6B8A3C");
                node.FillColor = ColorTranslator.FromHtml("#C5E492");
                node.GradientColor = ColorTranslator.FromHtml("#8CB83F");
                node.TextColor = ColorTranslator.FromHtml("#000000");
                node.DrawWidth = 3;
            }
            else if (node_name == "END")
            {
                node.Shape.Style = ShapeStyle.Connector;
                node.Size = new SizeF(50, 50);
                node.Location = new PointF(140, location);
                node.Text = node_name;
                node.Gradient = true;
                node.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                node.DrawColor = ColorTranslator.FromHtml("#A8504A");
                node.FillColor = ColorTranslator.FromHtml("#F0B09D");
                node.GradientColor = ColorTranslator.FromHtml("#B9503E");
                node.TextColor = ColorTranslator.FromHtml("#000000");
                node.DrawWidth = 3;


            }
            else
            {
                node.Shape.Style = ShapeStyle.RoundRect;
                node.Size = new SizeF(90, 60);
                node.Location = new PointF(0, location);
                node.Text = node_id + "\r\n" + node_name;
                node.Gradient = true;
                node.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                node.DrawColor = ColorTranslator.FromHtml("#5688B8");
                node.FillColor = ColorTranslator.FromHtml("#A0D3F3");
                node.GradientColor = ColorTranslator.FromHtml("#518FBC");
                node.TextColor = ColorTranslator.FromHtml("#000000");
                node.DrawWidth = 3;
            }
            node.LabelEdit = false;
            addflow.Nodes.Add(node);
        }
        #endregion

        #region AddFlow : after_addnode事件 停用
        public void after_addnode(object sender, AfterAddNodeEventArgs e)
        {
            /* if (processValue != "" )
             {                
                 e.Node.Text = processValue;
                 processValue = "";
                 if (combNode.SelectedIndex == 0)
                 {
                     e.Node.Shape.Style = ShapeStyle.Termination;
                     e.Node.Size = new SizeF(100, 40);
                     if (e.Node.Text == "START")
                     { e.Node.FillColor = Color.SpringGreen; }
                     else
                     {
                         e.Node.FillColor = Color.Red;
                     }

                 }
                 if (combNode.SelectedIndex == 1)
                 {
                     e.Node.Shape.Style = ShapeStyle.Document;
                     e.Node.Size = new SizeF(80, 80);
                     e.Node.FillColor = Color.RosyBrown;
                 }

                 e.Node.Shadow.Style = ShadowStyle.RightBottom;
                 e.Node.XSizeable = false;
                 e.Node.YSizeable = false;
                 e.Node.LabelEdit = false;


                 //e.Node.AutoSize = Lassalle.Flow.AutoSize.NodeToText;
                 //Node(RectangleF rect, Node model)
             }
             else
                 addflow.Nodes.Remove(e.Node);      */
        }
        #endregion 

        #region AddFlow : before_addnode事件 停用
        public void before_addnode(object sender, BeforeAddNodeEventArgs e)
        {/*
                _start_node();
                if (combNode.SelectedIndex == 0)
                {
                    if (start_node == true)
                    {
                        processValue = "END";
                    }
                    else
                        processValue = "START";
                }
                if (combNode.SelectedIndex == 1)
                {
                    if (start_node == true)
                    {
                        fProcess f = new fProcess();
                        f.Owner = this;
                        f.ShowDialog();   
                    }
                    else
                        MessageBox.Show("Please Add A START NODE !! ");                    
                } */
        }
        #endregion

        #region AddFlow : After_addlink事件
        public void After_addlink(object sender, AfterAddLinkEventArgs e)
        {
            if (comboLine.SelectedIndex == 1)
                e.Link.Line.Style = LineStyle.VH;
            if (comboLine.SelectedIndex == 2)
                e.Link.Line.Style = LineStyle.VHV;
            if (comboLine.SelectedIndex == 3)
                e.Link.Line.Style = LineStyle.HV;
            if (comboLine.SelectedIndex == 4)
                e.Link.Line.Style = LineStyle.HVH;
            e.Link.DrawColor = Color.Black;
            e.Link.BackMode = BackMode.Opaque;
            e.Link.TextColor = Color.Black;
            e.Link.DrawWidth = 2;
            if (e.Link.Org.OutLinks.Count > 1)
            {
                fLinkNamemodify lForm = new fLinkNamemodify();
                lForm.Owner = this;
                lForm.Link_Name = "";
                lForm.SetValue();
                if (lForm.ShowDialog() == DialogResult.OK)
                    e.Link.Text = link_name.ToUpper();
                else
                    e.Link.Remove();
                lForm.Dispose();
            }
            else
            { e.Link.Text = "NEXT"; }
            if (e.Link.Org == e.Link.Dst)
                e.Link.Line.Style = LineStyle.Bezier;
            e.Link.ArrowDst.Filled = true;

            e.Link.AdjustDst = false;
            e.Link.AdjustOrg = false;
        }
        #endregion

        #region AddFlow : Before_addlink事件
        public void Before_addlink(object sender, BeforeAddLinkEventArgs e)
        {
            if (e.Org.Text == "START")
            {
                if (e.Org.OutLinks.Count == 1)
                {
                    SajetCommon.Show_Message("START Node has one outlink !!", 0);
                    e.Cancel.Cancel = true;
                    return;
                }
            }
            if (e.Org.Text == "END")
            {
                SajetCommon.Show_Message("END Node can't build any OutLinks !!", 0);
                e.Cancel.Cancel = true;
                return;
            }
            if (e.Dst.Text == "START")
            {
                SajetCommon.Show_Message("START Node can't build any InLinks!!", 0);
                e.Cancel.Cancel = true;
                return;
            }
        }
        #endregion

        #region AddFlow : 解除Group
        public void de_group()
        {
            Item item = (Item)addflow.SelectedItem;
            if (item is Node)
            {
                Node node = (Node)item;
                if (node.Children.Count >= 1)
                {
                    foreach (Node node_children in node.Children)
                    {
                        if (!node_children.Text.EndsWith("AND") && !node_children.Text.EndsWith("OR"))
                        {
                            Node node2 = new Node();
                            node2.Shape.Style = node_children.Shape.Style;
                            node2.Size = new SizeF(node_children.Size.Width, node_children.Size.Height);
                            node2.Location = new PointF(node_children.Location.X, node_children.Location.Y);
                            node2.Text = node_children.Text;
                            node2.Gradient = node_children.Gradient;
                            node2.GradientMode = node_children.GradientMode;
                            node2.DrawColor = node_children.DrawColor;
                            node2.FillColor = node_children.FillColor;
                            node2.GradientColor = node_children.GradientColor;
                            node2.TextColor = node_children.TextColor;
                            node2.DrawWidth = node_children.DrawWidth;
                            node2.LabelEdit = false;
                            addflow.Nodes.Add(node2);
                        }
                    }
                }
            }

            addflow.DeleteSel();
        }
        #endregion

        #region AddFlow : key_down(DELETE)事件
        public void key_down(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
                return;
            addflow.DeleteSel();
        }
        #endregion                      

        #region AddFlow : mouse_down(右鍵事件) (刪除)
        public void MenuItem1_Click(object sender, EventArgs e)
        {
            addflow.DeleteSel();
        }
        #endregion

        #region AddFlow : mouse_down(右鍵事件) (修改名稱)
        public void MenuItem2_Click(object sender, EventArgs e)
        {
            fLinkNamemodify lForm = new fLinkNamemodify();
            lForm.Owner = this;
            lForm.Link_Name = addflow.SelectedItem.Text;
            lForm.SetValue();
            if (lForm.ShowDialog() == DialogResult.OK)
            {
                foreach (Item item in addflow.SelectedItems)
                {
                    if (item is Link)
                    {
                        Link link = (Link)item;
                        link.Text = link_name;
                    }
                }
                lForm.Dispose();
            }
        }
        #endregion

        #region AddFlow : mouse_down(右鍵事件) (解除群組)
        public void MenuItem4_Click(object sender, EventArgs e)
        {
            de_group();
        }
        #endregion

        #region AddFlow : mouse_down(右鍵事件) (修改連結_1)
        public void MenuItem3_1_Click(object sender, EventArgs e)
        {
            if (addflow.SelectedItems.Count > 1)
            {
                SajetCommon.Show_Message("ONLY ONE LINK!!", 0);
                return;
            }
            Item item = (Item)addflow.SelectedItem;
            if (item is Link)
            {
                Link old_link = (Link)item;
                Link new_link = new Link();
                new_link.Text = old_link.Text;
                new_link.Font = old_link.Font;
                new_link.DrawColor = old_link.DrawColor;
                new_link.BackMode = old_link.BackMode;
                new_link.TextColor = old_link.TextColor;
                old_link.Org.OutLinks.Add(new_link, old_link.Dst);
                old_link.Remove();
                new_link.AdjustDst = false;
                new_link.AdjustOrg = false;
                return;
            }
        }
        #endregion

        #region AddFlow : mouse_down(右鍵事件) (修改連結_2)
        public void MenuItem3_2_Click(object sender, EventArgs e)
        {
            if (addflow.SelectedItems.Count > 1)
            {
                SajetCommon.Show_Message("ONLY ONE LINK!!", 0);
                return;
            }
            foreach (Item item in addflow.SelectedItems)
            {
                if (item is Link)
                {
                    Link link = (Link)item;
                    link.Line.Style = LineStyle.VH;
                    link.AdjustDst = false;
                    link.AdjustOrg = false;
                }
            }
        }
        #endregion

        #region AddFlow : mouse_down(右鍵事件) (修改連結_3)
        public void MenuItem3_3_Click(object sender, EventArgs e)
        {
            if (addflow.SelectedItems.Count > 1)
            {
                SajetCommon.Show_Message("ONLY ONE LINK!!", 0);
                return;
            }
            foreach (Item item in addflow.SelectedItems)
            {
                if (item is Link)
                {
                    Link link = (Link)item;
                    link.Line.Style = LineStyle.VHV;
                    link.AdjustDst = false;
                    link.AdjustOrg = false;
                }
            }
        }
        #endregion

        #region AddFlow : mouse_down(右鍵事件) (修改連結_4)
        public void MenuItem3_4_Click(object sender, EventArgs e)
        {
            if (addflow.SelectedItems.Count > 1)
            {
                SajetCommon.Show_Message("ONLY ONE LINK!!", 0);
                return;
            }
            foreach (Item item in addflow.SelectedItems)
            {
                if (item is Link)
                {
                    Link link = (Link)item;
                    link.Line.Style = LineStyle.HV;
                    link.AdjustDst = false;
                    link.AdjustOrg = false;
                }
            }
        }
        #endregion

        #region AddFlow : mouse_down(右鍵事件) (修改連結_5)
        public void MenuItem3_5_Click(object sender, EventArgs e)
        {
            if (addflow.SelectedItems.Count > 1)
            {
                SajetCommon.Show_Message("ONLY ONE LINK!!", 0);
                return;
            }
            foreach (Item item in addflow.SelectedItems)
            {
                if (item is Link)
                {
                    Link link = (Link)item;
                    link.Line.Style = LineStyle.HVH;
                    link.AdjustDst = false;
                    link.AdjustOrg = false;
                }
            }
        }
        #endregion

        #region AddFlow : mouse_down(右鍵功能)
        public void mouse_down(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (addflow.ContextMenu != null)
                    addflow.ContextMenu.Dispose();
                Item item = addflow.SelectedItem;
                if (item is Node || item is Link)
                {
                    addflow.ContextMenu = new ContextMenu();
                    MenuItem notifyIconMenuItem1 = new MenuItem();
                    notifyIconMenuItem1.Text = "刪除(D&)";
                    notifyIconMenuItem1.Click += new EventHandler(MenuItem1_Click);
                    addflow.ContextMenu.MenuItems.Add(notifyIconMenuItem1);
                    MenuItem notifyIconMenuItem2 = new MenuItem();
                    notifyIconMenuItem2.Text = "修改名稱(M&)";
                    notifyIconMenuItem2.Click += new EventHandler(MenuItem2_Click);
                    addflow.ContextMenu.MenuItems.Add(notifyIconMenuItem2);

                    MenuItem notifyIconMenuItem3 = new MenuItem();
                    notifyIconMenuItem3.Text = "連結樣式(G&)";

                    MenuItem notifyIconMenuItem4 = new MenuItem();
                    notifyIconMenuItem4.Text = "解除群組(E&)";
                    notifyIconMenuItem4.Click += new EventHandler(MenuItem4_Click);
                    addflow.ContextMenu.MenuItems.Add(notifyIconMenuItem4);


                    MenuItem notifyIconMenuItem3_1 = new MenuItem();
                    notifyIconMenuItem3_1.Text = "直線";
                    notifyIconMenuItem3_1.Click += new EventHandler(MenuItem3_1_Click);
                    notifyIconMenuItem3.MenuItems.Add(notifyIconMenuItem3_1);

                    MenuItem notifyIconMenuItem3_2 = new MenuItem();
                    notifyIconMenuItem3_2.Text = "VH";
                    notifyIconMenuItem3_2.Click += new EventHandler(MenuItem3_2_Click);
                    notifyIconMenuItem3.MenuItems.Add(notifyIconMenuItem3_2);

                    MenuItem notifyIconMenuItem3_3 = new MenuItem();
                    notifyIconMenuItem3_3.Text = "VHV";
                    notifyIconMenuItem3_3.Click += new EventHandler(MenuItem3_3_Click);
                    notifyIconMenuItem3.MenuItems.Add(notifyIconMenuItem3_3);

                    MenuItem notifyIconMenuItem3_4 = new MenuItem();
                    notifyIconMenuItem3_4.Text = "HV";
                    notifyIconMenuItem3_4.Click += new EventHandler(MenuItem3_4_Click);
                    notifyIconMenuItem3.MenuItems.Add(notifyIconMenuItem3_4);

                    MenuItem notifyIconMenuItem3_5 = new MenuItem();
                    notifyIconMenuItem3_5.Text = "HVH";
                    notifyIconMenuItem3_5.Click += new EventHandler(MenuItem3_5_Click);
                    notifyIconMenuItem3.MenuItems.Add(notifyIconMenuItem3_5);

                    addflow.ContextMenu.MenuItems.Add(notifyIconMenuItem3);

                    if (item is Node)
                    {
                        notifyIconMenuItem2.Enabled = false;
                        notifyIconMenuItem3.Enabled = false;
                        Node g_node = (Node)item;
                        if (g_node.Children.Count == 0)
                            notifyIconMenuItem4.Enabled = false;
                    }
                    if (item is Link)
                    {
                        notifyIconMenuItem4.Enabled = false;
                    }
                }
            }
        }
        #endregion

        #region AddFlow : 新增Group

        private void btnGroup_Click(object sender, EventArgs e)
        {
            float x = 0, y = 0;
            if (addflow.SelectedItems.Count < 1)
            {
                SajetCommon.Show_Message("PLEASE INSERT A NEW NODE !!", 0);
                return;
            }
            float[] group_range_x = new float[addflow.SelectedItems.Count];
            float[] group_range_y = new float[addflow.SelectedItems.Count];
            float[] group_range_x2 = new float[addflow.SelectedItems.Count];
            float[] group_range_y2 = new float[addflow.SelectedItems.Count];
            int i = 0;
            foreach (Item item in addflow.SelectedItems)
            {
                if (item is Link)
                {
                    SajetCommon.Show_Message("The Group can't include Links ! !", 0);
                    return;
                }
                if (item is Node)
                {
                    Node node = (Node)item;
                    if (node.Links.Count != 0)
                    {
                        SajetCommon.Show_Message("The Group can't include Links ! !", 0);
                        return;
                    }
                    if (node.Text == "START")
                    {
                        SajetCommon.Show_Message("The Group can't include 'Start_Node' ! ! ", 0);
                        return;
                    }
                    if (node.Text == "END")
                    {
                        SajetCommon.Show_Message("The Group can't include 'End_Node' ! ! ", 0);
                        return;
                    }

                    if (node.Children.Count != 0)
                    {
                        SajetCommon.Show_Message("The Group can't include 'Group' ! ! ", 0);
                        return;
                    }
                    x += Convert.ToSingle(node.Location.X.ToString());
                    y += Convert.ToSingle(node.Location.Y.ToString());

                    group_range_x[i] = node.Location.X;
                    group_range_y[i] = node.Location.Y;
                    group_range_x2[i] = node.Location.X + node.Size.Width;
                    group_range_y2[i] = node.Location.Y + node.Size.Height;
                    i++;
                }
            }

            fGroupSelect m_s = new fGroupSelect();
            m_s.Owner = this;
            if (m_s.ShowDialog() == DialogResult.OK)
            {
                Node node_group = new Node(group_range_x.Min() - 20, group_range_y.Min() - 60, group_range_x2.Max() - group_range_x.Min() + 40, group_range_y2.Max() - group_range_y.Min() + 80);//group_range_x.Max() - group_range_x.Min()+80, group_range_y.Max() - group_range_y.Min()+80);
                i = 0;
                Node label = new Node(group_range_x.Min() - 20, group_range_y.Min() - 50, group_range_x2.Max() - group_range_x.Min() + 40, 40, group_text);
                label.Shape.Style = ShapeStyle.Data;
                label.DrawColor = Color.Transparent;    //透明無框
                label.TextColor = Color.Black;
                label.Transparent = true;
                label.Logical = false;
                label.Selectable = false;
                //label.AutoSize = Lassalle.Flow.AutoSize.NodeToText;
                label.AttachmentStyle = AttachmentStyle.Item;
                label.Alignment = Alignment.CenterBOTTOM;
                addflow.Nodes.Add(label);
                label.Parent = node_group;
                foreach (Item item in addflow.SelectedItems)
                {
                    if (item is Node)
                    {
                        Node node = (Node)item;
                        if (node.Shape.Style == ShapeStyle.PredefinedProcess)
                        {
                            node.Parent = node_group;
                            //node.Shape.Style = ShapeStyle.PredefinedProcess;
                            //node.FillColor = Color.SkyBlue;
                            //node.Location = new PointF(group_range_x.Min() + 20, group_range_y.Min() + 40 + (100 * i));
                        }
                        else
                        {
                            node.Parent = node_group;
                            //node.Shape.Style = ShapeStyle.Process;
                            //node.FillColor = Color.LightGoldenrodYellow;
                            //node.Location = new PointF(group_range_x.Min() + 20, group_range_y.Min() + 40 + (100 * i));
                        }
                        i++;
                    }
                }
                if (group_text.EndsWith("AND"))
                    node_group.Shape.Style = ShapeStyle.Rectangle;
                if (group_text.EndsWith("OR"))
                    node_group.Shape.Style = ShapeStyle.Custom;
                node_group.LabelEdit = false;
                node_group.Hidden = false;
                node_group.BackMode = BackMode.Transparent;
                node_group.Transparent = true;
                node_group.DrawColor = Color.Black;
                node_group.DrawWidth = 2;
                node_group.HighlightChildren = false;
                node_group.RemoveChildren = true;
                node_group.AttachmentStyle = AttachmentStyle.DestinationNode;
                node_group.AutoSize = Lassalle.Flow.AutoSize.NodeToText;
                node_group.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                addflow.Nodes.Add(node_group);
                foreach (Node node in node_group.Children)
                {
                    node.Selectable = false;
                }
            }
            m_s.Dispose();
        }
        #endregion

        #region GetNodeID :  Node_ID
        /// <summary>
        /// 取得唯一值NODE ID
        /// </summary>
        public string GetNodeID(string route_id, string seq)
        {
            string node_id = string.Empty;
            for (int i = 1; seq.Length < 4; i++)
            {
                seq = "0" + seq;
            }
            node_id = route_id + seq;
            return node_id;
        }
        #endregion

        #region GetNodeType :  Node_Type
        /// <summary>
        /// 取得NODE型態
        /// </summary>
        public string GetNodeType(string shape_style, string text)
        {
            string node_type = string.Empty;
            if (shape_style == "Connector")
            {
                if (text == "START")
                    node_type = "0";
                else
                    node_type = "9";
            }
            if (shape_style == "RoundRect")   //一般Process
                node_type = "1";
            if (shape_style == "Rectangle") //Group-AND
                node_type = "2";
            if (shape_style == "Custom")    //Group-OR
                node_type = "3";
            if (shape_style == "PredefinedProcess")     //SubRoute
                node_type = "4";
            return node_type;
        }
        #endregion

        #region GetNodeContent :  Node_Content
        /// <summary>
        /// 取得NODE Content
        /// </summary>
        public string GetNodeContent(string shape_style, string text)
        {
            string node_content = text;
            //if (shape_style == "PredefinedProcess")
            //    node_content = "SubRoute";
            return node_content;
        }
        #endregion

        #region SavetoDB
        private void btnSavexml_Click(object sender, EventArgs e)
        {
            bool check_link_text = true;
            if (addflow.Nodes.Count < 2)
            {
                SajetCommon.Show_Message("PLEASE INSERT A NEW NODE !!", 0);
                return;
            }
            foreach (Node node in addflow.Nodes)
            {
                if (node.OutLinks.Count > 0)
                {
                    foreach (Item item in node.OutLinks)
                    {
                        if (item is Link)
                        {
                            Link link = (Link)item;
                            if (link.Text == "NEXT")
                            { check_link_text = true; break; }
                            else
                                check_link_text = false;
                        }
                    }
                    if (!check_link_text)
                    {
                        SajetCommon.Show_Message("Node doesn't has NEXT link!!", 0);
                        return;
                    }
                }
                if (node.Text == "START")
                {
                    if (node.OutLinks.Count == 0)
                    {
                        SajetCommon.Show_Message("START doesn't has OutLinks!!", 0);
                        return;
                    }
                }
                else if (node.Text == "END")
                {
                    if (node.InLinks.Count == 0)
                    {
                        SajetCommon.Show_Message("END doesn't has InLinks!!", 0);
                        return;
                    }
                }
                else
                {
                    if (node.OutLinks.Count == 0)
                    {
                        Node n = (Node)node.Parent;
                        if (n == null)
                        {
                            SajetCommon.Show_Message("Node doesn't has OutLinks!!", 0);
                            return;
                        }
                    }
                }
            }
            string node_text = string.Empty;
            string link_text = string.Empty;
            string node_parent = string.Empty;
            string xml_index = string.Empty;
            string org_index = string.Empty;
            string dst_index = string.Empty;
            string p_name = string.Empty;
            string node_id = string.Empty;
            //int group_id = 1;
            string shape_style = string.Empty;
            string next_node_id = string.Empty;
            int update_seq = 1;

            fAddRoute lForm = new fAddRoute();
            lForm.Owner = this;
            lForm.Route_ID = route_id;
            if (lForm.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                #region Check_UPDATE_SEQ
                sSQL = string.Format(@"SELECT * FROM SAJET.SYS_RC_ROUTE WHERE ROUTE_ID={0} ", route_id);
                try
                {
                    dstemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dstemp.Tables[0].Rows.Count != 0)
                    {
                        update_seq = Convert.ToInt32(dstemp.Tables[0].Rows[0]["UPDATE_SEQ"].ToString()) + 1;

                        // 2016.8.4 By Jason
                        //sSQL = string.Format(@"UPDATE SAJET.SYS_RC_ROUTE SET ROUTE_NAME='{0}',UPDATE_SEQ={1},UPDATE_TIME={2}  WHERE ROUTE_ID={3} ",
                        //        route_name, update_seq, "SYSDATE", route_id);
                        sSQL = string.Format(@"UPDATE SAJET.SYS_RC_ROUTE SET ROUTE_NAME='{0}',UPDATE_SEQ={1},UPDATE_TIME={2},ROUTE_DESC='{3}' WHERE ROUTE_ID={4} ",
                                route_name, update_seq, "SYSDATE", route_desc, route_id);
                        // 2016.8.4 End

                        ClientUtils.ExecuteSQL(sSQL);
                        sSQL = string.Format(@"DELETE FROM SAJET.SYS_RC_ROUTE_DETAIL WHERE ROUTE_ID={0} ", route_id);
                        ClientUtils.ExecuteSQL(sSQL);
                        sSQL = string.Format(@"DELETE FROM SAJET.SYS_RC_ROUTE_MAP WHERE ROUTE_ID={0} ", route_id);
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                    else
                    {
                        sSQL = string.Format(@"INSERT INTO SAJET.SYS_RC_ROUTE VALUES({0},'{1}','{2}',{3},{4},'{5}',{6})",
                                route_id, route_name, route_desc, g_sUserID, "SYSDATE", "Y", update_seq);
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }
                catch (Exception ex)
                { SajetCommon.Show_Message(ex.ToString(), 0); return; }
                #endregion

                #region Save ADDFLOW in XML
                XmlWriterSettings settings = new XmlWriterSettings();
                XmlDocument doc = new XmlDocument();
                settings.Indent = true;
                settings.IndentChars = "\t";
                settings.CloseOutput = true;
                XmlWriter writer = XmlWriter.Create(g_sXmlfile + route_name + ".xml", settings);
                addflow.WriteXml(writer, false, true);
                writer.Close();
                doc.Load(g_sXmlfile + route_name + ".xml");
                #endregion

                #region Save Node to DB
                XmlNodeList NodeLists = doc.SelectNodes("AddFlow/Node");
                foreach (XmlNode item in NodeLists)
                {
                    for (int i = 0; i < item.ChildNodes.Count; i++)
                    {
                        p_name = item.ChildNodes[i].Name.ToString();
                        if (p_name == "Text")
                        {
                            node_text = item.ChildNodes[i].InnerText.ToString();
                            if (node_text != "START" && node_text != "END" && node_text != "AND" && node_text != "OR")
                                node_text = node_text.Substring(0, node_text.IndexOf(Environment.NewLine));
                        }
                        if (p_name == "Shape")
                        {
                            shape_style = item.ChildNodes[i].Attributes["Style"].Value.ToString();
                            if (shape_style == "Rectangle" || shape_style == "Custom")
                            {
                                //node_text = "";
                                //node_text = group_id.ToString();
                                //group_id++;
                            }
                        }
                    }
                    if (shape_style == "Data")
                        continue;
                    foreach (XmlAttribute Attr in item.Attributes)
                    {
                        p_name = Attr.Name.ToString();
                        if (p_name == "Index")
                        {
                            xml_index = item.Attributes[Attr.Name.ToString()].Value;
                            break;
                        }
                    }
                    node_id = GetNodeID(route_id, xml_index);
                    object[][] Params = new object[6][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", route_id };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", node_id };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_TYPE", GetNodeType(shape_style, node_text) };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_CONTENT", GetNodeContent(shape_style, node_text) };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "XML_INDEX", xml_index };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_SEQ", update_seq };
                    sSQL = "INSERT INTO SAJET.SYS_RC_ROUTE_DETAIL (ROUTE_ID,NODE_ID,NODE_TYPE,NODE_CONTENT,XML_INDEX,UPDATE_SEQ) VALUES( :ROUTE_ID,:NODE_ID,:NODE_TYPE,:NODE_CONTENT, :XML_INDEX,:UPDATE_SEQ  )";
                    try
                    { ClientUtils.ExecuteSQL(sSQL, Params); }
                    catch (Exception ex)
                    { SajetCommon.Show_Message(ex.ToString(), 0); return; }
                }
                #endregion

                #region Save Group_Node to DB

                sSQL = string.Format(@"SELECT NODE_TYPE FROM SAJET.SYS_RC_ROUTE_DETAIL WHERE  NODE_TYPE IN (2,3) AND ROUTE_ID={0} ", route_id);
                try
                {
                    dstemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dstemp.Tables[0].Rows.Count != 0)
                    {
                        foreach (XmlNode item in NodeLists)
                        {
                            for (int i = 0; i < item.ChildNodes.Count; i++)
                            {
                                p_name = item.ChildNodes[i].Name.ToString();
                                node_parent = "";
                                if (p_name == "Parent")
                                    node_parent = item.ChildNodes[i].InnerText.ToString();
                            }
                            foreach (XmlAttribute Attr in item.Attributes)
                            {
                                p_name = Attr.Name.ToString();
                                xml_index = "";
                                if (p_name == "Index")
                                {
                                    xml_index = item.Attributes[Attr.Name.ToString()].Value;
                                    break;
                                }
                            }
                            if (node_parent != "")
                            {
                                sSQL = string.Format(@"SELECT * FROM SAJET.SYS_RC_ROUTE_DETAIL WHERE ROUTE_ID={0} AND XML_INDEX={1} ", route_id, node_parent);
                                dstemp = ClientUtils.ExecuteSQL(sSQL);
                                if (dstemp.Tables[0].Rows.Count != 0)
                                {
                                    sSQL = string.Format(@"UPDATE SAJET.SYS_RC_ROUTE_DETAIL SET GROUP_ID={0} WHERE ROUTE_ID={1} AND XML_INDEX={2}",
                                        dstemp.Tables[0].Rows[0]["NODE_ID"].ToString(), route_id, xml_index);
                                    ClientUtils.ExecuteSQL(sSQL);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { SajetCommon.Show_Message(ex.ToString(), 0); return; }
                #endregion

                #region Save Link to DB
                NodeLists = doc.SelectNodes("AddFlow/Link");
                foreach (XmlNode item in NodeLists)
                {
                    for (int i = 0; i < item.ChildNodes.Count; i++)
                    {
                        p_name = item.ChildNodes[i].Name.ToString();
                        if (p_name == "Text")
                            link_text = item.ChildNodes[i].InnerText.ToString();
                    }
                    foreach (XmlAttribute Attr in item.Attributes)
                    {
                        p_name = Attr.Name.ToString();
                        if (p_name == "Org")
                        {
                            org_index = item.Attributes[Attr.Name.ToString()].Value;
                        }
                        if (p_name == "Dst")
                        {
                            dst_index = item.Attributes[Attr.Name.ToString()].Value;
                        }
                    }
                    sSQL = string.Format(@"SELECT NODE_ID FROM SAJET.SYS_RC_ROUTE_DETAIL WHERE ROUTE_ID={0} AND XML_INDEX={1}", route_id, dst_index);
                    dstemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dstemp.Tables[0].Rows.Count != 0)
                        next_node_id = dstemp.Tables[0].Rows[0]["NODE_ID"].ToString();
                    sSQL = string.Format(@"SELECT * FROM SAJET.SYS_RC_ROUTE_DETAIL WHERE ROUTE_ID={0} AND XML_INDEX={1}", route_id, org_index);
                    dstemp = ClientUtils.ExecuteSQL(sSQL);
                    string s_temp = dstemp.Tables[0].Rows[0]["GROUP_ID"].ToString();
                    if (dstemp.Tables[0].Rows[0]["NEXT_NODE_ID"].ToString() == "")
                    {
                        sSQL = string.Format(@"UPDATE SAJET.SYS_RC_ROUTE_DETAIL SET 
NEXT_NODE_ID={0},LINK_NAME='{1}' WHERE ROUTE_ID={2} AND XML_INDEX={3} ", next_node_id, link_text, route_id, org_index);
                        try { ClientUtils.ExecuteSQL(sSQL); }
                        catch (Exception ex)
                        { SajetCommon.Show_Message(ex.ToString(), 0); return; }
                    }
                    else
                    {
                        object[][] Params = new object[9][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", route_id };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", dstemp.Tables[0].Rows[0]["NODE_ID"].ToString() };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_TYPE", dstemp.Tables[0].Rows[0]["NODE_TYPE"].ToString() };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_CONTENT", dstemp.Tables[0].Rows[0]["NODE_CONTENT"].ToString() };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "GROUP_ID", s_temp };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_NODE_ID", next_node_id };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LINK_NAME", link_text };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_SEQ", dstemp.Tables[0].Rows[0]["UPDATE_SEQ"].ToString() };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "XML_INDEX", org_index };
                        sSQL = "INSERT INTO SAJET.SYS_RC_ROUTE_DETAIL VALUES( :ROUTE_ID,:NODE_ID,:NODE_TYPE,:NODE_CONTENT,:GROUP_ID,:NEXT_NODE_ID,:LINK_NAME,:UPDATE_SEQ,:XML_INDEX)";
                        try
                        { ClientUtils.ExecuteSQL(sSQL, Params); }
                        catch (Exception ex)
                        { SajetCommon.Show_Message(ex.ToString(), 0); return; }
                    }
                }
                #endregion

                #region Save XML to DB
                object[][] Params2 = new object[3][];   //當CLOB超過4000字符時，要改用參數Params2
                Params2[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", route_id };
                Params2[1] = new object[] { ParameterDirection.Input, OracleType.Clob, "ROUTE_MAP", doc.InnerXml.ToString() };
                Params2[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_SEQ", update_seq };
                sSQL = "INSERT INTO SAJET.SYS_RC_ROUTE_MAP VALUES(:ROUTE_ID,:ROUTE_MAP,:UPDATE_SEQ )";
                try { ClientUtils.ExecuteSQL(sSQL, Params2); }
                catch (Exception ex)
                { SajetCommon.Show_Message(ex.ToString(), 0); return; }
                #endregion

                SajetCommon.Show_Message("Save OK", 3);
                addflow.Nodes.Clear();
                labRouteID.Text = "";
                labRouteName.Text = "";
                System.IO.File.Delete(g_sXmlfile + route_name + ".xml");
                route_id = null;
                add_node("START", 50, "");//直接新增一個起始點和終點
                add_node("END", 80, "");
                this.Cursor = Cursors.Default;
                DialogResult = DialogResult.OK;
                this.Close();
            }
            lForm.Dispose();
        }
        #endregion

        #region Addflow: 新增Node
        private void btnAddnode_Click(object sender, EventArgs e)
        {
            fProcess f = new fProcess();
            f.Owner = this;
            f.ShowDialog();
        }
        #endregion

        #region Addflow: 清除畫面
        private void btnclear_Click(object sender, EventArgs e)
        {
            addflow.Nodes.Clear();
            labRouteID.Text = "";
            labRouteName.Text = "";
            add_node("START", 50, "");
            add_node("END", 100, "");
        }
        #endregion

        private void Check_DragMode()
        {
            if (rdDrag.Checked)
            {
                addflow.CanDrawLink = false;
                comboLine.Enabled = false;
            }
            else
            {
                addflow.CanDrawLink = true;
                comboLine.Enabled = true;
            }
            if (rdLink1.Checked)
            {
                addflow.CanDrawLink = true;
                comboLine.Enabled = true;
            }
            else
            {
                addflow.CanDrawLink = false;
                comboLine.Enabled = false;
            }
            comboLine.SelectedIndex = 0;
        }

        private void btnAddEnd_Click(object sender, EventArgs e)
        {
            add_node("END", 100, "");
        }

        private void rdDrag_CheckedChanged(object sender, EventArgs e)
        {
            Check_DragMode();
        }

        private void rdLink1_CheckedChanged(object sender, EventArgs e)
        {
            Check_DragMode();
        }

        // 2015/11/05, POLLY
        public void update_process_name()
        {
            if (dsProcess.Tables.Count == 0)
            {
                sSQL = "SELECT PROCESS_ID, PROCESS_NAME FROM SAJET.SYS_PROCESS";
                dsProcess = ClientUtils.ExecuteSQL(sSQL);
            }

            if (dsRoute.Tables.Count == 0)
            {
                sSQL = "SELECT ROUTE_ID, ROUTE_NAME FROM SAJET.SYS_RC_ROUTE";
                dsRoute = ClientUtils.ExecuteSQL(sSQL);
            }

            for (int i = 0; i < addflow.Nodes.Count; i++)
            {
                if (addflow.Nodes[i].Shape.Style == ShapeStyle.RoundRect)
                {
                    //MessageBox.Show(addflow.Nodes[i].Text.IndexOf("\n"));
                    using (StringReader sr = new StringReader(addflow.Nodes[i].Text))
                    {
                        //讀取第一行
                        string strProcess_ID = sr.ReadLine();

                        if (!string.IsNullOrEmpty(strProcess_ID))
                        {

                            DataRow[] drSel = dsProcess.Tables[0].Select("Process_ID = '" + strProcess_ID + "'");
                            if (drSel.Length > 0)
                            {
                                addflow.Nodes[i].Text = strProcess_ID + "\r\n" + drSel[0]["Process_Name"].ToString();
                            }
                        }
                    }
                }
                else if (addflow.Nodes[i].Shape.Style == ShapeStyle.PredefinedProcess)
                {
                    using (StringReader sr = new StringReader(addflow.Nodes[i].Text))
                    {
                        //讀取第一行
                        string strRoute_ID = sr.ReadLine();

                        if (!string.IsNullOrEmpty(strRoute_ID))
                        {

                            DataRow[] drSel = dsRoute.Tables[0].Select("Route_ID = '" + strRoute_ID + "'");
                            if (drSel.Length > 0)
                            {
                                addflow.Nodes[i].Text = strRoute_ID + "\r\n" + drSel[0]["Route_Name"].ToString();
                            }
                        }
                    }
                }
            }
        }
    }
}
