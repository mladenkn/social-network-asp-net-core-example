using System.Linq;
using ApplicationKernel.Domain;
using ApplicationKernel.Domain.DataPersistance;
using ApplicationKernel.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SocialNetwork.Domain.Posts;
using SocialNetwork.Domain.Users;
using SocialNetwork.Domain.PostRatings;
using Utilities;

namespace SocialNetwork.Infrastructure
{
    public class SocialNetworkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostRating> PostRatings { get; set; }
        
        public SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> options)
            : base(options)
        {
        }

        public IDatabaseTransaction RunTransaction() => new EfDatabaseTransaction(this);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .MakePropertyRequired(it => it.Text)
                .MakePropertyRequired(it => it.AuthorId)
                .MakePropertyRequired(it => it.CreatedAt);

            modelBuilder.Entity<User>()
                .MakePropertyRequired(it => it.UserName)
                .MakePropertyRequired(it => it.ProfileImageUrl);
            
            modelBuilder.Entity<PostRating>()
                .MakePropertyRequired(it => it.PostId)
                .MakePropertyRequired(it => it.UserId)
                .MakePropertyRequired(it => it.Type);

            modelBuilder.DisableCascadeDeletions();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ConfigureWarnings(it => it.Throw(RelationalEventId.QueryClientEvaluationWarning))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseLoggerFactory(Logger).EnableSensitiveDataLogging();
        }

        private static LoggerFactory Logger
        {
            get
            {
                bool ConsoleLoggerFilter(string category, LogLevel level)
                {
                    return level.EqualsAny(LogLevel.Information, LogLevel.Warning);
                }

                var consoleLogger = new ConsoleLoggerProvider(ConsoleLoggerFilter, true);
                var loggerFactory = new LoggerFactory(new[] { consoleLogger });

                return loggerFactory;
            }
        }
    }
}
