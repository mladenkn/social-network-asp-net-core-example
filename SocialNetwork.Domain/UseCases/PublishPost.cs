using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using SocialNetwork.Domain.Posts;

namespace SocialNetwork.Domain.UseCases
{
    public static class PublishPost
    {
        public struct Request : IRequest
        {
            public string Text { get; set; }
        }

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(c => c.Text).NotEmpty();
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
                var userId = _tools.CurrentUserId();
                var post = new Post
                {
                    AuthorId = userId,
                    CreatedAt = DateTime.Now,
                    Text = command.Text
                };
                await _tools.Transaction().Save(post).Commit();
                return Responses.Success(post);
            }
        }
    }
}
