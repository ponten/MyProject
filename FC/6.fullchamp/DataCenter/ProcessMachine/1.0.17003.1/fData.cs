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

namespace ProcessMachine
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        public string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;
        string sProcessID;

        private void fData_Load(object sender, EventArgs e)
        {
            //SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;

            if (g_sUpdateType == "APPEND")
            {
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                DataGridViewCheckBoxColumn tbc1 = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "",
                    Name = "check",
                    FalseValue = "false",
                    TrueValue = "true"
                };
                dgvMachine.Columns.Add(tbc1);
                dgvMachine.DataMember = dsTemp.Tables[0].TableName;
                dgvMachine.DataSource = dsTemp;
            }
            else
            {
                sProcessID = dataCurrentRow.Cells["PROCESS_ID"].Value.ToString();

                string s = @"
SELECT
    A.MACHINE_CODE,
    B.MACHINE_TYPE_NAME,
    A.MACHINE_DESC,
    A.MACHINE_ID,
    DECODE(C.MACHINE_ID, NULL, 'N', 'Y') STATUS
FROM
    SAJET.SYS_MACHINE        A,
    SAJET.SYS_MACHINE_TYPE   B,
    (
        SELECT
            MACHINE_ID
        FROM
            SAJET.SYS_RC_PROCESS_MACHINE
        WHERE
            PROCESS_ID = :PROCESS_ID
    ) C
WHERE
    A.MACHINE_TYPE_ID = B.MACHINE_TYPE_ID
    AND A.ENABLED = 'Y'
    AND A.MACHINE_ID = C.MACHINE_ID(+)
ORDER BY
    A.MACHINE_CODE
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID },
                };

                DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                DataGridViewCheckBoxColumn tbc1 = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "",
                    Name = "check",
                    FalseValue = "false",
                    TrueValue = "true"
                };

                dgvMachine.Columns.Add(tbc1);
                dgvMachine.DataMember = dsTemp.Tables[0].TableName;
                dgvMachine.DataSource = dsTemp;

                for (int i = 0; i < dgvMachine.Rows.Count; i++)
                {
                    if (dgvMachine.Rows[i].Cells["STATUS"].Value.ToString() == "Y")
                    {
                        dgvMachine.Rows[i].Cells["check"].Value = true;
                    }
                }

                editCode.Text = dataCurrentRow.Cells["PROCESS_NAME"].Value.ToString();
                editCode.Enabled = false;
                dgvMachine.Columns[5].Visible = false;
            }

            dgvMachine.Columns[0].Width = 40;
            dgvMachine.Columns[1].ReadOnly = true;
            dgvMachine.Columns[2].ReadOnly = true;
            dgvMachine.Columns[3].ReadOnly = true;
            dgvMachine.Columns[4].Visible = false;

            SajetCommon.SetLanguageControl(this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = panelControl.Controls[i].Text.Trim();
                }
            }

            if (string.IsNullOrWhiteSpace(editCode.Text))
            {
                string sData = LabCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }

            //Check Process
            sProcessID = GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", editCode.Text);
            if (sProcessID == "0")
            {
                SajetCommon.Show_Message("Process is err", 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }

            int iFlag = 0;
            for (int i = 0; i < dgvMachine.Rows.Count; i++)
            {
                if ((bool)dgvMachine.Rows[i].Cells["check"].EditedFormattedValue == true)
                {
                    iFlag = 1;
                }
            }
            if (iFlag == 0)
            {
                string sMsg = SajetCommon.SetLanguage("Machine is null", 2);
                SajetCommon.Show_Message(sMsg, 0);
                return;
            }

            //ÀË¬dCode¬O§_­«½Æ
            sSQL = $" SELECT * FROM SAJET.SYS_RC_PROCESS_MACHINE WHERE PROCESS_ID = '{sProcessID}' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and PROCESS_ID <> '" + sProcessID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabCode.Text + " : " + editCode.Text;
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
                    string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !" + Environment.NewLine + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
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

        private void AppendData()
        {
            try
            {
                //Add Data
                object[][] Params = new object[3][];
                sSQL = @"
INSERT INTO SAJET.SYS_RC_PROCESS_MACHINE 
(
    PROCESS_ID,
    MACHINE_ID,
    UPDATE_USERID,
    UPDATE_TIME,
    ENABLED
)
VALUES
(
    :PROCESS_ID,
    :MACHINE_ID,
    :UPDATE_USERID,
    SYSDATE,
    'Y'
)
";
                for (int i = 0; i < dgvMachine.Rows.Count; i++)
                {
                    if ((bool)dgvMachine.Rows[i].Cells["check"].EditedFormattedValue == true)
                    {
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", dgvMachine.Rows[i].Cells["MACHINE_ID"].Value.ToString() };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
                        dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    }
                }
            }
            catch (Exception e)
            {
                SajetCommon.Show_Message("Add Fail", 0);
                return;
            }

        }

        private void ModifyData()
        {
            //Delete Old Data
            sSQL = $" DELETE FROM SAJET.SYS_RC_PROCESS_MACHINE WHERE PROCESS_ID = :PROCESS_ID ";

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID },
            };

            ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            //Add Data
            object[][] Params = new object[3][];
            sSQL = @"
INSERT INTO SAJET.SYS_RC_PROCESS_MACHINE 
(
    PROCESS_ID,
    MACHINE_ID,
    UPDATE_USERID,
    UPDATE_TIME,
    ENABLED
)
VALUES
(
    :PROCESS_ID,
    :MACHINE_ID,
    :UPDATE_USERID,
    SYSDATE,
    'Y'
)
";
            for (int i = 0; i < dgvMachine.Rows.Count; i++)
            {
                if ((bool)dgvMachine.Rows[i].Cells["check"].EditedFormattedValue == true)
                {
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", dgvMachine.Rows[i].Cells["MACHINE_ID"].Value.ToString() };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
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

            for (int i = 0; i < dgvMachine.Rows.Count; i++)
            {
                dgvMachine.Rows[i].Cells["check"].Value = false;
            }
        }

        private void btnSearchPart_Click(object sender, EventArgs e)
        {
            string s = @"
SELECT
    PROCESS_ID,
    PROCESS_NAME
FROM
    SAJET.SYS_PROCESS
WHERE
    ENABLED = 'Y'
ORDER BY
    PROCESS_NAME
";
            var h = new List<string>
            {
                "PROCESS_ID",
            };

            using (var f = new SajetFilter.FFilter(sqlCommand: s, hiddenColumns: h))
            {
                f.Text = SajetCommon.SetLanguage("Process Name");

                if (f.ShowDialog() == DialogResult.OK)
                {
                    var result = f.ResultSets[0];

                    sProcessID = result["PROCESS_ID"].ToString();

                    editCode.Text = result["PROCESS_NAME"].ToString();
                }
            }
        }

        private void editCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            sProcessID = GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", editCode.Text.Trim());
            if (sProcessID == "0")
            {
                SajetCommon.Show_Message("Process is err", 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }
        }

        private string GetID(string sTable, string sFieldID, string sFieldName, string sValue)
        {
            if (string.IsNullOrEmpty(sValue)) return "0";

            string s = $@"
SELECT
    {sFieldID}
FROM
    {sTable}
WHERE
    TRIM({sFieldName}) = TRIM(:VALUE)
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE", sValue },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d.Tables[0].Rows.Count > 0)
                return d.Tables[0].Rows[0][sFieldID].ToString();
            else
                return "0";
        }
    }
}