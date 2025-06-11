using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Model;
using VASFx.UI.CogDisplayViews.Views;
using VASFx.VisionLibrary.Cognex;

namespace VASFx.MLCC.UI.ImageLogViews
{
    public class MLCCMainImageLogViewModel : BindableBase
    {
        #region Properties

        CogDisplayView cogDisplay = null;
        public CogDisplayView CogDisplay { get => this.cogDisplay; set => SetProperty(ref this.cogDisplay, value); }

        private ObservableCollection<ImageLogData> imageList = new ObservableCollection<ImageLogData>();
        public ObservableCollection<ImageLogData> ImageList
        {
            get { return imageList; }
            set { SetProperty(ref this.imageList, value); }
        }

        private ImageLogData imageSelected;
        public ImageLogData ImageSelected
        {
            get { return imageSelected; }
            set
            {
                if (SetProperty(ref this.imageSelected, value))
                {
                    LoadImage(imageSelected);
                }
            }
        }

        IContainerProvider provider = null;

        #endregion

        #region ICommands


        #endregion

        #region Construct

        public MLCCMainImageLogViewModel(IContainerProvider prov)
        {
            this.provider = prov;

            InitProperties();
        }

        public void Init()
        {
            ImageList = new ObservableCollection<ImageLogData>();

            this.CogDisplay.ClearImage();
        }

        private void InitProperties()
        {
            this.CogDisplay = provider.Resolve<CogDisplayView>();
        }

        #endregion

        #region public Method

        public void UpdateImageList(string tag)
        {
            var fileList = new List<ImageLogData>();

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(tag);

            foreach (System.IO.FileInfo File in di.GetFiles())
            {
                if (File.Extension.ToLower().CompareTo(".bmp") == 0)
                {
                    fileList.Add(new ImageLogData
                    {
                        FileName = File.Name,
                        ModifiedDate = File.LastWriteTime,
                        Category = File.Extension,
                        Size = File.Length,
                        Path = File.FullName
                    });
                }
            }

            fileList.ForEach(file =>
            {
                file.Size = Math.Round(file.Size / 1024 / 1024, 3);
            });

            if (fileList.Count > 0) ImageList = new ObservableCollection<ImageLogData>(fileList);
        }

        #endregion

        #region Private Method

        private void LoadImage(ImageLogData imageSelected)
        {
            if (imageSelected == null) return;
            var visionPro = new CogVisionPro();

            var image = visionPro.LoadImage(imageSelected.Path);

            this.cogDisplay.SetImage(image);
        }

        #endregion
    }
}
