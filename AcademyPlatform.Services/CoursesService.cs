namespace AcademyPlatform.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Services.Contracts;

    public class CoursesService : ICoursesService
    {
        private readonly IRepository<Course> _courses;
        private readonly IRepository<Product> _products;

        private readonly IRepository<User> _users;

        public CoursesService(IRepository<Course> courses, IRepository<User> users, IRepository<Product> products)
        {
            this._courses = courses;
            this._users = users;
            _products = products;
        }

        public IEnumerable<Course> GetAll()
        {
            return this._courses.All().OrderBy(x => x.Id);
        }

        public IEnumerable<Course> GetActiveCourses()
        {
            return this._courses.AllIncluding(x => x.Category);
        }

        public IEnumerable<Course> GetCoursesByUserId(string userId)
        {
            return this._users.GetById(userId).Courses;
        }

        public Course GetById(int id)
        {
            return this._courses.AllIncluding(x => x.Category).FirstOrDefault(x => x.Id == id);
        }


        public void Create(Course course)
        {
            this._courses.Add(course);
            this._courses.SaveChanges();

            if (course.PricingType == CoursePricingType.PaidAccess)
            {
                var product = GenerateProduct(course);
                _products.Add(product);
                _products.SaveChanges();
            }
        }
        public void Update(Course course)
        {
            this._courses.Update(course);
            this._courses.SaveChanges();

            if (course.PricingType == CoursePricingType.PaidAccess)
            {
                var product = course.Products.FirstOrDefault(x => x.Type == ProductType.CourseAccess);
                if (product == null)
                {
                    product = GenerateProduct(course);
                    _products.Add(product);

                }
                else
                {
                    product.Name = $"Достъп до обучение: '{course.Title}'";
                    product.Price = course.Price.Value;
                }

                _products.SaveChanges();
            }
        }

        public void Delete(Course course)
        {
            this._courses.Delete(course);
            this._courses.SaveChanges();
        }

        public bool IsPaidCourse(Course course)
        {
            return course.PricingType == CoursePricingType.PaidAccess;
        }

        private Product GenerateProduct(Course course)
        {
            if (!course.Price.HasValue)
            {
                throw new ArgumentException($"Course with title '{course.Title}' doesn't have a price");
            }

            Product product = new Product
            {
                Name = $"Достъп до обучение: '{course.Title}'",
                Price = course.Price.Value,
                Course = course,
                IsActive = true,
                Type = ProductType.CourseAccess
            };

            return product;
        }
    }
}