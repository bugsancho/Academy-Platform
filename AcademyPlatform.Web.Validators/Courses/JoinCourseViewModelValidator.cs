namespace AcademyPlatform.Web.Validators.Courses
{
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;

    using FluentValidation;

    public class JoinCourseViewModelValidator : AbstractValidator<JoinCourseViewModel>
    {
        private readonly ICoursesContentService _coursesContent;
        private readonly ICoursesService _courses;

        public JoinCourseViewModelValidator(ICoursesContentService coursesContent, ICoursesService courses)
        {
            _coursesContent = coursesContent;
            _courses = courses;

            RuleFor(x => x.AcceptLicenseTerms).Equal(true).WithMessage("Трябва да приемете тъпите лиценс търмс");
            When(
                x =>
                    {
                        Course course = _coursesContent.GetCourseByNiceUrl(x.CourseNiceUrl);
                        return _courses.IsPaidCourse(course);
                    },
                () =>
                    {
                        RuleFor(x => x.BillingInfo)
                            .NotNull()
                            .WithMessage("Моля попълнете информацията за фактура");
                    });
        }
    }
}
