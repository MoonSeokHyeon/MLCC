using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.UI.Interactivity
{
    public class NotificationViewModel : BindableBase
    {
        private string message;
        public string Message
        {
            get { return message; }
            set { this.SetProperty(ref this.message, value); }
        }

        public NotificationViewModel()
        {
        }
    }
}
