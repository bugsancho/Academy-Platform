namespace AcademyPlatform.Models.Base
{
    using System;

    public interface ISoftDeletableEntity : IEntity
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}