using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR_System_v2.Utils
{
   public class MathUtils
    {
         public static String RemoveSciNotation(double value)
        {
            return value.ToString(".0" + new string('#', 399));
        }

        public static double round(double value, int places)
        {

            long factor = (long)Math.Pow(10, places);
            value = value * factor;
            double tmp = Math.Round(value);
            return tmp / factor;
        }


        /// <summary>
        /// Calculates Greatest common divisor
        /// </summary>
        public static int calGCD(int a, int b)
        {
            while (a != 0 && b != 0) // until either one of them is 0
            {
                int c = b;
                b = a % b;
                a = c;
            }
            return a + b; // either one is 0, so return the non-zero value
        }
        public static double GetEquityValue(long date, String name) {
            return 0;
        }
    }
}
