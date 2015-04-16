namespace AcademyPlatform.Validators.Courses
{
    using AcademyPlatform.Models.Courses;
    using FluentValidation;

    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(x => x.Title).Length(5, 10);
        }
    }
}