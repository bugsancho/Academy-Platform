namespace AcademyPlatform.Web.Models.Other
{
    using System.ComponentModel.DataAnnotations;

    using AcademyPlatform.Models.Emails;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class InquiryViewModel : IMapFrom<Inquiry>
    {
        [Display(Name = "Име")]
        public string CustomerName { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Относно")]
        public string Subject { get; set; }

        [Display(Name = "Съобщение")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}
