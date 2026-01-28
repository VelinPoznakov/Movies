using AutoMapper;
using Movie.Entities;
using Movie.Mappers.Directors;

namespace Movie.Mappers.Comments
{
public class UpdateCommentRequestDtoProfile: Profile
{
    public UpdateCommentRequestDtoProfile()
        {
            CreateMap<UpdateDirectorRequestProfile, Comment>()
                .ForMember(c => c.CreatedAt, opt => opt.Ignore())
                .ForMember(c => c.UpdatedAt, opt => opt.Ignore())
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.MovieId, opt => opt.Ignore())
                .ForMember(c => c.UserId, opt => opt.Ignore());
        }
    }
}