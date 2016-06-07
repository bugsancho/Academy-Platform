namespace AcademyPlatform.Models.Base
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SoftDeletableLoggedEntity : ISoftDeletableEntity, ILoggedEntity
    {
        [Display(Name = "Deleted?")]
        [Editable(false)]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deletion date")]
        [Editable(false)]
        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}