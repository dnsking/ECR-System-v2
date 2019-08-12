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

            var result = await client.PostAsync("/addsecurity/", formData);

            string resultContent = await result.Content.ReadAsStringAsync();

            ReturnResult resultConten = JsonConvert.DeserializeObject<ReturnResult>(resultContent);

            if (resultConten.ReponseCode == App.SuccessReponseCode)
                notifyDataEntryListeners(mSecurity.GetType(), JsonConvert.SerializeObject(mSecurity));

            return resultConten;
        }
        public async Task<ReturnResult> AddFund(Fund mFund)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(JsonConvert.SerializeObject(mFund)), "fund");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.PostAsync("/addfund/", formData);

            string resultContent = await result.Content.ReadAsStringAsync();

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

            var result = await client.PostAsync("/addexpense/", formData);

            string resultContent = await result.Content.ReadAsStringAsync();

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

            var result = await client.GetAsync("/shares/" + nDays);

            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine("resultContent " + resultContent);

            Share[] resultConten = JsonConvert.DeserializeObject<Share[]>(resultContent);

            return resultConten;
        }
        public async Task<Fund[]> fetchFunds()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.GetAsync("/getfunds/");

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                Fund[] resultConten = JsonConvert.DeserializeObject<Fund[]>(resultContent);

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

            var result = await client.GetAsync("/getsecuritiesAll/" + fundName);

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                Security[] resultConten = JsonConvert.DeserializeObject<Security[]>(resultContent);

                return resultConten;
            }
            else
            {
                return new Security[] { };
            }
        }
        public async Task<Double[]> fetchSecuritiesPresentValueRange(String fundName, long start, long end, int days)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.GetAsync("/getsecuritiesPresentValueRangeTotal/" + fundName + "/" + start + "/" + end + "/" + days);

            string resultContent = await result.Content.ReadAsStringAsync();
            ////Console.WriteLine(resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
                Double[] resultConten = JsonConvert.DeserializeObject<Double[]>(resultContent);

                return resultConten;
            }
            else
            {
                return new Double[] { };
            }
        }
        public async Task<Double[]> fetchSecuritiesPresentValueRangeType(String fundName, long start, long end, int days, String type)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.GetAsync("/getsecuritiesPresentValueRangeTotalType/" + fundName + "/" + start + "/" + end + "/" + days + "/" + type);

            string resultContent = await result.Content.ReadAsStringAsync();

            if (resultContent != null & resultContent.Length > 0)
            {
                Double[] resultConten = JsonConvert.DeserializeObject<Double[]>(resultContent);

                return resultConten;
            }
            else
            {
                return new Double[] { };
            }
        }
        public async Task<Double> fetchSecuritiesPresentValueType(String fundName, long date, String type)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.GetAsync("/getsecuritiesPresentValueTotalWithType/" + fundName + "/" + date + "/" + type);

            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine("getsecuritiesPresentValueTotalWithType "+resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
                Double resultConten = JsonConvert.DeserializeObject<Double>(resultContent);

                return resultConten;
            }
            else
            {
                return 0.0;
            }
        }

        public async Task<Double[]> fetchSecuritiesPresentValueType(String fundName, long date, String[] types)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var formData = new MultipartFormDataContent();
            foreach (String type in types)
            {

                formData.Add(new StringContent(type), "fundtype");
            }

            var result = await client.PostAsync("/getsecuritiesPresentValueTotalAll/" + fundName + "/" + date, formData);

            string resultContent = await result.Content.ReadAsStringAsync();

            Console.WriteLine("getsecuritiesPresentValueTotalAll " + resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {

                return JsonConvert.DeserializeObject<Double[]>(resultContent);
            }
            else
            {
                return new Double[] { };
            }
        }
        /*
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
        }*/
        /*
        public async Task<ReturnResult> AddFund(Fund mFund) {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(JsonConvert.SerializeObject(mFund)), "fund");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.PostAsync("" ,
                new StringContent("{\"Action\":\"addfund\",\"fund\":" + JsonConvert.SerializeObject(mFund) + "}"));

            string resultContent = await result.Content.ReadAsStringAsync();
            //Console.WriteLine("AddFund " + resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
            }

            ReturnResult resultConten = JsonConvert.DeserializeObject<ReturnResult>(resultContent);

            if (resultConten.ReponseCode == App.SuccessReponseCode)
                notifyDataEntryListeners(mFund.GetType(), JsonConvert.SerializeObject(mFund));

            return resultConten;
        }*/
        /*
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
        }*/
        /*
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
        }*/
        public async Task<Share[]> fetchSharesFor(String nDays,String name)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            

            var result = await client.GetAsync("/sharesfor/"+nDays+"/"+ name);



            string resultContent = await result.Content.ReadAsStringAsync();
           /* if (resultContent != null & resultContent.Length > 0)
            {
                resultContent = resultContent.Substring(1, resultContent.Length - 2);
                resultContent = resultContent.Replace(@"\", "");
            }*/
            Share[] resultConten = JsonConvert.DeserializeObject< List<Share>>(resultContent).ToArray();

            return resultConten;
        }
        public async Task<Double[]> fetchSharesForRange(String name, long start, long end, int days)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();



            var result = await client.GetAsync("/sharesForRange/" + name+"/"+ start+"/"+end+"/"+ days);

            string resultContent = await result.Content.ReadAsStringAsync();
            ////Console.WriteLine(resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
                //resultContent = resultContent.Substring(1, resultContent.Length - 2);
               // resultContent = resultContent.Replace(@"\", "");
                Double[] resultConten = JsonConvert.DeserializeObject< List<Double>>(resultContent).ToArray();

                return resultConten;
            }
            else
            {
                return new Double[] { };
            }
        }

       /* public async Task<Fund[]> fetchFunds()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.PostAsync("",
            new StringContent("{\"Action\":\"getfunds\" }"));


            
            string resultContent = await result.Content.ReadAsStringAsync();
            //resultContent = resultContent.Replace("System.Windows.Controls.ListBoxItem: ", "");
           
            //Console.WriteLine("fetchFunds " + resultContent);
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
        }*/
        /*
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
        }*/
        public async Task<Security[]> fetchSecurities(String fundName, long start, long end,String funtype)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            
            var result = await client.GetAsync("/getsecuritiesPresentValueFull/"+ fundName+"/"+ start+"/"+ end+"/"+ funtype);

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
               // resultContent = resultContent.Substring(1, resultContent.Length - 2);
               // resultContent = resultContent.Replace(@"\", "");
                Security[] resultConten = JsonConvert.DeserializeObject<List<Security>>(resultContent).ToArray();

                return resultConten;
            }
            else
            {
                return new Security[] { };
            }
        }
        /*
        public async Task<Double[]> fetchSecuritiesPresentValueRange(String fundName,long start,long end,int days)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getsecuritiesPresentValueRangeTotal\",\"fundname\":\"" + fundName + "\",\"startdate\":\"" + start + "\",\"enddate\":\"" + end + "\",\"gap\":\"" + days + "\"}"));


            
            string resultContent = await result.Content.ReadAsStringAsync();
           //Console.WriteLine("fetchSecuritiesPresentValueRange "+resultContent);
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
        }*/
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
            var result = await client.PostAsync("/getsecuritiesPresentValueRangeTotalTypeAll/"+ fundName+"/"+ start+"/"+ end+"/"+ days, formData);

            string resultContent = await result.Content.ReadAsStringAsync();

            
            if (resultContent != null & resultContent.Length > 0)
            {
              //  resultContent = resultContent.Substring(1, resultContent.Length - 2);
               // resultContent = resultContent.Replace(@"\", "");
                Double[][] resultConten = JsonConvert.DeserializeObject<List<Double[]>>(resultContent).ToArray();

                return resultConten;
            }
            else
            {
                return new Double[][] { };
            }
        }
        /*
        public async Task<Double> fetchSecuritiesPresentValueType(String fundName, long date, String type)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();

            var result = await client.PostAsync("", new StringContent("{\"Action\":\"getsecuritiesPresentValueTotal\",\"fundname\":\"" + fundName + "\",\"date\":\"" + date + "\",\"fundtype\":\"" + type + "\"}"));



            string resultContent = await result.Content.ReadAsStringAsync();
            ////Console.WriteLine(resultContent);
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
        }*/
        /*
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
            
            ////Console.WriteLine(resultContent);
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
        }*/
        public async Task<ItemValue[]> fetchBalances(String fundName, long date, long enddate,String fundType)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(App.URL);
            client.DefaultRequestHeaders.Accept.Clear();
            
            var result = await client.GetAsync("/getBalaneces/"+ fundName + "/" + fundType + "/"+ date+"/"+ enddate);


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
              //  resultContent = resultContent.Substring(1, resultContent.Length - 2);
              //  resultContent = resultContent.Replace(@"\", "");

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


            var result = await client.PostAsync("/getDetailedIncomeSche/"+ fundName+"/"+ date+"/"+ enddate, formData);


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
                //resultContent = resultContent.Substring(1, resultContent.Length - 2);
                //resultContent = resultContent.Replace(@"\", "");
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


            var result = await client.PostAsync("/getExpenseItems/"+ fundName+"/"+ date+"/"+ enddate, formData);



            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
               // resultContent = resultContent.Substring(1, resultContent.Length - 2);
               // resultContent = resultContent.Replace(@"\", "");
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


            var result = await client.PostAsync("/getIncomestatementAssetsAndExpenses/"+ fundName+"/"+ date+"/"+ enddate, formData);
            
            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
             //   resultContent = resultContent.Substring(1, resultContent.Length - 2);
              //  resultContent = resultContent.Replace(@"\", "");
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
            

            var result = await client.GetAsync("/getChangesInEquity/"+ fundName+"/"+ date+"/"+ enddate);


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
              //  resultContent = resultContent.Substring(1, resultContent.Length - 2);
              //  resultContent = resultContent.Replace(@"\", "");
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


            var result = await client.GetAsync("/getStatementOfFinancialPosition/"+ fundName+"/"+ date);


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
            //    resultContent = resultContent.Substring(1, resultContent.Length - 2);
            //    resultContent = resultContent.Replace(@"\", "");
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
          
            var result = await client.GetAsync("/getBankItems/"+ fundName+"/"+ date);


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
            //    resultContent = resultContent.Substring(1, resultContent.Length - 2);
            //    resultContent = resultContent.Replace(@"\", "");
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
            
            var result = await client.GetAsync("/getFundUnitClients/"+ fundName+"/"+ open);


            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
             //   resultContent = resultContent.Substring(1, resultContent.Length - 2);
             //   resultContent = resultContent.Replace(@"\", "");
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

            var result = await client.PostAsync("/addFundUnitClientItem/", formData);

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
           //     resultContent = resultContent.Substring(1, resultContent.Length - 2);
           //     resultContent = resultContent.Replace(@"\", "");
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
            var result = await client.GetAsync("/getFundUnitTransItems/" + fundName+"/"+ date+"/" + clientName  +"/"+ TransactionType);

            string resultContent = await result.Content.ReadAsStringAsync();
            //Console.WriteLine("fetchFundUnitTransItems");
            //Console.WriteLine(resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
             //   resultContent = resultContent.Substring(1, resultContent.Length - 2);
             //   resultContent = resultContent.Replace(@"\", "");
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

            var result = await client.GetAsync("/getFundUnitTransItemsValueRangeTotal/"+ fundName+"/"+ start+"/"+ end+"/"+ days + "/" + clientName + "/"+ TransactionType);

            
            string resultContent = await result.Content.ReadAsStringAsync();
            //Console.WriteLine("fetchFundUnitTransItemsValueRangeTotal");
            //Console.WriteLine(resultContent);
            if (resultContent != null & resultContent.Length > 0)
            {
               // resultContent = resultContent.Substring(1, resultContent.Length - 2);
               // resultContent = resultContent.Replace(@"\", "");
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

            var result = await client.PostAsync("/addFundUnitTransItem/", formData);

            string resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent != null & resultContent.Length > 0)
            {
               // resultContent = resultContent.Substring(1, resultContent.Length - 2);
               // resultContent = resultContent.Replace(@"\", "");

                return JsonConvert.DeserializeObject<ReturnResult>(resultContent);
            }
            else
            {
                return new ReturnResult() { };
            }
        }
        
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
        }
    }
}
