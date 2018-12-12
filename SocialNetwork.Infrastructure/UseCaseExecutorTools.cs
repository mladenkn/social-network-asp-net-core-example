using ApplicationKernel.Domain.DataQueries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain;
using SocialNetwork.Domain.Users;

namespace SocialNetwork.Infrastructure
{
    public class UseCaseExecutorTools : ApplicationKernel.Infrastructure.UseCaseExecutorTools, IUseCaseExecutorTools
    {
        private readonly GetCurrentUserId _getCurrentUserId;

        public UseCaseExecutorTools(
            DbContext db,
            GetCurrentUserId getCurrentUserId, 
            IMapper mapper,
            IQuery query) 
            : base(db, query)
        {
            Mapper = mapper;
            _getCurrentUserId = getCurrentUserId;
        }

        public IMapper Mapper { get; }

        public string CurrentUserId() => _getCurrentUserId();
    }
}
