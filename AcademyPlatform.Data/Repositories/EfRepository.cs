namespace AcademyPlatform.Data.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public class EfRepository<T> : IRepository<T>
        where T : class
    {
        protected IAcademyPlatformDbContext context;
        protected IDbSet<T> set;

        public EfRepository(IAcademyPlatformDbContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.set;
        }

        public IQueryable<T> AllIncluding<TProp>(params Expression<Func<T,TProp>>[] expressions)
        {
            var query = this.All();
            foreach (var expression in expressions)
            {
                query = query.Include(expression);
            }

            return query;
        }

        public T GetById(object id)
        {
            return this.set.Find(id);
        }

        public void Add(T entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }

        public void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public void Delete(object id)
        {
            this.ChangeState(this.GetById(id), EntityState.Deleted);
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        private void ChangeState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            entry.State = state;
        }
    }
}
