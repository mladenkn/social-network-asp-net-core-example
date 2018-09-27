using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using SocialNetwork.Domain.Posts;

namespace SocialNetwork.Domain.UseCases
{
    public static class EditPost
    {
        public struct Request : IRequest
        {
            public long PostId { get; set; }
            public string NewText { get; set; }
        }

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(c => c.NewText).NotEmpty();
                RuleFor(c => c.PostId).GreaterThan(0);
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
                var post = await _tools.Query<Post>()
                    .Where(p => p.Id == command.PostId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (post == null)
                    return Responses.Failure("Post not found.");

                var userId = _tools.CurrentUserId();

                if (post.AuthorId == userId)
                {
                    post.Text = command.NewText;
                    await _tools.Transaction().Update(post).Commit();
                    return Responses.Success(post);
                }
                else
                    return Responses.Failure("User cant edit someone else's post");
            }
        }
    }
}
