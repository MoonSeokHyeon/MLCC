using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.VisionModel;

namespace VASFx.MLCC.VisionLibrary.Cognex
{
    public class CogLib : ICognexLib
    {
        public CogLib(CameraConfig config)
        {

        }

        public CameraConfig Config { get; set; }

        public bool IsConnected { get; set; }

        public CogResult ActionReferaceFind()
        {
            var result = new CogResult();

            return result;
        }

        public bool ConnectCamera()
        {
            IsConnected = true;
            return true;
        }

        public void CreatePatternGraphic()
        {
        }

        public bool DisconnectCamera()
        {
            return true;
        }

        public CogResult FindCorner()
        {
            var result = new CogResult();

            return result;
        }

        public CogResult FindPattern()
        {
            var result = new CogResult();

            return result;
        }

        public void GetPatternData()
        {
        }

        public void Init()
        {
        }

        public bool LoadModel(string modelName)
        {
            return true;
        }

        public bool SaveModel(string modelName)
        {
            return true;
        }

        public bool TrainPattern()
        {
            return true;
        }
    }
}
