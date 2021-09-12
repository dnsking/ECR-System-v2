using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECR_System_v2.Utils;

namespace ECR_System_v2.Data
{
    public class FundUnitTrans
    {
        public String Fund { set; get; }
        public String Client { set; get; }
        public int TransactionType { set; get; }
        public Double Amount { set; get; }
        public Double Units { set; get; }
        public long DateInMillis { set; get; }
        public String DateInMillisFormted { get { return
                 new DateTime( DateUtils.MillisToTicks(DateInMillis)).ToString("dd MMM yyyy"); } }

        public String TransactionTypeFormated {  get{ return App.TransactionTypes[TransactionType]; } }
        public String AmountFormated { get { return StringUtils.Format(Amount); } }

    }
}
