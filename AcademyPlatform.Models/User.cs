namespace AcademyPlatform.Models
{
    using System.Collections.Generic;

    using AcademyPlatform.Models.Base;
    using AcademyPlatform.Models.Certificates;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Payments;

    public class User : SoftDeletableEntity
    {
        private ICollection<Course> _courses;
        private ICollection<Certificate> _certificates;
        private ICollection<LectureVisit> _lectureVisits;

        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        public string ValidationCode { get; set; }

        public bool IsApproved { get; set; }

        public virtual BillingInfo BillingInfo { get; set; }

        //public virtual Profile Profile { get; set; }

        public virtual ICollection<Course> Courses
        {
            get { return _courses; }
            set { _courses = value; }
        }
        public virtual ICollection<Certificate> Certificates
        {
            get { return _certificates; }
            set { _certificates = value; }
        }

        public virtual ICollection<LectureVisit> LectureVisits
        {
            get { return _lectureVisits; }
            set { _lectureVisits = value; }
        }

        public User()
        {
            _courses = new HashSet<Course>();
            _certificates = new HashSet<Certificate>();
            _lectureVisits = new HashSet<LectureVisit>();
        }
    }
}