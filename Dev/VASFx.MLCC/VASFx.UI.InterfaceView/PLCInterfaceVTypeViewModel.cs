using GSG.NET.Extensions;
using GSG.NET.PLC.Mitsubishi;
using GSG.NET.PLC.Model;
using GSG.NET.PLC.SLMP;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VASFx.UI.Interactivity;
using VASFx.UI.InterfaceView.DataModel;

namespace VASFx.UI.InterfaceView
{
    public class PLCInterfaceVTypeViewModel : BindableBase
    {

        ObservableCollection<PLCBlockItem> toDataList = new ObservableCollection<PLCBlockItem>();
        public ObservableCollection<PLCBlockItem> ToDataList
        {
            get => this.toDataList;
            set => SetProperty(ref this.toDataList, value);
        }

        ObservableCollection<PLCBlockItem> fromDataList = new ObservableCollection<PLCBlockItem>();
        public ObservableCollection<PLCBlockItem> FromDataList
        {
            get => this.fromDataList;
            set => SetProperty(ref this.fromDataList, value);
        }

        private PLCBlockItem selectedItem;
        public PLCBlockItem SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref this.selectedItem, value); }
        }

        public ICommand ChangeBlockStateCommand { get; set; }

        public string SubText { get; set; }

        IContainerProvider provider = null;
        SlmpManager plc = null;

        public PLCInterfaceVTypeViewModel(IContainerProvider provider, SlmpManager plc)
        {
            this.provider = provider;
            this.plc = plc; 

            this.ChangeBlockStateCommand = new DelegateCommand<object>(ExecuteChageBlockStateCommand);
        }

        bool isInited = false;
        public void Init()
        {
            if (isInited) return;
            isInited = true;    

            //PLC 에 아직 Group Add 가 되지 않았으면 Event 로 전환 필요.
            var bitList = plc.GetGroup("B1").BlockList;
            bitList.ForEach(bit =>
            {
                if (bit.SubText.Equals(this.SubText))
                {
                    if (bit.Name.StartsWith("TO_"))
                        this.ToDataList.Add(new PLCBlockItem { Addr = bit.DspAddr, Tag = bit.Name, Value = bit.Value, IsBit = true });

                    if (bit.Name.StartsWith("FR_"))
                        this.FromDataList.Add(new PLCBlockItem { Addr = bit.DspAddr, Tag = bit.Name, Value = bit.Value, IsBit = true });
                }
            });

            var wordList = plc.GetGroup("W1").BlockList;
            wordList.ForEach(word =>
            {
                if (word.SubText.Equals(this.SubText))
                {
                    if (word.Name.StartsWith("TO_"))
                        this.ToDataList.Add(new PLCBlockItem { Addr = word.DspAddr, Tag = word.Name, Value = word.Value, IsBit = false });

                    if (word.Name.StartsWith("FR_"))
                        this.FromDataList.Add(new PLCBlockItem { Addr = word.DspAddr, Tag = word.Name, Value = word.Value, IsBit = false });
                }
            });

            plc.OnBitChanged += Plc_OnBitChanged;
            plc.OnWordChanged += Plc_OnWordChanged;
        }

        private void Plc_OnWordChanged(WordBlock block)
        {
            if (!block.SubText.Equals(this.SubText)) return; 

            {
                var item = this.ToDataList.FirstOrDefault(x => x.Tag.Equals(block.Name));
                if (item != null)
                {
                    item.Value = block.Value;
                    return;
                }
            }

            {
                var item = this.FromDataList.FirstOrDefault(x => x.Tag.Equals(block.Name));
                if (item != null)
                {
                    item.Value = block.Value;
                    return;
                }
            }
        }

        private void Plc_OnBitChanged(BitBlock block)
        {
            if (!block.SubText.Equals(this.SubText)) return;

            {
                var item = this.ToDataList.FirstOrDefault(x => x.Tag.Equals(block.Name));
                if (item != null)
                {
                    item.Value = block.Value;
                    return;
                }
            }

            {
                var item = this.FromDataList.FirstOrDefault(x => x.Tag.Equals(block.Name));
                if (item != null)
                {
                    item.Value = block.Value;
                    return;
                }
            }
        }

        async void ExecuteChageBlockStateCommand(object obj)
        {
            var item = CastTo<PLCBlockItem>.From(obj);

            if (item.IsBit)
            {
                var view = this.provider.Resolve<ComfirmationView>();
                view.ViewModel.Message = item.IsOn ? $"[{item.Addr}]{item.Tag} Bit Off ?" : $"[{item.Addr}]{item.Tag} Bit On ?";

                var result = await DialogHost.Show(view, "RootDialog") as bool?;
                if (result == true)
                    plc.WriteBit(item.Tag, item.IsOn ? false : true);
            }
            else
            {
                var view = this.provider.Resolve<InputValueView>();
                view.ViewModel.InputValueName = $"[{item.Addr}]" + item.Tag;
                view.ViewModel.CurrentValue = item.Value;

                var result = await DialogHost.Show(view, "RootDialog") as bool?;
                if ( result == true)
                {
                    double ret = 0d;
                    if ( double.TryParse(view.ViewModel.InputValue.Trim(), out ret))
                    {
                        plc.WriteWord(item.Tag, ret.ToString());
                    }
                    else
                    {
                        var notificationView = this.provider.Resolve<NotificationView>();
                        notificationView.ViewModel.Message = "Invalid Input Data !";
                        await DialogHost.Show(notificationView, "RootDialog");
                    }
                }
            }
        }

    }
}
