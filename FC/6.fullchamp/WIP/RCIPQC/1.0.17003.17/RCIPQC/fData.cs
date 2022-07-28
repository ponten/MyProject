using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;
using SajetFilter;

namespace RCIPQC
{
    public partial class fData : Form
    {
        DataGridView gvDefect;

        public DataGridViewRow dataCurrentRow;

        TextBox txtScrap;

        TextBox txtGood;

        Dictionary<string, int> slDefect;

        DataSet dsTemp;

        public string strSN;

        public string g_sUpdateType, g_sformText;

        public string g_sKeyID;

        string sSQL;

        bool bAppendSucess = false;

        public fData(DataGridView gvDefect, TextBox txtScrap, TextBox txtGood, Dictionary<string, int> slDefect, string strSN)
        {
            InitializeComponent();

            this.gvDefect = gvDefect;

            this.txtScrap = txtScrap;

            this.txtGood = txtGood;

            this.slDefect = slDefect;

            this.strSN = strSN;
        }

        private void FData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");

            panel2.BackgroundImageLayout = ImageLayout.Stretch;

            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");

            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.Text = g_sformText;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = panelControl.Controls[i].Text.Trim();
                }
            }

            if (combSN.Visible && string.IsNullOrEmpty(combSN.Text))
            {
                string sData = lablSN.Text;

                string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine + sData;

                SajetCommon.Show_Message(sMsg, 0);

                combSN.Focus();

                combSN.SelectAll();

                return;
            }

            if (editCode.Text == "")
            {
                string sData = lablDefect.Text;

                string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine + sData;

                SajetCommon.Show_Message(sMsg, 0);

                editCode.Focus();

                editCode.SelectAll();

                return;
            }

            //2015.06.02 Nancy Add 
            try
            {
                string sSQL = @"
SELECT
    DEFECT_CODE
   ,DEFECT_DESC
   ,CODE_LEVEL
FROM
    SAJET.SYS_DEFECT
WHERE
    DEFECT_CODE = :DEFECT_CODE
AND ENABLED = 'Y'
";

                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "DEFECT_CODE", editCode.Text },
                };

                DataSet ds = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

                if (ds.Tables[0].Rows.Count == 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Defect Invalid"), 0);

                    return;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);

                return;
            }

            if (string.IsNullOrEmpty(editQty.Text))
            {
                string sData = LabQty.Text;

                string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine + sData;

                SajetCommon.Show_Message(sMsg, 0);

                editQty.Focus();

                editQty.SelectAll();

                return;
            }
            else if (!decimal.TryParse(editQty.Text, out decimal d))
            {
                string sData = LabQty.Text;

                string sMsg = SajetCommon.SetLanguage("Data Error") + Environment.NewLine + sData;

                SajetCommon.Show_Message(sMsg, 0);

                editQty.Focus();

                editQty.SelectAll();

                return;
            }

            foreach (DataGridViewRow dr in gvDefect.Rows)
            {
                if (g_sUpdateType == "MODIFY" && dr.Index == gvDefect.CurrentRow.Index)
                {
                    continue;
                }

                if (gvDefect.Columns.Contains("SERIAL_NUMBER"))
                {
                    if (dr.Cells["SERIAL_NUMBER"].EditedFormattedValue.ToString() == combSN.Text && dr.Cells["DEFECT_CODE"].EditedFormattedValue.ToString() == editCode.Text)
                    {
                        string sData = lablSN.Text + ": " + combSN.Text + Environment.NewLine + lablDefect.Text + ": " + editCode.Text;

                        string sMsg = SajetCommon.SetLanguage("Data Duplicate") + Environment.NewLine + sData;

                        SajetCommon.Show_Message(sMsg, 0);

                        editCode.Focus();

                        editCode.SelectAll();

                        return;
                    }
                }
                else
                {
                    if (dr.Cells["DEFECT_CODE"].EditedFormattedValue.ToString() == editCode.Text)
                    {
                        string sData = lablDefect.Text + " : " + editCode.Text;

                        string sMsg = SajetCommon.SetLanguage("Data Duplicate") + Environment.NewLine + sData;

                        SajetCommon.Show_Message(sMsg, 0);

                        editCode.Focus();

                        editCode.SelectAll();

                        return;
                    }
                }
            }

            if (g_sUpdateType == "APPEND")
            {
                gvDefect.Rows.Add();

                if (gvDefect.Columns.Contains("SERIAL_NUMBER"))
                {
                    gvDefect.Rows[gvDefect.Rows.Count - 1].Cells["SERIAL_NUMBER"].Value = combSN.Text;

                    slDefect[combSN.Text] = slDefect[combSN.Text] + 1;
                }

                gvDefect.Rows[gvDefect.Rows.Count - 1].Cells["DEFECT_CODE"].Value = editCode.Text;

                gvDefect.Rows[gvDefect.Rows.Count - 1].Cells["QTY"].Value = editQty.Text;

                if (!combSN.Visible)
                {
                    txtScrap.Text = (decimal.Parse(txtScrap.Text) + decimal.Parse(editQty.Text)).ToString();

                    txtGood.Text = (decimal.Parse(txtGood.Text) - decimal.Parse(editQty.Text)).ToString();
                }

                string sMsg = SajetCommon.SetLanguage("Data Append OK") + " !" + Environment.NewLine + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";

                if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                {
                    ClearData();

                    editCode.Focus();

                    return;
                }

                DialogResult = DialogResult.OK;
            }
            else
            {
                decimal dScrap = decimal.Parse(txtScrap.Text) - decimal.Parse(gvDefect.Rows[gvDefect.Rows.Count - 1].Cells["QTY"].EditedFormattedValue.ToString());

                decimal dGood = decimal.Parse(txtGood.Text) + decimal.Parse(gvDefect.Rows[gvDefect.Rows.Count - 1].Cells["QTY"].EditedFormattedValue.ToString());

                gvDefect.Rows[gvDefect.Rows.Count - 1].Cells["DEFECT_CODE"].Value = editCode.Text;

                gvDefect.Rows[gvDefect.Rows.Count - 1].Cells["QTY"].Value = editQty.Text;

                if (!combSN.Visible)
                {
                    txtScrap.Text = (dScrap + decimal.Parse(editQty.Text)).ToString();

                    txtGood.Text = (dGood - decimal.Parse(editQty.Text)).ToString();
                }

                DialogResult = DialogResult.OK;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string sSQL = $@"
SELECT
    B.DEFECT_CODE
   ,B.DEFECT_DESC
   ,SAJET.SJ_DEFECT_LEVEL(B.CODE_LEVEL) CODE_LEVEL
FROM
    SAJET.SYS_RC_PROCESS_DEFECT A
   ,SAJET.SYS_DEFECT B
WHERE
    A.DEFECT_ID = B.DEFECT_ID
AND B.DEFECT_CODE Like :DEFECT_CODE || '%'
AND B.ENABLED = 'Y'
AND A.PROCESS_ID =
(
    SELECT
        PROCESS_ID
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :RC_NO
)
ORDER BY
    B.DEFECT_CODE
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_CODE", editCode.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", strSN },
            };

            using (var f = new SajetFilter.FFilter(sqlCommand: sSQL, @params: p))
            {
                f.Text = SajetCommon.SetLanguage("Defect");

                if (f.ShowDialog() == DialogResult.OK)
                {
                    editCode.Text = f.ResultSets[0]["DEFECT_CODE"].ToString();
                }
            }
        }

        private string GetID(string sTable, string sFieldID, string sFieldName, string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                return "0";
            }

            sSQL = $@"
select
    {sFieldID}
from
    {sTable}
where
    {sFieldName} = '{sValue}'
";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                return dsTemp.Tables[0].Rows[0][sFieldID].ToString();
            }
            else
            {
                return "0";
            }
        }
        private void ClearData()
        {
            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = "";
                }
                else if (panelControl.Controls[i] is ComboBox)
                {
                    ((ComboBox)panelControl.Controls[i]).SelectedIndex = -1;
                }
            }

            editCode.Text = string.Empty;

            editQty.Text = "1";

            if (combSN.Items.Count > 1)
            {
                combSN.SelectedIndex = -1;
            }
        }
    }
}