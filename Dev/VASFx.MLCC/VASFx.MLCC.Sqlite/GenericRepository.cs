using GSG.NET.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Interface;

namespace VASFx.MLCC.Sqlite
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        Logger logger = Logger.GetLogger();
        object lockObj = new object();

        DbContext Context = null;

        public int Count => GetAll().Count();

        DbSet<T> table = null;

        public GenericRepository(DbContext dbContext)
        {
            this.Context = dbContext;
            this.table = Context.Set<T>();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        public void Add(T entity)
        {
            lock (lockObj)
            {
                table.Add(entity);
                this.Save();
            }
        }

        public void Delete(object entity)
        {
            T existing = table.Find(entity);
            if (existing == null)
                return;

            lock (lockObj)
            {
                if (Context.Entry(existing).State == EntityState.Detached)
                {
                    table.Attach(existing);
                }

                table.Remove(existing);
                this.Save();
            }
        }

        public void Delete(Expression<Func<T, bool>> filter = null)
        {
            lock (lockObj)
            {
                var delList = this.table.Where(filter).ToList();
                if (delList.Any() && delList != null)
                {
                    this.table.RemoveRange(delList);
                    Save();
                }
            }
        }

        public void Edit(T entity)
        {
            if (entity == null)
            {
                logger.E($"[DataBase] - Update obj is Null {this.table.GetType().Name}");
                return;
            }

            lock (lockObj)
            {
                table.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
                this.Save();
            }
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = this.table;

            lock (lockObj)
            {
                if (filter != null)
                    query = query.Where(filter);

                if (includeProperties != null)
                {
                    foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        query = query.Include(includeProperty);
                }

                if (orderBy != null)
                    return orderBy(query).ToList();
                else
                    return query.ToList();
            }
        }

        public IEnumerable<T> GetAll()
        {
            lock (lockObj)
                return table.ToList();
        }
            
        public T GetById(object id)
        {
            lock (lockObj)
                return table.Find(id);
        }

        public void Clean()
        {
            lock (lockObj)
            {
                var delList = table.ToList();

                if (delList != null && delList.Any())
                {
                    table.RemoveRange(delList);
                    this.Save();
                }
            }
        }

        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        logger.E("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                return;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
                logger.E($"[DataBase] - DbUpdateConcurrencyException {this.table.GetType().Name}");
                return;
            }
        }

        public virtual T DetachEntity(T entityToDetach)
        {
            if (entityToDetach != null)
                Context.Entry(entityToDetach).State = EntityState.Detached;
            Context.SaveChanges();
            return entityToDetach;
        }

        public int Vacuum()
        {
            return Context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "VACUUM;");
        }

        public int Dump(string path, string fileName)
        {
            //this.Context.Database.Connection.Open();
            //var cmd = this.Context.Database.Connection.CreateCommand();
            //cmd.CommandText = $@"VACUUM INTO '{path +"/" + fileName}'";
            //cmd.ExecuteNonQuery();
            //this.Context.Database.Connection.Close();
            return 0;
        }
    }
}
