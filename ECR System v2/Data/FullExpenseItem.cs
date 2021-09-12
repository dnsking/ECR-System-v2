using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECR_System_v2.Utils;

namespace ECR_System_v2.Data
{
    public class ExpenseItem
    {
        public String Fund { set; get; }
        public String Expense { set; get; }
        public long Time { set; get; }
        public Double OpeningBalance { set; get; }
        public Double ClosingBalance { set; get; }
        public Double Payments { set; get; }
        public Double Provisions { set; get; }

        public String OpeningBalanceFormated { get { return StringUtils.Format(OpeningBalance); } }
        public String ClosingBalanceFormated { get { return StringUtils.Format(ClosingBalance); } }
        public String PaymentsFormated { get { return StringUtils.Format(Payments); } }
        public String ProvisionsFormated { get { return StringUtils.Format(Provisions); } }

    }
}
