using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Globalization;
using Microsoft.Win32;
using System.Data.OracleClient;
using SajetFilter;



namespace RTMaintain
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }
        public static string g_sUserID, g_sUserNo;
        public static string g_sProgram, g_sExeName;
        public int g_iPrivilege = 0;
        string [] fieldList = new string [3] {"RT_NO", "VENDOR_CODE", "PO_NO" };

       // public ChromaThreadLog m_log = new ChromaThreadLog();   //20160819
        public bool g_bDebug = false;
        
        private void CopyToHistory(string sDNID)
        {
            string sSQL = "";
            sSQL = "Insert Into SAJET.G_HT_DN_BASE "
                + " Select * from SAJET.G_DN_BASE "
                + " Where DN_ID ='" + sDNID + "' ";
            DataSet dsTemp = new DataSet();
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
        }

        public void check_privilege()
        {           
            g_iPrivilege = SajetCommon.Get_Privilege(g_sUserID, ClientUtils.fCurrentProject, out g_sProgram);
            btnAppend.Enabled = (g_iPrivilege >= 1);
            btnAppendItem.Enabled = btnAppend.Enabled;
            btnModify.Enabled = (g_iPrivilege >= 1);
            btnModifyItem.Enabled = btnModify.Enabled;
            btnDelete.Enabled = (g_iPrivilege >= 2);
            btnDeleteItem.Enabled = btnDelete.Enabled;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            //g_sExeName = ClientUtils.fCurrentProject;

            g_sUserID = ClientUtils.UserPara1;
            g_sUserNo = ClientUtils.fLoginUser;
            g_sExeName = ClientUtils.fCurrentProject;
            check_privilege();

            combFieldName.Items.Clear();
            foreach (string field in fieldList)
                combFieldName.Items.Add(field);
            combFieldName.SelectedIndex = 0;
            
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            ClientUtils.SetLanguage(this, g_sExeName);

            dgViewRT.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgViewDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";

            //Start V1.16004.0.6 
           // m_log.LogHeader = ClientUtils.fFunctionName;
           // m_log.LogDirectoryName = g_sExeName + "\\Log";
           // m_log.Started = true;

            string mErrMsg = "";
            string mStartDefaultDays = SajetCommon.GetSysBaseData(g_sExeName, "RT Maintain Before Days", ref mErrMsg);
            if (mStartDefaultDays.Length == 0)
            {
                mStartDefaultDays = "30";
            }
            //dtDateFrom.Value = DateTime.Today.AddMonths(-3);
            dtDateFrom.Value = DateTime.Today.AddDays(-int.Parse(mStartDefaultDays));
            //End
            dtDateTo.Value = DateTime.Today;
            GetData();
        }
        private void GetData()
        {
           // addLog(LogType.Debug, "GetData", "1");
            dgViewRT.Rows.Clear();
            dgViewDetail.Rows.Clear();
            lablCount.Text = "0";
            editRTNo.Text = editRTNo.Text.Trim();
            editVendor.Text = editVendor.Text.Trim();
            string sRTNo = editRTNo.Text;
            string sVendor = editVendor.Text; 
            //string sSQL = " SELECT A.RT_ID, A.RT_NO, B.VENDOR_CODE, B.VENDOR_NAME, A.PO_NO, A.RECEIVE_TIME "  //PO_NO move to detail 2016/08/19
            string sSQL = " SELECT A.RT_ID, A.RT_NO, B.VENDOR_CODE, B.VENDOR_NAME, A.RECEIVE_TIME "
                + "  FROM SAJET.G_ERP_RTNO A, SAJET.SYS_VENDOR B "
                + "  WHERE A.ENABLED = 'Y' "
                + "    AND A.VENDOR_ID = B.VENDOR_ID(+) ";
            if (chkbDate.Checked)
            {
                sSQL = sSQL
                     + " AND A.RECEIVE_TIME >= TO_DATE ('" + dtDateFrom.Value.ToString("yyy/MM/dd") + "', 'yyyy/mm/dd') "
                     + " AND A.RECEIVE_TIME <= TO_DATE ('" + dtDateTo.Value.ToString("yyy/MM/dd") + " 23:59:59', 'yyyy/mm/dd hh24:mi:ss') ";
            }
            if (!string.IsNullOrEmpty(sRTNo))
            {
                sRTNo = sRTNo + "%";
                sSQL = sSQL + "AND A.RT_NO LIKE '" + sRTNo + "' ";
            }
            if (!string.IsNullOrEmpty(sVendor))
            {
                sVendor = sVendor + "%";
                sSQL = sSQL + "AND B.VENDOR_CODE LIKE '" + sVendor + "' ";
            }
            sSQL = sSQL + "  ORDER BY A.RT_NO ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            bsRT.DataSource = dsTemp;
            bsRT.DataMember = dsTemp.Tables[0].ToString();
          

            //addLog(LogType.Debug, "GetData", "1.Count:" + dsTemp.Tables[0].Rows.Count.ToString());

            string[] rowData;
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                //V1.16004.0.8 for performance start
                //addLog(LogType.Debug, "GetData", "1.1:" + i.ToString());
                //dgViewRT.Rows.Add();
                //dgViewRT.Rows[dgViewRT.Rows.Count - 1].Cells["RT_NO"].Value = dsTemp.Tables[0].Rows[i]["RT_NO"].ToString();
                //dgViewRT.Rows[dgViewRT.Rows.Count - 1].Cells["VENDOR_CODE"].Value = dsTemp.Tables[0].Rows[i]["VENDOR_CODE"].ToString();
                ////dgViewRT.Rows[dgViewRT.Rows.Count - 1].Cells["PO_NO"].Value = dsTemp.Tables[0].Rows[i]["PO_NO"].ToString();
                //dgViewRT.Rows[dgViewRT.Rows.Count - 1].Cells["RECEIVE_TIME"].Value = dsTemp.Tables[0].Rows[i]["RECEIVE_TIME"].ToString();
                //dgViewRT.Rows[dgViewRT.Rows.Count - 1].Cells["RT_ID"].Value = dsTemp.Tables[0].Rows[i]["RT_ID"].ToString();
                //dgViewRT.Rows[dgViewRT.Rows.Count - 1].Cells["VENDOR_NAME"].Value = dsTemp.Tables[0].Rows[i]["VENDOR_NAME"].ToString();
                //addLog(LogType.Debug, "GetData", "1.2:" + i.ToString());

                rowData = new string[] { dsTemp.Tables[0].Rows[i]["RT_NO"].ToString(),
                                         dsTemp.Tables[0].Rows[i]["VENDOR_CODE"].ToString(),
                                         dsTemp.Tables[0].Rows[i]["VENDOR_NAME"].ToString(),
                                         dsTemp.Tables[0].Rows[i]["RECEIVE_TIME"].ToString(),
                                         dsTemp.Tables[0].Rows[i]["RT_ID"].ToString()
                };
                dgViewRT.Rows.Add(rowData);
                //End
            }

            lablCount.Text = dgViewRT.Rows.Count.ToString();
          //  addLog(LogType.Debug, "GetData", "2");
            if (dgViewRT.Rows.Count > 0)
            {
                GetDetail(dgViewRT.Rows[0].Cells["RT_ID"].Value.ToString());
                //GetDetail(dgViewRT.CurrentRow.Cells["RT_ID"].Value.ToString());
//                dgViewRT.Focus();
//                dgViewRT.Rows[0].Selected = true;
              //  SetSelectRow(dgViewRT, "", "RT_NO");
            }
           // addLog(LogType.Debug, "GetData", "3");

        }
        private void SetSelectRow(DataGridView GridData, String sPrimaryKey, String sField)
        {
            //addLog(LogType.Debug, "SetSelectRow", "1");
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

                //GridData.CurrentRow = GridData.Rows[iIndex];
                GridData.CurrentCell = GridData.Rows[iIndex].Cells[sShowField];
                GridData.Rows[iIndex].Selected = true;
            }
        }
        private void GetDetail(string sRTID)
        {
            //addLog(LogType.Debug, "GetDetail", "1" + sRTID);
            string sSQL = "Select A.ROWID, A.RT_ID, A.PART_ID, B.PART_NO, A.PART_VERSION, A.INCOMING_QTY, LOT_NO, "
                + "       A.VENDOR_LOTNO, A.VENDOR_PARTNO, A.INCOMING_TIME, DATECODE,A.RT_SEQ ,C.WAREHOUSE_NAME  "
                + "      ,DECODE(A.URGENT_TYPE,'Y','Yes','No') URGENT_TYPE, DECODE(A.RD_FLAG,'Y','Yes','No') RD_FLAG, A.PO_NO " //Show PO_NO 2016/08/19
                + "From SAJET.G_ERP_RT_ITEM A, "
                + "     SAJET.SYS_PART B, "
                + "     SAJET.SYS_WAREHOUSE C " 
                + "Where A.ENABLED = 'Y' "
                + "  AND A.RT_ID = '" + sRTID + "' "
                + "  and A.PART_ID = B.PART_ID "
                + "  AND A.LOCATION  = C.WAREHOUSE_ID(+) "
                + "Order By A.RT_SEQ, PART_NO ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            bsItem.DataSource = dsTemp;
            bsItem.DataMember = dsTemp.Tables[0].ToString();
            dgViewDetail.Rows.Clear();
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                dgViewDetail.Rows.Add();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["PART_NO"].Value = dsTemp.Tables[0].Rows[i]["PART_NO"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["PART_VERSION"].Value = dsTemp.Tables[0].Rows[i]["PART_VERSION"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["DATECODE"].Value = dsTemp.Tables[0].Rows[i]["DATECODE"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["INCOMING_QTY"].Value = dsTemp.Tables[0].Rows[i]["INCOMING_QTY"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["LOT_NO"].Value = dsTemp.Tables[0].Rows[i]["LOT_NO"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["INCOMING_TIME"].Value = dsTemp.Tables[0].Rows[i]["INCOMING_TIME"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["VENDOR_LOTNO"].Value = dsTemp.Tables[0].Rows[i]["VENDOR_LOTNO"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["VENDOR_PARTNO"].Value = dsTemp.Tables[0].Rows[i]["VENDOR_PARTNO"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["ROWID"].Value = dsTemp.Tables[0].Rows[i]["ROWID"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["RT_SEQ"].Value = dsTemp.Tables[0].Rows[i]["RT_SEQ"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["PO_NO"].Value = dsTemp.Tables[0].Rows[i]["PO_NO"].ToString(); //2016/08/19
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["WAREHOUSE_NAME"].Value = dsTemp.Tables[0].Rows[i]["WAREHOUSE_NAME"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["URGENT_TYPE"].Value = dsTemp.Tables[0].Rows[i]["URGENT_TYPE"].ToString();
                dgViewDetail.Rows[dgViewDetail.Rows.Count - 1].Cells["RD_FLAG"].Value = dsTemp.Tables[0].Rows[i]["RD_FLAG"].ToString();
            }
            //addLog(LogType.Debug, "GetDetail", "2");
            if (dgViewDetail.Rows.Count > 0)
            {
              // SetSelectRow(dgViewDetail, "", "RT_SEQ");
            }
         
        }
        private void btnAppend_Click(object sender, EventArgs e)
        {
            fData formData = new fData();
            try
            {
                formData.G_sEmpID = g_sUserID;
                if ((sender as Button).Name == "btnAppend")
                {
                    formData.initial("0", "0");
                }
                if ((sender as Button).Name == "btnModify")
                {
                    if (dgViewRT.CurrentRow == null)
                    {
                        return;
                    }

                    formData.initial("1", dgViewRT.CurrentRow.Cells["RT_ID"].Value.ToString());
                }
                DialogResult result = formData.ShowDialog();
                if ((formData.G_bAppendOK) || (result == DialogResult.OK))
                {
                    GetData();
                    int iIndex = bsRT.Find("RT_ID", formData.G_sRTID);
                    if (iIndex >= 0)
                    {
                        dgViewRT.CurrentCell = dgViewRT.Rows[iIndex].Cells[0];
                        dgViewRT.Rows[iIndex].Selected = true;
                        GetDetail(formData.G_sRTID);
                    }

                }
                //SetSelectRow(dgViewRT, formData.G_sRTID, 0);
            }
            finally
            {
                formData.Dispose();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgViewRT.CurrentRow == null)
            {
                return;
            }
            
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Do you want to delete this data ?") + Environment.NewLine
                                       + SajetCommon.SetLanguage("RT No")+" : " + dgViewRT.CurrentRow.Cells[0].Value.ToString(), 2) != DialogResult.Yes)
                return;

            string sRTID = dgViewRT.CurrentRow.Cells["RT_ID"].Value.ToString();
            int iIndex = 0;
            try
            {
                iIndex = bsRT.Find("RT_ID", sRTID);
            }
            catch
            {
            }
            string sSQL = " Select * from SAJET.G_ERP_RT_ITEM "
                           + "  WHERE RT_ID = '" + sRTID + "' ";
            DataSet dsRTItem = ClientUtils.ExecuteSQL(sSQL);
            if (dsRTItem.Tables[0].Rows.Count > 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("RT Detail Data Exist,Can't Delete RT Header"), 0);
                return;
            }                                    

            DataSet dsTemp = new DataSet();
            sSQL = "DELETE SAJET.G_ERP_RT_ITEM "
                + " WHERE RT_ID ='" + sRTID + "' ";
            ClientUtils.ExecuteSQL(sSQL);
            sSQL = "DELETE SAJET.G_ERP_RTNO "
                 + " WHERE RT_ID = '" + sRTID + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            GetData();
            if (dgViewRT.Rows.Count > 0)
            {
                if (iIndex >= dgViewRT.Rows.Count)
                    iIndex = dgViewRT.Rows.Count - 1;
                dgViewRT.CurrentCell = dgViewRT.Rows[iIndex].Cells[0];
                dgViewRT.Rows[iIndex].Selected = true;
            }
        }        

        private void dgViewRT_SelectionChanged(object sender, EventArgs e)
        {
            //addLog(LogType.Debug, "dgViewRT_SelectionChanged", "1");
           if (dgViewRT.Rows.Count == 0)
               return;

           if (dgViewRT.CurrentRow == null || dgViewRT.CurrentRow.Cells[0].Value == null)
               return;

          // addLog(LogType.Debug, "dgViewRT_SelectionChanged", "2");
           GetDetail(dgViewRT.CurrentRow.Cells["RT_ID"].Value.ToString());
           //addLog(LogType.Debug, "dgViewRT_SelectionChanged", "3");
       }

        private void btnAppendItem_Click(object sender, EventArgs e)
        {
            if (dgViewRT.Rows.Count == 0)
                return;
            if (dgViewRT.CurrentRow == null)
                return;

            fDataItem formData = new fDataItem();
            try
            {
                string sRTID = dgViewRT.CurrentRow.Cells["RT_ID"].Value.ToString();
                string sRTNo = dgViewRT.CurrentRow.Cells["RT_NO"].Value.ToString();
                string sVendor = dgViewRT.CurrentRow.Cells["VENDOR_CODE"].Value.ToString();
            
                if ((sender as Button).Name == "btnAppendItem")
                {
                    formData.initial("0", sRTID, sRTNo, sVendor, "");
                }
                if ((sender as Button).Name == "btnModifyItem")
                {
                    if (dgViewDetail.CurrentRow == null)
                    {
                        return;
                    }                    
                    string sRowID = dgViewDetail.CurrentRow.Cells["ROWID"].Value.ToString();
                    formData.initial("1", sRTID, sRTNo, sVendor, sRowID);
                }               
                DialogResult result = formData.ShowDialog();

                if ((formData.G_bAppendOK) || (result == DialogResult.OK))
                {
                    GetDetail(dgViewRT.CurrentRow.Cells["RT_ID"].Value.ToString());
                    try
                    {
                        int iIndex = bsItem.Find("ROWID", formData.G_sRowID);
                        dgViewDetail.CurrentCell = dgViewDetail.Rows[iIndex].Cells[0];
                        dgViewDetail.Rows[iIndex].Selected = true;
                    }
                    catch
                    {
                    }
                }
            }
            finally
            {
                formData.Dispose();
            }
        }
        private bool CheckIQCDataExist(string sRTID, string sRTSeq)
        {

            string sSQL = " Select LOT_NO from SAJET.G_IQC_LOT "
                 + " Where RT_ID = '" + sRTID + "' "
                 + " and RT_SEQ = '" + sRTSeq + "' "
                 + " and rownum = 1";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            string sLot = dsTemp.Tables[0].Rows[0]["LOT_NO"].ToString();

            sSQL = " Select * from sajet.G_IQC_TEST_TYPE "
                 + " Where LOT_NO = '" + sLot + "' "
                 + " and rownum = 1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = " Select * from sajet.G_IQC_TEST_TYPE_DEFECT "
                     + " Where LOT_NO = '" + sLot + "' "
                     + " and rownum = 1";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            if (dsTemp.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }
        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (dgViewDetail.Rows.Count == 0)
                return;
            string sRTID = dgViewRT.CurrentRow.Cells["RT_ID"].Value.ToString();
            string sRTSeq = dgViewDetail.CurrentRow.Cells["RT_SEQ"].Value.ToString();
           // string sRowID = dgViewDetail.CurrentRow.Cells["ROWID"].Value.ToString();
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Do you want to delete this data ?") + Environment.NewLine
                                       +SajetCommon.SetLanguage( "RT No")+" : " + dgViewRT.CurrentRow.Cells["RT_NO"].Value.ToString()+ Environment.NewLine
                                       + SajetCommon.SetLanguage("Part No")+" : " + dgViewDetail.CurrentRow.Cells["PART_NO"].Value.ToString(), 2) != DialogResult.Yes)
                return;
            //若已有IQC檢驗資料不可刪除
            if (CheckIQCDataExist(sRTID, sRTSeq))
            {
               if (SajetCommon.Show_Message(SajetCommon.SetLanguage("IQC Test Data Exist") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Do you want to delete this data ?") + Environment.NewLine
                                        + SajetCommon.SetLanguage("RT No") + " : " + dgViewRT.CurrentRow.Cells["RT_NO"].Value.ToString() + Environment.NewLine
                                        + SajetCommon.SetLanguage("RT Seq") + " : " + sRTSeq, 2) !=DialogResult.Yes)
                return;
            }
            string sSQL="UPDATE SAJET.G_ERP_RT_ITEM "
                       +"   SET ENABLED='Drop' "
                       +"      ,UPDATE_TIME = SYSDATE "
                       +"      ,UPDATE_USERID ='"+g_sUserID+"' "
                       +" WHERE RT_ID ='" + sRTID + "' "
                       +"   AND RT_SEQ ='" + sRTSeq + "' ";
            ClientUtils.ExecuteSQL(sSQL);

            sSQL =" INSERT INTO SAJET.G_HT_ERP_RT_ITEM "
                 +" SELECT * FROM SAJET.G_ERP_RT_ITEM " 
                       + " WHERE RT_ID ='" + sRTID + "' "
                       + "   AND RT_SEQ ='" + sRTSeq + "' ";
            ClientUtils.ExecuteSQL(sSQL);

            sSQL = "DELETE SAJET.G_ERP_RT_ITEM "
                + " WHERE RT_ID ='" + sRTID + "' "
                + "   AND RT_SEQ ='" + sRTSeq + "' ";
            ClientUtils.ExecuteSQL(sSQL);
            GetDetail(dgViewRT.CurrentRow.Cells["RT_ID"].Value.ToString());
        }

        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            if (dgViewRT.Rows.Count == 0)
                return;
            editValue.Text = editValue.Text;
            for (int i = 0; i <= dgViewRT.RowCount - 1; i++)
            {
                string sData = dgViewRT.Rows[i].Cells[fieldList[combFieldName.SelectedIndex]].Value.ToString();
                if (sData.Length > editValue.Text.Length)
                    sData = sData.Substring(0, editValue.Text.Length);

                if (sData == editValue.Text)
                {
                    dgViewRT.CurrentCell = dgViewRT.Rows[i].Cells[0];
                    dgViewRT.Rows[i].Selected = true;
                    break;
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (!chkbDate.Checked && string.IsNullOrEmpty(editRTNo.Text.Trim()) && string.IsNullOrEmpty(editVendor.Text.Trim()))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Query Condition"), 0);
                return;
            }
            GetData();
        }

        private void btnSearchWO_Click(object sender, EventArgs e)
        {
            try
            {
               
                    string sVendorCode = editVendor.Text.Trim();
                    sVendorCode = sVendorCode + "%";
                    string sSQL = "Select VENDOR_CODE,VENDOR_NAME FROM SAJET.SYS_VENDOR "
                          +" WHERE VENDOR_CODE LIKE  :VENDOR_CODE "
                          +"  AND ENABLED='Y' "
                          +" ORDER BY VENDOR_CODE ";
                    
                
                Object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_CODE", sVendorCode };
                fFilter f = new fFilter(Params, "1");
                try
                {
                    f.sSQL = sSQL;
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        if (f.dgvData.CurrentRow != null)
                        {
                           
                            editVendor.Text = f.dgvData.CurrentRow.Cells[0].Value.ToString();
                        }
                    }
                }
                finally
                {
                    f.Dispose();
                }
            }
            catch
            {
            }
        }

        /*
        public void addLog(LogType f_LogType, string f_sFunName, string f_sLog)
        {
           
            try
            {

                CommonControl.LogType lt = CommonControl.LogType.Normal;

                switch (f_LogType)
                {
                    case LogType.Debug:
                        lt = CommonControl.LogType.DebugNormal;
                        if (g_bDebug == false) { return; }  //V1.12007.0.13
                        break;
                    case LogType.Error: lt = CommonControl.LogType.Error; break;
                    case LogType.Normal: lt = CommonControl.LogType.Normal; break;
                    case LogType.Warning: lt = CommonControl.LogType.Warning; break;
                }

                m_log.addLog(lt, g_sProgram, f_sFunName, f_sLog);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                //myLog.WriteEntry("FunName[" + f_sFunName + "], Log[" + f_sLog + "], Exception[" + ex.Message + "]");  //V1.12007.0.13 - 2
            }
        }
         */ 
    }
}

