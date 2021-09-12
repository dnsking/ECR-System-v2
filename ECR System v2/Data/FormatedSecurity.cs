using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR_System_v2.Data
{
    public class FormatedSecurity
    {
        public FormatedSecurity(Security mSecurity) {
            Name = mSecurity.Name;
            Name = mSecurity.Name;
            Name = mSecurity.Name;
            Name = mSecurity.Name;
            Name = mSecurity.Name;
        }
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

    }
}
