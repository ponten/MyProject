using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;

namespace CWoManager
{
    public partial class fPKSpec : Form
    {
        string g_sSQL = string.Empty;
        Dictionary<string, fMain.TDBInitial> g_DBInitial = new Dictionary<string, fMain.TDBInitial>();

        public fPKSpec(Dictionary<string, fMain.TDBInitial> DBInitial)
        {
            InitializeComponent();
            g_DBInitial = DBInitial;
        }      

        private void fPKSpec_Load(object sender, EventArgs e)
        {
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            DialogResult = DialogResult.None;
            string sErrorMsg = string.Empty;
            if (g_DBInitial.ContainsKey("Packing Spec Filter"))
                g_sSQL = g_DBInitial["Packing Spec Filter"].sValue;
            else
                g_sSQL = @"SELECT PKSPEC_ID,PKSPEC_NAME,BOX_QTY,CARTON_QTY,PALLET_QTY
                    FROM SAJET.SYS_PKSPEC
                    WHERE ENABLED ='Y'
                    AND PKSPEC_NAME LIKE :PKSPEC_NAME
                    ORDER BY PKSPEC_NAME";
            ShowFilterData();
            SajetCommon.SetLanguageControl(this);
        }

        public void ShowFilterData()
        {
            string sSQL = g_sSQL;
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PKSPEC_NAME", editFilter.Text + "%" };
            DataSet dsPKSPEC = ClientUtils.ExecuteSQL(sSQL, Params);
            grdViewData.DataSource = dsPKSPEC;
            grdViewData.DataMember = dsPKSPEC.Tables[0].ToString();
            grdViewData.Columns[0].Name = "PKSPEC_ID";
            grdViewData.Columns[0].Visible = false;
        }

        private void editFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13)
                return;

            ShowFilterData();
        }

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

    }
}