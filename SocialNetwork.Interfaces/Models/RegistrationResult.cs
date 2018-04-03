namespace SocialNetwork.Interfaces.Models
{
    public class RegistrationResult
    {
        public RegistrationResult(bool hasSucceeded)
        {
            HasSucceeded = hasSucceeded;
        }

        public bool HasSucceeded { get; }
    }
}
