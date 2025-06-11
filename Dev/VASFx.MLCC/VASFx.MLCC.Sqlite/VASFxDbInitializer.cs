using GSG.NET.Extensions;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using VASFx.Common.Model;
using VASFx.Common.Shared;

namespace VASFx.MLCC.Sqlite
{
    public class VASFxDbInitializer : SqliteDropCreateDatabaseWhenModelChanges<VASFxContext>
    {
        public VASFxDbInitializer(DbModelBuilder modelBuilder) : base(modelBuilder, typeof(CustomHistory))
        {
        }

        protected override void Seed(VASFxContext context)
        {
            context.Set<SystemInfo>().Add(new SystemInfo { SystemID = "TBK", CurrentProductModelId = 1 });

            context.Set<SystemOption>().AddRange(new List<SystemOption>()
            {
                #region Align
                new SystemOption()
                {
                    Name = ConstAlignString.UseLengthCheck,
                    Desc = "UseLengthCheck",
                    EditTime = DateTime.Now,
                    Value = false,
                },
                new SystemOption()
                {
                    Name = ConstAlignString.Target3PAlign,
                    Desc = "Target3PAlign",
                    EditTime = DateTime.Now,
                    Value = false,
                },
                new SystemOption()
                {
                    Name = ConstAlignString.Object3PAlign,
                    Desc = "Object3PAlign",
                    EditTime = DateTime.Now,
                    Value = false,
                },
                new SystemOption()
                {
                    Name = ConstAlignString.Target2PAlign,
                    Desc = "Target2PAlign",
                    EditTime = DateTime.Now,
                    Value = false,
                },
                new SystemOption()
                {
                    Name = ConstAlignString.Object2PAlign,
                    Desc = "Object2PAlign",
                    EditTime = DateTime.Now,
                    Value = false,
                },
                new SystemOption()
                {
                    Name = ConstAlignString.UseCheckProjectionDegree,
                    Desc = "UseCheckProjectionDegree",
                    EditTime = DateTime.Now,
                    Value = true,
                },
                #endregion

                #region ETC
                new SystemOption()
                {
                    Name = ConstETCString.UseAutoModelChange,
                    Desc = "UseAutoModelChange",
                    EditTime = DateTime.Now,
                    Value = false,
                },
                new SystemOption()
                {
                    Name = ConstETCString.UseTimeSync,
                    Desc = "UseTimeSync",
                    EditTime = DateTime.Now,
                    Value = true,
                },
                new SystemOption()
                {
                    Name = ConstETCString.SaveOKImage,
                    Desc = "SaveOKImage",
                    EditTime = DateTime.Now,
                    Value = false,
                },
                new SystemOption()
                {
                    Name = ConstETCString.SaveNGImage,
                    Desc = "SaveNGImage",
                    EditTime = DateTime.Now,
                    Value = true,
                },
                new SystemOption()
                {
                    Name = ConstETCString.SaveGraphics,
                    Desc = "SaveGraphics",
                    EditTime = DateTime.Now,
                    Value = true,
                },
                new SystemOption()
                {
                    Name = ConstETCString.UseAlwaysLightOn,
                    Desc = "UseAlwaysLightOn",
                    EditTime = DateTime.Now,
                    Value = false,
                },
                #endregion

                #region Run Mode
                new SystemOption()
                {
                    Name = ConstRunModeString.UseDryRunMode,
                    Desc = "UseDryRunMode",
                    EditTime = DateTime.Now,
                    Value = false,
                },
                #endregion

                #region Graphic
                new SystemOption()
                {
                    Name = ConstGraphicString.ShowAutoGrabImage,
                    Desc = "ShowAutoGrabImage",
                    EditTime = DateTime.Now,
                    Value = true,
                },
                new SystemOption()
                {
                    Name = ConstGraphicString.ShowOnlyOKCaliper,
                    Desc = "ShowOnlyOKCaliper",
                    EditTime = DateTime.Now,
                    Value = true,
                },
                new SystemOption()
                {
                    Name = ConstGraphicString.ShowRegionOfInterest,
                    Desc = "ShowRegionOfInterest",
                    EditTime = DateTime.Now,
                    Value = true,
                },
                new SystemOption()
                {
                    Name = ConstGraphicString.ShowIntersectionPoint,
                    Desc = "ShowIntersectionPoint",
                    EditTime = DateTime.Now,
                    Value = true,
                },
                new SystemOption()
                {
                    Name = ConstGraphicString.ShowXYTScore,
                    Desc = "ShowXYTScore",
                    EditTime = DateTime.Now,
                    Value = true,
                },
                #endregion

            });

            context.Set<SystemSetting>().AddRange(new List<SystemSetting>()
            {
                #region Auto
                new SystemSetting()
                {
                    Name = ConstAutoString.GrabDelayTime,
                    Desc = "GrabDelayTime",
                    EditTime = DateTime.Now,
                    Value = "10" //ms
                },
                new SystemSetting()
                {
                    Name = ConstAutoString.MaxReAlignCount,
                    Desc = "MaxReAlignCount",
                    EditTime = DateTime.Now,
                    Value = "3" //Num
                },
                new SystemSetting()
                {
                    Name = ConstAutoString.AutoTimeOutLimit,
                    Desc = "AutoTimeOutLimit",
                    EditTime = DateTime.Now,
                    Value = "10" //sec
                },
                new SystemSetting()
                {
                    Name = ConstAutoString.AutoGrabRetryLimit,
                    Desc = "AutoGrabRetryLimit",
                    EditTime = DateTime.Now,
                    Value = "5"
                },
                new SystemSetting()
                {
                    Name = ConstAutoString.ManualMarkRetryLimit,
                    Desc = "ManualMarkRetryLimit",
                    EditTime = DateTime.Now,
                    Value = "2" //Num
                },
                #endregion

                #region Log
                new SystemSetting()
                {
                    Name = ConstLogString.LogSaveTime,
                    Desc = "LogSaveTime",
                    EditTime = DateTime.Now,
                    Value = "0" //hour
                },
                new SystemSetting()
                {
                    Name = ConstLogString.LogBackUpDays,
                    Desc = "LogBackUpDays",
                    EditTime = DateTime.Now,
                    Value = "30" //Day
                },
                new SystemSetting()
                {
                    Name = ConstLogString.ImageBackUpDays,
                    Desc = "ImageBackUpDays",
                    EditTime = DateTime.Now,
                    Value = "30" //Day
                },
                #endregion
            });

            #region Overlap Model
            context.Set<OverlapInfo>().Add(new OverlapInfo()
            {
                OverlapHexa = 60 * Math.Pow(10, 5),
                OverlapPenta = 45 * Math.Pow(10, 5),
                OverlapQuad = 33 * Math.Pow(10, 5),
                OverlapTriple = 25 * Math.Pow(10, 5),
                OverlapDouble = 12 * Math.Pow(10, 5),
                MinSize = 8 * Math.Pow(10, 5),
            });
            #endregion

            var cameras = new List<CameraInfo>();
            var zoneDatas = new List<ZoneData>();
            var lightDatas = new List<LightControllerData>();

            for (eCamID i = eCamID.Cam1; i < eCamID.Cam6 + 1; i++)
                cameras.Add(new CameraInfo
                {
                    CamID = i,
                    ImageRotateType = RotateFlipType.RotateNoneFlipNone,
                    Height = 2748,
                    Width = 3840,
                    SerialNumber = "12345678",
                });

            for (eExecuteZone i = eExecuteZone.NONE; i < eExecuteZone.MLCC_INSPECTION + 1; i++)
                zoneDatas.Add(new ZoneData
                {
                    Zone = i,
                    Score = 80,
                }); ;

            zoneDatas.EachExt(_ =>
            {
                var calList = new List<CalibrationData>();
                for (eGrabPosition j = eGrabPosition.First; j < eGrabPosition.Fourth + 1; j++)
                {
                    calList.Add(new CalibrationData
                    {
                        Zone = _.Zone,
                        GrabPos = j,
                    });
                }
                _.CalibrationDatas = calList;
            });

            for (int i = 1; i < 5; i++)
            {
                lightDatas.Add(new LightControllerData
                {
                    PortNumber = i,
                    BaudRate = 19200,
                    Parity = 0,
                    DataBits = 8,
                    StopBits = 1,
                    MaxChannel = 8,
                    MaxVolume = 255,
                });
            }

            lightDatas.EachExt(_ =>
            {
                var lightValueData = new List<LightValueData>();
                for (int i = 1; i < 9; i++)
                {
                    lightValueData.Add(new LightValueData
                    {
                        Channel = i,
                        ZoneID = CastTo<eExecuteZone>.From<int>(i),
                        GrabPos = CastTo<eGrabPosition>.From<int>(i),
                        LightValue = 100
                    });
                }
                _.LightValues = lightValueData;
            });

            context.Set<CameraInfo>().AddRange(cameras);

            context.Set<LightControllerData>().AddRange(lightDatas);

            var jobs = new List<JobData>();

            for (eExecuteZone i = eExecuteZone.MLCC_INSPECTION; i < eExecuteZone.MLCC_INSPECTION + 1; i++)
            {
                jobs.Add(new JobData
                {
                    ZoneID = i,
                    GrabPos = eGrabPosition.Fourth + 1
                });
            }

            context.Set<JobData>().AddRange(jobs);

            context.Set<ModelData>().Add(new ModelData()
            {
                Name = "DefaultModel",
                ModelId = 1,

                ZoneDatas = zoneDatas,
                LightControllerDatas = lightDatas,
            });
        }
    }
}
