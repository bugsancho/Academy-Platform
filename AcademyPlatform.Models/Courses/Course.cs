namespace AcademyPlatform.Models.Courses
{
    using System.Collections.Generic;

    using AcademyPlatform.Models.Base;
    using AcademyPlatform.Models.Payments;

    public class Course : SoftDeletableLoggedEntity
    {
        //private ICollection<Module> _lectures;
        //private ICollection<User> _participants;
        private ICollection<Product> _products;

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ModuleNames { get; set; }

        public int NumberOfHours { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime StartDate { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime EndDate { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public decimal? Price { get; set; }

        public CoursePricingType PricingType { get; set; }

        public CourseDifficultyType Difficulty { get; set; }

        public CourseStatus Status { get; set; }

        //public virtual ICollection<Module> Lectures
        //{
        //    get { return _lectures; }
        //    set { _lectures = value; }
        //}

        //public virtual ICollection<User> Participants
        //{
        //    get { return _participants; }
        //    set { _participants = value; }
        //}

        public virtual ICollection<Product> Products
        {
            get { return _products; }
            set { _products = value; }
        }

        public Course()
        {
            //    _lectures = new HashSet<Module>();
            //    _participants = new HashSet<User>();
            _products = new HashSet<Product>();
        }
    }
}