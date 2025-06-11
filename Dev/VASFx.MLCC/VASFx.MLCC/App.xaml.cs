using GSG.NET.Concurrent;
using GSG.NET.Logging;
using GSG.NET.PLC.Mitsubishi;
using GSG.NET.PLC.SLMP;
using GSG.NET.Quartz;
using GSG.NET.Utils;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VASFx.Common.Events;
using VASFx.Common.Interface;
using VASFx.Core;
using VASFx.MLCC.Sqlite;
using VASFx.MLCC.UI;
using VASFx.Scheduler.MLCC;
using VASFx.UI.CogDisplayViews;
using VASFx.UI.EditModelViews;
using VASFx.UI.Interactivity;
using VASFx.UI.InterfaceView;
using VASFx.UI.LightControlViews;
using VASFx.UI.LogControls;
using VASFx.UI.OptionControl;
using VASFx.UI.SplashScreenWindows.UI;
using VASFx.UI.VisionEditorViews;

namespace VASFx.MLCC
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : PrismApplication
    {
        Logger logger = Logger.GetLogger();
        public static ISplashScreen splashScreen;
        private ManualResetEvent resetSplashCreated;
        private Thread splashThread;

        private void PrismApplication_Startup(object sender, StartupEventArgs e)
        {
            if (!ProcessUtils.IsOnlyOneInstance)
            {
                MessageBox.Show("Program is already running.");
                this.Shutdown();
                return;
            }

            AppUtils.LogGlobalException();
            SetExceptionHanding();

            try
            {
                LogUtils.Configure("Config/MLCC_log4net.xml");
                GSG.NET.Quartz.QuartzUtils.Init(5);

                #region Splash
                resetSplashCreated = new ManualResetEvent(false);

                splashThread = new Thread(ShowSplash);
                splashThread.SetApartmentState(ApartmentState.STA);
                splashThread.IsBackground = true;
                splashThread.Name = "Splash Screen";
                splashThread.Start();

                resetSplashCreated.WaitOne();
                #endregion

                logger.I(string.Format(string.Empty.PadRight(40, '+') + $" Ver. {AssemblyUtils.GetVersion()} " + string.Empty.PadRight(40, '+')));
            }
            catch (Exception ex)
            {
                logger.E(ex);
                MessageBox.Show(ex.ToString());
                this.Shutdown();
            }
        }

        private void SetExceptionHanding()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";

            try
            {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
            }
            catch (Exception ex)
            {
                //logger.E(ex, "Exception in LogUnhandledException");
                logger.E(ex);
            }
            finally
            {
                //logger.E( exception, message );
                logger.E($"{exception} - {message}");
            }
        }

        private void ShowSplash()
        {
            // Create the window
            VisionSplashWindow animatedSplashScreenWindow = new VisionSplashWindow();
            splashScreen = animatedSplashScreenWindow;

            // Show it
            animatedSplashScreenWindow.SetRange(2);
            animatedSplashScreenWindow.Show();

            // Now that the window is created, allow the rest of the startup to run
            resetSplashCreated.Set();
            System.Windows.Threading.Dispatcher.Run();
        }

        private void PrismApplication_Exit(object sender, ExitEventArgs e)
        {
            var ea = this.Container.Resolve<IEventAggregator>();
            if (ea != null) ea.GetEvent<ApplicationExitEvent>().Publish("");

            var sql = this.Container.Resolve<SqlManager>();
            sql.Dispose();

            logger.I(string.Format(string.Empty.PadRight(100, '+')));
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IContainerProvider>(this.Container);
            containerRegistry.RegisterInstance<IContainerRegistry>(containerRegistry);

            containerRegistry.RegisterSingleton<SqlManager>();
            var sql = this.Container.Resolve<SqlManager>();
            sql.Init("MLCC_Db");

            splashScreen.AddMessage("Create Camera...");
            containerRegistry.RegisterSingleton<CameraManager>();
            var cc = this.Container.Resolve<CameraManager>();
            cc.CreateCameras();
            cc.CreateJobs();

            splashScreen.AddMessage("Create Light...");
            containerRegistry.RegisterSingleton<LightControlManager>();
            var lc = this.Container.Resolve<LightControlManager>();
            lc.CreateLightController();
            //lc.InitControllerValue();

            splashScreen.AddMessage("Initialization PLC...");
            containerRegistry.RegisterSingleton<SlmpManager>();

            App.splashScreen.AddMessage("Resist Scheduler...");
            containerRegistry.RegisterSingleton<MLCCScheduler>();

            var scheduler = this.Container.Resolve<MLCCScheduler>();
            scheduler.Init();

        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule(typeof(LightControlViewModule));
            moduleCatalog.AddModule(typeof(InteractivityModule));
            moduleCatalog.AddModule(typeof(InterfaceViewModule));
            moduleCatalog.AddModule(typeof(CogDisplayViewModule));
            moduleCatalog.AddModule(typeof(OptionViewsModule));
            moduleCatalog.AddModule(typeof(LogViewsModule));
            moduleCatalog.AddModule(typeof(EditModelViewModule));
            moduleCatalog.AddModule(typeof(VisionEditorViewModule));
            moduleCatalog.AddModule(typeof(ShellViewModule));
        }
    }
}

