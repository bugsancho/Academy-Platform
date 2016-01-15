namespace AcademyPlatform.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> All();

        T GetById(object id);

        void Add(T entity);

        void Update(T entity);

        void Delete(object id);

        int SaveChanges();

        void Dispose();

        IQueryable<T> AllIncluding<TProp>(params Expression<Func<T, TProp>>[] expressions);
    }
}
