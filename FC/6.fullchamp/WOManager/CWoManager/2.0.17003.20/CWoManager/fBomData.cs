using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetFilter;
using SajetClass;

namespace CWoManager
{
    public partial class fBomData : Form
    {
        public string g_sProcessID, g_sSelectProcess;
        public string g_sItemPartID;
        public string g_sItemPartType;
        public string g_sItemSpec1;
        public bool g_sChangeGroup;

        public fBomData()
        {
            InitializeComponent();
        }

        private void fBomData_Load(object sender, EventArgs e)
        {
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;

            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;

            SajetCommon.SetLanguageControl(this);

            combProcess.Items.Clear();
            combProcess.Items.Add("");

            string sSQL = @"
SELECT
    process_Name
FROM
    sajet.sys_process
WHERE
    enabled = 'Y'
ORDER BY
    process_name
";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combProcess.Items.Add(dsTemp.Tables[0].Rows[i]["process_Name"].ToString());
            }

            combProcess.SelectedIndex = combProcess.Items.IndexOf(g_sSelectProcess);
        }

        private void editSubPart_EnabledChanged(object sender, EventArgs e)
        {
            btnSearchPart.Enabled = editSubPart.Enabled;
        }

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            string sSQL;
            DataSet DS;

            if (string.IsNullOrWhiteSpace(editSubPart.Text))
            {
                string sData = LabSubPart.Text;

                string sMsg
                    = SajetCommon.SetLanguage("Data is null")
                    + Environment.NewLine
                    + sData;

                SajetCommon.Show_Message(sMsg, 0);

                editSubPart.Focus();

                return;
            }

            if (editQty.Text.Trim() == "0" | string.IsNullOrWhiteSpace(editQty.Text))
            {
                SajetCommon.Show_Message("Qty Error", 0);

                editQty.Focus();
                editQty.SelectAll();

                return;
            }

            if (string.IsNullOrWhiteSpace(editSubPartVer.Text))
            {
                editSubPartVer.Text = "N/A";
            }

            if (string.IsNullOrWhiteSpace(combProcess.Text))
            {
                g_sProcessID = "0";
            }
            else
            {
                g_sProcessID = GET_FIELD_ID("SAJET.SYS_PROCESS", "PROCESS_NAME", "PROCESS_ID", combProcess.Text);
            }

            if (GET_PART_ID(editSubPart.Text) == "0")
            {
                SajetCommon.Show_Message("Sub Part No Error", 0);
                editSubPart.Focus();

                return;
            }

            //若加入替代料,Group不可為0
            if ((g_sChangeGroup) & (editGroup.Text == "0" | string.IsNullOrWhiteSpace(editGroup.Text)))
            {
                SajetCommon.Show_Message("Please Change Relation (Relation<>0)", 0);

                editGroup.Focus();
                editGroup.SelectAll();

                return;
            }

            //是否重複
            sSQL = $@"
SELECT
    item_part_id
FROM
    sajet.g_wo_bom
WHERE
    work_order = '{LabWorkOrder.Text}'
AND NVL(process_ID, '0') = '{g_sProcessID}'
AND item_part_id = '{g_sItemPartID}'
";

            DS = ClientUtils.ExecuteSQL(sSQL);

            if (DS.Tables[0].Rows.Count > 0)
            {
                string sData = LabSubPart.Text + " : " + editSubPart.Text;

                string sMsg
                    = SajetCommon.SetLanguage("Data Duplicate")
                    + Environment.NewLine
                    + sData;

                SajetCommon.Show_Message(sMsg, 0);

                return;
            }

            DialogResult = DialogResult.OK;
        }

        private string GET_FIELD_ID(string sTable, string sFieldName, string sFieldID, string sFieldValue)
        {
            string sSQL = $@"
SELECT
    {sFieldID} FIELD_ID
FROM
    {sTable}
WHERE
    {sFieldName} = '{sFieldValue}'
";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);

            if (DS.Tables[0].Rows.Count > 0)
            {
                return DS.Tables[0].Rows[0]["FIELD_ID"].ToString();
            }
            else
            {
                return "0";
            }
        }

        private string GET_PART_ID(string sPartNo)
        {
            g_sItemPartID = "";
            g_sItemPartType = "";
            g_sItemSpec1 = "";

            string sSQL = $@"
SELECT
    PART_ID
   ,PART_TYPE
   ,SPEC1
FROM
    SAJET.SYS_PART
WHERE
    PART_NO = '{sPartNo}'
";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (DS.Tables[0].Rows.Count > 0)
            {
                g_sItemPartID = DS.Tables[0].Rows[0]["PART_ID"].ToString();
                g_sItemPartType = DS.Tables[0].Rows[0]["PART_TYPE"].ToString();
                g_sItemSpec1 = DS.Tables[0].Rows[0]["SPEC1"].ToString();
                return g_sItemPartID;
            }
            else
                return "0";
        }

        private void btnSearchPart_Click(object sender, EventArgs e)
        {
            string sSQL = $@"
SELECT
    part_no
   ,spec1
   ,spec2
FROM
    sajet.sys_part
WHERE
    enabled = 'Y'
AND part_no Like '{editSubPart.Text}%'
ORDER BY
    part_no
";
            fFilter f = new fFilter
            {
                sSQL = sSQL
            };

            if (f.ShowDialog() == DialogResult.OK)
            {
                editSubPart.Text = f.dgvData.CurrentRow.Cells["part_no"].Value.ToString();
            }
        }
    }
}