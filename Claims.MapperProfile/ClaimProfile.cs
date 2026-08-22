using AutoMapper;
using Claims.Domain.Models;
using Claims.Dto.Requests;

namespace Claims.MapperProfile
{
    public class ClaimProfile : Profile
    {
        ClaimProfile()
        {
            CreateMap<ClaimRequest, Claim>();
        }
    }
}
