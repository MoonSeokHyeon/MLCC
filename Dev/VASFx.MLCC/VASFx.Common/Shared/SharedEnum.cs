namespace VASFx.Common.Shared
{
    public enum eMainMenu
    {
        None = -1,
        Auto = 0,
        Setup,
        Calibration,
        Interface,
        Edit,
        Log,
    }
    public enum eSystemState
    {
        None = -1,
        Manual,
        Auto,
    }

    public enum eCoreMessageKind
    {
        SystemStateChange,
        ModelChange,
        ChangeModelList,
        LightValueChange,
        Calibration,
        CalibrationComplete,
        AddedAlignHistory,
        AddedInspectionHistory,
        FailModelChange,
        CameraPropertyChanged,
        MainViewChanged,
        ViewLoggerChanged,
        ResultGraphicChange,
        ResultChange,
    }
    public enum eGUIMessageKind
    {
        SystemStateChange,
        ModelChange,
        ChangeModelList,
        LightValueChange,
        Calibration,
        CalibrationComplete,
        AddedAlignHistory,
        AddedInspectionHistory,
        FailModelChange,
        CameraPropertyChanged,
        MainViewChanged,
        RunProcesser,
    }
    public enum eCameraEventKind
    {
        OneShotGrab,
        ContinuousGrab,
        StopContinususGrab,
        GrabComplated,
        StartGrabbing,
        StopGrabbing,
    }

    public enum eLightPointKind
    {
        InSide,
        OutSide,
        Back,
    }

    public enum eKindFindLine
    {
        Vertical,
        Horizontal,
    }

    public enum ePatternNum
    {
        None,
        Pattern1,
        Pattern2,
        Pattern3,
        Pattern4,
        CalibrationPattern,
    }

    public enum eBlobNum
    {
        None,
        Blob1,
        Blob2,
        Blob3,
        Blob4,
        Blob5,
        Blob6,
    }

    public enum eCamID
    {
        Cam1,
        Cam2,
        Cam3,
        Cam4,
        Cam5,
        Cam6,
        Cam7,
        Cam8,
        Cam9,
        Cam10,
        Cam11,
        Cam12,
        Cam13,
        Cam14,
        Cam15,
        Cam16,
        Cam17,
        Cam18,
        Cam19,
        Cam20,
        Cam21,
        Cam22,
        Cam23,
        Cam24,
        Cam25,
        Cam26,
        Cam27,
        Cam28,
        Cam29,
        Cam30,
        Cam31,
        Cam32
    }
    public enum eLightController
    {
        None = 0,
        COM1,
        COM2,
        COM3,
        COM4,
        COM5,
        COM6,
        COM7,
        COM8,
    }
    public enum eLightChannel
    {
        None = 0,
        Ch1,     
        Ch2, 
        Ch3, 
        Ch4, 
        Ch5, 
        Ch6, 
        Ch7, 
        Ch8,        
    }
    public enum eVisionTools
    {
        Pattern,
        FindLine,
        Fixture,
        Blob,
    }

    public enum eGrabPosition
    {
        First,
        Second,
        Third,
        Fourth
    }

    public enum ePlcKind
    {
        COMMON,
        ALIGN,
        CAL,
        FDC,
    }

    public enum eDisplayMouseMode
    {
        Pointer,
        Pan,
        ZoomIn,
        ZoomOut,
        UserDefined,
        Touch
    }

    public enum eViewLogArgsLevelKind
    {
        Warn,
        Fail,
        Info,
    }

    public enum eViewLogArgsKind
    {
        Align,
        Inspection,
        Calibration,
        PLC,
    }

    public enum eExecuteZone
    {
        NONE, //전체의 영역을 선택 시 사용하자.
        PRE_A,
        PRE_B,
        CHAMBER1_TARGET,
        CHAMBER1_SUBJECT,
        CHAMBER2_TARGET,
        CHAMBER2_SUBJECT,
        TAPE_FEEDER,
        INSPECTION,
        LOADER,
        UNLOADER,
        MLCC_INSPECTION,
    }

    public enum eMesh
    {
        None,
        MESH1,
        MESH2,
        MESH3,
        MESH4,
        MESH5,
        MESH6,
    }

    public enum eOverLap
    {
        None,
        OverLap12,
        OverLap34,
        OverLap56,
        OverLap13,
        OverLap24,
        OverLap35,
        OverLap46,
        OverLap123,
        OverLap124,
        OverLap134,
        OverLap135,
        OverLap345,
        OverLap234,
        OverLap346,
        OverLap246,
        OverLap356,
        OverLap456,
        OverLap1234,
        OverLap1235,
        OverLap1246,
        OverLap1345,
        OverLap2346,
        OverLap3456,
        OverLap2456,
        OverLap1356,
        OverLap12345,
        OverLap12346,
        OverLap12356,
        OverLap12456,
        OverLap13456,
        OverLap23456,
        OverLap123456,
    }

    public enum eVisionAlarm
    {
        None = 0,
        FailPatternMatching,
        FailHorizontalLineFind,
        FailVerticalLineFind,
        FailBolbImage,
        DisconnectedCamera,
        AlignRangeOver,
        ErrorCalibrationData,
        PatternIsNotTrained,
        HorizontalLineIsNotTrained,
        VerticalLineIsNotTrained,
        BlobIsNotTrained,
        InspectionRangeOver,
        InspectionDataIsNotTrained,
        IntersectAngleRangeOver,
        FailLightOn,
    }

}
