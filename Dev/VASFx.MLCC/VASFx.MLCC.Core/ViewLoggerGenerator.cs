using GSG.NET.Concurrent;
using GSG.NET.Logging;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Enum;
using VASFx.MLCC.Common.Events;

namespace VASFx.MLCC.Core
{
    public class ViewLoggerGenerator : IDisposable
    {
        TsQueue<ViewLoggerEventArgs> qEvent = new TsQueue<ViewLoggerEventArgs>();
        ThreadCancel threadCancel = new ThreadCancel();
        ViewLoggerPubSubEvent Publisher = null;

        Logger logger = Logger.GetLogger();

        bool isInited = false;

        public ViewLoggerGenerator(IEventAggregator ea)
        {
            Publisher = ea.GetEvent<ViewLoggerPubSubEvent>();
            ea.GetEvent<ApplicationExitEvent>().Subscribe((o) => Dispose(), true);
        }

        public void Init()
        {
            if (isInited) return;
            isInited = true;

            this.threadCancel.AddGo(Th_Pull);
        }

        public void Dispose()
        {
            this.threadCancel.Cancel();
            this.threadCancel.StopWaitAll();
        }

        public void Publish(string message, eViewLoggerArgsKind kind = eViewLoggerArgsKind.Align, eViewLoggerArgsLevelKind level = eViewLoggerArgsLevelKind.Info)
        {
            var arg = new ViewLoggerEventArgs();
            arg.Kind = kind;
            arg.Level = level;
            arg.Message = message;

            this.qEvent.Enqueue(arg);
        }

        void Th_Pull()
        {
            Thread.CurrentThread.Name = $"{this.GetType().Name}";
            logger.D($"Process Started {Thread.CurrentThread.Name}");

            while (!this.threadCancel.Canceled)
            {
                try
                {
                    var q = this.qEvent.Dequeue();
                    this.Publisher.Publish(q);
                }
                catch (Exception e)
                {
                    logger.E(e);
                }
            }

            logger.D($"Process Disposed - {Thread.CurrentThread.Name}");
        }

    }
}
