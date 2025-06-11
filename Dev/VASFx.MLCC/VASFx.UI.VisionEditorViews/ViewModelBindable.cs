using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.UI.VisionEditorViews
{
    public class ListItem : BindableBase
    {
        public EventHandler OnValueChanged;

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }      
    }
    public class ListBoxItem : BindableBase
    {
        public EventHandler OnValueChanged;

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private dynamic value;
        public dynamic Value
        {
            get { return value; }
            set
            {
                if (SetProperty(ref this.value, value))
                { OnValueChanged?.Invoke(this, null); }
            }
        }
    }
    public class ItemList : BindableBase
    {
        public EventHandler OnValueChanged;

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string data;
        public string Data
        {
            get { return data; }
            set { SetProperty(ref data, value); }
        }
    }
    public class ListBoxItem3Column : BindableBase
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private dynamic value1;
        public dynamic Value1
        {
            get { return value1; }
            set { SetProperty(ref this.value1, value); }
        }

        private dynamic value2;
        public dynamic Value2
        {
            get { return value2; }
            set { SetProperty(ref this.value2, value); }
        }

        private string unit;
        public string Unit
        {
            get { return unit; }
            set { SetProperty(ref this.unit, value); }
        }
    }
}
