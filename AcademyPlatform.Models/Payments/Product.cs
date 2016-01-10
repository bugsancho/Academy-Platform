namespace AcademyPlatform.Models.Payments
{
    using AcademyPlatform.Models.Courses;

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Course Course { get; set; }

        public int CourseId { get; set; }

        public decimal Price { get; set; }

        public ProductType Type { get; set; }

        public bool IsActive { get; set; }

    }
}
