using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utilities;
using SocialNetwork.Domain.PostRatings;
using SocialNetwork.Domain.Posts;

namespace SocialNetwork.Domain.UseCases
{
    public static class RatePost
    {
        public struct Request : IRequest
        {
            public long PostId { get; set; }
            public PostRatingType RatingType { get; set; }
        }

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(c => c.PostId).GreaterThan(0);
                RuleFor(c => c.RatingType).Must(input => EnumHelper.GetValues<PostRatingType>().Contains(input));
            }
        }

        public class Executor : IRequestHandler<Request>
        {
            private readonly IUseCaseExecutorTools _tools;

            public Executor(IUseCaseExecutorTools tools)
            {
                _tools = tools;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var userId = _tools.CurrentUserId();

                var postToRate = await _tools.Query<Post>()
                    .Where(p => p.Id == request.PostId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (postToRate == null)
                    return Responses.Failure("Post not found");

                if (postToRate.AuthorId == userId)
                    return Responses.Failure("User cant rate his own post");

                var rating = await _tools.Query<PostRating>()
                    .Where(r => r.PostId == request.PostId &&
                                r.UserId == userId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (rating == null)
                {
                    var newRating = new PostRating
                    {
                        PostId = request.PostId,
                        UserId = userId,
                        Type = request.RatingType
                    };
                    await _tools.Transaction().Save(newRating).Commit();
                    return Responses.Success(newRating);
                }
                else
                {
                    if (rating.Type == request.RatingType)
                        return Responses.Failure("Attempt to rate post by the same user with the same rating type");

                    rating.Type = request.RatingType;
                    await _tools.Transaction().Update(rating).Commit();

                    return Responses.Success(rating);
                }
            }
        }
    }
}
