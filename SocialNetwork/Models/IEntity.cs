namespace SocialNetwork.Models
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
