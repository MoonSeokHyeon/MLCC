using System.Data.Common;
using System.Data.Entity;

namespace VASFx.MLCC.Sqlite
{
    public class VASFxContext : DbContext
    {
        public VASFxContext(string nameOfConnectionString) : base(nameOfConnectionString)
        {
            this.Configure();
        }

        public VASFxContext(DbConnection connection, bool contextOwnsConnection) : base(connection, contextOwnsConnection)
        {
            this.Configure();
        }

        void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            var initializer = new VASFxDbInitializer(modelBuilder);
            Database.SetInitializer(initializer);
            //base.OnModelCreating( modelBuilder );
        }
    }
}
