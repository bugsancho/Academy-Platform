namespace AcademyPlatform.Web.Models.Courses.Validators
{
    using FluentValidation;

    public class CreateCourseViewModelValidator : AbstractValidator<CourseViewModel>
    {
        public CreateCourseViewModelValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate.Date).NotEmpty();
            RuleFor(x => x.Title).Length(10, 50).NotEmpty();
            RuleFor(x => x.ShortDescription).Length(20, 500).NotEmpty();
        }
    }
}