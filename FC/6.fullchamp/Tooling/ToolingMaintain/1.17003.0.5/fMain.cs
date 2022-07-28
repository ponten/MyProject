using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Globalization;
using SajetClass;
using System.Data.OracleClient;
using SajetFilter;
namespace ToolingMaintaindll
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }
        string g_sUserID, g_sUserNo, g_sProgram, g_sFunction, g_sParam, g_sProgramStatus, g_sSQLStatus, g_sCompany;
        
        DateTime g_dtTime;
        DataSet dsLocation;

        public static string g_sExeName;
        DataSet dsTemp;        
        string sSQL="";
        int g_iPrivilege;
        private void check_privilege()
        {
            g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram);
            
        }
        private void GetToolingType()
        {
            combToolingType.Items.Clear();
            combToolingType.Items.Add("ALL");
            sSQL = "Select * from SAJET.SYS_TOOLING_TYPE WHERE ENABLED='Y' ORDER BY TOOLING_TYPE_NO";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combToolingType.Items.Add(dsTemp.Tables[0].Rows[i]["TOOLING_TYPE_NO"].ToString());
            }

            //combToolingType.Items.Add(SajetCommon.SetLanguage("Knife"));
            //combToolingType.Items.Add(SajetCommon.SetLanguage("Tooling"));

            combToolingType.SelectedIndex = 0;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            g_sExeName = ClientUtils.fCurrentProject;
            g_sFunction = ClientUtils.fFunctionName;
            g_sProgram = ClientUtils.fProgramName;
            g_sParam = ClientUtils.fParameter;
            g_sProgramStatus = ClientUtils.fParameter;

            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";
            labStatus.Text = SajetCommon.SetLanguage(g_sProgramStatus);

            labStatus.BackColor = Color.DodgerBlue;
            g_sSQLStatus = Initial(g_sProgramStatus);
            if (g_sSQLStatus == "S")
                labStatus.BackColor = Color.Red;

            g_sUserID = ClientUtils.UserPara1;
            g_sUserNo = ClientUtils.fLoginUser;

            g_iPrivilege = 0;
            check_privilege();
            GetToolingType();

            //領用時,顯示機台輸入框
            editMachine.Tag = "";
            if (g_sSQLStatus != "T")
            {
                
                panelTake.Visible = false;
            }
        }
        private string Initial(string sStatus)
        {
            string sR_Status = "";
            switch (sStatus)
            {
                case "Take": //領用
                    sR_Status = "T";
                    break;
                case "Return": //歸還
                    sR_Status = "R";
                    break;
                case "Maintain": //保養
                    sR_Status = "M";
                    break;
                case "Scrap": //報廢
                    sR_Status = "S";
                    break;

                case "Acceptance": //驗收
                    sR_Status = "A";
                    break;
                case "Repair": //送修
                    sR_Status = "F";
                    break;
                default:
                    sR_Status = "I"; //初始
                    break;
            }

            return sR_Status;

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


        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvData.Rows.Clear();
        }

        private void btnFilterTooling_Click(object sender, EventArgs e)
        {
            string sType = "";

            /*
            if (string.IsNullOrEmpty(editToolingNo.Text))
            {
                SajetCommon.Show_Message("Please Input Prefix", 0);
                return;
            }
            */

            sSQL = string.Format(GetSQL(), " AND A.TOOLING_NO  LIKE '" + editToolingNo.Text + "%'");

            
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editToolingNo.Text = f.dgvData.CurrentRow.Cells["TOOLING_NO"].Value.ToString();
                ShowData(editToolingNo.Text);

                editToolingNo.SelectAll();
                editToolingNo.Focus();
            }
        }

        private string GetSQL()
        {
            //撈出資料同時由SYS_TOOLING_STATUS_MOVE檢查現在狀態是否合法
            sSQL = @"SELECT A.TOOLING_ID --, DECODE(A.TOOLING_TYPE,'T','Tooling','K','Knife') AS TOOLING_TYPE, 
                           ,C.TOOLING_TYPE_NO     
                           ,A.TOOLING_NO, A.TOOLING_NAME, A.TOOLING_DESC
                           ,D.LOCATION_NO
                           --,DECODE(STATUS, 'I','Initial','T','Take','R','Return','S','Scrap','A','Acceptance','F','Repair'  ) STATUS 
                           ,SAJET.SJ_TOOLING_STATUS_CHT(STATUS) STATUS
                           ,NVL(A.USED_TIME,0) USED_TIME
                           ,NVL(A.MAX_USED_COUNT,0) MAX_USED_COUNT 
                               -- ,A.LAST_MAINTAIN_TIME
                               -- ,DECODE(A.COMPANY, 'P','事欣','F','富士亨') COMPANY
                         FROM SAJET.SYS_TOOLING A, SAJET.SYS_TOOLING_STATUS_MOVE B
                             ,SAJET.SYS_TOOLING_TYPE c,SAJET.SYS_TOOLING_LOCATION D
                         WHERE A.ENABLED = 'Y' 
                         AND A.STATUS = B.CURRENT_STATUS_ID 
                         AND B.NEXT_STATUS_ID = '" + g_sSQLStatus + @"'
                         AND A.TOOLING_TYPE_ID = C.TOOLING_TYPE_ID(+) 
                         AND C.ENABLED = 'Y' 
                         AND A.LOCATION_ID = D.LOCATION_ID(+) 
                         {0}";

            if (combToolingType.SelectedIndex > 0)
            {
                /*
                if (combToolingType.SelectedIndex == 1)
                    sType = "K";
                else if (combToolingType.SelectedIndex == 2)
                    sType = "T";

                sSQL = sSQL + " AND TOOLING_TYPE ='" + sType + "' "; */

                sSQL = sSQL + " AND C.TOOLING_TYPE_NO ='" + combToolingType.Text + "' ";
            }

            sSQL = sSQL + " ORDER BY C.TOOLING_TYPE_NO, A.TOOLING_NO, STATUS ";

            return sSQL;
        }


        private void ShowData(bool bFlag)
        {
            string sType = "", sCom = "";
            bool bOverTime = false;

            
            sSQL = string.Format(GetSQL(), " AND A.TOOLING_NO  LIKE '" + editToolingNo.Text + "%'");

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            dgvData.Rows.Clear();


            if (bFlag && dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("No Data Found"), 0);
                editToolingNo.SelectAll();
                editToolingNo.Focus();
            }


            string sText = "";
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                dgvData.Rows.Add();

                //判斷已使用次數是否已經大於或等於可使用次數，是的以紅底表示
                int iUsedTime = int.Parse(dsTemp.Tables[0].Rows[i]["USED_TIME"].ToString());
                int iMaxUsedTime = int.Parse(dsTemp.Tables[0].Rows[i]["MAX_USED_COUNT"].ToString());
                if (iUsedTime >= iMaxUsedTime && iMaxUsedTime>0)
                {
                    //sText = sText + dsTemp.Tables[0].Rows[i]["TOOLING_NO"].ToString() + " || ";
                    dgvData.Rows[dgvData.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;
                    bOverTime = true;
                }

                
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["CHECKED"].Value = "N";
                //dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_TYPE"].Value = SajetCommon.SetLanguage(dsTemp.Tables[0].Rows[i]["TOOLING_TYPE"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_TYPE"].Value = dsTemp.Tables[0].Rows[i]["TOOLING_TYPE_NO"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_NO"].Value = (dsTemp.Tables[0].Rows[i]["TOOLING_NO"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_NAME"].Value = (dsTemp.Tables[0].Rows[i]["TOOLING_NAME"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_DESC"].Value = (dsTemp.Tables[0].Rows[i]["TOOLING_DESC"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["STATUS"].Value = SajetCommon.SetLanguage(dsTemp.Tables[0].Rows[i]["STATUS"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOTAL_USED_TIME"].Value = (dsTemp.Tables[0].Rows[i]["USED_TIME"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["MAX_USED_COUNT"].Value = (dsTemp.Tables[0].Rows[i]["MAX_USED_COUNT"].ToString());
                //dgvData.Rows[dgvData.Rows.Count - 1].Cells["LAST_MAINTAIN_TIME"].Value = (dsTemp.Tables[0].Rows[i]["LAST_MAINTAIN_TIME"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_ID"].Value = (dsTemp.Tables[0].Rows[i]["TOOLING_ID"].ToString());
                //dgvData.Rows[dgvData.Rows.Count - 1].Cells["COMPANY"].Value = (dsTemp.Tables[0].Rows[i]["COMPANY"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["USE_COUNT"].Value = "0";

                dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOL_LOCATION"].Value = dsTemp.Tables[0].Rows[i]["LOCATION_NO"].ToString();

            }

            if (g_sSQLStatus == "R")
            {
                dgvData.Columns["USE_COUNT"].Visible = true;
                dgvData.Columns["USE_COUNT"].ReadOnly = false;
            }

            if (g_sSQLStatus == "T")//領用僅顯示儲位資訊
            {
                dgvData.Columns["TOOL_LOCATION"].Visible = true;
            }

            if (bOverTime && (g_sSQLStatus == "T" || g_sSQLStatus == "M"))
            {
                string sMsg = SajetCommon.SetLanguage("Warning: Tooling Used Overtime");
                //sMsg = sMsg + Environment.NewLine + sText;
                SajetCommon.Show_Message(sMsg, 1);
            }


        }

        private void ShowData(string sToolingNo)
        {
            string sType = "", sCom = "";
            bool bOverTime = false;

            sSQL = string.Format(GetSQL(), " AND A.TOOLING_NO = '" + sToolingNo + "'");

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
                return;


            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                if (dgvData.Rows[i].Cells["TOOLING_NO"].Value.ToString() == (dsTemp.Tables[0].Rows[0]["TOOLING_NO"].ToString()))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Tooling No Duplicate"), 0);
                    editToolingNo.SelectAll();
                    editToolingNo.Focus();
                    return;
                }
            }


            dgvData.Rows.Add();

            //判斷已使用次數是否已經大於或等於可使用次數，是的以紅底表示
            int iUsedTime = int.Parse(dsTemp.Tables[0].Rows[0]["USED_TIME"].ToString());
            int iMaxUsedTime = int.Parse(dsTemp.Tables[0].Rows[0]["MAX_USED_COUNT"].ToString());
            if (iUsedTime >= iMaxUsedTime && iMaxUsedTime > 0)
            {

                dgvData.Rows[dgvData.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;
                bOverTime = true;
            }

            dgvData.Rows[dgvData.Rows.Count - 1].Cells["CHECKED"].Value = "Y";
            //dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_TYPE"].Value = SajetCommon.SetLanguage(dsTemp.Tables[0].Rows[0]["TOOLING_TYPE"].ToString());
            dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_TYPE"].Value = dsTemp.Tables[0].Rows[0]["TOOLING_TYPE_NO"].ToString();
            dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_NO"].Value = (dsTemp.Tables[0].Rows[0]["TOOLING_NO"].ToString());
            dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_NAME"].Value = (dsTemp.Tables[0].Rows[0]["TOOLING_NAME"].ToString());
            dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_DESC"].Value = (dsTemp.Tables[0].Rows[0]["TOOLING_DESC"].ToString());
            dgvData.Rows[dgvData.Rows.Count - 1].Cells["STATUS"].Value = SajetCommon.SetLanguage(dsTemp.Tables[0].Rows[0]["STATUS"].ToString());
            dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOTAL_USED_TIME"].Value = (dsTemp.Tables[0].Rows[0]["USED_TIME"].ToString());
            dgvData.Rows[dgvData.Rows.Count - 1].Cells["MAX_USED_COUNT"].Value = (dsTemp.Tables[0].Rows[0]["MAX_USED_COUNT"].ToString());
            //dgvData.Rows[dgvData.Rows.Count - 1].Cells["LAST_MAINTAIN_TIME"].Value = (dsTemp.Tables[0].Rows[0]["LAST_MAINTAIN_TIME"].ToString());
            dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOLING_ID"].Value = (dsTemp.Tables[0].Rows[0]["TOOLING_ID"].ToString());
            //dgvData.Rows[dgvData.Rows.Count - 1].Cells["COMPANY"].Value = (dsTemp.Tables[0].Rows[0]["COMPANY"].ToString());
            dgvData.Rows[dgvData.Rows.Count - 1].Cells["USE_COUNT"].Value = "0";
            dgvData.Rows[dgvData.Rows.Count - 1].Cells["TOOL_LOCATION"].Value = dsTemp.Tables[0].Rows[0]["LOCATION_NO"].ToString();

            if (g_sSQLStatus == "R") //狀態是歸還,可以輸入使用次數
            {
                dgvData.Columns["USE_COUNT"].Visible = true;
                dgvData.Columns["USE_COUNT"].ReadOnly = false;
            }

            if (g_sSQLStatus == "A")
            {
                dgvData.Columns["TOOL_LOCATION"].Visible = true;
                dgvData.Columns["TOOL_LOCATION"].ReadOnly = false;
            }

            if (bOverTime && (g_sSQLStatus == "T" || g_sSQLStatus == "M"))
            {
                string sMsg = SajetCommon.SetLanguage("Warning: Tooling Used Overtime");
                SajetCommon.Show_Message(sMsg, 1);
            }


        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            ShowData(true);
        }

        private void btnFilterMachine_Click(object sender, EventArgs e)
        {

            sSQL = " Select a.MACHINE_CODE,a.MACHINE_DESC,b.MACHINE_TYPE_NAME,a.MACHINE_ID "
                 + " FROM SAJET.SYS_MACHINE a,SAJET.SYS_MACHINE_TYPE b "
                 + " WHERE a.ENABLED = 'Y'  "
                 + " and a.MACHINE_TYPE_ID = b.MACHINE_TYPE_ID(+) "
                 + " order by a.MACHINE_CODE ";


            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editMachine.Text = f.dgvData.CurrentRow.Cells["MACHINE_CODE"].Value.ToString();
                LabMachineDesc.Text = f.dgvData.CurrentRow.Cells["MACHINE_DESC"].Value.ToString();
                editMachine.Tag = f.dgvData.CurrentRow.Cells["MACHINE_ID"].Value.ToString();

                editTakeUser.SelectAll();
                editTakeUser.Focus();
            }
        }

        private void editMachine_KeyPress(object sender, KeyPressEventArgs e)
        {
            LabMachineDesc.Text = "";
            editMachine.Tag = ""; 
            if (e.KeyChar != (char)Keys.Return)
                return;
            editMachine.Text = editMachine.Text.Trim();
            if (string.IsNullOrEmpty(editMachine.Text))
                return;


            sSQL = " Select a.MACHINE_CODE,a.MACHINE_DESC,a.MACHINE_ID "
                + " FROM SAJET.SYS_MACHINE a,SAJET.SYS_MACHINE_TYPE b "
                + " WHERE a.MACHINE_CODE=:MACHINE_CODE "
                + " AND a.ENABLED = 'Y' AND NVL(B.ENABLED,'Y')='Y' "
                + " and a.MACHINE_TYPE_ID = b.MACHINE_TYPE_ID(+) ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_CODE", editMachine.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count==0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Data not Exist")+Environment.NewLine+LabMachine.Text, 0);
                editMachine.SelectAll();
                editMachine.Focus();
                return;
            }
            LabMachineDesc.Text = dsTemp.Tables[0].Rows[0]["MACHINE_DESC"].ToString();
            editMachine.Tag = dsTemp.Tables[0].Rows[0]["MACHINE_ID"].ToString();

            editTakeUser.SelectAll();
            editTakeUser.Focus();
        }

        private void editTakeUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            LabTakeName.Text = "";
            editTakeUser.Tag = "";
            if (e.KeyChar != (char)Keys.Return)
                return;
            editTakeUser.Text = editTakeUser.Text.Trim();
            if (string.IsNullOrEmpty(editTakeUser.Text))
                return;


            sSQL = " Select a.EMP_NO,a.EMP_NAME,a.EMP_ID "
                + " FROM SAJET.SYS_EMP a "
                + " WHERE a.EMP_NO=:EMP_NO "
                + " AND a.ENABLED = 'Y' ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", editTakeUser.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Data not Exist") +Environment.NewLine+ label1.Text, 0);
                editTakeUser.SelectAll();
                editTakeUser.Focus();
                return;
            }
            LabTakeName.Text = dsTemp.Tables[0].Rows[0]["EMP_NAME"].ToString();
            editTakeUser.Tag = dsTemp.Tables[0].Rows[0]["EMP_ID"].ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sMemo = editMemo.Text = editMemo.Text.Trim();
            string sUseCnt = "0";
            string sToolingData = string.Empty;
            string sMsg;
            bool bOverTime = false;
            int iOverTimeCnt = 0;
            
            ComboBox combToolingID = new ComboBox();
            ComboBox combUseTime = new ComboBox();

            bool bCheckUseCount = true;
            int iQty = 0;
            for (int i = 0; i <= dgvData.Rows.Count - 1; i++)
            {
                if (dgvData.Rows[i].Cells["CHECKED"].Value.ToString() == "Y")
                {
                    iQty++;

                    combToolingID.Items.Add(dgvData.Rows[i].Cells["TOOLING_ID"].Value.ToString());
                    sToolingData = sToolingData + dgvData.Rows[i].Cells["TOOLING_ID"].Value.ToString() + "@";

                    //歸還：需key入此次使用次數
                    if (g_sSQLStatus == "R")
                    {
                        sUseCnt = dgvData.Rows[i].Cells["USE_COUNT"].Value.ToString();
                        if (sUseCnt == "0" && bCheckUseCount)
                        {
                            //SajetCommon.Show_Message("Please Enter Use Count", 0);
                            //return;

                            string sConfirmMsg = SajetCommon.SetLanguage("Please Enter Use Count") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Continue") + "?";
                            if (SajetCommon.Show_Message(sConfirmMsg, 2) == DialogResult.Yes)
                            {
                                bCheckUseCount = false;
                            }
                            else
                            {
                                return;
                            }

                        }

                        combUseTime.Items.Add(sUseCnt);
                    }

                    //領用,是否已超過使用次數
                    if (g_sSQLStatus == "T" && dgvData.Rows[i].DefaultCellStyle.ForeColor == Color.Red)
                    {
                        bOverTime = true;
                        iOverTimeCnt++;
                    }

                }
            }
            if (iQty == 0)
            {
                return;
            }

            //領用時,需輸入機台號(且必填)
            if (g_sSQLStatus == "T")
            {
                if (!string.IsNullOrEmpty(editMachine.Text) && string.IsNullOrEmpty(editMachine.Tag.ToString()))
                {
                    editMachine_KeyPress(editMachine, new KeyPressEventArgs((Char)Keys.Enter));
                    if (string.IsNullOrEmpty(editMachine.Tag.ToString()))
                        return;
                }

                if (string.IsNullOrEmpty(editMachine.Text) || string.IsNullOrEmpty(editMachine.Tag.ToString()))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Data is null") + Environment.NewLine + LabMachine.Text, 0);
                    editMachine.SelectAll();
                    editMachine.Focus();
                    return;
                }

                //領用人
                if (!string.IsNullOrEmpty(editTakeUser.Text) && string.IsNullOrEmpty(editTakeUser.Tag.ToString()))
                {
                    editTakeUser_KeyPress(editTakeUser, new KeyPressEventArgs((Char)Keys.Enter));
                    if (string.IsNullOrEmpty(editTakeUser.Tag.ToString()))
                        return;
                }

                if (string.IsNullOrEmpty(editTakeUser.Text) || string.IsNullOrEmpty(editTakeUser.Tag.ToString()))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Data is null") + Environment.NewLine + label1.Text, 0);
                    editTakeUser.SelectAll();
                    editTakeUser.Focus();
                    return;
                }

                g_sUserID = editTakeUser.Tag.ToString();
            }


            sMsg = SajetCommon.SetLanguage("Selected Tooling Qty") + " : " + iQty.ToString() + Environment.NewLine
                 + SajetCommon.SetLanguage("Sure to") + " 【" + SajetCommon.SetLanguage(g_sProgramStatus) + " 】" + " ?";

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;


            if (bOverTime)
            {
                sMsg = iOverTimeCnt.ToString() + " " + SajetCommon.SetLanguage("Toolings' Used Time Greater Than Max Used Time")
                     + Environment.NewLine + SajetCommon.SetLanguage("Continue")+ " ?";
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;
            }


            DateTime dtSysDate = ClientUtils.GetSysDate();
            for (int i = 0; i <= combToolingID.Items.Count - 1; i++)
            {
                string sToolingID = combToolingID.Items[i].ToString();

                //insert一筆至G_TOOLING_MT_TRAVEL
                sSQL = @"INSERT INTO SAJET.G_TOOLING_MT_TRAVEL
                        (TOOLING_ID, STATUS, UPDATE_USERID, UPDATE_TIME, MEMO, USED_COUNT,MACHINE_ID)
                        VALUES
                        (:TOOLING_ID, :STATUS, :UPDATE_USERID, :UPDATE_TIME, :MEMO, :USED_COUNT,:MACHINE_ID)";

                object[][] Params = new object[7][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STATUS", g_sSQLStatus };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtSysDate };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MEMO", sMemo };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "USED_COUNT", sUseCnt };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", string.IsNullOrEmpty(editMachine.Tag.ToString()) ? "0" : editMachine.Tag.ToString() };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                //紀錄使用機台:領用
                if (g_sSQLStatus == "T")
                {
                    sSQL = @"DELETE SAJET.G_MACHINE_TOOLING
                             WHERE TOOLING_ID = :TOOLING_ID ";
                    object[][] Params1 = new object[1][];
                    Params1[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
                    ClientUtils.ExecuteSQL(sSQL, Params1);

                    sSQL = @"INSERT INTO SAJET.G_MACHINE_TOOLING
                        (MACHINE_ID, TOOLING_ID, UP_TIME,UP_USERID)
                        VALUES
                        (:MACHINE_ID, :TOOLING_ID, :UP_TIME, :UP_USERID)";
                    Params1 = new object[4][];
                    Params1[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", editMachine.Tag.ToString() };
                    Params1[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
                    Params1[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UP_TIME", dtSysDate };
                    Params1[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UP_USERID", g_sUserID };
                    ClientUtils.ExecuteSQL(sSQL, Params1);
                }

                if (g_sSQLStatus == "R")  //歸還：SYS_TOOLING.USED_TIME要更新
                {
                    string sUseTime = combUseTime.Items[i].ToString();

                    sSQL = @"UPDATE SAJET.SYS_TOOLING 
                             SET STATUS = 'R', USED_TIME = NVL(USED_TIME,0) + :USED_TIME,
                                 UPDATE_TIME = :UPDATE_TIME
                             WHERE TOOLING_ID =  :TOOLING_ID";

                    Params = new object[3][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "USED_TIME", sUseTime };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtSysDate };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                    //將MchineID改為0表示在刀具間
                    sSQL = @"UPDATE SAJET.G_MACHINE_TOOLING
                             SET MACHINE_ID=0,DOWN_TIME=:DOWN_TIME,DOWN_USERID=:DOWN_USERID
                            WHERE TOOLING_ID=:TOOLING_ID";
                    Params = new object[3][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "DOWN_TIME", dtSysDate };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DOWN_USERID", g_sUserID };
                    ClientUtils.ExecuteSQL(sSQL, Params);
                }
                else
                {
                    if (g_sSQLStatus == "S")   //報廢：SYS_TOOLING.ENABLED要更新
                    {
                        sSQL = @"UPDATE SAJET.SYS_TOOLING 
                                 SET STATUS = 'S', ENABLED = 'N', 
                                     UPDATE_TIME = :UPDATE_TIME
                                 WHERE TOOLING_ID =  :TOOLING_ID";

                        Params = new object[2][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtSysDate };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
                        ClientUtils.ExecuteSQL(sSQL, Params);

                    }
                    else if (g_sSQLStatus == "M")   //保養：SYS_TOOLING.STATUS要更新最後保養時間
                    {
                        sSQL = @"UPDATE SAJET.SYS_TOOLING 
                                 SET STATUS = 'M', LAST_MAINTAIN_TIME = SYSDATE, 
                                     UPDATE_TIME = :UPDATE_TIME, LAST_MAIL_TIME = :LAST_MAIL_TIME 
                                 WHERE TOOLING_ID =  :TOOLING_ID";
                        Params = new object[3][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtSysDate };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "LAST_MAIL_TIME", dtSysDate };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
                        ClientUtils.ExecuteSQL(sSQL, Params);

                    }
                    else 
                    {
                        sSQL = string.Format(@"UPDATE SAJET.SYS_TOOLING 
                                 SET STATUS = '{0}', UPDATE_TIME = :UPDATE_TIME,UPDATE_USERID = :UPDATE_USERID
                                 WHERE TOOLING_ID =  :TOOLING_ID", g_sSQLStatus);
                        Params = new object[3][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtSysDate };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        ClientUtils.ExecuteSQL(sSQL, Params);

                    }
                    
                }
            }
            sMsg = SajetCommon.SetLanguage("Tooling") + " " + SajetCommon.SetLanguage(g_sProgramStatus)
                        + " OK";
            SajetCommon.Show_Message(sMsg, 3);
            editMemo.Text = string.Empty;
            ShowData(false);

            editMachine.Text = "";
            editMachine.Tag = "";
            LabMachineDesc.Text = "";
            editTakeUser.Text = "";
            editTakeUser.Tag = "";
            LabTakeName.Text = "";
        }

        private void btnSelectALL_Click(object sender, EventArgs e)
        {
            string sFlag = "Y";
            string sTag = (sender as Button).Tag.ToString();
            if (sTag=="1")
                sFlag ="N";
            for (int i = 0; i <= dgvData.Rows.Count - 1; i++)
            {
                dgvData.Rows[i].Cells["CHECKED"].Value = sFlag;
            }
        }

        private void dgvData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    dgvData.Rows[e.RowIndex].Cells[0].ReadOnly = false;
                }
                else if (g_sSQLStatus == "R" && dgvData.Columns[e.ColumnIndex].Name == "USE_COUNT")
                {
                    dgvData.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
                }
                else
                {
                    dgvData.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                }
            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Rows.Count == 0 || dgvData.CurrentRow == null || dgvData.SelectedCells.Count == 0)
            {
                return;
            }
            string sToolingID = dgvData.CurrentRow.Cells["TOOLING_ID"].Value.ToString();
            fHistory fData = new fHistory();
            try
            {
                fData.g_sFeederID = sToolingID;
                fData.ShowDialog();
            }
            finally
            {
                fData.Dispose();
            }
            


        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            editToolingNo.Text = null;
            dgvData.Rows.Clear();

            editMachine.Text = "";
            editMachine.Tag = "";
            LabMachineDesc.Text = "";

            editTakeUser.Text = "";
            editTakeUser.Tag = "";
            LabTakeName.Text = "";
        }


        private void editToolingNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            if (string.IsNullOrEmpty(editToolingNo.Text))
                return;


            if (!CheckToolingNo(editToolingNo.Text))
            {

                return;
            }

            ShowData(editToolingNo.Text);

            editToolingNo.SelectAll();
            editToolingNo.Focus();
        }

        private bool CheckToolingNo(string sToolingNo)
        {
            //先判斷此TOOLING NO是否存在
            sSQL = @"SELECT TOOLING_NO, STATUS
                     FROM SAJET.SYS_TOOLING
                     WHERE TOOLING_NO = :TOOLING_NO";

        

            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_NO", sToolingNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Tooling Not Exist"), 0);
                editToolingNo.SelectAll();
                editToolingNo.Focus();
                return false;
            }
            else
            {
                //檢查狀態
                string sStatus = dsTemp.Tables[0].Rows[0]["STATUS"].ToString();
                sSQL = @"SELECT * 
                         FROM SAJET.SYS_TOOLING_STATUS_MOVE
                         WHERE CURRENT_STATUS_ID = '" + sStatus + @"'
                         AND NEXT_STATUS_ID = '" + g_sSQLStatus + "'";

                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Tooling No Status Error"), 0);
                    editToolingNo.SelectAll();
                    editToolingNo.Focus();
                    return false;
                }
                return true;
            }
        }

    }
}


