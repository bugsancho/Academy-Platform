namespace AcademyPlatform.Models.Courses
{
    public class LectureResource
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public int Version { get; set; }

        public virtual Lecture Lecture { get; set; }

        public int LectureId { get; set; }

        public LectureResourceType Type { get; set; }
    }
}