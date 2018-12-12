using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.PostRatings;

namespace SocialNetwork.Domain.UseCases
{
    public static class UnratePost
    {
        public struct Request : IRequest
        {
            public long PostRatingId { get; set; }
        }

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(c => c.PostRatingId).GreaterThan(0);
            }
        }

        public class Executor : IRequestHandler<Request>
        {
            private readonly IUseCaseExecutorTools _tools;

            public Executor(IUseCaseExecutorTools tools)
            {
                _tools = tools;
            }

            public async Task<Response> Handle(Request command, CancellationToken cancellationToken)
            {
                var rating = await _tools.Query.Of<PostRating>()
                    .Where(r => r.Id == command.PostRatingId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (rating == null)
                    return Responses.Failure("PostRating not found.");

                if (rating.UserId != _tools.CurrentUserId())
                    return Responses.Failure("User cannot delete someone else's PostRating");

                _tools.UnitOfWork.Delete(rating);
                await _tools.UnitOfWork.PersistChanges();
                return Responses.Success();
            }
        }
    }
}
