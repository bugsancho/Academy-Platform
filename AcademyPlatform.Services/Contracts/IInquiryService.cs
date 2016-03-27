namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models.Emails;

    public interface IInquiryService
    {
        void Create(Inquiry inquiry);
    }
}