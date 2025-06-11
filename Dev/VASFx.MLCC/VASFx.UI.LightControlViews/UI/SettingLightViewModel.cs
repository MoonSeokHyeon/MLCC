using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VASFx.Common.Model;
using VASFx.Core;
using VASFx.MLCC.Sqlite;

namespace VASFx.UI.LightControlViews.UI
{
    public class SettingLightViewModel : BindableBase
    {
        private ObservableCollection<LightControllerData> lightControllerData;
        public ObservableCollection<LightControllerData> LightControllerData
        {
            get { return lightControllerData; }
            set { SetProperty(ref this.lightControllerData, value); }
        }
        private ObservableCollection<LightValueData> ligthValueList;
        public ObservableCollection<LightValueData> LigthValueList
        {
            get { return ligthValueList; }
            set { SetProperty(ref this.ligthValueList, value); }
        }
        private LightValueData selectedLigthValue;
        public LightValueData SelectedLigthValue
        {
            get { return selectedLigthValue; }
            set 
            { 
                if(SetProperty(ref this.selectedLigthValue, value))
                {
                    ChangeCannel();
                }

            }
        }
        private int targetValue;
        public int TargetValue
        {
            get { return targetValue; }
            set { if (SetProperty(ref this.targetValue, value)) { /*this.controller.LightOn( this.Chennel, this.TargetValue );*/ } }
        }

        public int PortId { get; set; } 
        public int Channel { get; set; }

        #region Command
        public ICommand SetLightValueCommand { get; set; }
        public ICommand OffLightValueCommand { get; set; }
        public ICommand SaveLightValueCommand { get; set; }
        public ICommand PlusLightValueCommand { get; set; }
        public ICommand MinusLightValueCommand { get; set; }
        #endregion

        SqlManager sql = null;
        IContainerProvider provider = null;
        LightControlManager LightControlManager = null;
        public SettingLightViewModel(SqlManager sql, IContainerProvider provider, IEventAggregator ea, LightControlManager lightControlManager)
        {
            this.sql = sql;
            this.provider = provider;
            this.LightControlManager = lightControlManager;

            SetLightValueCommand = new DelegateCommand(ExecuteSetLightValueCommand);
            OffLightValueCommand = new DelegateCommand(ExecuteOffLightValueCommand);
            SaveLightValueCommand = new DelegateCommand(ExecuteSaveLightValueCommand);
            PlusLightValueCommand = new DelegateCommand(ExecutePlusLightValueCommand);
            MinusLightValueCommand = new DelegateCommand(ExecuteLightValueCommand);
        }

        public void Init()
        {
            var modelData = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;

            this.lightControllerData = new ObservableCollection<LightControllerData>(modelData.LightControllerDatas.ToList().OrderBy(x => x.Id).ToList());
            var controllerDatas = lightControllerData.FirstOrDefault(x => x.PortNumber.Equals(PortId));

            this.LigthValueList = new ObservableCollection<LightValueData>(controllerDatas.LightValues.OrderBy(x => x.Id).ToList());

            this.SelectedLigthValue = LigthValueList.FirstOrDefault(x => x.Channel.Equals(Channel));

            TargetValue = SelectedLigthValue.LightValue;
        }
        private void ChangeCannel()
        {
            if (this.SelectedLigthValue == null) return;

            var currentValue = LigthValueList.Where(x => x.Channel.Equals(SelectedLigthValue.Channel)).FirstOrDefault();
            this.Channel = currentValue.Channel;
            TargetValue = currentValue.LightValue;
        }
        public void ExecuteSetLightValueCommand()
        {
            LightControlManager.SetLightValue(PortId, Channel, TargetValue);
            //LightControlManager.SetInspectionLightOn();
        }
        public void ExecuteOffLightValueCommand()
        {
            LightControlManager.SetLightOff(PortId, Channel);
            //LightControlManager.SetInspectionLightOff();
        }
        private void ExecuteSaveLightValueCommand()
        {
            var modelData = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;
            SelectedLigthValue.LightValue = TargetValue;

            sql.ModelData.Edit(modelData);
        }

        private void ExecutePlusLightValueCommand()
        {
            if (TargetValue < 255)
                TargetValue += 1;

            LightControlManager.SetLightValue(PortId, Channel, TargetValue);
        }
        private void ExecuteLightValueCommand()
        {
            if (TargetValue > 0)
                TargetValue -= 1;

            LightControlManager.SetLightValue(PortId, Channel, TargetValue);
        }

    }
}
