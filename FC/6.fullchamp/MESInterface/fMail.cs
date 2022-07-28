using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClassSajetIniFile;
using ClassMail;

namespace MESInterface
{
    public partial class fMail : Form
    {
        string g_sIni = fMain.g_sIniFile;
        string g_sSection = "EMail";

        public fMail()
        {
            InitializeComponent();
        }

        private void editMailTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editMailTo.Text = editMailTo.Text.Trim();
            if (string.IsNullOrEmpty(editMailTo.Text))
                return;
            
            AddMail(LVMailTo, editMailTo.Text, "");
            editMailTo.Focus();
            editMailTo.SelectAll();
        }
        
        private void AddMail(ListView LVTemp, string sEMAIL, string sEMP)
        {
            if (string.IsNullOrEmpty(sEMAIL))
                return;
            if (LVTemp.Items.Find(sEMAIL, false).Length > 0)
            {
                fMain.Show_Message("EMail Duplicate", 0);
                return;
            }
            LVTemp.Items.Add(sEMAIL);            
            LVTemp.Items[LVTemp.Items.Count - 1].Name = sEMAIL;
        }
        
        private void btnTo_Click(object sender, EventArgs e)
        {
            string sSQL = " Select EMP_NO,EMP_NAME,EMAIL from sajet.sys_emp "
                         + " where enabled = 'Y' "
                         //+ " and EMAIL is not null "
                         + " Order By EMP_NO ";
            fFilter fF = new fFilter();
            fF.sSQL = sSQL;
            if (fF.ShowDialog() == DialogResult.OK)
            {
                //LVTemp.Sorting = SortOrder.None;
                for (int i = 0; i <= fF.dgvData.SelectedRows.Count - 1; i++)
                {
                    string sEMP = fF.dgvData.SelectedRows[i].Cells["EMP_NAME"].Value.ToString().Trim();
                    string sEMAIL = fF.dgvData.SelectedRows[i].Cells["EMAIL"].Value.ToString().Trim();
                    AddMail(LVMailTo, sEMAIL, sEMP);
                }
                //LVTemp.Sorting = SortOrder.Ascending;
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string sMailTo = "";
            if (string.IsNullOrEmpty(editPort.Text))
                editPort.Text = "25";

            SajetIniFile SajetIniFile = new SajetIniFile();
            SajetIniFile.WriteIniFile(g_sIni, g_sSection, "SMTPHost", editHost.Text);
            SajetIniFile.WriteIniFile(g_sIni, g_sSection, "SMTPPort", editPort.Text);
            SajetIniFile.WriteIniFile(g_sIni, g_sSection, "User", editUser.Text);
            SajetIniFile.WriteIniFile(g_sIni, g_sSection, "Passwd", editPasswd.Text);
            SajetIniFile.WriteIniFile(g_sIni, g_sSection, "MailFrom", editMailFrom.Text);
            SajetIniFile.WriteIniFile(g_sIni, g_sSection, "MailSubject", editMailSub.Text);            
            for (int i = 0; i <= LVMailTo.Items.Count - 1; i++)
            {
                sMailTo = sMailTo + LVMailTo.Items[i].Text + ";";
            }
            sMailTo = sMailTo.TrimEnd(';');
            SajetIniFile.WriteIniFile(g_sIni, g_sSection, "MailTo", sMailTo);

            DialogResult = DialogResult.OK;
        }

        private void fMail_Load(object sender, EventArgs e)
        {
            SajetIniFile SajetIniFile = new SajetIniFile();
            editHost.Text = SajetIniFile.ReadIniFile(g_sIni, g_sSection, "SMTPHost", "");
            editPort.Text = SajetIniFile.ReadIniFile(g_sIni, g_sSection, "SMTPPort", "0");
            editUser.Text = SajetIniFile.ReadIniFile(g_sIni, g_sSection, "User", "");
            editPasswd.Text = SajetIniFile.ReadIniFile(g_sIni, g_sSection, "Passwd", "");
            editMailFrom.Text = SajetIniFile.ReadIniFile(g_sIni, g_sSection, "MailFrom", "");
            editMailSub.Text = SajetIniFile.ReadIniFile(g_sIni, g_sSection, "MailSubject", "");
            string sMailTo = SajetIniFile.ReadIniFile(g_sIni, g_sSection, "MailTo", "");
            string[] tsEmail = sMailTo.Split(';');   

            LVMailTo.Items.Clear();
            for (int i = 0; i <= tsEmail.Length - 1; i++)
            {
                string sEmail = tsEmail[i].ToString().Trim();
                if (!string.IsNullOrEmpty(sEmail))
                {
                    LVMailTo.Items.Add(sEmail);
                    LVMailTo.Items[LVMailTo.Items.Count - 1].Name = sEmail;
                }
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LVMailTo.SelectedItems == null)
                return;
            if (LVMailTo.SelectedItems.Count == 0)
                return;

            for (int i = LVMailTo.SelectedItems.Count - 1; i >= 0; i--)
                LVMailTo.Items.RemoveAt(LVMailTo.SelectedItems[i].Index);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sMailTo = "";
            string sConform = "";
            if (string.IsNullOrEmpty(editPort.Text))
                editPort.Text = "25";
            for (int i = 0; i <= LVMailTo.Items.Count - 1; i++)
            {
                sMailTo = sMailTo + LVMailTo.Items[i].Text + ";";
                sConform = sConform + LVMailTo.Items[i].Text + Environment.NewLine;
            }
            sMailTo = sMailTo.TrimEnd(';');
            if (fMain.Show_Message("Send Test Mail ?" + Environment.NewLine + sConform, 2) != DialogResult.Yes)
                return;

            //讀取Mail主機資訊及要寄送的Mail address
            MailItem g_MailItems = new MailItem();
            g_MailItems.To = sMailTo; //收件者            
            g_MailItems.From = editMailFrom.Text; //寄件者            
            g_MailItems.Subject = "Interface Test";  //郵件主旨
            g_MailItems.body = "這是一封Interface測試信"; //郵件內容

            g_MailItems.userName = editUser.Text;
            g_MailItems.password = editPasswd.Text;
            g_MailItems.smtpHost = editHost.Text;
            g_MailItems.smtpPort = Convert.ToInt32(editPort.Text);

            string sErrMsg = "";
            ClassMail.SendMail mail = new ClassMail.SendMail();
            bool bSendResult = mail.Send(g_MailItems, ref sErrMsg);
            if (!bSendResult)
            {
                MessageBox.Show("Send Fail:" + Environment.NewLine + sErrMsg);
            }
            else
            {
                MessageBox.Show("Send OK");
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LVMailTo.SelectedItems == null)
                return;
            if (LVMailTo.SelectedItems.Count == 0)
                return;

            for (int i = LVMailTo.SelectedItems.Count - 1; i >= 0; i--)
                LVMailTo.Items.RemoveAt(LVMailTo.SelectedItems[i].Index);
        }

        private void editPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == '.' || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }
        }
    }
}