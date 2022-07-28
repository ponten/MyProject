using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace RC_Split_Merge
{
    public partial class fMain : Form
    {
        //string g_sExeName, g_sFunction, g_sUserID, g_sProgram;
        string sSQL;
        string sSQL1;
        string sSQL2;
        string sSQL3;
        string sRC;
        string ssRC;
        string sTravelId;
        DataSet dsTemp;
        DataSet dsTemp1;
        DataSet dsTemp2;
        DataSet dsTemp3;

        public fMain()
        {
            InitializeComponent();
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }

        private void txtRC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            //sRC = txtRC.Text;
            //ShowRCSplitAndMergeList(sRC);
            ShowData();
        }

        public void ShowData()
        {
            sRC = txtRC.Text;
            //sSQL = @"select * from sajet.g_rc_status where rc_no = '"+ sRC +"'";
            //dsTemp = ClientUtils.ExecuteSQL(sSQL);
            //if (dsTemp.Tables[0].Rows.Count == 0)
            //{
            //    string sMsg = SajetCommon.SetLanguage("rc error");
            //    SajetCommon.Show_Message(sMsg,1);
            //    return;
            //}
            sSQL = @"select * from sajet.g_rc_split  where source_rc_no = '"+ sRC +"' or rc_no = '"+ sRC +"'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = @"select * from sajet.g_rc_merge where rc_no = '" + sRC + "' or rc_no = '"+ sRC +"'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    string sMsg = SajetCommon.SetLanguage("rc is not merge or split");
                    SajetCommon.Show_Message(sMsg, 1);
                    return;
                }
            }
            dtl.Rows.Clear();
            dtl.Columns.Clear();
            table.Rows.Clear();
            ShowRCSplitAndMergeList(sRC);
        }
        

    }
}
