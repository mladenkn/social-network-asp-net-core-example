using AutoMapper;

namespace SocialNetwork.Domain
{
    public interface IUseCaseExecutorTools : ApplicationKernel.Domain.IUseCaseExecutorTools
    {
        string CurrentUserId();
        IMapper Mapper { get; }
    }
}
