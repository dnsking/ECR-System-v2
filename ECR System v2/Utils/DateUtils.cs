using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR_System_v2.Utils
{
    public class DateUtils
    {

        public static int LONG = 1;
        public static int SHORT = 1;
        public static readonly String[] monthsLong = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public static readonly String[] monthsShort = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        public static readonly String[] quatersName = {"First Quater", "Second Quater", "Third Quater", "Forth Quater" };
       
        public static String ReplaceDate(String date)
        {
            for (int i = 0; i < monthsShort.Length; i++)
            {
                if (date.ToLower().Contains(monthsShort[i].ToLower()))
                    return monthsLong[i];
            }
            return date;
        }
        public static Boolean IsLastFriday(DateTime DateCheck) {
            var Date = new DateTime(DateCheck.Year, DateCheck.Month, 1).AddMonths(1).AddDays(-1);
            while (Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday)
            {
                Date = Date.AddDays(-1);
            }
            return DateCheck.Day == Date.Day;
        }
        public static DateTime GetLastFriday(DateTime DateCheck)
        {
            DateTime Date = new DateTime(DateCheck.Year, DateCheck.Month, 1).AddMonths(1).AddDays(-1);
            while (Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday)
            {
                Date = Date.AddDays(-1);
            }
            return Date;
        }
        public static String FindDate(String text) {
            String textLower = text.ToLower();
            String[] spiltTextArray = textLower.Split(' ');
            foreach (String spiltText in spiltTextArray) {
                
                foreach (String month in monthsShort)
                {

                    if (spiltText.Contains("-" + month.ToLower() + "-") || spiltText.Contains(" " + month.ToLower() + " "))
                    {
                        return spiltText.ToUpper().Replace(System.Environment.NewLine, "").Replace("INSTRUMENT","").Replace(month.ToUpper(), char.ToUpper(month[0]) + month.Substring(1).ToLower());
                    }
                }
                
                foreach (String month in monthsLong)
                {
                    if (spiltText.Contains("-" + month.ToLower() + "-")|| spiltText.Contains(" " + month.ToLower() + " ")) {
                   
                        return spiltText.ToUpper().Replace(System.Environment.NewLine, "").Replace("INSTRUMENT", "").Replace(month.ToUpper(), char.ToUpper(month[0]) + month.Substring(1).ToLower());
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Returns number of days between from (in millis) and to (in millis)
        /// </summary>
        public static int NumberOfDaysBetween(long from, long to)
        {
            DateTime c = new DateTime(MillisToTicks(from));

            DateTime k = new DateTime(MillisToTicks(to));
            
            return daysBetween(c, k);
        }
        public static int daysBetween(DateTime d1, DateTime d2)
        {
            return (int)((d2.Ticks - d1.Ticks) / (TimeSpan.TicksPerMillisecond* 1000 * 60 * 60 * 24));
        }
        public static int daysBetweenWeekdaysOnly(DateTime dtmStart, DateTime dtmEnd)
        {
            int dowStart = ((int)dtmStart.DayOfWeek == 0 ? 7 : (int)dtmStart.DayOfWeek);
            int dowEnd = ((int)dtmEnd.DayOfWeek == 0 ? 7 : (int)dtmEnd.DayOfWeek);
            TimeSpan tSpan = dtmEnd - dtmStart;
            if (dowStart <= dowEnd)
            {
                return (((tSpan.Days / 7) * 5) + Math.Max((Math.Min((dowEnd + 1), 6) - dowStart), 0));
            }
            return (((tSpan.Days / 7) * 5) + Math.Min((dowEnd + 6) - Math.Min(dowStart, 6), 5));

        }
        public static int DaysInMonth(DateTime date) {

            DateTime firstDay = new DateTime(date.Year, date.Month, 1);
            return firstDay.AddMonths(1).AddDays(-1).Day;
        }
        public static int DaysInQuater( DateTime date) {
            int quarterNumber = (date.Month - 1) / 3 + 1;
            DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
            DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3);
            return daysBetween(firstDayOfQuarter, lastDayOfQuarter);
        }
        public static DateTime FirstDayOfQuater(DateTime date)
        {
            int quarterNumber = (date.Month - 1) / 3 + 1;
            DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
            return firstDayOfQuarter;
        }

        public static DateTime LastDayOfQuater(DateTime date)
        {
            int quarterNumber = (date.Month - 1) / 3 + 1;
            DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
            DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3);
            return lastDayOfQuarter;
        }
        public static int DaysInQuaterWeekdaysOnly(DateTime date)
        {
            int quarterNumber = (date.Month - 1) / 3 + 1;
            DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
            DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3);
            return daysBetweenWeekdaysOnly(firstDayOfQuarter, lastDayOfQuarter);
        }
        /// <summary>
        /// Returns number of quaters between dt1 and dt2
        /// </summary>
        public static long GetQuarters(DateTime dt1, DateTime dt2)
        {
            double d1Quarter = GetQuarter(dt1.Month);
            double d2Quarter = GetQuarter(dt2.Month);
            double d1 = d2Quarter - d1Quarter;
            double d2 = (4 * (dt2.Year - dt1.Year));
            return Round(d1 + d2);
        }

        public static int GetQuarter(int nMonth)
        {
            if (nMonth <= 3)
                return 1;
            if (nMonth <= 6)
                return 2;
            if (nMonth <= 9)
                return 3;
            return 4;
        }

        private static long Round(double dVal)
        {
            if (dVal >= 0)
                return (long)Math.Floor(dVal);
            return (long)Math.Ceiling(dVal);
        }

        /// <summary>
        /// Returns quater of date
        /// </summary>
        public static int getQuarter(DateTime date)
        {
            if (date.Month >= 1 && date.Month <= 3)
            {
                return 0;
            }
            else if (date.Month >= 4 && date.Month <= 6)
            {
                return 1;
            }
            if (date.Month >= 7 && date.Month <= 9)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        
        public static long MillisToTicks(long time)
        {
            return time * TimeSpan.TicksPerMillisecond;
        }
        public static long TicksToMillis(long time)
        {

            return time / TimeSpan.TicksPerMillisecond;
        }
        public static long TicksToSecs(long time)
        {

            return time / TimeSpan.TicksPerSecond;
        }
        /// <summary>
        /// Compares whether date com is between a and b
        /// </summary>
        /// <param name="a">
        /// lower date in milliseconds
        /// </param>
        /// <param name="b">higher date in milliseconds</param>
        /// <param name="com">date in milliseconds to test</param>
        /// <returns>true wether com is between a and b, false otherwise</returns>
        public static bool isDateBetween(long a, long b, long com)
        {
            DateTime c = new DateTime(MillisToTicks(a));
            DateTime k = new DateTime(MillisToTicks(b));
            DateTime y = new DateTime(MillisToTicks(com));

            return isDateBefore(com, b) && isDateBefore(com, a);

        }

        /// <summary>
        /// Returns whether date is before comDate
        /// </summary>
        public static bool isDateBefore(long date, long comDate)
        {
            DateTime c = new DateTime(MillisToTicks(date));

            DateTime k = new DateTime(MillisToTicks(comDate));
            return c.Day <= k.Day && c.Month <= k.Month && c.Year <= k.Year;
        }

        /// <summary>
        /// Returns whether date is month before comDate
        /// </summary>
        public static bool isMonthBefore(long date, long comDate)
        {
            DateTime c = new DateTime(MillisToTicks(date));
            DateTime k = new DateTime(MillisToTicks(comDate));
            return c.Month <= k.Month && c.Year <= k.Year;
        }

        /// <summary>
        /// Returns whether date's month is the same as comDate
        /// </summary>
        public static bool isMonthSame(long date, long comDate)
        {
            DateTime c = new DateTime(MillisToTicks(date));
            DateTime k = new DateTime(MillisToTicks(comDate));

            return c.Month == k.Month && c.Year == k.Year; ;
        }

        /// <summary>
        /// Returns number of months between date and comDate
        /// </summary>
        public static int isMonthMonthDiff(long date, long comDate)
        {
            DateTime c = new DateTime(MillisToTicks(date));
            DateTime k = new DateTime(MillisToTicks(comDate));
            return c.Month - k.Month;
        }

        /// <summary>
        /// Returns whether date year is the same as comDate
        /// </summary>
        public static bool isYearSame(long date, long comDate)
        {
            DateTime c = new DateTime(MillisToTicks(date));
            DateTime k = new DateTime(MillisToTicks(comDate));

            return c.Year == k.Year;
        }

        public static bool isDateSame(long date, long comDate)
        {

            DateTime c = new DateTime(MillisToTicks(date));
            DateTime k = new DateTime(MillisToTicks(comDate));
            return c.Day == k.Day && c.Month == k.Month && c.Year == k.Year;
        }

        /// <summary>
        /// Returns date in millis from string format
        /// </summary>
        public static String ParseDateInverse(String date)
        {
            DateTime dateTime = DateTime.Parse(date);
            return TicksToMillis(dateTime.Ticks).ToString();

        }

        /// <summary>
        /// Returns date in millis from string format
        /// </summary>
        public static String ParseDateInverseNumbers(String date)
        {
            return ParseDateInverse(date);

        }

        /// <summary>
        /// Returns date in millis from string format
        /// </summary>
        public static String ParseDate(String date)
        {
            return ParseDateInverse(date);

        }


        /// <summary>
        /// Returns year from format
        /// </summary>
        public static String getYear(String date)
        {

            DateTime dateTime = DateTime.Parse(date);
            return dateTime.Year.ToString();
        }

        /// <summary>
        /// Returns formted date <param name="date" >time in millis</param>
        /// </summary>
        public static String getDate(long date, int style, bool containsDash)
        {
            String separator = (containsDash) ? "-" : " ";
            DateTime c = new DateTime(MillisToTicks(date));
            String monthStye= (style== LONG) ? "MMMM" : "MMM";
            return c.Day + separator + c.ToString(monthStye) + separator + c.Year;

        }
        public static String getDateYear(long date)
        {
            DateTime c = new DateTime(MillisToTicks(date));
            return c.Year.ToString();

        }
        public static String getDateNumber(long date, int style, bool containsDash)
        {
            String separator = (containsDash) ? "-" : " ";
            DateTime c = new DateTime(MillisToTicks(date));
            return c.Day + separator + c.Month + separator + c.Year;

        }

        public static String getDateNumber(long date, int style)
        {
            String separator = "/";
            DateTime c = new DateTime(MillisToTicks(date));
            String monthStye = (style == LONG) ? "MMMM" : "MMM";
            return c.Day + separator + c.ToString(monthStye) + separator + c.Year;

        }
        public static String getMonthYear(long date, int style, bool containsDash)
        {
            String separator = (containsDash) ? "-" : " ";
            DateTime c = new DateTime(MillisToTicks(date));
            String monthStye = (style == LONG) ? "MMMM" : "MMM";
            return  c.ToString(monthStye) + separator + c.Year;

        }
    }
}
