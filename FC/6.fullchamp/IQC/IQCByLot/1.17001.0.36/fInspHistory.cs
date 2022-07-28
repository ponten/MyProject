using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetFilter;
using SajetClass;
using System.Data.OracleClient;
using Excel;

namespace IQCbyLot
{
    public partial class fInspHistory : Form
    {
        bool g_bFinish;
        string sSQL;
        DataSet dsTemp;
        public string g_sFileName;
        public string g_sLotNoHistory;
        public static string g_sExeName;

        public fInspHistory()
        {
            InitializeComponent();
        }

        private void dtStart_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SetSelectRow(DataGridView GridData, String sPrimaryKey, String sField)
        {
            if (GridData.Rows.Count > 0)
            {
                int iIndex = 0;
                string sShowField = GridData.Columns[0].Name;

                for (int i = 0; i <= GridData.Columns.Count - 1; i++)
                {
                    if (GridData.Columns[i].Visible)
                    {
                        //第一個有顯示的欄位(focus到隱藏欄位會錯誤)
                        sShowField = GridData.Columns[i].Name;
                        break;
                    }
                }

                for (int i = 0; i <= GridData.Rows.Count - 1; i++)
                {
                    if (sPrimaryKey == GridData.Rows[i].Cells[sField].Value.ToString())
                    {
                        iIndex = i;
                        break;
                    }
                }
                GridData.Focus();
                GridData.CurrentCell = GridData.Rows[iIndex].Cells[sShowField];
                GridData.Rows[iIndex].Selected = true;
            }
        }

        private void btnPartFilter_Click(object sender, EventArgs e)　//料號主檔搜尋
        {
            //-----MAX(20110916)----------------------
            if (string.IsNullOrEmpty(editPartNo.Text))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Less Input Keyword"), 0);
                return;
            }
            //----------------------------------------
            string sFilter = editPartNo.Text.Trim();
            sFilter = sFilter + "%";
            string sSQL = " Select A.PART_NO,A.SPEC1  "
                 + "  From SAJET.SYS_PART A "
                 + " WHERE A.ENABLED='Y' "
                 + "   AND A.PART_NO LIKE '" + sFilter + "' "
                 + " ORDER BY A.PART_NO ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                editPartNo.Text = f.dgvData.CurrentRow.Cells["PART_NO"].Value.ToString();
            }
        }

        private void btnVendorFilter_Click(object sender, EventArgs e)  //供應商主檔搜尋
        {
            string sFilter = editVendor.Text.Trim();
            sFilter = sFilter + "%";
            string sSQL = " Select A.VENDOR_CODE,A.VENDOR_NAME  "
                 + "  From SAJET.SYS_VENDOR A "
                 + " WHERE A.ENABLED='Y' "
                 + "   AND A.VENDOR_CODE LIKE '" + sFilter + "' "
                 + " ORDER BY A.VENDOR_CODE ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                editVendor.Text = f.dgvData.CurrentRow.Cells["VENDOR_CODE"].Value.ToString();
            }
        }

        private string GetValueID(string sTableName, string sFieldName, string sValueField, string sValue)
        {
            string sSQL = "SELECT " + sFieldName
               + "  FROM " + sTableName
               + "  WHERE " + sValueField + " = '" + sValue + "' "
               + "    AND ROWNUM = 1";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0][0].ToString();
            else
                return "0";
        }

        private void GetLotResult(string sLotNo)
        {
            dgvLotNo.Rows.Clear();
            dgvLotType.Rows.Clear();
            dgvTypeMemo.Rows.Clear();
            g_bFinish = false;
            editPartNo.Text = editPartNo.Text.Trim();
            editVendor.Text = editVendor.Text.Trim();
            string sPartNo = editPartNo.Text + "%";
            string sVendor = editVendor.Text + "%";
            //-----------------------20110916 before Sql source---------------------------------------------
            //string sSQL = "SELECT A.LOT_NO ,A.LOT_SIZE, A.START_TIME, A.END_TIME,A.LOT_MEMO ,A.RECEIVE_QTY "
            //    + "       ,A.REJECT_QTY,A.LOT,A.DATECODE "
            //    + "       ,SAJET.INSPECTION_RESULT(A.QC_RESULT) QC_RESULT "
            //    + "       ,B.PART_NO,B.SPEC1,B.SPEC2 "
            //    + "       ,C.VENDOR_CODE,C.VENDOR_NAME "
            //    + "       ,E.EMP_NO||'-'||E.EMP_NAME EMP_NAME "
            //    + "       ,A.QC_RESULT QC_RESULT_1 "
            //    + "       ,DECODE(NVL(D.LOT_NO,'N/A'),'N/A','No','Yes') ROHS " 
            //    + "       ,DECODE(A.LOT_LEVEL,'0','Normal','1','Tight','2','Reduce','3','By Pass','N/A') LOT_LEVEL "
            //    + "       ,A.EXCEPTION_MEMO,A.DMDA,A.MATERIAL_VER "
            //    + "       ,A.WAIVE_NO ,A.QC_RESULT QC_RESULT_1 "
            //    + "       ,DECODE(F.LOT_NO,NULL,NULL,'*') REJECT_WAIVE "
            //    + "  FROM SAJET.G_IQC_LOT A "
            //    + "      ,SAJET.SYS_PART B "
            //    + "      ,SAJET.SYS_VENDOR C "
            //    + "      ,SAJET.SYS_EMP E "
            //    + "      ,SAJET.G_IQC_ROHS_RESULT D "
            //    + "      ,SAJET.G_IQC_PREREJECT F "
            //    + " WHERE A.END_TIME >=TO_DATE(:START_TIME,'YYYY/MM/DD') "
            //    + "   AND A.END_TIME <=TO_DATE(:END_TIME,'YYYY/MM/DD HH24:MI:SS') "
            //    + "   AND A.QC_RESULT <>'N/A' "
            //    + "   AND A.PART_ID = B.PART_ID "
            //    + "   AND A.VENDOR_ID = C.VENDOR_ID "
            //    + "   AND B.PART_NO LIKE '" + sPartNo + "' "
            //    + "   AND C.VENDOR_CODE LIKE '" + sVendor + "' "
            //    + "   AND A.EMP_ID = E.EMP_ID(+) "
            //    + "   AND A.LOT_NO = D.LOT_NO(+) "
            //    + "   AND A.LOT_NO = F.LOT_NO(+) "
            //    + "  ORDER BY A.END_TIME DESC ";
            string sSQL = "SELECT A.LOT_NO ,A.LOT_SIZE, A.START_TIME, A.END_TIME,A.LOT_MEMO ,A.RECEIVE_QTY "
                + "       ,A.REJECT_QTY,A.LOT,A.DATECODE "
                + "       ,SAJET.INSPECTION_RESULT(A.QC_RESULT) QC_RESULT "
                + "       ,B.PART_NO,B.SPEC1,B.SPEC2 "
                + "       ,C.VENDOR_CODE,C.VENDOR_NAME "
                + "       ,E.EMP_NO||'-'||E.EMP_NAME EMP_NAME "
                + "       ,A.QC_RESULT QC_RESULT_1 "
                + "       ,DECODE(NVL(D.LOT_NO,'N/A'),'N/A','No','Yes') ROHS "
                + "       ,DECODE(A.LOT_LEVEL,'0','Normal','1','Tight','2','Reduce','3','By Pass','N/A') LOT_LEVEL "
                + "       ,A.EXCEPTION_MEMO,A.DMDA,A.MATERIAL_VER "
                + "       ,A.WAIVE_NO ,A.QC_RESULT QC_RESULT_1 "
                + "       ,DECODE(F.LOT_NO,NULL,NULL,'*') REJECT_WAIVE "
                + "       ,G.MODEL_NAME "
                + "  FROM SAJET.G_IQC_LOT A "
                + "      ,SAJET.SYS_PART B "
                + "      ,SAJET.SYS_VENDOR C "
                + "      ,SAJET.SYS_EMP E "
                + "      ,SAJET.G_IQC_ROHS_RESULT D "
                + "      ,SAJET.G_IQC_PREREJECT F "
                + "      ,SAJET.SYS_MODEL G "
                + " WHERE A.END_TIME >=TO_DATE(:START_TIME,'YYYY/MM/DD') "
                + "   AND A.END_TIME <=TO_DATE(:END_TIME,'YYYY/MM/DD HH24:MI:SS') "
                + "   AND A.QC_RESULT <>'N/A' "
                + "   AND A.PART_ID = B.PART_ID "
                + "   AND A.VENDOR_ID = C.VENDOR_ID "
                + "   AND B.PART_NO LIKE '" + sPartNo + "' "
                + "   AND C.VENDOR_CODE LIKE '" + sVendor + "' "
                + "   AND A.EMP_ID = E.EMP_ID(+) "
                + "   AND A.LOT_NO = D.LOT_NO(+) "
                + "   AND A.LOT_NO = F.LOT_NO(+) "
                + "   AND B.MODEL_ID = G.MODEL_ID(+) "
                + "  ORDER BY A.END_TIME DESC ";
            //    string sPartID = SajetCommon.GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", editPartNo.Text, "Y");
            //    string sVendorID = SajetCommon.GetID("SAJET.SYS_VENDOR", "VENDOR_ID", "VENDOR_CODE", editVendor.Text, "Y");
            object[][] Params = new object[2][];
            string sStartDate = DateTime.Parse(dtStart.Text).ToString("yyyy/MM/dd");
            string sEndDate = DateTime.Parse(dtEnd.Text).ToString("yyyy/MM/dd") + " 23:59:59";
            //Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
            //Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sVendorID };
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "START_TIME", sStartDate };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "END_TIME", sEndDate };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvLotNo.Rows.Add();

                //-------MAX(20110916)--------------
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["SPEC1"].Value = dr["SPEC1"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["SPEC2"].Value = dr["SPEC2"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["MODEL_NAME"].Value = dr["MODEL_NAME"].ToString();
                //----------------------------------

                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["LOT_NO"].Value = dr["LOT_NO"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["LOT_SIZE"].Value = dr["LOT_SIZE"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["START_TIME"].Value = dr["START_TIME"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["END_TIME"].Value = dr["END_TIME"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["LOT_MEMO"].Value = dr["LOT_MEMO"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["RECEIVE_QTY"].Value = dr["RECEIVE_QTY"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["PART_NO"].Value = dr["PART_NO"].ToString();
                if (dr["REJECT_WAIVE"].ToString() == "*")
                {
                    dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["QC_RESULT"].Value = "[" + SajetCommon.SetLanguage("Reject") + "] / " + SajetCommon.SetLanguage(dr["QC_RESULT"].ToString());
                }
                else
                    dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["QC_RESULT"].Value = SajetCommon.SetLanguage(dr["QC_RESULT"].ToString());
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["VENDOR_NAME"].Value = dr["VENDOR_NAME"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["EMP_NAME"].Value = dr["EMP_NAME"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["LOT"].Value = dr["LOT"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["DATECODE"].Value = dr["DATECODE"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["ROHS"].Value = dr["ROHS"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["LOT_LEVEL"].Value = SajetCommon.SetLanguage(dr["LOT_LEVEL"].ToString());
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["EXCEPTION_MEMO"].Value = dr["EXCEPTION_MEMO"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["DMDA"].Value = dr["DMDA"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["MATERIAL_VER"].Value = dr["MATERIAL_VER"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["REJECT_QTY"].Value = dr["REJECT_QTY"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["WAIVE_NO"].Value = dr["WAIVE_NO"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["QC_RESULT_1"].Value = dr["QC_RESULT_1"].ToString();
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["REJECT_WAIVE"].Value = dr["REJECT_WAIVE"].ToString();

                Color c1 = Color.White;//background
                Color c2 = Color.Black;//font

                switch (dr["QC_RESULT_1"].ToString())
                {
                    case "0":
                        {
                            c1 = Color.Green;
                            c2 = Color.White;
                            break;
                        }
                    case "1":
                        {
                            c1 = Color.Red;
                            c2 = Color.White;
                            break;
                        }
                    case "2":
                        {
                            c1 = Color.Yellow; break;

                        }
                    case "4":
                        {
                            c1 = Color.Blue;
                            c2 = Color.White;
                            break;
                        }
                    case "7":
                        {
                            c1 = Color.Orange;
                            break;
                        }
                    default:
                        {
                            c1 = Color.White;
                            c2 = Color.Black;
                            break;
                        }
                }
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["QC_RESULT"].Style.BackColor = c1;
                dgvLotNo.Rows[dgvLotNo.Rows.Count - 1].Cells["QC_RESULT"].Style.ForeColor = c2;
            }
            g_bFinish = true;
            SetSelectRow(dgvLotNo, sLotNo, "LOT_NO");
        }

        private void GetResult1()
        {
            progressBar1.Visible = false;
            editStatus.Visible = false;
            dgvPart.Rows.Clear();
            string sStartDate = DateTime.Parse(dtStart.Text).ToString("yyyy/MM/dd");
            string sEndDate = DateTime.Parse(dtEnd.Text).ToString("yyyy/MM/dd") + " 23:59:59";
            string sPartNo = editPartNo.Text + "%";
            string sVendor = editVendor.Text + "%";

            sSQL = " SELECT B.PART_NO,A.VERSION,C.VENDOR_CODE,C.VENDOR_NAME,A.PASS_LOT,A.REJECT_LOT,A.PART_ID,A.VENDOR_ID  "
                + "       ,TRUNC(PASS_lot / DECODE((PASS_lot+reject_lot),0,1,PASS_lot+reject_lot) *100,2) PASS_RATE  "
                + " FROM "
                + " ( SELECT A.PART_ID,A.VERSION,A.VENDOR_ID,sum(decode(qc_result,'1',1,0)) reject_lot "
                + "       ,sum(decode(qc_result,'N/A',0,'1',0,1)) PASS_lot	"
                + "   FROM SAJET.G_IQC_LOT A "
                + "      ,SAJET.SYS_PART B "
                + "      ,SAJET.SYS_VENDOR C "
                + " WHERE A.END_TIME >=TO_DATE(:START_TIME,'YYYY/MM/DD') "
                + "   AND A.END_TIME <=TO_DATE(:END_TIME,'YYYY/MM/DD HH24:MI:SS') "
                + "   AND A.QC_RESULT <>'N/A' "
                + "   AND B.PART_NO LIKE '" + sPartNo + "' "
                + "   AND C.VENDOR_CODE LIKE '" + sVendor + "' "
                + "   AND A.PART_ID = B.PART_ID "
                + "   AND A.VENDOR_ID = C.VENDOR_ID "
                + "  GROUP BY A.PART_ID ,A.VERSION,A.VENDOR_ID ) A "
                + "    ,SAJET.SYS_PART B "
                + "   ,SAJET.SYS_VENDOR C "
                + " WHERE A.PART_ID = B.PART_ID "
                + "   AND A.VENDOR_ID = C.VENDOR_ID "
                + " ORDER BY B.PART_NO,C.VENDOR_NAME ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "START_TIME", sStartDate };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "END_TIME", sEndDate };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvPart.Rows.Add();
                dgvPart.Rows[dgvPart.Rows.Count - 1].Cells["PART_NO_3"].Value = dr["PART_NO"].ToString();
                dgvPart.Rows[dgvPart.Rows.Count - 1].Cells["VERSION_3"].Value = dr["VERSION"].ToString();
                dgvPart.Rows[dgvPart.Rows.Count - 1].Cells["VENDOR_NAME_3"].Value = dr["VENDOR_NAME"].ToString();
                dgvPart.Rows[dgvPart.Rows.Count - 1].Cells["PASS_LOT"].Value = dr["PASS_LOT"].ToString();
                dgvPart.Rows[dgvPart.Rows.Count - 1].Cells["REJECT_LOT"].Value = dr["REJECT_LOT"].ToString();
                dgvPart.Rows[dgvPart.Rows.Count - 1].Cells["PASS_RATE"].Value = dr["PASS_RATE"].ToString() + "%";
                dgvPart.Rows[dgvPart.Rows.Count - 1].Cells["PART_ID_3"].Value = dsTemp.Tables[0].Rows[i]["PART_ID"].ToString();
                dgvPart.Rows[dgvPart.Rows.Count - 1].Cells["VENDOR_ID_3"].Value = dsTemp.Tables[0].Rows[i]["VENDOR_ID"].ToString();
                dgvPart.Rows[dgvPart.Rows.Count - 1].Cells["DEFECT_DATA"].Value = string.Empty;
            }

            sSQL = " SELECT A.PART_ID,A.VENDOR_ID,B.DEFECT_CODE,B.DEFECT_DESC "
                + "   FROM  "
                + " (SELECT A.PART_ID,A.VENDOR_ID,D.DEFECT_ID	"
                + "   FROM SAJET.G_IQC_LOT A   "
                + "       ,SAJET.SYS_PART B "
                + "      ,SAJET.SYS_VENDOR C "
                + "       ,SAJET.G_IQC_TEST_TYPE_DEFECT D "
                + " WHERE A.END_TIME >=TO_DATE(:START_TIME,'YYYY/MM/DD') "
                + "   AND A.END_TIME <=TO_DATE(:END_TIME,'YYYY/MM/DD HH24:MI:SS') "
               + "   AND A.QC_RESULT <>'N/A' "
               + "   AND B.PART_NO LIKE '" + sPartNo + "' "
               + "   AND C.VENDOR_CODE LIKE '" + sVendor + "' "
               + "   AND A.PART_ID = B.PART_ID "
               + "   AND A.VENDOR_ID = C.VENDOR_ID "
               + "   AND A.LOT_NO = D.LOT_NO  "
               + " GROUP BY A.PART_ID,A.VENDOR_ID,D.DEFECT_ID ) A"
               + "  ,SAJET.SYS_DEFECT B "
               + " WHERE A.DEFECT_ID = B.DEFECT_ID ";
            Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "START_TIME", sStartDate };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "END_TIME", sEndDate };
            DataSet dsDefect = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsDefect.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsDefect.Tables[0].Rows[i];
                for (int j = 0; j <= dgvPart.Rows.Count - 1; j++)
                {
                    if ((dgvPart.Rows[j].Cells["PART_ID_3"].Value.ToString() == dr["PART_ID"].ToString())
                        && (dgvPart.Rows[j].Cells["VENDOR_ID_3"].Value.ToString() == dr["VENDOR_ID"].ToString()))
                    {
                        dgvPart.Rows[j].Cells["DEFECT_DATA"].Value = dgvPart.Rows[j].Cells["DEFECT_DATA"].Value + dr["DEFECT_CODE"].ToString() + "(" + dr["DEFECT_DESC"].ToString() + ") ,";
                    }
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            dgvLotNo.Columns["QC_RESULT"].Frozen = true;
            GetLotResult("");

            GetResult1();

            editPartNo.Text = editPartNo.Text.Trim();
            editVendor.Text = editVendor.Text.Trim();
            string sPartNo = editPartNo.Text + "%";
            string sVendor = editVendor.Text + "%";
            string sStartDate = DateTime.Parse(dtStart.Text).ToString("yyyy/MM/dd");
            string sEndDate = DateTime.Parse(dtEnd.Text).ToString("yyyy/MM/dd") + " 23:59:59";
            /*
            //wip
            dgvWIP.Rows.Clear();
            sSQL = " SELECT    A.ITEM_DESC,TO_CHAR(A.REQUEST_DATE,'YYYY/MM/DD') REQUEST_DATE ,A.VERSION "
                           + "       ,DECODE(A.ITEM_STATUS,'0','Wait Approve','1','Approved',A.ITEM_STATUS) ITEM_STATUS_DESC,A.ITEM_STATUS  "
                           + "       ,F.EMP_NO APPROVE_EMP_NO,F.EMP_NAME  APPROVE_EMP_NAME ,A.APPROVE_TIME"
                           + "       ,B.PART_NO "
                           + "       ,C.VENDOR_CODE,C.VENDOR_NAME "
                           + "       ,E.EMP_NO||'-'||E.EMP_NAME EMP_NAME "
                           + "  FROM SAJET.G_PRODUCT_COMPLAIN A "
                           + "      ,SAJET.SYS_PART B "
                           + "      ,SAJET.SYS_VENDOR C "
                           + "      ,SAJET.SYS_EMP E "
                           + "      ,SAJET.SYS_EMP F "
                           + " WHERE A.REQUEST_DATE >=TO_DATE(:START_TIME,'YYYY/MM/DD') "
                           + "   AND A.REQUEST_DATE <=TO_DATE(:END_TIME,'YYYY/MM/DD HH24:MI:SS') "
                           + "   AND A.ITEM_TYPE ='1' "//
                           + "   AND A.PART_ID = B.PART_ID "
                           + "   AND A.VENDOR_ID = C.VENDOR_ID "
                           + "   AND A.UPDATE_USERID = E.EMP_ID(+) "
                           + "   AND A.APPROVE_USERID = F.EMP_ID(+) "
                           + "   AND B.PART_NO LIKE '" + sPartNo + "' "
                           + "   AND C.VENDOR_CODE LIKE '" + sVendor + "' "
                           + "  ORDER BY A.REQUEST_DATE DESC,B.PART_NO,A.ITEM_STATUS ";
            object[][] Params = new object[2][];
            //  Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
            //  Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sVendorID };
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "START_TIME", sStartDate };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "END_TIME", sEndDate };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvWIP.Rows.Add();
                dgvWIP.Rows[dgvWIP.Rows.Count - 1].Cells["PART_NO_1"].Value = dr["PART_NO"].ToString();
                dgvWIP.Rows[dgvWIP.Rows.Count - 1].Cells["VERSION"].Value = dr["VERSION"].ToString();
                dgvWIP.Rows[dgvWIP.Rows.Count - 1].Cells["VENDOR_NAME_1"].Value = dr["VENDOR_NAME"].ToString();
                dgvWIP.Rows[dgvWIP.Rows.Count - 1].Cells["ITEM_DESC"].Value = dr["ITEM_DESC"].ToString();
                dgvWIP.Rows[dgvWIP.Rows.Count - 1].Cells["REQUEST_DATE"].Value = dr["REQUEST_DATE"].ToString();
                dgvWIP.Rows[dgvWIP.Rows.Count - 1].Cells["ITEM_STATUS"].Value = SajetCommon.SetLanguage(dr["ITEM_STATUS_DESC"].ToString());
                dgvWIP.Rows[dgvWIP.Rows.Count - 1].Cells["APPROVE_EMP_NAME"].Value = dsTemp.Tables[0].Rows[i]["APPROVE_EMP_NAME"].ToString();
                dgvWIP.Rows[dgvWIP.Rows.Count - 1].Cells["APPROVE_TIME"].Value = dsTemp.Tables[0].Rows[i]["APPROVE_TIME"].ToString();
            }
             */ 
        }

        private void fInspHistory_Load(object sender, EventArgs e)
        {
            this.Text = "IQC History";
            dtEnd.Value = DateTime.Now;
            dtStart.Value = DateTime.Now.AddMonths(-3);
            SajetCommon.SetLanguageControl(this);
            tabPage2.Parent = null;
            this.dgvLotNo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvItemNoValue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvLotType.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvWIP.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRoHSItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRejectWaive.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTypeMemo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            tabPage2.Parent = null;
        }

        private void GetTestItem(string sLotNo)
        {
            dgvLotType.Rows.Clear();
            dgvTypeMemo.Rows.Clear();
            string sSQL = string.Empty;
            sSQL = "SELECT A.LOT_NO,A.ITEM_TYPE_ID,NVL(D.ITEM_TYPE_CODE,A.ITEM_TYPE_ID) ITEM_TYPE_CODE ,D.ITEM_TYPE_NAME  "
                 + "      ,NVL(C.SAMPLING_TYPE,'N/A') SAMPLE_TYPE,A.SAMPLING_PLAN_ID SAMPLE_ID "
                 + "      ,SAJET.SAMPLE_LEVEL(A.SAMPLING_LEVEL) SAMPLE_LEVEL "
                 + "      ,DECODE(A.QC_RESULT,'0','OK','1','NG','N/A') INSP_RESULT "
                 + "      ,A.SAMPLING_SIZE SAMPLE_SIZE ,A.PASS_QTY + A.FAIL_QTY CHECK_QTY ,A.PASS_QTY,A.FAIL_QTY "
                 + "      ,A.START_TIME, A.END_TIME "
                 + "      ,E.EMP_NO||'-'||E.EMP_NAME EMP_NAME "
                 + "      ,A.MEMO "
                 + "  FROM SAJET.G_IQC_TEST_TYPE A "
                 + "  LEFT JOIN SAJET.SYS_QC_SAMPLING_PLAN C ON A.SAMPLING_PLAN_ID = C.SAMPLING_ID "
                 + "  LEFT JOIN SAJET.SYS_TEST_ITEM_TYPE D ON A.ITEM_TYPE_ID = D.ITEM_TYPE_ID "
                 + "  LEFT JOIN SAJET.SYS_EMP E ON A.INSP_EMPID = E.EMP_ID "
                 + " WHERE A.LOT_NO =:LOT_NO "
                 + " ORDER BY D.ITEM_TYPE_CODE ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvLotType.Rows.Add();
                int iIndex = dgvLotType.Rows.Count - 1;
                dgvLotType.Rows[iIndex].Cells["TYPE_CODE"].Value = dr["ITEM_TYPE_CODE"].ToString();
                dgvLotType.Rows[iIndex].Cells["TYPE_NAME"].Value = dr["ITEM_TYPE_NAME"].ToString();
                dgvLotType.Rows[iIndex].Cells["SAMPLE_TYPE"].Value = dr["SAMPLE_TYPE"].ToString();
                dgvLotType.Rows[iIndex].Cells["SAMPLE_LEVEL"].Value = SajetCommon.SetLanguage(dr["SAMPLE_LEVEL"].ToString());
                dgvLotType.Rows[iIndex].Cells["SAMPLE_SIZE"].Value = dr["SAMPLE_SIZE"].ToString();
                dgvLotType.Rows[iIndex].Cells["PASS_QTY"].Value = dr["PASS_QTY"].ToString();
                dgvLotType.Rows[iIndex].Cells["FAIL_QTY"].Value = dr["FAIL_QTY"].ToString();
                dgvLotType.Rows[iIndex].Cells["INSP_RESULT"].Value = dr["INSP_RESULT"].ToString();
                dgvLotType.Rows[iIndex].Cells["START_TIME1"].Value = dr["START_TIME"].ToString();
                dgvLotType.Rows[iIndex].Cells["END_TIME1"].Value = dr["END_TIME"].ToString();
                dgvLotType.Rows[iIndex].Cells["EMP_NAME1"].Value = dr["EMP_NAME"].ToString();
                dgvLotType.Rows[iIndex].Cells["ITEM_TYPE_ID"].Value = dr["ITEM_TYPE_ID"].ToString();
                dgvLotType.Rows[iIndex].Cells["LOT_NO1"].Value = dr["LOT_NO"].ToString();
                if (dr["INSP_RESULT"].ToString() == "NG")
                {
                    dgvLotType.Rows[dgvLotType.Rows.Count - 1].Cells["INSP_RESULT"].Style.BackColor = Color.Red;
                    dgvLotType.Rows[dgvLotType.Rows.Count - 1].Cells["INSP_RESULT"].Style.ForeColor = Color.White;
                }
                dgvLotType.Rows[iIndex].Cells["TESTTYPEMEMO"].Value = dr["MEMO"].ToString();
                dgvTypeMemo.Rows.Add();
                dgvTypeMemo.Rows[iIndex].Cells["TYPE_CODE_1"].Value = dr["ITEM_TYPE_CODE"].ToString();
                dgvTypeMemo.Rows[iIndex].Cells["TYPE_NAME_1"].Value = dr["ITEM_TYPE_NAME"].ToString();
                dgvTypeMemo.Rows[iIndex].Cells["TYPE_MEMO_1"].Value = dr["MEMO"].ToString();

            }
        }

        private void GetRoHSItem(string sLotNo)
        {
            dgvRoHSItem.Rows.Clear();
            editRoHSMemo.Text = string.Empty;
            sSQL = "SELECT POSITION,PB,CD,HG,CR,BR,CL,MEMO "
                + "  FROM SAJET.G_IQC_ROHS_ITEM "
                + " WHERE LOT_NO =:LOT_NO "
                + " ORDER BY POSITION ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_no", sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvRoHSItem.Rows.Add();
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["POSITION"].Value = dr["POSITION"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["PB"].Value = dr["PB"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["CD"].Value = dr["CD"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["HG"].Value = dr["HG"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["CR"].Value = dr["CR"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["BR"].Value = dr["BR"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["CL"].Value = dr["CL"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["MEMO"].Value = dr["MEMO"];
            }
            string sQCResult = "N/A";
            sSQL = "SELECT DECODE(NVL(QC_RESULT,'N/A'),'0','OK','1','NG',NVL(QC_RESULT,'N/A')) QC_RESULT,MEMO "
                 + "  FROM SAJET.G_IQC_ROHS_RESULT "
                 + " WHERE LOT_NO =:LOT_NO "
                 + "   AND ROWNUM = 1 ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sQCResult = dsTemp.Tables[0].Rows[0]["QC_RESULT"].ToString();
                editRoHSMemo.Text = dsTemp.Tables[0].Rows[0]["MEMO"].ToString();
            }
            lablRoHSResult.Text = sQCResult;

            //    editRoHSResult.Text =SajetCommon.SetLanguage("RoHS Result",1)+" : "+  sQCResult;
        }

        private void dgvLotNo_SelectionChanged(object sender, EventArgs e)
        {
            editLotNo.Text = "N/A";
            dgvLotType.Rows.Clear();
            dgvRoHSItem.Rows.Clear();
            dgvRejectWaive.Rows.Clear();
            dgvTypeMemo.Rows.Clear();
            dgvItemNoValue.Rows.Clear();
            dgvItemNoValue.Columns.Clear();
            dgvItemValue.Rows.Clear();
            dgvItemValue.Columns.Clear();

            tPageRejectWaive.Parent = null;
            if (dgvLotNo.Rows.Count == 0 || dgvLotNo.CurrentRow == null || dgvLotNo.SelectedCells.Count == 0)
            {
                return;
            }
            if (!g_bFinish)
                return;
            btnRejectReport.Enabled = false;
            btnRejectReport.Enabled = (dgvLotNo.CurrentRow.Cells["QC_RESULT_1"].Value.ToString() == "1" ||
                                      (dgvLotNo.CurrentRow.Cells["QC_RESULT_1"].Value.ToString() == "2" &&
                                       dgvLotNo.CurrentRow.Cells["REJECT_WAIVE"].Value.ToString() == "*"));
            string sLotNo = dgvLotNo.CurrentRow.Cells["LOT_NO"].Value.ToString();
            string sRejectWaive = dgvLotNo.CurrentRow.Cells["REJECT_WAIVE"].Value.ToString();
            if (sRejectWaive == "*")
            {
                tPageRejectWaive.Parent = tabControl2;
            }
            editLotNo.Text = sLotNo;
            GetTestItem(sLotNo);
            GetRoHSItem(sLotNo);
            GetRejectWaive(sLotNo);
            //GetTestItemValues(sLotNo);
            GetTestItemNoValue(sLotNo);
            GetTestItemValue(sLotNo);
        }
        private void GetTestItemValue(string sLotNo)
        {
            dgvItemValue.Rows.Clear();
            string sSQL = "SELECT NVL(MAX(A.ITEM_SEQ),0) MAX_SEQ FROM SAJET.G_IQC_TEST_ITEM_VALUE A "
                + " WHERE A.LOT_NO =:LOT_NO ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            int iMaxSeq = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MAX_SEQ"].ToString());
            this.dgvItemValue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvItemValue.Columns.Add("ITEM_CODE1", SajetCommon.SetLanguage("Item Code"));
            dgvItemValue.Columns.Add("ITEM_NAME1", SajetCommon.SetLanguage("Item Name"));
            dgvItemValue.Columns.Add("SAMPLING_SIZE1", SajetCommon.SetLanguage("Sample Size"));
            dgvItemValue.Columns.Add("PASS_QTY1", SajetCommon.SetLanguage("Pass Qty"));
            dgvItemValue.Columns.Add("FAIL_QTY1", SajetCommon.SetLanguage("Fail Qty"));
            
            dgvItemValue.Columns["ITEM_CODE1"].Width = 150;
            dgvItemValue.Columns["ITEM_NAME1"].Width = 150;
            dgvItemValue.Columns["PASS_QTY1"].Width = 150;
            dgvItemValue.Columns["FAIL_QTY1"].Width = 150;
            dgvItemValue.Columns["SAMPLING_SIZE1"].Width = 150;
            for (int i = 1; i <= iMaxSeq; i++)
            {
                dgvItemValue.Columns.Add(i.ToString(), i.ToString());
                dgvItemValue.Columns[i + 1].Width = 45;
            }
            sSQL = " select a.item_code,a.item_name, c.SAMPLING_SIZE ,c.SAMPLING_SIZE - a.fail_qty pass_qty,a.fail_qty,d.item_seq,d.value,d.result "
                     + " from  "
                     + " (select a.item_id ,b.item_code,b.item_name ,a.pass_qty, a.fail_qty,a.memo,b.item_type_id "
                     + " from sajet.g_iqc_test_item a ,SAJET.SYS_TEST_ITEM B  "
                     + "  where a.lot_no=:LOT_NO "
                     + " and a.item_id = b.item_id  "
                     + " ) a "
                     + ",( select item_id,sum(decode(result,0,1,0)) pass_qty,sum(decode(result,1,1,0)) fail_qty "
                     + "     from sajet.g_iqc_test_item_value "
                     + "  where lot_no=:LOT_NO "
                     + "  group by item_id ) b "
                     + " ,(SELECT ITEM_TYPE_ID , SAMPLING_SIZE FROM SAJET.G_IQC_TEST_TYPE WHERE LOT_NO=:LOT_NO ) c "//抽驗數量
                     + " ,( select item_id,value,result,item_seq "
                     + "      from sajet.g_iqc_test_item_value "
                     + "  where lot_no=:LOT_NO "
                     + " ) d "
                     + " where a.item_id = b.item_id  "
                     + " and a.item_id = d.item_id "
                     + "   and a.item_type_id = c.item_type_id  "
                     + "  order by a.item_code ";
            
            dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
              ComboBox combItemCode = new ComboBox();
              for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
              {
                  DataRow dr = dsTemp.Tables[0].Rows[i];
                  if (combItemCode.Items.IndexOf(dr["ITEM_CODE"].ToString()) < 0)
                  {
                      dgvItemValue.Rows.Add();
                      dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells["ITEM_CODE1"].Value = dr["ITEM_CODE"].ToString();
                      dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells["ITEM_NAME1"].Value = dr["ITEM_NAME"].ToString();
                      dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells["PASS_QTY1"].Value = dr["PASS_QTY"].ToString();
                      dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells["FAIL_QTY1"].Value = dr["FAIL_QTY"].ToString();
                      dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells["SAMPLING_SIZE1"].Value = dr["SAMPLING_SIZE"].ToString();
                      combItemCode.Items.Add(dr["ITEM_CODE"].ToString());
                  }
                  if (!string.IsNullOrEmpty(dr["VALUE"].ToString()))
                  {
                      dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells[dr["ITEM_SEQ"].ToString()].Value = dr["VALUE"].ToString();
                  }
                  if (dr["RESULT"].ToString() == "1")
                  {
                      dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells[dr["ITEM_SEQ"].ToString()].Style.BackColor = Color.Red;
                      dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells[dr["ITEM_SEQ"].ToString()].Style.ForeColor = Color.White;
                  }
              }
        }
        private void GetTestItemNoValue(string sLotNo)
        {
          
            dgvItemNoValue.Rows.Clear();
            dgvItemNoValue.Columns.Add("ITEM_CODE", SajetCommon.SetLanguage("Item Code"));
            dgvItemNoValue.Columns.Add("ITEM_NAME", SajetCommon.SetLanguage("Item Name"));
            dgvItemNoValue.Columns.Add("SAMPLING_SIZE", SajetCommon.SetLanguage("Sample Size"));
            dgvItemNoValue.Columns.Add("PASS_QTY", SajetCommon.SetLanguage("Pass Qty"));
            dgvItemNoValue.Columns.Add("FAIL_QTY", SajetCommon.SetLanguage("Fail Qty"));
            
            dgvItemNoValue.Columns.Add("MEMO", SajetCommon.SetLanguage("Memo"));
            dgvItemNoValue.Columns["ITEM_CODE"].Width = 150;
            dgvItemNoValue.Columns["ITEM_NAME"].Width = 150;
            dgvItemNoValue.Columns["PASS_QTY"].Width = 150;
            dgvItemNoValue.Columns["FAIL_QTY"].Width = 150;
            dgvItemNoValue.Columns["SAMPLING_SIZE"].Width = 150;
            dgvItemNoValue.Columns["MEMO"].Width = 250;

            string sSQL = " select a.item_code,a.item_name, a.pass_qty + a.fail_qty SAMPLING_SIZE,a.pass_qty,a.fail_qty,a.memo "
            + " from  "
            + " (select a.item_id ,b.item_code,b.item_name ,a.pass_qty, a.fail_qty,a.memo,b.item_type_id "
            + " from sajet.g_iqc_test_item a ,SAJET.SYS_TEST_ITEM B  "
            + "  where a.lot_no=:LOT_NO "
            + " and a.item_id = b.item_id  "
            + " ) a "
            + ",( select item_id,sum(decode(result,0,1,0)) pass_qty,sum(decode(result,1,1,0)) fail_qty "
            + "     from sajet.g_iqc_test_item_value "
            + "  where lot_no=:LOT_NO "
            + "  group by item_id ) b "
            + " ,(SELECT ITEM_TYPE_ID , SAMPLING_SIZE FROM SAJET.G_IQC_TEST_TYPE WHERE LOT_NO=:LOT_NO ) c "//抽驗數量
            + " where a.item_id = b.item_id(+) "
            + "   and a.item_type_id = c.item_type_id  "
            + "   and b.item_id is null "
            + "  order by a.item_code ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvItemNoValue.Rows.Add();
                dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["ITEM_CODE"].Value = dr["ITEM_CODE"].ToString();
                dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["ITEM_NAME"].Value = dr["ITEM_NAME"].ToString();
                dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["PASS_QTY"].Value = dr["PASS_QTY"].ToString();
                dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["FAIL_QTY"].Value = dr["FAIL_QTY"].ToString();
                dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["SAMPLING_SIZE"].Value = dr["SAMPLING_SIZE"].ToString();
                dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["MEMO"].Value = dr["MEMO"].ToString();
            }

        }
        /*
        private void GetTestItemValues(string sLotNo)
        {
            sSQL = "SELECT NVL(MAX(A.ITEM_SEQ),0) MAX_SEQ FROM SAJET.G_IQC_TEST_ITEM_VALUE A "
                 + " WHERE A.LOT_NO =:LOT_NO ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            int iMaxSeq = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MAX_SEQ"].ToString());
            this.dgvItemNoValue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvItemNoValue.Columns.Add("ITEM_CODE", SajetCommon.SetLanguage("Item Code"));
            dgvItemNoValue.Columns.Add("ITEM_NAME", SajetCommon.SetLanguage("Item Name"));
            dgvItemNoValue.Columns.Add("PASS_QTY1", SajetCommon.SetLanguage("Pass Qty"));
            dgvItemNoValue.Columns.Add("FAIL_QTY1", SajetCommon.SetLanguage("Fail Qty"));
            dgvItemNoValue.Columns.Add("SAMPLING_SIZE1", SajetCommon.SetLanguage("Sample Size"));
            dgvItemNoValue.Columns["ITEM_CODE"].Width = 150;
            dgvItemNoValue.Columns["ITEM_NAME"].Width = 150;
            dgvItemNoValue.Columns["PASS_QTY1"].Width = 150;
            dgvItemNoValue.Columns["FAIL_QTY1"].Width = 150;
            dgvItemNoValue.Columns["SAMPLING_SIZE1"].Width = 150;
            for (int i = 1; i <= iMaxSeq; i++)
            {
                dgvItemNoValue.Columns.Add(i.ToString(), i.ToString());
                dgvItemNoValue.Columns[i + 1].Width = 45;
            }
         
            sSQL = "SELECT C.*,D.ITEM_SEQ,D.VALUE, NVL(D.RESULT,'N/A') RESULT, E.SAMPLING_SIZE   FROM ( "
                          + " SELECT B.ITEM_CODE,B.ITEM_NAME,A.PASS_QTY,A.FAIL_QTY,A.ITEM_ID,B.ITEM_TYPE_ID  "
                          + " FROM SAJET.G_IQC_TEST_ITEM A       ,SAJET.SYS_TEST_ITEM B "
                          + " WHERE A.LOT_NO ='" + sLotNo + "' "
                          + " AND A.ITEM_ID = B.ITEM_ID ) C,("
                          + " SELECT B.ITEM_CODE,B.ITEM_NAME,A.ITEM_SEQ,A.VALUE,NVL(A.RESULT,'N/A') RESULT,A.ITEM_ID ,B.ITEM_TYPE_ID"
                          + " FROM SAJET.G_IQC_TEST_ITEM_VALUE A,SAJET.SYS_TEST_ITEM B "
                          + " WHERE A.LOT_NO ='" + sLotNo + "' "
                          + " AND A.ITEM_ID = B.ITEM_ID  ORDER BY B.ITEM_CODE,A.ITEM_SEQ) D "
                          + "  ,(SELECT ITEM_TYPE_ID , SAMPLING_SIZE FROM SAJET.G_IQC_TEST_TYPE WHERE LOT_NO='" + sLotNo + "' ) E  "
                          + " WHERE C.ITEM_ID = D.ITEM_ID(+) "
                          + "   AND C.ITEM_TYPE_ID = E.ITEM_TYPE_ID(+) "
                          + " ORDER BY C.ITEM_CODE ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            ComboBox combItemCode = new ComboBox();
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                if (combItemCode.Items.IndexOf(dr["ITEM_CODE"].ToString()) < 0)
                {
                    dgvItemNoValue.Rows.Add();
                    dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["ITEM_CODE"].Value = dr["ITEM_CODE"].ToString();
                    dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["ITEM_NAME"].Value = dr["ITEM_NAME"].ToString();
                    dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["PASS_QTY1"].Value = dr["PASS_QTY"].ToString();
                    dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["FAIL_QTY1"].Value = dr["FAIL_QTY"].ToString();
                    dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["SAMPLING_SIZE1"].Value = dr["SAMPLING_SIZE"].ToString();
                    combItemCode.Items.Add(dr["ITEM_CODE"].ToString());
                }
                if (!string.IsNullOrEmpty(dr["VALUE"].ToString()))
                {
                    dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells[dr["ITEM_SEQ"].ToString()].Value = dr["VALUE"].ToString();
                }
                if (dr["RESULT"].ToString() == "1")
                {
                    dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells[dr["ITEM_SEQ"].ToString()].Style.BackColor = Color.Red;
                    dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells[dr["ITEM_SEQ"].ToString()].Style.ForeColor = Color.White;
                }
                
            }
        }
         */ 


        private void GetRejectWaive(string sLotNo)
        {
            sSQL = "SELECT B.EMP_NAME,A.PREREJECT_TIME,A.MEMO "
                 + "  FROM SAJET.G_IQC_PREREJECT A ,SAJET.SYS_EMP B "
                 + " WHERE A.LOT_NO =:LOT_NO "
                 + "   AND A.EMP_ID = B.EMP_ID(+) "
                 + "   AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvRejectWaive.Rows.Add();
                dgvRejectWaive.Rows[dgvRejectWaive.Rows.Count - 1].Cells["PREREJECT_TIME"].Value = dr["PREREJECT_TIME"];
                dgvRejectWaive.Rows[dgvRejectWaive.Rows.Count - 1].Cells["REJ_EMP_NAME"].Value = dr["EMP_NAME"];
                dgvRejectWaive.Rows[dgvRejectWaive.Rows.Count - 1].Cells["REJ_MEMO"].Value = dr["MEMO"];
            }

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLotType.Rows.Count == 0 || dgvLotType.CurrentRow == null || dgvLotType.SelectedCells.Count == 0)
            {
                return;
            }

            string sLotNo = dgvLotType.CurrentRow.Cells["LOT_NO1"].Value.ToString();
            string sTypeID = dgvLotType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();
            string sTypeName = dgvLotType.CurrentRow.Cells["TYPE_NAME"].Value.ToString();
            fInspHistoryDetail fDetail = new fInspHistoryDetail();

            try
            {
                fDetail.g_sLotNo = sLotNo;
                fDetail.g_sTypeID = sTypeID;
                fDetail.g_sTypeName = sTypeName;
                fDetail.ShowDialog();

            }
            finally
            {
                fDetail.Dispose();
            }
        }

        private void dgvLotType_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void modifyMemoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLotType.Rows.Count == 0 || dgvLotType.CurrentRow == null || dgvLotType.SelectedCells.Count == 0)
            {
                return;
            }

            string sLotNo = dgvLotType.CurrentRow.Cells["LOT_NO1"].Value.ToString();
            string sTypeID = dgvLotType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();
            string sTypeName = dgvLotType.CurrentRow.Cells["TYPE_NAME"].Value.ToString();
            string sTypeMemo = dgvLotType.CurrentRow.Cells["TESTTYPEMEMO"].Value.ToString();

            fTypeMemo fData = new fTypeMemo();

            try
            {
                fData.g_sLotNo = sLotNo;
                fData.g_sTypeID = sTypeID;
                fData.g_sTypeName = sTypeName;
                fData.editTypeMemo.Text = sTypeMemo;
                if (fData.ShowDialog() == DialogResult.OK)
                {
                    string sMemo = fData.editTypeMemo.Text.Trim();
                    object[][] Params = new object[3][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MEMO", sMemo };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sTypeID };
                    sSQL = "UPDATE SAJET.G_IQC_TEST_TYPE "
                         + "   SET MEMO = :MEMO "
                         + "  WHERE LOT_NO =:LOT_NO "
                         + "    AND ITEM_TYPE_ID =:ITEM_TYPE_ID ";
                    DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    GetTestItem(sLotNo);
                }
            }
            finally
            {
                fData.Dispose();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (dgvLotNo.Rows.Count == 0 || dgvLotNo.CurrentRow == null || dgvLotNo.SelectedCells.Count == 0)
            {
                return;
            }

            string sLotNo = dgvLotNo.CurrentRow.Cells["LOT_NO"].Value.ToString();
            string sLotExceptionMemo = dgvLotNo.CurrentRow.Cells["EXCEPTION_MEMO"].Value.ToString();
            string sLotSize = dgvLotNo.CurrentRow.Cells["LOT_SIZE"].Value.ToString();
            string sReceiveQty = dgvLotNo.CurrentRow.Cells["RECEIVE_QTY"].Value.ToString();
            string sWaiveNo = dgvLotNo.CurrentRow.Cells["WAIVE_NO"].Value.ToString();

            fLotExceptionMemo fData = new fLotExceptionMemo();

            try
            {
                fData.g_sLotNo = sLotNo;
                fData.editTypeMemo.Text = sLotExceptionMemo;
                fData.editLotsize.Text = sLotSize;
                fData.editReceiveQty.Text = sReceiveQty;
                fData.editWaiveNo.Text = sWaiveNo;
                //fData.editLotsize.Enabled = false;
                //fData.editReceiveQty.Enabled = false;
                fData.Text = "Modify Lot Data";

                if (fData.ShowDialog() == DialogResult.OK)
                {
                    string sMemo = fData.editTypeMemo.Text.Trim();
                    sLotSize = fData.editLotsize.Text.Trim();
                    sReceiveQty = fData.editReceiveQty.Text.Trim();
                    sWaiveNo = fData.editWaiveNo.Text.Trim();
                    object[][] Params = new object[5][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EXCEPTION_MEMO", sMemo };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_SIZE", sLotSize };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECEIVE_QTY", sReceiveQty };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WAIVE_NO", sWaiveNo };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
                    sSQL = "UPDATE SAJET.G_IQC_LOT "
                         + "   SET EXCEPTION_MEMO = :EXCEPTION_MEMO "
                         + "      ,LOT_SIZE = :LOT_SIZE "
                         + "      ,RECEIVE_QTY =:RECEIVE_QTY "
                         + "      ,WAIVE_NO =:WAIVE_NO "
                         + "  WHERE LOT_NO =:LOT_NO ";

                    DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    GetLotResult(sLotNo);
                }
            }
            finally
            {
                fData.Dispose();
            }
        }

        private void modifyLotSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            if (dgvLotNo.Rows.Count == 0 || dgvLotNo.CurrentRow == null || dgvLotNo.SelectedCells.Count == 0)
            {
                return;
            }

            string sLotNo = dgvLotNo.CurrentRow.Cells["LOT_NO"].Value.ToString();
            string sLotExceptionMemo = dgvLotNo.CurrentRow.Cells["EXCEPTION_MEMO"].Value.ToString();
            string sLotSize = dgvLotNo.CurrentRow.Cells["LOT_SIZE"].Value.ToString();
            string sReceiveQty = dgvLotNo.CurrentRow.Cells["RECEIVE_QTY"].Value.ToString();
            fLotExceptionMemo fData = new fLotExceptionMemo();

            try
            {
                fData.g_sLotNo = sLotNo;
                fData.editTypeMemo.Text = sLotExceptionMemo;
                fData.editLotsize.Text = sLotSize;
                fData.editReceiveQty.Text = sReceiveQty;
                fData.editTypeMemo.Enabled = false;
                fData.Text = "Modify Lot Size";

                if (fData.ShowDialog() == DialogResult.OK)
                {
                    sLotSize = fData.editLotsize.Text.Trim();
                    sReceiveQty = fData.editReceiveQty.Text.Trim();
                    object[][] Params = new object[3][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_SIZE", sLotSize };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECEIVE_QTY", sReceiveQty };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
                    sSQL = "UPDATE SAJET.G_IQC_LOT "
                         + "   SET LOT_SIZE = :LOT_SIZE "
                         + "      ,RECEIVE_QTY =:RECEIVE_QTY "
                         + "  WHERE LOT_NO =:LOT_NO ";
                    DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    GetLotResult(sLotNo);
                }
            }
            finally
            {
                fData.Dispose();
            }
             */
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvPart.Columns.Count == 0)
                return;
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;
            ExportOfficeExcel.ExcelEdit ExcelClass = new ExportOfficeExcel.ExcelEdit();

            try
            {
                ExcelClass.Create();

                // ExcelClass.Open(editFileName.Text);
                // int iPageCount = ExcelClass.GetSheetCount();

                progressBar1.Visible = true;
                editStatus.Visible = true;
                progressBar1.Maximum = dgvPart.Rows.Count;
                progressBar1.Value = 0;
                editStatus.Text = string.Empty;
                int iCount = 0;
                int iStartRow = 1;
                ExcelClass.SetCellValue("Sheet1", 1, 1, "No");
                ExcelClass.SetCellColor("Sheet1", 1, 1, 1, 1, null, Color.Yellow);
                int iVisibleCol = 0;
                for (int i = 1; i <= dgvPart.Columns.Count; i++)
                {
                    if (dgvPart.Columns[i - 1].Visible)
                    {
                        iVisibleCol += 1;
                        ExcelClass.SetCellValue("Sheet1", i + 1, 1, "'" + dgvPart.Columns[i - 1].HeaderText.ToString());
                        ExcelClass.SetCellColor("Sheet1", i + 1, 1, i + 1, 1, null, Color.Yellow);
                    }
                }

                iStartRow += 1;
                for (int i = 0; i <= dgvPart.Rows.Count - 1; i++)
                {
                    iCount += 1;
                    string sDefectData = string.Empty;
                    /*
                    if (!string.IsNullOrEmpty(dgvPart.Rows[i].Cells["DEFECT_DATA"].Value.ToString()))
                    {
                        sDefectData = dgvPart.Rows[i].Cells["DEFECT_DATA"].Value.ToString().TrimEnd(',');
                    }
                     */
                    ExcelClass.SetCellValue("Sheet1", 1, iStartRow + i, (i + 1).ToString());
                    ExcelClass.SetCellValue("Sheet1", 2, iStartRow + i, "'" + dgvPart.Rows[i].Cells["PART_NO_3"].Value);
                    ExcelClass.SetCellValue("Sheet1", 3, iStartRow + i, "'" + dgvPart.Rows[i].Cells["VERSION_3"].Value);
                    ExcelClass.SetCellValue("Sheet1", 4, iStartRow + i, "'" + dgvPart.Rows[i].Cells["VENDOR_NAME_3"].Value);
                    ExcelClass.SetCellValue("Sheet1", 5, iStartRow + i, "'" + dgvPart.Rows[i].Cells["PASS_LOT"].Value);
                    ExcelClass.SetCellValue("Sheet1", 6, iStartRow + i, "'" + dgvPart.Rows[i].Cells["REJECT_LOT"].Value);
                    ExcelClass.SetCellValue("Sheet1", 7, iStartRow + i, "'" + dgvPart.Rows[i].Cells["PASS_RATE"].Value);
                    ExcelClass.SetCellValue("Sheet1", 8, iStartRow + i, "'" + dgvPart.Rows[i].Cells["DEFECT_DATA"].Value.ToString().TrimEnd(','));
                    progressBar1.Increment(1);

                    double iPer = (iCount * 100) / (dgvPart.Rows.Count);
                    // Display the textual value of the ProgressBar in the StatusBar control's first panel.
                    //SajetCommon.Show_Message(iPer.ToString(), 0);
                    editStatus.Text = iPer.ToString() + "% Completed";
                    editStatus.Refresh();

                }

                ExcelClass.SetColumnWidth("Sheet1", 1, 1, 1, 1, 5);
                ExcelClass.SetColumnWidth("Sheet1", 2, 2, 2, 2, 16);
                ExcelClass.SetColumnWidth("Sheet1", 4, 4, 4, 4, 25);
                ExcelClass.SetColumnWidth("Sheet1", 8, 8, 8, 8, 40);
                ExcelClass.SetCellLineStyle("Sheet1", 1, 1, iVisibleCol + 1, dgvPart.Rows.Count + iStartRow - 1, 1);
                ExcelClass.AddSheet("Sheet2");
                iVisibleCol = 0;

                for (int i = 1; i <= dgvLotNo.Columns.Count; i++)
                {
                    if (dgvLotNo.Columns[i - 1].Visible)
                    {
                        iVisibleCol += 1;
                        ExcelClass.SetCellValue("Sheet2", i, 1, "'" + dgvLotNo.Columns[i - 1].HeaderText.ToString());
                        ExcelClass.SetCellColor("Sheet2", i, 1, i, 1, null, Color.Yellow);
                    }
                }
                iCount = 0;
                iStartRow = 2;

                for (int i = 0; i <= dgvLotNo.Rows.Count - 1; i++)
                {
                    iCount += 1;
                    for (int j = 0; j <= dgvLotNo.Columns.Count - 1; j++)
                    {
                        ExcelClass.SetCellValue("Sheet2", j + 1, iStartRow + i, "'" + dgvLotNo.Rows[i].Cells[j].Value);
                    }
                }

                ExcelClass.SetCellLineStyle("Sheet2", 1, 1, iVisibleCol, dgvLotNo.Rows.Count + iStartRow - 1, 1);
                ExcelClass.SaveAs(sFileName);
            }
            finally
            {
                progressBar1.Visible = false;
                editStatus.Visible = false;
                ExcelClass.Close();
                SajetCommon.Show_Message("Save Excel OK", -1);
            }
        }

        private void linkPDLWaiveNoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            if (dgvLotNo.Rows.Count == 0 || dgvLotNo.CurrentRow == null || dgvLotNo.SelectedCells.Count == 0)
            {
                return;
            }
            if (dgvLotNo.CurrentRow.Cells["QC_RESULT_1"].Value.ToString() != "2")
                return;
            string sWaiveNo = dgvLotNo.CurrentRow.Cells["WAIVE_NO"].Value.ToString();
            AdlinkWebService.ClassService adlink = new AdlinkWebService.ClassService();
            string sUrl = "", sMessage = "";
            bool bOK = adlink.ProcessWaiveNo(sWaiveNo, ref sUrl, ref sMessage);
            if (!bOK)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage(sMessage) + " : " + sWaiveNo, 0);
            }
             */ 

            /* ADLINK 成功
            com.adlinktech.w3.Service adlink = new com.adlinktech.w3.Service();
            DataSet dsTemp = adlink.GetAgileChangeInfo("DEV-0003581");
            if (dsTemp != null)
            {
                string sUrl = dsTemp.Tables["URL"].Rows[0]["RtnStatus"].ToString();
                string sResult = dsTemp.Tables["Result"].Rows[0]["RtnStatus"].ToString();
                System.Diagnostics.Process.Start(sUrl);
            }
            else
            {
                SajetCommon.Show_Message("Get Url Fail", 0);
            }
             */
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (dgvLotNo.Rows.Count == 0 || dgvLotNo.CurrentRow == null || dgvLotNo.SelectedCells.Count == 0)
            {
                return;
            }
            linkPDLWaiveNoToolStripMenuItem.Visible = false;
            if (dgvLotNo.CurrentRow.Cells["QC_RESULT_1"].Value.ToString() == "2")
            {
                linkPDLWaiveNoToolStripMenuItem.Visible = true;
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (dgvTypeMemo.Rows.Count == 0 || dgvTypeMemo.CurrentRow == null || dgvTypeMemo.SelectedCells.Count == 0)
            {
                return;
            }
            int iIndex = dgvTypeMemo.CurrentRow.Index;


            string sLotNo = dgvLotType.Rows[iIndex].Cells["LOT_NO1"].Value.ToString();
            string sTypeID = dgvLotType.Rows[iIndex].Cells["ITEM_TYPE_ID"].Value.ToString();
            string sTypeName = dgvLotType.Rows[iIndex].Cells["TYPE_NAME"].Value.ToString();
            string sTypeMemo = dgvLotType.Rows[iIndex].Cells["TESTTYPEMEMO"].Value.ToString();

            fTypeMemo fData = new fTypeMemo();

            try
            {
                fData.g_sLotNo = sLotNo;
                fData.g_sTypeID = sTypeID;
                fData.g_sTypeName = sTypeName;
                fData.editTypeMemo.Text = sTypeMemo;
                if (fData.ShowDialog() == DialogResult.OK)
                {
                    string sMemo = fData.editTypeMemo.Text.Trim();
                    object[][] Params = new object[3][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MEMO", sMemo };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sTypeID };
                    sSQL = "UPDATE SAJET.G_IQC_TEST_TYPE "
                         + "   SET MEMO = :MEMO "
                         + "  WHERE LOT_NO =:LOT_NO "
                         + "    AND ITEM_TYPE_ID =:ITEM_TYPE_ID ";
                    DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    GetTestItem(sLotNo);
                }
            }
            finally
            {
                fData.Dispose();
            }
        }

        private void btnRejectReport_Click(object sender, EventArgs e)
        {
            g_sFileName = "";
            string g_sSampleFile = System.Windows.Forms.Application.StartupPath + "\\" + g_sExeName + "\\RejectReport.xlt";
            if (dgvLotNo.Columns.Count == 0 || dgvLotNo.CurrentRow == null)
                return;
            string sSQLHasFileName;
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", dgvLotNo.CurrentRow.Cells["LOT_NO"].Value.ToString() };
            sSQLHasFileName = @"SELECT   A.MCI_REPORT_FILENAME,B.VENDOR_NAME FROM SAJET.G_IQC_LOT A,SAJET.SYS_VENDOR B 
                                WHERE    A.LOT_NO = :LOT_NO 
                                AND      A.VENDOR_ID = B.VENDOR_ID
                                ORDER BY MCI_REPORT_FILENAME";
            DataSet dsTempFileName = ClientUtils.ExecuteSQL(sSQLHasFileName, Params);
            if (dsTempFileName.Tables[0].Rows.Count > 0)
            {
                g_sFileName = dsTempFileName.Tables[0].Rows[0]["MCI_REPORT_FILENAME"].ToString() + "-"
                            + dsTempFileName.Tables[0].Rows[0]["VENDOR_NAME"].ToString();
                g_sLotNoHistory = dgvLotNo.CurrentRow.Cells["LOT_NO"].Value.ToString();
            }
            fMain.RepeatCreateMCIReport(g_sFileName, g_sSampleFile, g_sLotNoHistory);
            //fMain.CreateMCIReport(g_sFileName, g_sSampleFile);
        }

        private void chkbAutoSizeColumn_Click(object sender, EventArgs e)
        {
            if (chkbAutoSizeColumn.Checked)
            {
                this.dgvLotNo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                this.dgvItemNoValue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                this.dgvLotType.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                this.dgvPart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                this.dgvWIP.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                this.dgvRoHSItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                this.dgvRejectWaive.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                this.dgvTypeMemo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            else
            {
                this.dgvLotNo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.dgvItemNoValue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.dgvLotType.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.dgvPart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.dgvWIP.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.dgvRoHSItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.dgvRejectWaive.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                this.dgvTypeMemo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            }
        }
    }
}