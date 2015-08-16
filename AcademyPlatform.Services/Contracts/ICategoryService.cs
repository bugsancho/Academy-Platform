namespace AcademyPlatform.Services.Contracts
{ 
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using AcademyPlatform.Models.Courses;
	
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();

		IEnumerable<T> GetAll<T>() where T : class;

        Category GetById(int id);

        T GetById<T>(int id);

        void Create(Category category);

		void Update(Category category);

        void Delete(int id);
		
        void Dispose();
    }
}