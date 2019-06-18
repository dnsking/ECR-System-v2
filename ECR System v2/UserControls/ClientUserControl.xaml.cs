using System;
using System.Collections.Generic;
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

namespace ECR_System_v2.UserControls
{
    /// <summary>
    /// Interaction logic for ClientUserControl.xaml
    /// </summary>
    public partial class ClientUserControl : UserControl, DataEntryListener
    {
        private Fund mFund;
        private DataLoader mDataLoader;
        private Client[] mClients;

        private String prevClient;
        private int prevDays;

        public ClientUserControl()
        {
            InitializeComponent();
        }
        private async void AddClient(object sender, RoutedEventArgs e)
        {
            var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
            Client mClient = new Client();
            mClient.Fund = mFund.Name;
            mClient.ClientName = ClientNameTextBox.Text;
            mClient.ClientEmailAdress = ClientEmailAddressTextBox.Text;
            mClient.ClientPhysicalAdress = ClientPhysicalAddressTextBox.Text;
            mClient.DateCreated = mNow;
            mClient.ClientId = ClientIdTextBox.Text;
            mClient.Open = App.Open;
            await mDataLoader.addFundUnitClients(mClient);
           // AddClientExpander.IsExpanded = false;
            UiUnits.ClearText(new TextBox[] { ClientNameTextBox, ClientEmailAddressTextBox, ClientPhysicalAddressTextBox, ClientIdTextBox });
            loadClient();
        }

        private  void SendReport(object sender, RoutedEventArgs e)
        {
            DialogGrid.IsOpen = true;

            LottieAnimationView.PauseAnimation();
            LottieAnimationView.FileName = "./Resources/4900-files-transfer-animation.json";
             LottieAnimationView.PlayAnimation();
            Exporter.ExportPdf(null);
        }
        private async void NewTranaction(object sender, RoutedEventArgs e)
        {
            var multiple = TransactionCombo.SelectedIndex == 0 ? 1 : (Double.Parse(NumberofUnitsTextBox.Text)<0? - 1:  1);
            var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);

            FundUnitTrans mFundUnitTrans = new FundUnitTrans();
            mFundUnitTrans.Amount = Double.Parse(NumberofUnitsTextBox.Text) * Double.Parse(UnitPriceTextBox.Text)* multiple;
            mFundUnitTrans.Client = mClients[ClientsListCombo.SelectedIndex - 1].ClientId;
            mFundUnitTrans.DateInMillis = mNow;
            mFundUnitTrans.Fund = mFund.Name;
            mFundUnitTrans.Units = Double.Parse(NumberofUnitsTextBox.Text) * multiple;
            mFundUnitTrans.TransactionType = TransactionCombo.SelectedIndex;
            await mDataLoader.addFundUnitTransItem(mFundUnitTrans);

            NewTransactionExpander.IsExpanded = false;
            UiUnits.ClearText(new TextBox[] { NumberofUnitsTextBox, UnitPriceTextBox});
            loadClient();
        }

        public async void load(Fund mFund) {
            if (mDataLoader == null)
            {
                mDataLoader = (Window.GetWindow(this) as MainWindow).getDataLoader();
                mDataLoader.addDataEntryListeners(this);
            }
            this.mFund = mFund;
            loadClient();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private List<Client> mClientList;
        private void ClientSearch(object sender,TextCompositionEventArgs e)
        {
            ClientsListCombo.IsDropDownOpen = true;
            if (e.Text.Length > 0)
            {

            }
            else {
                loadClient();
            }
        }
        private async void loadClient() {



           // FileName = "Resources/4900-files-transfer-animation.json"
            Client mClient = new Client();
            mClient.Fund = mFund.Name;
            mClient.ClientName = App.AllClients;
            mClient.ClientId = App.AllClients;
            //mClients = new Client[] { }.ToList();
            mClients =await mDataLoader.fetchFundUnitClients(mFund.Name, App.Open) as Client[];
           
            mClientList = mClients.ToList();
            //mClientList = new Client[] { mClients[0], mClients[1], mClients[2] }.ToList();
            mClientList.Insert(0, mClient);

            ClientsListCombo.ItemsSource = mClientList;
            ClientsListCombo.SelectionChanged += (a, b) =>
            {
                if(ClientsListCombo.SelectedIndex>-1)
                initClientList(mClientList[ClientsListCombo.SelectedIndex].ClientId, 7);
            };
            PurchasesCheckBox.Checked += (a, b)=>{
                initClientList(prevClient, prevDays);
            };


            RedemptionsCheckBox.Checked += (a, b) => {
                initClientList(prevClient, prevDays);
            };

            if (ClientsListCombo.SelectedIndex == -1)
                ClientsListCombo.SelectedIndex = 0;
        }
        private async void initClientList(String client,int days)
        {
            prevDays = days;
            prevClient = client;
            int TransactionType = ((bool)PurchasesCheckBox.IsChecked && (bool)RedemptionsCheckBox.IsChecked) ? App.All :
                ((bool)PurchasesCheckBox.IsChecked ? App.Purchase : App.Redemption);
            var mFirstDayOfQuater = DateUtils.TicksToMillis(DateUtils.FirstDayOfQuater(DateTime.Now).Ticks);
            var mNow = DateUtils.TicksToMillis(DateTime.Now.Ticks);
            ////Console.WriteLine(mNow);
            ////Console.WriteLine(mNow);
            var mFundUnitTrans = await mDataLoader.fetchFundUnitTransItems(mFund.Name, mNow, client, TransactionType) as FundUnitTrans[];
              if (client.Equals(App.AllClients))
              {
                AllClientTranstionsDataGrid.ItemsSource = mFundUnitTrans;
                long start = DateUtils.TicksToMillis(DateTime.Now.Ticks);
                  long end = DateUtils.TicksToMillis(DateTime.Now.AddMonths(-3).Ticks);

                double totalAmount = 0;
                foreach (var item in mFundUnitTrans)
                    totalAmount += item.Amount;
                AllClientTranstionsTotalsTextBlock.Text =MathUtils.round(totalAmount,2).ToString();
                ClientSelectionTransitioner.SelectedIndex = 0;
                UiUnits.AnimateSlider(ClientSelectionTransitioner, 0);
                Double[] mValues = await mDataLoader.fetchFundUnitTransItemsValueRangeTotal(mFund.Name, start, end, days, client, TransactionType);
                  UnitsCartesianChart.DisableAnimations = true;
                  SeriesCollection mSeriesCollection = new SeriesCollection
              {
                  new LineSeries
                  {
                      Title = "Total Number of Units",
                      Values = new ChartValues<double> (mValues.Reverse() ),

                      Foreground=(SolidColorBrush)new BrushConverter().ConvertFromString("#424242"),
                      LineSmoothness = 1,
                      StrokeThickness=4,PointGeometry = null
                      ,Stroke=(SolidColorBrush)new BrushConverter().ConvertFromString("#9C27B0")
                      ,Fill=Brushes.Transparent
                  }
              };
                  UnitsCartesianChart.Series = mSeriesCollection;
              }
              else
              {
                  SingleClientTranstionsDataGrid.ItemsSource = mFundUnitTrans;

                double totalAmount = 0;
                foreach (var item in mFundUnitTrans)
                    totalAmount += item.Amount;
                SingleClientTranstionsTotalsTextBlock.Text = MathUtils.round(totalAmount, 2).ToString();
                ClientSelectionTransitioner.SelectedIndex = 1;
                  UiUnits.AnimateSlider(ClientSelectionTransitioner, 1);
              }

            //AllClientTranstionsDataGrid.ItemsSource = new FundUnitTrans[] { mFundUnitTrans[0], mFundUnitTrans[1] };
            Console.WriteLine("initClientList complete");

        }

        public void SingleEntry(Type mType, string json)
        {
        }

        public void MultipleEntry(Type mType, string json)
        {
        }

        public void Deletion(Type mType, string json)
        {
        }
    }
}
