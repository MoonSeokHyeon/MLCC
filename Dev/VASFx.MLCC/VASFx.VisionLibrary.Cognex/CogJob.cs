using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.PMAlign;
using GSG.NET.Vision.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Model;
using VASFx.Common.Shared;

namespace VASFx.VisionLibrary.Cognex
{
    public class CogJob
    {
        public Dictionary<ePatternNum, CogPMAlignTool> PMAlignDic { get; private set; } = new Dictionary<ePatternNum, CogPMAlignTool>();
        public Dictionary<eKindFindLine, CogFindLineTool> FindLineDic { get; private set; } = new Dictionary<eKindFindLine, CogFindLineTool>();
        public Dictionary<eBlobNum, CogBlobTool> BlobDic { get; private set; } = new Dictionary<eBlobNum, CogBlobTool>();
        public CogFixtureTool fixtureTool { get; private set; } = new CogFixtureTool();
        public eExecuteZone eZoneID { get; set; }
        public eGrabPosition ePosID { get; set; }

        string modelID = null;
        public CogVisionPro visionProTools = null;

        public CogJob(JobData jobInfo)
        {
            visionProTools = new CogVisionPro();
        }

        public void JobInitialize(eExecuteZone ZoneID, eGrabPosition PosID, string ModelID)
        {
            ePosID = PosID;
            eZoneID = ZoneID;
            modelID = ModelID;

            LoadTools();
            SaveTools();
        }

        public bool IsCognexDongle()
        {
            if (CogVisionPro.GetLicenseList().Count > 0)
                return true;

            return false;
        }

        #region Model

        public void SaveTools()
        {
            if (!Directory.Exists($@"D:\Data\{modelID}\CogJob"))
                Directory.CreateDirectory($@"D:\Data\{modelID}\CogJob");

            var path = $@"D:\Data\{modelID}\CogJob\{eZoneID}{ePosID}";

            foreach (KeyValuePair<ePatternNum, CogPMAlignTool> item in PMAlignDic)
            {
                SavePMAlignTool(item.Key, item.Value, path);
            }

            foreach (KeyValuePair<eKindFindLine, CogFindLineTool> item in FindLineDic)
            {
                SaveLineTool(item.Key, item.Value, path);
            }
            foreach (KeyValuePair<eBlobNum, CogBlobTool> item in BlobDic)
            {
                SaveBlobTool(item.Key, item.Value, path);
            }

            SaveFixtureTool(fixtureTool, path);
        }

        public void SavePMAlignTool(ePatternNum patternNum, CogPMAlignTool pmAlignTool, string path)
        {
            path += $@"PMAlignTool{patternNum}.cog";

            visionProTools.SavePattern(path, pmAlignTool);
        }

        public void SaveLineTool(eKindFindLine kindFindLine, CogFindLineTool findLineTool, string path)
        {
            path += $@"FindLineTool{kindFindLine}.cog";

            visionProTools.SaveFindLineTool(path, findLineTool);
        }
        public void SaveBlobTool(eBlobNum blobNum, CogBlobTool blobTool, string path)
        {
            path += $@"BlobTool{blobNum}.cog";

            visionProTools.SaveBlobTool(path, blobTool);
        }
        public void SaveFixtureTool(CogFixtureTool fixtureTool, string path)
        {
            path += "FixtureTool.cog";

            visionProTools.SaveFixtureTool(path, fixtureTool);
        }

        public void LoadTools()
        {
            if (!Directory.Exists($@"D:\Data\{modelID}\CogJob"))
            {
                Directory.CreateDirectory($@"D:\Data\{modelID}\CogJob");

                return;
            }

            var path = $@"D:\Data\{modelID}\CogJob\{eZoneID}{ePosID}";

            for (ePatternNum i = ePatternNum.Pattern1; i < ePatternNum.CalibrationPattern + 1; i++)
            {
                PMAlignDic.Add(i, LoadPMAlignTool(i, path));
            }

            for (eKindFindLine i = eKindFindLine.Horizontal; i < eKindFindLine.Vertical + 1; i++)
            {
                FindLineDic.Add(i, LoadLineTool(i, path));
            }
            for (eBlobNum i = eBlobNum.Blob1; i < eBlobNum.Blob6 + 1; i++)
            {
                BlobDic.Add(i, LoadBlobTool(i, path));
            }

            fixtureTool = LoadFixtureTool(path);
        }

        public CogPMAlignTool LoadPMAlignTool(ePatternNum patternNum, string path)
        {
            path += $@"PMAlignTool{patternNum}.cog";

            var retTool = visionProTools.LoadPattern(path);

            if (retTool == null)
                retTool = new CogPMAlignTool();

            return retTool;
        }

        public CogFindLineTool LoadLineTool(eKindFindLine kindFindLine, string path)
        {
            path += $@"FindLineTool{kindFindLine}.cog";

            var retTool = visionProTools.LoadFindLine(path);

            if (retTool == null)
                retTool = new CogFindLineTool();

            return retTool;
        }
        public CogBlobTool LoadBlobTool(eBlobNum blobNum, string path)
        {
            path += $@"BlobTool{blobNum}.cog";

            var retTool = visionProTools.LoadBlobTool(path);

            if (retTool == null)
                retTool = visionProTools.CreateBlobTool("");

            return retTool;
        }
        public CogFixtureTool LoadFixtureTool(string path)
        {
            path += $@"FixtureTool.cog";

            var retTool = visionProTools.LoadFixtureTool(path);

            if (retTool == null)
                retTool = new CogFixtureTool();

            return retTool;
        }
        public void SaveImage(ICogImage grabImage, string path)
        {
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString();
            var day = DateTime.Now.Day.ToString();
            var hour = DateTime.Now.Hour.ToString();
            var minute = DateTime.Now.Minute.ToString();
            var second = DateTime.Now.Second.ToString();

            path += @"\\" + year + month + day;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            visionProTools.SaveImage(path + @"\\" + eExecuteZone.MLCC_INSPECTION.ToString() + "_" + hour + minute + second + "_" + ".bmp", grabImage);
        }

        #endregion

        #region PMAlignTool

        /// <summary>
        /// Single Display View, Core
        /// </summary>
        public void RunPMAlignTool(ICogImage inputImage, out PMAlignResult result)
        {
            result = new PMAlignResult();
            var pmAlignResult = new List<CogPMAlignResult>();

            foreach (KeyValuePair<ePatternNum, CogPMAlignTool> Item in PMAlignDic)
            {
                var cogPMAlignResult = visionProTools.RunPMAlignTool(Item.Value, inputImage);

                if (cogPMAlignResult == null)
                    continue;
                else
                {
                    pmAlignResult.Add(cogPMAlignResult);
                }
            }

            if (pmAlignResult.Count > 0)
            {
                var items = pmAlignResult.Cast<CogPMAlignResult>();
                var topResult = items.Where(x => x.Score == items.Max(i => i.Score)).First();

                result.Image = inputImage;
                result.resultXY = new XY(topResult.GetPose().TranslationX, topResult.GetPose().TranslationY);
                result.score = topResult.Score * 100;

                result.ResultGraphic.Add(topResult.CreateResultGraphics(CogPMAlignResultGraphicConstants.Origin));
                result.ResultGraphic.Add(topResult.CreateResultGraphics(CogPMAlignResultGraphicConstants.CoordinateAxes));
                result.ResultGraphic.Add(topResult.CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchFeatures));
                result.ResultGraphic.Add(topResult.CreateResultGraphics(CogPMAlignResultGraphicConstants.BoundingBox));
                result.ResultGraphic.Add(topResult.CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchShapeModels));

                var sb = new StringBuilder();

                sb.Append($" X[{Math.Round(topResult.GetPose().TranslationX, 2)}]   /   Y[{Math.Round(topResult.GetPose().TranslationY, 2)}]   / T[{Math.Round(topResult.GetPose().Rotation, 2)}]  / Score[{Math.Round(topResult.Score, 4) * 100}]");
                result.ResultGraphic.Add(visionProTools.CreateLabel(sb.ToString()));
            }
        }

        /// <summary>
        /// ModelEditorView
        /// </summary>
        public void RunPMAlignTool(ePatternNum patternNum, ICogImage inputImage, out PMAlignResult result)
        {
            result = new PMAlignResult();

            var tool = this.PMAlignDic[patternNum];

            var pmAlignResult = visionProTools.RunPMAlignTool(tool, inputImage);

            if (result != null)
            {
                result.Image = inputImage;
                result.resultXY = new XY(pmAlignResult.GetPose().TranslationX, pmAlignResult.GetPose().TranslationY);
                result.score = pmAlignResult.Score * 100;

                result.ResultGraphic.Add(pmAlignResult.CreateResultGraphics(CogPMAlignResultGraphicConstants.Origin));
                result.ResultGraphic.Add(pmAlignResult.CreateResultGraphics(CogPMAlignResultGraphicConstants.CoordinateAxes));
                result.ResultGraphic.Add(pmAlignResult.CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchFeatures));
                result.ResultGraphic.Add(pmAlignResult.CreateResultGraphics(CogPMAlignResultGraphicConstants.BoundingBox));
                result.ResultGraphic.Add(pmAlignResult.CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchShapeModels));

                var sb = new StringBuilder();

                sb.Append($"{patternNum} - X[{Math.Round(pmAlignResult.GetPose().TranslationX, 2)}]   /   Y[{Math.Round(pmAlignResult.GetPose().TranslationY, 2)}]   / T[{Math.Round(pmAlignResult.GetPose().Rotation, 2)}]  / Score[{Math.Round(pmAlignResult.Score, 4) * 100}]");
                result.ResultGraphic.Add(visionProTools.CreateLabel(sb.ToString()));
            }
        }

        /// <summary>
        /// PMAlignEditor Result
        /// </summary>
        public void RunPMAlignTool(ePatternNum patternNum, ICogImage inputImage, out List<PMAlignResult> result)
        {
            result = new List<PMAlignResult>();

            var tool = this.PMAlignDic[patternNum];

            var PMResult = visionProTools.RunPMAlignTools(tool, inputImage);

            if (PMResult != null)
            {
                foreach (CogPMAlignResult item in PMResult)
                {
                    var retResult = new PMAlignResult();

                    retResult.Image = inputImage;
                    retResult.resultXY = new XY(item.GetPose().TranslationX, item.GetPose().TranslationY);
                    retResult.score = item.Score * 100;

                    retResult.ResultGraphic.Add(item.CreateResultGraphics(CogPMAlignResultGraphicConstants.Origin));
                    retResult.ResultGraphic.Add(item.CreateResultGraphics(CogPMAlignResultGraphicConstants.CoordinateAxes));
                    retResult.ResultGraphic.Add(item.CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchFeatures));
                    retResult.ResultGraphic.Add(item.CreateResultGraphics(CogPMAlignResultGraphicConstants.BoundingBox));
                    retResult.ResultGraphic.Add(item.CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchShapeModels));

                    var sb = new StringBuilder();

                    sb.Append($"{patternNum} - X[{Math.Round(item.GetPose().TranslationX, 2)}]   /   Y[{Math.Round(item.GetPose().TranslationY, 2)}]   / T[{Math.Round(item.GetPose().Rotation, 2)}]  / Score[{Math.Round(item.Score, 4) * 100}]");
                    retResult.ResultGraphic.Add(visionProTools.CreateLabel(sb.ToString()));

                    result.Add(retResult);
                }
            }
        }

        #endregion

        #region FindLineTool

        //SingleView
        public void RunFindLineTools(out PMAlignResult resultXYT)
        {
            resultXYT = new PMAlignResult();
        }

        public void RunFindLineTool(eKindFindLine kindFindLine, out CogLine resultLine)
        {
            resultLine = new CogLine();
        }

        #endregion

        #region FixureTool

        public void RunFixtureCorner(out PMAlignResult resultXYT)
        {
            resultXYT = new PMAlignResult();
        }


        public void RunFixtureCorner(out XYT resultXYT, out CogPMAlignResult cogPMAlignResult, out CogLine resultVerLine, out CogLine resultHorLine)
        {
            resultXYT = new XYT();
            cogPMAlignResult = new CogPMAlignResult();
            resultVerLine = new CogLine();
            resultHorLine = new CogLine();
        }

        #endregion

        #region Blob

        public void RunBlobTool(eBlobNum blobNum, ICogImage inputImage, out List<BlobResult> result)
        {
            result = new List<BlobResult>();

            var tool = this.BlobDic[blobNum];

            var runBlobResults = visionProTools.RunBlobTool(tool, inputImage);

            if (runBlobResults == null) return;

            var blobResult = runBlobResults.GetBlobs();
            var blobResultImage = runBlobResults.CreateBlobImage();
            var blobSegmentedImage = runBlobResults.CreateSegmentedImage();

            if (blobResult != null)
            {
                foreach (CogBlobResult item in blobResult)
                {
                    if (item.Label == CogBlobLabelConstants.Hole)
                        continue;

                    var refResult = new BlobResult();

                    refResult.Image = inputImage;
                    refResult.BlobImage = blobResultImage;
                    refResult.SegmentImage = blobSegmentedImage;

                    refResult.Id = item.ID;
                    refResult.Area = item.Area;
                    refResult.CenterOfMassX = item.CenterOfMassX;
                    refResult.CenterOfMassY = item.CenterOfMassY;

                    refResult.BlobGraphic = item.GetBoundary();
                    refResult.BlobGraphic.Color = CogColorConstants.Green;

                    refResult.ImageBoundMaxX = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxX);
                    refResult.ImageBoundMaxY = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxY);
                    refResult.ImageBoundMinX = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinX);
                    refResult.ImageBoundMinY = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinY);
                    refResult.BoundHeight = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeHeight);
                    refResult.BoundWidth = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth);
                    result.Add(refResult);
                }
            }

        }

        public void RunBlobTool(eBlobNum blobNum, ICogImage inputImage, out List<BlobResult> result, int minSize = 10)
        {
            result = new List<BlobResult>();

            var tool = this.BlobDic[blobNum];
            tool.RunParams.ConnectivityMinPixels = minSize;

            var runBlobResults = visionProTools.RunBlobTool(tool, inputImage);

            if (runBlobResults == null) return;

            var blobResult = runBlobResults.GetBlobs();
            var blobResultImage = runBlobResults.CreateBlobImage();
            var blobSegmentedImage = runBlobResults.CreateSegmentedImage();

            if (blobResult != null)
            {
                foreach (CogBlobResult item in blobResult)
                {
                    if (item.Label == CogBlobLabelConstants.Hole)
                        continue;

                    var refResult = new BlobResult();

                    refResult.Image = inputImage;
                    refResult.BlobImage = blobResultImage;
                    refResult.SegmentImage = blobSegmentedImage;

                    refResult.Id = item.ID;
                    refResult.Area = item.Area;
                    refResult.CenterOfMassX = item.CenterOfMassX;
                    refResult.CenterOfMassY = item.CenterOfMassY;

                    refResult.BlobGraphic = item.GetBoundary();
                    refResult.BlobGraphic.Color = CogColorConstants.Green;

                    refResult.ImageBoundMaxX = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxX);
                    refResult.ImageBoundMaxY = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxY);
                    refResult.ImageBoundMinX = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinX);
                    refResult.ImageBoundMinY = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinY);
                    refResult.BoundHeight = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeHeight);
                    refResult.BoundWidth = item.GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth);
                    result.Add(refResult);
                }
            }

        }
        #endregion
    }

    public class PMAlignResult
    {
        public ICogImage Image { get; set; }
        public XY resultXY { get; set; }
        public double score { get; set; }
        public CogGraphicInteractiveCollection ResultGraphic { get; set; }

        public PMAlignResult()
        {
            resultXY = new XY();

            score = 0.0;

            ResultGraphic = new CogGraphicInteractiveCollection();
        }
    }

    public class FindLineResult
    {
        public ICogImage Image { get; set; }
        public List<XY> resultXYs { get; set; }
        public CogGraphicInteractiveCollection ResultGraphic { get; set; }

        public FindLineResult()
        {
            resultXYs = new List<XY>();

            ResultGraphic = new CogGraphicInteractiveCollection();
        }
    }

    public class BlobResult
    {
        public ICogImage Image { get; set; }
        public ICogImage BlobImage { get; set; }
        public ICogImage SegmentImage { get; set; }

        public int Id { get; set; }
        public double Area { get; set; }
        public double CenterOfMassX { get; set; }
        public double CenterOfMassY { get; set; }
        public double ImageBoundMaxX { get; set; }
        public double ImageBoundMaxY { get; set; }
        public double ImageBoundMinX { get; set; }
        public double ImageBoundMinY { get; set; }
        public double BoundWidth { get; set; }
        public double BoundHeight { get; set; }

        public CogPolygon BlobGraphic { get; set; }
        public CogGraphicInteractiveCollection ResultGraphic { get; set; }

        public BlobResult()
        {
            Id = 0;

            Area = 0.0;

            CenterOfMassX = 0.0;

            CenterOfMassY = 0.0;

            ImageBoundMaxX = 0.0;

            ImageBoundMaxY = 0.0;

            ImageBoundMinX = 0.0;

            ImageBoundMinY = 0.0;

            BoundWidth = 0.0;

            BoundHeight = 0.0;

            BlobGraphic = new CogPolygon();

            ResultGraphic = new CogGraphicInteractiveCollection();

        }
    }
}
