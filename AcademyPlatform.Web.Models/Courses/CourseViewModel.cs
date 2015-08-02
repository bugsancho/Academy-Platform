namespace AcademyPlatform.Web.Models.Courses
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class CourseViewModel : IMapFrom<Course>
    {
        [Display(Name = "Линк")]
        [ScaffoldColumn(false)]
        public string PrettyUrl { get; set; }

        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Display(Name = "Tag Line")]
        public string Tagline { get; set; }

        [Display(Name = "Кратко Описание")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        //[UIHint("tinymce_full")] TODO: fix multiple script references
        //TODO: change selector from all textareas to textarea with id
        public string ShortDescription { get; set; }

        [Display(Name = "Подробно Описание")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [UIHint("tinymce_full")]
        public string DetailedDescription { get; set; }

        [Display(Name = "Начална дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }


        [Display(Name = "Kartinka Описание")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase CourseImage { get; set; }

        [Display(Name = "Крайна дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }
    }
}