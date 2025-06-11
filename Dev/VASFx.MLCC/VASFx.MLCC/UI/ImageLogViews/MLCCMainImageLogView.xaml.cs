using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VASFx.Common.Shared;

namespace VASFx.MLCC.UI.ImageLogViews
{
    /// <summary>
    /// MLCCMainImageLogView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MLCCMainImageLogView : UserControl
    {
        public MLCCMainImageLogViewModel ViewModel => this.DataContext as MLCCMainImageLogViewModel;

        public MLCCMainImageLogView()
        {
            InitializeComponent();
            this.Loaded += MLCCMainImageLogView_Loaded;
        }
        private void MLCCMainImageLogView_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();

            this.ItemList.Items.Clear();

            if (!Directory.Exists(ConstLogString.ImageBackUpPath))
                Directory.CreateDirectory(ConstLogString.ImageBackUpPath);

            foreach (string str in Directory.GetDirectories(ConstLogString.ImageBackUpPath))   // 특정폴더
            //foreach (string str in Directory.GetLogicalDrives())   // 루트폴더
            {
                try
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = str;
                    item.Tag = str;
                    item.Expanded += new RoutedEventHandler(item_Expanded);   // 노드 확장시 추가
                    item.MouseDoubleClick += Item_MouseDoubleClick;

                    ItemList.Items.Add(item);
                    GetSubDirectories(item);
                }

                catch (Exception except)
                {
                    // MessageBox.Show(except.Message);   // 접근 거부 폴더로 인해 주석처리
                }
            }

        }

        private void Item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var i = sender as TreeViewItem;

            var tag = i.Tag.ToString();

            this.ViewModel.UpdateImageList(tag);
        }

        void GetSubDirectories(TreeViewItem itemParent)
        {
            if (itemParent == null) return;
            if (itemParent.Items.Count != 0) return;

            try
            {
                string strPath = itemParent.Tag as string;
                DirectoryInfo dInfoParent = new DirectoryInfo(strPath);
                foreach (DirectoryInfo dInfo in dInfoParent.GetDirectories())
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = dInfo.Name;
                    item.Tag = dInfo.FullName;
                    item.Expanded += new RoutedEventHandler(item_Expanded);
                    item.MouseDoubleClick += Item_MouseDoubleClick;
                    itemParent.Items.Add(item);
                }
            }

            catch (Exception except)
            {
                // MessageBox.Show(except.Message);   // 접근 거부 폴더로 인해 주석처리
            }
        }

        // 트리확장시 내용 추가
        void item_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem itemParent = sender as TreeViewItem;
            if (itemParent == null) return;
            if (itemParent.Items.Count == 0) return;
            foreach (TreeViewItem item in itemParent.Items)
            {
                GetSubDirectories(item);
            }
        }
    }
}
