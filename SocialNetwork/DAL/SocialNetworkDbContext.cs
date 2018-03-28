using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;

namespace SocialNetwork.DAL
{
    public class SocialNetworkDbContext : DbContext
    {
        public SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> builder) : base(builder)
        {
        }
        
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
