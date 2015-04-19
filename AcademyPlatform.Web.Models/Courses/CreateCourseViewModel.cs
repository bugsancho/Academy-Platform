namespace AcademyPlatform.Web.Models.Courses
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class CreateCourseViewModel : IMapFrom<Course>
    {
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Начална дата")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Крайна дата")]
        public DateTime EndDate { get; set; }
    }
}