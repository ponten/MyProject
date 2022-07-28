using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ClassSajetIniFile;

namespace MESInterface
{
    public partial class fSetup : Form
    {
        string g_sIniFile = fMain.g_sIniFile;
        string g_sIniSection = fMain.g_sIniSection;
        public string g_sDBName;

        public fSetup()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {            
           txtInterval.Text = txtInterval.Text.Trim();
           try
           {
               int iInterval = Convert.ToInt32(txtInterval.Text);
               if (iInterval <= 0)
               {
                   MessageBox.Show("Interval Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   txtInterval.Focus();
                   txtInterval.Select();
                   return;
               }
           }
           catch
           {
               MessageBox.Show("Interval Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               txtInterval.Focus();
               txtInterval.Select();
               return;
           }

           string sChecked = "0";
           if (chkbAutoTransfer.Checked)
               sChecked = "1";

           string sCheckedMail = "0";
           if (chkbMail.Checked)
               sCheckedMail = "1";           

           string sBreak = "0";
           if (chkbBreak.Checked)
               sBreak = "1";

           SajetIniFile SajetIniFile = new SajetIniFile();
           SajetIniFile.WriteIniFile(g_sIniFile, g_sIniSection, "AutoTransfer", sChecked);
           SajetIniFile.WriteIniFile(g_sIniFile, g_sIniSection, "Interval", txtInterval.Text);
           SajetIniFile.WriteIniFile(g_sIniFile, g_sIniSection, "SendMail", sCheckedMail);           
           SajetIniFile.WriteIniFile(g_sIniFile, g_sIniSection, "Break", sBreak);           
            
            for (int i = 0; i <= LVItem.Items.Count - 1; i++)
            {
                sChecked = "0";
                string sType = LVItem.Items[i].SubItems[0].Text;
                if (LVItem.Items[i].Checked)
                {
                    sChecked = "1";                   
                }
                SajetIniFile.WriteIniFile(g_sIniFile, g_sIniSection, "TransItem" + sType, sChecked);
            }

            //DB.cfg
            StreamWriter sw;
            string sFile = "DB.cfg";
            if (File.Exists(sFile))
                sw = new StreamWriter(sFile, false, Encoding.Default);
            else
                sw = File.CreateText(sFile);
            sw.WriteLine("DB = " + editDBName.Text);
            sw.Close();

            DialogResult = DialogResult.OK;           
        }                

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void fSetup_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            LVItem.Items.Clear();
            for (int i = 0; i <= fMain.g_tsItem.Length - 1; i++)
            {
                LVItem.Items.Add(fMain.g_tsItem[i].ToString());
                LVItem.Items[i].SubItems.Add(fMain.g_tsItemDesc[i].ToString());
            }
           
            string sChecked = "0";
            SajetIniFile SajetIniFile = new SajetIniFile();
            sChecked = SajetIniFile.ReadIniFile(g_sIniFile, g_sIniSection, "AutoTransfer", "0");
            chkbAutoTransfer.Checked = (sChecked == "1");
            
            txtInterval.Text = SajetIniFile.ReadIniFile(g_sIniFile, g_sIniSection, "Interval", "10");
            sChecked = SajetIniFile.ReadIniFile(g_sIniFile, g_sIniSection, "SendMail", "0");
            chkbMail.Checked = (sChecked == "1");
                       
            sChecked = SajetIniFile.ReadIniFile(g_sIniFile, g_sIniSection, "Break", "0");
            chkbBreak.Checked = (sChecked == "1");
            
            for (int i = 0; i <= LVItem.Items.Count - 1; i++)
            {
                string sType = LVItem.Items[i].SubItems[0].Text;
                sChecked = SajetIniFile.ReadIniFile(g_sIniFile, g_sIniSection, "TransItem" + sType, "0");
                LVItem.Items[i].Checked = (sChecked == "1");
            }

            //Read DB Name            
            editDBName.Text = g_sDBName;               
        }        
        
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVItem.Items.Count - 1; i++)
            {
                LVItem.Items[i].Checked = true;
            }
        }

        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVItem.Items.Count - 1; i++)
            {
                LVItem.Items[i].Checked = false;
            }
        }        
    }
}