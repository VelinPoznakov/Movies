using AutoMapper;
using Movie.Dtos.Comments.Response;
using Movie.Entities;

namespace Movie.Mappers.Comments
{
    public class CommentResponseDtoProfile: Profile
    {
        public CommentResponseDtoProfile()
        {
            CreateMap<Comment, CommentResponseDto>();
        }
    }
}