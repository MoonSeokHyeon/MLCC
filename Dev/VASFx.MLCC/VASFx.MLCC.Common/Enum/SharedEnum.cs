
namespace VASFx.MLCC.Common.Enum
{
    public enum eSystemState
    {
        None = -1,
        Manual,
        Auto
    }

    public enum ePLCKind
    {
        COMMON,
        ALIGN,
        CAL
    }

    public enum eUIKind
    {
        PairCam = 0,
        PairZone,
        Feeder,
        PairFeeder
    }

    public enum eVASFxCoreCommandKind
    {
        Align1st,
        Align2nd,
        Inspection1st,
        Inspection2nd,
        Calibration1st,
        Calibration2nd
    }

    public enum eViewLoggerArgsKind
    {
        Align,
        Inspection,
        Calibration,
        PLC
    }

    public enum eViewLoggerArgsLevelKind
    {
        Warn,
        Fail,
        Info
    }

    public enum eLightPointKind
    {
        Coaxial,  //동축조명
        Bar,      //바조명
        Back,     //백라이트
        Ring,     //링조명
        Dome      //돔조명
    }

    public enum eImageRotateKind
    {
        /// <summary>
        /// Image is not flipped or rotated.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Do not flip, but rotate the image pixels clockwise 90 degrees.
        ///  Rotate the image pixels clockwise 90 degrees, but do not flip the image.
        /// </summary>
        Rotate90Deg = 1,

        /// <summary>
        ///   Do not flip, but rotate the image pixels clockwise 180 degrees.
        ///   Rotate the image pixels clockwise 180 degrees, but do not flip the image.
        /// </summary>
        Rotate180Deg = 2,

        /// <summary>
        ///  Do not flip, but rotate the image pixels clockwise 270 degrees.
        ///  Rotate the image pixels clockwise 270 degrees, but do not flip the image.
        /// </summary>
        Rotate270Deg = 3,

        /// <summary>
        ///  Flip the image horizontally. Do not apply image rotation.
        /// </summary>
        Flip = 4,

        /// <summary>
        ///  Flip the image horizontally and then rotate the image pixels clockwise 90 degrees.
        /// </summary>
        FlipAndRotate90Deg = 5,

        /// <summary>
        ///    Flip the image horizontally and then rotate the image pixels clockwise 180 degrees.
        ///    This is the same as flipping the original image vertically.
        /// </summary>
        FlipAndRotate180Deg = 6,

        /// <summary>
        /// Flip the image horizontally and then rotate the image pixels clockwise 270 degrees.
        /// </summary>
        FlipAndRotate270Deg = 7
    }

    public enum eExecuteZone
    {
        ZONE1 = 1,
        ZONE2,
        ZONE3,
        ZONE4,
        ZONE5,
        ZONE6,
        ZONE7,
        ZONE8
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

    public enum eToolID
    {
        fitst,
        Second,
        Third,
        Fourth,
        Fifth
    }

    public enum eGrabQuarter
    {
        OneQuarter,
        TwoQuarter,
        ThreeQuarter,
        FourQuarter
    }

    public enum eFindReferanceKind
    {
        PatternMatching,
        LineIntersect,
        FixtureLineIntersect,
        Blob
    }

    public enum eVASFxEventKind
    {
        SystemStateChange,
        ModelChange,
        CalibrationComplete,
        AddedAlignHistory,
        AddedInspectionHistory,
    }

    public enum eVisionAlarm
    {
        None = 0,
        FailPatternMatching,
        FailHorizontalLineFind,
        FailVerticalLineFind,
        DisconnectedCamera,
        AlignRangeOver,
        ErrorCalibrationData,
        PatternIsNotTrained,
        HorizontalLineIsNotTrained,
        VerticalLineIsNotTrained,
        InspectionRangeOver,
        InspectionDataIsNotTrained,
        IntersectAngleRangeOver,
        ZoneFirstDataIsNull,
    }

    public enum eVisionTools
    {
        Pattern,
        FindLine,
    }
}
