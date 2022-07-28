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

namespace IQCbyLot
{
    public partial class fRoHSItem : Form
    {

        public string g_sLotNo, g_sUpdateType,g_sPosition;
        string sSQL;
        DataSet dsTemp;

        public fRoHSItem()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private bool CheckPositionDup()
        {
            sSQL = "SELECT * "
                + "  FROM SAJET.G_IQC_ROHS_ITEM "
                + " WHERE LOT_NO =:LOT_NO "
                + "   AND POSITION =:POSITION "
                + "   AND ROWNUM = 1 ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "POSITION", editPosition.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                SajetCommon.Show_Message("RoHS Position Duplicate", 0);
                editPosition.Focus();
                editPosition.SelectAll();
                return true;
            }
            return false;
        }

        private void Append()
        {
            sSQL = "INSERT INTO SAJET.G_IQC_ROHS_ITEM  "
                + "(LOT_NO,POSITION,PB,CD,HG,CR,BR,CL,UPDATE_USERID,UPDATE_TIME,MEMO) "
                + " VALUES "
                + "(:LOT_NO,:POSITION,:PB,:CD,:HG,:CR,:BR,:CL,:UPDATE_USERID,SYSDATE,:MEMO)";

            object[][] Params = new object[10][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "POSITION", editPosition.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PB", editPb.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CD", editCd.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HG", editHg.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CR", editCr.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BR", editBr.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CL", editCl.Text };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.Int32, "UPDATE_USERID", ClientUtils.UserPara1 };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MEMO", editMemo.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            SajetCommon.Show_Message("Save OK",3);
            editPosition.Text = string.Empty;
            editPb.Text = string.Empty;
            editCd.Text = string.Empty;
            editHg.Text = string.Empty;
            editCr.Text = string.Empty;
            editBr.Text = string.Empty;
            editCl.Text = string.Empty;
            editMemo.Text = string.Empty;
            editPosition.Focus();
            editPosition.SelectAll();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            editPosition.Text = editPosition.Text.Trim();
            editPb.Text = editPb.Text.Trim();
            editCd.Text = editCd.Text.Trim();
            editHg.Text = editHg.Text.Trim();
            editCr.Text = editCr.Text.Trim();
            editBr.Text = editBr.Text.Trim();
            editCl.Text = editCl.Text.Trim();
            editMemo.Text = editMemo.Text.Trim();

            try
            {
                if (string.IsNullOrEmpty(editPosition.Text))
                {
                    SajetCommon.Show_Message("Please Input Inspection Position",0);
                    editPosition.Focus();
                    return;
                }
                if (g_sUpdateType == "APPEND")
                {
                    if (CheckPositionDup())
                        return;
                    Append();
                }
                else if (g_sUpdateType == "MODIFY")
                {
                    if (editPosition.Text != g_sPosition)
                    {
                        if (CheckPositionDup())
                            return;
                    }
                    sSQL ="UPDATE SAJET.G_IQC_ROHS_ITEM "
                        + "   SET POSITION =:POSITION "
                        + "      ,PB =:PB "
                        + "      ,CD =:CD "
                        + "      ,HG =:HG "
                        + "      ,CR =:CR "
                        + "      ,BR =:BR "
                        + "      ,CL =:CL "
                        + "      ,UPDATE_USERID =:UPDATE_USERID "
                        + "      ,UPDATE_TIME = SYSDATE "
                        + "      ,MEMO =:MEMO "
                        + " WHERE LOT_NO =:LOT_NO "
                        + "   AND POSITION =:POSITION_OLD ";
                    object[][] Params = new object[11][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "POSITION", editPosition.Text };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PB", editPb.Text };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CD", editCd.Text };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HG", editHg.Text };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CR", editCr.Text };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BR", editBr.Text };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CL", editCl.Text };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
                    Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MEMO", editMemo.Text };
                    Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
                    Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "POSITION_OLD", g_sPosition };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);                
            }
        }

        private void fRoHSItem_Load(object sender, EventArgs e)
        {
            this.Text = SajetCommon.SetLanguage(g_sUpdateType, 1) + " " + SajetCommon.SetLanguage("RoHS", 1);
          //  ClientUtils.SetLanguage(this, ClientUtils.fCurrentProject);
            SajetCommon.SetLanguageControl(this);

            if (g_sUpdateType == "MODIFY")
            {
                sSQL = "SELECT * FROM SAJET.G_IQC_ROHS_ITEM "
                    + " WHERE LOT_NO =:LOT_NO "
                    + "   AND POSITION =:POSITION "
                    + "   AND ROWNUM = 1 ";
                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "POSITION", g_sPosition };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                editPosition.Text = g_sPosition;

                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    editPb.Text = dsTemp.Tables[0].Rows[0]["PB"].ToString();
                    editCd.Text = dsTemp.Tables[0].Rows[0]["CD"].ToString();
                    editHg.Text = dsTemp.Tables[0].Rows[0]["HG"].ToString();
                    editCr.Text = dsTemp.Tables[0].Rows[0]["CR"].ToString();
                    editBr.Text = dsTemp.Tables[0].Rows[0]["BR"].ToString();
                    editCl.Text = dsTemp.Tables[0].Rows[0]["CL"].ToString();
                    editMemo.Text = dsTemp.Tables[0].Rows[0]["MEMO"].ToString();
                }
                editPosition.Focus();
                editPosition.SelectAll();
            }
        }

        private void fRoHSItem_Shown(object sender, EventArgs e)
        {
            editPosition.Focus();
            editPosition.SelectAll();
        }

        private void editInspPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            int iTag = Convert.ToInt32((sender as TextBox).Tag);

            for (int i = 0; i <= panel2.Controls.Count - 1; i++)
            {
                if (panel2.Controls[i] is TextBox)
                {
                    (panel2.Controls[i] as TextBox).BackColor = Color.White;
                     if (Convert.ToInt32((panel2.Controls[i] as TextBox).Tag) == iTag + 1)
                    {
                        (panel2.Controls[i] as TextBox).Focus();
                        (panel2.Controls[i] as TextBox).SelectAll();
                        (panel2.Controls[i] as TextBox).BackColor = Color.FromArgb(255, 255, 128);
                    }
                }
            }
            if (iTag == 8)
            {
                btnSave.Focus();
            }
        }

        private void btnDefectFilter_Click(object sender, EventArgs e)
        {
            sSQL = " Select A.POSITION "
                 + "  From SAJET.SYS_IQC_ROHS_POSITION A  "
                 + " WHERE A.ENABLED='Y' "
                 + " ORDER BY A.POSITION ";

            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                editPosition.Text = f.dgvData.CurrentRow.Cells["POSITION"].Value.ToString();
                editPb.Focus();
            }
        }
    }
}