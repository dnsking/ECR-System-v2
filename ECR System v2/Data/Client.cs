using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR_System_v2.Data
{
    public class Client
    {

        public String Fund { set; get; }
        public String ClientName { set; get; }
        public String ClientPhysicalAdress { set; get; }
        public String ClientEmailAdress { set; get; }
        public Double DateCreated { set; get; }
        public int Open { set; get; }
        public override string ToString()
        {
            return ClientName;
        }
    }
}
