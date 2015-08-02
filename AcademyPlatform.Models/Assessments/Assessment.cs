namespace AcademyPlatform.Models.Assessments
{
    using System.Collections.Generic;

    public class Assessment
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; } //TODO change with a database entry

        public string Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

    }
}
