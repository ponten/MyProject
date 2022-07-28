using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;

namespace CRoute
{
    public partial class fCopyRoute : Form
    {
        public string sSQL;
        public DataSet dstemp;
        public static String g_sUserID;
        public static String g_sXmlfile;
        public string g_sformText;
        private string ROUTEID;

        private string Oldroute_name;
        public string Route_name
        {
            set
            {
                Oldroute_name = value;
            }
        }
        private string oldroute_id;
        public string Route_ID
        {
            set
            {
                oldroute_id = value;
            }
        }
        public fCopyRoute()
        {
            InitializeComponent();
        }

        private void fCopyRoute_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            textOldRouteName.Text = Oldroute_name;
        }
         
        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                ROUTEID = SajetCommon.GetMaxID("SAJET.SYS_RC_ROUTE", "ROUTE_ID", 8);
                if (textNewRouteName.Text == "")
                {
                    SajetCommon.Show_Message("Type New Route Name !!", 0);
                    return;
                }
                CopyRoute();
                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            Close(); 
        }
        //执行复制
        public bool CopyRoute()
        {
            try
            {
                object[][] Params = new object[6][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "I_NEWROUTEID", ROUTEID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_NEWROUTENAME", textNewRouteName.Text }; //待商榷
                Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "I_OLDROUTEID", oldroute_id };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_ROUTEDESC", string.IsNullOrEmpty(textRouteDesc.Text) ? Convert.DBNull : textRouteDesc.Text };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_EMPID", ClientUtils.UserPara1  };
                Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "O_RES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_COPY_ROUTE", Params);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() != "OK")
                    {
                        SajetCommon.Show_Message(ds.Tables[0].Rows[0][0].ToString(), 0);
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return false;
            }

            return true;
        }

       
    }
}
