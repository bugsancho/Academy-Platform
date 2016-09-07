namespace AcademyPlatform.Web.Models.Surveys
{
    using System;

    public class AssessmentFailureViewModel
    {
        public int CorrectAnswers { get; set; }

        public int RequiredAnswers { get; set; }

        public int NumberOfQuestions { get; set; }

        public string CourseTitle { get; set; }

        public string CourseUrl { get; set; }

        public DateTime NextAttempt { get; set; }
    }
}
