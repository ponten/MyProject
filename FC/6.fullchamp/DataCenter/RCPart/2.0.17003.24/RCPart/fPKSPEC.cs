using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace RCPart
{
    public partial class fPKSPEC : Form
    {
        public fPKSPEC()
        {
            InitializeComponent();
        }
        
        private void fPKSPEC_Load(object sender, EventArgs e)
        {           
            DialogResult = DialogResult.None;
            ShowFilterData();

            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }

        public void ShowFilterData()
        {
            string sSQL = " Select PKSPEC_ID,PKSPEC_NAME,BOX_QTY,CARTON_QTY,PALLET_QTY "
                         + " from SAJET.SYS_PKSPEC "
                         + " Where ENABLED ='Y' ";
            if (!string.IsNullOrEmpty(editFilter.Text))
                sSQL = sSQL + " and PKSPEC_NAME like '" + editFilter.Text + "%' ";
            sSQL = sSQL + " ORDER BY PKSPEC_NAME ";
            DataSet dsPKSPEC = ClientUtils.ExecuteSQL(sSQL);
            grdViewData.DataSource = dsPKSPEC;
            grdViewData.DataMember = dsPKSPEC.Tables[0].ToString();
            grdViewData.Columns[0].Name = "PKSPEC_ID";
            grdViewData.Columns[0].Visible = false;
            if (grdViewData.Rows.Count > 0)
            {
                grdViewData.Rows[0].Selected = true;
                grdViewData.CurrentCell = grdViewData.Rows[0].Cells["PKSPEC_NAME"]; 
            }
        }

        private void editFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13)
                return;

            ShowFilterData();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            DialogResult = DialogResult.OK;
        }        
    }
}