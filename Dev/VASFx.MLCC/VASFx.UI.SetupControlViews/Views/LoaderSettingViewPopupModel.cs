using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using VASFx.UI.CogDisplayViews.Views;

namespace VASFx.UI.SetupControlViews.Views
{
    public class LoaderSettingViewPopupModel : BindableBase
    {
        #region Properties

        public CogDisplaySingleView camViewHost;
        public CogDisplaySingleView CamViewHost
        {
            get { return camViewHost; }
            set { SetProperty(ref camViewHost, value); }
        }

        public string _camNum;
        public string CamNum
        {
            get { return _camNum; }
            set { SetProperty(ref _camNum, value); }
        }

        public LoaderSettingViewParams _loaderSettingViewParamsDisplay;
        public LoaderSettingViewParams LoaderSettingViewParamsDisplay
        {
            get { return _loaderSettingViewParamsDisplay; }
            set { SetProperty(ref _loaderSettingViewParamsDisplay, value); }
        }

        public LoaderSettingViewLight _loaderSettingViewLightDisplay;
        public LoaderSettingViewLight LoaderSettingViewLightDisplay
        {
            get { return _loaderSettingViewLightDisplay; }
            set { SetProperty(ref _loaderSettingViewLightDisplay, value); }
        }

        public LoaderSettingViewFocus _loaderSettingViewFocusDisplay;
        public LoaderSettingViewFocus LoaderSettingViewFocusDisplay
        {
            get { return _loaderSettingViewFocusDisplay; }
            set { SetProperty(ref _loaderSettingViewFocusDisplay, value); }
        }

        #endregion



        #region ICommand
        public ICommand Close { get; set; }
        

        #endregion


        IContainerProvider provider;

        public LoaderSettingViewPopupModel(IContainerProvider provider)
        {
            this.provider = provider;
            Close = new DelegateCommand<string>(ExecuteCloseCommand);

        }

        internal void Init()
        {
            this.CamViewHost = provider.Resolve<CogDisplaySingleView>();
            LoaderSettingViewParamsDisplay = provider.Resolve<LoaderSettingViewParams>();
            LoaderSettingViewLightDisplay = provider.Resolve<LoaderSettingViewLight>();
            LoaderSettingViewFocusDisplay = provider.Resolve<LoaderSettingViewFocus>();
        }

        private void ExecuteCloseCommand(string obj)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
