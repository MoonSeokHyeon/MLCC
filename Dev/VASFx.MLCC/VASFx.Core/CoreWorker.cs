using GSG.NET.Concurrent;
using GSG.NET.Logging;
using GSG.NET.Vision.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VASFx.Core
{
    public class CoreWorker : IDisposable
    {
        static Logger logger = Logger.GetLogger(typeof(CoreWorker));
        public bool IsProcessing { get; set; }
        public TsQueue<eVASFxCoreCommandKind> WorkQueue { get; set; } = new TsQueue<eVASFxCoreCommandKind>();

        #region Actions
        public Action Align1st { get; set; }
        public Action Align2nd { get; set; }
        public Action Calibration1st { get; set; }
        public Action Calibration2nd { get; set; }
        public Action Inspection1st { get; set; }
        public Action Inspection2nd { get; set; }
        #endregion

        string _name = string.Empty;
        public string Name { get => this._name; }
        public CoreWorker(string name)
        {
            this._name = name;
            Thread4.Go(Run);
        }

        public void Dispose()
        {
            this.WorkQueue.Enqueue(eVASFxCoreCommandKind.Dispose);
        }

        void Run()
        {
            Thread.CurrentThread.Name = this._name;
            logger.D($"Core Worker {this._name} - Start Thread");

            while (true)
            {
                try
                {
                    IsProcessing = false;
                    var q = WorkQueue.Dequeue();
                    IsProcessing = true;

                    switch (q)
                    {
                        case eVASFxCoreCommandKind.Align1st:
                            Align1st();
                            break;
                        case eVASFxCoreCommandKind.Align2nd:
                            Align2nd();
                            break;
                        case eVASFxCoreCommandKind.Calibration1st:
                            Calibration1st();
                            break;
                        case eVASFxCoreCommandKind.Calibration2nd:
                            Calibration2nd();
                            break;
                        case eVASFxCoreCommandKind.Inspection1st:                           
                            Inspection1st();
                            break;
                        case eVASFxCoreCommandKind.Inspection2nd:
                            Inspection2nd();
                            break;
                        case eVASFxCoreCommandKind.Dispose:
                            logger.I($"Core Worker Thread Disposed - {this._name}");
                            return;
                        default:
                            break;
                    }

                }
                catch (Exception ex)
                {
                    logger.E($"Core Worker Expection - {ex}");
                }
            }
        }
    }


    public enum eVASFxCoreCommandKind
    {
        Align1st,
        Align2nd,
        Inspection1st,
        Inspection2nd,
        Calibration1st,
        Calibration2nd,
        Dispose,
    }
}
