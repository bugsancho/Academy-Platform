namespace AcademyPlatform.Models.Emails
{
    using AcademyPlatform.Models.Base;

    public class Inquiry : SoftDeletableEntity
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }
        
        public string Email { get; set; }
        
        public string Subject { get; set; }
        
        public string Message { get; set; }
    }
}
