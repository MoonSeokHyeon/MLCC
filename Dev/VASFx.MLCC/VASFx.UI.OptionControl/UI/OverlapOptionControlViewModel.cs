using GSG.NET.Extensions;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using VASFx.MLCC.Sqlite;

namespace VASFx.UI.OptionControl.UI
{
    #region Properties
    public class OverlapOptionControlViewModel : BindableBase
    {
        private ObservableCollection<OverlapInfo> overlapDataList;
        public ObservableCollection<OverlapInfo> OverlapDataList
        {
            get { return this.overlapDataList; }
            set { SetProperty(ref this.overlapDataList, value); }
        }

        private OverlapInfo selectedOverlapConfig;
        public OverlapInfo SelectedOverlapConfig
        {
            get { return selectedOverlapConfig; }
            set { SetProperty(ref this.selectedOverlapConfig, value); }
        }

        SqlManager sql = null;
        IContainerProvider provider = null;

        #endregion

        #region Struct
        public OverlapOptionControlViewModel(SqlManager sql, IContainerProvider provider, IEventAggregator ea)
        {
            this.sql = sql;
            this.provider = provider;
        }
        public void Init()
        {
        }

        #endregion

        #region Public Method
        public void LoadOverlaprOption()
        {
            var overlapData = sql.OverlapInfo.GetAll().ToList();
            this.OverlapDataList = new ObservableCollection<OverlapInfo>(overlapData);

            this.SelectedOverlapConfig = this.OverlapDataList.First();
        }
        public void SaveOverlaprOption()
        {
            var overlapList = sql.OverlapInfo.GetAll().OrderBy(x => x.Id).ToList();

            overlapList.ForEach(x =>
            {
                var item = this.OverlapDataList.FirstOrDefault(i => i.Id.Equals(x.Id));

                if (item == null)
                {
                    this.sql.CameraInfo.Delete(x.Id);
                }
            });

            this.OverlapDataList.ToList().ForEach(x =>
            {
                var item = sql.OverlapInfo.GetAll().OrderBy(i => i.Id).ToList().FirstOrDefault(j => j.Id.Equals(x.Id));

                if (item == null)
                    this.sql.OverlapInfo.Add(x);

                item = x;

                sql.OverlapInfo.Edit(item);
            });
        }

        #endregion
    }
}
