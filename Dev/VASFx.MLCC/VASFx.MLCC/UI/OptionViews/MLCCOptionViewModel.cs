using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VASFx.Common.Model;
using VASFx.MLCC.Sqlite;
using VASFx.UI.OptionControl.UI;

namespace VASFx.MLCC.UI.OptionViews
{
    public class MLCCOptionViewModel : BindableBase
    {
        #region Properties

        SystemOptionView systemOptionsControl = null;
        public SystemOptionView SystemOptionsControl { get => this.systemOptionsControl; set => SetProperty(ref this.systemOptionsControl, value); }

        SystemSettingView systemSettingControl = null;
        public SystemSettingView SystemSettingControl { get => this.systemSettingControl; set => SetProperty(ref this.systemSettingControl, value); }

        CameraOptionControlView camerasOptionControl = null;
        public CameraOptionControlView CamerasOptionControl { get => this.camerasOptionControl; set => SetProperty(ref this.camerasOptionControl, value); }

        LightOptionControlView lightsOptionControl = null;
        public LightOptionControlView LightsOptionControl { get => this.lightsOptionControl; set => SetProperty(ref this.lightsOptionControl, value); }

        OverlapOptionControlView overlapOptionControl = null;
        public OverlapOptionControlView OverlapOptionControl { get => this.overlapOptionControl; set => SetProperty(ref this.overlapOptionControl, value); }

        private bool isSaving;
        public bool IsSaving { get => this.isSaving; set => SetProperty(ref this.isSaving, value); }

        IContainerProvider provider = null;
        SqlManager sql = null;

        #endregion

        #region ICommand

        public ICommand CloseCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        #endregion     

        public MLCCOptionViewModel(IContainerProvider prov, SqlManager sql)
        {
            this.provider = prov;
            this.sql = sql;

            this.CamerasOptionControl = provider.Resolve<CameraOptionControlView>();
            this.LightsOptionControl = provider.Resolve<LightOptionControlView>();
            this.OverlapOptionControl = provider.Resolve<OverlapOptionControlView>();
            this.SystemOptionsControl = provider.Resolve<SystemOptionView>();
            this.SystemSettingControl = provider.Resolve<SystemSettingView>();

            InitDelegeteCommand();
        }

        public void Init()
        {
            this.CamerasOptionControl.ViewModel.LoadCameraOption();
            this.LightsOptionControl.ViewModel.LoadControllerOption();
            this.SystemOptionsControl.ViewModel.LoadSystemOptions();
            this.SystemSettingControl.ViewModel.LoadSystemSetting();
            this.OverlapOptionControl.ViewModel.LoadOverlaprOption();

            //this.LoadOptions();
        }

        private void InitDelegeteCommand()
        {
            this.CloseCommand = new DelegateCommand(ExecuteCloseCommand);
            this.SaveCommand = new DelegateCommand(ExecuteSaveCommand);
        }

        private async void ExecuteSaveCommand()
        {
            this.IsSaving = true;

            this.CamerasOptionControl.ViewModel.SaveCameraOption();
            this.LightsOptionControl.ViewModel.SaveControllerOption();
            this.SystemOptionsControl.ViewModel.SaveSystemOptions();
            this.SystemSettingControl.ViewModel.SaveSystemSetting();
            this.OverlapOptionControl.ViewModel.SaveOverlaprOption();
            //this.SaveOptions();

            await Task.Delay(TimeSpan.FromSeconds(1));

            this.IsSaving = false;

            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void ExecuteCloseCommand()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void LoadOptions()
        {
            var ll = this.sql.SystemOption.GetAll().ToList();
            ll.ForEach(x =>
            {
                //if (!x.Name.Equals(SettingName.UseTimeSync) || !x.Name.Equals(SettingName.UseAlwaysLightOn) || !x.Name.Equals(SettingName.ShowOnlyOKCaliper))
                    this.SystemOptionsControl.ViewModel.OptionList.Add(new SelectableModel { Name = x.Name, Description = x.Desc, IsSelected = x.Value, Code = 'N' });
            });

            var rr = this.sql.SystemSetting.GetAll().ToList();
            rr.ForEach(x =>
            {
                //if (!x.Name.Equals(SettingName.UseTimeSync) || !x.Name.Equals(SettingName.UseAlwaysLightOn) || !x.Name.Equals(SettingName.ShowOnlyOKCaliper))
                    this.SystemSettingControl.ViewModel.SettingList.Add(new SystemSetting { Name = x.Name, Desc = x.Desc, Value = x.Value});
            });
        }

        private void SaveOptions()
        {
            var ll = this.SystemOptionsControl.ViewModel.OptionList.ToList();
            ll.ForEach(x =>
            {
                var item = sql.SystemOption.FindBy(i => i.Name.Equals(x.Name)).FirstOrDefault();
                if (x.IsSelected)
                    item.Value = true;
                else
                    item.Value = false;

                sql.SystemOption.Edit(item);
            });

            var settingll = this.SystemSettingControl.ViewModel.SettingList.ToList();
            settingll.ForEach(x =>
            {
                sql.SystemSetting.Edit(x);
            });
        }
    }
}
