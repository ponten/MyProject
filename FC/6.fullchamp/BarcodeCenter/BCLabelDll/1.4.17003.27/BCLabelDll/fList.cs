using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace BCLabelDll
{
    public partial class fList : Form
    {
        public fList()
        {
            InitializeComponent();
        }

        public string sSQL;

        private void fList_Load(object sender, EventArgs e)
        {
            DataSet ds = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= ds.Tables[0].Columns.Count - 1; i++)
            {
                string sColumnName = ds.Tables[0].Columns[i].ToString();
                if (i >= 1 && i <= 2)
                    LVList.Columns.Add(sColumnName, 300);
                else
                    LVList.Columns.Add(sColumnName, 150);
            }

            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                LVList.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                for (int j = 1; j <= ds.Tables[0].Columns.Count - 1; j++)
                {
                    LVList.Items[i].SubItems.Add(ds.Tables[0].Rows[i][j].ToString());
                }                
            }

            SajetCommon.SetLanguageControl(this);
        }
    }
}