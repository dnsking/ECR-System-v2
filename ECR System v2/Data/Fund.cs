using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ECR_System_v2.Data
{
    public class Fund
    {
        [JsonProperty("Name")]
        public String Name { set; get; }

        [JsonProperty("HasClient")]
        public String HasClient { set; get; }

        [JsonProperty("Currency")]
        public String Currency { set; get; }

        [JsonProperty("UnitPriceFixed")]
        public String UnitPriceFixed { set; get; }

        [JsonProperty("UnitPriceFloating")]
        public String UnitPriceFloating { set; get; }

    }
}
