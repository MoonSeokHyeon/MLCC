using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Model;
using VASFx.MLCC.Sqlite;

namespace VASFx.UI.OptionControl.UI
{
    public class SystemOptionViewModel : BindableBase
    {
        #region Properties
        private string groupBoxHeader;
        public string GroupBoxHeader
        {
            get { return groupBoxHeader; }
            set { SetProperty(ref this.groupBoxHeader, value); }
        }

        private ObservableCollection<SelectableModel> optionList = new ObservableCollection<SelectableModel>();
        public ObservableCollection<SelectableModel> OptionList
        {
            get { return optionList; }
            set { SetProperty(ref this.optionList, value); }
        }

        SqlManager sql = null;
        IContainerProvider provider = null;

        #endregion

        #region Struct
        public SystemOptionViewModel(IContainerProvider provider, SqlManager sql)
        {
            this.sql = sql;
            this.provider = provider;
        }

        public void Init()
        {
        }

        #endregion

        #region public Method
        public void LoadSystemOptions()
        {
            var ll = this.sql.SystemOption.GetAll().ToList();
            ll.ForEach(x =>
            {
                //if (!x.Name.Equals(ConstETCString.UseTimeSync) || !x.Name.Equals(ConstETCString.UseAlwaysLightOn) || !x.Name.Equals(ConstGraphicString.ShowOnlyOKCaliper))
                OptionList.Add(new SelectableModel { Name = x.Name, Description = x.Desc, IsSelected = x.Value, Code = 'N' });
            });
        }

        public void SaveSystemOptions()
        {
            var ll = this.OptionList.ToList();
            ll.ForEach(x =>
            {
                var item = sql.SystemOption.FindBy(i => i.Name.Equals(x.Name)).FirstOrDefault();

                item.Value = x.IsSelected;

                //if (!x.Name.Equals(ConstETCString.UseTimeSync) || !x.Name.Equals(ConstETCString.UseAlwaysLightOn) || !x.Name.Equals(ConstGraphicString.ShowOnlyOKCaliper))
                sql.SystemOption.Edit(item);
            });
        }

        #endregion
    }
}
