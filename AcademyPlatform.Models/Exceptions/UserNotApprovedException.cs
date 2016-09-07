namespace AcademyPlatform.Models.Exceptions
{
    using System;

    public class UserNotApprovedException : ApplicationException
    {
        public UserNotApprovedException(string username): base($"User: -{username}- is not approved.")
        {
            
        }
    }
}
