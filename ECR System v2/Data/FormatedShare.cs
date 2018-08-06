using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ECR_System_v2.Utils;
using MahApps.Metro.IconPacks;

namespace ECR_System_v2.Data
{
    public class FormatedShare
    {
        public FormatedShare(Share mShare,String color)
        {
            LusaName = mShare.LusaName;
            Price = MathUtils.round(Double.Parse(mShare.Price), 2).ToString() +" ZMW";
            TradeSummaryName = mShare.TradeSummaryName;
            CompanyName = mShare.CompanyName;
            Change = MathUtils.round(Double.Parse(mShare.Change),2).ToString() ;
            Date = mShare.Date;
            PercentageChange = MathUtils.round(Double.Parse(mShare.PercentageChange), 2).ToString() +" %";
            ChangeDate = mShare.ChangeDate;
            Color =(SolidColorBrush)(new BrushConverter().ConvertFrom(color)) ;
            if (Double.Parse(mShare.Change) < 0) { Arrow = PackIconOcticonsKind.ArrowSmallDown; }
           else if (Double.Parse(mShare.Change) > 0) { Arrow = PackIconOcticonsKind.ArrowSmallUp; }
           else if (Double.Parse(mShare.Change) == 0) { Arrow = PackIconOcticonsKind.Dash; }
        }
        public PackIconOcticonsKind Arrow { set; get; }
        public String LusaName { set; get; }
        public String Price { set; get; }
        public String TradeSummaryName { set; get; }
        public String CompanyName { set; get; }
        public String Change { set; get; }
        public String Date { set; get; }
        public String PercentageChange { set; get; }
        public String ChangeDate { set; get; }

        public Brush Color { set; get; }
    }
}
