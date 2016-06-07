namespace AcademyPlatform.Models.Payments
{
    public enum OnlineTransactionResultType
    {
        None,
        Ok,
        Failed,
        Created,
        Pending,
        Declined,
        Reversed,
        AutoReversed,
        Timeout
    }
}
