using ApplicationKernel.Domain.DataQueries;
using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationKernel.Domain;
using SocialNetwork.Domain.Posts;

namespace SocialNetwork.Domain.UseCases
{
    public static class GetPosts
    {
        public struct Request : IRequest
        {
            public string AuthorId { get; set; }
            public Paging Paging { get; set; }
            public IReadOnlyList<OrderCriteria<Post>> Order { get; set; }
        }

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(q => q.Order).NotNull();
                RuleFor(q => q.Paging.PageNumber).GreaterThan(0);
                RuleFor(q => q.Paging.PageSize).GreaterThan(0);
            }
        }

        public class Executor : IRequestHandler<Request>
        {
            private readonly IUseCaseExecutorTools _tools;
            private readonly GetUserActionsForPost _getUserActionsForPost;

            public Executor(IUseCaseExecutorTools tools, GetUserActionsForPost getUserActionsForPost)
            {
                _tools = tools;
                _getUserActionsForPost = getUserActionsForPost;
            }

            public async Task<Response> Handle(Request args, CancellationToken cancellationToken)
            {
                var query = _tools.Query<Post>();

                if (args.AuthorId != null)
                    query = query.Where(p => p.AuthorId == args.AuthorId);

                var posts = await query
                    .Include(p => p.Ratings)
                    .Include(p => p.Author)
                    .Order(args.Order)
                    .Paging(args.Paging)
                    .ToListAsync(cancellationToken);
                
                var postsCount = await query.CountAsync(cancellationToken);

                var userId = _tools.CurrentUserId();

                var postsWithActions = posts.Select(post =>
                {
                    var rating = post.Ratings.FirstOrDefault(r => r.UserId == userId);
                    var mappedPost = _tools.Mapper.Map<PostDtoLarge>(post);
                    mappedPost.Actions = _getUserActionsForPost(userId, post, rating);
                    return mappedPost;
                }).ToList();

                var pagedList = PagedList.Create(postsWithActions, args.Paging.PageNumber, postsCount);

                return Responses.Success(pagedList);
            }
        }
    }
}
