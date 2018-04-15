using System;
using System.Threading.Tasks;
using SocialNetwork.Interface.DAL;
using SocialNetwork.Interface.Models.Entities;

namespace SocialNetwork.Web.Utilities
{
    public static class RepositoryExtensions
    {
        public static Task<TEtnity> GetOne<TEtnity, TId>(this IRepository<TEtnity> repository, TId id,
            params string[] propsToInclude)
            where TEtnity : IEntity<TId> =>
            repository.GetOne(it => it.Id.Equals(id), propsToInclude);

        public static Task<TEtnity> Update<TEtnity, TId>(this IRepository<TEtnity> repository, TId id,
                    Action<TEtnity> action, params string[] propsToInclude)
            where TEtnity : IEntity<TId> =>
            repository.Update(it => it.Id.Equals(id), action, propsToInclude);

        public static Task Delete<TEtnity, TId>(this IRepository<TEtnity> repository, TId id)
            where TEtnity : IEntity<TId> =>
            repository.Delete(it => it.Id.Equals(id));

        public static Task<bool> Contains<TEtnity, TId>(this IRepository<TEtnity> repository, TId id)
            where TEtnity : IEntity<TId> =>
            repository.Contains(it => it.Id.Equals(id));
    }
}
