using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;

namespace ToolingAcceptancedll
{
    public partial class fHistory : Form
    {
        public string g_sFeederID;
        public fHistory()
        {
            InitializeComponent();
        }

        private void fHistory_Load(object sender, EventArgs e)
        {
            
            string sSQL = "SELECT B.TOOLING_NO,A.MEMO,A.UPDATE_TIME "
                 + "       ,SAJET.SJ_TOOLING_STATUS_CHT(A.STATUS) STATUS,A.RESULT "
                 + "       ,C.EMP_NAME "
                 + "  FROM SAJET.G_TOOLING_MT_TRAVEL A "
                 + "      ,SAJET.SYS_TOOLING B "
                 + "      ,SAJET.SYS_EMP C "
                 + "  WHERE A.TOOLING_ID =:TOOLING_ID "
                 + "   AND A.TOOLING_ID = B.TOOLING_ID "
                 + "   AND A.UPDATE_USERID = C.EMP_ID(+) "
                 + "  ORDER BY A.UPDATE_TIME DESC ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", g_sFeederID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            dgvData.DataSource = dsTemp;
            dgvData.DataMember = dsTemp.Tables[0].ToString();

            SajetCommon.SetLanguageControl(this);

        }
    }
}