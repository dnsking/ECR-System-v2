using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECR_System_v2.Utils;

namespace ECR_System_v2.Data
{
    public class Security
    {
        public String Name { set; get; }
        public String Fund { set; get; }
        public Double Nshares { set; get; }
        public Double TransactionDate { set; get; }
        public String Type { set; get; }
        public Double SellDate { set; get; }
        public String Address { set; get; }
        public Double Value { set; get; }
        public Double DailyInterest { set; get; }
        public Double MaturityDate { set; get; }
        public Double CurrentValue { set; get; }
        public Double InterestReceived { set; get; }
        public Double AccuredInterest { set; get; }
        
        public Double EndValue { set; get; }
        public String EndValueFormated { get { return StringUtils.Format(EndValue); } }
        public String CurrentFormated { get { return StringUtils.Format(CurrentValue); } }
        public String InterestReceivedFormated { get { return StringUtils.Format(InterestReceived); } }
        public String AccuredInterestFormated { get { return StringUtils.Format(AccuredInterest); } }
        public String DateFormated { get { return new DateTime(DateUtils.MillisToTicks((long)TransactionDate)).ToString("dd MMM yyyy"); } }
        public String MaturityDateFormated { get { return new DateTime(DateUtils.MillisToTicks((long)MaturityDate)).ToString("dd MMM yyyy"); } }

    }
}
