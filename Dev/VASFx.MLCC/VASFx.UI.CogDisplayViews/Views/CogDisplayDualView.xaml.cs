﻿using System;
using System.Collections.Generic;
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

namespace VASFx.UI.CogDisplayViews.Views
{
    /// <summary>
    /// CogDisplayDualView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CogDisplayDualView : UserControl
    {
        public CogDisplayDualViewModel ViewModel { get => this.DataContext as CogDisplayDualViewModel; }

        public CogDisplayDualView()
        {
            InitializeComponent();

            this.Loaded += UserControl_Loaded;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Init();
        }
    }
}
