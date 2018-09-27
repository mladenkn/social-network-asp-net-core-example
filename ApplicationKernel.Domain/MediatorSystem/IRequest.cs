using MediatR;

namespace ApplicationKernel.Domain.MediatorSystem
{
    public interface IRequest : IRequest<Response>
    {
    }
}
