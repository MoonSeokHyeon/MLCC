using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VASFx.Common.Model;
using VASFx.MLCC.Sqlite;

namespace VASFx.UI.OptionControl.UI
{
    public class LightOptionControlViewModel : BindableBase
    {
        #region Propertes
        private ObservableCollection<LightControllerData> controllerList;
        public ObservableCollection<LightControllerData> ControllerList
        {
            get { return this.controllerList; }
            set { SetProperty(ref this.controllerList, value); }
        }

        private LightControllerData seletedControllerConfig;
        public LightControllerData ControllerSelected
        {
            get { return seletedControllerConfig; }
            set { 
                if(SetProperty(ref this.seletedControllerConfig, value))
                {
                    LoadLightOption(seletedControllerConfig);
                }
            }
        }

        private ObservableCollection<LightValueData> lightDataList;
        public ObservableCollection<LightValueData> LightDataList
        {
            get { return this.lightDataList; }
            set { SetProperty(ref this.lightDataList, value); }
        }

        private LightValueData seletedLightConfig;
        public LightValueData SelectedLightConfig
        {
            get { return seletedLightConfig; }
            set { SetProperty(ref this.seletedLightConfig, value); }
        }
        List<LightControllerData> _lightControlValues { get; set; }
        int beforeSelected = 0;

        SqlManager sql = null;
        IContainerProvider provider = null;

        #endregion

        #region ICommand

        public ICommand AddLightControllerCommand { get; set; }
        public ICommand DeleteLightControllerCommand { get; set; }

        public ICommand AddLightCommand { get; set; }
        public ICommand DeleteLightCommand { get; set; }

        #endregion

        #region Contruct
        public LightOptionControlViewModel(IContainerProvider provider, SqlManager sql)
        {
            this.sql = sql;
            this.provider = provider;

            this.AddLightControllerCommand = new DelegateCommand(ExecuteAddLightControllerCommand);
            this.DeleteLightControllerCommand = new DelegateCommand(ExecuteDeleteLightControllerCommand);

            this.AddLightCommand = new DelegateCommand(ExecuteAddLightCommand);
            this.DeleteLightCommand = new DelegateCommand(ExecuteDeleteLightCommand);
        }

        public void Init()
        {
        }

        #endregion

        #region Public Method
        public void LoadControllerOption()
        {
            var modelData = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;

            _lightControlValues = sql.LightController.GetAll().Where(x => x.ModelData.Equals(modelData)).ToList();

            this.ControllerList = new ObservableCollection<LightControllerData>(_lightControlValues);
            this.ControllerSelected = this.ControllerList.First();
        }

        public void SaveControllerOption()
        {
            var modelData = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;
            var controllerDatas = modelData.LightControllerDatas.ToList();

            controllerDatas.ForEach(x =>
            {
                var item = this.ControllerList.FirstOrDefault(i => i.Id.Equals(x.Id));

                if (item == null)
                {
                    var deItem = controllerDatas.FirstOrDefault(i => i.Id.Equals(x.Id));

                    sql.LightController.Delete(deItem.Id);
                }
            });

            this.ControllerList.ToList().ForEach(x =>
            {
                var item = controllerDatas.FirstOrDefault(i => i.Id.Equals(x.Id));

                if (item == null)
                    modelData.LightControllerDatas.Add(x);
            });

            modelData.LightControllerDatas = this.ControllerList.ToList();
            sql.ModelData.Edit(modelData);
        }
       
        public void LoadLightOption(LightControllerData before)
        {
            if (this.seletedControllerConfig == null)
            {
                this.LightDataList.Clear();

                return;
            }

            if (beforeSelected != 0)
                _lightControlValues.FirstOrDefault(x => x.Id.Equals(beforeSelected)).LightValues = this.LightDataList.ToList();

            var controller = _lightControlValues.FirstOrDefault(x => x.Id.Equals(this.seletedControllerConfig.Id));

            if (controller == null)
            {
                controller = new LightControllerData();

                this._lightControlValues.Add(controller);
            }

            this.LightDataList = new ObservableCollection<LightValueData>(controller.LightValues);

            beforeSelected = this.seletedControllerConfig.Id;

            if (this.LightDataList.Count != 0)
                this.SelectedLightConfig = this.LightDataList.First();
        }

        public void SaveLightOption()
        {
            var modelData = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;
            var LightControllers = modelData.LightControllerDatas.ToList();

            LightControllers.ForEach(x =>
            {
                var item = _lightControlValues.FirstOrDefault(i => i.Id.Equals(x.Id));

                if (item == null)
                {
                    sql.LightController.Delete(x);
                }
            });

            LightControllers = this._lightControlValues;
            sql.ModelData.Edit(modelData);
        }
        #endregion

        #region Cammand Method
        private void ExecuteAddLightControllerCommand()
        {
            var createLightController = new LightControllerData();
            createLightController.ModelData = sql.SystemInfo.GetAll().First().CurrentModel;

            this.ControllerList.Add(createLightController);
        }
        private void ExecuteDeleteLightControllerCommand()
        {
            this.ControllerList.Remove(this.ControllerSelected);
        }
        private void ExecuteAddLightCommand()
        {
            
        }

        private void ExecuteDeleteLightCommand()
        {
        }

        #endregion
    }
}
