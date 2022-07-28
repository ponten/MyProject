using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;


namespace CMachine
{
    public partial class fDetailData : Form
    {
        public string g_sType;
        string sSQL;
        DataSet dsTemp;
        string g_sSelectDate;
        public bool g_bUpdateSuccess;
        public string g_sMachineID, g_sMachineCode;
        public string g_sUserID;
        public int g_iMonthQty;
        public int g_iPlanDateQty;
        string[] g_sScheduleDate;
        public struct TPlanDate
        {
            public string sMachineID;
            public string sPlanDate;
            public string sValue;
            public string sExecuteFlag;
            public bool bExist;
        }
        public TPlanDate[] G_tPlanDate;
        public struct TControlData
        {
            public string sYear;
            public string sMonth;
            public DataGridView dgvData;
        }
        public TControlData[] tControlAdd;

        public fDetailData()
        {
            InitializeComponent();
        }

        private void fDetailData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            if (g_sType == "OUTPUT_QTY")
            {
                lablValueTitle.Text = SajetCommon.SetLanguage("Output Qty");
                lablUnit.Text = SajetCommon.SetLanguage("(Qty)");
                this.Text = SajetCommon.SetLanguage("Maintain Machine Output Qty");
            }
            if (g_sType == "WORKING_TIME")
            {
                lablValueTitle.Text = SajetCommon.SetLanguage("Working Time");
                lablUnit.Text = SajetCommon.SetLanguage("(Min.)");
                this.Text = SajetCommon.SetLanguage("Maintain Machine Working Time");
            }
            dateTimePicker3.Value = DateTime.Now;
            dateTimePicker4.Value = DateTime.Now;
            editYear.Text = DateTime.Now.ToString("yyyy");
            int iYear = Convert.ToInt32(editYear.Text);
            combYear.Items.Clear();
            for (int i = 10; i >= 1; i--)
            {
                combYear.Items.Add(iYear - i);
            }
            for (int i = 0; i<=5;i++)
            {
                combYear.Items.Add(iYear + i);
            }
            dgvData.Columns[3].HeaderText = lablValueTitle.Text;
             
                       
            g_sScheduleDate = new string[0];
            PanelMonth.Controls.Clear();
            g_iPlanDateQty = 0;
          //  DateTime dtTime1 = Convert.ToDateTime(DateTime.Parse(dateTimePicker1.Text).ToString("yyyy/MM/dd"));
         //   DateTime dtTime2 = Convert.ToDateTime(DateTime.Parse(dateTimePicker2.Text).ToString("yyyy/MM/dd"));
            DateTime dtTime1 = Convert.ToDateTime(DateTime.Parse(editYear.Text+"/01/01").ToString("yyyy/MM/dd"));
            DateTime dtTime2 = Convert.ToDateTime(DateTime.Parse(editYear.Text+"/12/31").ToString("yyyy/MM/dd"));

            //DisplayCalendar(dtTime1, dtTime2);

            g_bUpdateSuccess = false;
            labMachineCode.Text = g_sMachineCode;
            combYear.SelectedIndex = combYear.FindString(editYear.Text);
            //btnQuery_Click(sender, e);

        }
        private void Get_Day(int iYear, int iMonth, ref int iWeekNum, ref int iLastDay)
        {
            DateTime tDate = new DateTime(iYear, iMonth, 1);
            iWeekNum = Convert.ToInt32((tDate.DayOfWeek).ToString("d"));
            iLastDay = DateTime.DaysInMonth(iYear, iMonth);
        }
        private string[] GetScheduleDate(DateTime dtStart, DateTime dtEnd, string sCycleUnit)
        {
            string sResult = string.Empty;
            string sStartDate = dtStart.ToString("yyyyMMdd");
            string sEndDate = dtEnd.ToString("yyyyMMdd");
            sResult = sResult + sStartDate + ",";
            if (sCycleUnit == "Y")
            {
                int iYearQty = 0;
                while (true)
                {
                    dtStart = dtStart.AddYears(1);
                    sStartDate = dtStart.ToString("yyyyMMdd");
                    if (Convert.ToInt32(sStartDate) > Convert.ToInt32(sEndDate))
                        break;
                    else
                        sResult = sResult + sStartDate + ",";
                }
            }
            if (sCycleUnit == "M")
            {
                int iMontyQty = 0;
                while (true)
                {
                    iMontyQty += 1;
                    dtStart = dtStart.AddMonths(1);
                    sStartDate = dtStart.ToString("yyyyMMdd");
                    if (Convert.ToInt32(sStartDate) > Convert.ToInt32(sEndDate))
                        break;
                    else
                        sResult = sResult + sStartDate + ",";
                }
            }
            if (sCycleUnit == "Q")
            {
                while (true)
                {
                    dtStart = dtStart.AddMonths(3);
                    sStartDate = dtStart.ToString("yyyyMMdd");
                    if (Convert.ToInt32(sStartDate) > Convert.ToInt32(sEndDate))
                        break;
                    else
                        sResult = sResult + sStartDate + ",";
                }
            }
            if (sCycleUnit == "S")
            {
                while (true)
                {
                    dtStart = dtStart.AddMonths(6);
                    sStartDate = dtStart.ToString("yyyyMMdd");
                    if (Convert.ToInt32(sStartDate) > Convert.ToInt32(sEndDate))
                        break;
                    else
                        sResult = sResult + sStartDate + ",";
                }
            }
            if (sCycleUnit == "W")
            {
                while (true)
                {
                    dtStart = dtStart.AddDays(7);
                    sStartDate = dtStart.ToString("yyyyMMdd");
                    if (Convert.ToInt32(sStartDate) > Convert.ToInt32(sEndDate))
                        break;
                    else
                        sResult = sResult + sStartDate + ",";
                }
            }
            if (sCycleUnit == "2W")
            {
                while (true)
                {
                    dtStart = dtStart.AddDays(14);
                    sStartDate = dtStart.ToString("yyyyMMdd");
                    if (Convert.ToInt32(sStartDate) > Convert.ToInt32(sEndDate))
                        break;
                    else
                        sResult = sResult + sStartDate + ",";
                }
            }
            if (sCycleUnit == "D")
            {
                while (true)
                {
                    dtStart = dtStart.AddDays(1);
                    sStartDate = dtStart.ToString("yyyyMMdd");
                    if (Convert.ToInt32(sStartDate) > Convert.ToInt32(sEndDate))
                        break;
                    else
                        sResult = sResult + sStartDate + ",";
                }
            }
            string[] s = sResult.Trim(',').Split(new Char[] { ',' });
            return s;
        }
        private int GetMonthQty(DateTime dtStart, DateTime dtEnd)
        {
            int iQty = 1;
            string sStartDate = dtStart.ToString("yyyyMM");
            string sEndDate = dtEnd.ToString("yyyyMM");
            while (true)
            {
                dtStart = dtStart.AddMonths(1);
                sStartDate = dtStart.ToString("yyyyMM");
                if (Convert.ToInt32(sStartDate) > Convert.ToInt32(sEndDate))
                    break;
                else
                    iQty += 1;
            }
            return iQty;
        }
        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!(sender is DataGridView))
                return;
            int iTag = (Int32)((sender as DataGridView).TabIndex);
            string sDate = GetSelectedDate(iTag);// sYear + sMonth + g_sSelectDate.PadLeft(2, '0');
            bool bExist = false;
            try
            {
                string sDate1 = sDate.Substring(0, 4) + "/" + sDate.Substring(4, 2) + "/" + sDate.Substring(6, 2);
                dateTimePicker3.Value = Convert.ToDateTime(sDate1);
                dateTimePicker4.Value = Convert.ToDateTime(sDate1);
                for (int i = 0; i <= g_iPlanDateQty - 1; i++)
                {
                    bExist = (sDate == G_tPlanDate[i].sPlanDate);
                    if (bExist)
                    {
                        editValue.Text = G_tPlanDate[i].sValue;
                        break;
                    }
                }
            }
            catch
            {
            }
            /*
            bool bExist = false;
            int iIndex = -1;
            for (int i = 0; i <= g_iPlanDateQty - 1; i++)
            {
                bExist = (sDate == G_tPlanDate[i].sPlanDate);
                if (bExist)
                {
                    CancelData(sDate);
                    return;
                }
            }
            for (int i = 0; i <= g_sScheduleDate.Length - 1; i++)
            {
                bExist = (sDate == g_sScheduleDate[i].ToString());
                if (bExist)
                {
                    CancelData(sDate);
                    return;
                }
            }
            AppendData(sDate);
             */ 

        }
        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (sender is DataGridView)
            {
                int iRow = e.RowIndex;
                int iCol = e.ColumnIndex;
                g_sSelectDate = (sender as DataGridView).Rows[iRow].Cells[iCol].Value.ToString();
            }

        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            /*
            if (string.IsNullOrEmpty(editStartDate.Text) || string.IsNullOrEmpty(editEndDate.Text))
            {
                btnQuery_Click(sender, e);
                if (string.IsNullOrEmpty(editStartDate.Text) || string.IsNullOrEmpty(editEndDate.Text))
                {
                    return;
                }
            }
          //  combUnit.SelectedIndex = combCycle.SelectedIndex;
            string sCycleUnit = SajetCommon.GetID("SAJET.SYS_MACHINE_MT_CYCLE", "CYCLE_UNIT", "CYCLE_ID", combCycleID.Text);
            DateTime dtTime1 = Convert.ToDateTime(editStartDate.Text);
            string sStartMonth = editStartDate.Text.Substring(0, 4) + editStartDate.Text.Substring(5, 2);
            DateTime dtTime2 = Convert.ToDateTime(editEndDate.Text);
            DateTime dtTime3 = Convert.ToDateTime(DateTime.Parse(dateTimePicker3.Text).ToString("yyyy/MM/dd"));
            string[] sScheduleData = GetScheduleDate(dtTime3, dtTime2, sCycleUnit);
            string sData = string.Empty;
            g_sScheduleDate = new string[0];
            //系統排好的日期若已存在資料庫
            for (int i = 0; i <= sScheduleData.Length - 1; i++)
            {
                bool bExist = false;
                
                for (int j = 0; j <= g_iPlanDateQty - 1; j++)
                {
                    bExist = (sScheduleData[i] == G_tPlanDate[j].sPlanDate);
                    if (bExist)
                        break;
                    
                }
                if (!bExist)
                {
                    int iTemp = Convert.ToInt32(sScheduleData[i].Substring(0,6));
                    bExist = (iTemp < Convert.ToInt32(sStartMonth));
                }
                if (!bExist)
                    sData = sData + sScheduleData[i] + ",";
            }
            if (!string.IsNullOrEmpty(sData))
            {
                g_sScheduleDate = sData.Trim(',').Split(new Char[] { ',' });
            }
            DisplayExistData();
            DisplayPreviewResult();
            editPlanDays.Text = Convert.ToString(g_iPlanDateQty + g_sScheduleDate.Length);
            editBaseDate.Text = DateTime.Parse(dateTimePicker3.Text).ToString("yyyy/MM/dd");
             */ 
        }
        private void DisplayPreviewResult()
        {
            
            for (int i = 0; i <= g_iMonthQty - 1; i++)
            {
                for (int j = 0; j <= tControlAdd[i].dgvData.Rows.Count - 1; j++)
                {
                    for (int k = 0; k <= 6; k++)
                    {
                        string sDate1 = tControlAdd[i].dgvData.Rows[j].Cells[k].Value.ToString();
                        if (string.IsNullOrEmpty(sDate1))
                            continue;
                        sDate1 = tControlAdd[i].sYear + tControlAdd[i].sMonth + sDate1.PadLeft(2, '0');
                        for (int m = 0; m <= g_sScheduleDate.Length - 1; m++)
                        {
                            if (sDate1 == g_sScheduleDate[m].ToString())
                            {
                                if (tControlAdd[i].dgvData.Rows[j].Cells[k].Style.BackColor == Color.White || tControlAdd[i].dgvData.Rows[j].Cells[k].Style.BackColor == Color.Silver)
                                    tControlAdd[i].dgvData.Rows[j].Cells[k].Style.BackColor = Color.Red;
                                break;
                            }
                        }
                    }
                }
                tControlAdd[i].dgvData.ClearSelection();
//                tControlAdd[i].dgvData.Rows[tControlAdd[i].dgvData.Rows.Count - 1].Cells[tControlAdd[i].dgvData.Columns.Count - 1].
            }
             /* 
            editPlanDays.Text = Convert.ToString(g_iPlanDateQty + g_sScheduleDate.Length);
             */ 
        }
        private string GetSelectedDate(int iTag)
        {
            string sYear = tControlAdd[iTag].sYear;
            string sMonth = tControlAdd[iTag].sMonth;
            string sDate = sYear + sMonth + g_sSelectDate.PadLeft(2, '0');
            return sDate;
        }
        private void AppendData(string sDate)
        {
            bool bExist = false;
            int iIndex = -1;
            for (int i = 0; i <= g_iPlanDateQty - 1; i++)
            {
                bExist = (sDate == G_tPlanDate[i].sPlanDate);
                if (bExist)
                {
                    break;
                }
            }
            if (!bExist)
            {
                for (int i = 0; i <= g_sScheduleDate.Length - 1; i++)
                {
                    bExist = (sDate == g_sScheduleDate[i].ToString());
                    if (bExist)
                    {
                        break;
                    }
                }
            }
            if (!bExist)
            {
                string[] sData = new string[g_sScheduleDate.Length + 1];
                for (int i = 0; i <= g_sScheduleDate.Length - 1; i++)
                {
                    sData[i] = g_sScheduleDate[i];
                }
                sData[sData.Length - 1] = sDate;
                g_sScheduleDate = sData;
                DisplayPreviewResult();
            }

        }
        private void tspAppend_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem))
                return;
            int iTag = (Int32)((sender as ToolStripMenuItem).Tag);
            string sDate = GetSelectedDate(iTag);// sYear + sMonth + g_sSelectDate.PadLeft(2, '0');
            
            try
            {
                sDate = sDate.Substring(0, 4) + "/" + sDate.Substring(4, 2) + "/" + sDate.Substring(6, 2);
                dateTimePicker3.Value = Convert.ToDateTime(sDate);
            }
            catch
            {
            }
            
            //Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "/" + "01" + "/01");
            //AppendData(sDate);
            /*
            bool bExist = false;
            int iIndex = -1;
            for (int i = 0; i <= g_iPlanDateQty - 1; i++)
            {
                bExist = (sDate == G_tPlanDate[i].sPlanDate);
                if (bExist)
                {
                    break;
                }
            }
            if (!bExist)
            {
                for (int i = 0; i <= g_sScheduleDate.Length - 1; i++)
                {
                    bExist = (sDate == g_sScheduleDate[i].ToString());
                    if (bExist)
                    {
                        break;
                    }
                }
            }
            if (!bExist)
            {
                string[] sData = new string[g_sScheduleDate.Length +1];
                for (int i = 0; i <= g_sScheduleDate.Length - 1; i++)
                {
                    sData[i] = g_sScheduleDate[i];
                }
                sData[sData.Length - 1] = sDate;
                g_sScheduleDate = sData;
                DisplayPreviewResult();
            }
             */ 
        }
        private void tspDisplay_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem))
                return;
            int iTag = (Int32)((sender as ToolStripMenuItem).Tag);
            string sDate = GetSelectedDate(iTag);// sYear + sMonth + g_sSelectDate.PadLeft(2, '0');
           
            try
            {
                sDate = sDate.Substring(0, 4) + "/" + sDate.Substring(4, 2) + "/" + sDate.Substring(6, 2);
                dateTimePicker4.Value = Convert.ToDateTime(sDate);
            }
            catch
            {
            }
            /*
            bool bExist = false;
            int iIndex = -1;
            for (int i = 0; i <= g_iPlanDateQty - 1; i++)
            {
                bExist = (sDate == G_tPlanDate[i].sPlanDate);
                if (bExist)
                {
                    break;
                }
            }
            if (!bExist)
                return;
                
            /*
            fCycleItem fData = new fCycleItem();
            try
            {
                fData.g_sPlanID = g_sPlanID;
                fData.g_sDate = sDate;
                fData.ShowDialog();
            }
            finally
            {
                fData.Dispose();
            }
             */ 
            

        }
        private void CancelData(string sDate)
        {
            bool bExist = false;
            int iIndex = -1;
            for (int i = 0; i <= g_iPlanDateQty - 1; i++)
            {
                bExist = (sDate == G_tPlanDate[i].sPlanDate);
                if (bExist)
                {
                    if (G_tPlanDate[i].sExecuteFlag == "Y")
                        return;

                    iIndex = i;
                    break;
                }
            }
            if (bExist)
            {

                TPlanDate[] tPlanData = new TPlanDate[g_iPlanDateQty - 1];
                int iRow = 0;
                for (int i = 0; i <= g_iPlanDateQty - 1; i++)
                {
                    if (i == iIndex)
                        continue;
                    tPlanData[iRow] = G_tPlanDate[i];
                    iRow += 1;
                }
                g_iPlanDateQty -= 1;
                G_tPlanDate = tPlanData;
            }
            else
            {
                for (int i = 0; i <= g_sScheduleDate.Length - 1; i++)
                {
                    bExist = (sDate == g_sScheduleDate[i].ToString());
                    if (bExist)
                    {
                        iIndex = i;
                        break;
                    }
                }
                if (bExist)
                {
                    string[] sData = new string[g_sScheduleDate.Length - 1];
                    int iRow = 0;
                    for (int i = 0; i <= g_sScheduleDate.Length - 1; i++)
                    {
                        if (i == iIndex)
                            continue;
                        sData[iRow] = g_sScheduleDate[i];
                        iRow += 1;
                    }
                    g_sScheduleDate = sData;
                }
            }
            if (bExist)
            {
                DisplayExistData();
                DisplayPreviewResult();
            }

        }
        private void tspCancel_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem))
                return;
            int iTag = (Int32)((sender as ToolStripMenuItem).Tag);
            string sDate = GetSelectedDate(iTag);
            CancelData(sDate);
            /*
            bool bExist = false;
            int iIndex = -1;
            for (int i = 0; i <= g_iPlanDateQty - 1; i++)
            {
                bExist = (sDate == G_tPlanDate[i].sPlanDate);
                if (bExist)
                {
                    if (G_tPlanDate[i].sExecuteFlag == "Y")
                        return;

                    iIndex = i;
                    break;
                }
            }
            if (bExist)
            {
                
                TPlanDate[] tPlanData = new TPlanDate[g_iPlanDateQty-1];
                int iRow = 0;
                for (int i = 0; i <= g_iPlanDateQty - 1; i++)
                {
                    if (i == iIndex)
                        continue;
                    tPlanData[iRow] = G_tPlanDate[i];
                    iRow += 1;
                }
                g_iPlanDateQty -= 1;
                G_tPlanDate = tPlanData;
            }
            else
            {
                for (int i = 0; i <= g_sScheduleDate.Length - 1; i++)
                {
                    bExist = (sDate == g_sScheduleDate[i].ToString());
                    if (bExist)
                    {
                        iIndex = i;
                        break;
                    }
                }
                if (bExist)
                {
                    string[] sData = new string[g_sScheduleDate.Length - 1];
                    int iRow = 0;
                    for (int i = 0; i <= g_sScheduleDate.Length - 1; i++)
                    {
                        if (i == iIndex)
                            continue;
                        sData[iRow] = g_sScheduleDate[i];
                        iRow += 1;
                    }
                    g_sScheduleDate = sData;
                }
            }
            if (bExist)
            {
                DisplayExistData();
                DisplayPreviewResult();
            }
             */ 

        }
        
        private void tsp_Click(object sender, EventArgs e)
        {

            if (!(sender is ToolStripMenuItem))
                return;
            int iTag = (Int32)((sender as ToolStripMenuItem).Tag);
            string sDate = GetSelectedDate(iTag);
            bool bExist = false;
            int iIndex = -1;
            for (int i = 0; i <= g_iPlanDateQty - 1; i++)
            {
                if ((G_tPlanDate[i].sPlanDate == sDate) && (G_tPlanDate[i].sExecuteFlag == "Y"))
                    return;
            }
            for (int i = 0; i <= g_sScheduleDate.Length - 1; i++)
            {
                bExist = (sDate == g_sScheduleDate[i].ToString());
                if (bExist)
                {
                    iIndex = i;
                    break;
                }
            }
            /*
            if (bExist)
            {
                fChangePlanDate f = new fChangePlanDate();
                try
                {
                    f.g_sCycleName = combCycle.Text;
                    f.g_sOldDate = g_sScheduleDate[iIndex];
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        for (int i = 0; i <= g_sScheduleDate.Length - 1; i++)
                        {
                            bExist = (f.g_sNewDate == g_sScheduleDate[i].ToString());
                            if (bExist)
                            {
                                SajetCommon.Show_Message(SajetCommon.SetLanguage("Plan Date") + " : " + DecodePlanDate(f.g_sNewDate)+Environment.NewLine
                                                       + SajetCommon.SetLanguage("Date Exist"), 0);
                                return;
                            }
                        }
                        g_sScheduleDate[iIndex] = f.g_sNewDate;
                     //   DisplayResult();
                    }
                }
                finally
                {
                    f.Dispose();
                }
            }
             */ 

            //string sDay = tControlAdd[iTag].dgvData.CurrentRow.Cells["INSP_SN"].Value.ToString();

        }
        private string DecodePlanDate(string sPlanDDate)
        {
            if (sPlanDDate.Length >= 8)
                return sPlanDDate.Substring(0, 4) + "/" + sPlanDDate.Substring(4, 2) + "/" + sPlanDDate.Substring(6, 2);
            else
                return sPlanDDate;
        }
        private bool IsExist(string sPlanID,string sCycleID,string sStartDate,string sEndData)
        {
            object[][] Params = new object[4][];
            sSQL = "SELECT * FROM SAJET.SYS_MACHINE_MT_PLAN_DETAIL  "
                + " WHERE PLAN_ID =:PLAN_ID "
                + "   AND CYCLE_ID =:CYCLE_ID "
                + "   AND PLAN_DATE >=:PLAN_DATE_1 AND PLAN_DATE <=:PLAN_DATE_2 ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PLAN_ID", sPlanID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CYCLE_ID", sCycleID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PLAN_DATE_1", sStartDate };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PLAN_DATE_2", sEndData };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }
        private bool IsExecute(string sPlanID, string sCycleID, string sStartDate, string sEndData)
        {
            object[][] Params = new object[4][];
            sSQL = "SELECT PLAN_DATE,EXECUTE_FLAG FROM SAJET.SYS_MACHINE_MT_PLAN_DETAIL "
               + " WHERE PLAN_ID =:PLAN_ID "
               + "   AND CYCLE_ID =:CYCLE_ID "
                + "   AND PLAN_DATE >=:PLAN_DATE_1 AND PLAN_DATE <=:PLAN_DATE_2 ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PLAN_ID", sPlanID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CYCLE_ID", sCycleID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PLAN_DATE_1", sStartDate };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PLAN_DATE_2", sEndData };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {

            }
            if ((dsTemp.Tables[0].Rows.Count > 0) && (dsTemp.Tables[0].Rows[0]["EXECUTE_FLAG"].ToString() == "Y"))

                return true;
            else
                return false;
        }
        private void DeleteUnExecute(string sPlanID, string sCycleID, string sStartDate, string sEndDate)
        {
            object[][] Params = new object[4][];
            sSQL = "DELETE SAJET.SYS_MACHINE_MT_PLAN_DETAIL "
                + " WHERE PLAN_ID =:PLAN_ID "
                + "   AND CYCLE_ID =:CYCLE_ID "
                + "   AND PLAN_DATE >=:PLAN_DATE_1 AND PLAN_DATE <=:PLAN_DATE_2 "
                + "   AND EXECUTE_FLAG ='N' ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PLAN_ID", sPlanID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CYCLE_ID", sCycleID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PLAN_DATE_1", sStartDate };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PLAN_DATE_2", sEndDate };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

        }
        private void btnOK_Click(object sender, EventArgs e)
        {
        }
        private TPlanDate[] GetCyclePlanData(DateTime dtStart, DateTime dtEnd, string sMachineID)
        {

            string sStartDate = dtStart.ToString("yyyyMM") + "01";
            int iYear = Convert.ToInt32(dtEnd.ToString("yyyy"));
            int iMonth = Convert.ToInt32(dtEnd.ToString("MM"));
            int iLastDay = DateTime.DaysInMonth(iYear, iMonth);
            string sEndDate = dtEnd.ToString("yyyyMM") + iLastDay.ToString().PadLeft(2, '0');
            if (g_sType == "OUTPUT_QTY")
            {
                sSQL = "SELECT MACHINE_ID,WORK_DATE,OUTPUT_QTY VALUE "
                    + "  FROM SAJET.SYS_MACHINE_OUTPUT_QTY "
                    + " WHERE MACHINE_ID =:MACHINE_ID "
                    + "    AND WORK_DATE >=:WORK_DATE_1 AND WORK_DATE <=:WORK_DATE_2 "
                    + " ORDER BY WORK_DATE ";
            }
            else
            {
                sSQL = "SELECT MACHINE_ID,WORK_DATE,WORKING_TIME VALUE "
                    + "  FROM SAJET.SYS_MACHINE_WORKING_TIME "
                    + " WHERE MACHINE_ID =:MACHINE_ID "
                    + "    AND WORK_DATE >=:WORK_DATE_1 AND WORK_DATE <=:WORK_DATE_2 "
                    + " ORDER BY WORK_DATE ";
            }
            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", sMachineID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_DATE_1", sStartDate };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_DATE_2", sEndDate };



            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            TPlanDate[] tPlanDate = new TPlanDate[dsTemp.Tables[0].Rows.Count];
            g_iPlanDateQty = dsTemp.Tables[0].Rows.Count;
            dgvData.Rows.Clear();
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                dgvData.Rows.Add();
                string sWorkDate = dsTemp.Tables[0].Rows[i]["WORK_DATE"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["YEAR"].Value = sWorkDate.Substring(0, 4);
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["MONTH"].Value = sWorkDate.Substring(4, 2);
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["DAY"].Value = sWorkDate.Substring(6, 2);
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["VALUE"].Value = dsTemp.Tables[0].Rows[i]["VALUE"].ToString();
                tPlanDate[i].sMachineID = dsTemp.Tables[0].Rows[i]["MACHINE_ID"].ToString();
                tPlanDate[i].sPlanDate = dsTemp.Tables[0].Rows[i]["WORK_DATE"].ToString();
                tPlanDate[i].sValue = dsTemp.Tables[0].Rows[i]["VALUE"].ToString();
                tPlanDate[i].sExecuteFlag = "N";
                tPlanDate[i].bExist = true;
            }
            return tPlanDate;
        }
        private void DisplayCalendar(DateTime dtStart, DateTime dtEnd)
        {
            int numcells = 4;
            g_iMonthQty = GetMonthQty(dtStart, dtEnd);
            tControlAdd = new TControlData[g_iMonthQty];
            int iHeight = 135;
            int iWidth = 200;
            int iTop = 3;
            int iMonthQty = 0;
            int iMonthIndex = 0;
            string[] sWeekend = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            for (int i = 0; i <= sWeekend.Length - 1; i++)
            {
                sWeekend[i] = SajetCommon.SetLanguage(sWeekend[i]);
            }
            while (iMonthIndex <= g_iMonthQty - 1)
            {
                int iLeft = 10;
                for (int i = 1; i <= numcells; i++)
                {
                    DateTime dtTime3 = dtStart;
                    dtTime3 = dtTime3.AddMonths(iMonthQty);
                    Label lablMonth = new Label();
                    lablMonth.Text = dtTime3.ToString("yyyy/MM");
                    tControlAdd[iMonthIndex].sYear = dtTime3.ToString("yyyy");
                    tControlAdd[iMonthIndex].sMonth = dtTime3.ToString("MM");
                    TextBox editMonth = new TextBox();
                    editMonth.Top = iTop;
                    editMonth.Height = 20;
                    editMonth.Left = iLeft;
                    editMonth.Text = dtTime3.ToString("yyyy/MM");
                    editMonth.Width = 200;
                    editMonth.TextAlign = HorizontalAlignment.Center;
                    editMonth.BorderStyle = BorderStyle.FixedSingle;
                    editMonth.ReadOnly = true;
                    iMonthQty += 1;
                    editMonth.TabStop = false;
                    PanelMonth.Controls.Add(editMonth);
                    int iYear = Convert.ToInt32(dtTime3.ToString("yyyy"));
                    int iMonth = Convert.ToInt32(dtTime3.ToString("MM"));
                    DataGridView dgvData = new DataGridView();
                    dgvData.ReadOnly = true;
                    dgvData.AllowUserToAddRows = false;
                    dgvData.AllowUserToDeleteRows = false;
                    dgvData.EnableHeadersVisualStyles = false;
                    dgvData.RowHeadersVisible = false;
                    dgvData.BackgroundColor = Color.White;
                    dgvData.RowTemplate.Height = 16;
                    dgvData.Size = new System.Drawing.Size(iWidth, iHeight);
                    dgvData.AllowUserToResizeColumns = false;
                    dgvData.AllowUserToResizeRows = false;
                    dgvData.AllowUserToOrderColumns = false;
                  //  dgvData.AllowDrop = false;
                    dgvData.ClearSelection();
                    dgvData.Left = iLeft;
                    dgvData.Top = iTop + 16;
                    
                    
                    for (int k = 0; k <= sWeekend.Length - 1; k++)
                    {
                        dgvData.Columns.Add(k.ToString(), SajetCommon.SetLanguage(sWeekend[k].ToString()));
                        dgvData.Columns[dgvData.Columns.Count - 1].Width = 24;
                    }
                    dgvData.TabIndex = iMonthIndex;
                    dgvData.CellEnter += new DataGridViewCellEventHandler(dgvData_CellContentClick);

                    ContextMenuStrip cms = new ContextMenuStrip();
                    cms.Tag = iMonthIndex; 
                    cms.Opening +=new CancelEventHandler(cms_Opening);
                    ToolStripMenuItem tspCancel = new ToolStripMenuItem();
                    tspCancel.Text = SajetCommon.SetLanguage("Cancel");
                    tspCancel.Click += new EventHandler(tspCancel_Click);
                    tspCancel.Tag = iMonthIndex;

                    ToolStripMenuItem tspAppend = new ToolStripMenuItem();
                    tspAppend.Text = SajetCommon.SetLanguage("Set Start Date");
                    tspAppend.Click += new EventHandler(tspAppend_Click);
                    tspAppend.Tag = iMonthIndex;

                    ToolStripMenuItem tspDisplay = new ToolStripMenuItem();
                    tspDisplay.Text = SajetCommon.SetLanguage("Set End Date");
                    tspDisplay.Click += new EventHandler(tspDisplay_Click);
                    tspDisplay.Tag = iMonthIndex;
                    /*
                    if (combCycleID.Text == "-1")
                    {
                        cms.Items.Add(tspDisplay);
                    }
                    else
                    {
                        dgvData.CellDoubleClick += new DataGridViewCellEventHandler(dgvData_CellDoubleClick);
                        cms.Items.Add(tspAppend);
                        //  cms.Items.Add(tsp);
                        cms.Items.Add(tspCancel);
                    }
                     */ 
                     dgvData.CellDoubleClick += new DataGridViewCellEventHandler(dgvData_CellDoubleClick);
                        cms.Items.Add(tspAppend);
                        //  cms.Items.Add(tsp);
                        cms.Items.Add(tspDisplay);
                
                    dgvData.ContextMenuStrip = cms;

                    int iWeekNum = 0;
                    int iDays = 0;
                    Get_Day(iYear, iMonth, ref iWeekNum, ref iDays);
                    int iWeekTemp = iWeekNum;
                    int iTemp = 0;
                    string sTemp = "&nbsp";
                    if (iWeekNum != 7) //先將第一排補齊
                    {
                        dgvData.Rows.Add();
                        for (int k = 1; k <= 7; k++)
                        {
                            if (k <= iWeekNum)
                            {
                                dgvData.Rows[dgvData.Rows.Count - 1].Cells[k - 1].Value = "";
                            }
                            else
                            {
                                dgvData.Rows[dgvData.Rows.Count - 1].Cells[k - 1].Style.BackColor = Color.White;
                                if (iWeekNum + iTemp == 6)
                                {
                                    dgvData.Rows[dgvData.Rows.Count - 1].Cells[k - 1].Style.BackColor = Color.Silver;
                                }

                                iTemp += 1;
                                dgvData.Rows[dgvData.Rows.Count - 1].Cells[k - 1].Value = iTemp.ToString();
                            }
                        }
                    }
                    for (int k = 1; k <= 5; k++)
                    {
                        dgvData.Rows.Add();
                        for (int m = 1; m <= 7; m++)
                        {
                            iTemp += 1;
                            if (iTemp > iDays)
                            {
                                dgvData.Rows[dgvData.Rows.Count - 1].Cells[m - 1].Value = "";
                            }
                            else
                            {
                                dgvData.Rows[dgvData.Rows.Count - 1].Cells[m - 1].Style.BackColor = Color.White;
                                if (m == 1 || m == 7)
                                {
                                    dgvData.Rows[dgvData.Rows.Count - 1].Cells[m - 1].Style.BackColor = Color.Silver;
                                }
                                dgvData.Rows[dgvData.Rows.Count - 1].Cells[m - 1].Value = iTemp.ToString();
                            }
                        }
                    }
                    PanelMonth.Controls.Add(dgvData);
                    iLeft += iWidth + 5;
                    tControlAdd[iMonthIndex].dgvData = dgvData;
                    tControlAdd[iMonthIndex].dgvData.ClearSelection();
                    for (int j = 0; j <= tControlAdd[iMonthIndex].dgvData.Columns.Count - 1; j++)
                    {
                        tControlAdd[iMonthIndex].dgvData.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    tControlAdd[iMonthIndex].dgvData.TabStop = false;
                    tControlAdd[iMonthIndex].dgvData.DefaultCellStyle.SelectionBackColor = Color.Transparent;
                    tControlAdd[iMonthIndex].dgvData.DefaultCellStyle.SelectionForeColor = Color.Red;
                    iMonthIndex += 1;
                    if (iMonthIndex > g_iMonthQty - 1)
                        break;
                }
                iTop += iHeight + 20;
            }
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            PanelMonth.Controls.Clear();
            g_sScheduleDate = new string[0];
            g_iPlanDateQty = 0;
            editYear.Text = editYear.Text.Trim();
            try
            {
                Convert.ToDateTime(DateTime.Parse(editYear.Text+"/01/01").ToString("yyyy/MM/dd"));
            }
            catch
            {
                SajetCommon.Show_Message("Year Error", 0);
                editYear.Focus();
                editYear.SelectAll();
                return;
            }
            DateTime dtTime1 = Convert.ToDateTime(DateTime.Parse(editYear.Text+"/01/01").ToString("yyyy/MM/dd"));
            DateTime dtTime2 = Convert.ToDateTime(DateTime.Parse(editYear.Text+"/12/31").ToString("yyyy/MM/dd"));
    
            //DateTime dtTime1 = Convert.ToDateTime(DateTime.Parse(dateTimePicker1.Text).ToString("yyyy/MM/dd"));
            //DateTime dtTime2 = Convert.ToDateTime(DateTime.Parse(dateTimePicker2.Text).ToString("yyyy/MM/dd"));
            DisplayCalendar(dtTime1, dtTime2);
            G_tPlanDate = GetCyclePlanData(dtTime1, dtTime2, g_sMachineID);
            DisplayExistData();            
            //tabControl1.SelectedIndex = 0;
        }
        private void DisplayExistData()
        {

            ComboBox combTemp = new ComboBox();
            for (int i = 0; i <= g_iMonthQty - 1; i++)
            {
                for (int j = 0; j <= tControlAdd[i].dgvData.Rows.Count - 1; j++)
                {
                    for (int k = 0; k <= 6; k++)
                    {
                        tControlAdd[i].dgvData.Rows[j].Cells[k].ToolTipText = "";
                        string sDate1 = tControlAdd[i].dgvData.Rows[j].Cells[k].Value.ToString();
                        string sDateTemp = sDate1;
                        tControlAdd[i].dgvData.Rows[j].Cells[k].Style.BackColor = Color.White;
                        //                        if (tControlAdd[i].dgvData.Rows[j].Cells[k].Style.BackColor == Color.Red)
                        //                        {
                        tControlAdd[i].dgvData.Rows[j].Cells[k].Style.BackColor = Color.White;
                        if ((k == 0 || k == 6) && (!string.IsNullOrEmpty(sDate1)))
                            tControlAdd[i].dgvData.Rows[j].Cells[k].Style.BackColor = Color.Silver;
                        //                        }
                        if (string.IsNullOrEmpty(sDate1))
                            continue;
                        sDate1 = tControlAdd[i].sYear + tControlAdd[i].sMonth + sDate1.PadLeft(2, '0');
                        for (int m = 0; m <= g_iPlanDateQty - 1; m++)
                        {
                            if (sDate1 == G_tPlanDate[m].sPlanDate)
                            {
                                tControlAdd[i].dgvData.Rows[j].Cells[k].ToolTipText = tControlAdd[i].sYear + "/" + tControlAdd[i].sMonth + "/" + sDateTemp.PadLeft(2, '0') + " : " + G_tPlanDate[m].sValue;
                                if (G_tPlanDate[m].sExecuteFlag == "Y")
                                    tControlAdd[i].dgvData.Rows[j].Cells[k].Style.BackColor = Color.Green;
                                else
                                    tControlAdd[i].dgvData.Rows[j].Cells[k].Style.BackColor = Color.Orange;
                            }
                        }
                    }
                }
                tControlAdd[i].dgvData.ClearSelection();
            }
           // editPlanDays.Text = Convert.ToString(g_iPlanDateQty + g_sScheduleDate.Length);
        }
        private void ClearData()
        {
            g_iMonthQty = 0;
            PanelMonth.Controls.Clear();
            g_sScheduleDate = new string[0];
            g_iPlanDateQty = 0;
            
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            int iValue = 0;
            try
            {
                iValue = Convert.ToInt32(editValue.Text);
            }
            catch
            {
                
                SajetCommon.Show_Message(lablValueTitle.Text +" " + SajetCommon.SetLanguage("Error"),0);
                editValue.SelectAll();
                editValue.Focus();
                return;
            }

            DateTime dtTime1 = Convert.ToDateTime(DateTime.Parse(dateTimePicker3.Text).ToString("yyyy/MM/dd"));
            DateTime dtTime2 = Convert.ToDateTime(DateTime.Parse(dateTimePicker4.Text).ToString("yyyy/MM/dd"));
            string sStartDate = DateTime.Parse(dateTimePicker3.Text).ToString("yyyyMMdd");
            string sEndDate = DateTime.Parse(dateTimePicker4.Text).ToString("yyyyMMdd");
            if (dtTime2 < dtTime1)
            {
                SajetCommon.Show_Message("End Date Lower than Start Date", 0);
                return;
            }
            string sTable = string.Empty;
            if (g_sType == "OUTPUT_QTY")
                sTable = "SAJET.SYS_MACHINE_OUTPUT_QTY";
            if (g_sType == "WORKING_TIME")
                sTable = "SAJET.SYS_MACHINE_WORKING_TIME ";
            if (string.IsNullOrEmpty(sTable))
                return;
            sSQL = "DELETE " + sTable
                + " WHERE WORK_DATE >=:DATE1 "
                + "   AND WORK_DATE <=:DATE2 ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATE1", sStartDate };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATE2", sEndDate };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (g_sType == "OUTPUT_QTY")
            {
                sSQL = "INSERT INTO SAJET.SYS_MACHINE_OUTPUT_QTY "
                    + " (MACHINE_ID,WORK_DATE,OUTPUT_QTY,UPDATE_USERID,UPDATE_TIME ) "
                    + " VALUES (:MACHINE_ID,:WORK_DATE,:OUTPUT_QTY,:UPDATE_USERID,SYSDATE ) ";
                Params = new object[4][];
                while (true)
                {
                    if (dtTime1 > dtTime2)
                        break;
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", g_sMachineID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_DATE",  dtTime1.ToString("yyyyMMdd")};
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OUTPUT_QTY", iValue.ToString() };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID",g_sUserID };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
                    dtTime1 = dtTime1.AddDays(1);
                }
            }
            if (g_sType == "WORKING_TIME")
            {
                sSQL = "INSERT INTO SAJET.SYS_MACHINE_WORKING_TIME "
                    + " (MACHINE_ID,WORK_DATE,WORKING_TIME,UPDATE_USERID,UPDATE_TIME ) "
                    + " VALUES (:MACHINE_ID,:WORK_DATE,:WORKING_TIME,:UPDATE_USERID,SYSDATE ) ";
                Params = new object[4][];
                while (true)
                {
                    if (dtTime1 > dtTime2)
                        break;
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", g_sMachineID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_DATE", dtTime1.ToString("yyyyMMdd") };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORKING_TIME", iValue.ToString() };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    dtTime1 = dtTime1.AddDays(1);
                }
            }

          //  if (string.IsNullOrEmpty(editStartDate.Text) || string.IsNullOrEmpty(editEndDate.Text) || string.IsNullOrEmpty(editCycle.Text))
         //       return;

            //string sCycleID = SajetCommon.GetID("SAJET.SYS_MACHINE_MT_CYCLE", "CYCLE_ID", "CYCLE_NAME", editCycle.Text);
           // string sCycleID = combCycleID.Text;
          //  string sStartDate = editStartDate.Text.Substring(0, 4) + editStartDate.Text.Substring(5, 2) + "01";
           // int iYear = Convert.ToInt32(editEndDate.Text.Substring(0,4));
            //int iMonth = Convert.ToInt32(editEndDate.Text.Substring(5,2));
          //  int iLastDay = DateTime.DaysInMonth(iYear, iMonth);
           // string sEndDate = iYear.ToString()+iMonth.ToString().PadLeft(2,'0') + iLastDay.ToString().PadLeft(2, '0');
            /*
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Reset Cycle") + " : " + editCycle.Text + Environment.NewLine
                                        +SajetCommon.SetLanguage("Date")+" : "+DecodePlanDate(sStartDate) +" ~ "+DecodePlanDate(sEndDate)+Environment.NewLine
                                        +SajetCommon.SetLanguage("Continue?"),2)!=DialogResult.Yes)
                return;
            DeleteUnExecute(g_sPlanID, sCycleID, sStartDate, sEndDate);
             */ 
            btnQuery_Click(sender, e);
        }

        private void combCycle_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void cms_Opening(object sender, CancelEventArgs e)
        {
            if (!(sender is ContextMenuStrip))
                return;
            int iTag = (Int32)((sender as ContextMenuStrip).Tag);
            string sDate = GetSelectedDate(iTag);// sYear + sMonth + g_sSelectDate.PadLeft(2, '0');
            
            try
            {
                sDate = sDate.Substring(0, 4) + "/" + sDate.Substring(4, 2) + "/" + sDate.Substring(6, 2);
                Convert.ToDateTime(sDate);
            }
            catch
            {
                e.Cancel = true;
            }
            /*
            bool bExist = false;
            int iIndex = -1;
            for (int i = 0; i <= g_iPlanDateQty - 1; i++)
            {
                bExist = (sDate == G_tPlanDate[i].sPlanDate);
                if (bExist)
                {
                    break;
                }

            }
            if (!bExist)
            {
                e.Cancel = true;
            }
             */ 
            /*
            for (int i = 0; i <= (sender as ContextMenuStrip).Items.Count - 1; i++)
            {
                (sender as ContextMenuStrip).Items[i].Visible = bExist;
            }
             */ 
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DateTime dtTime1 = Convert.ToDateTime(DateTime.Parse(dateTimePicker3.Text).ToString("yyyy/MM/dd"));
            DateTime dtTime2 = Convert.ToDateTime(DateTime.Parse(dateTimePicker4.Text).ToString("yyyy/MM/dd"));
            string sStartDate = DateTime.Parse(dateTimePicker3.Text).ToString("yyyyMMdd");
            string sEndDate = DateTime.Parse(dateTimePicker4.Text).ToString("yyyyMMdd");
            if (dtTime2 < dtTime1)
            {
                SajetCommon.Show_Message("End Date Lower than Start Date", 0);
                return;
            }
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Clear Data ?")+Environment.NewLine
                                        +SajetCommon.SetLanguage("Date")+" : "+ dtTime1.ToString("yyyy/MM/dd")+ " ~ "+dtTime2.ToString("yyyy/MM/dd"),2)!=DialogResult.Yes)
                return;
                       
            string sTable = string.Empty;

            if (g_sType == "OUTPUT_QTY")
                sTable = "SAJET.SYS_MACHINE_OUTPUT_QTY";
            if (g_sType == "WORKING_TIME")
                sTable = "SAJET.SYS_MACHINE_WORKING_TIME ";
            if (string.IsNullOrEmpty(sTable))
                return;
            sSQL = "DELETE "
                  + sTable
                + " WHERE WORK_DATE >=:DATE1 "
                + "   AND WORK_DATE <=:DATE2 ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATE1", sStartDate };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATE2", sEndDate };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            btnQuery_Click(sender, e);
        }

        private void combYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            editYear.Text = combYear.Text;
            btnQuery_Click(sender, e);
        }

        private void dgvData_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Rows.Count == 0 || dgvData.CurrentRow == null || dgvData.SelectedCells.Count == 0)
            {
                return;
            }
            string sYear = dgvData.CurrentRow.Cells["YEAR"].Value.ToString();
            string sMonth = dgvData.CurrentRow.Cells["MONTH"].Value.ToString();
            string sDay = dgvData.CurrentRow.Cells["DAY"].Value.ToString();
            string sValue = dgvData.CurrentRow.Cells["VALUE"].Value.ToString();
            try
            {
                string sDate = sYear + "/" + sMonth + "/" + sDay;
                dateTimePicker3.Value = Convert.ToDateTime(sDate);
                dateTimePicker4.Value = Convert.ToDateTime(sDate);
                editValue.Text = sValue;
               // editValue.Focus();                
            }
            catch
            {
            }
        }
    }
}