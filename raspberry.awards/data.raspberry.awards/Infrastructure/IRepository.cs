namespace data.raspberry.awards.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;

    public interface IRepository<T> where T : class
    {
        void Add(T entity, long idUser);
        void AddAll(IEnumerable<T> entities, long idUser);
        void Update(T entity, long idUser);
        void UpdateAll(IEnumerable<T> entities, long idUser);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        bool Exists(Expression<Func<T, bool>> where);
        T GetById(long id);
        T Get(Expression<Func<T, bool>> where, List<string> includes = null);
        IEnumerable<T> GetAll(List<string> includes = null);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where, List<string> includes = null);

        void SaveChanges();
    }
}
