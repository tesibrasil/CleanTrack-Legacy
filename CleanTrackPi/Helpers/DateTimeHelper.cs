using System;

namespace CleanTrackPi.Helpers
{
    public  class DateTimeHelper
    {
        public static string ConvertDateTime(DateTime dtConvert)
        {
            return dtConvert.Year.ToString("D4") + 
                dtConvert.Month.ToString("D2") + 
                dtConvert.Day.ToString("D2") + 
                dtConvert.Hour.ToString("D2") + 
                dtConvert.Minute.ToString("D2") + 
                dtConvert.Second.ToString("D2");
        }
        public static string ConvertDateTime(string strConvert)
        {
            if (strConvert.Length != 14)
                return "";

            return strConvert.Substring(6, 2) + "/" + 
                strConvert.Substring(4, 2) + "/" + 
                strConvert.Substring(0, 4) + " " + 
                strConvert.Substring(8, 2) + ":" + 
                strConvert.Substring(10, 2);
        }
    }
}

