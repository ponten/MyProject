using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using System.Data.OracleClient;


namespace DefectTypeDll
{
    public partial class fDetailData : Form
    {
        public fDetailData()
        {
            InitializeComponent();
        }
        /// <summary>
        /// g_sKeyID = DEFECT_TYPE_ID
        /// g_sTypeID = TYPE_EMP_ID
        /// g_sEmpID = EMP_ID
        /// g_sEmpName = EMP_NAME
        /// g_sEmpNO = EMP_NO
        /// g_sDefectCode = DEFECT_TYPE_CODE
        /// g_sDefectDESC = DEFECT_TYPE_ID
        /// </summary>
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID,g_sEmpID,g_sEmpName,g_sEmpNO,g_sTypeID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;
        public string g_sDefectCode, g_sDefectDESC;
        
        //窗体加载
        private void fDetailData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.Text = g_sformText;

            if(g_sUpdateType == "APPEND")
            {
                editCode.Text = g_sDefectCode;
                editDesc.Text = g_sDefectDESC;

                //获取DEFECT_TYPE_ID
                sSQL = "Select DEFECT_TYPE_ID FROM sajet.sys_defect_type WHERE DEFECT_TYPE_CODE = " + "'" + editCode.Text + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                g_sKeyID = dsTemp.Tables[0].Rows[0]["DEFECT_TYPE_ID"].ToString();

            }

            if (g_sUpdateType == "MODIFY")
            {
                //g_sEmpNO = dataCurrentRow.Cells["EMP_NO"].Value.ToString();
                //g_sEmpName = dataCurrentRow.Cells["EMP_NAME"].Value.ToString();
                
                //获取选取行的所有数据
                //sSQL = "Select  a.DEFECT_TYPE_CODE,a.DEFECT_TYPE_DESC,b.EMP_ID,c.EMP_NAME,c.EMP_NO from sajet.sys_defect_type a,sajet.sys_defect_type_emp b,sajet.sys_emp c"   
                //    + " where c.emp_no ='" + g_sEmpNO +"'"
                //    + " and b.emp_id = c.emp_id "
                //    + "and a.defect_type_id = b.defect_type_id";

                //dsTemp = ClientUtils.ExecuteSQL(sSQL);
                //editCode.Text = dsTemp.Tables[0].Rows[0]["DEFECT_TYPE_CODE"].ToString();
                //editDesc.Text = dsTemp.Tables[0].Rows[0]["DEFECT_TYPE_DESC"].ToString();
                //editEmpId.Text = dsTemp.Tables[0].Rows[0]["EMP_NO"].ToString();
                //editName.Text = dsTemp.Tables[0].Rows[0]["EMP_NAME"].ToString();

                editCode.Text = g_sDefectCode;
                editDesc.Text = g_sDefectDESC;
                g_sEmpNO = dataCurrentRow.Cells["EMP_NO"].Value.ToString();
                sSQL = "select a.emp_no,a.emp_name from sajet.sys_emp a,sajet.sys_defect_type_emp b where b.emp_id = a.emp_id and a.emp_no = '"+ g_sEmpNO +"'";

                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                editEmpId.Text = dsTemp.Tables[0].Rows[0]["emp_no"].ToString();
                editName.Text = dsTemp.Tables[0].Rows[0]["emp_name"].ToString();

            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (editCode.Text == "")
            {
                string sData = labelCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }
            if (editDesc.Text == "")
            {
                string sData = labelDesc.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editDesc.Focus();
                editDesc.SelectAll();
                return;
            }

//            if (g_sUpdateType == "MODIFY")
//            {

//                if (editEmpId != null && editEmpId.Text != "")
//                {
//                    //判断输入责任人是否为DB中存在的EMP
//                    sSQL = "Select * From sajet.sys_emp where emp_no = '" + editEmpId.Text + "' and enabled = 'Y'";
//                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

//                    //EMP存在
//                    if (dsTemp.Tables[0].Rows.Count > 0)
//                    {

//                        //string sSQL1 = "select * from sajet.sys_defect_type_emp "
//                        //    + " where defect_type_id = '" + g_sKeyID + "'"
//                        //    + " and type_emp_id =(select type_emp_id from sajet.sys_defect_type_emp "
//                        //    + " where emp_id = (select emp_id from sajet.sys_emp where emp_no = '" + editEmpId.Text + "'"
//                        //    + "))";
////                        string sSQL1 = @"select * from sajet.sys_defect_type a,sajet.sys_defect_type_emp b,sajet.sys_emp c where 
////                            a.defect_type_code = '"+ editCode.Text +"' and a.defect_type_id = b.defect_type_id and b.emp_id = c.emp_id and c.emp_no = '"+ editEmpId.Text +"'";

////                        dsTemp = ClientUtils.ExecuteSQL(sSQL1);
////                        //判断Modify的数据是否重复
////                        if (dsTemp.Tables[0].Rows.Count > 0)
////                        {
////                            string sData = labelEmpID.Text + " : " + editEmpId.Text;
////                            string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
////                            SajetCommon.Show_Message(sMsg, 0);
////                            return;
////                        }
//                    }
//                    //EMP不存在弹框数据不存在
//                    else
//                    {
//                        string sData = labelEmpID.Text + " : " + editEmpId.Text;
//                        string sMsg = SajetCommon.SetLanguage("Data No Eixst", 2) + Environment.NewLine + sData;
//                        SajetCommon.Show_Message(sMsg, 0);
//                        return;
//                    }
//                }
//                else
//                {
//                    string sData = labelEmpID.Text;
//                    string sMsg = SajetCommon.SetLanguage("Data Is Null") + Environment.NewLine + sData;
//                    SajetCommon.Show_Message(sMsg, 0);
//                    return;
//                }
//            }

            //Update DB
            try
            {
                if (g_sUpdateType == "APPEND")
                {
                    if (editEmpId.Text != null && editEmpId.Text != "")
                    {
                        sSQL = "Select emp_id from sajet.sys_emp where emp_no = '" + editEmpId.Text + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (dsTemp.Tables[0].Rows.Count == 0)
                        {
                            string sData = labelEmpID.Text + " : " + editEmpId.Text;
                            string sMsg = SajetCommon.SetLanguage("Data No Exist") + Environment.NewLine + sData;
                            SajetCommon.Show_Message(sMsg, 0);
                            return;
                        }
                        g_sEmpID = dsTemp.Tables[0].Rows[0]["EMP_ID"].ToString();

                        string sSQL1 = "select * from sajet.sys_defect_type_emp a,sajet.sys_defect_type b where a.emp_id = '" + g_sEmpID + "' and a.defect_type_id = b.defect_type_id and b.defect_type_code = '" + editCode.Text + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL1);
                        if (dsTemp.Tables[0].Rows.Count == 0)
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
                        else
                        {
                            string sData = labelEmpID.Text + " : " + editEmpId.Text;
                            string sMsg = SajetCommon.SetLanguage("Data Duplicate") + Environment.NewLine + sData;
                            SajetCommon.Show_Message(sMsg, 0);
                            return;
                        }
                    }
                    else
                    {
                        string sData = labelEmpID.Text;
                        string sMsg = SajetCommon.SetLanguage("Data Is Null") + Environment.NewLine + sData;
                        SajetCommon.Show_Message(sMsg, 0);
                        return;
                    }
                }
                else if (g_sUpdateType == "MODIFY")
                {
                    if (editEmpId.Text != null && editEmpId.Text != "")
                    {

                        //判断输入责任人是否为DB中存在的EMP
                        sSQL = "Select * From sajet.sys_emp where emp_no = '" + editEmpId.Text + "' and enabled = 'Y'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        //EMP存在
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {

                            sSQL = "Select emp_id from sajet.sys_emp where emp_no = '" + editEmpId.Text + "'";
                            dsTemp = ClientUtils.ExecuteSQL(sSQL);
                            g_sEmpID = dsTemp.Tables[0].Rows[0]["EMP_ID"].ToString();

                            string sSQL1 = "select * from sajet.sys_defect_type_emp a,sajet.sys_defect_type b where a.emp_id = '" + g_sEmpID + "' and a.defect_type_id = b.defect_type_id and b.defect_type_code = '" + editCode.Text + "'";
                            dsTemp = ClientUtils.ExecuteSQL(sSQL1);
                            if (dsTemp.Tables[0].Rows.Count <= 0)
                            {

                                ModifyData();
                                string sMsg = SajetCommon.SetLanguage("Data Modify OK") + " !";
                                SajetCommon.Show_Message(sMsg,3);
                                DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                string sData = labelEmpID.Text + " : " + editEmpId.Text;
                                string sMsg = SajetCommon.SetLanguage("Data Duplicate") + Environment.NewLine + sData;
                                SajetCommon.Show_Message(sMsg, 0);
                                return;
                            }
                        }
                        //EMP不存在弹框数据不存在
                        else
                        {
                            string sData = labelEmpID.Text + " : " + editEmpId.Text;
                            string sMsg = SajetCommon.SetLanguage("Data No Exist") + Environment.NewLine + sData;
                            SajetCommon.Show_Message(sMsg, 0);
                            return;
                        }

                    }
                    else
                    {
                        string sData = labelEmpID.Text;
                        string sMsg = SajetCommon.SetLanguage("Data Is Null") + Environment.NewLine + sData;
                        SajetCommon.Show_Message(sMsg, 0);
                        return;
                    }

                }
                
            }
            catch(Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }

        }

        private void AppendData()
        {
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_DEFECT_TYPE_EMP", "TYPE_EMP_ID", 10);

            object[][] Params = new object[4][];
            sSQL = " Insert into SAJET.SYS_DEFECT_TYPE_EMP"
                + "(TYPE_EMP_ID,EMP_ID,DEFECT_TYPE_ID,UPDATE_TIME,UPDATE_USER_ID,ENABLED)"
                + " Values "
                + " (:TYPE_EMP_ID,:EMP_ID,:DEFECT_TYPE_ID,SYSDATE,:UPDATE_USER_ID,'Y') ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPE_EMP_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_ID", g_sEmpID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_TYPE_ID", g_sKeyID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USER_ID", fMain.g_sUserID };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                

        }

        private void ModifyData()
        {
           
            string sSQL = "Select emp_id from sajet.sys_emp where emp_no = '" + editEmpId.Text + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
                return;
            g_sEmpID = dsTemp.Tables[0].Rows[0]["EMP_ID"].ToString();

            //sSQL = "Select a.TYPE_EMP_ID FROM sajet.sys_defect_type_emp a,sajet.sys_defect_type b "
            //    + " WHERE b.defect_type_code = " + "'" + editCode.Text + "'"
            //    + " and a.defect_type_id = b.defect_type_id ";

            g_sEmpNO = dataCurrentRow.Cells["EMP_NO"].Value.ToString();

            sSQL = @"select a.type_emp_id from sajet.sys_defect_type_emp a,sajet.sys_defect_type b,sajet.sys_emp c where 
                    a.defect_type_id = b.defect_type_id and b.defect_type_code = '"+ g_sDefectCode +"' and c.emp_no = '"+ g_sEmpNO +"' and a.emp_id = c.emp_id";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
                return;
            g_sTypeID = dsTemp.Tables[0].Rows[0]["TYPE_EMP_ID"].ToString();

            object[][] Params = new object[2][];
            sSQL = "Update sajet.sys_defect_type_emp"
                + " set EMP_ID = :EMP_ID "
                + " WHERE TYPE_EMP_ID = :TYPE_EMP_ID ";
            Params[0] = new object[] { ParameterDirection.Input,OracleType.VarChar,"EMP_ID",g_sEmpID};
            Params[1] = new object[] { ParameterDirection.Input,OracleType.VarChar,"TYPE_EMP_ID",g_sTypeID};

            dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
        }


        private void ClearData()
        {
            editCode.Text = "";
            editDesc.Text = "";
            editEmpId.Text = "";
            editName.Text = "";
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;

        }


        //输入EMP_NO后回车获取对应员工姓名
        private void editEmpId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            string str = editEmpId.Text;
            sSQL = "Select emp_name from sajet.sys_emp where EMP_NO =" + "'" + str + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                string sData = labelEmpID.Text + " : " + editEmpId.Text;
                string sMsg = SajetCommon.SetLanguage("Data No Exist") + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                return;
            }
            editName.Text = dsTemp.Tables[0].Rows[0]["EMP_NAME"].ToString();
        }

        


        
    }
}
