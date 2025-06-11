using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.PMAlign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Model;
using VASFx.MLCC.Common.VisionModel;

namespace VASFx.MLCC.VisionLibrary.Cognex
{
    public interface ICam
    {
        event Action<CogResult> OnDisplayUpdate;
        bool IsConnected { get; }
        CameraConfig Config { get; }
        List<VisionTool> VisionTools { get; }
        ICogAcqFifo Fifo { get; }
        bool ConnectCamera();
        void DisconnectCamera();
        void Init(string path);
        (XY point1, XY point2, XY point3, double score) RunLineTools();
        (XY point1, XY point2, XY point3, double score) RunFixtureLineTools();
        XY RunLineTool(int score, VisionTool visionTool);
        XY RunLineFixtureTool(int score, VisionTool visionTool);
        XY RunFindPattern(int score);
        (CogGraphicInteractiveCollection graphic, ICogImage image) CreateFindLineGraphic(CogFindLineTool visionTool);
        CogGraphicInteractiveCollection CreatePatternGraphic(CogPMAlignTool cogPMAlignTool);
        void GrabImageCamera();
        ICogImage GrabImage();
        CogGraphicInteractiveCollection DrawCenterGrid();
        void SetPMAlignTool(CogPMAlignTool tool);
        void SetFindLineTool(CogFindLineTool tool);
        CogPMAlignTool GetCogPMAlignTool(VisionTool visionTool);
        CogFindLineTool GetCogFindLineTool(VisionTool visionTool);
        bool TrainLineTool(VisionTool visionTool, CogFindLineTool registTool);
        bool TrainPatternTool(VisionTool visionTool, CogPMAlignTool registTool);
        void LoadTools(string path);
        void SaveTools(string path);
        List<CogFindLineTool> FindLineTools { get; }
        List<CogPMAlignTool> PMAlignTools { get; }
        ICogImage CreateImage();
        bool IsLicense();
    }
}
