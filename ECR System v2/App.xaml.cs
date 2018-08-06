using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ECR_System_v2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Boolean IsDebug = true;
        public static String URL = "https://2bfe6vyg88.execute-api.us-east-1.amazonaws.com/EcrSys";

        public static int ErrorReponseCode = 0;
        public static int SuccessReponseCode = 1;
        public static String AccountExistsReponseValue = "AccountExists";
        public static String AccountCreatedReponseValue = "AccountCreated";

        public static class Types
        {
            public static String PropertyType = "Property";
            public static String GovernmentBondType         = "Government Bond";
            public static String GovernmentTreasuryBillType = "Government Treasury Bill";
            public static String ListedEquityType = "Listed Equity";
            public static String UnlistedEquityType = "Unlisted Equity";
            public static String CISType = "CIS";
            public static String OtherInvestmentsType = "Other Investments";
        }
        public static readonly int Open = 0;
        public static readonly int Closed = 1;
        
        public static readonly int Purchase = 0;
        public static readonly int Redemption = 1;
        public static readonly int All = -1;

        public static readonly String[] TransactionTypes = new String[] { "Purchase", "Redemption" };

        public static readonly String AllClients = "All";

        public static readonly String LuseIndex = "LASI";
        public static String[] Expenses = new String[] { "Management Fees", "Trustees Fees","Audit Fees"
        , "Custodial Fees", "Bank Activity Fees and TT charges", "Issuer Fees","Colletive Insvestment Scheme (CIS) Levy"};
    }
}
