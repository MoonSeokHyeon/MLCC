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
using VASFx.MLCC.Sqlite;

namespace VASFx.UI.OptionControl.UI
{
    public class CameraOptionControlViewModel : BindableBase
    {
        #region Properties

        private string optionGroupBoxHeader;
        public string OptionGroupBoxHeader
        {
            get { return optionGroupBoxHeader; }
            set { SetProperty(ref this.optionGroupBoxHeader, value); }
        }


        private ObservableCollection<CameraInfo> dataList;
        public ObservableCollection<CameraInfo> DataList
        {
            get { return this.dataList; }
            set { SetProperty(ref this.dataList, value); }
        }

        private CameraInfo seletedCamConfig;
        public CameraInfo SelectedCamConfig
        {
            get { return seletedCamConfig; }
            set { SetProperty(ref this.seletedCamConfig, value); }
        }     

        private bool isSaving;
        public bool IsSaving { get => this.isSaving; set => SetProperty(ref this.isSaving, value); }

        List<LightControllerData> _lightControlValues { get; set; }
        int beforeSelected = 0;

        IContainerProvider provider = null;
        SqlManager sql = null;

        #endregion

        #region ICommand

        public ICommand CreateCamCommand { get; set; }
        public ICommand DeleteCamCommand { get; set; }

        #endregion

        #region Construct
        public CameraOptionControlViewModel(SqlManager sql, IContainerProvider provider, IEventAggregator ea)
        {
            this.sql = sql;
            this.provider = provider;

            this.CreateCamCommand = new DelegateCommand(ExecuteCrateCamCommand);
            this.DeleteCamCommand = new DelegateCommand(ExecuteDeleteCamCommand);
        }

        public void Init()
        {
        }

        #endregion

        #region Public Method
        public void LoadCameraOption()
        {
            var cameraList = sql.CameraInfo.GetAll().ToList().OrderBy(x => x.Id).ToList();

            this.DataList = new ObservableCollection<CameraInfo>(cameraList);
            this.SelectedCamConfig = this.DataList.First();
        }

        public void SaveCameraOption()
        {
            var cameraList = sql.CameraInfo.GetAll().ToList().OrderBy(x => x.Id).ToList();

            cameraList.ForEach(x =>
            {
                var item = this.DataList.FirstOrDefault(i => i.Id.Equals(x.Id));

                if (item == null)
                {
                    this.sql.CameraInfo.Delete(x.Id);
                }
            });

            this.DataList.ToList().ForEach(x =>
            {
                var item = sql.CameraInfo.GetAll().ToList().OrderBy(i => i.Id).ToList().FirstOrDefault(j => j.Id.Equals(x.Id));

                if (item == null)
                    this.sql.CameraInfo.Add(x);

                item = x;

                sql.CameraInfo.Edit(item);
            });
        }

        #endregion

        #region Command Method
        private void ExecuteDeleteCamCommand()
        {
            var camerall = this.dataList.ToList();
            camerall.Remove(this.SelectedCamConfig);

            this.DataList = new ObservableCollection<CameraInfo>(camerall.OrderBy(x => x.Id).ToList());
        }

        private void ExecuteCrateCamCommand()
        {
            //var camerall = sql.CameraInfo.GetAll().ToList().OrderBy(x => x.Id).ToList();
            var camerall = this.DataList;
            var newCamera = new CameraInfo();

            camerall.Add(newCamera);

            this.DataList = new ObservableCollection<CameraInfo>(camerall.OrderBy(x => x.Id).ToList());
        }

        #endregion
    }
}
