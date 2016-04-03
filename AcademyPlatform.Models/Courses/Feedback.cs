namespace AcademyPlatform.Models.Courses
{
    using AcademyPlatform.Models.Base;

    public class Feedback : SoftDeletableEntity
    {
        public int Id { get; set; }

        public virtual Course Course { get; set; }

        public int CourseId { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// JSON representation of all the answers including the questions themseves.
        /// </summary>
        public string Submission { get; set; }

        public string AdditionalNotes { get; set; }
    }
}
