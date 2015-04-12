namespace AcademyPlatform.Models.Base
{
    using System;

    public class LoggedEntity : ILoggedEntity
    {
        public DateTime CreatedOn { get; set; }
        
        public DateTime? ModifiedOn { get; set; }
    }
}