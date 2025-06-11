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

namespace VASFx.UI.Interactivity
{
    /// <summary>
    /// ComfirmationView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ComfirmationView : UserControl
    {
        public ComfirmationViewModel ViewModel { get => this.DataContext as ComfirmationViewModel; }

        public ComfirmationView()
        {
            InitializeComponent();
        }
    }
}
