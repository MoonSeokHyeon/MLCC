using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace VASFx.Common.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        int Count { get; }
        T GetById(object id);
        IEnumerable<T> GetAll();
        IEnumerable<T> FindBy(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        void Add(T entity);
        void Delete(object entity);
        void Delete(Expression<Func<T, bool>> filter = null);
        void Edit(T entity);
        void Save();
        void Clean();
        void Dispose();
        int Vacuum();
        int Dump(string path, string fileName);
    }
}
