using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;

namespace SocialNetwork.DAL
{
    public class SocialNetworkDbContext : IdentityDbContext<User>
    {
        public SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> builder) : base(builder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PostRating>()
                .HasKey(it => new {it.PostId, it.UserId});
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostRating> PostRatings { get; set; }
    }
}
