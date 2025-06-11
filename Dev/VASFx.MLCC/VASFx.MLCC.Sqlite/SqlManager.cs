using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.HistoryModel;
using VASFx.Common.Interface;
using VASFx.Common.Model;

namespace VASFx.MLCC.Sqlite
{
    public class SqlManager : IDisposable
    {
        public IGenericRepository<JobData> JobInfo { get; set; }
        public IGenericRepository<CameraInfo> CameraInfo { get; set; }
        public IGenericRepository<SystemInfo> SystemInfo { get; set; }
        public IGenericRepository<SystemSetting> SystemSetting { get; set; }
        public IGenericRepository<SystemOption> SystemOption { get; set; }
        public IGenericRepository<ModelData> ModelData { get; set; }
        public IGenericRepository<AlignHistory> AlignHistory { get; set; }
        public IGenericRepository<InspectionHistory> InspectionHistory { get; set; }
        public IGenericRepository<LightControllerData> LightController { get; set; }
        public IGenericRepository<OverlapInfo> OverlapInfo { get; set; }

        public string ConnentionString { get; private set; }

        VASFxContext dbContext = null;
        public SqlManager()
        {
        }

        public void Init(string connectionString)
        {
            this.ConnentionString = connectionString;

            dbContext = new VASFxContext(this.ConnentionString);
            this.JobInfo = new GenericRepository<JobData>(dbContext);
            this.CameraInfo = new GenericRepository<CameraInfo>(dbContext);
            this.SystemInfo = new GenericRepository<SystemInfo>(dbContext);
            this.SystemSetting = new GenericRepository<SystemSetting>(dbContext);
            this.SystemOption = new GenericRepository<SystemOption>(dbContext);
            this.ModelData = new GenericRepository<ModelData>(dbContext);

            this.AlignHistory = new GenericRepository<AlignHistory>(dbContext);
            this.InspectionHistory = new GenericRepository<InspectionHistory>(dbContext);
            this.LightController = new GenericRepository<LightControllerData>(dbContext);
            this.OverlapInfo = new GenericRepository<OverlapInfo>(dbContext);
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }
    }
}
