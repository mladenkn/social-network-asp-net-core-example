using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Interface.Models.Entities;

namespace SocialNetwork.DAL
{
    public class SocialNetworkDbContext : IdentityDbContext<User>
    {
        public SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> builder) : base(builder)
        {
        }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<_Rating>()
                .Ignore(it => it.RatingType)
                .HasKey(it => new {it.PostId, it.UserId});
        }
    }
}
