using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using SajetClass;
using SajetTable;
using RCPart.Enums;
using RCPart.Models;
using EpSrv = RCPart.Services.EndProcessService;
using OtSrv = RCPart.Services.OtherService;

namespace RCPart
{
    public partial class fData : Form
    {
        //private DataSet m_DsTemp;
        private bool bAppendSucess = false;
        private fMain fMainControl;
        private string m_OldPartNo = string.Empty;

        public UpdateType UpdateTypeEnum;
        public string g_sformText;
        public string g_sKeyID, g_sRouteID, g_sPartID;
        public string sNow;
        public DataGridViewRow dataCurrentRow;

        public fData() : this(f: null) { }
        public fData(fMain f)
        {
            InitializeComponent();
            fMainControl = f;

            TsmiRouteDetail.Click += TsmiRouteDetail_Click;
        }

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;

            SetDefaultRuleSetToComboBox();
            SetDefaultRouteToComboBox();

            switch (UpdateTypeEnum)
            {
                case UpdateType.Modify:
                    SetValueToUiControl();
                    //用 g_sKeyID 找報工結束製程
                    TbEndProcess.Text = EpSrv.GetCurrentEndProcess(PART_ID: g_sKeyID);
                    editPart.Text = dataCurrentRow.Cells["PART_NO"].Value.ToString();
                    m_OldPartNo = editPart.Text;
                    GetChildPartNo();
                    combRoute.SelectedIndexChanged += CombRoute_SelectedIndexChanged;
                    break;
                case UpdateType.Copy:
                    SetValueToUiControl();
                    var partNo = dataCurrentRow.Cells["PART_NO"].Value.ToString();
                    var uniquePartNo = GetUniquePartNo(partNo);
                    editPart.Text = uniquePartNo;
                    m_OldPartNo = editPart.Text;
                    break;
                case UpdateType.Append:
                    editVersion.Text = "N/A";
                    combRuleSet.Text = combRuleSet.Items[combRuleSet.Items.Count - 1].ToString();
                    break;
                default:
                    //do nothing
                    break;
            }
        }

        private void fData_Shown(object sender, EventArgs e)
        {
            LVPKSpec.Focus();
        }

        private string GetUniquePartNo(string partNo)
        {
            int count = 0;
            while (!HasUniquePartNo(partNo))
            {
                int index = partNo.LastIndexOf(" - (");
                partNo = index > -1 ? partNo.Remove(index) : partNo;
                partNo = $"{partNo} - ({++count})";
            }

            return partNo;
        }

        /// <summary>
        /// 顯示已經設定的基本資料在畫面上
        /// </summary>
        private void SetValueToUiControl()
        {
            g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

            editVersion.Text = dataCurrentRow.Cells["VERSION"].Value.ToString();
            editCustPart.Text = dataCurrentRow.Cells["CUST_PART_NO"].Value.ToString();
            editSpec1.Text = dataCurrentRow.Cells["SPEC1"].Value.ToString();
            s.Text = dataCurrentRow.Cells["UPC"].Value.ToString();
            combRuleSet.SelectedIndex = combRuleSet.Items.IndexOf(dataCurrentRow.Cells["RULE_SET"].Value.ToString());
            //Route
            RestoreRouteDetail(g_sKeyID);
            combRoute.SelectedIndex = combRoute.Items.IndexOf(dataCurrentRow.Cells["ROUTE_NAME"].Value.ToString());

            editVendorPart.Text = dataCurrentRow.Cells["VENDOR_PART_NO"].Value.ToString();
            editSpec2.Text = dataCurrentRow.Cells["SPEC2"].Value.ToString();
            tbUnitCount.Text = dataCurrentRow.Cells["OPTION1"].Value.ToString();
            tbBluePrint.Text = dataCurrentRow.Cells["OPTION4"].Value.ToString();
            tbProductWeight.Text = dataCurrentRow.Cells["OPTION5"].Value.ToString();
            // Customer code
            TbCustomerCode.Text = dataCurrentRow.Cells["OPTION6"].Value.ToString();
            tbOldCode.Text = dataCurrentRow.Cells["OPTION2"].Value.ToString();

            tbForgingNo.Text = dataCurrentRow.Cells["OPTION7"].Value.ToString();
            tbForgingWeight.Text = dataCurrentRow.Cells["OPTION8"].Value.ToString();
            tbMaterial.Text = dataCurrentRow.Cells["MATERIAL_TYPE"].Value.ToString();

            // 滿棧板數量
            tbToPallet.Text = dataCurrentRow.Cells["OPTION9"].Value.ToString();

            RestorePartPkSpec(g_sKeyID);
        }

        private void SetDefaultRuleSetToComboBox()
        {
            combRuleSet.Items.Clear();
            combRuleSet.Items.Add("");
            var sql = " Select distinct Function_Name "
                   + "From SAJET.SYS_MODULE_PARAM "
                   + "Where MODULE_NAME = 'W/O RULE' "
                   + "Order By Function_Name ";
            var dataSet = ClientUtils.ExecuteSQL(sql);
            for (int i = 0; i <= dataSet.Tables[0].Rows.Count - 1; i++)
            {
                if (dataSet.Tables[0].Rows[i]["Function_Name"].ToString() != "")
                    combRuleSet.Items.Add(dataSet.Tables[0].Rows[i]["Function_Name"].ToString());
            }
        }

        private void SetDefaultRouteToComboBox()
        {
            combRoute.Items.Clear();
            combRoute.Items.Add("");

            for (int i = 0; i < dgvRoute.Rows.Count; i++)
            {
                combRoute.Items.Add(dgvRoute.Rows[i].Cells["ROUTE_NAME"].Value.ToString());
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                btnOK.Enabled = false;
                btnCancel.Enabled = false;
                Cursor = Cursors.WaitCursor;

                foreach (var control in this.Controls)
                {
                    if (control is TextBox t)
                    {
                        t.Text = t.Text.Trim();
                    }
                }

                // check all required fields have values
                if (!HavaRequiredValues())
                {
                    return;
                }

                // unique Part_No. is allowed
                if (!HasUniquePartNo())
                {
                    string sData = LabPart.Text + " : " + editPart.Text;
                    string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                    SajetCommon.Show_Message(sMsg, 0);
                    editPart.Focus();
                    editPart.SelectAll();
                    editPart.Text = m_OldPartNo;
                    return;
                }

                if (!string.IsNullOrWhiteSpace(TbChildPart.Text) && !ChildBomServices.CheckChildPartNo(partNo: TbChildPart.Text))
                {
                    string message
                        = SajetCommon.SetLanguage("Child part no does not exist")
                        + ": "
                        + TbChildPart.Text
                        ;

                    SajetCommon.Show_Message(message, 0);

                    return;
                }

                if (!string.IsNullOrWhiteSpace(TbNewChildPartNo.Text) && !ChildBomServices.CheckChildPartNo(partNo: TbNewChildPartNo.Text))
                {
                    string message
                        = SajetCommon.SetLanguage("Child part no does not exist")
                        + ": "
                        + TbNewChildPartNo.Text
                        ;

                    SajetCommon.Show_Message(message, 0);

                    return;
                }

                //Update DB
                switch (UpdateTypeEnum)
                {
                    case UpdateType.Append:
                        UpdateData();//SAJET.SYS_PART_ROUTE
                        AppendData();
                        bAppendSucess = true;
                        string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !" + Environment.NewLine + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
                        fMainControl?.ShowData();
                        if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                        {
                            foreach (var control in this.Controls)
                            {
                                if (control is TextBox t)
                                {
                                    t.Text = t.Text.Trim();
                                }
                                else if (control is ComboBox c)
                                {
                                    c.SelectedIndex = -1;
                                }
                            }

                            LVPKSpec.Items.Clear();
                            dgvRoute.Rows.Clear();

                            editPart.Focus();
                            return;
                        }
                        DialogResult = DialogResult.OK;
                        break;

                    case UpdateType.Modify:
                        ModifyData();
                        UpdateData();//載陔善善SAJET.SYS_PART_ROUTE
                        DialogResult = DialogResult.OK;
                        break;

                    case UpdateType.Copy:
                        var msg = SajetCommon.SetLanguage("Are you sure you want to make a copy?");
                        if (SajetCommon.Show_Message(msg, 2) == DialogResult.Yes)
                        {
                            UpdateData(); //SAJET.SYS_PART_ROUTE
                            AppendData();
                            bAppendSucess = true;
                            fMainControl?.ShowData();
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            return;
                        }
                        break;

                    default:
                        throw new NotSupportedException($"Not supported update type: {UpdateTypeEnum.ToString()}");
                }

                #region 更新子階料號的 BOM

                string main_part_no = editPart.Text?.Trim();

                string main_version = editVersion.Text?.Trim();

                string child_part_no = TbChildPart.Text?.Trim();

                if (!string.IsNullOrWhiteSpace(TbNewChildPartNo.Text) &&
                    TbNewChildPartNo.Text != TbChildPart.Text)
                {
                    child_part_no = TbNewChildPartNo.Text?.Trim();
                }

                string update_user_id = ClientUtils.UserPara1;

                ChildBomServices.RenewChildPartNo(main_part_no, main_version, child_part_no, update_user_id);

                #endregion
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
            }
            finally
            {
                btnOK.Enabled = true;
                btnCancel.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void BtnCustomer_Click(object sender, EventArgs e)
        {
            string s = @"
SELECT
    CUSTOMER_CODE
   ,CUSTOMER_NAME
FROM
    SAJET.SYS_CUSTOMER
";
            using (var f = new SajetFilter.fFilter(sqlCommand: s, multiSelect: false, advancedSearch: false))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    TbCustomerCode.Text = f.ResultSets.First()?["CUSTOMER_CODE"]?.ToString();
                }
                else
                {
                    TbCustomerCode.Text = "";
                }
            }
        }

        private void BtnChildPart_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (string.IsNullOrWhiteSpace(editPart.Text))
            {
                string message = SajetCommon.SetLanguage("Please determine part no");

                SajetCommon.Show_Message(message, 0);

                return;
            }

            string partNo = editPart.Text.Trim();

            string s = @"
SELECT
    part_no,
    spec1,
    spec2,
    option2,
    option4
FROM
    sajet.sys_part
WHERE
    part_no <> :part_no
    AND enabled = 'Y'
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "part_no", partNo },
            };

            using (var f = new SajetFilter.fFilter(sqlCommand: s, @params: p, multiSelect: false, advancedSearch: true, hiddenColumns: null))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    TbNewChildPartNo.Text = f.ResultSets.First()?["part_no"]?.ToString();

                    if (TbNewChildPartNo.Text != TbChildPart.Text)
                    {
                        TbNewChildPartNo.BackColor = System.Drawing.SystemColors.Control;

                        TbNewChildPartNo.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        TbNewChildPartNo.BackColor = System.Drawing.SystemColors.Control;

                        TbNewChildPartNo.ForeColor = System.Drawing.SystemColors.WindowText;
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void CombRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            string route_name = combRoute.Text;

            TbEndProcess.Text = EpSrv.GetCurrentEndProcess(PART_ID: g_sKeyID, ROUTE_NAME: route_name);
        }

        private bool HasUniquePartNo(string partNo = null)
        {
            if (partNo == null)
            {
                partNo = editPart.Text;
            }
            var sql = " Select * from " + TableDefine.gsDef_Table + " "
                      + " Where PART_NO = '" + partNo + "' ";
            if (UpdateTypeEnum == UpdateType.Modify)
            {
                sql = sql + " and " + TableDefine.gsDef_KeyField + " <> '" + g_sKeyID + "'";
            }

            var dataSet = ClientUtils.ExecuteSQL(sql);
            return dataSet.Tables[0].Rows.Count == 0;
        }

        private bool HavaRequiredValues()
        {
            if (string.IsNullOrEmpty(editPart.Text.Trim()))
            {
                string sData = LabPart.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editPart.Focus();
                editPart.SelectAll();
                return false;
            }

            if (string.IsNullOrEmpty(combRuleSet.Text.Trim()))
            {
                string sData = LabRuleSet.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                combRuleSet.Focus();
                combRuleSet.SelectAll();
                return false;
            }

            if (string.IsNullOrEmpty(combRoute.Text.Trim()))
            {
                string sData = LabRoute.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                combRoute.Focus();
                combRoute.SelectAll();
                return false;
            }

            if (string.IsNullOrEmpty(tbOldCode.Text.Trim()))
            {
                string sData = lblOldCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                tbOldCode.Focus();
                tbOldCode.SelectAll();
                return false;
            }

            if (string.IsNullOrEmpty(tbBluePrint.Text.Trim()))
            {
                string sData = lblBluePrint.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                tbBluePrint.Focus();
                tbBluePrint.SelectAll();
                return false;
            }

            if (string.IsNullOrEmpty(editVersion.Text.Trim()))
            {
                editVersion.Text = "N/A";
            }

            // 棧板用量(單位用量/OPTION1)非必填，給預設值為 0
            // 展工單時再決定 2.0.17003.13

            //by Lee@180517 單位用量必填 Quantity_Per_Unit(OPTION1) is required
            if (string.IsNullOrEmpty(tbUnitCount.Text.Trim()) ||
                Convert.ToInt32(tbUnitCount.Text.Trim()) < 1)
            {
                tbUnitCount.Text = "0";
                /*
                string sData = lblUnitCount.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData + " > 0 ";
                SajetCommon.Show_Message(sMsg, 0);
                tbUnitCount.Focus();
                tbUnitCount.SelectAll();
                return false;
                //*/
            }

            //2.0.17003.13

            return true;
        }

        //Update SAJET.SYS_PART_ROUTE
        private void UpdateData()
        {
            switch (UpdateTypeEnum)
            {
                case UpdateType.Append:
                case UpdateType.Copy:
                    AppendNewRouteData();
                    break;
                case UpdateType.Modify:
                    ModifyRouteData();
                    break;
                default:
                    break;
            }
        }

        private void ModifyRouteData()
        {
            var sql = "Select SYSDATE from dual ";
            var dataSet = ClientUtils.ExecuteSQL(sql);
            DateTime dtNow = (DateTime)dataSet.Tables[0].Rows[0]["SYSDATE"]; //轉型成日期型別
            sNow = dtNow.ToString("yyyy/MM/dd HH:mm:ss"); //定義日期格式

            for (int i = 0; i <= dgvRoute.Rows.Count - 1; i++)
            {
                string sRoute = dgvRoute.Rows[i].Cells["ROUTE_NAME"].Value.ToString();
                GetRoute(sRoute);
                sql = " SELECT * FROM SAJET.SYS_PART_ROUTE A "
                      + " ,SAJET.SYS_RC_ROUTE B "
                      + " WHERE A.PART_ID = '" + g_sKeyID + "'"
                      + " AND A.ROUTE_ID = B.ROUTE_ID"
                      + " AND B.ROUTE_NAME = '" + sRoute + "'";
                dataSet = ClientUtils.ExecuteSQL(sql);
                if (dataSet.Tables[0].Rows.Count <= 0)
                {
                    object[][] Params = new object[5][];
                    var aSql = @"INSERT INTO SAJET.SYS_PART_ROUTE (PART_ID,ROUTE_ID,UPDATE_USERID,UPDATE_TIME,ENABLED) 
                            VALUES (:PART_ID,:ROUTE_ID,:UPDATE_USERID,:UPDATE_TIME,:ENABLED)";
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sKeyID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", g_sRouteID };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", fMain.g_sUserID };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtNow };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", "Y" };
                    ClientUtils.ExecuteSQL(aSql, Params);
                }
            }
        }

        private void AppendNewRouteData()
        {
            g_sPartID = SajetCommon.GetMaxID("SAJET.SYS_PART", "PART_ID", 10);

            var sql = "Select SYSDATE from dual ";
            var dataSet = ClientUtils.ExecuteSQL(sql);
            DateTime dtNow = (DateTime)dataSet.Tables[0].Rows[0]["SYSDATE"]; //轉型成日期型別
            sNow = dtNow.ToString("yyyy/MM/dd HH:mm:ss"); //定義日期格式

            for (int i = 0; i <= dgvRoute.Rows.Count - 1; i++)
            {
                string sRoute = dgvRoute.Rows[i].Cells["ROUTE_NAME"].Value.ToString();
                GetRoute(sRoute);
                sql = " SELECT * FROM SAJET.SYS_PART_ROUTE A "
                      + " ,SAJET.SYS_RC_ROUTE B "
                      + " WHERE A.PART_ID = '" + g_sPartID + "'"
                      + " AND A.ROUTE_ID = B.ROUTE_ID"
                      + " AND B.ROUTE_NAME = '" + sRoute + "'";
                dataSet = ClientUtils.ExecuteSQL(sql);
                if (dataSet.Tables[0].Rows.Count <= 0)
                {
                    object[][] Params = new object[5][];
                    var aSql = @"INSERT INTO SAJET.SYS_PART_ROUTE (PART_ID,ROUTE_ID,UPDATE_USERID,UPDATE_TIME,ENABLED) 
                            VALUES (:PART_ID,:ROUTE_ID,:UPDATE_USERID,:UPDATE_TIME,:ENABLED)";
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", g_sRouteID };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", fMain.g_sUserID };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtNow };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", "Y" };
                    dataSet = ClientUtils.ExecuteSQL(aSql, Params);
                }
            }
        }

        private void GetRoute(string sRoute)
        {
            try
            {
                var sql = "SELECT ROUTE_ID, ROUTE_DESC FROM SAJET.SYS_RC_ROUTE WHERE ROUTE_NAME ='" + sRoute + "' ";
                var dataSet = ClientUtils.ExecuteSQL(sql);

                if (dataSet.Tables[0].Rows.Count == 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Error Route Name ", 1), 0);
                }

                g_sRouteID = dataSet.Tables[0].Rows[0]["ROUTE_ID"].ToString();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
            }
        }

        private void AppendData()
        {
            var sMaxId = SajetCommon.GetMaxID("SAJET.SYS_PART", "PART_ID", 10);
            var sRouteId = GetId("SAJET.SYS_RC_ROUTE", "ROUTE_ID", "ROUTE_NAME", combRoute.Text);

            object[][] Params = new object[20][];
            var sql = @"
Insert into SAJET.SYS_PART
(
    PART_ID
   ,PART_NO
   ,VERSION
   ,CUST_PART_NO
   ,SPEC1
   ,UPC
   ,RULE_SET
   ,ROUTE_ID
   ,VENDOR_PART_NO
   ,SPEC2
   ,OPTION1
   ,OPTION4
   ,OPTION5
   ,OPTION6
   ,OPTION2
   ,OPTION7
   ,OPTION8
   ,OPTION9
   ,MATERIAL_TYPE
   ,ENABLED
   ,UPDATE_USERID
   ,UPDATE_TIME
)
Values
(
    :PART_ID
   ,:PART_NO
   ,:VERSION
   ,:CUST_PART_NO
   ,:SPEC1
   ,:UPC
   ,:RULE_SET
   ,:ROUTE_ID
   ,:VENDOR_PART_NO
   ,:SPEC2
   ,:OPTION1
   ,:OPTION4
   ,:OPTION5
   ,:OPTION6
   ,:OPTION2
   ,:OPTION7
   ,:OPTION8
   ,:OPTION9
   ,:MATERIAL_TYPE
   ,'Y'
   ,:UPDATE_USERID
   ,SYSDATE
)";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sMaxId };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", editPart.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", editVersion.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUST_PART_NO", editCustPart.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC1", editSpec1.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPC", s.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RULE_SET", combRuleSet.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRouteId };

            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_PART_NO", editVendorPart.Text };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC2", editSpec2.Text };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION1", tbUnitCount.Text };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION4", tbBluePrint.Text };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION5", tbProductWeight.Text };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION6", TbCustomerCode.Text };
            Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION2", tbOldCode.Text };

            Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION7", tbForgingNo.Text };
            Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION8", tbForgingWeight.Text };
            Params[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION9", tbToPallet.Text };
            Params[18] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MATERIAL_TYPE", tbMaterial.Text };

            Params[19] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };

            ClientUtils.ExecuteSQL(sql, Params);

            InsertPartPkSpec(sMaxId);
            fMain.CopyToHistory(sMaxId);
            if (UpdateTypeEnum == UpdateType.Copy)
            {
                CopyParams(sMaxId);
            }
        }

        private void ModifyData()
        {
            string sRouteId = GetId("SAJET.SYS_RC_ROUTE", "ROUTE_ID", "ROUTE_NAME", combRoute.Text);

            object[][] Params = new object[20][];
            var sql = @"
Update SAJET.SYS_PART
set PART_NO = :PART_NO
   ,VERSION = :VERSION
   ,CUST_PART_NO = :CUST_PART_NO
   ,SPEC1 = :SPEC1
   ,UPC = :UPC
   ,RULE_SET = :RULE_SET
   ,ROUTE_ID = :ROUTE_ID
   ,VENDOR_PART_NO = :VENDOR_PART_NO
   ,SPEC2 = :SPEC2
   ,OPTION1 = :OPTION1
   ,OPTION4 = :OPTION4
   ,OPTION5 = :OPTION5
   ,OPTION6 = :OPTION6
   ,OPTION2= :OPTION2
   ,OPTION7 = :OPTION7
   ,OPTION8 = :OPTION8
   ,OPTION9 = :OPTION9
   ,MATERIAL_TYPE = :MATERIAL_TYPE
   ,UPDATE_USERID = :UPDATE_USERID
   ,UPDATE_TIME = SYSDATE
where PART_ID = :PART_ID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", editPart.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", editVersion.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUST_PART_NO", editCustPart.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC1", editSpec1.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPC", s.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RULE_SET", combRuleSet.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRouteId };

            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_PART_NO", editVendorPart.Text };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC2", editSpec2.Text };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION1", tbUnitCount.Text };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION4", tbBluePrint.Text };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION5", tbProductWeight.Text };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION6", TbCustomerCode.Text };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION2", tbOldCode.Text };

            Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION7", tbForgingNo.Text };
            Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION8", tbForgingWeight.Text };
            Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION9", tbToPallet.Text };
            Params[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MATERIAL_TYPE", tbMaterial.Text };

            Params[18] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[19] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", g_sKeyID };

            ClientUtils.ExecuteSQL(sql, Params);

            InsertPartPkSpec(g_sKeyID);
            fMain.CopyToHistory(g_sKeyID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private void RestorePartPkSpec(string sPartId)
        {
            string sql = " select a.Part_ID,a.PKSPEC_ID,b.PKSPEC_NAME,b.PALLET_QTY,b.CARTON_QTY,b.BOX_QTY "
                        + " from sajet.sys_part_pkspec a,sajet.sys_pkspec b "
                        + " where a.PKSPEC_ID = b.PKSPEC_ID "
                        + " and a.part_id = '" + sPartId + "' "
                        + " and b.enabled = 'Y' "
                        + " order by b.pkspec_name ";
            DataSet dspkSpec = ClientUtils.ExecuteSQL(sql);
            LVPKSpec.Items.Clear();
            for (int i = 0; i <= dspkSpec.Tables[0].Rows.Count - 1; i++)
            {
                LVPKSpec.Items.Add(dspkSpec.Tables[0].Rows[i]["PKSPEC_NAME"].ToString());
                LVPKSpec.Items[i].SubItems.Add(dspkSpec.Tables[0].Rows[i]["PALLET_QTY"].ToString());
                LVPKSpec.Items[i].SubItems.Add(dspkSpec.Tables[0].Rows[i]["CARTON_QTY"].ToString());
                LVPKSpec.Items[i].SubItems.Add(dspkSpec.Tables[0].Rows[i]["BOX_QTY"].ToString());
                LVPKSpec.Items[i].SubItems.Add(dspkSpec.Tables[0].Rows[i]["PKSPEC_ID"].ToString());
                LVPKSpec.Items[i].Name = dspkSpec.Tables[0].Rows[i]["PKSPEC_ID"].ToString();
            }
        }

        public void InsertPartSampling(string partId, string sSamplingPlan)
        {
            var sql = " Delete SAJET.SYS_QC_SAMPLING_DEFAULT "
                 + " where PART_ID = '" + partId + "' ";
            var dataSet = ClientUtils.ExecuteSQL(sql);

            if (sSamplingPlan != "")
            {
                sql = " select Sampling_ID from SAJET.SYS_QC_SAMPLING_PLAN "
                     + " where Sampling_Type = '" + sSamplingPlan + "' ";
                dataSet = ClientUtils.ExecuteSQL(sql);
                if (dataSet.Tables[0].Rows.Count == 0)
                    return;

                var sSamplingId = dataSet.Tables[0].Rows[0]["Sampling_ID"].ToString();
                sql = " Insert Into SAJET.SYS_QC_SAMPLING_DEFAULT "
                     + " (PART_ID,SAMPLING_ID,UPDATE_USERID) "
                     + " Values ('" + partId + "','" + sSamplingId + "','" + fMain.g_sUserID + "') ";
                ClientUtils.ExecuteSQL(sql);
            }
        }

        public void InsertPartPkSpec(string sPartId)
        {
            var sql = " Delete SAJET.SYS_PART_PKSPEC "
                 + " where PART_ID = '" + sPartId + "' ";
            var dataSet = ClientUtils.ExecuteSQL(sql);

            for (int i = 0; i <= LVPKSpec.Items.Count - 1; i++)
            {
                string sPkspecId = LVPKSpec.Items[i].SubItems[4].Text;

                sql = " Insert Into SAJET.SYS_PART_PKSPEC "
                     + " (PART_ID,PKSPEC_ID,UPDATE_USERID) "
                     + " Values "
                     + " ('" + sPartId + "','" + sPkspecId + "','" + fMain.g_sUserID + "') ";
                ClientUtils.ExecuteSQL(sql);
            }
        }

        private string GetId(string sTable, string sFieldId, string sFieldName, string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return "0";
            var sql = "select " + sFieldId + " from " + sTable + " "
                 + "where " + sFieldName + " = '" + sValue + "' ";
            var dataSet = ClientUtils.ExecuteSQL(sql);
            if (dataSet.Tables[0].Rows.Count > 0)
                return dataSet.Tables[0].Rows[0][sFieldId].ToString();
            else
                return "0";
        }

        private void MenuDeletePKSpec_Click(object sender, EventArgs e)
        {
            if (LVPKSpec.Items.Count == 0 || LVPKSpec.SelectedItems.Count == 0)
                return;

            LVPKSpec.SelectedItems[0].Remove();
        }

        private void MenuAppendPKSpec_Click(object sender, EventArgs e)
        {
            var msg = SajetCommon.SetLanguage("The function is not supported.");
            SajetCommon.Show_Message(msg, 3);
        }

        public void RestoreRouteDetail(string partId)
        {
            combRoute.Items.Clear();
            combRoute.Items.Add("");

            dgvRoute.Rows.Clear();
            var sql = $@"
SELECT
    A.ROUTE_ID,
    B.ROUTE_NAME,
    B.ROUTE_DESC,
    C.EMP_ID,
    C.EMP_NO,
    TO_CHAR(A.UPDATE_TIME, 'YYYY/MM/DD HH24:MI:SS') UPDATE_TIME
FROM
    SAJET.SYS_PART_ROUTE   A,
    SAJET.SYS_RC_ROUTE     B,
    SAJET.SYS_EMP          C
WHERE
    A.PART_ID = :PART_ID
    AND A.ROUTE_ID = B.ROUTE_ID (+)
    AND A.UPDATE_USERID = C.EMP_ID
ORDER BY
    B.ROUTE_NAME,
    C.EMP_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", partId },
            };

            var dataSet = ClientUtils.ExecuteSQL(sql, p.ToArray());

            for (int i = 0; i <= dataSet.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dataSet.Tables[0].Rows[i];
                dgvRoute.Rows.Add();
                dgvRoute.Rows[dgvRoute.Rows.Count - 1].Cells["ROUTE_NAME"].Value = dr["ROUTE_NAME"].ToString();
                dgvRoute.Rows[dgvRoute.Rows.Count - 1].Cells["ROUTE_DESC"].Value = dr["ROUTE_DESC"].ToString();
                dgvRoute.Rows[dgvRoute.Rows.Count - 1].Cells["EMP_NO"].Value = dr["EMP_NO"].ToString();
                dgvRoute.Rows[dgvRoute.Rows.Count - 1].Cells["UPDATE_TIME"].Value = dr["UPDATE_TIME"].ToString();
                dgvRoute.Rows[dgvRoute.Rows.Count - 1].Cells["ROUTE_ID"].Value = dr["ROUTE_ID"].ToString();
                combRoute.Items.Add(dr["ROUTE_NAME"].ToString());
            }

            dgvRoute.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        //by Lee@180517 只能輸入數字
        private void tbUnitCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b') //允許輸入 Backspace
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9')) //允許輸入0-9
                {
                    e.Handled = true;
                }
            }
        }

        private void tbToPallet_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar));
        }

        private void MenuAppendRoute_Click(object sender, EventArgs e)
        {
            fDetailRoute f = new fDetailRoute();
            try
            {
                f.UpdateTypeEnum = UpdateTypeEnum;
                f.g_sformText = UpdateTypeEnum.ToString();
                f.g_sPartID = g_sKeyID;
                f.g_sUserID = fMain.g_sUserID;
                f.g_sVersion = editVersion.Text;

                if (f.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i <= dgvRoute.Rows.Count - 1; i++)
                    {
                        if (dgvRoute.Rows[i].Cells["ROUTE_ID"].Value.ToString() == f.g_sRouteID)
                        {
                            var sData = SajetCommon.SetLanguage("Route Name") + " : " + dgvRoute.Rows[i].Cells["ROUTE_NAME"].Value + Environment.NewLine;
                            var sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                            SajetCommon.Show_Message(sMsg, 0);
                            return;
                        }
                    }

                    dgvRoute.Rows.Add();
                    dgvRoute.Rows[dgvRoute.Rows.Count - 1].Cells["ROUTE_NAME"].Value = f.g_sRouteName;
                    dgvRoute.Rows[dgvRoute.Rows.Count - 1].Cells["ROUTE_DESC"].Value = f.g_Description;
                    dgvRoute.Rows[dgvRoute.Rows.Count - 1].Cells["EMP_NO"].Value = ClientUtils.fLoginUser;
                    dgvRoute.Rows[dgvRoute.Rows.Count - 1].Cells["UPDATE_TIME"].Value = OtSrv.GetDBDateTimeNow().ToString("yyyy/MM/dd HH:mm:ss");
                    dgvRoute.Rows[dgvRoute.Rows.Count - 1].Cells["ROUTE_ID"].Value = f.g_sRouteID;
                    combRoute.Items.Add(f.g_sRouteName);
                    combRoute.Text = f.g_sRouteName;
                }
            }
            finally
            {
                f.Dispose();

                if (f.g_contiue == "Y")
                {
                    MenuAppendRoute_Click(sender, e);
                }
            }
        }

        private void MenuDeleteRoute_Click(object sender, EventArgs e)
        {
            if (dgvRoute.Rows.Count == 0 || dgvRoute.CurrentRow == null)
                return;

            try
            {
                if (UpdateTypeEnum == UpdateType.Append)
                    g_sKeyID = SajetCommon.GetMaxID("SAJET.SYS_PART", "PART_ID", 10);
                string sRouteId = dgvRoute.CurrentRow.Cells["ROUTE_ID"].Value.ToString();
                string sRouteName = dgvRoute.CurrentRow.Cells["ROUTE_NAME"].Value.ToString();
                if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Remove") + Environment.NewLine
                         + SajetCommon.SetLanguage("Route Name") + " : " + sRouteName + " ?", 2) != DialogResult.Yes)
                    return;

                if (UpdateTypeEnum == UpdateType.Append)
                {
                    combRoute.Items.Remove(sRouteName);
                    dgvRoute.Rows.Remove(dgvRoute.CurrentRow);
                }
                else
                {
                    var sql = " SELECT COUNT(*) AS COUNTS FROM SAJET.SYS_PART_ROUTE "
                         + " WHERE PART_ID = '" + g_sKeyID + "' "
                         + " AND ROUTE_ID = '" + sRouteId + "' ";
                    var dataSet = ClientUtils.ExecuteSQL(sql);

                    if (dataSet.Tables[0].Rows[0]["COUNTS"].ToString() == "1")
                    {
                        sql = " DELETE SAJET.SYS_PART_ROUTE "
                             + " WHERE PART_ID = '" + g_sKeyID + "' "
                             + " AND ROUTE_ID = '" + sRouteId + "' ";
                        ClientUtils.ExecuteSQL(sql);
                    }

                    combRoute.Items.Remove(sRouteName);
                    dgvRoute.Rows.Remove(dgvRoute.CurrentRow);
                }

                if (!(dgvRoute.Rows.Count == 0 || dgvRoute.CurrentRow == null))
                {
                    combRoute.Text = dgvRoute.CurrentRow.Cells["ROUTE_NAME"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }

        /// <summary>
        /// 檢視生產途程的細節
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmiRouteDetail_Click(object sender, EventArgs e)
        {
            if (dgvRoute.Rows.Count == 0 || dgvRoute.CurrentRow == null) return;

            string RouteId = dgvRoute.CurrentRow.Cells["ROUTE_ID"].Value.ToString();

            string s = @"
WITH ROUTE_DETAIL AS (
    SELECT
        ROWNUM IDX,
        NODE_CONTENT,
        ROUTE_ID,
        NODE_ID
    FROM
        SAJET.SYS_RC_ROUTE_DETAIL
    WHERE
        ROUTE_ID = :ROUTE_ID
    START WITH
        NODE_CONTENT = 'START'
    CONNECT BY
        PRIOR NEXT_NODE_ID = NODE_ID
), ROUTE_PROCESS_DETAIL AS (
    SELECT
        NVL(B.ROUTE_NAME, '0') ROUTE_NAME,
        B.ROUTE_DESC,
        B.ROUTE_ID,
        NVL(A.PROCESS_CODE, '0') PROCESS_CODE,
        A.PROCESS_NAME,
        A.PROCESS_ID,
        C.NODE_ID,
        C.IDX
    FROM
        SAJET.SYS_PROCESS    A,
        SAJET.SYS_RC_ROUTE   B,
        ROUTE_DETAIL         C
    WHERE
        B.ROUTE_ID = :ROUTE_ID
        AND B.ROUTE_ID = C.ROUTE_ID
        AND TO_CHAR(A.PROCESS_ID) = C.NODE_CONTENT
        AND A.ENABLED = 'Y'
        AND B.ENABLED = 'Y'
    ORDER BY
        C.IDX
), END_PROCESS AS (
    SELECT
        NODE_ID
    FROM
        SAJET.SYS_END_PROCESS
    WHERE
        PART_ID = :PART_ID
        AND ROUTE_ID = :ROUTE_ID
        AND ENABLED = 'Y'
)
SELECT
    COUNT(D.NODE_ID) END_PROCESS,
    A.ROUTE_NAME,
    A.ROUTE_DESC,
    A.ROUTE_ID,
    A.PROCESS_CODE,
    A.PROCESS_NAME,
    A.PROCESS_ID,
    A.NODE_ID
FROM
    ROUTE_PROCESS_DETAIL   A,
    END_PROCESS            D
WHERE
    A.NODE_ID = D.NODE_ID (+)
GROUP BY
    A.ROUTE_NAME,
    A.ROUTE_DESC,
    A.ROUTE_ID,
    A.PROCESS_CODE,
    A.PROCESS_NAME,
    A.PROCESS_ID,
    A.NODE_ID,
    A.IDX
ORDER BY
    A.IDX
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", RouteId },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", g_sKeyID ?? string.Empty },
            };

            var h = new List<string>
            {
                "route_name",
                "rOute_dESC",
                "Route_Id",
                "PROCESS_CODE",
                "PROCESS_ID",
                "NODE_ID",
            };

            string name = "END_PROCESS";

            bool readOnly = UpdateTypeEnum == UpdateType.Append;

            using (var f = new SajetFilter.fFilter(sqlCommand: s, @params: p, readOnly: readOnly, checkBox: true, checkBoxColumnName: name, advancedSearch: false, hiddenColumns: h))
            {
                f.Text = SajetCommon.SetLanguage(MessageEnum.RouteDetails.ToString());

                if (f.ShowDialog() == DialogResult.OK)
                {
                    var result = f.ResultSets;

                    switch (UpdateTypeEnum)
                    {
                        case UpdateType.Append:
                        case UpdateType.Copy:
                            break;
                        case UpdateType.Modify:
                            {
                                SysEndProcessModel data = new SysEndProcessModel();

                                if (result.Count > 0)
                                {
                                    data = EpSrv.GetModel(result[0]);
                                }

                                EpSrv.SaveEndProcessSetting(PART_ID: g_sKeyID, ROUTE_ID: RouteId, node: data);

                                break;
                            }
                        default:
                            break;
                    }
                }

                TbEndProcess.Text = EpSrv.GetCurrentEndProcess(PART_ID: g_sKeyID);
            }
        }

        /// <summary>
        /// 複製製程參數
        /// </summary>
        /// <param name="PART_ID">新料號 ID</param>
        private void CopyParams(string PART_ID)
        {
            string userID = ClientUtils.UserPara1;
            DateTime now = OtSrv.GetDBDateTimeNow();
            string SQL = $@"
SELECT PART_ID
      ,PROCESS_ID
      ,ITEM_ID
      ,ITEM_NAME
      ,ITEM_PHASE
      ,ITEM_TYPE
      ,ITEM_SEQ
      ,VALUE_TYPE
      ,INPUT_TYPE
      ,VALUE_DEFAULT
      ,VALUE_LIST
      ,NECESSARY
      ,CONVERT_TYPE
      ,UPDATE_USERID
      ,UPDATE_TIME
      ,ENABLED
      ,PRINT
      ,COLUMN_ITEM
      ,ROW_ITEM
      ,UNIT_ID
FROM SAJET.SYS_RC_PROCESS_PARAM_PART
WHERE PART_ID = {dataCurrentRow.Cells["PART_ID"].Value}
";
            DataSet set = ClientUtils.ExecuteSQL(SQL);
            if (set != null && set.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    string MaxItemID = SajetCommon.GetMaxID("SAJET.SYS_RC_PROCESS_PARAM_PART", "ITEM_ID", 8);
                    SQL = $@"
INSERT INTO SAJET.SYS_RC_PROCESS_PARAM_PART
(
    PART_ID
   ,PROCESS_ID
   ,ITEM_ID
   ,ITEM_NAME
   ,ITEM_PHASE
   ,ITEM_TYPE
   ,ITEM_SEQ
   ,VALUE_TYPE
   ,INPUT_TYPE
   ,VALUE_DEFAULT
   ,VALUE_LIST
   ,NECESSARY
   ,CONVERT_TYPE
   ,UPDATE_USERID
   ,UPDATE_TIME
   ,ENABLED
   ,PRINT
   ,COLUMN_ITEM
   ,ROW_ITEM
   ,UNIT_ID
)
VALUES
(
    :PART_ID
   ,:PROCESS_ID
   ,:ITEM_ID
   ,:ITEM_NAME
   ,:ITEM_PHASE
   ,:ITEM_TYPE
   ,:ITEM_SEQ
   ,:VALUE_TYPE
   ,:INPUT_TYPE
   ,:VALUE_DEFAULT
   ,:VALUE_LIST
   ,:NECESSARY
   ,:CONVERT_TYPE
   ,:UPDATE_USERID
   ,:UPDATE_TIME
   ,:ENABLED
   ,:PRINT
   ,:COLUMN_ITEM
   ,:ROW_ITEM
   ,:UNIT_ID
)";
                    object[][] Params = new object[20][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", PART_ID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", row["PROCESS_ID"].ToString() };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", MaxItemID };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", row["ITEM_NAME"].ToString() };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PHASE", row["ITEM_PHASE"].ToString() };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE", row["ITEM_TYPE"].ToString() };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_SEQ", row["ITEM_SEQ"].ToString() };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_TYPE", row["VALUE_TYPE"].ToString() };
                    Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INPUT_TYPE", row["INPUT_TYPE"].ToString() };
                    Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_DEFAULT", row["VALUE_DEFAULT"].ToString() };
                    Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_LIST", row["VALUE_LIST"].ToString() };
                    Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NECESSARY", row["NECESSARY"].ToString() };
                    Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CONVERT_TYPE", row["CONVERT_TYPE"].ToString() };
                    Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", userID };
                    Params[14] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", now };
                    Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", row["ENABLED"].ToString() };
                    Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRINT", row["PRINT"].ToString() };
                    Params[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "COLUMN_ITEM", row["COLUMN_ITEM"].ToString() };
                    Params[18] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROW_ITEM", row["ROW_ITEM"].ToString() };
                    Params[19] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UNIT_ID", row["UNIT_ID"].ToString() };
                    ClientUtils.ExecuteSQL(SQL, Params);
                }
            }
        }

        /// <summary>
        /// 取得子階料號
        /// </summary>
        private void GetChildPartNo()
        {
            var d = ChildBomServices.GetChildPartNo(partNo: editPart.Text?.Trim(), version: editVersion.Text?.Trim());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                TbChildPart.Text = d.Tables[0].Rows[0]["child_part_no"].ToString();
            }
        }
    }
}