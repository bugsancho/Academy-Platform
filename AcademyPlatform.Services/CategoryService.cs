namespace AcademyPlatform.Services
{ 
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;
	using AutoMapper.QueryableExtensions;

	using AcademyPlatform.Models.Courses;
	using AcademyPlatform.Services.Contracts;
	using AcademyPlatform.Data.Repositories;	

    public class CategoryService : ICategoryService
    {
		private readonly IRepository<Category> _categories;

		public  CategoryService (IRepository<Category> categories)
		{
			_categories = categories;
		}

        public IEnumerable<Category> GetAll()
        {
             return _categories.All().ToList(); 
        }

		public IEnumerable<T> GetAll<T>() where T : class
        {
            var categories = _categories.All();

            return categories.Project().To<T>().ToList();
        }
		
        public Category GetById(int id)
        {
            return _categories.GetById(id);

        }
		
		public T GetById<T>(int id)
        {
            return Mapper.Map<T>(_categories.GetById(id));

        }

        public void Create(Category category)
        { 	
		    _categories.Add(category);
		    _categories.SaveChanges();
        }

		public void Update(Category category)
        { 	
		    _categories.Update(category);
			_categories.SaveChanges();
        }

        public void Delete(int id)
        {
             _categories.Delete(id);
			 _categories.SaveChanges();
        }

        public void Dispose() 
        {
            _categories.Dispose();
        }
    }
}