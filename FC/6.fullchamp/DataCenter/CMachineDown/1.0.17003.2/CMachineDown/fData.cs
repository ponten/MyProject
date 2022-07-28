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

namespace CMachineDown
{
    public partial class fData : Form
    {
        public string g_sUpdateType, g_sformText;

        public string g_sKeyID;

        string sSQL;

        bool bAppendSucess = false;

        public DataGridViewRow dataCurrentRow;

        DataSet dsTemp;

        private readonly fMain fMainControl;

        public fData() : this(null) { }
        public fData(fMain f)
        {
            InitializeComponent();

            fMainControl = f;
        }

        private void FData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");

            panel2.BackgroundImageLayout = ImageLayout.Stretch;

            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");

            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.Text = g_sformText;

            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                editCode.Text = dataCurrentRow.Cells["TYPE_CODE"].Value.ToString();

                editDesc.Text = dataCurrentRow.Cells["TYPE_DESC"].Value.ToString();
            }
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

            if (editCode.Text == "")
            {
                string sData = LabName.Text;

                string sMsg = SajetCommon.SetLanguage("Data is null", 2)
                    + Environment.NewLine + sData;

                SajetCommon.Show_Message(sMsg, 0);

                editCode.Focus();

                editCode.SelectAll();

                return;
            }

            sSQL = @"
SELECT *
FROM
    SAJET.SYS_MACHINE_DOWN_TYPE
WHERE
    TYPE_CODE = :TYPE_CODE
AND ENABLED IN ('Y', 'N')
";

            var P = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPE_CODE", editCode.Text }
            };

            if (g_sUpdateType == "MODIFY")
            {
                sSQL += " AND TYPE_ID <> :TYPE_ID";

                P.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPE_ID", g_sKeyID });
            }

            dsTemp = ClientUtils.ExecuteSQL(sSQL, P.ToArray());

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabName.Text + " : " + editCode.Text;

                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2)
                    + Environment.NewLine + sData;

                SajetCommon.Show_Message(sMsg, 0);

                editCode.Focus();

                editCode.SelectAll();

                return;
            }

            //Update DB
            try
            {
                if (g_sUpdateType == "APPEND")
                {
                    AppendData();

                    bAppendSucess = true;

                    string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !"
                        + Environment.NewLine + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";

                    if (fMainControl != null)
                    {
                        fMainControl.ShowData();//新增後即時顯示新增資料在表格上
                    }

                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();

                        editCode.Focus();

                        return;
                    }

                    DialogResult = DialogResult.OK;
                }
                else if (g_sUpdateType == "MODIFY")
                {
                    ModifyData();

                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);

                return;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void AppendData()
        {
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_MACHINE_DOWN_TYPE", "TYPE_ID", 5);

            sSQL = @"
INSERT INTO
    SAJET.SYS_MACHINE_DOWN_TYPE
(
    TYPE_ID
   ,TYPE_CODE
   ,TYPE_DESC
   ,ENABLED
   ,UPDATE_USERID
   ,UPDATE_TIME
)
VALUES
(
    :TYPE_ID
   ,:TYPE_CODE
   ,:TYPE_DESC
   ,'Y'
   ,:UPDATE_USERID
   ,SYSDATE
)";
            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPE_ID", sMaxID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPE_CODE", editCode.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPE_DESC", editDesc.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 }
            };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            fMain.CopyToHistory(sMaxID);
        }

        private void ModifyData()
        {
            sSQL = @"
UPDATE
    SAJET.SYS_MACHINE_DOWN_TYPE
SET
    TYPE_CODE = :TYPE_CODE
   ,TYPE_DESC = :TYPE_DESC
   ,UPDATE_USERID = :UPDATE_USERID
   ,UPDATE_TIME = SYSDATE
WHERE
    TYPE_ID = :TYPE_ID
";

            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPE_CODE", editCode.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPE_DESC", editDesc.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPE_ID", g_sKeyID }
            };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            fMain.CopyToHistory(g_sKeyID);
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
        }
    }
}