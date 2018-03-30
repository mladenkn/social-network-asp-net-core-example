using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.Utilities
{
    public static class RepositoryExtensions
    {
        public static Task<TEnity> GetOne<TEnity, TId>(this IRepository<TEnity> repository, TId id,
            params string[] propsToInclude)
            where TEnity : IEntity<TId> =>
            repository.GetOne(it => it.Id.Equals(id), propsToInclude);

        public static Task<TEnity> Update<TEnity, TId>(this IRepository<TEnity> repository, TId id,
                    Action<TEnity> action, params string[] propsToInclude)
            where TEnity : IEntity<TId> =>
            repository.Update(it => it.Id.Equals(id), action, propsToInclude);

        public static Task Delete<TEnity, TId>(this IRepository<TEnity> repository, TId id)
            where TEnity : IEntity<TId> =>
            repository.Delete(it => it.Id.Equals(id));
    }
}
