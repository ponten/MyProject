using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Xml;
using System.Data.OracleClient;
using System.IO;
using Lassalle.Flow;


namespace CRoute
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }
        string sSQL;
        DataSet dstemp = new DataSet();
        DataSet dsProcess = new DataSet();
        DataSet dsRoute = new DataSet();
        AddFlow addflow = new AddFlow();
        public static String g_sUserID;

        public bool bCheck = false;
        public string sTempRouteName = "";

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            g_sUserID = ClientUtils.UserPara1;
            combShow.SelectedIndex = 0;


            addflow.Parent = this.panel3;
            addflow.Dock = DockStyle.Fill;
            addflow.AutoScroll = true;
            addflow.BackColor = SystemColors.Window;
            addflow.CanDrawNode = false;
            addflow.CanSizeNode = false;
            addflow.CanDrawLink = false;
            addflow.CanReflexLink = false;
            addflow.CanStretchLink = false;
            addflow.CanDragScroll = false;
            addflow.CanChangeOrg = false;
            addflow.CanChangeDst = false;
            addflow.CanMoveNode = false;

        }

        private void ShowData()
        {
            gvData.DataSource = null;
            sSQL = "SELECT A.ROUTE_ID,B.ROUTE_NAME,B.ROUTE_DESC FROM SAJET.SYS_RC_ROUTE_MAP A,SAJET.SYS_RC_ROUTE B WHERE A.ROUTE_ID=B.ROUTE_ID  AND 1=1 ";
            if (combShow.SelectedIndex == 0)
                sSQL += "  AND B.ENABLED='Y' ";
            else if (combShow.SelectedIndex == 1)
                sSQL += "  AND B.ENABLED='N' ";
            if (textFilter.Text != "")
            {
                // 已經移除使用 ROUTE_ID 搜尋的功能
                //if (combFilter.SelectedIndex == 0)
                //{
                //    // 2016.8.4 By Jason
                //    int iID = 0;

                //    if (!Int32.TryParse(textFilter.Text, out iID))
                //    {
                //        if (bCheck == false)
                //        {
                //            SajetCommon.Show_Message("Please Input Route ID", 0);
                //            return;
                //        }
                //    }
                //    // 2016.8.4 End


                //    // 2016.8.4 By Jason
                //    //sSQL += " AND A.ROUTE_ID = " + textFilter.Text + " ";
                //    if (bCheck == true)
                //    {
                //        sSQL += " AND A.ROUTE_ID = " + sTempRouteID + " ";
                //    }
                //    else
                //    {
                //        sSQL += " AND A.ROUTE_ID = " + textFilter.Text + " ";
                //    }
                //    // 2016.8.4 End
                //}

                // 2017.3.27 By Jason
                if (combFilter.SelectedIndex == 0)
                {
                    sSQL += " AND B.ROUTE_NAME LIKE '%" + textFilter.Text + "%' ";
                }
                if (combFilter.SelectedIndex == 1)
                {
                    sSQL += " AND B.ROUTE_DESC LIKE '%" + textFilter.Text + "%' ";
                }
                // 2017.3.27 End
            }

            // 2017.3.27 By Jason
            sSQL += " ORDER BY A.ROUTE_ID DESC ";
            // 2017.3.27 End

            // 2016.8.4 By Jason
            bCheck = false;
            // 2016.8.4 End

            try
            {
                gvData.DataSource = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                gvData.Columns["ROUTE_ID"].Visible = false;
                combFilter.Items.Clear();
                for (int i = 0; i <= gvData.Columns.Count - 1; i++)
                {
                    gvData.Columns[i].HeaderText = SajetCommon.SetLanguage(gvData.Columns[i].HeaderText.ToString(), 1);
                    if (gvData.Columns[i].HeaderText != SajetCommon.SetLanguage("ROUTE_ID", 1))
                    {
                        combFilter.Items.Add(gvData.Columns[i].HeaderText);
                    }
                }
                combFilter.SelectedIndex = 0;
                addflow.Nodes.Clear();
                if (gvData.Rows.Count != 0 && gvData.CurrentRow != null)
                    addflow.ReadXml(GetRouteXML());

                // 2015/08/13, Aaron
                update_process_name();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.ToString(), 0);
                return;
            }

        }

        private void textFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 用 ROUTE_ID 搜尋才需要卡控只能填數字，已經移除用 ROUTE_ID 搜尋的功能
            //if (combFilter.SelectedIndex == 0)
            //{
            //    if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            //        e.Handled = false;
            //    else
            //        e.Handled = true;
            //}
            if (e.KeyChar == (char)Keys.Return)
            {
                ShowData();
            }
        }

        private void After_Selection()
        {
            fEdit lForm1 = new fEdit();
            lForm1.Owner = this;
            lForm1.g_sformText = btnModify.Text;
            lForm1.XmlReader = GetRouteXML();
            lForm1.Route_ID = gvData.CurrentRow.Cells[0].Value.ToString();
            lForm1.Route_name = gvData.CurrentRow.Cells[1].Value.ToString();
            if (lForm1.ShowDialog() == DialogResult.OK)
            {
                // 2016.8.4 By Jason
                bCheck = true;
                combFilter.Text = SajetCommon.SetLanguage("ROUTE_NAME");
                sTempRouteName = gvData.CurrentRow.Cells["ROUTE_NAME"].Value.ToString();
                textFilter.Text = sTempRouteName;
                // 2016.8.4 End

                ShowData();
            }
        }

        private XmlReader GetRouteXML()
        {
            XmlReader Xmlreader = null;
            sSQL = string.Format(@"SELECT A.ROUTE_ID,A.ROUTE_MAP,A.UPDATE_SEQ,B.ROUTE_NAME FROM SAJET.SYS_RC_ROUTE_MAP A,SAJET.SYS_RC_ROUTE B 
                        WHERE A.ROUTE_ID=B.ROUTE_ID AND A.ROUTE_ID = {0}  ", gvData.CurrentRow.Cells[0].Value.ToString());
            try
            {
                dstemp = ClientUtils.ExecuteSQL(sSQL);
                XmlDocument Xmldoc = new XmlDocument();
                Xmldoc.LoadXml(dstemp.Tables[0].Rows[0]["ROUTE_MAP"].ToString());//從指定的字串載入XML文件                
                Xmlreader = XmlReader.Create(new System.IO.StringReader(Xmldoc.OuterXml));//輸入透過StringReader讀取Xmldoc中的Xmldoc字串輸出                                                           
                return Xmlreader;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.ToString(), 0);
                Xmlreader = null;
                return Xmlreader;
            }
        }

        private void gvData_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            After_Selection();
        }

        private void btnDisabled_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            string sID = gvData.CurrentRow.Cells[0].Value.ToString();
            string sType = "";
            string sEnabled = "";
            if (sender == btnDisabled)
            {
                sType = btnDisabled.Text;
                sEnabled = "N";
            }
            else if (sender == btnEnabled)
            {
                sType = btnEnabled.Text;
                sEnabled = "Y";
            }

            if (SajetCommon.Show_Message("確認?", 2) != DialogResult.Yes)
                return;
            sSQL = string.Format(@"UPDATE SAJET.SYS_RC_ROUTE SET ENABLED='{1}',UPDATE_USERID ={2},UPDATE_TIME = SYSDATE  WHERE ROUTE_ID={0} ", sID, sEnabled, g_sUserID);
            ClientUtils.ExecuteSQL(sSQL);
            ShowData();
        }

        private void btnEnabled_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            string sID = gvData.CurrentRow.Cells[0].Value.ToString();
            string sType = "";
            string sEnabled = "";
            if (sender == btnDisabled)
            {
                sType = btnDisabled.Text;
                sEnabled = "N";
            }
            else if (sender == btnEnabled)
            {
                sType = btnEnabled.Text;
                sEnabled = "Y";
            }
            if (SajetCommon.Show_Message("確認?", 2) != DialogResult.Yes)
                return;
            sSQL = string.Format(@"UPDATE SAJET.SYS_RC_ROUTE SET ENABLED='{1}',UPDATE_USERID ={2},UPDATE_TIME = SYSDATE  WHERE ROUTE_ID={0} ", sID, sEnabled, g_sUserID);
            ClientUtils.ExecuteSQL(sSQL);
            ShowData();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
            {
                return;
            }

            After_Selection();
        }

        private void combShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Visible = (combShow.SelectedIndex == 1);
            btnDisabled.Visible = (combShow.SelectedIndex == 0);
            btnEnabled.Visible = (combShow.SelectedIndex == 1);
            ShowData();
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            fEdit lForm1 = new fEdit();
            try
            {
                lForm1.Owner = this;
                lForm1.g_sformText = btnAppend.Text;
                if (lForm1.ShowDialog() == DialogResult.OK)
                {
                    textFilter.Text = "";
                    ShowData();
                }
            }
            finally
            {
                lForm1.Dispose();
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            string sID = gvData.CurrentRow.Cells[0].Value.ToString();
            if (SajetCommon.Show_Message("確認?", 2) != DialogResult.Yes)
                return;
            sSQL = string.Format(@"UPDATE SAJET.SYS_RC_ROUTE SET Enabled = 'Drop',UPDATE_USERID ={0},UPDATE_TIME = SYSDATE WHERE ROUTE_ID={1} ", sID, g_sUserID);
            ClientUtils.ExecuteSQL(sSQL);
            sSQL = string.Format(@"DELETE SAJET.SYS_RC_ROUTE WHERE ROUTE_ID={0}", sID);
            ClientUtils.ExecuteSQL(sSQL);
            sSQL = string.Format(@"DELETE SAJET.SYS_RC_ROUTE_DETAIL WHERE ROUTE_ID={0}", sID);
            ClientUtils.ExecuteSQL(sSQL);
            sSQL = string.Format(@"DELETE SAJET.SYS_RC_ROUTE_MAP WHERE ROUTE_ID={0}", sID);
            ClientUtils.ExecuteSQL(sSQL);
            ShowData();
        }
        private void gvData_CurrentCellChanged(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            addflow.Nodes.Clear();
            addflow.ReadXml(GetRouteXML());

            // 2015/08/13, Aaron
            update_process_name();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            fCopyRoute lForm1 = new fCopyRoute();
            lForm1.g_sformText = btnCopy.Text;
            lForm1.Owner = this;
            lForm1.Route_ID = gvData.CurrentRow.Cells[0].Value.ToString();
            lForm1.Route_name = gvData.CurrentRow.Cells[1].Value.ToString();
            if (lForm1.ShowDialog() == DialogResult.OK)
            { ShowData(); }
        }

        // 2015/08/13, Aaron
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
