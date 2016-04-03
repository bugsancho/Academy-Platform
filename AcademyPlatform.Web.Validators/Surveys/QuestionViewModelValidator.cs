namespace AcademyPlatform.Web.Validators.Surveys
{
    using System.Linq;

    using AcademyPlatform.Web.Models.Assessments;

    using FluentValidation;

    public class QuestionViewModelValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionViewModelValidator()
        {
            RuleFor(x => x.Answers).Must(x => x.Any(y => y.IsCorrect)).WithMessage("Моля, изберете поне един верен отговор");
        }
    }
}
