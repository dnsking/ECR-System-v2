using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;

namespace ECR_System_v2.Data
{
    public class ItemValue: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private String name;
        private String value;
        private Object secondaryValue;
        private Object thirdValue;
        private Object forthValue;
        public ItemValue() { }
        public ItemValue(String Name, String Value, Object ValueDouble)
        {
            this.name = Name;
            this.value = Value;
            this.secondaryValue = ValueDouble;
        }
        public ItemValue(String Name, String Value, Object ValueDouble, Object ValueDouble2, Object ValueDouble3) {
            this.name = Name;
            this.value = Value;
            this.secondaryValue = ValueDouble;
            this.thirdValue = ValueDouble2;
            this.forthValue = ValueDouble3;
        }
        public String Name
        {
            set
            {
                name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
            get { return name; }
        }
        public String Value { set {
                this.value = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            } get { return value; } }
        public Object SecondaryValue
        {
            set
            {
                secondaryValue = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SecondaryValue"));
            }
            get { return secondaryValue; }
        }
        public String SecondaryName { get; set; }

        public Object ThirdValue
        {
            set
            {
                thirdValue = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ThirdValue"));
            }
            get { return thirdValue; }
        }
        public String ThirdName { get; set; }
        public Object ForthValue
        {
            set
            {
                forthValue = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("forthValue"));
            }
            get { return forthValue; }
        }

    }
}
