namespace AcademyPlatform.Web.Models.Courses
{
    using System;
    using AcademyPlatform.Models.Courses;
    using System.ComponentModel.DataAnnotations;

    public class CoursesListViewModel
    {
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

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