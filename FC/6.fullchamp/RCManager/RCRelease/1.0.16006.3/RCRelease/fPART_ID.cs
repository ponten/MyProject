using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace RC_Release
{
    public partial class fPART_ID : Form
    {
        public fPART_ID()
        {
            InitializeComponent();
        }

        string sSQL;
        DataSet dsTemp;
        public string sPARTNO;

        private void fPART_ID_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            sSQL = @"select distinct part_no,part_id,part_type from sajet.sys_part order by part_id";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    fPART_ID_dgv.Rows.Add();
                    fPART_ID_dgv.Rows[i].Cells[0].Value = false;
                    fPART_ID_dgv.Rows[i].Cells[1].Value = dsTemp.Tables[0].Rows[i]["part_id"].ToString();
                    fPART_ID_dgv.Rows[i].Cells[2].Value = dsTemp.Tables[0].Rows[i]["part_no"].ToString();
                    fPART_ID_dgv.Rows[i].Cells[3].Value = dsTemp.Tables[0].Rows[i]["part_type"].ToString();
                    fPART_ID_dgv.Columns[0].ReadOnly = false;
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool IsChecked;
            int n = 0;
            if (fPART_ID_dgv.Rows.Count <= 0)
                return;
            try
            {
                for (int i = 0; i < fPART_ID_dgv.Rows.Count; i++)
                {
                    IsChecked = Convert.ToBoolean(fPART_ID_dgv.Rows[i].Cells[0].FormattedValue);
                    if (IsChecked)
                    {
                        n++;
                        sPARTNO += fPART_ID_dgv.Rows[i].Cells[2].Value.ToString() + ",";
                    }
                }
                

                if (n > 0)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    string sMsg = SajetCommon.SetLanguage("please select PartID");
                    SajetCommon.Show_Message(sMsg, 1);
                    return;
                }
            }
            catch(Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
