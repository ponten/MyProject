using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
namespace RCProcessParam
{
    public partial class fAlert : Form
    {
        public fAlert()
        {
            InitializeComponent();
        }
        public string PartID = "";
        public string ProcessID = "";
        public string Type = "";
        public string ItemName = "";
        public string ItemID = "";
        public string ItemSeq = "";
        private void fAlert_Load(object sender, EventArgs e)
        {
            this.Text = this.Type;
            if (Type.Equals("Add"))
            {

            }
            else if (Type.Equals("Modify"))
            {
                txtName.Enabled = false;
                string cmd = " SELECT ITEM_SEQ, ITEM_NAME,item_id,ITEM_PHASE,item_seq, Alert_TYPE,item_title,item_content "
                            + " FROM SAJET.sys_rc_process_part_alert "
                            + " WHERE PART_ID ='" + PartID + "' "
                            + " AND PROCESS_ID = '" + ProcessID + "' "
                            + " AND ITEM_NAME = '" + ItemName + "' "
                            + " AND ROWNUM = 1 ";
                DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    ItemID = dt.Rows[0]["ITEM_ID"].ToString();
                    txtName.Text = dt.Rows[0]["ITEM_NAME"].ToString();
                    txtTitle.Text = dt.Rows[0]["item_title"].ToString();
                    txtContent.Text = dt.Rows[0]["item_content"].ToString();
                    nudSeq.Value = int.Parse(dt.Rows[0]["item_seq"].ToString());
                    nudSeq.Enabled = false;
                    string ItemPhase = dt.Rows[0]["ITEM_PHASE"].ToString();
                    if (ItemPhase.Equals("A"))
                    {
                        combPhase.SelectedIndex = 0;
                    }
                    else if (ItemPhase.Equals("I"))
                    {
                        combPhase.SelectedIndex = 1;
                    }
                    else if (ItemPhase.Equals("O"))
                        combPhase.SelectedIndex = 2;
                    else
                    {
                        SajetCommon.Show_Message("Unknown Item Phase:" + ItemPhase, 0);
                        return;
                    }
                    string AlertType = dt.Rows[0]["Alert_TYPE"].ToString();
                    if (AlertType.Equals("I"))
                    {
                        cbAlertType.SelectedIndex = 0;
                    }
                    else if (AlertType.Equals("W"))
                    {
                        cbAlertType.SelectedIndex = 1;
                    }
                    else if (AlertType.Equals("E"))
                        cbAlertType.SelectedIndex = 2;
                    else
                    {
                        SajetCommon.Show_Message("Unknown Alert Type:" + AlertType, 0);
                        return;
                    }
                }
            }
            SajetCommon.SetLanguageControl(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()) || string.IsNullOrEmpty(txtTitle.Text.Trim()) || string.IsNullOrEmpty(txtContent.Text.Trim()) || cbAlertType.SelectedIndex < 0 || combPhase.SelectedIndex < 0)
            {
                SajetCommon.Show_Message("Needed Item Cannot Be Empty", 0);
                return;
            }
            string cmd = "select * from sajet.sys_rc_process_part_alert t where 1=1 and(t.item_name='" + txtName.Text.Trim() + "' or t.item_seq='" + nudSeq.Value.ToString() + "') and t.part_id='" + PartID + "' and t.process_id='" + ProcessID + "'";
            if (this.Type.Equals("Add"))
            {


            }
            else if (this.Type.Equals("Modify"))
            {
                cmd += " and t.item_name !='" + this.ItemName + "' and t.item_seq!='" + nudSeq.Value.ToString() + "'";
            }
            DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
            if (dt.Rows.Count > 0)
            {
                SajetCommon.Show_Message("Item Name Or Seq Already Exist", 0);
                return;
            }
            string ItemPhase = "";
            switch (combPhase.SelectedIndex)
            {
                case 0:
                    ItemPhase = "A";
                    break;
                case 1:
                    ItemPhase = "I";
                    break;
                case 2:
                    ItemPhase = "O";
                    break;
                default:
                    ItemPhase = "A";
                    break;
            }
            string AlertType = "";
            switch (cbAlertType.SelectedIndex)
            {
                case 0:
                    AlertType = "I";
                    break;
                case 1:
                    AlertType = "W";
                    break;
                case 2:
                    AlertType = "E";
                    break;
                default:
                    AlertType = "I";
                    break;
            }
            if (this.Type.Equals("Add"))
            {
                ItemID = SajetCommon.GetMaxID("SAJET.sys_rc_process_part_alert", "item_id", 10);
                cmd = @"insert into SAJET.sys_rc_process_part_alert(item_id,part_id,process_id,item_name,item_phase,alert_type,item_seq,item_title,item_content,update_userid)
                        values('" + ItemID + "','" + PartID + "','" + ProcessID + "','" + txtName.Text.Trim() + "','" + ItemPhase + "','" + AlertType + "','" + nudSeq.Value.ToString() + "','" + txtTitle.Text + "','" + txtContent.Text + "','" + ClientUtils.UserPara1 + "')";
            }
            else if (this.Type.Equals("Modify"))
            {
                cmd = "update SAJET.sys_rc_process_part_alert set item_name='" + txtName.Text.Trim() + "',item_phase='" + ItemPhase + "',alert_type='" + AlertType + "',item_title='" + txtTitle.Text + "',item_content='" + txtContent.Text + "',update_time=sysdate,update_userid='" + ClientUtils.UserPara1 + "' where part_id='" + PartID + "' and process_id='" + ProcessID + "' and item_name='" + ItemName + "'";
            }
            ClientUtils.ExecuteSQL(cmd);
            this.DialogResult = DialogResult.OK;
        }
    }
}
