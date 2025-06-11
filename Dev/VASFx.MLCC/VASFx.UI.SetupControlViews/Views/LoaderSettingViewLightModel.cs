using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace VASFx.UI.SetupControlViews.Views
{
    public class LoaderSettingViewLightModel : BindableBase
    {

        #region Properties

        public TextBox chanel1Value;
        public TextBox Chanel1Value
        {
            get { return chanel1Value; }
            set { SetProperty(ref chanel1Value, value); }
        }

        public TextBox chanel2Value;
        public TextBox Chanel2Value
        {
            get { return chanel2Value; }
            set { SetProperty(ref chanel2Value, value); }
        }

        #endregion

        #region ICommand

        public ICommand Chanel1 { get; set; }
        public ICommand Chanel2 { get; set; }

        #endregion
        public string Title => "LoaderSettingViewLightModel";

        public event Action<IDialogResult> RequestClose;

        public LoaderSettingViewLightModel()
        {
            Chanel1 = new DelegateCommand<string>(ExecuteChanel1Command);
            Chanel2 = new DelegateCommand<string>(ExecuteChanel2Command);
        }

        internal void Init()
        {

        }

        private void ExecuteChanel1Command(string obj)
        {
            if (obj.Equals("Up"))
            {
                double value = Double.Parse(Chanel1Value.Text.ToString()) + 1;
                if (value >= 255)
                    Chanel1Value.Text = "255";
                else
                    Chanel1Value.Text = value.ToString();
            }
            else
            {
                double value = Double.Parse(Chanel1Value.Text.ToString()) - 1;
                if (value <= 0)
                    Chanel1Value.Text = "0";
                else
                    Chanel1Value.Text = value.ToString();
            }

        }

        private void ExecuteChanel2Command(string obj)
        {
            if (obj.Equals("Up"))
            {
                double value = Double.Parse(Chanel2Value.Text.ToString()) + 1;
                if (value >= 255)
                    Chanel2Value.Text = "255";
                else
                    Chanel2Value.Text = value.ToString();
            }
            else
            {
                double value = Double.Parse(Chanel2Value.Text.ToString()) - 1;
                if (value <= 0)
                    Chanel2Value.Text = "0";
                else
                    Chanel2Value.Text = value.ToString();
            }
        }

    }
}
