using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;
using SajetTable;
using SajetFilter;
using System.Linq;

namespace RCPart
{
    public partial class fDetailRoute : Form
    {
        public UpdateType UpdateTypeEnum;
        public string g_sVersion;
        public string sNow;
        public string g_sformText, g_sPartID, g_sUserID, g_contiue;
        public string g_sRouteName, g_sRouteID, g_Description;
        public DataGridViewRow dataCurrentRow;
        object[][] Params;
        DataSet dsTemp;
        string sSQL, editDataSQL;
        bool btnAppendSucess = false;

        public fDetailRoute()
        {
            InitializeComponent();
        }

        private void fDetailRoute_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            //this.Text = g_sformText;

            if (UpdateTypeEnum == UpdateType.Append)
            {
                g_sPartID = SajetCommon.GetMaxID("SAJET.SYS_PART", "PART_ID", 10);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            g_sRouteName = editRouteName.Text.Trim();
            sSQL = @"
SELECT
    ROUTE_NAME,
    ROUTE_DESC
FROM
    SAJET.SYS_RC_ROUTE
WHERE
    ENABLED = 'Y'
";
            var p = new List<object[]>();

            if (!string.IsNullOrEmpty(editRouteName.Text.Trim()))
            {
                sSQL += " AND ROUTE_NAME LIKE '%' || :ROUTE_NAME || '%' ";

                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_NAME", editRouteName.Text.Trim() });
            }

            using (var f = new fFilter(sqlCommand: sSQL, @params: p, multiSelect: false, advancedSearch: false, checkBox: false))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    editRouteName.Text = f.ResultSets.First()?["ROUTE_NAME"]?.ToString();
                    editRouteDesc.Text = f.ResultSets.First()?["ROUTE_DESC"]?.ToString();

                    KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);

                    editItemName_KeyPress(sender, Key);
                }
            }
        }

        private void editItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (!CheckRoute(editRouteName.Text.Trim()))
                return;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (editRouteName.Text == "")
            {
                string sData = lblRouteName.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editRouteName.Focus();
                editRouteName.SelectAll();
                return;
            }

            ////取得系統時間
            //sSQL = "Select SYSDATE from dual ";
            //dsTemp = ClientUtils.ExecuteSQL(sSQL);
            //DateTime dtNow = (DateTime)dsTemp.Tables[0].Rows[0]["SYSDATE"]; //轉型成日期型別
            //sNow = dtNow.ToString("yyyy/MM/dd HH:mm:ss"); //定義日期格式

            try
            {

                //檢查Route Name是否重複
                sSQL = " SELECT * FROM SAJET.SYS_PART_ROUTE A "
                     + " ,SAJET.SYS_RC_ROUTE B "
                     + " WHERE A.PART_ID = '" + g_sPartID + "'"
                     + " AND A.ROUTE_ID = B.ROUTE_ID"
                     + " AND B.ROUTE_NAME = '" + editRouteName.Text.Trim() + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    string sData = lblRouteName.Text + " : " + editRouteName.Text + Environment.NewLine;
                    string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                    SajetCommon.Show_Message(sMsg, 0);
                    editRouteName.Focus();
                    editRouteName.SelectAll();
                    return;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }

            if (!CheckRoute(editRouteName.Text.Trim()))
                return;

            //Updata DB Info
            try
            {
                //AppendData();
                //UpdateData();
                //string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !" + Environment.NewLine + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
                string sMsg = SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
                // 2016.7.19 By Jason
                g_contiue = "N";

                g_sRouteName = editRouteName.Text.Trim();
                g_Description = editRouteDesc.Text.Trim();
                DialogResult = DialogResult.OK;
                // 2016.7.19 End

                if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                {
                    // 2016.7.19 By Jason
                    g_contiue = "Y";
                    // 2016.7.19 End

                    ClearData();
                    editRouteName.Focus();
                    return;
                }

                // 2016.7.19 By Jason
                //g_sRouteName = editRouteName.Text.Trim();
                //g_Description = editRouteDesc.Text.Trim();
                //DialogResult = DialogResult.OK;
                // 2016.7.19 End
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private bool CheckRoute(string sRoute)
        {
            try
            {
                sSQL = "SELECT ROUTE_ID, ROUTE_DESC FROM SAJET.SYS_RC_ROUTE WHERE ROUTE_NAME ='" + sRoute + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Error Route Name ", 1), 0);
                    return false;
                }
                editRouteDesc.Text = dsTemp.Tables[0].Rows[0]["ROUTE_DESC"].ToString();
                g_sRouteID = dsTemp.Tables[0].Rows[0]["ROUTE_ID"].ToString();
                return true;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return false;
            }
        }

        private void UpdateData()
        {
            editDataSQL = @"INSERT INTO SAJET.SYS_PART_ROUTE (PART_ID,ROUTE_ID,UPDATE_USERID,UPDATE_TIME,ENABLED) 
                            VALUES (:PART_ID,:ROUTE_ID,:UPDATE_USERID,:UPDATE_TIME,:ENABLED)";
            Params = new object[5][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", g_sRouteID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", g_sUserID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", sNow };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", "Y" };
            dsTemp = ClientUtils.ExecuteSQL(editDataSQL, Params);
        }

        private void ClearData()
        {
            editRouteName.Text = "";
            editRouteDesc.Text = "";
        }

        private void CopyToHistory(string sPartID, string sProcessID, string sAssyItemID)
        {
            string s = " INSERT INTO SAJET.SYS_HT_PART_ASSY_SETUP "
                    + " Select * FROM SAJET.SYS_PART_ASSY_SETUP "
                    + " WHERE PART_ID='" + sPartID + "' "
                    + "   AND PROCESS_ID ='" + sProcessID + "' "
                    + "   AND ASSY_ITEM_ID ='" + sAssyItemID + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

        }
    }
}