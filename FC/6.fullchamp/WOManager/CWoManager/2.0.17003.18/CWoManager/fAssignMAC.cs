using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Reflection;
using SajetClass;
using System.IO;

namespace CWoManager
{
    public partial class fAssignMAC : Form
    {
        public string g_sFieldName;
        string sSQL;
        DataSet dsTemp;

        public fAssignMAC()
        {
            InitializeComponent();
        }

        public void ShowData()
        {
            dgvData.Rows.Clear();

            string sSQL = $@"
SELECT
    a.*
   ,b.FACTORY_CODE
   ,c.PART_NO
FROM
    sajet.g_wo_base     a
   ,sajet.sys_factory   b
   ,sajet.sys_part      c
WHERE
    a.{g_sFieldName}    = 'Y'
AND a.part_id           = c.part_id
AND a.factory_id        = b.factory_id (+)
";
            if (combFactory.SelectedIndex > 0)
            {
                sSQL += $" AND b.Factory_Code = '{combFactory.Text} ";
            }

            sSQL += @"
ORDER BY
    b.Factory_Code
   ,a.work_order
";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                dgvData.Rows.Add();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["CHECKED"].Value = "N";
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["WORK_ORDER"].Value = (dsTemp.Tables[0].Rows[i]["WORK_ORDER"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["PART_NO"].Value = (dsTemp.Tables[0].Rows[i]["PART_NO"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["FACTORY_CODE"].Value = (dsTemp.Tables[0].Rows[i]["FACTORY_CODE"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["TARGET_QTY"].Value = (dsTemp.Tables[0].Rows[i]["TARGET_QTY"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["PART_ID"].Value = (dsTemp.Tables[0].Rows[i]["PART_ID"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["VERSION"].Value = (dsTemp.Tables[0].Rows[i]["VERSION"].ToString());
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["FINISH"].Value = "N";
            }
        }

        private void fAssignMAC_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;

            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;

            combFactory.Items.Clear();
            combFactory.Items.Add("ALL");

            sSQL = @"
SELECT
    *
FROM
    sajet.sys_factory
WHERE
    enabled = 'Y'
ORDER BY
    factory_code
";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combFactory.Items.Add(dsTemp.Tables[0].Rows[i]["FACTORY_CODE"].ToString());
            }

            combFactory.SelectedIndex = 0;

            ShowData();
        }

        private void dgvData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    dgvData.Rows[e.RowIndex].Cells[0].ReadOnly = false;
                }
                else
                {
                    dgvData.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            ShowData();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dgvData.Rows.Count - 1; i++)
            {
                dgvData.Rows[i].Cells["CHECKED"].Value = "Y";
            }
        }

        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dgvData.Rows.Count - 1; i++)
            {
                dgvData.Rows[i].Cells["CHECKED"].Value = "N";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sFile = Application.StartupPath + "\\" + fMain.g_sExeName + "\\AssignMAC.dll";

            if (!File.Exists(sFile))
            {
                string sMsg
                    = SajetCommon.SetLanguage("File not exist")
                    + Environment.NewLine
                    + sFile;

                SajetCommon.Show_Message(sMsg, 0);

                return;
            }

            //動態Load DLL(AssignMAC.dll)
            Assembly mAssembly = Assembly.LoadFrom(sFile);
            MethodInfo Method = mAssembly.GetTypes()[0].GetMethod("Assign_MAC_Init");
            object objSport = Activator.CreateInstance(mAssembly.GetTypes()[0]);
            StringCollection tsParam = new StringCollection();
            StringCollection tsValue = new StringCollection();

            for (int i = 0; i <= dgvData.Rows.Count - 1; i++)
            {
                if (dgvData.Rows[i].Cells["CHECKED"].Value.ToString() == "N")
                {
                    continue;
                }

                if (dgvData.Rows[i].Cells["FINISH"].Value.ToString() == "Y")
                {
                    continue;
                }

                string sWo = dgvData.Rows[i].Cells["WORK_ORDER"].Value.ToString();
                string sTargetQty = dgvData.Rows[i].Cells["TARGET_QTY"].Value.ToString();
                string sPartID = dgvData.Rows[i].Cells["PART_ID"].Value.ToString();
                string sVersion = dgvData.Rows[i].Cells["VERSION"].Value.ToString();
                string sFactory = dgvData.Rows[i].Cells["FACTORY_CODE"].Value.ToString();

                try
                {
                    //參數    
                    tsParam.Clear();
                    tsValue.Clear();
                    tsParam.Add("WO"); tsValue.Add(sWo);
                    tsParam.Add("TARGET_QTY"); tsValue.Add(sTargetQty);
                    tsParam.Add("PART_ID"); tsValue.Add(sPartID);
                    tsParam.Add("VERSION"); tsValue.Add(sVersion);
                    tsParam.Add("FACTORY_CODE"); tsValue.Add(sFactory);
                    tsParam.Add("SHOW"); tsValue.Add("N");

                    object[] objParam = new object[2];
                    objParam[0] = tsParam;
                    objParam[1] = tsValue;

                    object objRes = Method.Invoke(objSport, objParam);
                    bool bRes = (bool)objRes;

                    if (bRes)
                    {
                        dgvData.Rows[i].Cells["FINISH"].Value = "Y";
                        dgvData.Rows[i].Cells["RES"].Value = "Y";
                        dgvData.Rows[i].Cells["MESSAGE"].Value = SajetCommon.SetLanguage("Success");
                    }
                    else
                    {
                        string sErrMsg = "Fail";
                        int iIndex = ((StringCollection)objParam[0]).IndexOf("ERRMAG");

                        if (iIndex > -1)
                        {
                            sErrMsg = ((StringCollection)objParam[1])[iIndex].ToString();
                        }

                        dgvData.Rows[i].Cells["FINISH"].Value = "N";
                        dgvData.Rows[i].Cells["RES"].Value = "N";
                        dgvData.Rows[i].Cells["MESSAGE"].Value = sErrMsg;
                    }
                }
                catch (Exception ex)
                {
                    SajetCommon.Show_Message(ex.Message, 0);
                }
            }
        }

        private void dgvData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvData.Rows[e.RowIndex].Cells["RES"].Value != null)
            {
                if (dgvData.Rows[e.RowIndex].Cells["RES"].Value.ToString() == "N")
                {
                    dgvData.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                    dgvData.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
                }
            }
        }
    }
}
