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

namespace RCMerge
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        public String g_sUserID, g_sProgram, g_sFunction;
        string sSQL;
        DataSet dsTemp, dsRcData, dsMergeData;
        List<childRC> listChildRC = new List<childRC>();
        public List<string> lsRcToMerge = new List<string>();

        class childRC
        {
            public string sRcNo { get; set; }
            public string sQty { get; set; }
            public string sTravel_ID { get; set; }
            public childRC(string rcNo, string qty, string travel_ID)
            {
                sRcNo = rcNo;
                sQty = qty;
                sTravel_ID = travel_ID;
            }
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            TableDefine.Initial_Table();

            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            SajetCommon.SetLanguageControl(this);

            if (lsRcToMerge.Count > 0)
            {
                editRC.Text = lsRcToMerge.FirstOrDefault();
                editRC_KeyPress(null, new KeyPressEventArgs((char)Keys.Return));
                SetSelected(lsRcToMerge.ToArray());
                editRC.Enabled = false;
                btnSearchWo.Enabled = false;
                gvData.ReadOnly = true;
                btnSelectAll.Enabled = false;
                btnSelectNone.Enabled = false;
            }
        }

        private void btnSearchWo_Click(object sender, EventArgs e)
        {

            // 2016.11.24 By Jason
            string sSQL = "  SELECT A.WORK_ORDER,A.RC_NO,A.CURRENT_QTY"
                        + "    FROM SAJET.G_RC_STATUS A,SAJET.G_WO_BASE B"
                        + "   WHERE A.WORK_ORDER = B.WORK_ORDER"
                        + "     AND A.CURRENT_STATUS >= 0 AND A.CURRENT_STATUS < 2"
                        + "     AND A.RELEASE = 'Y' "
                        + "     AND B.WO_STATUS = '3'"
                        + "ORDER BY A.WORK_ORDER ASC,A.RC_NO ASC";
            // 2016.11.24 End

            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                editRC.Text = f.dgvData.CurrentRow.Cells["RC_NO"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editRC_KeyPress(sender, Key);
            }
        }

        public void editRC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 3)
                ClearData();

            if (e.KeyChar != (char)Keys.Return)
                return;

            sSQL = " SELECT * "
                 + "   FROM SAJET.G_RC_STATUS "
                 + "  WHERE RC_NO = '" + editRC.Text + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("RC number error", 0);
                editRC.Focus();
                editRC.SelectAll();
                return;
            }

            sSQL = " SELECT * "
                 + "   FROM SAJET.G_RC_STATUS "
                 + "  WHERE RC_NO = '" + editRC.Text + "' "
                 + "    AND RELEASE = 'Y'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("RC number not release", 0);
                editRC.Focus();
                editRC.SelectAll();
                return;
            }

            sSQL = " SELECT A.*,B.* "
                 + "   FROM SAJET.G_RC_STATUS A,SAJET.G_WO_BASE B"
                 + "  WHERE A.WORK_ORDER = B.WORK_ORDER "
                 + "    AND A.RC_NO = '" + editRC.Text + "' "
                 + "    AND A.RELEASE = 'Y'"
                 + "    AND B.WO_STATUS = '3'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Work Order Status must be WIP", 0);
                editRC.Focus();
                editRC.SelectAll();
                return;
            }

            ShowData();
        }

        private void ShowData()
        {
            string sSQL = "    SELECT A.*,"
                        + "           B.PART_ID,B.PART_NO,C.PROCESS_ID,C.PROCESS_NAME,B.SPEC1,B.SPEC2"
                        + "      FROM SAJET.G_RC_STATUS A"
                        + " LEFT JOIN SAJET.SYS_PART B ON A.PART_ID = B.PART_ID"
                        + " LEFT JOIN SAJET.SYS_PROCESS C ON A.PROCESS_ID = C.PROCESS_ID"
                        + " LEFT JOIN SAJET.G_WO_BASE D ON A.WORK_ORDER = D.WORK_ORDER"
                        + "     WHERE A.RC_NO = '" + editRC.Text + "'"
                        + "       AND A.CURRENT_STATUS >= 0 AND A.CURRENT_STATUS < 2"
                        + "       AND A.RELEASE = 'Y' "
                        + "       AND D.WO_STATUS = '3'";
            dsRcData = ClientUtils.ExecuteSQL(sSQL);

            // 2016.4.19 By Jason (防呆)
            if (dsRcData.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("RC Current Status Close", 0);
                editRC.Focus();
                editRC.SelectAll();
                return;
            }
            // 2016.4.19 End

            LabWO.Text = dsRcData.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            LabPart.Text = dsRcData.Tables[0].Rows[0]["SPEC1"].ToString();
            LabSpec2.Text = dsRcData.Tables[0].Rows[0]["SPEC2"].ToString();
            LabVersion.Text = dsRcData.Tables[0].Rows[0]["VERSION"].ToString();
            LabProcess.Text = dsRcData.Tables[0].Rows[0]["PROCESS_NAME"].ToString();
            LabQty.Text = dsRcData.Tables[0].Rows[0]["CURRENT_QTY"].ToString();

            sSQL = " SELECT A.RC_NO, A.CURRENT_QTY, B.SPEC1, A.VERSION,"
                 + " A.HAVE_SN, A.TRAVEL_ID, A.CURRENT_STATUS, A.INITIAL_QTY "
                 + " FROM SAJET.G_RC_STATUS A "
                 + " LEFT JOIN SAJET.SYS_PART B ON A.PART_ID = B.PART_ID "
                 + " LEFT JOIN SAJET.G_WO_BASE C ON A.WORK_ORDER = C.WORK_ORDER"
                 + " WHERE A.WORK_ORDER = '" + LabWO.Text + "' "
                 + " AND A.PART_ID = '" + dsRcData.Tables[0].Rows[0]["PART_ID"].ToString() + "' "
                 + " AND A.PROCESS_ID = '" + dsRcData.Tables[0].Rows[0]["PROCESS_ID"].ToString() + "' "
                 + " AND RC_NO <> '" + editRC.Text + "'"
                 + " AND CURRENT_STATUS >= 0 AND CURRENT_STATUS < 2 "
                 + " AND A.RELEASE = 'Y' "
                 + " AND C.WO_STATUS = '3' ";

            if (lsRcToMerge.Count > 0)
                sSQL += $" ORDER BY  CASE WHEN RC_NO IN ('{string.Join("','", lsRcToMerge)}') THEN 0 ELSE 1 END, RC_NO   ";
            else
                sSQL += " ORDER BY " + TableDefine.gsDef_OrderField;

            dsMergeData = ClientUtils.ExecuteSQL(sSQL);

            gvData.DataSource = dsMergeData;
            gvData.DataMember = dsMergeData.Tables[0].ToString();

            for (int i = 0; i <= gvData.Columns.Count - 1; i++)
            {
                gvData.Columns[i].Visible = false;
            }

            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridField[i].sFieldName;

                if (gvData.Columns.Contains(sGridField))
                {
                    gvData.Columns[sGridField].HeaderText = TableDefine.tGridField[i].sCaption;
                    gvData.Columns[sGridField].ReadOnly = true;
                    gvData.Columns[sGridField].Visible = true;
                }
            }

            if (gvData.RowCount > 0)
            {
                DataGridViewCheckBoxColumn cbCol = new DataGridViewCheckBoxColumn();
                cbCol.Width = 45;
                cbCol.Name = "SELECT";
                cbCol.HeaderText = "";
                cbCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                gvData.Columns.Insert(0, cbCol);

                btnSelectAll.Enabled = true;
                btnSelectNone.Enabled = true;
                btnMerge.Enabled = true;
                gvData.Focus();
            }
        }

        /// <summary>
        /// 將傳入的RC 設為選取
        /// </summary>
        /// <param name="saSelectedRc"></param>
        public void SetSelected(string[] saSelectedRc)
        {
            foreach (string sRC in saSelectedRc)
                foreach (DataGridViewRow r in gvData.Rows)
                    if (sRC == (Convert.ToString(r.Cells["RC_NO"].Value)))
                    {
                        (r.Cells["SELECT"] as DataGridViewCheckBoxCell).Value = true;
                        break;
                    }
        }
        private void ClearData()
        {
            for (int i = 0; i <= panel1.Controls.Count - 1; i++)
            {
                if (panel1.Controls[i] is Label && panel1.Controls[i].Name.Contains("Lab"))
                {
                    panel1.Controls[i].Text = "";
                }
            }

            btnSelectAll.Enabled = false;
            btnSelectNone.Enabled = false;
            btnMerge.Enabled = false;
            gvData.Columns.Clear();
            gvData.DataSource = null;
            gvDetail.DataSource = null;
            gvDetail.Columns.Clear();
        }

        private void gvData_SelectionChanged(object sender, EventArgs e)
        {
            if (gvData.RowCount == 0 || gvData.CurrentRow == null)
                return;

            ShowDetail();
        }

        private void ShowDetail()
        {
            sSQL = " Select SERIAL_NUMBER "
                 + " From SAJET.G_SN_STATUS where "
                 + " RC_NO = '" + gvData.CurrentRow.Cells["RC_NO"].Value.ToString() + "' "
                 + " and CURRENT_STATUS >= 0 and CURRENT_STATUS < 2 "
                 + " Order by SERIAL_NUMBER";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            gvDetail.DataSource = dsTemp;
            gvDetail.DataMember = dsTemp.Tables[0].ToString();

            foreach (DataGridViewColumn gvColumn in gvDetail.Columns)
            {
                gvColumn.HeaderText = SajetCommon.SetLanguage("Serial Number", 1);
            }

            if (gvDetail.RowCount < 1)
                gvDetail.Columns.Clear();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= gvData.Rows.Count - 1; i++)
            {
                gvData.Rows[i].Cells[0].Value = true;
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= gvData.Rows.Count - 1; i++)
            {
                gvData.Rows[i].Cells[0].Value = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            if (!Check_Keyin()) return;

            try
            {
                SaveData();
                string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !";
                SajetCommon.Show_Message(sMsg, 1);
                if (lsRcToMerge.Count > 0)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ClearData();
                    editRC.Text = "";
                    editRC.Focus();
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);

                return;
            }
        }

        private bool Check_Keyin()
        {
            string sMsg;
            bool isNothingSelected = true;

            if (gvData.RowCount == 0)
            {
                sMsg = SajetCommon.SetLanguage("No Data", 1);
                SajetCommon.Show_Message(sMsg, 0);
                return false;
            }

            foreach (DataGridViewRow gvRow in gvData.Rows)
            {
                if (gvRow.Cells["SELECT"].Value == null) gvRow.Cells["SELECT"].Value = false;
                if ((bool)gvRow.Cells["SELECT"].Value)
                {
                    isNothingSelected = false;
                    break;
                }
            }

            if (isNothingSelected)
            {
                sMsg = SajetCommon.SetLanguage("Please select RC", 1);
                SajetCommon.Show_Message(sMsg, 0);
                return false;
            }

            return true;
        }

        private void SaveData()
        {
            DateTime datExeTime = DateTime.Now;

            // 2016.5.25 By Jason
            long lTravel_Id = Convert.ToInt64(datExeTime.ToString("yyyyMMddHHmmssf"));
            // 2016.5.25 End

            // 2016.7.5 By Jason
            int iWorkTime = Convert.ToInt32(dsRcData.Tables[0].Rows[0]["WORKTIME"].ToString());
            // 2016.7.5 End

            listChildRC.Clear();
            decimal iSourceQty = 0, iTotalQty = 0, iTotalInitialQty = 0;
            int iRcTravelID = 0;
            decimal.TryParse(LabQty.Text, out iSourceQty);
            Int32.TryParse(dsRcData.Tables[0].Rows[0]["TRAVEL_ID"].ToString(), out iRcTravelID);
            listChildRC.Add(new childRC(editRC.Text, iSourceQty.ToString(), iRcTravelID.ToString()));
            iRcTravelID++;

            sSQL = " SELECT SERIAL_NUMBER,TRAVEL_ID FROM SAJET.G_SN_STATUS "
                 + " WHERE RC_NO = '" + editRC.Text + "' "
                 + " AND CURRENT_STATUS >= 0 AND CURRENT_STATUS < 2 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    object[][] snParams = new object[5][];
                    string sSerial = dsTemp.Tables[0].Rows[i]["SERIAL_NUMBER"].ToString();
                    sSQL = " INSERT INTO SAJET.G_RC_MERGESN VALUES ( "
                         + " :RC_NO, :SERIAL_NUMBER, :TRAVEL_ID, :UPDATE_USERID, :UPDATE_TIME)";
                    snParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRC.Text };
                    snParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerial };
                    snParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", iRcTravelID };
                    snParams[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                    snParams[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    ClientUtils.ExecuteSQL(sSQL, snParams);
                }
            }

            foreach (DataGridViewRow gvRow in gvData.Rows)
            {
                if (gvRow.Cells["SELECT"].Value == null) gvRow.Cells["SELECT"].Value = false;

                if ((bool)gvRow.Cells["SELECT"].Value)
                {
                    decimal iQty = 0;
                    decimal.TryParse(gvRow.Cells["CURRENT_QTY"].Value.ToString(), out iQty);
                    iTotalQty += iQty;

                    decimal iInitialQty = 0;
                    decimal.TryParse(gvRow.Cells["INITIAL_QTY"].Value.ToString(), out iInitialQty);
                    iTotalInitialQty += iInitialQty;

                    string sRCNo = gvRow.Cells["RC_NO"].Value.ToString();
                    string sTravelID = gvRow.Cells["TRAVEL_ID"].Value.ToString();

                    //UPDATE G_SN_STATUS
                    if (gvRow.Cells["HAVE_SN"].Value.ToString() == "1")
                    {
                        sSQL = " SELECT SERIAL_NUMBER,TRAVEL_ID FROM SAJET.G_SN_STATUS "
                             + " WHERE RC_NO = '" + sRCNo + "' "
                             + " AND CURRENT_STATUS >= 0 AND CURRENT_STATUS < 2 ";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                object[][] snParams = new object[1][];
                                string sSerial = dsTemp.Tables[0].Rows[i]["SERIAL_NUMBER"].ToString();
                                sSQL = " UPDATE SAJET.G_SN_STATUS SET RC_NO = '" + editRC.Text + "', "
                                    //+ " TRAVEL_ID = " + iRcTravelID + ", "
                                     + " UPDATE_USERID = '" + g_sUserID + "', UPDATE_TIME = :UPDATE_TIME "
                                     + " WHERE SERIAL_NUMBER = '" + sSerial + "'";
                                snParams[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                                ClientUtils.ExecuteSQL(sSQL, snParams);

                                snParams = new object[5][];
                                sSQL = " INSERT INTO SAJET.G_RC_MERGESN VALUES ( "
                                     + " :RC_NO, :SERIAL_NUMBER, :TRAVEL_ID, :UPDATE_USERID, :UPDATE_TIME)";
                                snParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRC.Text };
                                snParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerial };
                                snParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", iRcTravelID };
                                snParams[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                                snParams[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                                ClientUtils.ExecuteSQL(sSQL, snParams);

                            }
                        }

                        if (dsRcData.Tables[0].Rows[0]["CURRENT_STATUS"].ToString() == "1")
                            UpdateTravelParam(iRcTravelID.ToString(), sRCNo);
                    }

                    listChildRC.Add(new childRC(sRCNo, iQty.ToString(), sTravelID));

                    //2015.08.11 polly 
                    //更新被合并的RC的CURRENT_STATUS为7
                    //sSQL = " DELETE SAJET.G_RC_STATUS WHERE RC_NO = '" + sRCNo + "' AND TRAVEL_ID = '" + sTravelID + "'";
                    //ClientUtils.ExecuteSQL(sSQL);

                    object[][] rcParams = new object[2][];
                    // 2016.5.25 By Jason
                    //sSQL = " update SAJET.G_RC_STATUS set CURRENT_STATUS=7, CURRENT_QTY=0, TRAVEL_ID = " + (Convert.ToInt32(sTravelID) + 1) + ", UPDATE_USERID = :UPDATE_USERID, UPDATE_TIME = :UPDATE_TIME WHERE RC_NO = '" + sRCNo + "' AND TRAVEL_ID = '" + sTravelID + "'";
                    sSQL = " update SAJET.G_RC_STATUS set CURRENT_STATUS=7, CURRENT_QTY=0, BONUS_QTY=0, WORKTIME=0, INITIAL_QTY=0, TRAVEL_ID = " + lTravel_Id + ", UPDATE_USERID = :UPDATE_USERID, UPDATE_TIME = :UPDATE_TIME, RELEASE = 'Y' WHERE RC_NO = '" + sRCNo + "' AND TRAVEL_ID = '" + sTravelID + "'";
                    // 2016.5.25 End
                    rcParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                    rcParams[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    ClientUtils.ExecuteSQL(sSQL, rcParams);

                    if (gvRow.Cells["CURRENT_STATUS"].Value.ToString() == "1")
                        DeleteExtraTables(gvRow, sTravelID);
                }
            }

            iTotalQty += iSourceQty;

            //UPDATE G_RC_MERGE      
            for (int i = 0; i < listChildRC.Count; i++)
            {
                string sProcessID = dsRcData.Tables[0].Rows[0]["PROCESS_ID"].ToString();
                object[][] Params = new object[9][];

                sSQL = " INSERT INTO SAJET.G_RC_MERGE VALUES ( "
                     + " :RC_NO, :RC_QTY, :SOURCE_RC_TRAVEL_ID, :MERGE_RC_NO, :MERGE_RC_QTY, :TRAVEL_ID, :PROCESS_ID, "
                     + " :UPDATE_USERID, :UPDATE_TIME)";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", listChildRC[i].sRcNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_QTY", listChildRC[i].sQty };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SOURCE_RC_TRAVEL_ID", listChildRC[i].sTravel_ID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MERGE_RC_NO", editRC.Text };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MERGE_RC_QTY", iTotalQty.ToString() };
                // 2016.5.25 By Jason
                //Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", Convert.ToInt32(listChildRC[i].sTravel_ID) + 1 };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", lTravel_Id };
                // 2016.5.25 End
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                Params[8] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                ClientUtils.ExecuteSQL(sSQL, Params);
            }


            //UPDATE G_SN_STATUS PARENT TRAVEL_ID
            //sSQL = " UPDATE SAJET.G_SN_STATUS SET TRAVEL_ID = " + iRcTravelID + ", "
            //     + " UPDATE_USERID = '" + g_sUserID + "', UPDATE_TIME = :UPDATE_TIME "
            //     + " WHERE RC_NO = '" + editRC.Text + "' AND TRAVEL_ID = '" + (iRcTravelID - 1).ToString() + "'";
            //ClientUtils.ExecuteSQL(sSQL, Params1);

            //UPDATE G_RC_STATUS
            object[][] Params1 = new object[1][];
            sSQL = " UPDATE SAJET.G_RC_STATUS SET CURRENT_QTY = " + iTotalQty + ", "
                // 2016.7.5 By Jason
                 + " BONUS_QTY = 0 , INITIAL_QTY = INITIAL_QTY + " + iTotalInitialQty + ", "
                // 2016.7.5 End
                // 2016.5.25 By Jason
                //+ " TRAVEL_ID = " + iRcTravelID + ", "
                 + " TRAVEL_ID = " + lTravel_Id + ", "
                // 2016.5.25 End
                 + " RELEASE = 'Y', "
                 + " UPDATE_USERID = '" + g_sUserID + "', UPDATE_TIME = :UPDATE_TIME "
                 + " WHERE RC_NO = '" + editRC.Text + "'";
            Params1[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
            ClientUtils.ExecuteSQL(sSQL, Params1);

            if (dsRcData.Tables[0].Rows[0]["CURRENT_STATUS"].ToString() == "1")
                UpdateExtraTables(iRcTravelID.ToString());
        }

        private void DeleteExtraTables(DataGridViewRow gvRow, string sTravelID)
        {
            sSQL = " Delete SAJET.G_RC_TRAVEL_MACHINE "
                 + " Where RC_NO = '" + gvRow.Cells["RC_NO"].Value.ToString() + "' "
                 + " And TRAVEL_ID = '" + sTravelID + "'";
            ClientUtils.ExecuteSQL(sSQL);

            sSQL = " Delete SAJET.G_RC_TRAVEL_TOOLING "
                 + " Where RC_NO = '" + gvRow.Cells["RC_NO"].Value.ToString() + "' "
                 + " And TRAVEL_ID = '" + sTravelID + "'";
            ClientUtils.ExecuteSQL(sSQL);
        }

        private void UpdateExtraTables(string sRcTravelID)
        {
            sSQL = " UPDATE SAJET.G_RC_TRAVEL_MACHINE SET TRAVEL_ID = '" + sRcTravelID + "', "
                 + " UPDATE_USERID = '" + g_sUserID + "', UPDATE_TIME = SYSDATE "
                 + " WHERE RC_NO = '" + editRC.Text + "'";
            ClientUtils.ExecuteSQL(sSQL);

            if (dsRcData.Tables[0].Rows[0]["HAVE_SN"].ToString() == "1")
            {
                sSQL = " UPDATE SAJET.G_RC_TRAVEL_PARAM SET TRAVEL_ID = '" + sRcTravelID + "', "
                     + " UPDATE_USERID = '" + g_sUserID + "', UPDATE_TIME = SYSDATE "
                     + " WHERE RC_NO = '" + editRC.Text + "'";
                ClientUtils.ExecuteSQL(sSQL);
            }

            sSQL = " UPDATE SAJET.G_RC_TRAVEL_TOOLING SET TRAVEL_ID = '" + sRcTravelID + "', "
                 + " UPDATE_USERID = '" + g_sUserID + "', UPDATE_TIME = SYSDATE "
                 + " WHERE RC_NO = '" + editRC.Text + "'";
            ClientUtils.ExecuteSQL(sSQL);
        }

        private void UpdateTravelParam(string sRcTravelID, string sRCNo)
        {
            sSQL = " SELECT SERIAL_NUMBER FROM SAJET.G_RC_TRAVEL_PARAM "
                             + " WHERE RC_NO = '" + sRCNo + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    string sSerial = dsTemp.Tables[0].Rows[i]["SERIAL_NUMBER"].ToString();
                    sSQL = " UPDATE SAJET.G_RC_TRAVEL_PARAM SET RC_NO = '" + editRC.Text + "', "
                         + " TRAVEL_ID = " + sRcTravelID + ", "
                         + " UPDATE_USERID = '" + g_sUserID + "', UPDATE_TIME = SYSDATE "
                         + " WHERE SERIAL_NUMBER = '" + sSerial + "'";
                    ClientUtils.ExecuteSQL(sSQL);
                }
            }
        }
    }
}
