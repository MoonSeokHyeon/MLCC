using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.Common.Interface
{
    public interface ISplashScreen
    {
        void ReportTotalProgress(int persent);
        void AddMessage(string message);
        void LoadComplete();
        void ReportProgress(int persent);
        void SetRange(int count);
        void StepIt();
    }
}
