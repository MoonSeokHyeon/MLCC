using GSG.NET.Concurrent;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VASFx.Common.Events;
using VASFx.Common.Model;
using VASFx.MLCC.Sqlite;

namespace VASFx.UI.Interactivity
{
    public class ModelManagerViewModel : BindableBase
    {
        #region Properties
        private List<ModelData> dataList;
        public List<ModelData> DataList
        {
            get { return dataList; }
            set { SetProperty(ref this.dataList, value); }
        }

        private List<LightValueData> lightControlValueDatas;
        public List<LightValueData> LightControlValueDatas
        {
            get { return lightControlValueDatas; }
            set { SetProperty(ref this.lightControlValueDatas, value); }
        }

        private ModelData selectedModel;
        public ModelData SelectedModel
        {
            get { return selectedModel; }
            set
            {
                if (SetProperty(ref this.selectedModel, value))
                {
                    if (value != null)
                    {
                        var lightControlDatas = value.LightControllerDatas.OrderBy(d => d.PortNumber).ToList();
                        var lughtValueDats = new List<LightValueData>();

                        foreach (var controlData in lightControlDatas)
                        {
                            foreach (var lightValueData in controlData.LightValues)
                            {
                                lughtValueDats.Add(lightValueData);
                            }
                        }
                        this.LightControlValueDatas = lughtValueDats;
                    }
                }
            }
        }

        private bool isSaving;
        public bool IsSaving { get => this.isSaving; set => SetProperty(ref this.isSaving, value); }

        SqlManager sql = null;
        IContainerProvider provider = null;
        GUIMessageEvent DBTableChange = null;

        #endregion

        #region ICammand
        public ICommand CloseCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CreateModelCommand { get; set; }
        public ICommand EditModelCommand { get; set; }
        public ICommand DeleteModelCommand { get; set; }
        #endregion

        #region Struct
        public ModelManagerViewModel(SqlManager sql, IContainerProvider provider, IEventAggregator ea)
        {
            this.sql = sql;
            this.provider = provider;

            this.SaveCommand = new DelegateCommand(ExecuteSaveCommand);
            this.CloseCommand = new DelegateCommand(ExecuteCloseCommand);

            this.CreateModelCommand = new DelegateCommand(ExecuteCreateCommand);
            this.DeleteModelCommand = new DelegateCommand(ExecuteDeleteCommand);

            DBTableChange = ea.GetEvent<GUIMessageEvent>();
        }
        internal void Init()
        {
            this.DataList = this.sql.ModelData.GetAll().ToList();
            this.SelectedModel = this.DataList.FirstOrDefault();

            var lightControlDatas = this.SelectedModel.LightControllerDatas.OrderBy(d => d.PortNumber).ToList();
            var lughtValueDats = new List<LightValueData>();

            foreach (var controlData in lightControlDatas)
            {
                foreach (var lightValueData in controlData.LightValues)
                {
                    lughtValueDats.Add(lightValueData);
                }
            }
            this.LightControlValueDatas = lughtValueDats;
        }

        #endregion

        #region Command
        private async void ExecuteCreateCommand()
        {
            var view = provider.Resolve<InputValueView>();
            view.ViewModel.InputValueName = "New Model Name";

            var result = await DialogHost.Show(view, "ModelManagerDialog") as bool?;
            if (result == true)
            {
                var newModel = new ModelData();
                newModel.Name = view.ViewModel.InputValue.Trim();

                newModel.ZoneDatas = new List<ZoneData>();
                this.SelectedModel.ZoneDatas.ForEach(c =>
                {
                    var newZoneData = new ZoneData()
                    {
                        Zone = c.Zone,
                        Score = c.Score,
                    };

                    newModel.ZoneDatas.Add(newZoneData);
                });

                newModel.LightControllerDatas = new List<LightControllerData>();
                this.SelectedModel.LightControllerDatas.ForEach(d =>
                {
                    var newLightController = new LightControllerData()
                    {
                        PortNumber = d.PortNumber,
                        BaudRate = d.BaudRate,
                        Parity = d.Parity,
                        DataBits = d.DataBits,
                        StopBits = d.StopBits,
                        MaxChannel = d.MaxChannel,
                        MaxVolume = d.MaxVolume,
                    };

                    d.LightValues.ForEach(x =>
                    {
                        newLightController.LightValues.Add(new LightValueData()
                        {
                            Channel = x.Channel,
                            ZoneID = x.ZoneID,
                            GrabPos = x.GrabPos,
                            LightValue = x.LightValue,
                        });
                    });

                    newModel.LightControllerDatas.Add(newLightController);
                });

                sql.ModelData.Add(newModel);
                LockUtils.Lock(100);
                this.DataList = sql.ModelData.GetAll().ToList();
                this.SelectedModel = newModel;

                var currentModelName = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel.Name;
                var sourcePath = @"D:\Data\" + currentModelName;
                var destPath = @"D:\Data\" + newModel.Name;

                if (Directory.Exists(sourcePath))
                    GSG.NET.FileSystem.FileUtils.DirectoryCopy(sourcePath, destPath, true);

                var msg = new GUIEventArgs()
                {
                    MessageKind = Common.Shared.eGUIMessageKind.ChangeModelList,
                };

                DBTableChange.Publish(msg);
            }
        }
        private async void ExecuteDeleteCommand()
        {
            var view = provider.Resolve<ComfirmationView>();
            view.ViewModel.Message = " Do you want to Model delete ? ";

            var result = await DialogHost.Show(view, "ModelManagerDialog") as bool?;
            if (result == true)
            {
                if (this.DataList.Count == 1) return;

                var deleteModel = this.SelectedModel;
                sql.ModelData.Delete(deleteModel.Id);

                GSG.NET.FileSystem.FileUtils.DeleteFileIfExist($@"D:\Data\{deleteModel.Name}");

                this.DataList = this.sql.ModelData.GetAll().ToList();
                this.SelectedModel = this.DataList.FirstOrDefault();

                var msg = new GUIEventArgs()
                {
                    MessageKind = Common.Shared.eGUIMessageKind.ChangeModelList,
                };

                DBTableChange.Publish(msg);
            }
        }
        private void ExecuteCloseCommand()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async void ExecuteSaveCommand()
        {
            this.IsSaving = true;

            this.DataList.ForEach(x =>
            {
                sql.ModelData.Edit(x);
            });

            await Task.Delay(TimeSpan.FromSeconds(1));

            this.IsSaving = false;

            DialogHost.CloseDialogCommand.Execute(null, null);
        }
        #endregion
    }
}
