namespace AcademyPlatform.Services
{
    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models.Emails;
    using AcademyPlatform.Services.Contracts;

    public class InquiryService : IInquiryService
    {
        private readonly IRepository<Inquiry> _inquiries;

        public InquiryService(IRepository<Inquiry> inquiries)
        {
            _inquiries = inquiries;
        }

        public void Create(Inquiry inquiry)
        {
            _inquiries.Add(inquiry);
            _inquiries.SaveChanges();
        }
    }
}
