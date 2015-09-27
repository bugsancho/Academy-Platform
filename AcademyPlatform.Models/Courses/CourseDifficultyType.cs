namespace AcademyPlatform.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    public enum CourseDifficultyType
    {
        [Display(Name = "Трудност")]
        Beginner,
        [Display(Name = "Трудност")]
        Intermediate,
        [Display(Name = "Трудност")]
        Challenging
    }
}
