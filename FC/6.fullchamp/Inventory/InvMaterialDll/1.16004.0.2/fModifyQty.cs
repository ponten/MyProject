using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace InvMaterialdll
{
    public partial class fModifyQty : Form
    {
        public string sQty, sReel, g_sUserID;


        public fModifyQty()
        {
            InitializeComponent();
        }

        private void fModifyQty_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            lablReelNo.Text = sReel;

            editQty.SelectAll();
            editQty.Focus();
        }


        private void editQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                //¥u¯à¶ñ¼Æ¦r
                if (!((e.KeyChar >= '0') && (e.KeyChar <= '9') || (e.KeyChar == '\b')))
                {
                    e.KeyChar = (char)Keys.None;
                }
                return;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                sQty = editQty.Text;
                string sSQL;
                sSQL = @"INSERT INTO SAJET.G_MATERIAL_HT 
                         SELECT * FROM SAJET.G_MATERIAL WHERE REEL_NO = '" + sReel + "' ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                sSQL = "UPDATE SAJET.G_MATERIAL SET REEL_QTY = " + sQty 
                     + ", UPDATE_USERID = " + g_sUserID + ", UPDATE_TIME = SYSDATE " 
                     + "WHERE REEL_NO = '" + sReel + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}