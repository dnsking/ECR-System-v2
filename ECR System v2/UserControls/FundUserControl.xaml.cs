using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ECR_System_v2.Data;
using ECR_System_v2.IO;
using ECR_System_v2.Loaders;
using ECR_System_v2.Loaders.Listeners;
using ECR_System_v2.Utils;
using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using Newtonsoft.Json;
using Notifications.Wpf;

namespace ECR_System_v2.UserControls
{
    /// <summary>
    /// Interaction logic for FundUserControl.xaml
    /// </summary>
    public partial class FundUserControl : UserControl, DataEntryListener
    {
        private Fund mFund;

        private DataLoader mDataLoader;
        private Security[] mSecurities;
        private Boolean HaveBalancesLoaded = false;
        public FundUserControl()
        {
            InitializeComponent();
            DateSelector.SelectedDateChanged += (a, b) => {
                mCurrentDate = DateSelector.SelectedDate.Value; ;
                load(Fund mFund)
            };
        }
        private double issuesTransAmount;
        private double issuesTransUnits;
        private double redemptionTransAmount;
        private double redemptionTransUnits;
        private DateTime mCurrentDate;
        public async void load(Fund mFund) {
            if (mDataLoader == null)
            {
                mDataLoader = (Window.GetWindow(this) as MainWindow).getDataLoader();
                mDataLoader.addDataEntryListeners(this);
            }
            this.mFund = mFund;


            IssuesCurrency.Text = mFund.Currency.Substring(mFund.Currency.Length - 4, 3);
            var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);

            var mFundUnitIssuesTrans = await mDataLoader.fetchFundUnitTransItems(mFund.Name, mNow, App.AllClients, App.Purchase) as FundUnitTrans[];
            issuesTransAmount = 0;
            issuesTransUnits = 0;
            foreach (var mFundUnitIssuesTran in mFundUnitIssuesTrans)
            {
                issuesTransAmount += mFundUnitIssuesTran.Amount;
                issuesTransUnits += mFundUnitIssuesTran.Units;
            }
            IssuesAmount.Text = StringUtils.Format(MathUtils.round(issuesTransAmount, 2));
            IssuesUnits.Text = StringUtils.Format(MathUtils.round(issuesTransUnits, 2)) + " units";




            RedemptionsCurrency.Text = mFund.Currency.Substring(mFund.Currency.Length - 4, 3);

            var mFundUnitRedemptionTrans = await mDataLoader.fetchFundUnitTransItems(mFund.Name, mNow, App.AllClients, App.Redemption) as FundUnitTrans[];
            redemptionTransAmount = 0;
            redemptionTransUnits = 0;
            foreach (var mFundUnitIssuesTran in mFundUnitRedemptionTrans)
            {
                redemptionTransAmount += mFundUnitIssuesTran.Amount;
                redemptionTransUnits += mFundUnitIssuesTran.Units;
            }
            RedemptionsAmount.Text = StringUtils.Format(MathUtils.round(redemptionTransAmount, 2));
            RedemptionsUnits.Text = StringUtils.Format(MathUtils.round(redemptionTransUnits, 2)) + " units";


            mSecurities = await mDataLoader.fetchSecurities(mFund.Name) as Security[];
            if (mSecurities.Length > 0) {
                ContentPane.Visibility = Visibility.Visible;
                EmptyPane.Visibility = Visibility.Collapsed;
                initFundUi();
                initChartValue(mFund, 7);
            }
            else
            {
                EmptyPane.Visibility = Visibility.Visible;
                ContentPane.Visibility = Visibility.Collapsed;
            }
            StatementsCombo.SelectionChanged += (a, b) => {
                int index = StatementsCombo.SelectedIndex;
                StatementsSection.SelectedIndex = index;
                Console.WriteLine("StatementsSection.SelectedIndex " + index);
                if(index == 1) { initDetailedSche(); }
             else if (index == 2) { initExpenseItemData(); }
             else if (index == 3) { initIncomeStatementGrid(); }
             else if (index == 4) { initBankDetails(); }
             else if (index == 5) { initChangesInEquity(); }
             else if (index == 6) { initStatementOfFinancialPosition(); }
   
            };
            StatementsCombo.SelectedIndex = 0;

            Grid[] AssetDetailPanes = new Grid[] { NewPurchaseContentPropertyPane, NewPurchaseContentEquityPane, NewPurchaseContentCISPane, NewPurchaseContentFixIncomePane };

            NewPurchaseContentFundCombo.SelectionChanged += (a, b) =>
            {
                if(NewPurchaseContentFundCombo.SelectedIndex==0)
                setAssetDetailPaneVisibility(AssetDetailPanes, 1);
               else if (NewPurchaseContentFundCombo.SelectedIndex == 4|| NewPurchaseContentFundCombo.SelectedIndex == 5)
                    setAssetDetailPaneVisibility(AssetDetailPanes, 0);
                else if (NewPurchaseContentFundCombo.SelectedIndex == 6)
                    setAssetDetailPaneVisibility(AssetDetailPanes, 2);
                else
                    setAssetDetailPaneVisibility(AssetDetailPanes, 3);
            };

            setAssetDetailPaneVisibility(AssetDetailPanes, 1);
        }
        private void setAssetDetailPaneVisibility(Grid[] AssetDetailPanes, int index)
        {
            for (int i = 0; i < AssetDetailPanes.Length; i++)
                AssetDetailPanes[i].Visibility = (i == index) ? Visibility.Visible : Visibility.Collapsed;

        }
        private void MenuItemEdit(object sender, RoutedEventArgs e)
        {
            DetailedScheduleTransitioner.SelectedIndex = 3;
            Console.WriteLine("MenuItemEdit " + sender.ToString());

            var menuItem = (MenuItem)sender;

            var contextMenu = (ContextMenu)menuItem.Parent;

            var item = (DataGrid)contextMenu.PlacementTarget;

            var mSecurity = (Security)item.SelectedCells[0].Item;


            Calendar.SelectedDate = new DateTime(DateUtils.MillisToTicks(long.Parse(mSecurity.TransactionDate.ToString())));
            AssetNameTextBox.Text = mSecurity.Name;

            var mTypes = new String[] { App.Types.PropertyType, App.Types.GovernmentBondType
            , App.Types.TermDepositType, App.Types.GovernmentTreasuryBillType, App.Types.ListedEquityType
            , App.Types.UnlistedEquityType, App.Types.CISType, App.Types.OtherInvestmentsType};
            for(int i=0;i< mTypes.Length; i++)
                if (mSecurity.Type.Equals(mTypes[i]))
                {
                    NewPurchaseContentFundCombo.SelectedIndex = i;

                    if (NewPurchaseContentFundCombo.SelectedIndex == 0) {
                        PropertyAddressTextBox.Text = mSecurity.Address;
                        PropertyValueTextBox.Text = mSecurity.Value.ToString();
                    }
                    else if (NewPurchaseContentFundCombo.SelectedIndex == 4 || NewPurchaseContentFundCombo.SelectedIndex == 5)
                    {
                        EquityNameTextBox.Text = mSecurity.Name;
                        SharePriceTextBox.Text = mSecurity.Value.ToString();
                        NumberSharesTextBox.Text = mSecurity.Nshares.ToString();
                    }
                    else if (NewPurchaseContentFundCombo.SelectedIndex == 6) {
                        CisNameTextBox.Text = mSecurity.Name;
                        UnitPriceTextBox.Text = mSecurity.Value.ToString();
                        NumberUnitsTextBox.Text = mSecurity.Nshares.ToString();
                    }
                    else {
                        
                        NominalValueTextBox.Text = mSecurity.Value.ToString();
                        DailyInterestTextBox.Text= mSecurity.DailyInterest.ToString();
                        MaturityDatePicker.SelectedDate = new DateTime(DateUtils.MillisToTicks(long.Parse(mSecurity.MaturityDate.ToString())));
                    }
                }
        }
        private Rectangle getRecFromColour(String color) {
            Rectangle mRectangle =    new Rectangle();
            mRectangle.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(color);
            return mRectangle;
        }

        private void ShowStatements(object sender, RoutedEventArgs e) {
            BtnStatementsRec.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#f44336");
            BtnBalancessRec.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#757575");
            FundUserControlSection.SelectedIndex = 0;
            // FundUserControlSection.Se
        }
        private void ShowBalances(object sender, RoutedEventArgs e)
        {
            BtnStatementsRec.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#757575");
            BtnBalancessRec.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#f44336");
            FundUserControlSection.SelectedIndex = 1;
            
                HaveBalancesLoaded = true;
                initBalances();
                StatementsCombo.SelectionChanged += (a, b) => {

                };
            
            
        }
        private async void initIncomeStatementGrid()
        {
           // IncomeStatementInvestmentIncomeGrid.Items.Clear();
           //IncomeStatementExpenseGrid.Items.Clear();

            String[] Types = new String[] {
                 App.Types.PropertyType
                ,App.Types.GovernmentBondType
                ,App.Types.GovernmentTreasuryBillType
                ,App.Types.TermDepositType
                ,App.Types.OtherInvestmentsType
                ,App.Types.ListedEquityType
                ,App.Types.UnlistedEquityType
                ,App.Types.CISType};
            var mFirstDayOfQuater = DateUtils.TicksToMillis(DateUtils.FirstDayOfQuater(DateTime.Now).Ticks);
            var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
            ItemValue[] Values = await mDataLoader.fetchIncomestatementAssetsAndExpenses(mFund.Name, mFirstDayOfQuater, mNow, Types);
            IncomeStatementInvestmentIncomeGrid.ItemsSource = Values.ToList().GetRange(0, Types.Length);
            IncomeStatementExpenseGrid.ItemsSource = Values.ToList().GetRange( Types.Length, Values.Length- Types.Length);
        }
        private async void initExpenseItemData()
        {
            var mFirstDayOfQuater = DateUtils.TicksToMillis(DateUtils.FirstDayOfQuater(DateTime.Now).Ticks);
            var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
            ExpenseItem[] Values = await mDataLoader.fetchFundItems(mFund.Name, mFirstDayOfQuater, mNow, App.Expenses);
            ExpensesStatementDataGrid.ItemsSource = Values;
        }

        private async void initChangesInEquity()
        {
            var mFirstDayOfQuater = DateUtils.TicksToMillis(DateUtils.FirstDayOfQuater(DateTime.Now).Ticks);
            var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
            ItemValue[] Values = await mDataLoader.fetchChangesInEquity(mFund.Name, mFirstDayOfQuater, mNow);
            ChangeInEquityDataGrid.ItemsSource = Values;
        }

        private async void initStatementOfFinancialPosition()
        {
            double Balance = 0;
            var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
            ItemValue[][] Values = await mDataLoader.fetchStatementOfFinancialPosition(mFund.Name, mNow);
            int k = 0;
            foreach(ItemValue[] items in Values)
            {
                foreach(ItemValue item in items) {
                  //  Balance += Double.Parse(item.Value);
                    item.Value = StringUtils.Format(Double.Parse(item.Value)) + " "+mFund.Currency.Substring(mFund.Currency.Length - 4, 3); ;
                }
                k++;
            }
         //   Balance =Double.Parse( Values[0][Values.Length - 1].Value);
            AssetsSOFPDataGrid.ItemsSource = Values[0];
            LiabilitesSOFPDataGrid.ItemsSource = Values[1];
            CapitalSOFPDataGrid.ItemsSource = Values[2];
            AssetsSOFPDataGridBalance.Text = Values[0][1].Value  ;
        }

        private async void initBankDetails()
        {
            var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
            Bank[] Values = await mDataLoader.fetchBankItems(mFund.Name,  mNow);
            BankDataGrid.ItemsSource = Values;
        }
        private async void initDetailedSche()
        {
            double Balance = 0;

            String[] Types = new String[] {
                 App.Types.PropertyType
                ,App.Types.GovernmentBondType
                ,App.Types.GovernmentTreasuryBillType
                ,App.Types.TermDepositType
                ,App.Types.OtherInvestmentsType
                ,App.Types.ListedEquityType
                ,App.Types.UnlistedEquityType
                ,App.Types.CISType};
            var mFirstDayOfQuater = DateUtils.TicksToMillis(DateUtils.FirstDayOfQuater(DateTime.Now).Ticks);
            var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
            ItemValue[] Values = await mDataLoader.fetchDetailedIncomeSche(mFund.Name, mFirstDayOfQuater, mNow, Types);
            foreach(ItemValue mItemValue in Values) {
                if (mItemValue.SecondaryName != null)
                {
                    mItemValue.Name = mItemValue.SecondaryName;
                }
                else {
                    if (mItemValue.Value != null)
                    {

                        mItemValue.SecondaryValue = StringUtils.Format(Double.Parse(mItemValue.Value)) + " " + mFund.Currency.Substring(mFund.Currency.Length - 4, 3); ;
                        Balance += Double.Parse(mItemValue.Value);
                    }
                }
            }
           StatmentsDataGrid.Columns.Clear();
            StatmentsDataGrid.HeadersVisibility = DataGridHeadersVisibility.None;
            String[] Bindings = new String[] { "Name", "SecondaryValue" };

            for (int i = 0; i < 2; i++)
            {

                StatmentsDataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Binding = new Binding(Bindings[i]),
                        IsReadOnly = true
                    });
                

            }
            StatmentsDataGridBalance.Text = StringUtils.Format(Balance) + " " + mFund.Currency.Substring(mFund.Currency.Length - 4, 3); ; 
            StatmentsDataGrid.ItemsSource = Values;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e) { }
        
        private  void SelectStatement(object sender, RoutedEventArgs e)
        {
            ObservableCollection<ItemValue> items =  IncomeStatementDataGrid.ItemsSource as ObservableCollection<ItemValue>;
            items[IncomeStatementDataGrid.SelectedIndex].SecondaryValue = ShadowDepth.Depth3;
            for(int i=0;i< items.Count; i++)
            {
                if(i!= IncomeStatementDataGrid.SelectedIndex) items[i].SecondaryValue = ShadowDepth.Depth0;
            }
            // IncomeStatementDataGrid.ItemsSource = items;
            SelectStatement(IncomeStatementDataGrid.SelectedIndex);
        }
        private void AnimateSlider(Transitioner mTransitioner, int ControlIndex)
        {
            var storyboard = new Storyboard();
            var openingEffect = ((TransitionerSlide)mTransitioner.Items[ControlIndex]).OpeningEffect?.Build(((TransitionerSlide)mTransitioner.Items[ControlIndex]));
            if (openingEffect != null)
                storyboard.Children.Add(openingEffect);
            foreach (var effect in ((TransitionerSlide)mTransitioner.Items[ControlIndex]).OpeningEffects.Select(e => e.Build(((TransitionerSlide)mTransitioner.Items[ControlIndex]))).Where(tl => tl != null))
            {
                storyboard.Children.Add(effect);
            }

            storyboard.Begin(((TransitionerSlide)mTransitioner.Items[ControlIndex]));
        }
        private async void SelectStatement(int index)
        {

            String[] Types = new String[] {
                 App.Types.PropertyType
                ,App.Types.GovernmentBondType
                ,App.Types.GovernmentTreasuryBillType
                ,App.Types.TermDepositType
                ,App.Types.OtherInvestmentsType
                ,App.Types.ListedEquityType
                ,App.Types.UnlistedEquityType
                ,App.Types.CISType
                ,App.Types.PropertyType
                ,"*"};

            String[] BalanceTypes = new String[] {
                  "Balance At Start of Period"
                  ,"Acquired During Period"
                  ,"Disposed During Period"
                  ,"Change In Fair Value"
                  ,"Change In Disposal"
                  ,"Change In Exchange"
                  ,"Interest Received"
                  ,"Maturity (Disposal) During Period"
                  ,"Accured Interest"
                  ,"Balance At End of Period"};

            String[][] SupoortedBalacneTypes = new String[][] {
            new String[] {
             BalanceTypes [0]
            ,BalanceTypes [1]
            ,BalanceTypes [2]
            ,BalanceTypes [3]
            ,BalanceTypes [9]}

            , new String[] {
             BalanceTypes [0]
            ,BalanceTypes [1]
            ,BalanceTypes [8]
            ,BalanceTypes [6]
            ,BalanceTypes [3]
            ,BalanceTypes [7]
            ,BalanceTypes [9]}

            , new String[] {
             BalanceTypes [0]
            ,BalanceTypes [1]
            ,BalanceTypes [8]
            ,BalanceTypes [6]
            ,BalanceTypes [3]
            ,BalanceTypes [7]
            ,BalanceTypes [9]}

            , new String[] {
             BalanceTypes [0]
            ,BalanceTypes [1]
            ,BalanceTypes [8]
            ,BalanceTypes [6]
            ,BalanceTypes [3]
            ,BalanceTypes [7]
            ,BalanceTypes [9]}
              
            ,new String[] {
             BalanceTypes [0]
            ,BalanceTypes [1]
            ,BalanceTypes [2]
            ,BalanceTypes [3]
            ,BalanceTypes [9]}

            ,new String[] {
             BalanceTypes [0]
            ,BalanceTypes [1]
            ,BalanceTypes [2]
            ,BalanceTypes [3]
            ,BalanceTypes [9]}

            , new String[] {
             BalanceTypes [0]
            ,BalanceTypes [1]
            ,BalanceTypes [8]
            ,BalanceTypes [6]
            ,BalanceTypes [3]
            ,BalanceTypes [7]
            ,BalanceTypes [9]
            }
            ,    new String[] {
             BalanceTypes [0]
            ,BalanceTypes [1]
            ,BalanceTypes [2]
            ,BalanceTypes [3]
            ,BalanceTypes [9]}
            };


            int ControlIndex = (index == (IncomeStatementDataGrid.Items.Count-1)) ? 0 : 1;

            BalancesTransitioner.SelectedIndex = ControlIndex;
            DetailedScheduleTransitioner.SelectedIndex = ControlIndex;
            AnimateSlider(BalancesTransitioner, ControlIndex);
            AnimateSlider(DetailedScheduleTransitioner, ControlIndex);
            


            if (index == (IncomeStatementDataGrid.Items.Count-1))
            {
               // BalancesTransitioner.SelectedIndex = 0;
            }
            else
            {
                var mFirstDayOfQuater = DateUtils.TicksToMillis(DateUtils.FirstDayOfQuater(DateTime.Now).Ticks);
                var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
                List<ItemValue> SupportedValues = new List<ItemValue>();
                ItemValue[] Values = await mDataLoader.fetchBalances(mFund.Name, mFirstDayOfQuater, mNow, Types[index]);
                foreach(ItemValue mItemValue in Values)
                {
                    mItemValue.SecondaryValue = mFund.Currency.Substring(mFund.Currency.Length-4, 3);
                    mItemValue.Value = StringUtils.Format(Double.Parse(mItemValue.Value));
                    if (SupoortedBalacneTypes[index].Contains(mItemValue.Name))
                        SupportedValues.Add(mItemValue);
                }
                BalancesDataGrid.ItemsSource = SupportedValues;

                Security[] mSecurities = await mDataLoader.fetchSecurities(mFund.Name, mFirstDayOfQuater, mNow, Types[index]);
                String[] Hidden = new String[] { "Value at Start of Quater", "Interest Received", "Accured Interest" };
                String[][] Headers = new String[][] {
                    new String[] {"Property","Purchase","Value at Start of Quater", "Value" }
                    , new String[] {"Asset", "Purchase","Maturity","Nominal Value", "Value at Start of Quater", "Interest Received","Accured Interest", "Value" }
                    , new String[] {"Asset", "Purchase","Maturity","Nominal Value", "Value at Start of Quater", "Interest Received","Accured Interest", "Value" }
                    , new String[] {"Asset", "Purchase","Maturity","Nominal Value", "Value at Start of Quater", "Interest Received","Accured Interest", "Value" }
                    , new String[] {"Asset", "Purchase","Maturity","Nominal Value", "Value at Start of Quater", "Interest Received","Accured Interest", "Value" }
                    , new String[] {"Equity", "Purchase","Number of Shares", "Value at Start of Quater", "Value" }
                    , new String[] {"Equity", "Purchase","Number of Shares", "Value at Start of Quater", "Value" }
                    , new String[] {"CIS", "Purchase","Number of Units", "Value at Start of Quater", "Value" }
                    ,new String[] {"Property","Purchase","Value at Start of Quater", "Value" }
                  };
                String[][] ColumnBindings = new String[][] {
                    new String[] { "Name", "DateFormated", "CurrentFormated", "EndValueFormated" }
                    , new String[] { "Name", "DateFormated", "MaturityDateFormated", "Value", "CurrentFormated", "InterestReceivedFormated", "AccuredInterestFormated", "EndValueFormated" }
                    , new String[] { "Name", "DateFormated", "MaturityDateFormated", "Value", "CurrentFormated", "InterestReceivedFormated", "AccuredInterestFormated", "EndValueFormated" }
                    , new String[] { "Name", "DateFormated", "MaturityDateFormated", "Value", "CurrentFormated", "InterestReceivedFormated", "AccuredInterestFormated", "EndValueFormated" }
                    , new String[] { "Name", "DateFormated", "MaturityDateFormated", "Value", "CurrentFormated", "InterestReceivedFormated", "AccuredInterestFormated", "EndValueFormated" }
                    , new String[] { "Name", "DateFormated", "Nshares", "CurrentFormated", "EndValueFormated" }
                    , new String[] { "Name", "DateFormated", "Nshares", "CurrentFormated", "EndValueFormated" }
                    , new String[] { "Name", "DateFormated", "Nshares", "CurrentFormated", "EndValueFormated" }
                   , new String[] { "Name", "DateFormated", "CurrentFormated", "EndValueFormated" }
                  };
                DetailedScheduleDataGrid.Columns.Clear();

               // CurrentFormatedPanel.Visibility = Visibility.Collapsed;
               // InterestReceivedFormatedPanel.Visibility = Visibility.Collapsed;
              //  AccuredInterestFormatedPanel.Visibility = Visibility.Collapsed;

                for (int i=0;i< Headers[index].Length; i++) {
                    if (Hidden.Contains(Headers[index][i])) {
                        if (Headers[index][i].Equals(Hidden[0]))
                        {
                      //      CurrentFormatedPanel.Visibility = Visibility.Visible;
                        }
                        else if (Headers[index][i].Equals(Hidden[1]))
                        {
                      //      InterestReceivedFormatedPanel.Visibility = Visibility.Visible;
                        }
                        else if (Headers[index][i].Equals(Hidden[2]))
                        {
                         //   AccuredInterestFormatedPanel.Visibility = Visibility.Visible;
                        }
                    }
                    else {
                        DetailedScheduleDataGrid.Columns.Add(new DataGridTextColumn
                        {
                            Header = Headers[index][i]
             ,
                            Binding = new Binding(ColumnBindings[index][i]),
                            IsReadOnly = true
                        });
                    }
                    
                }
                DetailedScheduleDataGrid.ItemsSource = mSecurities;
            }
        }
        private Double[][] mChartValuesReceived;
        private List<String> DateLables;
        private List<DateTime> DateLablesRow;
        private void initSelectedPieChart(int position) {

            SeriesCollection mSeriesCollection = new SeriesCollection();
            String[] NamesShort = new String[] {"Property","Bonds","Treasury Bills","Term Deposits","Other Investments","Equities"
                           ,"Unlisted Equities","CIS","Loans And Recievables","Total Investment Income"};
            Double[] mValues = new Double[NamesShort.Length - 1];
            for (int m = 0; m < mValues.Length; m++)
            {
                Console.WriteLine("init selected position " + position + " m " + m);
              //  Console.WriteLine("data " + mChartValuesReceived);
                mSeriesCollection.Add(new PieSeries
                {

                    Values = new ChartValues<double>(new double[] { MathUtils.round( mChartValuesReceived[m][position],2) })
                    ,DataLabels=true,
                    Title = NamesShort[m],
                    LabelPoint  = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)

            });
            }

            ReturnDataGridPieChart.Series= mSeriesCollection;
            ReturnDataGridPieChart.AxisY[0].Title = DateLables[position];
        }
        private async void initBalances()
        {
            int days =30;
            long start = DateUtils.TicksToMillis(DateTime.Now.Ticks);
            long end = DateUtils.TicksToMillis(DateTime.Now.AddMonths(-6).Ticks);

            String[] Types = new String[] {App.Types.PropertyType,App.Types.GovernmentBondType,App.Types.GovernmentTreasuryBillType
                ,App.Types.TermDepositType,
                App.Types.OtherInvestmentsType,App.Types.ListedEquityType
                           ,App.Types.UnlistedEquityType,App.Types.CISType,App.Types.PropertyType,"*"};

            String[] TypesMinusOne = new String[] {App.Types.PropertyType,App.Types.GovernmentBondType,App.Types.GovernmentTreasuryBillType,
                App.Types.TermDepositType,
                App.Types.OtherInvestmentsType,App.Types.ListedEquityType
                           ,App.Types.UnlistedEquityType,App.Types.CISType,App.Types.PropertyType};

            List<Double[]> ChartValues = new List<double[]>();
            String[] Names = new String[] {"Investment Properties","Investment Bonds","Investment Treasury Bills","Investment Term Deposit","Funds Placements And Other Investments","Listed Equities"
                           ,"Unlisted Equities","Collective Investment Schemes","Loans And Recievables","Total Investment Income"};

            String[] NamesShort= new String[] {"Property","Bonds","Treasury Bills","Term Deposits","Other Investments","Equities"
                           ,"Unlisted Equities","CIS","Loans And Recievables","Total Investment Income"};
            
            ShadowDepth[] ValuesObjs = new ShadowDepth[] { ShadowDepth.Depth0, ShadowDepth.Depth0, ShadowDepth.Depth0, ShadowDepth.Depth0, ShadowDepth.Depth0, ShadowDepth.Depth0
                ,ShadowDepth.Depth0, ShadowDepth.Depth0, ShadowDepth.Depth0, ShadowDepth.Depth0 };
            List<ItemValue> mItemValues = new List<ItemValue>();

            double[] Values = await mDataLoader.fetchSecuritiesPresentValueType(mFund.Name, DateUtils.TicksToMillis(DateTime.Now.Ticks), Types);

            for (int i=0;i< Values.Length; i++) {
                
                mItemValues.Add(new ItemValue(Names[i], StringUtils.Format(Values[i]) + " ", ValuesObjs[i]));
            }

            IncomeStatementDataGrid.ItemsSource =new ObservableCollection<ItemValue> (mItemValues);



            ReturnDataGridChart.DisableAnimations = false;
            SeriesCollection mSeriesCollection = new SeriesCollection();
            Double[][] mValuesReceived = await mDataLoader.fetchSecuritiesPresentValueRangeType(mFund.Name, start, end, days, TypesMinusOne) as Double[][];
           ChartValues.AddRange(mValuesReceived);
            mChartValuesReceived = ChartValues.ToArray() ;
            int count = ChartValues[0].Length;
             DateLables = new List<string>();
            DateLablesRow = new List<DateTime>();
            for (int i=0;i< count; i++)
            {
                DateLablesRow.Add(DateTime.Now.AddDays(-1 * (30 * (i))));
                DateLables.Add(DateTime.Now.AddDays(-1 * (30 * (i))).ToString("dd MMM yyyy"));
            }
            var StackedColumnSeriesCollection = new List<StackedColumnSeries>();
            for (int i = 0; i < Types.Length - 1; i++)
            {
                Double[] mValues = new Double[Types.Length-1];
                for (int m= 0; m < count; m++)
                {
                    mValues[m] = ChartValues[i][m];
                }
                StackedColumnSeriesCollection.Add(new StackedColumnSeries
                {

                    Values = new ChartValues<double>(mValues),
                    StackMode = StackMode.Values,
                    DataLabels = false,
                    Title = NamesShort[i]
                });
                
            }

            DateLables.Reverse();
            mSeriesCollection.Reverse();
            DateLablesRow.Reverse();

            mSeriesCollection.AddRange(StackedColumnSeriesCollection);
            ReturnDataGridChart.AxisX[0].Title = "Date";
            ReturnDataGridChart.AxisX[0].Labels = DateLables;

            ReturnDataGridChart.AxisY[0].Title = "Market Value";
            ReturnDataGridChart.Series = mSeriesCollection;
        }
        private int selectedChartPoint;
        private void ReturnDataGridChart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            DetailedScheduleTransitioner.SelectedIndex = 2;
            selectedChartPoint = chartpoint.Key;
            initSelectedPieChart(chartpoint.Key);
          //  MessageBox.Show(chartpoint.Key.ToString());
        }
        private void BackToValuesChart(object sender, RoutedEventArgs chartpoint)
        {
            DetailedScheduleTransitioner.SelectedIndex = 0;
            //  MessageBox.Show(chartpoint.Key.ToString());
        }

        private async void ExportValuesFromChart(object sender, RoutedEventArgs chartpoint)
        {
            int position = selectedChartPoint;
            String[] NamesShort = new String[] {"Property","Bonds","Treasury Bills","Term Deposits","Other Investments","Equities"
                           ,"Unlisted Equities","CIS","Loans And Recievables","Total Investment Income"};
            Double[] mValues = new Double[NamesShort.Length - 1];
            List<ItemValue> mItemValues = new List<ItemValue>();
            for (int m = 0; m < mValues.Length; m++)
            {
                Console.WriteLine("init selected position " + position + " m " + m);
                var value = MathUtils.round(mChartValuesReceived[m][position], 2);
                if (value > 0)
                {
                    mItemValues.Add(new ItemValue(NamesShort[m], value.ToString()));
                }

            }
            var mNowFirst = DateUtils.TicksToMillis(DateLablesRow[position].AddMonths(-1).Ticks);
            var mNow = DateUtils.TicksToMillis(DateLablesRow[position].Ticks);

            using (var dirPicker = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dirPicker.ShowDialog();

                String dir = dirPicker.SelectedPath;
                if (dir != null)
                {
                    UiUnits.ShowNotification("Exporting to " + dir, NotificationType.Information);

                    Security[] mSecurities = await mDataLoader.fetchSecurities(mFund.Name, mNowFirst, mNow, App.Types.All);
                    Exporter.ExportChartValuesNampak(mItemValues, mSecurities, dir + "/" + DateLablesRow[position].ToString("mm MMM yyyy") + ".xlsx");

                }


            }

            //  MessageBox.Show(chartpoint.Key.ToString());
        }
        private void initFundUi() {
            FundTransDataGrid.ItemsSource = mSecurities;
        }
        private async void initChartValue(Fund mFund,int days) {
            long start = DateUtils.TicksToMillis(DateTime.Now.Ticks);
            long end = DateUtils.TicksToMillis(DateTime.Now.AddMonths(-3).Ticks);

            Double[] mValues = await mDataLoader.fetchSecuritiesPresentValueRange(mFund.Name,start,end,days) as Double[];
            await mDataLoader.fetchTotalsFundTransFor(mFund.Name);

            FundValueCartesianChart.DisableAnimations = true;
            SeriesCollection mSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Total Fund Value",
                    Values = new ChartValues<double> (mValues.Reverse() ),
                    
                    Foreground=(SolidColorBrush)new BrushConverter().ConvertFromString("#424242"),
                    LineSmoothness = 1,
                    StrokeThickness=4,PointGeometry = null
                    ,Stroke=(SolidColorBrush)new BrushConverter().ConvertFromString("#9C27B0")
                    ,Fill=Brushes.Transparent
                }
            };
            FundValueCartesianChart.Series = mSeriesCollection;


            if (issuesTransAmount > 0 && mValues.Length>0)
            {

                var mFirstDayOfQuater = DateUtils.TicksToMillis(DateUtils.FirstDayOfQuater(DateTime.Now).Ticks);
                var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
                Security[] mSecurities = await mDataLoader.fetchSecurities(mFund.Name, mFirstDayOfQuater, mNow, App.Types.All);
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
                Console.WriteLine("totalSecValues " + totalSecValues);
                if (totalSecValues >= issuesTransAmount)
                {
                    FundSizeUnitPrice.Text = StringUtils.Format(MathUtils.round((mValues[0] / (issuesTransUnits - Math.Abs(redemptionTransUnits))), 2));

                }
                else {
                    FundSizeUnitPrice.Text = StringUtils.Format(MathUtils.round(((mValues[0]+(issuesTransAmount- totalSecValues)) / (issuesTransUnits - Math.Abs(redemptionTransUnits))), 2));

                }


                //  FundSizeUnitPrice.Text = StringUtils.Format(MathUtils.round(((issuesTransAmount - Math.Abs(redemptionTransAmount)) / (issuesTransUnits - Math.Abs(redemptionTransUnits))), 2));
                FundSizeTotalUnits.Text = StringUtils.Format(MathUtils.round((issuesTransUnits - Math.Abs(redemptionTransUnits)), 2))+" Units";
            }
        }

        private void PurchaseSecurity(object sender, RoutedEventArgs e)
        {
            (Window.GetWindow(this) as MainWindow).startSecurityPurchase();
        }
        public void SingleEntry(Type mType, string json)
        {
            if (mType.Equals(typeof(Security)))
            {
                Security mSecurity = JsonConvert.DeserializeObject<Security>(json);
                if(mSecurity.Fund.Equals(mFund.Name))
                load(mFund);
            }
        }

        public void MultipleEntry(Type mType, string json)
        {
        }

        public void Deletion(Type mType, string json)
        {
        }
    }
}
