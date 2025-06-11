using Cognex.VisionPro;
using GSG.NET.Concurrent;
using GSG.NET.Logging;
using GSG.NET.Utils;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Events;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using VASFx.Core;
using VASFx.MLCC.Sqlite;
using VASFx.VisionLibrary.Cognex;

namespace VASFx.InspectionFormulas.MeshAvailability
{
    public class AlgorithmProcesser
    {
        #region Properties
        Logger logger = Logger.GetLogger();
        CameraManager cameraManager = null;
        LightControlManager lightControlManager = null;
        SqlManager sql = null;

        #endregion

        #region Struct

        public AlgorithmProcesser(SqlManager sql, CameraManager cameraManager, LightControlManager lightControlManager)
        {
            this.sql = sql;
            this.cameraManager = cameraManager;
            this.lightControlManager = lightControlManager;
        }

        #endregion

        #region Mesh Inspection Method
        public ResultMLCCInspection RunMeshAvailability(eExecuteZone zoneID, eGrabPosition grabPos, eCamID camID, ICogImage loadImage = null)
        {
            var retInspection = new ResultMLCCInspection((int)eExecuteZone.MLCC_INSPECTION);

            var modelData = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;
            var overlapDatas = sql.OverlapInfo.GetAll().FirstOrDefault();
            logger.E($"1. DB Model & overlap data get success");

            LockUtils.Wait(50);

            //overlap size 
            var overlapHexa = overlapDatas.OverlapHexa;
            var overlapPenta = overlapDatas.OverlapPenta;
            var overlapQuad = overlapDatas.OverlapQuad;
            var overlapTriple = overlapDatas.OverlapTriple;
            var overlapDouble = overlapDatas.OverlapDouble;
            var minArea = overlapDatas.MinSize;


            var inspectionResults = new List<ResultMLCCItem>();
            var overLapList = new List<eOverLap>();

            // Light On
            //var lightController = modelData.LightControllerDatas.FirstOrDefault(x => x.PortNumber.Equals((int)eLightController.COM1));

            //var ch1LightData = lightController.LightValues.FirstOrDefault(x => x.Channel.Equals((int)eLightChannel.Ch1));
            //var ch2LightData = lightController.LightValues.FirstOrDefault(x => x.Channel.Equals((int)eLightChannel.Ch2));
            //var ch3LightData = lightController.LightValues.FirstOrDefault(x => x.Channel.Equals((int)eLightChannel.Ch3));
            //var ch4LightData = lightController.LightValues.FirstOrDefault(x => x.Channel.Equals((int)eLightChannel.Ch4));

            //var ch1Light = lightControlManager.LightControllers[(int)eLightController.COM1].LightOn(ch1LightData.Channel, ch1LightData.LightValue);
            //var ch2Light = lightControlManager.LightControllers[(int)eLightController.COM1].LightOn(ch2LightData.Channel, ch2LightData.LightValue);
            //var ch3Light = lightControlManager.LightControllers[(int)eLightController.COM1].LightOn(ch3LightData.Channel, ch3LightData.LightValue);
            //var ch4Light = lightControlManager.LightControllers[(int)eLightController.COM1].LightOn(ch4LightData.Channel, ch4LightData.LightValue);
            lightControlManager.SetInspectionLightOn();
            logger.E($"2. Light controller on success");

            LockUtils.Wait(100);

            //if(!ch1Light || !ch2Light) 
            //{
            //    retInspection.ResultMLCCItems = inspectionResults;
            //    retInspection.Alarm = eVisionAlarm.FailLightOn;
            //    return retInspection;
            //}

            object grabImage = null;
           
            grabImage = cameraManager.GrabOneShot(camID);            

            if(grabImage == null)
            {
                logger.E($"Camera image data null");

                retInspection.ResultMLCCItems = inspectionResults;
                retInspection.Alarm = eVisionAlarm.DisconnectedCamera;
                return retInspection;
            }

            var cogImage = grabImage as CogImage8Grey;

            cameraManager.CogJobs[zoneID][grabPos].SaveImage(cogImage, ConstLogString.ImageBackUpPath);
            logger.E($"3. Grab Image save success");

            //Run BlobTool
            cameraManager.CogJobs[zoneID][grabPos].RunBlobTool(eBlobNum.Blob1, cogImage, out List<BlobResult> blobResult, (int)minArea);
            logger.E($"4. Blob tool run success");

            #region Mesh Inspection 

            for (int i = 0; i < blobResult.Count; i++)
            {
                // Mesh Size Check
                var hexaOverlap = blobResult[i].Area >= overlapHexa;
                var pentaOverlap = blobResult[i].Area >= overlapPenta && blobResult[i].Area < overlapHexa;
                var quadOverlap = blobResult[i].Area >= overlapQuad && blobResult[i].Area < overlapPenta;
                var TripleOverlap = blobResult[i].Area >= overlapTriple && blobResult[i].Area < overlapQuad;
                var doubleOverlap = blobResult[i].Area >= overlapDouble && blobResult[i].Area < overlapTriple;
                var normal = blobResult[i].Area > minArea && blobResult[i].Area < overlapDouble;

                // Mesh IsExistence Detecting
                var result = new ResultMLCCItem(i);
                result.MeshID = InspectionExistence(blobResult[i], cogImage);
                result.Area = blobResult[i].Area;
                result.IsExistence = true;

                // OverLab Detecting
                if (hexaOverlap)
                {
                    var overLapIndex = eOverLap.OverLap123456;
                    overLapList.Add(overLapIndex);
                }
                else if (pentaOverlap)
                {
                    var overLapIndex = InspectionOverLapFiveLayer(blobResult[i], cogImage);
                    overLapList.Add(overLapIndex);
                }
                else if (quadOverlap)
                {
                    var overLapIndex = InspectionOverLapFourLayer(blobResult[i], cogImage);
                    overLapList.Add(overLapIndex);
                }
                else if (TripleOverlap)
                {
                    var overLapIndex = InspectionOverLapThreeLayer(blobResult[i], cogImage);
                    overLapList.Add(overLapIndex);
                }
                else if (doubleOverlap)
                {
                    var overLapIndex = InspectionOverLapTwoLayer(blobResult[i], cogImage);
                    overLapList.Add(overLapIndex);
                }

                inspectionResults.Add(result);
            }

            // isExistence False Mesh Create
            for (int i = (int)eMesh.MESH1; i <= (int)eMesh.MESH6; i++)
            {
                var existMesh = inspectionResults.Where(x => x.MeshID.Equals((eMesh)i)).FirstOrDefault();

                if (existMesh == null)
                {
                    var result = new ResultMLCCItem(i);
                    result.MeshID = (eMesh)i;
                    result.IsExistence = false;
                    inspectionResults.Add(result);
                }
            }

            // IsOverlab Data Change
            foreach (var item in overLapList)
            {
                switch (item)
                {
                    case eOverLap.OverLap12:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap13:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap24:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap34:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap35:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap46:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap56:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap123:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap124:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap134:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap135:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap234:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap246:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap345:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap346:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap356:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap456:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap1234:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap1235:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap1246:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap1345:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap1356:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap2346:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap2456:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap3456:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap12345:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap12346:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap12356:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap12456:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap13456:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH1)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                    case eOverLap.OverLap23456:
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH2)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH3)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH4)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH5)).FirstOrDefault().IsOverlap = true;
                        inspectionResults.Where(x => x.MeshID.Equals(eMesh.MESH6)).FirstOrDefault().IsOverlap = true;
                        break;
                }
            }

            #endregion

            //Light off
            //lightControlManager.LightControllers[(int)eLightController.COM1].LightOff(ch1LightData.Channel);
            //lightControlManager.LightControllers[(int)eLightController.COM1].LightOff(ch2LightData.Channel);
            //lightControlManager.LightControllers[(int)eLightController.COM1].LightOff(ch3LightData.Channel);
            //lightControlManager.LightControllers[(int)eLightController.COM1].LightOff(ch4LightData.Channel);

            lightControlManager.SetInspectionLightOff();
            logger.E($"5. Light controller off success");

            //GUI Graphic Publish
            var graphic = new CogGraphicInteractiveCollection();

            for (int i = 0; i < blobResult.Count; i++)
            {
                graphic.Add(blobResult[i].BlobGraphic);
            }

            //return Result Data
            retInspection.ResultMLCCItems = inspectionResults;
            retInspection.Alarm = eVisionAlarm.None;
            retInspection.Image = cogImage;
            retInspection.Graphics = graphic;
            logger.E($"6. Mesh inspection success");

            return retInspection;
        }

        private eMesh InspectionExistence(BlobResult result, ICogImage inputImage)
        {
            var resultCenterX = result.CenterOfMassX;
            var resultCenterY = result.CenterOfMassY;
            var boundWidth = result.BoundWidth;
            var boundHeight = result.BoundHeight;
            var boundMinX = result.ImageBoundMinX;
            var boundMaxX = result.ImageBoundMaxX;

            var scale = 1.3;

            #region Area Setting

            var fristVer = inputImage.Width / 3;
            var secondVer = (inputImage.Width / 3) * 2;
            var fristHor = inputImage.Height / 2;

            var area1 = (secondVer < resultCenterX) && (resultCenterY < fristHor);
            var area2 = (secondVer < resultCenterX) && (resultCenterY >= fristHor);
            var area3 = (fristVer < resultCenterX && secondVer >= resultCenterX) && (resultCenterY < fristHor);
            var area4 = (fristVer < resultCenterX && secondVer >= resultCenterX) && (resultCenterY >= fristHor);
            var area5 = (fristVer >= resultCenterX) && (resultCenterY < fristHor);
            var area6 = (fristVer >= resultCenterX) && (resultCenterY >= fristHor);
            
            #endregion

            if (area1)          
                return eMesh.MESH1;
            if (area2)
                return eMesh.MESH2;
            if (area3)
                return eMesh.MESH3;
            if (area4)
                return eMesh.MESH4;
            if (area5)
                return eMesh.MESH5;
            if (area6)
                return eMesh.MESH6;

            return eMesh.None;
        }
        private eOverLap InspectionOverLapTwoLayer(BlobResult result, ICogImage inputImage)
        {
            var resultCenterX = result.CenterOfMassX;
            var resultCenterY = result.CenterOfMassY;
            var boundWidth = result.BoundWidth;
            var boundHeight = result.BoundHeight;
            var boundMinX = result.ImageBoundMinX;
            var boundMaxX = result.ImageBoundMaxX;

            var scale = 1.3;

            #region Area Setting

            var fristVer = inputImage.Width / 3;
            var secondVer = (inputImage.Width / 3) * 2;
            var fristHor = inputImage.Height / 2;

            var area1 = (secondVer < resultCenterX) && (resultCenterY < fristHor);
            var area2 = (secondVer < resultCenterX) && (resultCenterY >= fristHor);
            var area3 = (fristVer < resultCenterX && secondVer >= resultCenterX) && (resultCenterY < fristHor);
            var area4 = (fristVer < resultCenterX && secondVer >= resultCenterX) && (resultCenterY >= fristHor);
            var area5 = (fristVer >= resultCenterX) && (resultCenterY < fristHor);
            var area6 = (fristVer >= resultCenterX) && (resultCenterY >= fristHor);

            var overLapLaft = boundMinX < fristVer;
            var overLapLight = boundMaxX >= secondVer;
            #endregion

            // over lap
            if (area1)
            {
                if (boundWidth >= boundHeight * scale)
                    return eOverLap.OverLap13;

                else if (boundHeight >= boundWidth * scale)
                    return eOverLap.OverLap12;
            }
            if (area2)
            {
                if (boundWidth >= boundHeight * scale)
                    return eOverLap.OverLap24;

                else if (boundHeight >= boundWidth * scale)
                    return eOverLap.OverLap12;
            }

            if (area3)
            {
                if (boundWidth >= boundHeight * scale)
                    if (overLapLaft)
                        return eOverLap.OverLap35;
                    else
                        return eOverLap.OverLap13;

                else if (boundHeight >= boundWidth * scale)
                    return eOverLap.OverLap34;
            }

            if (area4)
            {
                if (boundWidth >= boundHeight * scale)
                    if (overLapLaft)
                        return eOverLap.OverLap46;
                    else
                        return eOverLap.OverLap24;

                else if (boundHeight >= boundWidth * scale)
                    return eOverLap.OverLap34;
            }

            if (area5)
            {
                if (boundWidth >= boundHeight * scale)
                    return eOverLap.OverLap35;

                else if (boundHeight >= boundWidth * scale)
                    return eOverLap.OverLap56;
            }

            if (area6)
            {
                if (boundWidth >= boundHeight * scale)
                    return eOverLap.OverLap46;

                else if (boundHeight >= boundWidth * scale)
                    return eOverLap.OverLap56;
            }

            return eOverLap.None;
        }
        private eOverLap InspectionOverLapThreeLayer(BlobResult result, ICogImage inputImage)
        {
            var resultCenterX = result.CenterOfMassX;
            var resultCenterY = result.CenterOfMassY;
            var boundWidth = result.BoundWidth;
            var boundHeight = result.BoundHeight;
            var boundMinX = result.ImageBoundMinX;
            var boundMaxX = result.ImageBoundMaxX;

            var scale = 1.3;

            #region Area Setting

            var fristVer = inputImage.Width / 3;
            var secondVer = (inputImage.Width / 3) * 2;
            var fristHor = inputImage.Height / 2;

            var area1 = (secondVer < resultCenterX) && (resultCenterY < fristHor);
            var area2 = (secondVer < resultCenterX) && (resultCenterY >= fristHor);
            var area3 = (fristVer < resultCenterX && secondVer >= resultCenterX) && (resultCenterY < fristHor);
            var area4 = (fristVer < resultCenterX && secondVer >= resultCenterX) && (resultCenterY >= fristHor);
            var area5 = (fristVer >= resultCenterX) && (resultCenterY < fristHor);
            var area6 = (fristVer >= resultCenterX) && (resultCenterY >= fristHor);

            var overLapLaft = boundMinX < fristVer;
            var overLapLight = boundMaxX >= secondVer;
            var overLapHorBottom = resultCenterY >= fristHor;

            #endregion

            // over lap
            if (area1)
                return eOverLap.OverLap123;

            if (area2)
                return eOverLap.OverLap124;

            if (area3)
            {
                if (overLapLaft)
                {
                    if (overLapLight)
                        return eOverLap.OverLap135;
                    else
                    {
                        if(overLapHorBottom)
                        {
                            return eOverLap.OverLap345;
                        }
                        return eOverLap.OverLap356;
                    }
                }
                else
                {


                    return eOverLap.OverLap134;
                }
            }

            if (area4)
            {
                if (overLapLaft)
                {
                    if (overLapLight)
                        return eOverLap.OverLap246;
                    else
                    {
                        if(overLapHorBottom)
                        {
                            return eOverLap.OverLap346;

                        }
                        return eOverLap.OverLap345;
                    }
                }
                else
                {
                    return eOverLap.OverLap234;
                }
            }

            if (area5)
                return eOverLap.OverLap356;

            if (area6)
                return eOverLap.OverLap456;

            return eOverLap.None;
        }
        private eOverLap InspectionOverLapFourLayer(BlobResult result, ICogImage inputImage)
        {
            var resultCenterX = result.CenterOfMassX;
            var resultCenterY = result.CenterOfMassY;
            var boundWidth = result.BoundWidth;
            var boundHeight = result.BoundHeight;
            var boundMinX = result.ImageBoundMinX;
            var boundMaxX = result.ImageBoundMaxX;

            var scale = 1.3;

            #region Area Setting

            var fristVer = inputImage.Width / 3;
            var secondVer = (inputImage.Width / 3) * 2;
            var fristHor = inputImage.Height / 2;

            var centerLine = inputImage.Width / 2;

            var area1 = (secondVer < resultCenterX) && (resultCenterY < fristHor);
            var area2 = (secondVer < resultCenterX) && (resultCenterY >= fristHor);
            var area3 = (fristVer < resultCenterX && secondVer >= resultCenterX) && (resultCenterY < fristHor);
            var area4 = (fristVer < resultCenterX && secondVer >= resultCenterX) && (resultCenterY >= fristHor);
            var area5 = (fristVer >= resultCenterX) && (resultCenterY < fristHor);
            var area6 = (fristVer >= resultCenterX) && (resultCenterY >= fristHor);

            var centerLeft = (centerLine >= resultCenterX);

            var overLapLaft = boundMinX < fristVer;
            var overLapLight = boundMaxX >= secondVer;
            #endregion

            // over lap
            if (area1)
            {
                if (overLapLaft)
                    return eOverLap.OverLap1235;
                else
                    return eOverLap.OverLap1234;
            }
            if (area2)
            {
                if (overLapLaft)
                    return eOverLap.OverLap1246;
                else
                    return eOverLap.OverLap1234;
            }

            if (area3)
            {
                if (overLapLaft && overLapLight)
                    return eOverLap.OverLap1345;
                else if (centerLeft)
                    return eOverLap.OverLap1234;
                else
                    return eOverLap.OverLap3456;
            }

            if (area4)
            {
                if (overLapLaft && overLapLight)
                    return eOverLap.OverLap2346;
                else if (centerLeft)
                    return eOverLap.OverLap1234;
                else
                    return eOverLap.OverLap3456;
            }

            if (area5)
            {
                if (overLapLight)
                    return eOverLap.OverLap1356;
                else
                    return eOverLap.OverLap3456;
            }

            if (area6)
            {
                if (overLapLight)
                    return eOverLap.OverLap2456;
                else
                    return eOverLap.OverLap3456;
            }

            return eOverLap.None;
        }
        private eOverLap InspectionOverLapFiveLayer(BlobResult result, ICogImage inputImage)
        {
            var resultCenterX = result.CenterOfMassX;
            var resultCenterY = result.CenterOfMassY;
            var boundWidth = result.BoundWidth;
            var boundHeight = result.BoundHeight;
            var boundMinX = result.ImageBoundMinX;
            var boundMaxX = result.ImageBoundMaxX;

            var scale = 1.3;

            #region Area Setting

            var fristVer = inputImage.Width / 3;
            var secondVer = (inputImage.Width / 3) * 2;
            var fristHor = inputImage.Height / 2;

            var centerLine = inputImage.Width / 2;

            var area1 = (secondVer < resultCenterX) && (resultCenterY < fristHor);
            var area2 = (secondVer < resultCenterX) && (resultCenterY >= fristHor);
            var area3 = (fristVer < resultCenterX && secondVer >= resultCenterX) && (resultCenterY < fristHor);
            var area4 = (fristVer < resultCenterX && secondVer >= resultCenterX) && (resultCenterY >= fristHor);
            var area5 = (fristVer >= resultCenterX) && (resultCenterY < fristHor);
            var area6 = (fristVer >= resultCenterX) && (resultCenterY >= fristHor);

            var centerLeft = (centerLine >= resultCenterX);

            var overLapLaft = boundMinX < fristVer;
            var overLapLight = boundMaxX >= secondVer;
            #endregion

            // over lap
            if (area1)
                return eOverLap.OverLap12345;
            if (area2)
                return eOverLap.OverLap12346;
            if (area3)
                return eOverLap.OverLap12356;
            if (area4)
                return eOverLap.OverLap12456;
            if (area5)
                return eOverLap.OverLap13456;
            if (area6)
                return eOverLap.OverLap23456;

            return eOverLap.None;
        }

        #endregion
    }
}

