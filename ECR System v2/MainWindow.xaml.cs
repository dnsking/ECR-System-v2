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
using System.Windows.Threading;
using ECR_System_v2.Data;
using ECR_System_v2.IO;
using ECR_System_v2.Loaders;
using ECR_System_v2.Loaders.Listeners;
using ECR_System_v2.Utils;
using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;

namespace ECR_System_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, DataEntryListener
    {
        private DataLoader mDataLoader;
        private DispatcherTimer timer;
        private Boolean doneLoading;

        public MainWindow()
        {
            InitializeComponent();
            mDataLoader = new DataLoader();
            mDataLoader.addDataEntryListeners(this);

            DragPanel.DragEnter += new DragEventHandler(DragFile_DragEnter);
            DragPanel.Drop += new DragEventHandler(DragFile_DragDrop);
           // new Exporter().SendMails();
            
        }
        private void selectFund(Fund mFund)
        {
            ContentTransitioner.SelectedIndex = 1;
            FundControl.load( mFund);
        }

        private void DragFile_DragDrop(object sender, DragEventArgs e)
        {
                Array szFile = (Array)e.Data.GetData(DataFormats.FileDrop);
               String szFilePath = szFile.GetValue(0).ToString();
                new Exporter().SendEmailAddressAndFile(szFilePath);
                /*
                
              ImportPane.Visibility = Visibility.Visible;
              ImportingTextBlock.Text = "Saving";
                */
            
        }
        private void DragFile_DragEnter(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effects = DragDropEffects.Copy;

        }

        private void selectClient(Fund mFund)
        {
            ContentTransitioner.SelectedIndex = 2;
            ClientUserControl.load(mFund);
        }
        private void SwitchToSignIn(object sender, RoutedEventArgs e)
        {
            AccountTransitioner.SelectedIndex = 0;
        }

        private void SwitchToSignUp(object sender, RoutedEventArgs e)
        {
            AccountTransitioner.SelectedIndex = 1;
        }
        private async void SignIn(object sender, RoutedEventArgs e)
        {
            Account mAccount = new Account();
            mAccount.Name = SignInUserNameTextBox.Text;
            mAccount.Password = SignInPasswordBox.Password;
            login();
            /*
            Token value = await mDataLoader.login(mAccount);

            AccountNotExists.Visibility = Visibility.Collapsed;
            NoInternetAccessError_0.Visibility = Visibility.Collapsed;
            if (value != null)
            {
                if (value.Key != null)
                {

                    login();
                }
                else {
                    AccountNotExists.Visibility = Visibility.Visible;
                }
            }
            else
            {
                NoInternetAccessError_0.Visibility = Visibility.Visible;
            }*/
        }
        private async void SignUp(object sender, RoutedEventArgs e)
        {
            Account mAccount = new Account();
            mAccount.Name = SignupUserNameTextBox.Text;
            mAccount.Password = SignupPasswordBox.Password;
            PasswordDontMatchError.Visibility = Visibility.Collapsed;
            NoInternetAccessError_1.Visibility = Visibility.Collapsed;
            AccountExistsError.Visibility = Visibility.Collapsed;
            if (SignupPasswordBox.Password.Equals(SignupPasswordBoxRepeat.Password)) {
                login();
                /*ReturnResult value = await mDataLoader.addaccount(mAccount);
                if (value != null)
                {
                    if (value.ReponseValue.Equals(App.AccountCreatedReponseValue)) { login(); }
                    else
                    {
                        AccountExistsError.Visibility = Visibility.Visible;
                    }
                }
                else
                {

                    NoInternetAccessError_1.Visibility = Visibility.Visible;
                }*/
            }
            else {
                PasswordDontMatchError.Visibility = Visibility.Visible;
            }
            
        }
        private void login()
        {
            WindowTransitioner.SelectedIndex = 1;
            LuseControl.init();
            initFundTree();
            initNewFundContent();
        }
        public void SingleEntry(Type mType, string json)
        {
            if (mType.Equals(typeof(Fund))) {
                initFundTree();
            }
        }

        public void MultipleEntry(Type mType, string json)
        {
        }

        public void Deletion(Type mType, string json)
        {
        }
        private async void initFundTree() {
            Fund[] mFunds = await getDataLoader().fetchFunds() as Fund[];

            List<TreeViewItem> mTreeItems = new List<TreeViewItem>();
            foreach(Fund mFund in mFunds)
            {
                TreeViewItem mTreeViewItem = new TreeViewItem();
                mTreeViewItem.Header = mFund.Name;
                mTreeViewItem.Selected += (a, b) => selectFund(mFund);
                mTreeItems.Add(mTreeViewItem);
            }
                FundTreeItem.ItemsSource = mTreeItems;

            List<TreeViewItem> mTreeItemsClients = new List<TreeViewItem>();
            foreach (Fund mFund in mFunds)
            {
                TreeViewItem mTreeViewItem = new TreeViewItem();
                mTreeViewItem.Header = mFund.Name;
                mTreeViewItem.Selected += (a, b) => selectClient(mFund);
                mTreeItemsClients.Add(mTreeViewItem);
            }
            ClientTreeItem.ItemsSource = mTreeItemsClients;

            initPurchaseContentDialog(mFunds);

        }
        private Share findShare(String name)
        {
          return  LuseControl.findShare(name);
        }
        private void initPurchaseContentDialog(Fund[] mFunds)
        {
            List<String> FundNames = new List<String>();
            foreach(Fund mFund in mFunds)
            {
                FundNames.Add(mFund.Name);
            }
            NewPurchaseContentFundCombo.ItemsSource = FundNames;
            if (FundNames.Count > 0)
                NewPurchaseContentFundCombo.SelectedIndex = 0;
            Grid[] AssetDetailPanes = new Grid[] { NewPurchaseContentPropertyPane, NewPurchaseContentEquityPane, NewPurchaseContentCISPane, NewPurchaseContentFixIncomePane };
         
            NewPurchaseContentPropertyRadioBtn.Checked += (a, b) => {
                if ((Boolean)NewPurchaseContentPropertyRadioBtn.IsChecked) {
                    setAssetDetailPaneVisibility(AssetDetailPanes, 0);
                }
            };
            NewPurchaseContentListedEquityRadioBtn.Checked += (a, b) => {
                if ((Boolean)NewPurchaseContentListedEquityRadioBtn.IsChecked) {
                    setAssetDetailPaneVisibility(AssetDetailPanes, 1);
                 
                }

            };
            NewPurchaseContentUnlistedEquityRadioBtn.Checked += (a, b) => {
                if ((Boolean)NewPurchaseContentUnlistedEquityRadioBtn.IsChecked)
                {
                    setAssetDetailPaneVisibility(AssetDetailPanes, 1);
                    EquityNameTextBox.Text = AssetNameTextBox.Text;
                    SharePriceTextBox.Text = "0.00";
                }
            };

            NewPurchaseContentCISRadioBtn.Checked += (a, b) => {
                if ((Boolean)NewPurchaseContentCISRadioBtn.IsChecked)
                {
                    setAssetDetailPaneVisibility(AssetDetailPanes, 2);
                }
            };

            NewPurchaseContentFundPlacementsAndOtherInvestmentsRadioBtn.Checked += (a, b) => {
                if ((Boolean)NewPurchaseContentFundPlacementsAndOtherInvestmentsRadioBtn.IsChecked)
                {
                    setAssetDetailPaneVisibility(AssetDetailPanes, 3);
                }
            };

            NewPurchaseContentGovernmentBondRadioBtn.Checked += (a, b) => {
                if ((Boolean)NewPurchaseContentGovernmentBondRadioBtn.IsChecked)
                {
                    setAssetDetailPaneVisibility(AssetDetailPanes, 3);
                }
            };
            NewPurchaseContentGovernmentTBRadioBtn.Checked += (a, b) => {
                if ((Boolean)NewPurchaseContentGovernmentTBRadioBtn.IsChecked)
                {
                    setAssetDetailPaneVisibility(AssetDetailPanes, 3);
                }
            };

        }
        private void setAssetDetailPaneVisibility(Grid[] AssetDetailPanes,int index)
        {
            for(int i = 0;i< AssetDetailPanes.Length;i++) 
                AssetDetailPanes[i].Visibility = (i == index) ? Visibility.Visible : Visibility.Collapsed;
            
        }
        private void initNewFundContent()
        {

            
            UnitPriceFixedRadioBtn.Checked += (a, b) => {
                UnitPriceFixedTextBox.IsEnabled =(Boolean) UnitPriceFixedRadioBtn.IsChecked;
            };
            TrusteeFeesCheckBox.Checked += (a, b) => {

                TrusteeFeesManualRadioButton.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoRadioButton.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoPercent.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoRepeat.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoRepeatAt.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
            };
            TrusteeFeesAutoRadioButton.Checked += (a, b) => {
                
                TrusteeFeesAutoPercent.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoRepeat.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoRepeatAt.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
            };



            AuditFeesCheckBox.Checked += (a, b) => {

                AuditFeesManualRadioButton.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                AuditFeesAutoRadioButton.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                AuditFeesAutoPercent.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                AuditFeesAutoRepeat.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                AuditFeesAutoRepeatAt.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
            };
            AuditFeesAutoRadioButton.Checked += (a, b) => {

                AuditFeesAutoPercent.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                AuditFeesAutoRepeat.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                AuditFeesAutoRepeatAt.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
            };



            CustodialFeesCheckBox.Checked += (a, b) => {

                CustodialFeesManualRadioButton.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                CustodialFeesAutoRadioButton.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                CustodialFeesAutoPercent.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                CustodialFeesAutoRepeat.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                CustodialFeesAutoRepeatAt.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
            };
            CustodialFeesAutoRadioButton.Checked += (a, b) => {

                CustodialFeesAutoPercent.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                CustodialFeesAutoRepeat.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                CustodialFeesAutoRepeatAt.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
            };


            ManagementFeesCheckBox.Checked += (a, b) => {

                ManagementFeesManualRadioButton.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                ManagementFeesAutoRadioButton.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                ManagementFeesAutoPercent.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                ManagementFeesAutoRepeat.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                ManagementFeesAutoRepeatAt.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
            };
            ManagementFeesAutoRadioButton.Checked += (a, b) => {

                ManagementFeesAutoPercent.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                ManagementFeesAutoRepeat.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                ManagementFeesAutoRepeatAt.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
            };

            TrusteeFeesCheckBox.Checked += (a, b) => {

                TrusteeFeesManualRadioButton.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoRadioButton.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoPercent.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoRepeat.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoRepeatAt.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
            };
            TrusteeFeesAutoRadioButton.Checked += (a, b) => {

                TrusteeFeesAutoPercent.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoRepeat.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
                TrusteeFeesAutoRepeatAt.IsEnabled = (Boolean)UnitPriceFixedRadioBtn.IsChecked;
            };
        }
        public DataLoader getDataLoader()
        {
            return mDataLoader;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private  void ShowLuse(object sender, RoutedEventArgs e) {
            ContentTransitioner.SelectedIndex = 0;
        }
        private async void SaveFund(object sender, RoutedEventArgs e)
        {

            DialogGrid.IsOpen = false;

            Fund mFund = new Fund();
            mFund.Name = FundNameTextBox.Text;
            mFund.Currency = CurrencyListBox.SelectedValue.ToString();
            mFund.HasClient = HasClinetsCheckBox.IsChecked.ToString();
            mFund.UnitPriceFloating = UnitPriceFloatingRadioBtn.IsChecked.ToString();
            mFund.UnitPriceFixed = UnitPriceFixedTextBox.Text;
            ReturnResult mReturnResult =  await getDataLoader().AddFund(mFund) as ReturnResult;
            


            String[] DefaultExpenses = new string[] { "Trustee Fees", "Audit Fees", "Custodial Fees", "Management Fees" };

            CheckBox[] ExpensesCheckBoxes = new CheckBox[] { TrusteeFeesCheckBox, AuditFeesCheckBox, CustodialFeesCheckBox, ManagementFeesCheckBox };

            RadioButton[] ManualRadioButtons = new RadioButton[] { TrusteeFeesManualRadioButton, AuditFeesManualRadioButton, CustodialFeesManualRadioButton, ManagementFeesManualRadioButton };
            RadioButton[] AutoRadioButtons = new RadioButton[]   { TrusteeFeesAutoRadioButton,   AuditFeesAutoRadioButton,   CustodialFeesAutoRadioButton,   ManagementFeesAutoRadioButton };
            TextBox[] AutoPercents = new TextBox[]               { TrusteeFeesAutoPercent,       AuditFeesAutoPercent,       CustodialFeesAutoPercent,       ManagementFeesAutoPercent };
            ComboBox[] AutoRepeats = new ComboBox[]              { TrusteeFeesAutoRepeat,        AuditFeesAutoRepeat,        CustodialFeesAutoRepeat,        ManagementFeesAutoRepeat };
            ComboBox[] AutoRepeatAts = new ComboBox[]            { TrusteeFeesAutoRepeatAt,      AuditFeesAutoRepeatAt,      CustodialFeesAutoRepeatAt,      ManagementFeesAutoRepeatAt };
            for (int i=0;i< ExpensesCheckBoxes.Length; i++) {
                if ((Boolean)ExpensesCheckBoxes[i].IsChecked) {
                    Expense mExpense = new Expense();
                    mExpense.ExpeName = DefaultExpenses[i];
                    mExpense.Fund = mFund.Name;
                    mExpense.Auto = AutoRadioButtons[i].IsChecked.ToString();
                    mExpense.AutoEvery = AutoRepeats[i].SelectedItem.ToString();
                    mExpense.AutoRatio = AutoRepeatAts[i].SelectedValue.ToString();
                    mExpense.Manual = ManualRadioButtons[i].IsChecked.ToString();
                    mExpense.AutoRatio = AutoPercents[i].Text;
                    mReturnResult = await getDataLoader().AddExpense(mExpense) as ReturnResult;
                }
            }

        }

        private void AddNewFund(object sender, RoutedEventArgs e)
        {
            NewFundContent.Visibility = Visibility.Visible;
            NewPurchaseContent.Visibility = Visibility.Collapsed;
            DialogGrid.IsOpen = true;
        }

        private void PurchaseSecurity(object sender, RoutedEventArgs e) {
            startSecurityPurchase();
        }
        public void startSecurityPurchase()
        {
            NewFundContent.Visibility = Visibility.Collapsed;
            NewPurchaseContent.Visibility = Visibility.Visible;
            DialogGrid.IsOpen = true;
            
        }

        private async void StepperNext(object sender, RoutedEventArgs e)
        {
            StepperNavigationEventArgs step = e as StepperNavigationEventArgs;
            if (step.NextStep != null && step.NextStep.Header != null)
            {
                StepTitleHeader header = step.NextStep.Header as StepTitleHeader;
                String headerText = header.FirstLevelTitle;
                if (headerText.Equals("Asset Details") && (Boolean)NewPurchaseContentListedEquityRadioBtn.IsChecked)
                {
                    Share mShare = findShare(AssetNameTextBox.Text);
                    if (mShare != null)
                    {

                        EquityNameTextBox.Text = mShare.CompanyName;
                        SharePriceTextBox.Text = mShare.Price;
                    }
                }
                else if (headerText.Equals("Asset Details"))
                {

                    PurchaseDatePicker.SelectedDate = (DateTime)Calendar.SelectedDate;
                    CisNameTextBox.Text = AssetNameTextBox.Text;
                }
            }
            else
            {
                if (!step.Cancel)
                {

                    DialogGrid.IsOpen = false;

                    Security mSecurity = new Security();
                    mSecurity.Name = AssetNameTextBox.Text;
                    mSecurity.Fund = NewPurchaseContentFundCombo.SelectedValue.ToString();
                    mSecurity.TransactionDate = DateUtils.TicksToMillis(((DateTime)Calendar.SelectedDate).Ticks);

                    if ((Boolean)NewPurchaseContentPropertyRadioBtn.IsChecked)
                    {
                        mSecurity.Type = NewPurchaseContentPropertyRadioBtn.Content.ToString();
                        mSecurity.Value = Double.Parse(PropertyValueTextBox.Text);
                        mSecurity.Address = PropertyAddressTextBox.Text;
                    }
                    else if ((Boolean)NewPurchaseContentListedEquityRadioBtn.IsChecked)
                    {
                        mSecurity.Type = NewPurchaseContentListedEquityRadioBtn.Content.ToString();
                        mSecurity.Name = EquityNameTextBox.Text;
                        mSecurity.Nshares = Double.Parse(NumberSharesTextBox.Text);
                        mSecurity.Value = Double.Parse(SharePriceTextBox.Text);
                    }
                    else if ((Boolean)NewPurchaseContentUnlistedEquityRadioBtn.IsChecked)
                    {
                        mSecurity.Type = NewPurchaseContentUnlistedEquityRadioBtn.Content.ToString();
                        mSecurity.Nshares = Double.Parse(NumberSharesTextBox.Text);
                        mSecurity.Value = Double.Parse(SharePriceTextBox.Text);
                    }
                    else if ((Boolean)NewPurchaseContentCISRadioBtn.IsChecked)
                    {
                        mSecurity.Type = NewPurchaseContentCISRadioBtn.Content.ToString();
                        mSecurity.Nshares = Double.Parse(NumberUnitsTextBox.Text);
                        mSecurity.Value = Double.Parse(UnitPriceTextBox.Text);
                    }

                    else if ((Boolean)NewPurchaseContentFundPlacementsAndOtherInvestmentsRadioBtn.IsChecked)
                    {
                        mSecurity.Type = NewPurchaseContentFundPlacementsAndOtherInvestmentsRadioBtn.Content.ToString();
                        mSecurity.Value = Double.Parse(NominalValueTextBox.Text);
                        mSecurity.DailyInterest = Double.Parse(DailyInterestTextBox.Text);
                        mSecurity.MaturityDate = DateUtils.TicksToMillis(((DateTime)MaturityDatePicker.SelectedDate).Ticks);
                    }
                    else if ((Boolean)NewPurchaseContentGovernmentBondRadioBtn.IsChecked)
                    {
                        mSecurity.Type = NewPurchaseContentGovernmentBondRadioBtn.Content.ToString();
                        mSecurity.Type = NewPurchaseContentFundPlacementsAndOtherInvestmentsRadioBtn.Content.ToString();
                        mSecurity.Value = Double.Parse(NominalValueTextBox.Text);
                        mSecurity.DailyInterest = Double.Parse(DailyInterestTextBox.Text);
                        mSecurity.MaturityDate = DateUtils.TicksToMillis(((DateTime)MaturityDatePicker.SelectedDate).Ticks);
                    }
                    else if ((Boolean)NewPurchaseContentGovernmentTBRadioBtn.IsChecked)
                    {
                        mSecurity.Type = NewPurchaseContentGovernmentTBRadioBtn.Content.ToString();
                        mSecurity.Type = NewPurchaseContentFundPlacementsAndOtherInvestmentsRadioBtn.Content.ToString();
                        mSecurity.Value = Double.Parse(NominalValueTextBox.Text);
                        mSecurity.DailyInterest = Double.Parse(DailyInterestTextBox.Text);
                        mSecurity.MaturityDate = DateUtils.TicksToMillis(((DateTime)MaturityDatePicker.SelectedDate).Ticks);
                    }

                    ReturnResult mReturnResult = await mDataLoader.AddSecurity(mSecurity) as ReturnResult;

                    NewPurchaseContentPropertyRadioBtn.IsChecked = true;
                    AssetNameTextBox.Text = null;

                }
            }

        }
        private void tryComplete(object sender, EventArgs e)
        { }

    }
}
