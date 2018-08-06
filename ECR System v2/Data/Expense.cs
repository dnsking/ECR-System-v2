using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR_System_v2.Data
{
    public class Expense
    {
        public String Fund { set; get; }
        public String ExpeName { set; get; }
        public String Manual { set; get; }
        public String Auto { set; get; }
        public String AutoRatio { set; get; }
        public String AutoEvery { set; get; }
    }
}
