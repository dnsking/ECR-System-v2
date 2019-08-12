using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using ECR_System_v2.Loaders;
using ECR_System_v2.Utils;
using LiveCharts;
using LiveCharts.Wpf;

namespace ECR_System_v2.UserControls
{
    /// <summary>
    /// Interaction logic for LuseUserControl.xaml
    /// </summary>
    public partial class LuseUserControl : UserControl
    {
        private DataLoader mDataLoader;
        private Share[] mShares;
        private FormatedShare[] mFormatedShares;
        private String[] DaysArray = new String[] { "1", "7", "30", "90", "120", "365" };
        private Boolean hasHistory = false;
        public LuseUserControl()
        {
            InitializeComponent();
            if((Window.GetWindow(this) as MainWindow) != null)
            {

                mDataLoader = (Window.GetWindow(this) as MainWindow).getDataLoader();
                loadShares();
            }
        }
        public Share[] getShares()
        {
            return mShares;
        }

        public Share findShare(String name)
        {
            foreach (Share mShare in mShares) {
                if (mShare.LusaName.ToLower().Contains(name.ToLower())
                    | mShare.CompanyName.ToLower().Contains(name.ToLower()))
                {
                    return mShare;
                }
            }
            return null;
        }
        public void init()
        {
            mDataLoader = (Window.GetWindow(this) as MainWindow).getDataLoader();
            loadShares();
            SharesTimelineCombo.SelectionChanged += (a, b)=> loadShares(DaysArray[SharesTimelineCombo.SelectedIndex]);
        }
        private async void loadShares(String days="90")
        {
           // var Index = await mDataLoader.fetchSharesFor(days, App.LuseIndex) as Share[];
            mShares = await mDataLoader.fetchShares(days) as Share[];
            mFormatedShares = new FormatedShare[mShares.Length];
            int i = 0;
            foreach (Share mShare in mShares)
            {
                mFormatedShares[i] = new FormatedShare(mShare, getColorFromShare(mShare));
                i++;
            }
         //   Console.WriteLine("Index[0].LusaName " + Index[0].LusaName + " Index[0].Price " + Index[0].Price  + " Index[0].Change " + Index[0].Change + " Index[0].PercentageChange " + Index[0].PercentageChange);
         //   LuseValue.Text = Index[0].Price;
           // LuseChangeValue.Text = StringUtils.Format(Double.Parse(Index[0].Change));
           // LuseChangePer.Text   = StringUtils.Format(Double.Parse(Index[0].PercentageChange))+"%";



            SharePricesListView.ItemsSource = mFormatedShares;
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (EquitySection.SelectedIndex != 0)
            {

                GoBackBtn.IsEnabled = false;
                GoForwardBtn.IsEnabled = true;
                EquitySection.SelectedIndex = 0;
                UiUnits.AnimateSlider(EquitySection, 0);
            }
        }
        private void GoForward(object sender, RoutedEventArgs e)
        {
            if (hasHistory&&EquitySection.SelectedIndex != 1)
            {

                GoBackBtn.IsEnabled = true;
                GoForwardBtn.IsEnabled = false;
                EquitySection.SelectedIndex = 1;
                UiUnits.AnimateSlider(EquitySection, 1);
            }
        }
        private  void SelectEquity(object sender, RoutedEventArgs e) {
            Button btn = (Button)sender;
            String sec = ((TextBlock)((StackPanel)btn.Content).Children[1]).Text;
            Share simpleName = new Share();
            foreach(Share mShare in mShares)
            {
                if (mShare.LusaName.Equals(sec))
                    simpleName = mShare;
            }
            initChartValue(simpleName.TradeSummaryName, simpleName.CompanyName);
            EquitySection.SelectedIndex = 1;
            UiUnits.  AnimateSlider(EquitySection, 1);
            hasHistory = true;
            GoBackBtn.IsEnabled = true;
            GoForwardBtn.IsEnabled = false;
        }
        private async void initChartValue(String equity,String fullName, int days=7)
        {
            long start = DateUtils.TicksToMillis(DateTime.Now.Ticks);
// long end = DateUtils.TicksToMillis(DateTime.Now.AddMonths(-12).Ticks);
            List<String> DateLables = new List<string>();
            

            Double[] mValues = await mDataLoader.fetchSharesForRange(equity, start, 365, days) as Double[];
            for (int i = 0; i < mValues.Length; i++)
            {
                DateLables.Add(DateTime.Now.AddDays(-1 * (days * (i))).ToString("dd MMM yyyy"));
            }
            EquityValueCartesianChart.DisableAnimations = true;
            SeriesCollection mSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = fullName,
                    Values = new ChartValues<double> (mValues.Reverse() ),

                    Foreground=(SolidColorBrush)new BrushConverter().ConvertFromString("#424242"),
                    LineSmoothness = 1,
                    StrokeThickness=4,PointGeometry = null
                    ,Stroke=(SolidColorBrush)new BrushConverter().ConvertFromString("#9C27B0")
                    ,Fill=Brushes.Transparent
                }
            };

            EquityValueCartesianChart.AxisX[0].Title = "Date";
            EquityValueCartesianChart.AxisX[0].Labels = DateLables;

            EquityValueCartesianChart.AxisY[0].Title = "Market Value";
            EquityValueCartesianChart.Series = mSeriesCollection;

        }
        private String getColorFromShare(Share mShare)
        {
            float changePercentage = 0;
            try
            {
                 changePercentage = float.Parse(mShare.PercentageChange);
            }
            catch(Exception e) { }
            if (changePercentage < -30)
                return "#FF4D94";

            else if (changePercentage < -20)
                return "#F880CA";

            else if (changePercentage < -10)
                return "#F0B3FF";

            else if (changePercentage < -5)
                return "#D1C6FF";

            else if (changePercentage == 0)
                return "#B3D9FF";

            else if (changePercentage < 5)
                return "#A6ECE6";

            else if (changePercentage < 10)
                return "#99FFCC";

            else if (changePercentage < 20)
                return "#CCFFB3";

            else if (changePercentage < 50)
                return "#FFFF99";

            else if (changePercentage < 100)
                return "#FFFFCC";

            return "#26A69A";
        }
    }
}
