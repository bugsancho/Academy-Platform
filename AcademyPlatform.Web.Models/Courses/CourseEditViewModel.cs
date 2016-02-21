namespace AcademyPlatform.Web.Models.Courses
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class CourseEditViewModel : IMapFrom<Course>
    {
        [Display(Name = "Заглавие")]
        public string Title { get; set; }
        
        [UIHint("EntityDropdown")]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        
        [Display(Name = "Вид на курса")]
        public CoursePricingType PricingType { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }
    }
}