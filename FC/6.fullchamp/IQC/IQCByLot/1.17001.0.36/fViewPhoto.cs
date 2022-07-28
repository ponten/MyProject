using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using SajetClass;
namespace IQCbyLot
{
    public partial class fViewPhoto : Form
    {
        public string g_sLotno, g_sExeName;
        string g_sFilePath;
        TabControl tc = new TabControl();
        public fViewPhoto()
        {
            InitializeComponent();
        }

        private void fViewPhoto_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            lablLotNo.Text = g_sLotno;
            g_sFilePath = System.Windows.Forms.Application.StartupPath + "\\" + g_sExeName + "\\Image";
            if (!Directory.Exists(g_sFilePath))
                Directory.CreateDirectory(g_sFilePath);
            else
            {
                string[] FileList = new string[] { "" };
                FileList = Directory.GetFiles(g_sFilePath, "*.*");
                for (int i = 0; i <= FileList.Length - 1; i++)
                {
                    try
                    {
                        File.Delete(FileList[i].ToString());
                    }
                    catch
                    {
                    }
                }
            }
            Get_Lot_Defect_Image();
        }
        private void Get_Lot_Defect_Image()
        {
            tc.Parent = PanelImage;
            tc.Dock = DockStyle.Fill;

            string sSQL = " Select A.* ,TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') FILENAME "
                        + "   from SAJET.G_IQC_LOT_IMAGE A "
                        + " where A.LOT_NO = '" + g_sLotno + "' "
                        + " ORDER BY A.FILE_NAME ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            ListBox listTemp = new ListBox();
            listTemp.Items.Add(".JPG");
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {

                byte[] blobFile = new byte[0];
                if (!dsTemp.Tables[0].Rows[i]["UPLOAD_FILE"].ToString().Equals(""))
                {
                    blobFile = (byte[])dsTemp.Tables[0].Rows[i]["UPLOAD_FILE"];
                }
                string sImgFile = dsTemp.Tables[0].Rows[i]["FILE_NAME"].ToString();
                FileInfo f = new FileInfo(sImgFile);
                string sExt = f.Extension;
                if (listTemp.Items.IndexOf(sExt.ToUpper()) < 0)
                    continue;

                string sFileName = g_sFilePath + "\\" + dsTemp.Tables[0].Rows[i]["FILENAME"].ToString() + i.ToString() + sExt;
                File.WriteAllBytes(sFileName, blobFile);
                TabPage t1 = new TabPage();
                t1.Tag = dsTemp.Tables[0].Rows[i]["FILE_NAME"].ToString();
                t1.Text = dsTemp.Tables[0].Rows[i]["FILE_NAME"].ToString();
                tc.TabPages.Add(t1);
                Panel p = new Panel();
                p.Parent = t1;
                p.Dock = DockStyle.Fill;
                p.Tag = sFileName;
                p.DoubleClick += new EventHandler(p_DoubleClick);
                p.BackgroundImage = Image.FromFile(sFileName);
                p.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
        private void p_DoubleClick(object sender, EventArgs e)
        {
            if (!(sender is Panel))
                return;
            string sTage = (sender as Panel).Tag.ToString();
            if (File.Exists(sTage))
                System.Diagnostics.Process.Start(sTage);
        }

    }
}