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
using SajetFilter;

namespace CMachine
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
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;

            editVendor.Tag = "";

            cmbLoc.Items.Clear();
            sSQL = "Select distinct machine_loc "
                 + "From SAJET.SYS_MACHINE "
                 + "Where ENABLED = 'Y' "
                 + "AND MACHINE_LOC is not null "
                 + "Order By machine_loc ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                cmbLoc.Items.Add(dsTemp.Tables[0].Rows[i]["machine_loc"].ToString());
            }
           
            sSQL = "Select machine_type_name,machine_type_id "
                    + "From SAJET.SYS_machine_type "
                    + "Where ENABLED = 'Y' "
                    + "union "
                    + "select null,0 "
                    + "from dual "
                    + "Order By machine_type_id,machine_type_name ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            cmbType.DataSource = dsTemp.Tables[0];
            cmbType.DisplayMember = dsTemp.Tables[0].Columns[0].ColumnName;
            cmbType.ValueMember = dsTemp.Tables[0].Columns[1].ColumnName;
            cmbType.SelectedIndex = 0;

            CreateOptionControl();

            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                editCode.Text = dataCurrentRow.Cells["MACHINE_CODE"].Value.ToString();
                editDesc.Text = dataCurrentRow.Cells["MACHINE_DESC"].Value.ToString();
                cmbLoc.Text = dataCurrentRow.Cells["MACHINE_LOC"].Value.ToString();
                cmbType.Text = dataCurrentRow.Cells["MACHINE_TYPE_NAME"].Value.ToString();
                chkUtilization.Checked = dataCurrentRow.Cells["UTILIZATION_FLAG"].Value.ToString() == "Y" ? true : false;
                editVendor.Text = dataCurrentRow.Cells["VENDOR_NAME"].Value.ToString();
                editVendor.Tag = dataCurrentRow.Cells["VENDOR_ID"].Value.ToString();
                if (dataCurrentRow.Cells["AUTO_RUN"].Value.ToString() == "Y")
                {
                    cmbStatus.Text = "是";
                }
                else
                {
                    cmbStatus.Text = "否";
                }

                string sArrivalDate = dataCurrentRow.Cells["ARRIVAL_TIME"].Value.ToString();
                if (sArrivalDate == "")
                    dtArrivalDate.Checked = false;
                else
                {
                    dtArrivalDate.Checked = true;
                    dtArrivalDate.Value = DateTime.Parse(sArrivalDate);
                }

                string sWarrantyDate = dataCurrentRow.Cells["WARRANTY_TIME"].Value.ToString();
                if (sWarrantyDate == "")
                    dtWarrantyDate.Checked = false;
                else
                {
                    dtWarrantyDate.Checked = true;
                    dtWarrantyDate.Value = DateTime.Parse(sWarrantyDate);
                }

                if (tControlAdd != null)
                {
                    for (int i = 0; i <= tControlAdd.Length - 1; i++)
                    {
                        string sSQL_NAME = tControlAdd[i].sSQLNAME;
                        string sSQL_TYPE = tControlAdd[i].sSQLTYPE;
                        string sSQL_VALUE = dataCurrentRow.Cells[sSQL_NAME].Value.ToString(); ;
                        if (sSQL_TYPE == "L")
                            tControlAdd[i].combControl.SelectedIndex = tControlAdd[i].combControl.Items.IndexOf(sSQL_VALUE);
                        else if (sSQL_TYPE == "S")
                        {
                            int iSelectIndex = tControlAdd[i].combControl.Items.IndexOf(sSQL_VALUE);
                            if (iSelectIndex == -1)
                                iSelectIndex = tControlAdd[i].combControlID.Items.IndexOf(sSQL_VALUE);
                            tControlAdd[i].combControlID.SelectedIndex = iSelectIndex;
                            tControlAdd[i].combControl.SelectedIndex = iSelectIndex;
                        }
                        else
                            tControlAdd[i].editControl.Text = sSQL_VALUE;
                    }
                }           
            }
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
            //if (cmbLoc.SelectedValue == null)
            //    cmbLoc.SelectedIndex = 0;
            if (cmbType.SelectedValue == null)
                cmbType.SelectedIndex = 0;

            if (editCode.Text == "")
            {
                string sData = LabCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }
            //檢查Code是否重複
            sSQL = " Select * from SAJET.SYS_MACHINE "
                 + " Where MACHINE_CODE = '" + editCode.Text + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and machine_id <> '" + g_sKeyID + "'";
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

            if (tControlAdd != null)
            {
                for (int i = 0; i <= tControlAdd.Length - 1; i++)
                {
                    string sSQL_NAME = tControlAdd[i].sSQLNAME;
                    string sSQL_TYPE = tControlAdd[i].sSQLTYPE;
                    if (sSQL_TYPE == "L")
                    {
                        tControlAdd[i].sValue = tControlAdd[i].combControl.Text;
                    }
                    else if (sSQL_TYPE == "S")
                    {
                        if (tControlAdd[i].combControl.SelectedIndex == -1)
                            tControlAdd[i].sValue = "";
                        else
                            tControlAdd[i].sValue = tControlAdd[i].combControlID.Items[tControlAdd[i].combControl.SelectedIndex].ToString();
                    }
                    else
                        tControlAdd[i].sValue = tControlAdd[i].editControl.Text;
                }
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
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_MACHINE", "MACHINE_ID", 8);

            string sOptionField = "";
            string sOptionParam = "";
            if (tControlAdd != null)
            {
                for (int i = 0; i <= tControlAdd.Length - 1; i++)
                {
                    string sSQL_NAME = tControlAdd[i].sSQLNAME;
                    sOptionField = sOptionField + "," + sSQL_NAME;
                    sOptionParam = sOptionParam + ",:" + sSQL_NAME;
                }
            }

            int iLength = 11;
            if (tControlAdd != null)
                iLength = iLength + tControlAdd.Length;
            object[][] Params = new object[iLength][];            

            sSQL = " Insert into SAJET.SYS_MACHINE "
                 + " (MACHINE_ID,MACHINE_CODE,MACHINE_DESC,MACHINE_LOC,MACHINE_TYPE_ID,UTILIZATION_FLAG "
                 + " ,ENABLED,UPDATE_USERID,UPDATE_TIME,VENDOR_ID,ARRIVAL_TIME,WARRANTY_TIME,AUTO_RUN "
                 + sOptionField
                 + " ) "
                 + " Values "
                 + " (:MACHINE_ID,:MACHINE_CODE,:MACHINE_DESC,:MACHINE_LOC,:MACHINE_TYPE_ID,:UTILIZATION_FLAG "
                 + " ,'Y',:UPDATE_USERID,SYSDATE,:VENDOR_ID,:ARRIVAL_TIME,:WARRANTY_TIME,:AUTO_RUN "
                 + sOptionParam
                 + " ) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_CODE", editCode.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_DESC", editDesc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_LOC", cmbLoc.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_TYPE_ID", cmbType.SelectedValue.ToString() };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UTILIZATION_FLAG", chkUtilization.Checked == true ? "Y" : "N" };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", editVendor.Tag };
            if (cmbStatus.Text == "是")
            {
                Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "AUTO_RUN", "Y" };
            }
            else
            {
                Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "AUTO_RUN", "N" };
            }
            if (dtArrivalDate.Checked)
                Params[9] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ARRIVAL_TIME", dtArrivalDate.Value.Date };
            else
                Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ARRIVAL_TIME", "" };
            if (dtWarrantyDate.Checked)
                Params[10] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WARRANTY_TIME", dtWarrantyDate.Value.Date };
            else
                Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WARRANTY_TIME", "" };

            if (tControlAdd != null)
            {
                iLength = iLength - tControlAdd.Length;
                for (int i = 0; i <= tControlAdd.Length - 1; i++)
                {
                    string sSQL_NAME = tControlAdd[i].sSQLNAME;
                    string sSQL_Value = tControlAdd[i].sValue;
                    Params[iLength + i] = new object[] { ParameterDirection.Input, OracleType.VarChar, sSQL_NAME, sSQL_Value };
                }
            }            
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            fMain.CopyToHistory(sMaxID);

            string sStatusID = "";
            sSQL = "SELECT STATUS_ID FROM SAJET.SYS_MACHINE_STATUS WHERE DEFAULT_STATUS = 'Y'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                sStatusID = dsTemp.Tables[0].Rows[0]["STATUS_ID"].ToString();

            object[][] statusParams = new object[3][];
            sSQL = " Insert into SAJET.G_MACHINE_STATUS "
                 + " (MACHINE_ID,CURRENT_STATUS_ID,STATUS_UPDATE_TIME,DOWN_STATUS,STATUS_NOTE,UPDATE_USERID) "
                 + " VALUES "
                 + " (:MACHINE_ID,:CURRENT_STATUS_ID,SYSDATE,0,'init',:UPDATE_USERID) ";

            statusParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", sMaxID };
            statusParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CURRENT_STATUS_ID", sStatusID };
            statusParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            ClientUtils.ExecuteSQL(sSQL, statusParams);
        }
        private void ModifyData()
        {
            string sOptionField = "";
            if (tControlAdd != null)
            {
                for (int i = 0; i <= tControlAdd.Length - 1; i++)
                {
                    string sSQL_NAME = tControlAdd[i].sSQLNAME;
                    sOptionField = sOptionField + "," + sSQL_NAME + "=:" + sSQL_NAME;
                }
            }
            int iLength = 11;
            if (tControlAdd != null)
                iLength = iLength + tControlAdd.Length;

            object[][] Params = new object[iLength][];               
            sSQL = " Update SAJET.SYS_MACHINE "
                 + " set MACHINE_CODE = :MACHINE_CODE "
                 + "    ,MACHINE_DESC = :MACHINE_DESC "
                 + "    ,MACHINE_LOC = :MACHINE_LOC "
                 + "    ,MACHINE_TYPE_ID = :MACHINE_TYPE_ID "
                 + "    ,UTILIZATION_FLAG = :UTILIZATION_FLAG "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + "    ,VENDOR_ID = :VENDOR_ID "
                 + "    ,ARRIVAL_TIME = :ARRIVAL_TIME "
                 + "    ,WARRANTY_TIME = :WARRANTY_TIME "
                 + "    ,AUTO_RUN = :AUTO_RUN "
                 + sOptionField
                 + " where MACHINE_ID = :MACHINE_ID ";

            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_CODE", editCode.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_DESC", editDesc.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_LOC", cmbLoc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_TYPE_ID", cmbType.SelectedValue.ToString() };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UTILIZATION_FLAG", chkUtilization.Checked == true ? "Y" : "N" };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", g_sKeyID };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", editVendor.Tag };
            if (cmbStatus.Text == "是")
            {
                Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "AUTO_RUN", "Y" };
            }
            else
            {
                Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "AUTO_RUN", "N" };
            }
            if (dtArrivalDate.Checked)
                Params[9] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ARRIVAL_TIME", dtArrivalDate.Value.Date };
            else
                Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ARRIVAL_TIME", "" };
            if (dtWarrantyDate.Checked)
                Params[10] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WARRANTY_TIME", dtWarrantyDate.Value.Date };
            else
                Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WARRANTY_TIME", "" };

            if (tControlAdd != null)
            {
                iLength = iLength - tControlAdd.Length;
                for (int i = 0; i <= tControlAdd.Length - 1; i++)
                {
                    string sSQL_NAME = tControlAdd[i].sSQLNAME;
                    string sSQL_Value = tControlAdd[i].sValue;
                    Params[iLength + i] = new object[] { ParameterDirection.Input, OracleType.VarChar, sSQL_NAME, sSQL_Value };
                }
            }   
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(g_sKeyID);
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
                    panelControl.Controls[i].Tag = "";
                }
                else if (panelControl.Controls[i] is ComboBox)
                {
                    ((ComboBox)panelControl.Controls[i]).SelectedIndex = -1;
                }
                else if (panelControl.Controls[i] is CheckBox)
                {
                    ((CheckBox)panelControl.Controls[i]).Checked = false;
                }
                else if (panelControl.Controls[i] is DateTimePicker)
                {
                    ((DateTimePicker)panelControl.Controls[i]).Checked = false;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sSQL = " select vendor_id, vendor_code, vendor_name, vendor_desc "
                 + " from sajet.sys_vendor "
                 + " where enabled = 'Y' "
                 + " order by vendor_code ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;            
            if (f.ShowDialog() == DialogResult.OK)
            {
                editVendor.Text = f.dgvData.CurrentRow.Cells["vendor_name"].Value.ToString();
                editVendor.Tag = f.dgvData.CurrentRow.Cells["vendor_id"].Value.ToString();
            }
        }

        //=======Create Component======================
        public struct TControlData
        {
            public string sDisPlayName;
            public string sSQLNAME;
            public string sSQLTYPE;
            public string sValue;
            public Label LabControl;
            public TextBox editControl;
            public ComboBox combControl;
            public ComboBox combControlID;
        }
        public TControlData[] tControlAdd;

        private void CreateOptionControl()
        {
            //動態建立元件
            int iTopLabel = 280;//第一個OPTION Label起始位置
            int iTopControl = 280;//第一個OPTION元件起始位置
            int iRowHeight = 29; //元件相隔的高度   
            int iLeftLabel = LabCode.Left;
            int iLeftEdit = editCode.Left;
            int iWidth = editCode.Width;
            int iHeight = editCode.Height;
            Font Ffont = LabCode.Font;

            int iRowNo;
            string sSQL_NAME = "";
            string sDisplay_Name = "";
            string sSQL = " select a.*,to_number(substr(sql_name,9,2)) option_no "
                        + " from SAJET.SYS_SQL a "
                        + " Where substr(a.SQL_NAME,1,8) ='M_OPTION' "
                        + " Order By option_no ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
                return;
            
            tControlAdd = new TControlData[dsTemp.Tables[0].Rows.Count];
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                sSQL_NAME = dsTemp.Tables[0].Rows[i]["SQL_NAME"].ToString();
                sDisplay_Name = dsTemp.Tables[0].Rows[i]["DISPLAY_NAME"].ToString();
                iRowNo = i;
                //Label =============================               
                Label labelTemp = new Label();
                labelTemp.Left = iLeftLabel;
                labelTemp.Top = iTopLabel + iRowNo * iRowHeight;
                labelTemp.Name = "LabOption" + Convert.ToString(i + 1);
                labelTemp.Size = new System.Drawing.Size(35, iHeight);
                labelTemp.AutoSize = true;
                labelTemp.Text = SajetCommon.SetLanguage(sDisplay_Name, 1);
                labelTemp.Font = Ffont;
                panelControl.Controls.Add(labelTemp);
                tControlAdd[i].LabControl = labelTemp;

                //Combobox========================================
                if (dsTemp.Tables[0].Rows[i]["SQL_TYPE"].ToString() == "L" || dsTemp.Tables[0].Rows[i]["SQL_TYPE"].ToString() == "S")
                {
                    ComboBox combTemp = new ComboBox();
                    combTemp.Left = iLeftEdit;
                    combTemp.Top = iTopControl + iRowNo * iRowHeight;
                    combTemp.Name = "combOption" + Convert.ToString(i + 1);
                    combTemp.Size = new System.Drawing.Size(iWidth, iHeight);
                    combTemp.Text = "";
                    combTemp.DropDownStyle = ComboBoxStyle.DropDownList;

                    //加入Combobox的下拉選項
                    string sSQLValue = dsTemp.Tables[0].Rows[i]["SQL_VALUE"].ToString();
                    //固定值
                    if (dsTemp.Tables[0].Rows[i]["SQL_TYPE"].ToString() == "L")
                    {
                        string[] split = sSQLValue.Split(new Char[] { ',' });
                        for (int j = 0; j <= split.Length - 1; j++)
                        {
                            combTemp.Items.Add(split[j].ToString().Trim());
                        }
                    }
                    else //由SQL找出(第一個欄位是ID(填到DB),第二個是NAME(用來顯示))
                    {
                        ComboBox combTempID = new ComboBox();
                        combTempID.Visible = false;
                        combTempID.Left = iLeftEdit;
                        combTempID.Top = iTopControl + iRowNo * iRowHeight;
                        combTempID.Name = "combOptionID" + Convert.ToString(i + 1);
                        combTempID.Size = new System.Drawing.Size(iWidth, iHeight);
                        combTempID.Text = "";

                        DataSet dsTempSQL = ClientUtils.ExecuteSQL(sSQLValue);
                        for (int j = 0; j <= dsTempSQL.Tables[0].Rows.Count - 1; j++)
                        {
                            combTempID.Items.Add(dsTempSQL.Tables[0].Rows[j][0].ToString()); //ID
                            combTemp.Items.Add(dsTempSQL.Tables[0].Rows[j][1].ToString());   //NAME                            
                        }
                        tControlAdd[i].combControlID = combTempID;
                    }
                    panelControl.Controls.Add(combTemp);
                    tControlAdd[i].combControl = combTemp;
                }
                //TextBox=======================================================
                else
                {
                    TextBox editTemp = new TextBox();
                    editTemp.Left = iLeftEdit;
                    editTemp.Top = iTopControl + iRowNo * iRowHeight;
                    editTemp.Name = "editOption" + Convert.ToString(i + 1);
                    editTemp.Size = new System.Drawing.Size(iWidth, iHeight);
                    editTemp.Text = "";

                    panelControl.Controls.Add(editTemp);
                    tControlAdd[i].editControl = editTemp;
                }
                //
                tControlAdd[i].sDisPlayName = sDisplay_Name;
                tControlAdd[i].sSQLNAME = dsTemp.Tables[0].Rows[i]["SQL_NAME"].ToString();
                tControlAdd[i].sSQLTYPE = dsTemp.Tables[0].Rows[i]["SQL_TYPE"].ToString();
            }
            int iWindowExtraHeight = iRowHeight * dsTemp.Tables[0].Rows.Count;
            this.Size = new Size(this.Width, this.Height + iWindowExtraHeight);
            this.Location = new Point(this.Location.X, this.Location.Y - iWindowExtraHeight);
        }
    }
}