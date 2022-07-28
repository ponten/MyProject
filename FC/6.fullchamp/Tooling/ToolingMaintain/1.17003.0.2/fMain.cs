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
            for (int i=0;i<=dsTemp.Tables[0].Rows.Count-1;i++)
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
            labStatus.Text =  SajetCommon.SetLanguage(g_sProgramStatus);
            
            labStatus.BackColor = Color.DodgerBlue;
            g_sSQLStatus = Initial(g_sProgramStatus);
            if (g_sSQLStatus == "S")
                labStatus.BackColor = Color.Red;

            g_sUserID = ClientUtils.UserPara1;
            g_sUserNo = ClientUtils.fLoginUser;
            
            g_iPrivilege = 0;
            check_privilege();
            GetToolingType();
        }

        private string Initial(string sStatus)
        {
            string sR_Status = "";
            switch (sStatus)
            {
                case "Take": //���
                    sR_Status = "T";
                    break;
                case "Return": //�k��
                    sR_Status = "R";
                    break;
                case "Maintain": //�O�i
                    sR_Status = "M";
                    break;
                case "Scrap": //���o
                    sR_Status = "S";
                    break;

                case "Acceptance": //�禬
                    sR_Status = "A";
                    break;
                case "Repair": //�e��
                    sR_Status = "F";
                    break;
                default:
                    sR_Status = "I"; //��l
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
                        //�Ĥ@�Ӧ���ܪ����(focus���������|���~)
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
            //���X��ƦP�ɥ�SYS_TOOLING_STATUS_MOVE�ˬd�{�b���A�O�_�X�k
            sSQL = @"SELECT A.TOOLING_ID --, DECODE(A.TOOLING_TYPE,'T','Tooling','K','Knife') AS TOOLING_TYPE, 
                           ,C.TOOLING_TYPE_NO     
                           ,A.TOOLING_NO, A.TOOLING_NAME, A.TOOLING_DESC
                           ,D.LOCATION_NO
                           --,DECODE(STATUS, 'I','Initial','T','Take','R','Return','S','Scrap','A','Acceptance','F','Repair'  ) STATUS 
                           ,SAJET.SJ_TOOLING_STATUS_CHT(STATUS) STATUS
                           ,NVL(A.USED_TIME,0) USED_TIME
                           ,NVL(A.MAX_USED_COUNT,0) MAX_USED_COUNT 
                               -- ,A.LAST_MAINTAIN_TIME
                               -- ,DECODE(A.COMPANY, 'P','�ƪY','F','�I�h��') COMPANY
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


        private void ShowData()
        {
            string sType = "", sCom = "";
            bool bOverTime = false;

            
            sSQL = string.Format(GetSQL(), " AND A.TOOLING_NO  LIKE '" + editToolingNo.Text + "%'");

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            dgvData.Rows.Clear();


            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("No Data Found"), 0);
                editToolingNo.SelectAll();
                editToolingNo.Focus();
            }


            string sText = "";
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                dgvData.Rows.Add();

                //�P�_�w�ϥΦ��ƬO�_�w�g�j��ε���i�ϥΦ��ơA�O���H�������
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

            if (g_sSQLStatus == "T")//��ζ�����x���T
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

            //�P�_�w�ϥΦ��ƬO�_�w�g�j��ε���i�ϥΦ��ơA�O���H�������
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

            if (g_sSQLStatus == "R") //���A�O�k��,�i�H��J�ϥΦ���
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
            ShowData();
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

                    //�k�١G��key�J�����ϥΦ���
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

                    //���,�O�_�w�W�L�ϥΦ���
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

            sMsg = SajetCommon.SetLanguage("Selected Tooling Qty") + " : " + iQty.ToString() + Environment.NewLine
                 + SajetCommon.SetLanguage("Sure to") + " �i" + SajetCommon.SetLanguage(g_sProgramStatus) + " �j" + " ?";

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;


            if (bOverTime)
            {
                sMsg = iOverTimeCnt.ToString() + " " + SajetCommon.SetLanguage("Toolings' Used Time Greater Than Max Used Time")
                     + Environment.NewLine + SajetCommon.SetLanguage("Continue")+ " ?";
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;
            }


            for (int i = 0; i <= combToolingID.Items.Count - 1; i++)
            {
                string sToolingID = combToolingID.Items[i].ToString();

                //insert�@����G_TOOLING_MT_TRAVEL
                sSQL = @"INSERT INTO SAJET.G_TOOLING_MT_TRAVEL
                        (TOOLING_ID, STATUS, UPDATE_USERID, UPDATE_TIME, MEMO, USED_COUNT)
                        VALUES
                        (:TOOLING_ID, :STATUS, :UPDATE_USERID, SYSDATE, :MEMO, :USED_COUNT)";

                object[][] Params = new object[5][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STATUS", g_sSQLStatus };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MEMO", sMemo };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "USED_COUNT", sUseCnt };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);


                if (g_sSQLStatus == "R")  //�k�١GSYS_TOOLING.USED_TIME�n��s
                {
                    string sUseTime = combUseTime.Items[i].ToString();

                    sSQL = @"UPDATE SAJET.SYS_TOOLING 
                             SET STATUS = 'R', USED_TIME = NVL(USED_TIME,0) + :USED_TIME,
                                 UPDATE_TIME = SYSDATE
                             WHERE TOOLING_ID =  :TOOLING_ID";

                    Params = new object[2][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "USED_TIME", sUseTime };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                }
                else
                {
                    if (g_sSQLStatus == "S")   //���o�GSYS_TOOLING.ENABLED�n��s
                    {
                        sSQL = @"UPDATE SAJET.SYS_TOOLING 
                                 SET STATUS = 'S', ENABLED = 'N', 
                                     UPDATE_TIME = SYSDATE
                                 WHERE TOOLING_ID =  :TOOLING_ID";

                    }
                    else if (g_sSQLStatus == "M")   //�O�i�GSYS_TOOLING.STATUS�n��s�̫�O�i�ɶ�
                    {
                        sSQL = @"UPDATE SAJET.SYS_TOOLING 
                                 SET STATUS = 'M', LAST_MAINTAIN_TIME = SYSDATE, 
                                     UPDATE_TIME = SYSDATE, LAST_MAIL_TIME = SYSDATE 
                                 WHERE TOOLING_ID =  :TOOLING_ID";

                    }
                    else 
                    {
                        sSQL = string.Format(@"UPDATE SAJET.SYS_TOOLING 
                                 SET STATUS = '{0}', UPDATE_TIME = SYSDATE
                                 WHERE TOOLING_ID =  :TOOLING_ID", g_sSQLStatus);

                    }
                    Params = new object[1][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                }
            }
            sMsg = SajetCommon.SetLanguage("Tooling") + " " + SajetCommon.SetLanguage(g_sProgramStatus)
                        + " OK";
            SajetCommon.Show_Message(sMsg, 3);
            editMemo.Text = string.Empty;
            ShowData();
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
            //���P�_��TOOLING NO�O�_�s�b
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
                //�ˬd���A
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


