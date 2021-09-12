using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECR_System_v2.Utils
{
   public class StringUtils
    {
        public static Boolean isStringAllCaps(String input) {
            return input.All(c => char.IsUpper(c));
        }
        public static String KeepOnlyDate(String number)
        {
            return Regex.Replace(number,"[^\\/0123456789]", "");
        }
        public static String KeepOnlyLetters(String str)
        {

            return Regex.Replace(str,"[^A-Za-z]+", "");
        }

        public static String RemoveNumbers(String str)
        {
            return Regex.Replace(str, @"[\d-]", "");
        }
        public static String KeepOnlyNumbers(String number)
        {
            return Regex.Replace(number,"[^\\.0123456789]", "");
        }

        public static bool ContainsNumbers(String str)
        {
        return Regex.Replace(str, "[^\\.0123456789]", "").Length>0;
        }

        public static bool ContainsLetters(String str)
        {
        return Regex.IsMatch(str, ".*[a-zA-Z]+.*");
        }
        public static String escapeSql(String str)
        {
            if (str == null)
            {
                return null;
            }
            return str.Replace("'", "''");
        }
        public static String TrimStartAndEnd(String value)
        {
            value = TrimStart(value, " ");
            return TrimEnd(value, " ");
        }
        public static string TrimStart( string target, string trimChars)
        {
            return target.TrimStart(trimChars.ToCharArray());
        }
        public static string TrimEnd( string target, string trimChars)
        {
            return target.TrimEnd(trimChars.ToCharArray());
        }
        public static String Escape(String value)
        {
            return value.Replace(@",", "").Replace(@"'", "").Replace("\"", "").Replace(@":", "").Replace(@"$", "").Replace(@"@", "").Replace(@"#", "").Replace(@"^", "").Replace(@"*", "");
        }
        public static String getWhereClauseForDate(long date)
        {
            String prefix = "";
            if (DateUtils.getDate(date, DateUtils.SHORT, true).Split('-')[0].Length == 1)
                prefix = "0";
            /*  if (useYear)
                  return " WHERE DATE LIKE '%" + prefix + DateUtils.getDateYear(date) + "'";*/
              return " WHERE DATE = '" + prefix + DateUtils.getDate(date, DateUtils.SHORT, true) + "' OR DATE = '" + prefix + DateUtils.getDate(date, DateUtils.LONG, true)
                      + "' OR DATE = '" + prefix + DateUtils.getDate(date, DateUtils.SHORT, false) + "' OR DATE = '" + prefix + DateUtils.getDate(date, DateUtils.LONG, false) + "'"
                      + " OR DATE = '" + DateUtils.getDate(date, DateUtils.SHORT, true) + "' OR DATE = '" + DateUtils.getDate(date, DateUtils.LONG, true)
                      + "' OR DATE = '" + DateUtils.getDate(date, DateUtils.SHORT, false) + "' OR DATE = '" + DateUtils.getDate(date, DateUtils.LONG, false) + "'";
          }

          public static String getWhereClauseForDateAlt(long date)
          {
              String prefix = "";
              if (DateUtils.getDate(date, DateUtils.SHORT, true).Split('-')[0].Length == 1)
                  prefix = "0";
            /*  if (useYear)
                  return " WHERE DATE LIKE '%" + prefix + DateUtils.getDateYear(date) + "'";*/
            return " WHERE DATE = '" + prefix + DateUtils.getDate(date, DateUtils.SHORT, true) + "' OR DATE = '" + prefix + DateUtils.getDate(date, DateUtils.LONG, true)
                    + "' OR DATE = '" + prefix + DateUtils.getDate(date, DateUtils.SHORT, false) + "' OR DATE = '" + prefix + DateUtils.getDate(date, DateUtils.LONG, false) + "'";
        }
        public static String getWhereClauseForDateNumber(long date)
        {
            String prefix = "";
            if (DateUtils.getDate(date, DateUtils.SHORT, true).Split('-')[0].Length == 1)
                prefix = "0";
          /*  if (useYear)
                return " WHERE DATE LIKE '%" + prefix + DateUtils.getDateYear(date) + "'";*/

            return " WHERE DATE = '" + prefix + DateUtils.getDateNumber(date, DateUtils.SHORT) + "' OR DATE = '" + prefix + DateUtils.getDateNumber(date, DateUtils.LONG)
                    + " OR DATE = '" + DateUtils.getDateNumber(date, DateUtils.SHORT) + "' OR DATE = '" + DateUtils.getDateNumber(date, DateUtils.LONG);
        }

        public static String ToFirstLetterUpper(String word)
        {

            return word.Substring(0, 1).ToUpper() + word.Substring(1, word.Length-1);
        }
        public static String Format(double value)
        {
            return String.Format("{0:0,0.00}", value);
        }
    }
}
