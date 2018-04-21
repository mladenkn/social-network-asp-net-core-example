using System.Collections.Generic;

namespace SocialNetwork.Interface.Models
{
    public abstract class Result
    {
    }

    public class Success : Result
    {
    }

    public class Failure<T> : Result
    {
        public Failure(params T[] errors) 
        {
            Errors = errors;
        }

        public IReadOnlyCollection<T> Errors { get; }
    }

    public enum RegistrationError
    {
        DuplicateUserName
    }
}
