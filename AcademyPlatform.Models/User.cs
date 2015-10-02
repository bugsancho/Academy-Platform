namespace AcademyPlatform.Models
{
    using System;
    using System.Collections.Generic;

    using AcademyPlatform.Models.Courses;

    public class User //: IdentityUser
    {
        private ICollection<Course> _courses;

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom use\r claims here
        //    return userIdentity;
        //}

        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Course> Courses
        {
            get { return _courses; }
            set { _courses = value; }
        }

        public User()
        {
            _courses = new HashSet<Course>();
        }
    }
}