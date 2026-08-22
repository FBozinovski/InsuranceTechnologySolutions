using AutoMapper;
using Claims.Domain.Models;
using Claims.Dto.Requests;
using Claims.Dto.Responses;

namespace Claims.MapperProfile
{
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            CreateMap<ClaimRequest, Claim>();
            CreateMap<Claim, ClaimResponse>();
        }
    }
}
