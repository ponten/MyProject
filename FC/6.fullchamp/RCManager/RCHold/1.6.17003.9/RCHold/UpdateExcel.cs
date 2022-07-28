using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace ExportExcel
{
    class UpdateExcel
    {
        //string Path;
        //Excel.Application excelApp;
        //Excel.Workbook excelDoc;

        //Path = System.Environment.CurrentDirectory;

        public static void Export(DataSet tHold, string hold_no)
        {
            if (tHold.Tables[0].Rows.Count == 0)
                return;
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.DefaultExt = "xls";
            //sfd.Filter = "All File(*.xls)|*.xls";
            //sfd.FileName = hold_no;

            //if (sfd.ShowDialog() != DialogResult.OK)
            //    return;

            string sFileName = System.Windows.Forms.Application.StartupPath + @"\RCManager\RC_HOLD" + @"\" + hold_no + ".xlsx";
            //string sFileName = System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + ClientUtils.fCurrentProject + Path.DirectorySeparatorChar + @"RC_HOLD" + @"\" + hold_no + ".xlsx";
            if (string.IsNullOrEmpty(sFileName))
                return;

            System.IO.File.Copy(System.Windows.Forms.Application.StartupPath + @"\RCManager\RC_HOLD" + @"\RC_Hold.xlsx", sFileName, true);
            //System.IO.File.Copy(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + ClientUtils.fCurrentProject + Path.DirectorySeparatorChar + @"RC_HOLD" + @"\RC_Hold.xlsx", sFileName, true);

            Excel.Application excelApp = new Excel.Application();
            Excel.Worksheet excelWs;

            Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(sFileName,
             Type.Missing, Type.Missing, Type.Missing, Type.Missing,
             Type.Missing, Type.Missing, Type.Missing, Type.Missing,
             Type.Missing, Type.Missing, Type.Missing, Type.Missing,
             Type.Missing, Type.Missing);

            //取得第一個sheet
            excelWs = (Excel.Worksheet)excelWorkbook.Worksheets.get_Item(1);


            ((Excel.Worksheet)excelWs).Cells[3, 3] = hold_no;
            ((Excel.Worksheet)excelWs).Cells[3, 5] = DateTime.Now.ToString();

            DataSet ds = new DataSet();
            // 2016.4.20 By Jason
            //ds = ClientUtils.ExecuteSQL("select hold_userid,hold_time,hold_desc from sajet.g_rc_hold_status where hold_no = '"+ hold_no+"'");
            ds = ClientUtils.ExecuteSQL("select b.emp_name,a.hold_time,a.hold_desc from sajet.g_rc_hold_status a,sajet.sys_emp b where a.hold_userid = b.emp_id and a.hold_no = '" + hold_no + "'");
            // 2016.4.20 End

            ((Excel.Worksheet)excelWs).Cells[4, 3] = ds.Tables[0].Rows[0][0].ToString();
            ((Excel.Worksheet)excelWs).Cells[4, 5] = ds.Tables[0].Rows[0][1].ToString();
            ((Excel.Worksheet)excelWs).Cells[6, 3] = ds.Tables[0].Rows[0][2].ToString();

            ds = ClientUtils.ExecuteSQL("select a.defect_type_desc from sajet.sys_defect_type a,sajet.g_rc_hold_status b where a.defect_type_id = b.defect_type_id and b.hold_no = '" + hold_no + "'");
            ((Excel.Worksheet)excelWs).Cells[5, 3] = ds.Tables[0].Rows[0][0].ToString();

            int sheetRow = 8;
            //int cn = 1;
            if (tHold.Tables[0].Rows.Count > 0 && tHold != null)
            {
                for (int i = 0; i < tHold.Tables[0].Rows.Count; i++)
                {
                    ((Excel.Worksheet)excelWs).Cells[sheetRow, 2] = tHold.Tables[0].Rows[i][0].ToString();
                    ((Excel.Worksheet)excelWs).Cells[sheetRow, 4] = tHold.Tables[0].Rows[i][1].ToString();
                    ((Excel.Worksheet)excelWs).Cells[sheetRow, 5] = tHold.Tables[0].Rows[i][2].ToString();

                    sheetRow++;
                }
            }

            //ClearCom(excelWs);
            excelWorkbook.Close(true, Type.Missing, Type.Missing);

            // ClearCom(excelWorkbook);
            excelApp.Workbooks.Close();
            excelApp.Quit();
            //ClearCom(excelApp);
        }
    }
}

