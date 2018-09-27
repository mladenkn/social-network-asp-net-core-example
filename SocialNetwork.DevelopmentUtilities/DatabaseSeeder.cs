using System;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Domain.Posts;
using SocialNetwork.Domain.Users;
using SocialNetwork.Infrastructure;
using Utilities;

namespace SocialNetwork.DevelopmentUtilities
{
    public class DatabaseSeeder
    {
        public async Task Seed(IServiceProvider services)
        {
            var db = services.GetService<SocialNetworkDbContext>();
            var settings = services.GetService<Settings>();
            
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            new Faker<User>()
                .RuleFor(u => u.ProfileImageUrl, f => f.Internet.UserName())
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .GenerateBetween(10, 15)
                .SaveTo(out var users);

            new Faker<User>()
                .RuleFor(u => u.ProfileImageUrl, f => f.Internet.UserName())
                .RuleFor(u => u.UserName, f => settings.CurrentUserUserName)
                .Generate()
                .SaveTo(out var currentUser);

            users.Add(currentUser);
            
            db.AddRange(users);
            await db.SaveChangesAsync();

            new Faker<Post>()
                .RuleFor(p => p.AuthorId, f => f.PickRandom(users).Id)
                .RuleFor(p => p.CreatedAt, f => f.Date.Recent())
                .RuleFor(p => p.Text, f => f.Lorem.Text())
                .GenerateBetween(100, 150)
                .SaveTo(out var posts);

            db.AddRange(posts);
            await db.SaveChangesAsync();
        }
    }
}
