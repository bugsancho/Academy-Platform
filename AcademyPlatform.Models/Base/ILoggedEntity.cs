namespace AcademyPlatform.Models.Base
{
    using System;

    public interface ILoggedEntity
    {
        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}