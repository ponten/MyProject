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

namespace CProcess
{
    public partial class fDetailData : Form
    {
        public DataGridViewRow dataCurrentRow;
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID;
        public string g_sStageName, g_sStageID;
        DataSet dsTemp;
        string sSQL;
        bool bAppendSucess = false;

        public fDetailData()
        {
            InitializeComponent();
        }

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;
            LabStageName.Text = g_sStageName;

            combOperate.Items.Clear();
            sSQL = "Select TYPE_NAME from sajet.sys_operate_type order by type_name";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combOperate.Items.Add(dsTemp.Tables[0].Rows[i]["TYPE_NAME"].ToString());
            }

            AppendYesNoItemsToComboBox(ref combWIPERP);
            AppendYesNoItemsToComboBox(ref CbPrint);
            AppendYesNoItemsToComboBox(ref CbT4);
            AppendYesNoItemsToComboBox(ref CbT6);
            AppendYesNoItemsToComboBox(ref CbStartProcess);
            AppendYesNoItemsToComboBox(ref CbWarehouseProcess);

            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();

                editName.Text = dataCurrentRow.Cells["PROCESS_NAME"].Value.ToString();
                editDesc.Text = dataCurrentRow.Cells["PROCESS_DESC"].Value.ToString();
                editDesc2.Text = dataCurrentRow.Cells["PROCESS_DESC2"].Value.ToString();
                editCode.Text = dataCurrentRow.Cells["PROCESS_CODE"].Value.ToString();
                combOperate.SelectedIndex = combOperate.Items.IndexOf(dataCurrentRow.Cells["TYPE_NAME"].Value.ToString());

                combWIPERP.SelectedIndex = combWIPERP.Items.IndexOf(dataCurrentRow.Cells["WIP_ERP"].Value.ToString());

                sSQL = @"
SELECT
    option1,
    option2,
    option3,
    option4,
    option5,
    option6,
    option7,
    option8,
    option9,
    option10
FROM
    sajet.sys_process_option
WHERE
    process_id = :process_id
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "process_id", g_sKeyID }
                };

                var d = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

                if (d != null && d.Tables[0].Rows.Count > 0)
                {
                    CbPrint.Text = d.Tables[0].Rows[0]["OPTION1"]?.ToString();
                    CbT4.Text = d.Tables[0].Rows[0]["OPTION2"]?.ToString();
                    CbT6.Text = d.Tables[0].Rows[0]["OPTION3"]?.ToString();
                    CbStartProcess.Text = d.Tables[0].Rows[0]["OPTION4"]?.ToString();
                    CbWarehouseProcess.Text = d.Tables[0].Rows[0]["OPTION5"]?.ToString();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= TlpForm.Controls.Count - 1; i++)
            {
                if (TlpForm.Controls[i] is TextBox x)
                {
                    x.Text = x.Text.Trim();
                }
            }

            if (editName.Text == "")
            {
                string sData = LabName.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editName.Focus();
                editName.SelectAll();
                return;
            }

            if (combOperate.SelectedIndex == -1)
            {
                string sData = LabOperateType.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                combOperate.Focus();
                combOperate.SelectAll();
                return;
            }

            if (combWIPERP.SelectedIndex == -1)
            {
                string sData = LabWIPERP.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                combWIPERP.Focus();
                combWIPERP.SelectAll();
                return;
            }

            //檢查Name是否重複
            sSQL = $@"
SELECT
    *
FROM
    sajet.sys_process
WHERE
    process_name = '{editName.Text}'
";
            if (g_sUpdateType == "MODIFY")
            {
                sSQL += $@"
    AND process_id <> '{g_sKeyID}'
";
            }

            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabName.Text + " : " + editName.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editName.Focus();
                editName.SelectAll();
                return;
            }

            //Update DB
            try
            {
                if (g_sUpdateType == "APPEND")
                {
                    AppendData();
                    bAppendSucess = true;

                    string sMsg
                        = SajetCommon.SetLanguage("Data Append OK", 2)
                        + " !"
                        + Environment.NewLine
                        + SajetCommon.SetLanguage("Append Other Data", 2)
                        + " ?";

                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();

                        editName.Focus();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private void AppendData()
        {
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_PROCESS", "PROCESS_ID", 6);
            string sOperateID = GetOperateID(combOperate.Text);

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "process_id", sMaxID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "process_name", editName.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "process_desc", editDesc.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "process_desc2", editDesc2.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "process_code", editCode.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "stage_id", g_sStageID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "operate_id", sOperateID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "update_userid", fMain.g_sUserID },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "update_time", DateTime.Now },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "wip_erp", combWIPERP.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "option1", CbPrint.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "option2", CbT4.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "option3", CbT6.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "option4", CbStartProcess.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "option5", CbWarehouseProcess.Text.Trim() },
            };

            string s = @"
    INSERT INTO sajet.sys_process (
        process_id,
        process_name,
        process_desc,
        process_desc2,
        process_code,
        stage_id,
        operate_id,
        enabled,
        update_userid,
        update_time,
        wip_erp
    ) VALUES (
        :process_id,
        :process_name,
        :process_desc,
        :process_desc2,
        :process_code,
        :stage_id,
        :operate_id,
        'Y',
        :update_userid,
        :update_time,
        :wip_erp
    );

    INSERT INTO sajet.sys_ht_process (
        factory_id,
        process_id,
        process_name,
        stage_id,
        process_code,
        process_desc,
        operate_id,
        update_userid,
        update_time,
        enabled,
        process_desc2,
        wip_erp
    )
        SELECT
            factory_id,
            process_id,
            process_name,
            stage_id,
            process_code,
            process_desc,
            operate_id,
            update_userid,
            update_time,
            enabled,
            process_desc2,
            wip_erp
        FROM
            sajet.sys_process
        WHERE
            process_id = :process_id;

    INSERT INTO sajet.sys_process_option (
        factory_id,
        stage_id,
        process_id,
        option1,
        option2,
        option3,
        option4,
        option5,
        update_userid,
        update_time
    ) VALUES (
        (
            SELECT
                factory_id
            FROM
                sajet.sys_factory
            WHERE
                ROWNUM = 1
        ),
        :stage_id,
        :process_id,
        :option1,
        :option2,
        :option3,
        :option4,
        :option5,
        :update_userid,
        :update_time
    );

    INSERT INTO sajet.sys_ht_process_option (
        factory_id,
        stage_id,
        process_id,
        option1,
        option2,
        option3,
        option4,
        option5,
        option6,
        option7,
        option8,
        option9,
        option10,
        update_userid,
        update_time
    )
        SELECT
            factory_id,
            stage_id,
            process_id,
            option1,
            option2,
            option3,
            option4,
            option5,
            option6,
            option7,
            option8,
            option9,
            option10,
            update_userid,
            update_time
        FROM
            sajet.sys_process_option
        WHERE
            process_id = :process_id;
";
            if (sOperateID == "2" || sOperateID == "5" || sOperateID == "10")
            {
                string sSheetName = "";

                if (sOperateID == "5")
                {
                    sSheetName = "RC Output";
                }
                else
                {
                    sSheetName = "RC Quality";
                }

                s += @"
    INSERT INTO sajet.sys_rc_process_sheet (
        process_id,
        sheet_seq,
        sheet_name,
        sheet_phase,
        next_sheet,
        link_name,
        update_userid,
        update_time
    ) VALUES (
        :process_id,
        :sheet_seq,
        :sheet_name,
        :sheet_phase,
        :next_sheet,
        :link_name,
        :update_userid,
        sysdate
    );
";
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "process_id", sMaxID });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "sheet_seq", 0 });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "sheet_name", sSheetName });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "sheet_phase", "O" });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "next_sheet", "END" });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "link_name", "NEXT" });
            }

            s = $@"
BEGIN
{s}
END;
";
            ClientUtils.ExecuteSQL(s, p.ToArray());
        }

        private void ModifyData()
        {
            string sOperateID = GetOperateID(combOperate.Text);
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "process_code", editCode.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "process_name", editName.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "process_desc", editDesc.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "process_desc2", editDesc2.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "operate_id", sOperateID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "update_userid", fMain.g_sUserID },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "update_time", DateTime.Now },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "process_id", g_sKeyID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "stage_id", g_sStageID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "wip_erp", combWIPERP.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "option1", CbPrint.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "option2", CbT4.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "option3", CbT6.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "option4", CbStartProcess.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "option5", CbWarehouseProcess.Text.Trim() },
            };
            string s = @"
BEGIN
    UPDATE sajet.sys_process
    SET
        process_code = :process_code,
        process_name = :process_name,
        process_desc = :process_desc,
        process_desc2 = :process_desc2,
        operate_id = :operate_id,
        update_userid = :update_userid,
        update_time = :update_time,
        wip_erp = :wip_erp
    WHERE
        process_id = :process_id;

    INSERT INTO sajet.sys_ht_process (
        factory_id,
        process_id,
        process_name,
        stage_id,
        process_code,
        process_desc,
        operate_id,
        update_userid,
        update_time,
        enabled,
        process_desc2,
        wip_erp
    )
        SELECT
            factory_id,
            process_id,
            process_name,
            stage_id,
            process_code,
            process_desc,
            operate_id,
            update_userid,
            update_time,
            enabled,
            process_desc2,
            wip_erp
        FROM
            sajet.sys_process
        WHERE
            process_id = :process_id;

    INSERT INTO sajet.sys_ht_process_option (
        factory_id,
        stage_id,
        process_id,
        option1,
        option2,
        option3,
        option4,
        option5,
        option6,
        option7,
        option8,
        option9,
        option10,
        update_userid,
        update_time
    )
        SELECT
            factory_id,
            stage_id,
            process_id,
            option1,
            option2,
            option3,
            option4,
            option5,
            option6,
            option7,
            option8,
            option9,
            option10,
            update_userid,
            update_time
        FROM
            sajet.sys_process_option
        WHERE
            process_id = :process_id;

    DELETE sajet.sys_process_option
    WHERE
        process_id = :process_id;

    INSERT INTO sajet.sys_process_option (
        factory_id,
        stage_id,
        process_id,
        option1,
        option2,
        option3,
        option4,
        option5,
        update_userid,
        update_time
    ) VALUES (
        (
            SELECT
                factory_id
            FROM
                sajet.sys_factory
            WHERE
                ROWNUM = 1
        ),
        :stage_id,
        :process_id,
        :option1,
        :option2,
        :option3,
        :option4,
        :option5,
        :update_userid,
        :update_time
    );

END;
";
            ClientUtils.ExecuteSQL(s, p.ToArray());
        }

        private void ClearData()
        {
            for (int i = 0; i <= TlpForm.Controls.Count - 1; i++)
            {
                if (TlpForm.Controls[i] is TextBox x)
                {
                    x.Text = "";
                }
                else if (TlpForm.Controls[i] is ComboBox c)
                {
                    c.SelectedIndex = -1;
                }
            }
        }

        private string GetOperateID(string sTypeName)
        {
            sSQL = $@"
SELECT
    operate_id
FROM
    sajet.sys_operate_type
WHERE
    type_name = '{sTypeName}'
";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                return dsTemp.Tables[0].Rows[0]["OPERATE_ID"].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 給 ComboBox 加入 Yes/ No 選項
        /// </summary>
        /// <param name="c"></param>
        private void AppendYesNoItemsToComboBox(ref ComboBox c)
        {
            c.Items.Clear();

            c.Items.Add("Y");

            c.Items.Add("N");

            c.SelectedIndex = 1;
        }
    }
}