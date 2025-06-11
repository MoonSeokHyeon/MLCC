using Prism.Mvvm;

namespace VASFx.UI.Interactivity
{
    public class InputValueViewModel : BindableBase
    {
        private string inputValue;
        public string InputValue
        {
            get { return inputValue; }
            set { SetProperty(ref this.inputValue, value); }
        }

        private string inputValueName;
        public string InputValueName
        {
            get { return inputValueName; }
            set { SetProperty(ref this.inputValueName, value); }
        }

        private string currentValue;

        public string CurrentValue
        {
            get { return currentValue; }
            set { SetProperty(ref this.currentValue, value); }
        }

        public InputValueViewModel()
        {
        }
    }
}
