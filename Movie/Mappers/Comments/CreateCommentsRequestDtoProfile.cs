using AutoMapper;
using Movie.Dtos.Comments.Request;
using Movie.Entities;


namespace Movie.Mappers.Comments
{
    public class CreateCommentsRequestDtoProfile: Profile
    {
        public CreateCommentsRequestDtoProfile()
        {
            CreateMap<CreateCommentsRequestDto, Comment>()
                .ForMember(c => c.CreatedAt, opt => opt.Ignore())
                .ForMember(c => c.UpdatedAt, opt => opt.Ignore())
                .ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}