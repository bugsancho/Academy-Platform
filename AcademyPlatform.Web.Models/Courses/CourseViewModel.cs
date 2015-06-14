namespace AcademyPlatform.Web.Models.Courses
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class CourseViewModel : IMapFrom<Course>
    {
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Display(Name = "Кратко Описание")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [Display(Name = "Подробно Описание")]
        [DataType(DataType.MultilineText)]
        public string DetailedDescription { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Начална дата")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Крайна дата")]
        public DateTime EndDate { get; set; }
    }
}