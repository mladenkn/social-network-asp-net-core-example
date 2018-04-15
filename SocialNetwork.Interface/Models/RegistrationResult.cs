using System.Collections.Generic;

namespace SocialNetwork.Interface.Models
{
    public abstract class RegistrationResult
    {
    }

    public class RegistrationSuccess : RegistrationResult
    {
    }

    public class RegistrationFailure : RegistrationResult
    {
        public RegistrationFailure(params RegistrationError[] errors) 
        {
            Errors = errors;
        }

        public IReadOnlyCollection<RegistrationError> Errors { get; }
    }

    public enum RegistrationError
    {
        DuplicateUserName
    }
}
