namespace AcademyPlatform.Services.Contracts
{
    public interface IApplicationSettings
    {
        int AssessmentLockoutTime { get; }

        string InquiryRecievedEmail { get; }
    }
}
