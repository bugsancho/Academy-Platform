namespace AcademyPlatform.Models.Exceptions
{
    using System;

    public class UserNotFoundException : ApplicationException
    {
        public UserNotFoundException(string username) : base($"Could not find user with username or id {username}")
        {
            
        }
    }
}
