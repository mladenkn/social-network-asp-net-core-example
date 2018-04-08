namespace SocialNetwork.Interface.Models.Entities
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
