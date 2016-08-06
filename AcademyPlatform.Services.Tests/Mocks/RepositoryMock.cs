namespace AcademyPlatform.Services.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models.Base;

    public class RepositoryMock<T> : IRepository<T> where T : class, ISoftDeletableEntity
    {
        private readonly List<T> _items = new List<T>();

        public void Add(T entity)
        {
            _items.Add(entity);
        }

        public IQueryable<T> All()
        {
            return _items.AsQueryable();
        }

        public IQueryable<T> AllIncluding<TProp>(params System.Linq.Expressions.Expression<Func<T, TProp>>[] expressions)
        {
            return _items.AsQueryable();
        }

        public void Delete(object id)
        {
            _items.RemoveAll(x => x.Id == (int)id);
        }

        public void Dispose()
        {
        }

        public T GetById(object id)
        {
            return _items.Find(x => x.Id == (int)id);
        }

        public int SaveChanges()
        {
            return 0;
        }

        public void Update(T entity)
        {
            var item = GetById(entity.Id);
            int index = _items.IndexOf(entity);
            _items.RemoveAt(index);
            _items.Insert(index, entity);
        }
    }
}
