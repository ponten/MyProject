using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using Oracle.ManagedDataAccess.Client;

using Assistant;
using Models;
using System.Threading;

namespace FCLoaderTransfer
{
    public partial class FormMain : Form
    {
        private readonly string MsConn = ConfigurationManager.ConnectionStrings["MSSQL"].ConnectionString;

        private readonly string OraConn = ConfigurationManager.ConnectionStrings["FCMESDB"].ConnectionString;

        private System.Timers.Timer Timer { get; } = new System.Timers.Timer();

        /// <summary>
        /// GOOD_NG_FLAG 寫入資料庫的內容。
        /// </summary>
        private readonly string[] sGoodNG_Flag = new string[3]
        {
            "I",
            "G",
            "N"
        };

        readonly int[][] flag_cutting = new int[][]
        {
           new int[] { 1, 2, 3 },
           new int[] { 28, 29, 30 }
        };

        readonly int[] ng_weight = new int[] { 4, 31 };

        readonly int[] wo_cutting = new int[] { 12, 39 };

        readonly int[] line = new int[] { 12000, 8000 };

        private List<PdData> ListCutting;

        private int ID_cutting = 0;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            "** Form load.".WriteLog(Level.INFO);
            dataPreview.Rows.Add("下料資料解析", "Stop");
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            labelInfo.Visible = false;
            buttonStart.PerformClick();
        }

        #region 圖形介面操作
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (buttonStop.Enabled)
            {
                buttonStop.PerformClick();
            }
            "== Exit.".WriteLog(Level.INFO);
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                "Background mode.".WriteLog(Level.INFO);
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                notifyIcon1.Tag = string.Empty;
                notifyIcon1.ShowBalloonTip(3000);
                ContextMenuStripItemsCheck();
            }
        }

        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //如果目前是縮小狀態，才要回覆成一般大小的視窗
                "Form show.".WriteLog(Level.INFO);
                this.Show();
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = false;
            }
            // Activate the form.
            this.Activate();
            this.Focus();
        }

        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonStart.PerformClick();
            ContextMenuStripItemsCheck();
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonStop.PerformClick();
            ContextMenuStripItemsCheck();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (buttonStop.Enabled)
            {
                buttonStop.PerformClick();
            }
            this.Close();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            Timer.Elapsed += new ElapsedEventHandler(DataCutting);
            Timer.Interval = 3 * 1000;
            $">> Service start. Timer raised interval {Timer.Interval} ms.".WriteLog(Level.INFO);
            Timer.Start();

            dataPreview.Rows.Cast<DataGridViewRow>()
                .First(m => m.Cells["LoaderName"].Value.ToString() == "下料資料解析")
                .Cells["LoaderStatus"].Value = "Running";

            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
            labelInfo.Visible = false;
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            Timer.Stop();
            Timer.Elapsed -= new ElapsedEventHandler(DataCutting);

            dataPreview.Rows.Cast<DataGridViewRow>()
                .First(m => m.Cells["LoaderName"].Value.ToString() == "下料資料解析")
                .Cells["LoaderStatus"].Value = "Stop";

            buttonStart.Enabled = true;
            buttonStop.Enabled = false;

            $"<< Stop service.".WriteLog(Level.INFO);
        }
        #endregion

        private void DataCutting(object sender, ElapsedEventArgs e)
        {
            "Event raised, fetch data from table: PD_DATARECORD.".WriteLog(Level.INFO);
            try
            {
                string SQL;
                if (ID_cutting == 0)
                {
                    SQL = @"
WITH A AS
(
    SELECT DATARECORD_ID
          ,RANK() OVER (ORDER BY DATARECORD_ID DESC) RNK
      FROM SAJET.G_PLC_CUTTING_WEIGHT
)
SELECT A.DATARECORD_ID
  FROM A
 WHERE RNK <= 1
";
                    using (var conn = new OracleConnection(OraConn))
                    {
                        conn.Open();
                        ID_cutting = conn.QueryFirstOrDefault<int>(SQL);
                    }
                }

                SQL = @"
SELECT TOP (100)
       [DATARECORD_ID]
      ,[DATARECORD_MSG]
      ,[MONITOR_ID]
      ,[CREATE_DATETIME]
      ,[CREATE_USER_ID]
  FROM [FCWS_DB].[dbo].[PD_DATARECORD]
 WHERE [DATARECORD_ID]       > @DATARECORD_ID
   AND [MONITOR_ID]          = 2
   AND LEN([DATARECORD_MSG]) > 53
   AND LEN([DATARECORD_MSG]) < 55
";
                using (var conn = new SqlConnection(MsConn))
                {
                    conn.Open();
                    ListCutting = conn.Query<PdData>(SQL,
                        new
                        {
                            DATARECORD_ID = ID_cutting
                        }).ToList();

                    $"{ListCutting.Count} records fetched.".WriteLog(Level.INFO);

                    if (ListCutting.Count > 0)
                    {
                        $"Get DATARECORD_ID from {ListCutting[0].DATARECORD_ID} to {ListCutting[ListCutting.Count - 1].DATARECORD_ID}.".WriteLog(Level.INFO);
                        ParseDataCutting();
                    }
                }
            }
            catch (SqlException ex)
            {
                ShowMessage(title: "SQL Server conn failed.", exception: ex.Message);
                $"SQL Server conn failed. {ex.Message}".WriteLog(Level.ERROR);
            }
            catch (OracleException ex)
            {
                ShowMessage(title: "ORACLE conn failed.", exception: ex.Message);
                $"ORACLE conn failed. {ex.Message}".WriteLog(Level.ERROR);
            }
            catch (Exception ex)
            {
                ShowMessage(title: "Unexpected error occurred: conn", exception: ex.Message);
                $"Unexpected error occurred: conn. {ex.Message}".WriteLog(Level.ERROR);
            }
            "Event finished. Sleep.".WriteLog(Level.INFO);
        }

        private void ParseDataCutting()
        {
            var ListCW = new List<CuttingWeight>();

            try
            {
                "Parse data.".WriteLog(Level.INFO);
                foreach (var cutting in ListCutting)
                {
                    ID_cutting = (int)cutting.DATARECORD_ID;

                    // message
                    string message = cutting.DATARECORD_MSG;

                    for (int i = 0; i < 2; i++) // 12000 / 8000
                    {
                        string workOrder = RestructWO(message.Substring(wo_cutting[i] - 1, 16).Replace("\0", string.Empty).Replace("=", string.Empty));
                        int weight = Convert.ToInt32(message.Substring(ng_weight[i] - 1, 8), 16);

                        if (!string.IsNullOrWhiteSpace(workOrder))
                        {
                            for (int j = 0; j < 3; j++) // input / ok / ng
                            {
                                int.TryParse(message.Substring(flag_cutting[i][j] - 1, 1), out int flag);

                                if (flag == 1 || weight > 0)
                                {
                                    ListCW.Add(
                                        new CuttingWeight
                                        {
                                            DATARECORD_ID = cutting.DATARECORD_ID,
                                            WORK_ORDER = workOrder,
                                            LINE = line[i],
                                            STATUS_FLAG = sGoodNG_Flag[j],
                                            WEIGHT = weight,
                                            DATETIME = cutting.CREATE_DATETIME
                                        });
                                }
                            }
                        }
                        else
                        {
                            $"Line: {line[i]}, DATARECORD_ID: {cutting.DATARECORD_ID} did not find WORK_ORDER".WriteLog(Level.WARNING);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                $"Data format error. {ex.Message}".WriteLog(Level.ERROR);
            }

            try
            {
                "Write data to table: G_PLC_CUTTING_WEIGHT.".WriteLog(Level.INFO);
                using (var conn = new OracleConnection(OraConn))
                {
                    conn.Open();
                    if (ListCW.Count > 0)
                    {
                        string SQL = @"
INSERT INTO G_PLC_CUTTING_WEIGHT
(
    DATARECORD_ID,
    WORK_ORDER,
    LINE,
    STATUS_FLAG,
    WEIGHT,
    DATETIME
)
VALUES
(
    :DATARECORD_ID,
    :WORK_ORDER,
    :LINE,
    :STATUS_FLAG,
    :WEIGHT,
    :DATETIME
)";
                        conn.Execute(SQL, ListCW);
                    }
                    $"{ListCW.Count} records inserted.".WriteLog(Level.INFO);
                }
            }
            catch (OracleException ex)
            {
                ShowMessage(title: "ORACLE insert data failed.", exception: ex.Message);
                $"ORACLE insert data failed. {ex.Message}".WriteLog(Level.ERROR);
            }
            catch (Exception ex)
            {
                ShowMessage(title: "Unexpected error occurred: insert data", exception: ex.Message);
                $"Unexpected error occurred: insert data. {ex.Message}".WriteLog(Level.ERROR);
            }
        }

        /// <summary>
        /// 重組 PLC 得到的工單。工單格式和實際長得不太一樣。
        /// </summary>
        /// <param name="WO"></param>
        /// <returns></returns>
        private string RestructWO(string WO)
        {
            char cZero = '0';
            string[] sChop = WO.Split('-');
            if (sChop.Length == 4)
                //10A-201904-19-9 => 10A2019040019009
                return sChop[0] + sChop[1] + sChop[2].PadLeft(4, cZero) + sChop[3].PadLeft(3, cZero);
            else return WO;
        }

        /// <summary>
        /// 更新錯誤訊息到畫面上
        /// </summary>
        /// <param name="title"></param>
        /// <param name="exception"></param>
        public void ShowMessage(string title, string exception)
        {
            labelInfo.InvokeIfRequired(() =>
            {
                labelInfo.Visible = true;
                labelInfo.Text = $"{title}\n{DateTime.Now}\n{exception}";
            });
            buttonStop.InvokeIfRequired(() =>
            {
                buttonStop.PerformClick();
            });
            this.InvokeIfRequired(() =>
            {
                this.Height = labelInfo.Height + 170;
            });
        }

        /// <summary>
        /// 啟用 / 停用右鍵選單選項
        /// </summary>
        private void ContextMenuStripItemsCheck()
        {
            StartToolStripMenuItem.Enabled = buttonStart.Enabled;
            StopToolStripMenuItem.Enabled = buttonStop.Enabled;
        }
    }
}
