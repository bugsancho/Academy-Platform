namespace AcademyPlatform.Web.Validators.Courses
{
    using AcademyPlatform.Web.Models.Courses;

    using FluentValidation;

    public class CreateCourseViewModelValidator : AbstractValidator<CourseEditViewModel>
    {
        public CreateCourseViewModelValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate.Date).NotEmpty();
            RuleFor(x => x.Title).Length(10, 50).NotEmpty();
        }
    }
}