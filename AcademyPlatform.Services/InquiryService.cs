namespace AcademyPlatform.Services
{
    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models.Emails;
    using AcademyPlatform.Services.Contracts;

    public class InquiryService : IInquiryService
    {
        private readonly IRepository<Inquiry> _inquiries;
        private readonly IMessageService _messageService;


        public InquiryService(IRepository<Inquiry> inquiries, IMessageService messageService)
        {
            _inquiries = inquiries;
            _messageService = messageService;
        }

        public void Create(Inquiry inquiry)
        {
            _inquiries.Add(inquiry);
            _inquiries.SaveChanges();

            _messageService.SendInquiryRecievedMessage(inquiry);
        }
    }
}
