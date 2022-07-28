using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace CRoute
{
    public partial class fAddRoute : Form
    {
        public fAddRoute()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Route_ID回傳
        private string route_id;
        public string Route_ID
        {
            set
            {
                route_id = value;
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textRouteName.Text))
            {
                SajetCommon.Show_Message("Type Route Name !!", 0);
                return;
            }

            // 1.1.17003.14 途程名稱不可重複
            string SQL = $@"
SELECT *
FROM SAJET.SYS_RC_ROUTE
WHERE ROUTE_ID <> {textRouteID.Text}
AND ROUTE_NAME = '{textRouteName.Text}'
";
            DataSet set = ClientUtils.ExecuteSQL(SQL);
            if (set != null && set.Tables[0].Rows.Count > 0)
            {
                SajetCommon.Show_Message("Duplicate Route Name !!", 0);
                return;
            } // 1.1.17003.14

            fEdit lForm1 = (fEdit)this.Owner;
            lForm1.Route_ID = textRouteID.Text;
            lForm1.Route_name = textRouteName.Text;
            lForm1.Route_Desc = textRouteDesc.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void fAddRoute_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            string sSQL = string.Empty;
            DataSet dstemp = new DataSet();
            if (route_id != null)
            {
                textRouteID.Text = route_id;
                sSQL = string.Format(@"SELECT * FROM  SAJET.SYS_RC_ROUTE WHERE ROUTE_ID= {0} ", route_id);
                dstemp = ClientUtils.ExecuteSQL(sSQL);
                textRouteName.Text = dstemp.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
                textRouteDesc.Text = dstemp.Tables[0].Rows[0]["ROUTE_DESC"].ToString();
            }
            else
            {
                textRouteID.Text = SajetCommon.GetMaxID("SAJET.SYS_RC_ROUTE", "ROUTE_ID", 8);
            }
        }
    }
}
