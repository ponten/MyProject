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
using MachineDown;

namespace CMachineDown
{
    public partial class fDetailData : Form
    {
        public DataGridViewRow dataCurrentRow;

        public string g_sUpdateType, g_sformText;

        public string g_sKeyID;

        public string g_sDesc, g_sTypeID;

        private readonly fMain fMainControl;

        Dictionary<string, int> dict = new Dictionary<string, int>();

        DataSet dsTemp;

        string sSQL;

        bool bAppendSucess = false;

        public fDetailData() : this(null) { }
        public fDetailData(fMain f)
        {
            InitializeComponent();

            fMainControl = f;

            LoadComboBoxItems(ref CbCountWorktime);
        }

        private void FData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");

            panel2.BackgroundImageLayout = ImageLayout.Stretch;

            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");

            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.Text = g_sformText;

            LabType.Text = g_sDesc;

            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();

                editCode.Text = dataCurrentRow.Cells["REASON_CODE"].Value.ToString();

                editDesc1.Text = dataCurrentRow.Cells["REASON_DESC"].Value.ToString();

                editDesc2.Text = dataCurrentRow.Cells["DESC2"].Value.ToString();

                CbCountWorktime.Text = SajetCommon.SetLanguage(dataCurrentRow.Cells["COUNT_WORKTIME"].Value.ToString());
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

                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;

                SajetCommon.Show_Message(sMsg, 0);

                editCode.Focus();

                editCode.SelectAll();

                return;
            }

            sSQL = @"
SELECT *
FROM
    SAJET.SYS_MACHINE_DOWN_DETAIL
WHERE
    REASON_CODE = :REASON_CODE
AND ENABLED IN ('Y', 'N')
";

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_CODE", editCode.Text }
            };

            if (g_sUpdateType == "MODIFY")
            {
                sSQL += " AND REASON_ID <> :REASON_ID";

                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_ID", g_sKeyID });
            }

            dsTemp = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabName.Text + " : " + editCode.Text;

                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;

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
                        + Environment.NewLine
                        + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";

                    if (fMainControl != null)
                    {
                        fMainControl.ShowDetailData();
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

        private void LoadComboBoxItems(ref ComboBox cbCountWorktime)
        {
            dict.Add(key: SajetCommon.SetLanguage(IsWorktimeCountEnum.Yes.ToString()), value: ((int)IsWorktimeCountEnum.Yes));
            dict.Add(key: SajetCommon.SetLanguage(IsWorktimeCountEnum.No.ToString()), value: ((int)IsWorktimeCountEnum.No));

            foreach (var kvp in dict)
            {
                cbCountWorktime.Items.Add(kvp.Key);
            }
        }

        private void AppendData()
        {
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_MACHINE_DOWN_DETAIL", "REASON_ID", 6);

            sSQL = @"
INSERT INTO
    SAJET.SYS_MACHINE_DOWN_DETAIL
(
    TYPE_ID
   ,REASON_ID
   ,REASON_CODE
   ,REASON_DESC
   ,DESC2
   ,COUNT_WORKTIME
   ,ENABLED
   ,UPDATE_USERID
   ,UPDATE_TIME
)
VALUES
(
    :TYPE_ID
   ,:REASON_ID
   ,:REASON_CODE
   ,:REASON_DESC
   ,:DESC2
   ,:COUNT_WORKTIME
   ,'Y'
   ,:UPDATE_USERID
   ,SYSDATE
)";

            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_ID", sMaxID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_CODE", editCode.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_DESC", editDesc1.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "DESC2", editDesc2.Text },
                new object[] { ParameterDirection.Input, OracleType.Number, "COUNT_WORKTIME", dict[CbCountWorktime.Text] },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPE_ID", g_sTypeID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 }
            };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            fMain.CopyToDetailHistory(sMaxID);
        }

        private void ModifyData()
        {
            sSQL = @"
UPDATE
    SAJET.SYS_MACHINE_DOWN_DETAIL
SET
    REASON_CODE = :REASON_CODE
   ,REASON_DESC = :REASON_DESC
   ,DESC2 = :DESC2
   ,COUNT_WORKTIME = :COUNT_WORKTIME
   ,UPDATE_USERID = :UPDATE_USERID
   ,UPDATE_TIME = SYSDATE
WHERE
    REASON_ID = :REASON_ID
";
            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_CODE", editCode.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_DESC", editDesc1.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "DESC2", editDesc2.Text },
                new object[] { ParameterDirection.Input, OracleType.Number, "COUNT_WORKTIME", dict[CbCountWorktime.Text] },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_ID", g_sKeyID }
            };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            fMain.CopyToDetailHistory(g_sKeyID);
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