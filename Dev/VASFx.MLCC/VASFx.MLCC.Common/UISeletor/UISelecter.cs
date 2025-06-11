using GSG.NET.Concurrent;
using GSG.NET.Logging;
using GSG.NET.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Enum;
using VASFx.MLCC.Common.Theme;

namespace VASFx.MLCC.Common.UISeletor
{
    public class UISelecter : GSG.NET.ObjectBase.SingletonBase<UISelecter>, IDisposable
    {
        static Logger logger = Logger.GetLogger();

        public eUIKind SelectedUI { get; private set; } = eUIKind.PairCam;

        public string AutoViewName { get; set; }
        public string LogViewName { get; set; }
        public string CalibrationViewName { get; set; }
        public string MainInterfaceViewName { get; set; }
        public string EditViewName { get; set; }

        private UISelecter()
        {
        }

        public void SetUI(eUIKind ui) => this.SelectedUI = ui;

        public void Init()
        {
            Assert.IsTrue(ProcessUtils.IsOnlyOneInstance, "Program is already running.");

            ThreadUtils.InitPoolSize(10);

            //Theme Setting
            ThemeHelper.SetTheme(ThemeHelper.LoadTheme());

            logger.I(string.Format(string.Empty.PadRight(40, '+') + $" Ver. {AssemblyUtils.GetVersion()} " + string.Empty.PadRight(40, '+')));
        }

        public void Dispose()
        {
            logger.I(string.Empty.PadRight(100, '-'));
        }
    }
}
