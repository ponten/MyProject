using System;
using System.Collections.Generic;
using System.Text;

namespace MathFormul
{
    class Formula
    {
        private static decimal GetRange(decimal[] data)
        {
            //衡程籔程ぇ畉
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
            //衡キА
            int len = 0;//data.Length;
            if (data.Length == 0)
            {
                //throw new Exception("No data");
                return 0;
            }

            decimal sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (!string.IsNullOrEmpty(data[i].ToString()))
                {
                    sum += data[i];
                    len++;
                }
            }
            return sum / len;
        }
        public static double GetStdDev(decimal[] num, decimal avg)
        {
            //夹非畉
            double SumOfSqrs = 0;
            for (int i = 0; i < num.Length; i++)
            {
                SumOfSqrs += Math.Pow(((double)(num[i] - avg)), 2);
            }
            return Math.Sqrt(SumOfSqrs / (num.Length - 1));
        }
        public static double GetMedian(decimal[] pNumbers)
        {
            //い计(璝案计,玥い丁ㄢ计キА)
            int size = pNumbers.Length;
            int mid = size / 2;
            double median = (size % 2 != 0) ? (double)pNumbers[mid] :
            ((double)pNumbers[mid] + (double)pNumbers[mid - 1]) / 2;
            return median;
        }
        public static double GetMax(decimal[] data)
        {
            //程    
            double dMaxValue = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (i == 0)
                    dMaxValue = double.Parse(data[i].ToString());
                else
                    dMaxValue = Math.Max(dMaxValue, double.Parse(data[i].ToString()));
            }
            return dMaxValue;
        }
        public static double GetMin(decimal[] data)
        {
            //程    
            double dMinValue = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (i == 0)
                    dMinValue = double.Parse(data[i].ToString());
                else
                    dMinValue = Math.Min(dMinValue, double.Parse(data[i].ToString()));
            }
            return dMinValue;
        }
        public static decimal[] GetSort(decimal[] data)
        {
            //パ逼            
            Array.Sort(data);            
            /*
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 1; j < data.Length; j++)
                {
                    if (data[j - 1] > data[j])
                    {
                        decimal temp = data[j - 1];
                        data[j - 1] = data[j];
                        data[j] = temp;
                    }                    
                }
            }
             */ 
            return data;
        }
        public static double GetPercent(decimal[] data, double dPercent)
        {
            //κだ计
            double dResultValue = 0;
            int iIndex = 0;
            
            int iNo = data.Length;
            double d = (iNo - 1) * dPercent + 1;            
            bool bIntType = int.TryParse(d.ToString(), out iIndex);
            if (bIntType) //俱计
            {
                dResultValue = double.Parse(data[iIndex - 1].ToString());
            }
            else
            {
                int iIndex1 = Convert.ToInt32(Math.Truncate(d));
                
                //0.25*A+(1-0.25)*B
                decimal dValue = decimal.Parse(dPercent.ToString()) * decimal.Parse(data[iIndex1 - 1].ToString()) + decimal.Parse(Convert.ToString(1 - dPercent)) * decimal.Parse(data[iIndex1].ToString());
                dResultValue = double.Parse(dValue.ToString());                
            }            
            return dResultValue;
        }
    }
    
}
