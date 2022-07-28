using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace CRole
{
    public partial class fProcess : Form
    {
        public string sRole;
        public string sUser;
        DataSet dsData;

        public fProcess()
        {
            InitializeComponent();
        }

        private void FProcess_Load(object sender, EventArgs e)
        {
            string sSQL = $@"
SELECT ROLE_NAME
  FROM SAJET.SYS_ROLE
 WHERE ROLE_ID = {sRole}
";
            lbEMP.Text = SajetCommon.SetLanguage("Role Name", 1)
                + ": "
                + ClientUtils.ExecuteSQL(sSQL).Tables[0].Rows[0]["ROLE_NAME"].ToString();

            sSQL = $@"
   SELECT COUNT(B.ROLE_ID) ""CHECK""
         ,A.PROCESS_ID
         ,A.PROCESS_NAME
     FROM SAJET.SYS_PROCESS A
LEFT JOIN (SELECT ROLE_ID
                 ,PROCESS_ID
             FROM SAJET.SYS_ROLE_OP_PRIVILEGE
            WHERE ROLE_ID = {sRole}) B
       ON A.PROCESS_ID = B.PROCESS_ID
    WHERE A.OPERATE_ID = 5
 GROUP BY A.PROCESS_ID
         ,A.PROCESS_NAME
 ORDER BY A.PROCESS_ID";

            dsData = ClientUtils.ExecuteSQL(sSQL);

            dgvProcess.DataSource = dsData;
            dgvProcess.DataMember = dsData.Tables[0].ToString();
            SajetCommon.SetLanguageControl(this);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            string sInsert = string.Empty;
            string sVal = $@"
INTO SAJET.SYS_ROLE_OP_PRIVILEGE
(
    ROLE_ID
   ,PROCESS_ID
   ,UPDATE_USERID
   ,UPDATE_TIME
)
VALUES
(   {sRole}
   ,{{0}}
   ,{sUser}
   ,SYSDATE
)";

            foreach (DataGridViewRow viewRow in dgvProcess.Rows)
            {
                if ((bool)viewRow.Cells["CHECK"].FormattedValue)
                    sInsert += string.Format(sVal, viewRow.Cells["PROCESS_ID"].Value);
            }

            ClientUtils.ExecuteSQL($"DELETE FROM SAJET.SYS_ROLE_OP_PRIVILEGE WHERE ROLE_ID = {sRole}");

            if (!string.IsNullOrEmpty(sInsert.Trim()))
            {
                sInsert = $@"INSERT ALL {sInsert} SELECT 1 FROM DUAL";
                ClientUtils.ExecuteSQL(sInsert);
            }

            DialogResult = DialogResult.OK;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnDeSelect_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in dsData.Tables[0].Rows)
                row["CHECK"] = 0;
            dsData.AcceptChanges();
            dgvProcess.Rows[0].Cells[0].Selected = true;
        }
    }
}
