using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VASFx.Common.Interface;

namespace VASFx.UI.SplashScreenWindows.UI
{
    /// <summary>
    /// VisionSplashWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VisionSplashWindow : Window, ISplashScreen, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged( string name )
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if ( handler != null )
            {
                handler( this, new PropertyChangedEventArgs( name ) );
            }
        }

        #region Properties
        string totalProgressText;
        private int _progressValue;

        public int ProgressValue
        {
            get { return _progressValue; }
            set
            {
                _progressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }
        public string TotalProgressText
        {
            get { return totalProgressText; }
            set
            {
                if (totalProgressText == value)
                    return;
                totalProgressText = value;
                OnPropertyChanged(nameof(TotalProgressText));
            }
        }
        #endregion

        public void SetRange(int count)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                ProgressValue = 0;
                //ProgressBar.Maximum = count;
            }));
        }

        public void StepIt()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                //if (this.ProgressBar.Maximum <= ProgressValue)
                //    return;

                this.ProgressValue += 1;
            }));
        }

        public VisionSplashWindow()
        {
            InitializeComponent();
        }

       public void AddMessage(string message)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                this.UpdateMessageLabel.Text = message;
            });
        }

        public void ReportProgress(int persent)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                ProgressValue = persent;
            });
        }

        public void ReportTotalProgress(int persent)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                this.TotalProgressText = $"Total {persent}%";
            });
        }

        public void LoadComplete()
        {
            Thread.Sleep(500);
            Dispatcher.InvokeShutdown();
        }

    }
}
