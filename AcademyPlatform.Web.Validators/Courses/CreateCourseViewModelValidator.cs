namespace AcademyPlatform.Web.Validators.Courses
{
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Web.Models.Courses;

    using FluentValidation;

    public class CreateCourseViewModelValidator : AbstractValidator<CourseEditViewModel>
    {
        public CreateCourseViewModelValidator()
        {
            RuleFor(x => x.PricingType).NotEqual(x => CoursePricingType.None).WithMessage("Моля, посочете тип на курса");
            RuleFor(x => x.Title).Length(10, 50).NotEmpty();
        }
    }
}