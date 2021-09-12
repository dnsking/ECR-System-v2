using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECR_System_v2.Utils;

namespace ECR_System_v2.Data
{
    public class Bank
    {
       public String Name { set; get; }
        public String AccountName { set; get; }
        public String AccountNumber { set; get; }
        public String Fund { set; get; }
        public String Date { set; get; }
        public Double Amount { set; get; }
        public String AmountFormated { get { return StringUtils.Format(Amount); } }

    }
}
