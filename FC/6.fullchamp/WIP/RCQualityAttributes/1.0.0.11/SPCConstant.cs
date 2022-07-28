using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;
namespace SPCConstantValue
{
    class SPCConstant
    {
        public static decimal A2;
        public static decimal D3;
        public static decimal D4;
        public static decimal A3;
        public static decimal B3;
        public static decimal B4;
        public static decimal d2;
        public static decimal c4;
        public static decimal E2; 
  
        public static void GetConstant(int iSample)
        {
            switch (iSample)
            {
                case 2:
                    A2 = decimal.Parse("1.880");
                    D3 = decimal.Parse("0");
                    D4 = decimal.Parse("3.267");
                    A3 = decimal.Parse("2.659");
                    B3 = decimal.Parse("0");
                    B4 = decimal.Parse("3.267");
                    d2 = decimal.Parse("1.128");
                    c4 = decimal.Parse("0.7979");
                    E2 = decimal.Parse("2.660");
                    break;
                case 3:
                    A2 = decimal.Parse("1.023");
                    D3 = decimal.Parse("0");
                    D4 = decimal.Parse("2.574");
                    A3 = decimal.Parse("1.954");
                    B3 = decimal.Parse("0");
                    B4 = decimal.Parse("2.568");
                    d2 = decimal.Parse("1.693");
                    c4 = decimal.Parse("0.8862");
                    E2 = decimal.Parse("1.772");
                    break;
                case 4:
                    A2 = decimal.Parse("0.729");
                    D3 = decimal.Parse("0");
                    D4 = decimal.Parse("2.282");
                    A3 = decimal.Parse("1.628");
                    B3 = decimal.Parse("0");
                    B4 = decimal.Parse("2.266");
                    d2 = decimal.Parse("2.059");
                    c4 = decimal.Parse("0.9213");
                    E2 = decimal.Parse("1.457");
                    break;
                case 5:
                    A2 = decimal.Parse("0.577");
                    D3 = decimal.Parse("0");
                    D4 = decimal.Parse("2.114");
                    A3 = decimal.Parse("1.427");
                    B3 = decimal.Parse("0");
                    B4 = decimal.Parse("2.089");
                    d2 = decimal.Parse("2.326");
                    c4 = decimal.Parse("0.9400");
                    E2 = decimal.Parse("1.290");
                    break;
                case 6:
                    A2 = decimal.Parse("0.483");
                    D3 = decimal.Parse("0");
                    D4 = decimal.Parse("2.004");
                    A3 = decimal.Parse("1.287");
                    B3 = decimal.Parse("0.303");
                    B4 = decimal.Parse("1.970");
                    d2 = decimal.Parse("2.534");
                    c4 = decimal.Parse("0.9515");
                    E2 = decimal.Parse("1.184");
                    break;
                case 7:
                    A2 = decimal.Parse("0.419");
                    D3 = decimal.Parse("0.076");
                    D4 = decimal.Parse("1.924");
                    A3 = decimal.Parse("1.182");
                    B3 = decimal.Parse("0.118");
                    B4 = decimal.Parse("1.882");
                    d2 = decimal.Parse("2.704");
                    c4 = decimal.Parse("0.9594");
                    E2 = decimal.Parse("1.109");
                    break;
                case 8:
                    A2 = decimal.Parse("0.373");
                    D3 = decimal.Parse("0.136");
                    D4 = decimal.Parse("1.864");
                    A3 = decimal.Parse("1.099");
                    B3 = decimal.Parse("0.185");
                    B4 = decimal.Parse("1.815");
                    d2 = decimal.Parse("2.847");
                    c4 = decimal.Parse("0.9650");
                    E2 = decimal.Parse("1.054");
                    break;
                case 9:
                    A2 = decimal.Parse("0.337");
                    D3 = decimal.Parse("0.184");
                    D4 = decimal.Parse("1.816");
                    A3 = decimal.Parse("1.032");
                    B3 = decimal.Parse("0.239");
                    B4 = decimal.Parse("1.761");
                    d2 = decimal.Parse("2.97");
                    c4 = decimal.Parse("0.9693");
                    E2 = decimal.Parse("1.010");
                    break;
                case 10:
                    A2 = decimal.Parse("0.308");
                    D3 = decimal.Parse("0.223");
                    D4 = decimal.Parse("1.777");
                    A3 = decimal.Parse("0.975");
                    B3 = decimal.Parse("0.284");
                    B4 = decimal.Parse("1.716");
                    d2 = decimal.Parse("3.078");
                    c4 = decimal.Parse("0.9727");
                    E2 = decimal.Parse("0.975");
                    break;
                case 11:
                    A2 = decimal.Parse("0.285");
                    D3 = decimal.Parse("0.256");
                    D4 = decimal.Parse("1.744");
                    A3 = decimal.Parse("0.927");
                    B3 = decimal.Parse("0.321");
                    B4 = decimal.Parse("1.679");
                    d2 = decimal.Parse("3.173");
                    c4 = decimal.Parse("0.9754");
                    E2 = decimal.Parse("0.946");
                    break;
                case 12:
                    A2 = decimal.Parse("0.266");
                    D3 = decimal.Parse("0.283");
                    D4 = decimal.Parse("1.717");
                    A3 = decimal.Parse("0.886");
                    B3 = decimal.Parse("0.354");
                    B4 = decimal.Parse("1.646");
                    d2 = decimal.Parse("3.258");
                    c4 = decimal.Parse("0.9776");
                    E2 = decimal.Parse("0.921");
                    break;
                case 13:
                    A2 = decimal.Parse("0.249");
                    D3 = decimal.Parse("0.307");
                    D4 = decimal.Parse("1.693");
                    A3 = decimal.Parse("0.850");
                    B3 = decimal.Parse("0.382");
                    B4 = decimal.Parse("1.618");
                    d2 = decimal.Parse("3.336");
                    c4 = decimal.Parse("0.9794");
                    E2 = decimal.Parse("0.899");
                    break;
                case 14:
                    A2 = decimal.Parse("0.235");
                    D3 = decimal.Parse("0.328");
                    D4 = decimal.Parse("1.672");
                    A3 = decimal.Parse("0.817");
                    B3 = decimal.Parse("0.406");
                    B4 = decimal.Parse("1.594");
                    d2 = decimal.Parse("3.407");
                    c4 = decimal.Parse("0.9810");
                    E2 = decimal.Parse("0.881");
                    break;
                case 15:
                    A2 = decimal.Parse("0.223");
                    D3 = decimal.Parse("0.347");
                    D4 = decimal.Parse("1.653");
                    A3 = decimal.Parse("0.789");
                    B3 = decimal.Parse("0.428");
                    B4 = decimal.Parse("1.572");
                    d2 = decimal.Parse("3.472");
                    c4 = decimal.Parse("0.9823");
                    E2 = decimal.Parse("0.864");
                    break;
                case 16:
                    A2 = decimal.Parse("0.212");
                    D3 = decimal.Parse("0.363");
                    D4 = decimal.Parse("1.637");
                    A3 = decimal.Parse("0.763");
                    B3 = decimal.Parse("0.448");
                    B4 = decimal.Parse("1.552");
                    d2 = decimal.Parse("3.532");
                    c4 = decimal.Parse("0.9835");
                    E2 = decimal.Parse("0.849");
                    break;
                case 17:
                    A2 = decimal.Parse("0.203");
                    D3 = decimal.Parse("0.378");
                    D4 = decimal.Parse("1.622");
                    A3 = decimal.Parse("0.739");
                    B3 = decimal.Parse("0.466");
                    B4 = decimal.Parse("1.534");
                    d2 = decimal.Parse("3.588");
                    c4 = decimal.Parse("0.9845");
                    E2 = decimal.Parse("0.836");
                    break;
                case 18:
                    A2 = decimal.Parse("0.194");
                    D3 = decimal.Parse("0.391");
                    D4 = decimal.Parse("1.608");
                    A3 = decimal.Parse("0.718");
                    B3 = decimal.Parse("0.482");
                    B4 = decimal.Parse("1.518");
                    d2 = decimal.Parse("3.640");
                    c4 = decimal.Parse("0.9854");
                    E2 = decimal.Parse("0.824");
                    break;
                case 19:
                    A2 = decimal.Parse("0.187");
                    D3 = decimal.Parse("0.403");
                    D4 = decimal.Parse("1.597");
                    A3 = decimal.Parse("0.698");
                    B3 = decimal.Parse("0.497");
                    B4 = decimal.Parse("1.503");
                    d2 = decimal.Parse("3.689");
                    c4 = decimal.Parse("0.9862");
                    E2 = decimal.Parse("0.813");
                    break;
                case 20:
                    A2 = decimal.Parse("0.180");
                    D3 = decimal.Parse("0.415");
                    D4 = decimal.Parse("1.585");
                    A3 = decimal.Parse("0.680");
                    B3 = decimal.Parse("0.510");
                    B4 = decimal.Parse("1.490");
                    d2 = decimal.Parse("3.735");
                    c4 = decimal.Parse("0.9869");
                    E2 = decimal.Parse("0.803");
                    break;
                case 21:
                    A2 = decimal.Parse("0.173");
                    D3 = decimal.Parse("0.425");
                    D4 = decimal.Parse("1.575");
                    A3 = decimal.Parse("0.663");
                    B3 = decimal.Parse("0.523");
                    B4 = decimal.Parse("1.477");
                    d2 = decimal.Parse("3.778");
                    c4 = decimal.Parse("0.9876");
                    E2 = decimal.Parse("0.794");
                    break;
                case 22:
                    A2 = decimal.Parse("0.167");
                    D3 = decimal.Parse("0.434");
                    D4 = decimal.Parse("1.566");
                    A3 = decimal.Parse("0.647");
                    B3 = decimal.Parse("0.534");
                    B4 = decimal.Parse("1.466");
                    d2 = decimal.Parse("3.819");
                    c4 = decimal.Parse("0.9882");
                    E2 = decimal.Parse("0.785");
                    break;
                case 23:
                    A2 = decimal.Parse("0.162");
                    D3 = decimal.Parse("0.443");
                    D4 = decimal.Parse("1.557");
                    A3 = decimal.Parse("0.633");
                    B3 = decimal.Parse("0.545");
                    B4 = decimal.Parse("1.455");
                    d2 = decimal.Parse("3.858");
                    c4 = decimal.Parse("0.9887");
                    E2 = decimal.Parse("0.778");
                    break;
                case 24:
                    A2 = decimal.Parse("0.157");
                    D3 = decimal.Parse("0.451");
                    D4 = decimal.Parse("1.548");
                    A3 = decimal.Parse("0.619");
                    B3 = decimal.Parse("0.555");
                    B4 = decimal.Parse("1.445");
                    d2 = decimal.Parse("3.895");
                    c4 = decimal.Parse("0.9892");
                    E2 = decimal.Parse("0.770");
                    break;
                case 25:
                    A2 = decimal.Parse("0.153");
                    D3 = decimal.Parse("0.459");
                    D4 = decimal.Parse("1.541");
                    A3 = decimal.Parse("0.606");
                    B3 = decimal.Parse("0.565");
                    B4 = decimal.Parse("1.435");
                    d2 = decimal.Parse("3.931");
                    c4 = decimal.Parse("0.9896");
                    E2 = decimal.Parse("0.763");
                    break;                
                default:
                    SajetCommon.Show_Message("Sample Size Range from 2 ~ 25", 0);
                    return;
                    break;
            }
        }

        public static void GetCapability(string sChartType, string sUCL, string sCL, string sLCL, decimal dAvgX, decimal dAvgR, decimal[] dRData,ref string[] slCapa)
        {
            //計算Ca,Cp,Cpk值            
            decimal dUSL = 0;
            decimal dSL = 0;
            decimal dLSL = 0;
            decimal Ca = 0;
            decimal Cp = 0;
            if (!string.IsNullOrEmpty(sUCL))
                dUSL = decimal.Parse(sUCL);
            if (!string.IsNullOrEmpty(sCL))
                dSL = decimal.Parse(sCL);
            if (!string.IsNullOrEmpty(sLCL))
                dLSL = decimal.Parse(sLCL);

            //Ca =(X|| - SL)/min(SL - LSL, USL - SL) 單邊規格時無Ca  
            if (!string.IsNullOrEmpty(sUCL) && !string.IsNullOrEmpty(sLCL))
            {
                decimal min = Math.Min(dSL - dLSL, dUSL - dSL);
                if (min == 0)
                {
                    slCapa = new string[] { "", "", "" };
                    return;
                }
                Ca = (dAvgX - dSL) / min;
            }
            
            //Cp =min((SL - LSL)/3σ,(USL - SL)/3σ)
            decimal S = decimal.Parse(MathFormul.Formula.GetStdDev(dRData, dAvgR).ToString());
            //decimal S = 0;
            //if (sChartType == "XBRC")
            //    S = dAvgR / SPCConstant.d2;
            //else if (sChartType == "XBSC")
            //    S = dAvgR / SPCConstant.c4;
            //else if (sChartType.Substring(0, 4) == "XRMC" && dRData.Length > 1)            
            //    S = decimal.Parse(MathFormul.Formula.GetStdDev(dRData, dAvgR).ToString());
            if (S == 0)
                S = 1;

            if (string.IsNullOrEmpty(sUCL))
                Cp = (dSL - dLSL) / (3 * S);
            else if (string.IsNullOrEmpty(sLCL))
                Cp = (dUSL - dSL) / (3 * S);
            else if (!string.IsNullOrEmpty(sUCL) && !string.IsNullOrEmpty(sLCL) && S != 0)
                Cp = Math.Min((dSL - dLSL) / (3 * S), (dUSL - dSL) / (3 * S));

            //Cpk = ( 1 - |Ca| ) * Cp	單邊規格時, Cpk = Cp            
            decimal Cpk = 0;
            if (string.IsNullOrEmpty(sUCL) || string.IsNullOrEmpty(sLCL))
                Cpk = Cp;
            else
                Cpk = (1 - Math.Abs(Ca)) * Cp;

            Array.Resize(ref slCapa, 3);
            if (!string.IsNullOrEmpty(sUCL) && !string.IsNullOrEmpty(sLCL))
                slCapa[0] = Ca.ToString();
            else
                slCapa[0] = "N/A";
            slCapa[1] = Cp.ToString();
            slCapa[2] = Cpk.ToString();
        }

        public static string GetCapaLevel(decimal dValue,string sType)
        {
            //等級
            string sLevel = "";
            switch (sType.ToUpper())
            {
                case "CA":
                    decimal CaTemp = Math.Abs(dValue * 100);
                    if (CaTemp <= decimal.Parse("12.5"))
                        sLevel = "A";
                    else if (CaTemp > decimal.Parse("12.5") && CaTemp <= decimal.Parse("25"))
                        sLevel = "B";
                    else if (CaTemp > decimal.Parse("25") && CaTemp <= decimal.Parse("50"))
                        sLevel = "C";
                    else if (CaTemp > decimal.Parse("50"))
                        sLevel = "D";
                    break;
                case "CP":
                    if (dValue >= decimal.Parse("1.33"))
                        sLevel = "A";
                    else if (dValue >= decimal.Parse("1.00") && dValue < decimal.Parse("1.33"))
                        sLevel = "B";
                    else if (dValue >= decimal.Parse("0.83") && dValue < decimal.Parse("1.00"))
                        sLevel = "C";
                    else if (dValue < decimal.Parse("0.83"))
                        sLevel = "D";
                    break;
                case "CPK":
                    if (dValue >= decimal.Parse("1.33"))
                        sLevel = "A";
                    else if (dValue >= decimal.Parse("1.00") && dValue < decimal.Parse("1.33"))
                        sLevel = "B";
                    else if (dValue < decimal.Parse("1.00"))
                        sLevel = "C";
                    break;
            }
            return sLevel;
        }        
    }
}
