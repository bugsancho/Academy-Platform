namespace AcademyPlatform.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    public enum CourseDifficultyType
    {
        [Display(Name = "Лесен")]
        Beginner,
        [Display(Name = "Среден")]
        Intermediate,
        [Display(Name = "Труден")]
        Challenging
    }
}
