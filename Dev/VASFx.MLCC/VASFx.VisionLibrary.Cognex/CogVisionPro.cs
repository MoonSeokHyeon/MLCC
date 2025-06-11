using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.PMAlign;
using GSG.NET.Vision.Model;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VASFx.VisionLibrary.Cognex
{
    public class CogVisionPro
    {
        #region License
        public static CogStringCollection GetLicenseList()
        {
            var list = CogLicense.GetLicensedFeatures(true, false);

            return list;
        }
        #endregion

        #region Camera
        public CogFrameGrabbers CreateFrameGrabbers()
        {
            CogFrameGrabbers cogFrameGrabbers;

            try
            {
                cogFrameGrabbers = new CogFrameGrabbers();
            }
            catch
            {
                return null;
            }

            return cogFrameGrabbers;
        }

        public ICogFrameGrabber CreateFrameGrabber(string serialNumber)
        {
            var cogFrameGrabbers = new CogFrameGrabbers();
            try
            {
                var cogFramegrabber = cogFrameGrabbers.Cast<ICogFrameGrabber>().FirstOrDefault(x => x.SerialNumber.Equals(serialNumber));

                return cogFramegrabber;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ICogAcqFifo ConnectCamera(ICogFrameGrabber cogFramGrabber)
        {
            if (cogFramGrabber != null)
            {
                var CameraFifo = cogFramGrabber.CreateAcqFifo("Generic GigEVision (Mono)", CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);
                return CameraFifo;
            }

            return null;
        }

        public bool DisconnectCamera(ICogFrameGrabber cogFramGrabber)
        {
            if (cogFramGrabber == null) return false;

            cogFramGrabber.Disconnect(true);

            return true;
        }

        public ICogImage GrabImage(ICogAcqFifo fifo)
        {
            int trigger = 0;
            ICogImage retImage;

            //TODO : Grab Test
            if (fifo != null)
            {
                try
                {
                    retImage = fifo.Acquire(out trigger);

                    return retImage;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Pattern

        public bool SetPatternParams(CogPMAlignRunParams param, ref CogPMAlignTool tool)
        {
            if (param == null || tool == null) return false;

            try
            {
                tool.RunParams = param;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool TrainPattern(CogPMAlignPattern registPattern, CogPMAlignTool tool)
        {
            try
            {
                if (registPattern == null) return false;

                tool.Pattern = registPattern;

                tool.Pattern.Train();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public CogGraphicInteractiveCollection CreatePatternGraphic(CogPMAlignTool tool)
        {
            if (tool == null) return null;

            var graphic = new CogGraphicInteractiveCollection();

            if (tool.Pattern.Origin == null)
            {
                tool.Pattern.Origin.TranslationX = 0;
                tool.Pattern.Origin.TranslationY = 0;
                tool.Pattern.Origin.Rotation = 0;
                tool.Pattern.Origin.Skew = 0;
            }

            var origin = new CogCoordinateAxes();
            origin.Transform = tool.Pattern.Origin;
            origin.DisplayedXAxisLength = 100.0;
            origin.DisplayedAspect = 1.0;
            origin.GraphicDOFEnable = CogCoordinateAxesDOFConstants.All;
            origin.Interactive = true;

            var trainRegion = (ICogGraphicInteractive)tool.Pattern.TrainRegion;

            trainRegion.TipText = "Pattern";
            origin.TipText = "Pattern Origin";

            graphic.Add(trainRegion);
            graphic.Add(origin);

            return graphic;
        }

        public ICogGraphicInteractive CreatePatternROIGraphic(CogPMAlignTool tool)
        {
            if (tool == null) return null;

            if (tool.InputImage == null) return null;

            ICogGraphicInteractive graphic;

            if (tool.SearchRegion == null)
            {
                var mPMSearchRegion = new CogRectangle();
                mPMSearchRegion.SetXYWidthHeight(0, 0, tool.InputImage.Width, tool.InputImage.Height);
                mPMSearchRegion.GraphicDOFEnable = CogRectangleDOFConstants.Size | CogRectangleDOFConstants.Position;
                mPMSearchRegion.Interactive = true;

                tool.SearchRegion = mPMSearchRegion;
            }

            graphic = (ICogGraphicInteractive)((ICloneable)tool.SearchRegion).Clone();
            graphic.Interactive = true;

            return graphic;
        }

        public CogPMAlignTool CreatePMAlignTool(string name)
        {
            var tool = new CogPMAlignTool();
            tool.Name = name;
            tool.RunParams.RunAlgorithm = CogPMAlignRunAlgorithmConstants.PatMax;

            tool.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.Nominal;

            tool.RunParams.ZoneScale.Configuration = CogPMAlignZoneConstants.Nominal;

            return tool;
        }

        public CogPMAlignTool CreatePMAlignTool(string name, CogPMAlignTool tool)
        {
            var retTool = new CogPMAlignTool(tool);

            retTool.Name = name;

            return retTool;
        }

        public CogPMAlignResults RunPMAlignTools(CogPMAlignTool tool, ICogImage inputImage)
        {
            if (inputImage == null || tool == null || !tool.Pattern.Trained) return null;

            tool.InputImage = inputImage;
            tool.Run();

            if (tool.Results == null || tool.Results.Count == 0) return null;

            return tool.Results;
        }

        public CogPMAlignResult RunPMAlignTool(CogPMAlignTool tool, ICogImage inputImage)
        {
            CogPMAlignResult result = null;
            if (inputImage == null || tool == null) return result;

            tool.InputImage = inputImage;
            tool.Run();

            if (tool.Results == null || tool.Results.Count == 0) return result;

            var items = tool.Results.Cast<CogPMAlignResult>();
            var topResult = items.Where(x => x.Score == items.Max(i => i.Score)).First();

            return result = topResult;
        }
        #endregion

        #region Line

        public bool SetLineParams(CogFindLine param, ref CogFindLineTool tool)
        {
            if (param == null || tool == null) return false;

            try
            {
                tool.RunParams = param;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public CogGraphicInteractiveCollection CreateLineGraphic(CogFindLineTool tool)
        {
            var retGraphic = new CogGraphicInteractiveCollection();

            var cogFindLineTool = new CogFindLineTool(tool);

            var segment = new CogLineSegment();
            segment = cogFindLineTool.RunParams.ExpectedLineSegment;
            segment.GraphicDOFEnable = CogLineSegmentDOFConstants.All;
            segment.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
            segment.Interactive = true;
            segment.TipText = "Vertical Segment";

            retGraphic.Add(segment);

            return retGraphic;
        }

        public CogGraphicInteractiveCollection CreateLineGraphic(ICogImage fixtureImage, CogFindLineTool lineTool)
        {
            var retGraphic = new CogGraphicInteractiveCollection();

            var cogFindLineTool = new CogFindLineTool(lineTool);

            cogFindLineTool.InputImage = (CogImage8Grey)fixtureImage;

            CogLineSegment segment;
            segment = cogFindLineTool.RunParams.ExpectedLineSegment;
            segment.GraphicDOFEnable = CogLineSegmentDOFConstants.All;
            segment.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
            segment.Interactive = true;
            segment.TipText = "Vertical Segment";

            retGraphic.Add(segment);

            var myRec = cogFindLineTool.CreateCurrentRecord();
            var myArc = (CogLineSegment)myRec.SubRecords["InputImage"].SubRecords["ExpectedShapeSegment"].Content;
            var myRegions = (CogGraphicCollection)myRec.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;

            retGraphic.Add(myArc);
            foreach (ICogGraphic g in myRegions)
                retGraphic.Add((ICogGraphicInteractive)g);

            return retGraphic;
        }

        public CogFindLineTool CreateFindLineTool(string toolName, bool isVertical)
        {
            var tool = new CogFindLineTool();

            tool.Name = toolName;

            if (isVertical)
            {
                tool.RunParams.ExpectedLineSegment.StartX = 100;
                tool.RunParams.ExpectedLineSegment.EndX = 100;
                tool.RunParams.ExpectedLineSegment.StartY = 100;
                tool.RunParams.ExpectedLineSegment.EndY = 500;
            }
            else
            {
                tool.RunParams.ExpectedLineSegment.StartX = 100;
                tool.RunParams.ExpectedLineSegment.EndX = 500;
                tool.RunParams.ExpectedLineSegment.StartY = 100;
                tool.RunParams.ExpectedLineSegment.EndY = 100;
            }

            tool.RunParams.CaliperRunParams.EdgeMode = CogCaliperEdgeModeConstants.SingleEdge;
            tool.RunParams.CaliperRunParams.Edge0Polarity = CogCaliperPolarityConstants.LightToDark;
            tool.RunParams.CaliperRunParams.ContrastThreshold = 5;
            tool.RunParams.CaliperRunParams.FilterHalfSizeInPixels = 2;
            tool.RunParams.NumCalipers = 5;
            tool.RunParams.NumToIgnore = 0;
            tool.RunParams.CaliperSearchLength = 100;
            tool.RunParams.CaliperProjectionLength = 10;
            tool.RunParams.ExpectedLineSegment.GraphicDOFEnable = CogLineSegmentDOFConstants.All;

            return tool;
        }

        public CogFindLineTool CreateFindLineTool(string toolName, CogFindLineTool registTool)
        {
            var tool = new CogFindLineTool(registTool);
            tool.Name = toolName;

            return tool;
        }

        public CogFindLineResults RunFineLineTool(CogFindLineTool tool, ICogImage image)
        {
            if (tool == null || image == null) return null;

            tool.InputImage = image as CogImage8Grey;
            tool.Run();

            if (tool.Results == null) return null;

            return tool.Results;
        }

        public XYT GetCrossPoint(CogLine vertical, CogLine horizontal, CogFixtureTool fixtureTool, bool useTransform = true)
        {
            var intersect = new CogIntersectLineLineTool();
            intersect.InputImage = fixtureTool.OutputImage;
            intersect.LineA = vertical;
            intersect.LineB = horizontal;

            intersect.Run();

            if (intersect.NumPoints <= 0)
                return null;

            double outX, outY;

            XYT result = null;

            if (useTransform)
            {
                fixtureTool.RunParams.UnfixturedFromFixturedTransform.MapPoint(intersect.X, intersect.Y, out outX, out outY);
                result = new XYT(outX, outY, 0d);
            }
            else
                result = new XYT(intersect.X, intersect.Y, 0d);

            result.T = intersect.Angle / (Math.PI / 180.0);

            return result;

        }

        public XYT GetCrossPoint(CogLine ver, CogLine Hor, ICogImage image)
        {
            var intersect = new CogIntersectLineLineTool();
            intersect.InputImage = image;
            intersect.LineA = ver;
            intersect.LineB = Hor;

            intersect.Run();

            if (intersect.NumPoints <= 0)
                return null;

            var result = new XYT(intersect.X, intersect.Y, 0d);

            result.T = intersect.Angle / (Math.PI / 180.0);

            return result;

        }

        #endregion

        #region Fixture

        public CogFixtureTool CreateFixtureTool(string name)
        {
            var tool = new CogFixtureTool();
            tool.Name = name;
            tool.RunParams.FixturedSpaceName = name;
            return tool;
        }

        public ICogImage RunFixtureTool(CogFixtureTool tool, ICogImage inputImage, CogPMAlignResult result)
        {
            ICogImage retImage = null;
            if (tool == null || inputImage == null || result == null) return retImage;

            tool.InputImage = inputImage;
            tool.RunParams.UnfixturedFromFixturedTransform = result.GetPose();
            tool.Run();

            retImage = tool.OutputImage;

            return retImage;
        }

        #endregion

        #region Blob

        public bool SetBlobParams(CogBlob param, ref CogBlobTool tool)
        {
            if (param == null) return false;

            try
            {
                tool.RunParams = param;
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public CogBlobTool CreateBlobTool(string name)
        {
            CogBlobTool retBlob = new CogBlobTool();
            retBlob.Name = name;

            retBlob.RunParams.ConnectivityMode = CogBlobConnectivityModeConstants.GreyScale;
            retBlob.RunParams.ConnectivityCleanup = CogBlobConnectivityCleanupConstants.Fill;
            retBlob.RunParams.ConnectivityMinPixels = 10000;

            retBlob.RunParams.SegmentationParams.Mode = CogBlobSegmentationModeConstants.HardFixedThreshold;
            retBlob.RunParams.SegmentationParams.HardFixedThreshold = 125;

            retBlob.RunParams.RunTimeMeasures.Add(new CogBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxX));
            retBlob.RunParams.RunTimeMeasures.Add(new CogBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMaxY));
            retBlob.RunParams.RunTimeMeasures.Add(new CogBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinX));
            retBlob.RunParams.RunTimeMeasures.Add(new CogBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeMinY));        
            retBlob.RunParams.RunTimeMeasures.Add(new CogBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeHeight));        
            retBlob.RunParams.RunTimeMeasures.Add(new CogBlobMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth));        
            
            retBlob.RunParams.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.LightBlobs;
            
            retBlob.RunParams.SaveSegmentedImage = true;

            return retBlob;
        }

        public CogBlobTool CreateBlobTool(string name, CogBlobTool tool)
        {
            var retTool = new CogBlobTool(tool);

            retTool.Name = name;

            return retTool;
        }

        public CogBlobResults RunBlobTool(CogBlobTool tool, ICogImage inputImage)
        {
            if (inputImage == null || tool == null) return null;

            CogBlobResults result = null;

            tool.InputImage = inputImage;
            tool.Run();

            if (tool.Results == null) return result;

            result = tool.Results;

            return result;
        }

        #endregion

        #region Graphics

        public ICogImage CreateDummyImage(string message, int width, int height)
        {
            ICogImage image = new CogImage8Grey(width, height);
            return image;
        }

        public CogGraphicLabel CreateLabel(string message)
        {
            var lable = new CogGraphicLabel();
            lable.SetXYText(10, 0, message);
            lable.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
            lable.BackgroundColor = CogColorConstants.Black;
            lable.Color = CogColorConstants.Yellow;
            lable.SelectedSpaceName = "*";

            return lable;
        }

        public ICogImage RotateImage(CogIPOneImageFlipRotateOperationConstants rotate, ICogImage image)
        {
            var IpOneImageTool = new CogIPOneImageTool();
            IpOneImageTool.Operators.Add(new CogIPOneImageFlipRotate() { OperationInPixelSpace = rotate });
            IpOneImageTool.InputImage = image;
            IpOneImageTool.Run();

            IpOneImageTool.OutputImage.PixelFromRootTransform = new CogTransform2DLinear(); //! 회전 후 이미지 좌상단을 0, 0으로 다시 만든다.
            ICogImage ret = IpOneImageTool.OutputImage;
            IpOneImageTool.Dispose();

            return ret;
        }

        public void DrawCenterGrid(ICogImage image, CogGraphicInteractiveCollection cogGraphicCollection)
        {
            CogLine lineH = new CogLine();
            lineH.Color = CogColorConstants.Orange;
            CogLine lineV = new CogLine();
            lineV.Color = CogColorConstants.Orange;

            int width = image.Width / 2;
            int height = image.Height / 2;
            lineH.SetFromStartXYEndXY(width, 0, width, height);
            lineV.SetFromStartXYEndXY(0, height, width, height);

            cogGraphicCollection.Add(lineV);
            cogGraphicCollection.Add(lineH);
        }
        public void DrawSixGrid(ICogImage image, CogGraphicInteractiveCollection cogGraphicCollection)
        {
            CogLine lineH = new CogLine();
            lineH.Color = CogColorConstants.Orange;

            CogLine _lineH = new CogLine();
            _lineH.Color = CogColorConstants.Orange;

            CogLine lineV = new CogLine();
            lineV.Color = CogColorConstants.Orange;

            int width = image.Width / 3;
            int _width = (image.Width / 3) * 2;
            int height = image.Height / 2;

            lineH.SetFromStartXYEndXY(width, 0, width, height);
            _lineH.SetFromStartXYEndXY(_width, 0, _width, height);
            lineV.SetFromStartXYEndXY(0, height, width, height);

            cogGraphicCollection.Add(lineV);
            cogGraphicCollection.Add(_lineH);
            cogGraphicCollection.Add(lineH);
        }
        public CogGraphicInteractiveCollection GetGraphicCollectionDrawCenterGrid(int width, int height)
        {
            var rt = new CogGraphicInteractiveCollection();

            CogLine lineH = new CogLine();
            lineH.Color = CogColorConstants.Orange;
            CogLine lineV = new CogLine();
            lineV.Color = CogColorConstants.Orange;

            width = width / 2;
            height = height / 2;
            lineH.SetFromStartXYEndXY(width, 0, width, height);
            lineV.SetFromStartXYEndXY(0, height, width, height);

            rt.Add(lineV);
            rt.Add(lineH);

            return rt;
        }

        public void CreateCircleGraphic(CogGraphicInteractiveCollection collection, XY centerXY, double radius)
        {
            var circle = new CogCircle();

            circle.CenterX = centerXY.X;
            circle.CenterY = centerXY.Y;
            circle.Radius = radius;
            circle.GraphicDOFEnable = CogCircleDOFConstants.None;
            circle.Interactive = false;
            circle.Color = CogColorConstants.Cyan;

            collection.Add(circle);
        }

        #endregion

        #region Model

        public CogFindLineTool LoadFindLine(string path)
        {
            CogFindLineTool tool = null;

            if (File.Exists(path))
            {
                IFormatter formatter = new BinaryFormatter();

                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

                try
                {
                    tool = (CogFindLineTool)formatter.Deserialize(stream);
                }
                catch
                {
                    stream.Close();

                    return tool;
                }

                stream.Close();
            }

            return tool;
        }

        public CogPMAlignTool LoadPattern(string path)
        {
            CogPMAlignTool tool = null;

            if (File.Exists(path))
            {
                IFormatter formatter = new BinaryFormatter();

                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

                try
                {
                    tool = (CogPMAlignTool)formatter.Deserialize(stream);
                }
                catch
                {
                    stream.Close();

                    return tool;
                }

                stream.Close();
            }

            return tool;
        }

        public CogFixtureTool LoadFixtureTool(string path)
        {
            CogFixtureTool tool = null;

            if (File.Exists(path))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

                try
                {
                    tool = (CogFixtureTool)formatter.Deserialize(stream);
                }
                catch
                {
                    stream.Close();

                    return tool;
                }
                stream.Close();

            }

            return tool;
        }

        public CogBlobTool LoadBlobTool(string path)
        {
            CogBlobTool tool = null;

            if (File.Exists(path))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

                try
                {
                    tool = (CogBlobTool)formatter.Deserialize(stream);
                }
                catch
                {
                    stream.Close();
                    return tool;
                }
                stream.Close();

            }

            return tool;
        }

        public bool SaveFindLineTool(string path, CogFindLineTool tool)
        {
            if (tool != null)
            {
                tool.InputImage = null;
                tool.Run();

                IFormatter formatter = new BinaryFormatter();
                Stream stream;

                stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, tool);
                stream.Close();

                return true;
            }
            else
                return false;
        }

        public bool SavePattern(string path, CogPMAlignTool tool)
        {
            if (tool != null)
            {
                tool.InputImage = null;
                tool.Run();

                IFormatter formatter = new BinaryFormatter();
                Stream stream;

                stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, tool);
                stream.Close();

                return true;
            }
            else
                return false;
        }

        public bool SaveFixtureTool(string path, CogFixtureTool tool)
        {
            if (tool != null)
            {
                tool.InputImage = null;
                tool.Run();

                IFormatter formatter = new BinaryFormatter();
                Stream stream;

                stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, tool);
                stream.Close();

                return true;
            }

            return false;
        }

        public bool SaveBlobTool(string path, CogBlobTool tool)
        {
            if (tool != null)
            {
                tool.InputImage = null;
                tool.Run();

                IFormatter formatter = new BinaryFormatter();
                Stream stream;

                stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, tool);
                stream.Close();

                return true;
            }
            else
                return false;
        }

        public ICogImage LoadImage(string path)
        {
            ICogImage retImage = null;

            var imageFile = new CogImageFile();

            try
            {
                imageFile.Open(path, CogImageFileModeConstants.Read);
                retImage = imageFile[0];
                imageFile.Close();
            }
            catch (Exception e)
            {
                return retImage;
            }

            return retImage;
        }

        public bool SaveImage(string path, ICogImage image)
        {
            if (image == null) return false;

            var imageFile = new CogImageFile();

            try
            {
                imageFile.Open(path, CogImageFileModeConstants.Write);
                imageFile.Append(image);
                imageFile.Close();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
