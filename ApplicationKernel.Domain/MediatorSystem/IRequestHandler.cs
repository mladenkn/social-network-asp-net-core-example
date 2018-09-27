using MediatR;

namespace ApplicationKernel.Domain.MediatorSystem
{
    public interface IRequestHandler<in TRequst> 
        : IRequestHandler<TRequst, Response> 
        where TRequst : IRequest<Response>
    {
    }
}
