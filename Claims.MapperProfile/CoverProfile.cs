using AutoMapper;
using Claims.Domain.Models;
using Claims.Dto.Requests;
using Claims.Dto.Responses;

namespace Claims.MapperProfile
{
    public class CoverProfile : Profile
    {
        public CoverProfile()
        {
            CreateMap<CoverRequest, Cover>();
            CreateMap<Cover, CoverResponse>();
        }
    }
}
