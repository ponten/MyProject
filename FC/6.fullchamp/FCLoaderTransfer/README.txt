========== 版本號：1.0.0.4 ========== 2019.11.28
1.調整圖形介面操作
2.記錄 Log 文件

========== 版本號：1.0.0.3 ========== 2019.11.22
1.不做自動過站，一律透過 SajetMES 手動報工
2.改負責收集下料段生產資料：
  工單、重量、時間、IN/OK/NG、往12000T/往8000T

========== 版本號：1.0.0.2 ========== 2019/10/22
1.製程代碼：I010、A020、I020、I030。
2.加入下料段自動過站。
3.新資料表 G_PLC_AUTOWIP_NG_WEIGHT   :記錄下料重量異常
           G_PLC_AUTOWIP_CUTTING_QTY :記錄鋸床加工數量
4.中介表 G_PLC_AUTOWIP_IO_TEMP 增加欄位記錄途程ID、節點ID、NG重量以應付下料段需求。

========== 版本號：1.0.0.1 ========== 2019/05/29
1.製程代碼：PB015、PB020、PB025、PB030。
2.機台代碼：12000T：F005；
            8000T：F001。
3.更新 DB 連線名稱、帳號、密碼。
4.DBScript：
    G_PLC_AUTOWIP_ERR_LOG、G_PLC_AUTOWIP_ERR_LOG-IDSEQ 
    和 G_PLC_AUTOWIP_IO_TEMP 要在 ORACLE 執行；
    SYS_MES_PLC 要在 SQL Server 執行。

========== 版本號：1.0.0.0 ========== 2019/05/22
Windows Service : PLC 自動過站功能

Windows Service 上線前還需要設定的參數：
A.Service1.cs
    1.製程 ID
    2.機台代碼
    3.程式執行的間隔頻率 Time Interval
B.App.config
    1.FCLoaderTransfer_Log.txt 的路徑
    2.DB 連線（帳號密碼等等）
C.專案屬性
    1.版本號
    2.建置路徑
    3.專案輸出類型

安裝 Windows Service：
    1.以系統管理員身分執行「適用於 VS 2017 的開發人員命令提示字元」
    2.前往 exe 檔所在目錄
    3.安裝服務，輸入 installutil FCLoaderTransfer.exe
    4.要解除安裝的話，輸入 installutil /u FCLoaderTransfer.exe
參考：https://docs.microsoft.com/zh-tw/dotnet/framework/windows-services/how-to-install-and-uninstall-services