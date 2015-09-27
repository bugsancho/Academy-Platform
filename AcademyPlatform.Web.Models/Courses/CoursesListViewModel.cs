namespace AcademyPlatform.Web.Models.Courses
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class CoursesListViewModel : IMapFrom<Course>
    {
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        public string PrettyUrl { get; set; }

        [Display(Name = "Категория")]
        public string CategoryTitle { get; set; }

        [Display(Name = "Кратко Описание")]
        public string ShortDescription { get; set; }

        [Display(Name = "Подробно Описание")]
        public string DetailedDescription { get; set; }

        [Display(Name = "Лектор")]
        public string LecturerName { get; set; }

        [Display(Name = "Начална Дата")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Крайна Дата")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Трудност")]
        public CourseDifficultyType Difficulty { get; set; }

    }
}