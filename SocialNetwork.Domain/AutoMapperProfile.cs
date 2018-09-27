using AutoMapper;
using SocialNetwork.Domain.PostRatings;
using SocialNetwork.Domain.Posts;
using SocialNetwork.Domain.Users;
using SocialNetwork.Domain.UseCases;

namespace SocialNetwork.Domain
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostDtoLarge>()
                .ForMember(p => p.Actions, m => m.Ignore());
            CreateMap<User, UserDtoBasic>();
            CreateMap<PostRating, PostRatingDtoBasic>();
        }
    }
}
