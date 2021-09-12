using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECR_System_v2.Data;
using ECR_System_v2.Loaders;

namespace ECR_System_v2.Utils
{
    public class FundUnits
    {
        public static async Task<double> GetUnitPrice(String fundName, long mFirstDayOfQuater, long mNow)
        {
            DataLoader mDataLoader = new DataLoader();
            //  var mFirstDayOfQuater = DateUtils.TicksToMillis(DateUtils.FirstDayOfQuater(DateTime.Now).Ticks);
            //  var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);



            Double[] mValues = await mDataLoader.fetchSecuritiesPresentValueRange(fundName, mNow, mFirstDayOfQuater, 7) as Double[];

            var mFundUnitIssuesTrans = await mDataLoader.fetchFundUnitTransItems(fundName, mNow, App.AllClients, App.Purchase) as FundUnitTrans[];
            double issuesTransAmount = 0;
            double issuesTransUnits = 0;
            foreach (var mFundUnitIssuesTran in mFundUnitIssuesTrans)
            {
                issuesTransAmount += mFundUnitIssuesTran.Amount;
                issuesTransUnits += mFundUnitIssuesTran.Units;
            }


            var mFundUnitRedemptionTrans = await mDataLoader.fetchFundUnitTransItems(fundName, mNow, App.AllClients, App.Redemption) as FundUnitTrans[];
            double redemptionTransAmount = 0;
            double redemptionTransUnits = 0;
            foreach (var mFundUnitIssuesTran in mFundUnitRedemptionTrans)
            {
                redemptionTransAmount += mFundUnitIssuesTran.Amount;
                redemptionTransUnits += mFundUnitIssuesTran.Units;
            }


            Security[] mSecurities = await mDataLoader.fetchSecurities(fundName, mFirstDayOfQuater, mNow, App.Types.All);
            double totalSecValues = 0;
            foreach (Security mSecurity in mSecurities)
            {
                if (mSecurity.Nshares > 0)
                {
                    totalSecValues += (mSecurity.Nshares * mSecurity.Value);
                }
                else
                {

                    totalSecValues += (mSecurity.Value);
                }
            }
            Console.WriteLine("redemptionTransUnits " + redemptionTransUnits);
            Console.WriteLine("issuesTransUnits " + issuesTransUnits);
            Console.WriteLine("totalSecValues " + totalSecValues);
            Console.WriteLine("issuesTransAmount " + issuesTransAmount);
            if (totalSecValues >= issuesTransAmount)
            {
                return MathUtils.round((mValues[0] / (issuesTransUnits - Math.Abs(redemptionTransUnits))), 2);

            }
            else
            {
                return MathUtils.round(((mValues[0] + (issuesTransAmount - totalSecValues)) / (issuesTransUnits - Math.Abs(redemptionTransUnits))), 2);

            }
        }

    }
}
