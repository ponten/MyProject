using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;

namespace BCRuleDll
{
    public partial class fWOData : Form
    {
        
        public fWOData()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            String sRule = lablRuleName.Text;

            for (int i = 0; i <= dgvSNList.Rows.Count - 1; i++)
            {
                if (dgvSNList.Rows[i].Cells["CHECKED"].Value.ToString() == "Y")
                {
                    DataSet dsTemp = new DataSet();
                    object[][] Params = new object[1][];
                    string sWO = dgvSNList.Rows[i].Cells["WORK_ORDER"].Value.ToString();
                    string sWORuleSet = dgvSNList.Rows[i].Cells["WO_RULE"].Value.ToString();
                    string sSQL = " Delete SAJET.G_WO_PARAM "
                                + "  WHERE WORK_ORDER =:WORK_ORDER "
                                + "    And MODULE_NAME in (select upper(label_name) || ' RULE' from sajet.sys_label) ";
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
                    Params = new object[3][];
                    sSQL = "Insert Into SAJET.G_WO_PARAM "
                         + "(WORK_ORDER,MODULE_NAME,FUNCTION_NAME,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID,UPDATE_TIME)"
                         + "Select :WORK_ORDER,B.RULE_TYPE||' RULE',B.RULE_NAME,D.PARAME_NAME,D.PARAME_ITEM,D.PARAME_VALUE,:UPDATE_USERID,SYSDATE "
                         + " From SAJET.SYS_MODULE_PARAM A  "
                         + "     ,SAJET.SYS_RULE_NAME B "
                         + " 	 ,SAJET.SYS_RULE_PARAM D "
                         + "     ,sajet.sys_label c "
                         + "Where A.MODULE_NAME = 'W/O RULE' "
                         + "and A.FUNCTION_NAME = :RULE_NAME "
                         + "and A.PARAME_NAME = c.label_name || ' Rule'  "
                         + "and A.PARAME_ITEM = B.RULE_NAME  "
                         + "and B.RULE_TYPE = upper(c.label_name) "
                         + "and c.type <> 'U' "
                         + "and B.RULE_ID = D.RULE_ID ";
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RULE_NAME", sWORuleSet };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void fWOData_Load(object sender, EventArgs e)
        {
            
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
            panel3.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            /*
                                    string sSQL = "SELECT A.WORK_ORDER ,C.PART_NO,A.TARGET_QTY,A.INPUT_QTY,A.OUTPUT_QTY "
                                                + "  FROM (SELECT WORK_ORDER FROM SAJET.G_WO_PARAM "
                                                + "         WHERE MODULE_NAME =:MODULE_NAME "
                                                +"           AND FUNCTION_NAME =:FUNCTION_NAME "
                                                +"         GROUP BY WORK_ORDER ) B "
                                                + "       ,SAJET.G_WO_BASE A "
                                                + "       ,SAJET.SYS_PART C "
                                                + " WHERE  A.WORK_ORDER = B.WORK_ORDER "
                                            //    + "   AND (A.OUTPUT_QTY < A.TARGET_QTY OR A.INPUT_QTY < A.TARGET_QTY)  "
                                                + "   AND A.WO_STATUS <>'6' "
                                                + "   AND A.PART_ID = C.PART_ID(+) "
                                                + " ORDER BY A.WORK_ORDER ";
                                    object[][] Params = new object[2][];
                                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODULE_NAME", lablType.Text };
                                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FUNCTION_NAME", lablRuleName.Text };
                                    DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                                    for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                                    {
                                        DataRow dr = dsTemp.Tables[0].Rows[i];
                                        dgvSNList.Rows.Add();
                                        dgvSNList.Rows[dgvSNList.Rows.Count - 1].Cells["WORK_ORDER"].Value = dr["WORK_ORDER"].ToString(); ;
                                        dgvSNList.Rows[dgvSNList.Rows.Count - 1].Cells["PART_NO"].Value = dr["PART_NO"].ToString(); ;
                                        dgvSNList.Rows[dgvSNList.Rows.Count - 1].Cells["TARGET_QTY"].Value = dr["TARGET_QTY"].ToString(); ;
                                        dgvSNList.Rows[dgvSNList.Rows.Count - 1].Cells["INPUT_QTY"].Value = dr["INPUT_QTY"].ToString(); ;
                                        dgvSNList.Rows[dgvSNList.Rows.Count - 1].Cells["OUTPUT_QTY"].Value = dr["OUTPUT_QTY"].ToString(); ;
                                        dgvSNList.Rows[dgvSNList.Rows.Count - 1].Cells["CHECKED"].Value = "N";
                                    }
                                     */ 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            string sChecked = "N";
            if ((sender as Button).Tag.ToString() == "1")
                sChecked = "Y";

            for (int i = 0; i <= dgvSNList.Rows.Count - 1; i++)
            {
                dgvSNList.Rows[i].Cells["CHECKED"].Value = sChecked;
            }  
        }
    }
}