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

namespace VASFx.UI.CalibrationControlViews.UI
{
    /// <summary>
    /// LoaderClibrationViewResult.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoaderCalibrationViewResult : UserControl
    {
        public LoaderCalibrationViewResultModel ViewModel => this.DataContext as LoaderCalibrationViewResultModel;
        public LoaderCalibrationViewResult()
        {
            InitializeComponent();
            this.Loaded += LoaderCalibrationViewResult_Loaded;
        }

        bool isInited = false;
        private void LoaderCalibrationViewResult_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isInited)
            {
                isInited = true;
                this.ViewModel.Init();
            }
        }
    }
}
