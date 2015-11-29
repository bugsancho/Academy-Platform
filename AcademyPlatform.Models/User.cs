namespace AcademyPlatform.Models
{
    using System;
    using System.Collections.Generic;

    using AcademyPlatform.Models.Courses;

    public class User //: IdentityUser
    {
        private ICollection<Course> _courses;
        private ICollection<LectureVisit> _lectureVisits;

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom use\r claims here
        //    return userIdentity;
        //}

        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string ValidationCode { get; set; }

        public bool IsApproved { get; set; }

        public virtual ICollection<Course> Courses
        {
            get { return _courses; }
            set { _courses = value; }
        }

        public virtual ICollection<LectureVisit> LectureVisits
        {
            get { return _lectureVisits; }
            set { _lectureVisits = value; }
        }

        public User()
        {
            _courses = new HashSet<Course>();
            _lectureVisits = new HashSet<LectureVisit>();
        }
    }
}