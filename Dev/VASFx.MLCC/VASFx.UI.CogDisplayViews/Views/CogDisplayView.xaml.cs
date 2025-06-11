using Cognex.VisionPro;
using GSG.NET.Utils;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using VASFx.Common.Shared;

namespace VASFx.UI.CogDisplayViews.Views
{
    /// <summary>
    /// CogDisplayView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CogDisplayView : UserControl
    {
        #region Properties
        //public CogDisplayViewModel ViewModel { get => this.DataContext as CogDisplayViewModel; }

        private WindowsFormsHost camViewHost = new WindowsFormsHost();
        public WindowsFormsHost CamViewHost
        {
            get { return camViewHost; }
            set { this.camViewHost = value; }
        }

        CogRecordDisplay cogRecord = null;

        public bool FixAirspace
        {
            get { return this.asfCogView.FixAirspace; }
            set { this.asfCogView.FixAirspace = value; }
        }

        //private System.Windows.Media.Brush _cameraStateDisplayBorderBrush;
        //public System.Windows.Media.Brush CameraStateDisplayBorderBrush
        //{
        //    get { return _cameraStateDisplayBorderBrush; }
        //    set { _cameraStateDisplayBorderBrush = value; }
        //}

        public event Action CogDisplayLoaded;

        #endregion

        #region Struct
        public CogDisplayView()
        {
            InitializeComponent();
            this.DataContext = this;

            this.Loaded += CogDisplayView_Loaded;
        }

        bool _isInted = false;
        private void CogDisplayView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isInted)
            {
                _isInted = true;

                CamViewHost.Child = this.cogRecord = new CogRecordDisplay();
                this.cogRecord.HorizontalScrollBar = false;
                this.cogRecord.VerticalScrollBar = false;
                this.cogRecord.AutoFit = true;
                this.cogRecord.BackColor = Color.DarkGray;
                this.cogRecord.MouseDown += CogRecord_MouseDown;
                this.cogRecord.MouseUp += CogRecord_MouseUp;
                this.cogRecord.MouseMove += CogRecord_MouseMove;

                CogDisplayLoaded?.Invoke();
            }
        }

        #endregion

        #region Public Method

        public bool SetImage(ICogImage image)
        {
            Assert.NotNull(image, $"Image is Null");

            this.cogRecord.Image = image;
            return true;
        }
        public void ClearImage()
        {
            if (cogRecord == null) return;
            cogRecord.Image = null;
        }
        public ICogImage GetCogImage()
        {
            var image = this.cogRecord.Image;

            Assert.NotNull(image, $"Image is Null");

            return image;
        }
        public void StartLiveDisplay(ICogAcqFifo fifo)
        {
            if (cogRecord == null || fifo == null) return;

            cogRecord.StartLiveDisplay(fifo, false);
        }
        public bool StopLiveDisplay()
        {
            if (cogRecord == null) return true;

            if (cogRecord.LiveDisplayRunning)
                cogRecord.StopLiveDisplay();

            return true;
        }
        public bool IsLiveDisplay()
        {
            if (cogRecord == null) return false;

            return cogRecord.LiveDisplayRunning;
        }
        public void SetGraphic(CogGraphicInteractiveCollection cogGraphicInteractiveCollection, string groupName, bool checkForDuplicates)
        {
            if (this.cogRecord == null) return;

            try
            {
            this.cogRecord.InteractiveGraphics.AddList(cogGraphicInteractiveCollection, groupName, checkForDuplicates);
            }
            catch(Exception e)
            {

            }
        }
        public void RemoveGraphic(string gropName)
        {
            if (cogRecord == null || cogRecord.InteractiveGraphics == null) return;

            if (cogRecord.InteractiveGraphics.FindItem(gropName, Cognex.VisionPro.Display.CogDisplayZOrderConstants.Front) != -1)
                cogRecord.InteractiveGraphics.Remove(gropName);
        }
        public void SetMouseMode(eDisplayMouseMode mouseMode)
        {
            if (this.cogRecord == null) return;

            this.cogRecord.MouseMode = (Cognex.VisionPro.Display.CogDisplayMouseModeConstants)mouseMode;
        }
        public void SetFitImage()
        {
            if (this.cogRecord == null) return;

            this.cogRecord.AutoFit = true;
            this.cogRecord.AutoFitWithGraphics = true;
            this.cogRecord.Fit();
        }   
        public void SetCameraConnection(bool isConnected)
        {
            if (isConnected)
                this.bodMain.BorderBrush = System.Windows.Media.Brushes.DeepSkyBlue;
            else
                this.bodMain.BorderBrush = System.Windows.Media.Brushes.Red;
        }

        public void ClearGraphics()
        {
            if (cogRecord == null || cogRecord.InteractiveGraphics == null ) return;
            cogRecord.InteractiveGraphics.Clear();
        }

        #endregion

        #region Cog Record Event
        private void CogRecord_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void CogRecord_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void CogRecord_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }
        #endregion
    }
}
