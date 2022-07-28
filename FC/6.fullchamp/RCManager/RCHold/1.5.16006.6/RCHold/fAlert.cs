using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace RCHold
{
    public partial class fAlert : Form
    {
        public fAlert()
        {
            InitializeComponent();
        }

        string sSQL;
        DataSet dsTemp;
        public string sPARTNO, sSpec1;

        private void fAlert_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            sSQL = @"select distinct part_no,part_id,part_type,spec1 from sajet.sys_part order by part_id";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    fAlert_dgv.Rows.Add();
                    fAlert_dgv.Rows[i].Cells[0].Value = false;
                    fAlert_dgv.Rows[i].Cells[1].Value = dsTemp.Tables[0].Rows[i]["part_id"].ToString();
                    fAlert_dgv.Rows[i].Cells[2].Value = dsTemp.Tables[0].Rows[i]["part_no"].ToString();
                    fAlert_dgv.Rows[i].Cells[3].Value = dsTemp.Tables[0].Rows[i]["part_type"].ToString();
                    // 2016.5.2 By Jason
                    fAlert_dgv.Rows[i].Cells[4].Value = dsTemp.Tables[0].Rows[i]["spec1"].ToString();
                    // 2016.5.2 End
                    fAlert_dgv.Columns[0].ReadOnly = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool IsChecked;
            int n = 0;
            if (fAlert_dgv.Rows.Count <= 0)
                return;
            try
            {
                for (int i = 0; i < fAlert_dgv.Rows.Count; i++)
                {
                    IsChecked = Convert.ToBoolean(fAlert_dgv.Rows[i].Cells[0].FormattedValue);

                    if (IsChecked)
                    {
                        n++;
                        sPARTNO += fAlert_dgv.Rows[i].Cells[2].Value.ToString() + ",";

                        // 2016.5.2 By Jason
                        if (fAlert_dgv.Rows[i].Cells[4].Value.ToString().Length > 0)
                        {
                            sSpec1 += fAlert_dgv.Rows[i].Cells[4].Value.ToString() + ",";
                        }
                        else
                        {
                            sSpec1 += "N/A,";
                        }
                        // 2016.5.2 End
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
            catch (Exception ex)
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
