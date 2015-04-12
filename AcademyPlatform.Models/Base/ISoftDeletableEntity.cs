namespace AcademyPlatform.Models.Base
{
    using System;

    public interface ISoftDeletableEntity
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}