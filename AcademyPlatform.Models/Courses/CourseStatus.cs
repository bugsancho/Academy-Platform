namespace AcademyPlatform.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    public enum CourseStatus
    {
        None,
        [Display(Name = "Активен")]
        Active,
        [Display(Name = "Очаквайте скоро")]
        AwaitingRelease,
        [Display(Name = "Скрит")]
        Hidden
    }
}