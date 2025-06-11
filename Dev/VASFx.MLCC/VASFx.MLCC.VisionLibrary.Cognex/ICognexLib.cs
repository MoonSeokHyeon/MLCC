using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.VisionModel;

namespace VASFx.MLCC.VisionLibrary.Cognex
{
    public interface ICognexLib
    {
        CameraConfig Config { get; set; }
        bool IsConnected { get; set; }

        void Init();
        bool ConnectCamera();
        bool DisconnectCamera();
        //bool SaveImage(string path, ICogImage image);
        bool SaveModel(string modelName);
        bool LoadModel(string modelName);
        bool TrainPattern();
        CogResult FindPattern();
        CogResult FindCorner();
        CogResult ActionReferaceFind();
        void GetPatternData();
        void CreatePatternGraphic();
    }
}
