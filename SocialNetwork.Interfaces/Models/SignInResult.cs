namespace SocialNetwork.Interfaces.Models
{
    public class SignInResult
    {
        public SignInResult(bool hasSucceeded)
        {
            HasSucceeded = hasSucceeded;
        }

        public bool HasSucceeded { get; }
    }
}
