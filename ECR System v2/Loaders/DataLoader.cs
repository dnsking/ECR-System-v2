using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ECR_System_v2.Data;
using ECR_System_v2.Loaders.Listeners;
using Newtonsoft.Json;

namespace ECR_System_v2.Loaders
{
    public class DataLoader
    {
        private List<DataEntryListener> mDataEntryListeners = new List<DataEntryListener>();
        public void addDataEntryListeners(DataEntryListener mDataEntryListener) {
            mDataEntryListeners.Add(mDataEntryListener);
        }
        private void notifyDataEntryListeners(Type mType, String json)
        {
            foreach(DataEntryListener mDataEntryListener in mDataEntryListeners)
                mDataEntryListener.SingleEntry(mType, json);
            
        }

        public async Task<ReturnResult> AddSecurity(Security mSecurity)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(JsonConvert.SerializeObject(mSecurity)), "security");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();


            var result = await client.PostAsync("", new StringContent("{\"Action\":\"addsecurity\",\"security\":" + JsonConvert.SerializeObject(mSecurity)+"}"));

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
            }

            ReturnResult resultConten = JsonConvert.DeserializeObject<ReturnResult>(resultContent);

            if (resultConten.ReponseCode == App.SuccessReponseCode)
                notifyDataEntryListeners(mSecurity.GetType(), JsonConvert.SerializeObject(mSecurity));

            return resultConten;
        }
        public async Task<ReturnResult> AddFund(Fund mFund) {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(JsonConvert.SerializeObject(mFund)), "fund");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.PostAsync("" ,
                new StringContent("{\"Action\":\"addfund\",\"fund\":" + JsonConvert.SerializeObject(mFund) + "}"));

            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine("AddFund " + resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
            }

            ReturnResult resultConten = JsonConvert.DeserializeObject<ReturnResult>(resultContent);

            if (resultConten.ReponseCode == App.SuccessReponseCode)
                notifyDataEntryListeners(mFund.GetType(), JsonConvert.SerializeObject(mFund));

            return resultConten;
        }

        public async Task<ReturnResult> AddExpense(Expense mExpense)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(JsonConvert.SerializeObject(mExpense)), "expense");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.PostAsync("",
                 new StringContent("{\"Action\":\"addexpense\",\"fund\":" + JsonConvert.SerializeObject(mExpense) + "}"));

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
            }

            ReturnResult resultConten = JsonConvert.DeserializeObject<ReturnResult>(resultContent);

            if (resultConten.ReponseCode == App.SuccessReponseCode)
                notifyDataEntryListeners(mExpense.GetType(), JsonConvert.SerializeObject(mExpense));

            return resultConten;
        }
        public async Task<Share[]> fetchShares(String nDays)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            
            var result = await client.PostAsync("",
            new StringContent("{\"Action\":\"getShares\",\"timeframe\":\"" + nDays + "\""+ "}"));

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                Share[] resultConten = JsonConvert.DeserializeObject<List<Share>>(resultContent).ToArray();
                return resultConten;
            }
            else {

                return new Share[] { };
            }
        }
        public async Task<Share[]> fetchSharesFor(String nDays,String name)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            

            var result = await client.PostAsync("",
            new StringContent("{\"Action\":\"getSharesFor\",\"timeframe\":\"" + nDays + "\",\"name\":\"" + name + "\"}"));



            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
            }
            Share[] resultConten = JsonConvert.DeserializeObject< List<Share>>(resultContent).ToArray();

            return resultConten;
        }
        public async Task<Double[]> fetchSharesForRange(String name, long start, long end, int days)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();



            var result = await client.PostAsync("",
            new StringContent("{\"Action\":\"getSharesForRange\",\"name\":\"" + name + "\",\"startdate\":\"" + start + "\",\"enddate\":\"" + end + "\",\"gap\":\"" + days + "\"}"));

            string resultContent = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                Double[] resultConten = JsonConvert.DeserializeObject< List<Double>>(resultContent).ToArray();

                return resultConten;
            }
            else
            {
                return new Double[] { };
            }
        }

        public async Task<Fund[]> fetchFunds()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.PostAsync("",
            new StringContent("{\"Action\":\"getfunds\" }"));


            
            string resultContent = await result.Content.ReadAsStringAsync();
            //resultContent = resultContent.Replace("System.Windows.Controls.ListBoxItem: ", "");
           
            Console.WriteLine("fetchFunds " + resultContent);
            if (resultContent != null & resultContent.Length>0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                Fund[] resultConten = JsonConvert.DeserializeObject<Fund[]>(resultContent, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                return resultConten;
            }
            else
            {
                return new Fund[] { };
            }
        }

        public async Task<Security[]> fetchSecurities(String fundName)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.PostAsync("",new StringContent("{\"Action\":\"getsecuritiesAll\",\"fundname\":\"" + fundName + "\"" + "}"));
            
            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {  resultContent = resultContent.Substring(1, resultContent.Length - 2);
                    resultContent = resultContent.Replace(@"\", "");
                    Security[] resultConten = JsonConvert.DeserializeObject< List<Security>>(resultContent).ToArray();

                return resultConten;
            }
            else
            {
                return new Security[] { };
            }
        }
        public async Task<Security[]> fetchSecurities(String fundName, long start, long end,String funtype)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            
            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getsecuritiesPresentValueFull\",\"fundname\":\"" + fundName + "\",\"startdate\":\"" + start + "\",\"enddate\":\"" + end + "\",\"fundtype\":\"" + funtype + "\"}"));

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                Security[] resultConten = JsonConvert.DeserializeObject<List<Security>>(resultContent).ToArray();

                return resultConten;
            }
            else
            {
                return new Security[] { };
            }
        }
        public async Task<Double[]> fetchSecuritiesPresentValueRange(String fundName,long start,long end,int days)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getsecuritiesPresentValueRangeTotal\",\"fundname\":\"" + fundName + "\",\"startdate\":\"" + start + "\",\"enddate\":\"" + end + "\",\"gap\":\"" + days + "\"}"));


            
            string resultContent = await result.Content.ReadAsStringAsync();
           Console.WriteLine("fetchSecuritiesPresentValueRange "+resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                Double[] resultConten = JsonConvert.DeserializeObject<List<Double>>(resultContent).ToArray();

                return resultConten;
            }
            else
            {
                return new Double[] { };
            }
        }
        public async Task<Double[][]> fetchSecuritiesPresentValueRangeType(String fundName, long start, long end, int days,String[] types)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var formData = new MultipartFormDataContent();
            foreach (String type in types)
            {

                formData.Add(new StringContent(type), "fundtype");
            }
            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getsecuritiesPresentValueRangeTotalTypeAll\",\"fundname\":\"" + fundName + "\",\"startdate\":\"" + start + "\",\"enddate\":\"" + end + "\",\"fundtypes\":" + JsonConvert.SerializeObject(types) + "}"));

            string resultContent = await result.Content.ReadAsStringAsync();

            
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                Double[][] resultConten = JsonConvert.DeserializeObject<List<Double[]>>(resultContent).ToArray();

                return resultConten;
            }
            else
            {
                return new Double[][] { };
            }
        }
        public async Task<Double> fetchSecuritiesPresentValueType(String fundName, long date, String type)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getsecuritiesPresentValueTotal\",\"fundname\":\"" + fundName + "\",\"date\":\"" + date + "\",\"fundtype\":\"" + type + "\"}"));



            string resultContent = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                Double resultConten = JsonConvert.DeserializeObject<Double>(resultContent);

                return resultConten;
            }
            else
            {
                return  0.0;
            }
        }

        public async Task<Double[]> fetchSecuritiesPresentValueType(String fundName, long date, String[] types)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var formData = new MultipartFormDataContent();
            foreach(String type in types)
            {

                formData.Add(new StringContent(type), "fundtype");
            }
            
            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getsecuritiesPresentValueTotalTypeAll\",\"fundname\":\"" + fundName + "\",\"date\":\"" + date + "\",\"fundtypes\":" + JsonConvert.SerializeObject(types) + "}"));


            
            string resultContent = await result.Content.ReadAsStringAsync();
            
            //Console.WriteLine(resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                return JsonConvert.DeserializeObject< List<Double>>(resultContent).ToArray();
            }
            else
            {
                return new Double[]{ };
            }
        }
        public async Task<ItemValue[]> fetchBalances(String fundName, long date, long enddate,String fundType)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            
            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getBalaneces\",\"fundname\":\"" + fundName + "\",\"nStartDate\":\"" + date + "\",\"nEndDate\":\"" + enddate + "\",\"fundType\":\"" + fundType + "\"}"));


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");

                return JsonConvert.DeserializeObject< List<ItemValue>>(resultContent).ToArray();
            }
            else
            {
                return new ItemValue[] { };
            }
        }

        public async Task<ItemValue[]> fetchDetailedIncomeSche(String fundName, long date, long enddate, String[] types)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var formData = new MultipartFormDataContent();
            foreach (String type in types)
            {

                formData.Add(new StringContent(type), "fundtype");
            }


            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getDetailedIncomeSche\",\"fundname\":\"" + fundName + "\",\"nStartDate\":\"" + date + "\",\"nEndDate\":\"" + enddate + "\",\"fundtypes\":" + JsonConvert.SerializeObject(types) + "}"));


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                return JsonConvert.DeserializeObject< List<ItemValue>>(resultContent).ToArray();
            }
            else
            {
                return new ItemValue[] { };
            }
        }
        public async Task<ExpenseItem[]> fetchFundItems(String fundName, long date, long enddate, String[] expenses)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var formData = new MultipartFormDataContent();
            foreach (String type in expenses)
            {

                formData.Add(new StringContent(type), "expenses");
            }


            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getExpenseItems\",\"fundname\":\"" + fundName + "\",\"nStartDate\":\"" + date + "\",\"nEndDate\":\"" + enddate + "\",\"expenses\":" + JsonConvert.SerializeObject(expenses) + "}"));



            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                return JsonConvert.DeserializeObject< List<ExpenseItem>>(resultContent).ToArray();
            }
            else
            {
                return new ExpenseItem[] { };
            }
        }
        public async Task<ItemValue[]> fetchIncomestatementAssetsAndExpenses(String fundName, long date, long enddate, String[] types)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var formData = new MultipartFormDataContent();
            foreach (String type in types)
            {

                formData.Add(new StringContent(type), "fundtype");
            }


            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getIncomestatementAssetsAndExpenses\",\"fundname\":\"" + fundName + "\",\"nStartDate\":\"" + date + "\",\"nEndDate\":\"" + enddate + "\",\"fundtypes\":" + JsonConvert.SerializeObject(types) + "}"));
            
            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                return JsonConvert.DeserializeObject< List<ItemValue>>(resultContent).ToArray();
            }
            else
            {
                return new ItemValue[] { };
            }
        }

        public async Task<ItemValue[]> fetchChangesInEquity(String fundName, long date, long enddate)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            

            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getChangesInEquity\",\"fundname\":\"" + fundName + "\",\"nStartDate\":\"" + date + "\",\"nEndDate\":\"" + enddate + "\"}"));


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                return JsonConvert.DeserializeObject< List<ItemValue>>(resultContent).ToArray();
            }
            else
            {
                return new ItemValue[] { };
            }
        }
        public async Task<ItemValue[][]> fetchStatementOfFinancialPosition(String fundName, long date)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();


            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getStatementOfFinancialPosition\",\"fundname\":\"" + fundName + "\",\"date\":\"" + date  + "\"}"));


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                return JsonConvert.DeserializeObject< List< ItemValue[]>>(resultContent).ToArray();
            }
            else
            {
                return new ItemValue[][] { };
            }
        }
        public async Task<Bank[]> fetchBankItems(String fundName, long date)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
          
            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getBankItems\",\"fundname\":\"" + fundName + "\",\"date\":\"" + date + "\"}"));


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                return JsonConvert.DeserializeObject< List<Bank>>(resultContent).ToArray();
            }
            else
            {
                return new Bank[] { };
            }
        }

        public async Task<Client[]> fetchFundUnitClients(String fundName, long open)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            
            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getFundUnitClients\",\"fundname\":\"" + fundName + "\",\"openfrom\":\"" + open + "\"}"));


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                return JsonConvert.DeserializeObject< List<Client>>(resultContent).ToArray();
            }
            else
            {
                return new Client[] { };
            }
        }

        public async Task<ReturnResult> addFundUnitClients(Client mClient)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(JsonConvert.SerializeObject(mClient)), "Client");

            var result = await client.PostAsync("",
                 new StringContent("{\"Action\":\"addFundUnitClientItem\",\"Client\":" + JsonConvert.SerializeObject(mClient) + "}"));

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                return JsonConvert.DeserializeObject<ReturnResult>(resultContent);
            }
            else
            {
                return new ReturnResult() { };
            }
        }


        public async Task<FundUnitTrans[]> fetchFundUnitTransItems(String fundName, long date,String clientName,int TransactionType)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getFundUnitTransItems\",\"fundname\":\"" + fundName + "\",\"date\":\"" + date + "\",\"TransactionType\":\"" + TransactionType + "\",\"client\":\"" + clientName + "\"}"));

            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine("fetchFundUnitTransItems");
            Console.WriteLine(resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                return JsonConvert.DeserializeObject< List<FundUnitTrans>>(resultContent).ToArray();
            }
            else
            {
                return new FundUnitTrans[] { };
            }
        }

        public async Task<Double[]> fetchFundUnitTransItemsValueRangeTotal(String fundName, long start, long end, int days, String clientName, int TransactionType)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getFundUnitTransItemsValueRangeTotal\",\"fundname\":\"" + fundName + "\",\"startdate\":\"" + start + "\",\"enddate\":\"" + end + "\",\"gap\":\"" + days + "\",\"TransactionType\":\"" + TransactionType + "\",\"client\":\"" + clientName + "\"}"));

            
            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine("fetchFundUnitTransItemsValueRangeTotal");
            Console.WriteLine(resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
                Double[] resultConten = JsonConvert.DeserializeObject< List<Double>>(resultContent).ToArray();

                return resultConten;
            }
            else
            {
                return new Double[] { };
            }
        }

        public async Task<ReturnResult> addFundUnitTransItem(FundUnitTrans mFundUnitTrans)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(JsonConvert.SerializeObject(mFundUnitTrans)), "FundUnitTrans");

            var result = await client.PostAsync("",
                 new StringContent("{\"Action\":\"addFundUnitTransItem\",\"FundUnitTrans\":" + JsonConvert.SerializeObject(mFundUnitTrans) + "}"));

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");

                return JsonConvert.DeserializeObject<ReturnResult>(resultContent);
            }
            else
            {
                return new ReturnResult() { };
            }
        }
        /*
        public async Task<Token> login(Account mAccount)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(JsonConvert.SerializeObject(mAccount)), "account");
            var result = await client.PostAsync("/login", formData);

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {

                return JsonConvert.DeserializeObject<Token>(resultContent);
            }
            else
            {
                return null;
            }
        }
        public async Task<ReturnResult> addaccount(Account mAccount)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(JsonConvert.SerializeObject(mAccount)), "account");
            var result = await client.PostAsync("/addaccount", formData);

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {

                return JsonConvert.DeserializeObject<ReturnResult>(resultContent);
            }
            else
            {
                return null;
            }
        }*/

    }
}
