using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;
using SajetClass;

namespace IQCbyLot
{
    public partial class fImage : Form
    {
        public string g_sflag, g_sExeName;
        public string g_sRecID;
        DataSet dsTemp;
        string sSQL;

        public fImage()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void fImage_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            viewPhotoToolStripMenuItem.Visible = (g_sflag == "Y");
            GetImgFile(g_sRecID);
        }

        private void dgvImage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvImage_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "jpg";
            openFileDialog1.Filter = "All Files(*.jpg)|*.jpg";
            //openFileDialog1.Filter = "All Files(*.*)|*.*";
            openFileDialog1.FileName = String.Empty;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFile = openFileDialog1.FileName;

            if (LVFile.Items.Find(Path.GetFileName(sFile), false).Length > 0)
            {
                string sMsg = SajetCommon.SetLanguage("File Name Duplicate") + Environment.NewLine + sFile;
                SajetCommon.Show_Message(sMsg, 0);
                return;
            }
            if (InsertImgFile(g_sRecID, sFile))
                GetImgFile(g_sRecID);
        }

        private void GetImgFile(string sRECID)
        {
            if (g_sflag == "Y")
            {
                LVFile.Items.Clear();
                sSQL = "SELECT FILE_NAME,UPLOAD_FILE "
                     + " FROM SAJET.G_IQC_LOT_IMAGE "
                     + " WHERE LOT_NO =:LOT_NO "
                     + " ORDER BY FILE_NAME ";
                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sRECID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    byte[] blobFile = new byte[0];
                    if (!dsTemp.Tables[0].Rows[i]["UPLOAD_FILE"].ToString().Equals(""))
                    {
                        blobFile = (byte[])dsTemp.Tables[0].Rows[i]["UPLOAD_FILE"];
                    }
                    Double iSize = Convert.ToDouble(blobFile.Length) / 1024;
                    LVFile.Items.Add(dsTemp.Tables[0].Rows[i]["FILE_NAME"].ToString());
                    LVFile.Items[LVFile.Items.Count - 1].Name = dsTemp.Tables[0].Rows[i]["FILE_NAME"].ToString();
                    LVFile.Items[LVFile.Items.Count - 1].SubItems.Add(iSize.ToString("0.00"));
                }
            }
            else
            {
                LVFile.Items.Clear();
                sSQL = "SELECT FILE_NAME,UPLOAD_FILE "
                     + " FROM SAJET.G_IQC_NOTES_IMAGE "
                     + " WHERE RECID =:RECID "
                     + " ORDER BY FILE_NAME ";
                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", sRECID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    byte[] blobFile = new byte[0];
                    if (!dsTemp.Tables[0].Rows[i]["UPLOAD_FILE"].ToString().Equals(""))
                    {
                        blobFile = (byte[])dsTemp.Tables[0].Rows[i]["UPLOAD_FILE"];
                    }
                    Double iSize = Convert.ToDouble(blobFile.Length) / 1024;
                    LVFile.Items.Add(dsTemp.Tables[0].Rows[i]["FILE_NAME"].ToString());
                    LVFile.Items[LVFile.Items.Count - 1].Name = dsTemp.Tables[0].Rows[i]["FILE_NAME"].ToString();
                    LVFile.Items[LVFile.Items.Count - 1].SubItems.Add(iSize.ToString("0.00"));
                }
            }
        }

        private void DeleteImgFile(string sFileName)
        {
            if (g_sflag == "Y")
            {
                sFileName = Path.GetFileName(sFileName);
                sSQL = " DELETE SAJET.G_IQC_LOT_IMAGE "
                     + "  WHERE LOT_NO =:LOT_NO "
                     + "    AND FILE_NAME =:FILE_NAME ";
                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sRecID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FILE_NAME", sFileName };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            else
            {
                sFileName = Path.GetFileName(sFileName);
                sSQL = " DELETE SAJET.G_IQC_NOTES_IMAGE "
                     + "  WHERE RECID =:RECID "
                     + "    AND FILE_NAME =:FILE_NAME ";
                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sRecID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FILE_NAME", sFileName };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
        }

        private bool InsertImgFile(string sRECID, string sFile)
        {
            string sMessage = "";
            string sFileSize = SajetCommon.GetSysBaseData("IQC", "Upload File Size", ref sMessage);
            int iFileSize = 1024;
            try
            {
                iFileSize = Convert.ToInt32(sFileSize);
            }
            catch
            {
                iFileSize = 1024;
            }                
            string sFileName = Path.GetFileName(sFile);
            FileStream fs = new FileStream(sFile, FileMode.OpenOrCreate, FileAccess.Read);
            byte[] BlobFile = new byte[fs.Length];
            try
            {
                
                fs.Read(BlobFile, 0, System.Convert.ToInt32(fs.Length));
                int iFileSize1 =(int)(Convert.ToDouble(fs.Length) / (double)1024);
                //if (Convert.ToDouble(fs.Length) / 1024 / 1024 > 5)
                if (iFileSize1 > iFileSize)
               
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("File Name") + " : " + sFileName+Environment.NewLine
                                           + SajetCommon.SetLanguage("File Size")+ " : " + iFileSize1.ToString() + "(KB)"+Environment.NewLine+Environment.NewLine
                                           + SajetCommon.SetLanguage("Standard Size is") + " " + iFileSize.ToString() + "(KB)", 0);
                    return false;
                }
            } 
            finally
            {
                fs.Close();
            }

            if (g_sflag == "Y")
            {
                object[][] Params = new object[4][];
                sSQL = " INSERT INTO SAJET.G_IQC_LOT_IMAGE "
                     + " (LOT_NO,FILE_NAME,UPLOAD_FILE,UPDATE_USERID,UPDATE_TIME ) "
                     + " VALUES "
                     + " (:LOT_NO,:FILE_NAME,:UPLOAD_FILE,:UPDATE_USERID,SYSDATE) ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sRECID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FILE_NAME", sFileName };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.Blob, "UPLOAD_FILE", BlobFile };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                return true;
            }
            else
            {
                object[][] Params = new object[4][];
                sSQL = " INSERT INTO SAJET.G_IQC_NOTES_IMAGE "
                     + " (RECID,FILE_NAME,UPLOAD_FILE,UPDATE_USERID,UPDATE_TIME ) "
                     + " VALUES "
                     + " (:RECID,:FILE_NAME,:UPLOAD_FILE,:UPDATE_USERID,SYSDATE) ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", sRECID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FILE_NAME", sFileName };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.Blob, "UPLOAD_FILE", BlobFile };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                return true;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LVFile.Items.Count == 0)
                return;
            if (LVFile.SelectedItems.Count == 0)
                return;
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete File") + " : " + LVFile.SelectedItems[0].SubItems[0].Text + " ?", 2) != DialogResult.Yes)
                return;

            DeleteImgFile(LVFile.SelectedItems[0].SubItems[0].Text);
            GetImgFile(g_sRecID);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void LVFile_Click(object sender, EventArgs e)
        {
            /*
            if (LVFile.Items.Count == 0)
                return;
            if (LVFile.SelectedItems.Count == 0)
                return;


            tableLayoutPanel1.BackgroundImage = null;

            string sFileName = LVFile.SelectedItems[0].SubItems[0].Text;
            string sFilePath = Application.StartupPath + "\\" + ClientUtils.fCurrentProject + "\\";
            DateTime dtStart = DateTime.Now;
            if (File.Exists(sFilePath + sFileName))
            {
                while (true)
                {
                    try
                    {
                        File.Delete(sFilePath + sFileName);
                        break;
                    }
                    catch
                    {
                        DateTime dtEnd = DateTime.Now;
                        if (dtStart.AddSeconds(5) < dtEnd)
                        {
                            SajetCommon.Show_Message("Display File Error", 0);
                            return;
                        }
                    }
                }
            }

            sSQL = "SELECT FILE_NAME,UPLOAD_FILE "
                       + " FROM SAJET.G_IQC_NOTES_IMAGE "
                       + " WHERE RECID =:RECID "
                       + "   AND FILE_NAME =:FILE_NAME ";

            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sRecID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FILE_NAME", sFileName };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                byte[] blobFile = new byte[0];
                if (!dsTemp.Tables[0].Rows[0]["UPLOAD_FILE"].ToString().Equals(""))
                {
                    blobFile = (byte[])dsTemp.Tables[0].Rows[0]["UPLOAD_FILE"];
                }
                File.WriteAllBytes(sFilePath + sFileName, blobFile);
                tableLayoutPanel1.BackgroundImage = Image.FromFile(sFilePath + sFileName);
            }
             */ 
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LVFile.Items.Count == 0)
                return;
            if (LVFile.SelectedItems.Count == 0)
                return;
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Download File") + " : " + LVFile.SelectedItems[0].SubItems[0].Text + " ?", 2) != DialogResult.Yes)
                return;
            DownloadFile(LVFile.SelectedItems[0].SubItems[0].Text);
        }

        private void DownloadFile(string sFileName)
        {
            if (g_sflag == "Y")
            {
                sSQL = "SELECT FILE_NAME,UPLOAD_FILE "
                     + " FROM SAJET.G_IQC_LOT_IMAGE "
                     + " WHERE LOT_NO =:LOT_NO "
                     + "   AND FILE_NAME =:FILE_NAME ";

                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sRecID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FILE_NAME", sFileName };

                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            else
            {
                sSQL = "SELECT FILE_NAME,UPLOAD_FILE "
                     + " FROM SAJET.G_IQC_NOTES_IMAGE "
                     + " WHERE RECID =:RECID "
                     + "   AND FILE_NAME =:FILE_NAME ";

                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sRecID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FILE_NAME", sFileName };

                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sFileName = dsTemp.Tables[0].Rows[0]["FILE_NAME"].ToString();
                byte[] blobFile = new byte[0];

                if (!dsTemp.Tables[0].Rows[0]["UPLOAD_FILE"].ToString().Equals(""))
                {
                    blobFile = (byte[])dsTemp.Tables[0].Rows[0]["UPLOAD_FILE"];
                }
                saveFileDialog1.FileName = sFileName;
                if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                    return;

                string sFile = saveFileDialog1.FileName;
                File.WriteAllBytes(sFile, blobFile);
                SajetCommon.Show_Message("Download File Finish", -1);
            }
        }

        private void viewPhotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
             if (LVFile.Items.Count == 0)
                return;         
                
            fViewPhoto fData = new fViewPhoto();
            try
            {
                fData.g_sLotno = g_sRecID;
                fData.g_sExeName = g_sExeName;               
                fData.ShowDialog();
            }
            finally
            {
                fData.Dispose();
            }

        }
    }
}