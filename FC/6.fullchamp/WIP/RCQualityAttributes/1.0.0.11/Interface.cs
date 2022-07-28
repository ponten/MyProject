using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using SajetClass;

public class Interface
{
    //public static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString
    //  + "User ID=" + new DBLogin().DBUser() + ";Password=" + new DBLogin().DBPassword();
    //public static OracleConnection connection = new OracleConnection(connectionString);
    public static Dictionary<string, string> tableData = new Dictionary<string, string>();
    public static string sRC_NO = string.Empty;

    public static void InitTable()
    {
        tableData.Add("1", "SAJET.G_SPC_XBRC,MEAN,RANGE");
        tableData.Add("2", "SAJET.G_SPC_XBSC,MEAN,SIGMA");
        //tableData.Add("3", "SAJET.G_SPC_XRMC,X,RM");
    }

    /// <summary>
    /// 以測試大項的抽驗數量作為SPC Monitor一組的數量單位
    /// </summary>
    /// <param name="iSize">抽驗數量 = 數量單位 </param>
    public static void MoveSPC(string sRC, int iSize)
    {
        // 圖表加入Process, 若有XRMC之資料, 不處理, 只將資料搬走 (由測試時自行填入)
        try
        {
            sRC_NO = sRC;
            //connection.Close();
            //connection.Open();

           // string sSQL = "SELECT A.ROWID, A.*,TO_CHAR(A.SPC_VALUE) SPCVALUE "
           //+ ",B.SAMPLE_SIZE, NVL(B.CHART_TYPE, -1) CHART_TYPE "
           //+ "FROM SAJET.G_SPC A, (SELECT B.SAMPLE_SIZE, B.CHART_TYPE, B.PART_ID, B.PROCESS_ID, C.SPC_ID "
           //+ "FROM SAJET.SYS_SPC_MASTER B, SAJET.SYS_SPC_DETAIL C "
           //+ "WHERE B.MASTER_ID = C.MASTER_ID) B "
           //+ "WHERE A.PART_ID = B.PART_ID(+) "
           //+ "AND A.PROCESS_ID = B.PROCESS_ID(+) AND A.SPC_ID = B.SPC_ID(+) "
           //+ "ORDER BY A.PART_ID,A.WORK_ORDER, A.SPC_ID, A.TERMINAL_ID, A.UPDATE_TIME";
            string sSQL = @"SELECT A.ROWID, A.*,TO_CHAR(A.SPC_VALUE) SPCVALUE               
                                           FROM SAJET.G_SPC A
                                          ORDER BY A.PART_ID,A.WORK_ORDER, A.SPC_ID, A.PROCESS_ID, A.UPDATE_TIME";
            DateTime startTime = DateTime.Now;
            DataSet ds = ClientUtils.ExecuteSQL(sSQL, null);
            string sModelField = "MODEL_ID";
            if (ds.Tables[0].Columns.Contains("MODEL_NAME"))
                sModelField = "MODEL_NAME";
            string sSPC = "", sPart = "", sProcess = "", sWO = "";
            int iCount = 0;
            decimal[] aData = new decimal[1];
            string[] sRow = new string[1];
            string sType = "", sMaxID = "";
            DateTime dtStartTime = new DateTime();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (sPart != dr["PART_ID"].ToString() || sSPC != dr["SPC_ID"].ToString() || sProcess != dr["PROCESS_ID"].ToString() || sWO != dr["WORK_ORDER"].ToString())
                {
                    aData = new decimal[iSize];
                    sRow = new string[iSize];
                    iCount = 0;
                    //sType = dr["CHART_TYPE"].ToString();
                }
                if (iCount == 0)
                    dtStartTime = (DateTime)dr["UPDATE_TIME"];
                sSPC = dr["SPC_ID"].ToString();
                sPart = dr["PART_ID"].ToString();
                sProcess = dr["PROCESS_ID"].ToString();
                sWO = dr["WORK_ORDER"].ToString();
                aData[iCount] = decimal.Parse(dr["SPCVALUE"].ToString());
                sRow[iCount] = dr["ROWID"].ToString();
                iCount++;
                if (iCount == iSize)
                {
                    sMaxID = GetRECID();
                    RemoveSPC(sRow, sMaxID);

                    decimal range = GetRange(aData);
                    decimal mean = GetAverage(aData);
                    // XBRC
                    InsertSPC("1", mean.ToString(), range.ToString(), dr, dtStartTime, sMaxID, sModelField);
                    // XBSC
                    double stddev = GetStdDev(aData, mean);
                    InsertSPC("2", mean.ToString(), string.Format("{0:0.0000}", stddev), dr, dtStartTime, sMaxID, sModelField);
                    // XRMC    
                    InsertSPC("3", dr, dtStartTime, sMaxID, sModelField);
                    //if ((sType == "1") || (sType == "2"))
                    //{
                    //    decimal range = GetRange(aData);
                    //    decimal mean = GetAverage(aData);
                    //    switch (sType)
                    //    {
                    //        case "1":
                    //            InsertSPC(sType, mean.ToString(), range.ToString(), dr, dtStartTime, sMaxID, sModelField);
                    //            break;
                    //        case "2":
                    //            double stddev = GetStdDev(aData, mean);
                    //            InsertSPC(sType, mean.ToString(), string.Format("{0:0.0000}", stddev), dr, dtStartTime, sMaxID, sModelField);
                    //            break;
                    //    }
                    //}
                    //else
                    //{
                    //    InsertSPC(sType, dr, dtStartTime, sMaxID, sModelField);
                    //}
                    iCount = 0;
                }
            }
        }
        catch (Exception ex)
        {
            try
            {
                object[][] Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEXE", "SPCInterface" };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMSG", ex.Message };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                ClientUtils.ExecuteProc("SAJET.SJ_INSERT_INTERFACE_ALARM", Params);
            }
            catch { }
        }
    }

    //public static void MoveSPC()
    //{
    //    // 圖表加入Process, 若有XRMC之資料, 不處理, 只將資料搬走 (由測試時自行填入)
    //    try
    //    {
    //        //connection.Close();
    //        //connection.Open();

    //        string sSQL = "SELECT A.ROWID, A.*,TO_CHAR(A.SPC_VALUE) SPCVALUE "
    //       + ",B.SAMPLE_SIZE, NVL(B.CHART_TYPE, -1) CHART_TYPE "
    //       + "FROM SAJET.G_SPC A, (SELECT B.SAMPLE_SIZE, B.CHART_TYPE, B.PART_ID, B.PROCESS_ID, C.SPC_ID "
    //       + "FROM SAJET.SYS_SPC_MASTER B, SAJET.SYS_SPC_DETAIL C "
    //       + "WHERE B.MASTER_ID = C.MASTER_ID) B "
    //       + "WHERE A.PART_ID = B.PART_ID(+) "
    //       + "AND A.PROCESS_ID = B.PROCESS_ID(+) AND A.SPC_ID = B.SPC_ID(+) "
    //       + "ORDER BY A.PART_ID,A.WORK_ORDER, A.SPC_ID, A.TERMINAL_ID, A.UPDATE_TIME";

    //        DateTime startTime = DateTime.Now;
    //        DataSet ds = ClientUtils.ExecuteSQL(sSQL, null);
    //        string sModelField = "MODEL_ID";
    //        if (ds.Tables[0].Columns.Contains("MODEL_NAME"))
    //            sModelField = "MODEL_NAME";
    //        string sSPC = "", sPart = "", sTerminal = "", sWO = "";
    //        int iCount = 0;
    //        decimal[] aData = new decimal[1];
    //        string[] sRow = new string[1];
    //        string sType = "", sMaxID = "";
    //        DateTime dtStartTime = new DateTime();
    //        foreach (DataRow dr in ds.Tables[0].Rows)
    //        {
    //            if (dr["CHART_TYPE"].ToString() == "-1")
    //            {
    //                sRow = new string[1];
    //                sRow[0] = dr["ROWID"].ToString();
    //                RemoveSPC(sRow, "");
    //            }
    //            else
    //            {
    //                //Lot Control SAMPLE_SIZE設為1, int iSize = int.Parse(dr["SAMPLE_SIZE"].ToString());
    //                int iSize = 1;
    //                if (sPart != dr["PART_ID"].ToString() || sSPC != dr["SPC_ID"].ToString() || sTerminal != dr["TERMINAL_ID"].ToString() || sWO != dr["WORK_ORDER"].ToString())
    //                {
    //                    aData = new decimal[iSize];
    //                    sRow = new string[iSize];
    //                    iCount = 0;
    //                    sType = dr["CHART_TYPE"].ToString();
    //                }
    //                if (iCount == 0)
    //                    dtStartTime = (DateTime)dr["UPDATE_TIME"];
    //                sSPC = dr["SPC_ID"].ToString();
    //                sPart = dr["PART_ID"].ToString();
    //                sTerminal = dr["TERMINAL_ID"].ToString();
    //                sWO = dr["WORK_ORDER"].ToString();
    //                aData[iCount] = decimal.Parse(dr["SPCVALUE"].ToString());
    //                sRow[iCount] = dr["ROWID"].ToString();
    //                iCount++;
    //                if (iCount == iSize)
    //                {
    //                    sMaxID = GetRECID();
    //                    RemoveSPC(sRow, sMaxID);
    //                    if ((sType == "1") || (sType == "2"))
    //                    {
    //                        decimal range = GetRange(aData);
    //                        decimal mean = GetAverage(aData);
    //                        switch (sType)
    //                        {
    //                            case "1":
    //                                InsertSPC(sType, mean.ToString(), range.ToString(), dr, dtStartTime, sMaxID, sModelField);
    //                                break;
    //                            case "2":
    //                                double stddev = GetStdDev(aData, mean);
    //                                InsertSPC(sType, mean.ToString(), string.Format("{0:0.0000}", stddev), dr, dtStartTime, sMaxID, sModelField);
    //                                break;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        InsertSPC(sType, dr, dtStartTime, sMaxID, sModelField);
    //                    }
    //                    iCount = 0;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        try
    //        {
    //            object[][] Params = new object[3][];
    //            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEXE", "SPCInterface" };
    //            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMSG", ex.Message };
    //            Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
    //            ClientUtils.ExecuteProc("SAJET.SJ_INSERT_INTERFACE_ALARM", Params);
    //        }
    //        catch { }
    //    }
    //}
//    public static void MoveSPC()
//    {
//        // 圖表加入Process, 若有XRMC之資料, 不處理, 只將資料搬走 (由測試時自行填入)
//        try
//        {
//            //connection.Close();
//            //connection.Open();
//            string sSQL = @"SELECT A.ROWID, A.*,TO_CHAR(A.SPC_VALUE) SPCVALUE               
//                                            FROM SAJET.G_SPC A
//                                            ORDER BY A.PART_ID,A.WORK_ORDER, A.SPC_ID, A.TERMINAL_ID, A.UPDATE_TIME";

//            DateTime startTime = DateTime.Now;
//            DataSet ds = ClientUtils.ExecuteSQL(sSQL, null);
//            string sModelField = "MODEL_ID";
//            //if (ds.Tables[0].Columns.Contains("MODEL_NAME"))
//            //    sModelField = "MODEL_NAME";
//            string sSPC = "", sPart = "", sWO = "";   //  sTerminal = "",
//            int iCount = 0;
//            decimal[] aData = new decimal[1];
//            string[] sRow = new string[1];
//            string sType = "", sMaxID = "";
//            DateTime dtStartTime = new DateTime();
//            foreach (DataRow dr in ds.Tables[0].Rows)
//            {
//                //Lot Control SAMPLE_SIZE設為1
//                int iSize = 1;
//                if (sPart != dr["PART_ID"].ToString() || sSPC != dr["SPC_ID"].ToString() || sWO != dr["WORK_ORDER"].ToString())
//                {
//                    aData = new decimal[iSize];
//                    sRow = new string[iSize];
//                    iCount = 0;
//                    //sType = dr["CHART_TYPE"].ToString();
//                }
//                if (iCount == 0)
//                    dtStartTime = (DateTime)dr["UPDATE_TIME"];
//                sSPC = dr["SPC_ID"].ToString();
//                sPart = dr["PART_ID"].ToString();
//                sWO = dr["WORK_ORDER"].ToString();
//                aData[iCount] = decimal.Parse(dr["SPCVALUE"].ToString());
//                sRow[iCount] = dr["ROWID"].ToString();

//                sMaxID = GetRECID();
//                RemoveSPC(sRow, sMaxID);

//                decimal range = GetRange(aData);
//                decimal mean = GetAverage(aData);
//                // XBRC
//                InsertSPC("1", mean.ToString(), range.ToString(), dr, dtStartTime, sMaxID, sModelField);
//                // XBSC
//                double stddev = GetStdDev(aData, mean);
//                InsertSPC("2", mean.ToString(), string.Format("{0:0.0000}", stddev), dr, dtStartTime, sMaxID, sModelField);
//                // XRMC    
//                InsertSPC("3", dr, dtStartTime, sMaxID, sModelField);

//            }
//        }
//        catch (Exception ex)
//        {
//            try
//            {
//                object[][] Params = new object[3][];
//                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEXE", "SPCInterface" };
//                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMSG", ex.Message };
//                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
//                ClientUtils.ExecuteProc("SAJET.SJ_INSERT_INTERFACE_ALARM", Params);
//            }
//            catch { }
//        }
//    }

    private static void RemoveSPC(string[] sRow, string sMaxID)
    {
        object[][] Params = new object[3][];
        foreach (string sRowId in sRow)
        {
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TROWID", sRowId };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRECID", sMaxID };
            Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            ClientUtils.ExecuteProc("SAJET.SJ_GSPC_TO_RSPC", Params);
        }
    }
    private static string GetRECID()
    {
        string sID = "0";
        string sSQL = "Select TO_CHAR(SYSDATE,'YYYYMMDD') || LPAD(SAJET.SPC_RECID.NEXTVAL, 5,'0') SNID "
             + "From DUAL ";
        DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, null);
        sID = dsTemp.Tables[0].Rows[0]["SNID"].ToString();
        return sID;
    }
    private static decimal GetRange(decimal[] data)
    {
        decimal iMax = data[0], iMin = data[0];
        for (int i = 1; i < data.Length; i++)
        {
            if (iMax < data[i])
                iMax = data[i];
            if (iMin > data[i])
                iMin = data[i];
        }
        return iMax - iMin;
    }
    public static decimal GetAverage(decimal[] data)
    {
        int len = data.Length;
        if (len == 0)
            throw new Exception("No data");

        decimal sum = 0;
        for (int i = 0; i < data.Length; i++)
            sum += data[i];
        return sum / len;
    }
    public static double GetStdDev(decimal[] num, decimal avg)
    {
        double SumOfSqrs = 0;
        for (int i = 0; i < num.Length; i++)
        {
            SumOfSqrs += Math.Pow(((double)(num[i] - avg)), 2);
        }
        if (num.Length == 1)
            return Math.Sqrt(SumOfSqrs / num.Length);
        else
            return Math.Sqrt(SumOfSqrs / (num.Length - 1));
    }
    private static void InsertSPC(string sType, string sX1, string sX2, DataRow drRow, DateTime dtStartTime, string sMaxID, string sModelField)
    {
        string sList;
        tableData.TryGetValue(sType, out sList);
        string[] slList = sList.Split(',');
        object[][] Params = new object[14][];
        string sSQL = "INSERT INTO " + slList[0]
            + "(SPC_ID,UPDATE_TIME,PDLINE_ID,STAGE_ID,PROCESS_ID,TERMINAL_ID,WORK_ORDER,"
            + sModelField + ",PART_ID," + slList[1] + "," + slList[2] + ",END_TIME,RECID, RC_NO)"
            + "VALUES(:SPC_ID,:UPDATE_TIME,:PDLINE_ID,:STAGE_ID,:PROCESS_ID,:TERMINAL_ID,:WORK_ORDER,"
            + ":MODEL_ID,:PART_ID,:X1,:X2,:END_TIME,:RECID,:RC_NO)";
        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPC_ID", drRow["SPC_ID"] };
        Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtStartTime };
        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_ID", drRow["PDLINE_ID"] };
        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STAGE_ID", drRow["STAGE_ID"] };
        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", drRow["PROCESS_ID"] };
        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TERMINAL_ID", drRow["TERMINAL_ID"] };
        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", drRow["WORK_ORDER"] };
        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", drRow[sModelField] };
        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", drRow["PART_ID"] };
        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "X1", sX1 };
        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "X2", sX2 };
        Params[11] = new object[] { ParameterDirection.Input, OracleType.DateTime, "END_TIME", drRow["UPDATE_TIME"] };
        Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", sMaxID };
        Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC_NO };
        ClientUtils.ExecuteSQL(sSQL, Params);

        try
        {
            Params = new object[6][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "TDATE", dtStartTime };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TCHARTYPE", sType };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSPCID", drRow["SPC_ID"] };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPARTID", drRow["PART_ID"] };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", drRow["TERMINAL_ID"] };
            Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            ClientUtils.ExecuteProc("SAJET.DYNAKACP_SPC_CPK", Params);
        }
        catch(Exception exp) 
        {
 
        }

    }
    private static void InsertSPC(string sType, DataRow drRow, DateTime dtStartTime, string sMaxID, string sModelField)
    {
        object[][] Params = new object[14][];
        string sSQL = "INSERT INTO SAJET.G_SPC_XRMC "
            + "(SPC_ID,UPDATE_TIME,PDLINE_ID,STAGE_ID,PROCESS_ID,TERMINAL_ID,"
            + "WORK_ORDER," + sModelField + ",PART_ID,END_TIME,SERIAL_NUMBER,EMP_ID,X,RECID)"
            + "VALUES(:SPC_ID,:UPDATE_TIME,:PDLINE_ID,:STAGE_ID,:PROCESS_ID,:TERMINAL_ID,"
            + ":WORK_ORDER,:MODEL_ID,:PART_ID,:END_TIME,:SERIAL_NUMBER,:EMP_ID,:X,:RECID)";
        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPC_ID", drRow["SPC_ID"] };
        Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtStartTime };
        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_ID", drRow["PDLINE_ID"] };
        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STAGE_ID", drRow["STAGE_ID"] };
        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", drRow["PROCESS_ID"] };
        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TERMINAL_ID", drRow["TERMINAL_ID"] };
        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", drRow["WORK_ORDER"] };
        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", drRow[sModelField] };
        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", drRow["PART_ID"] };
        Params[9] = new object[] { ParameterDirection.Input, OracleType.DateTime, "END_TIME", drRow["UPDATE_TIME"] };
        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", drRow["SERIAL_NUMBER"] };
        Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_ID", drRow["EMP_ID"] };
        Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "X", drRow["SPCVALUE"] };
        Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", sMaxID };
        ClientUtils.ExecuteSQL(sSQL, Params);
        try
        {
            Params = new object[6][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "TDATE", dtStartTime };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TCHARTYPE", sType };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSPCID", drRow["SPC_ID"] };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPARTID", drRow["PART_ID"] };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", drRow["TERMINAL_ID"] };
            Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            ClientUtils.ExecuteProc("SAJET.DYNAKACP_SPC_CPK", Params);
        }
        catch { }
    }

    public static object editRC { get; set; }
}
