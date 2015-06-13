using System.ComponentModel.DataAnnotations;
namespace AcademyPlatform.Models.Courses
{
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
