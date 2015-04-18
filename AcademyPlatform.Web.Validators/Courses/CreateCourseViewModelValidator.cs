namespace AcademyPlatform.Web.Validators.Courses
{
    using System;
    using AcademyPlatform.Web.Models.Courses;
    using FluentValidation;

    public class CreateCourseViewModelValidator : AbstractValidator<CreateCourseViewModel>
    {
        public CreateCourseViewModelValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate.Date).GreaterThanOrEqualTo(x => x.StartDate.Date).NotEmpty();
            RuleFor(x => x.Title).Length(10, 50).NotEmpty();
            RuleFor(x => x.Description).Length(20, 500).NotEmpty();
        }
    }
}