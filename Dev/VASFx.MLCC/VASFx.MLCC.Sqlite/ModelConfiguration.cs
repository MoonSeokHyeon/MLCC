using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.HistoryModel;
using VASFx.Common.Model;

namespace VASFx.MLCC.Sqlite
{
    public class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobData>();
            modelBuilder.Entity<SystemInfo>();
            modelBuilder.Entity<SystemOption>();
            modelBuilder.Entity<SystemSetting>();

            modelBuilder.Entity<ModelData>();
            modelBuilder.Entity<InspectionHistory>();
            modelBuilder.Entity<OverlapInfo>();

            modelBuilder.Entity<CameraInfo>();

            modelBuilder.Entity<ZoneData>()
                .HasRequired(c => c.ModelData)
                .WithMany(pd => pd.ZoneDatas)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CalibrationData>()
                .HasRequired(c => c.ZoneData)
                .WithMany(cc => cc.CalibrationDatas)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<LightControllerData>()
                .HasRequired(c => c.ModelData)
                .WithMany(l => l.LightControllerDatas)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<LightValueData>()
                .HasRequired(c => c.LightControllerDatas)
                .WithMany(l => l.LightValues)
                .WillCascadeOnDelete(true);
        }
    }
}
