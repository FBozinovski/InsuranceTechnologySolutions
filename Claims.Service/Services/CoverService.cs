using AutoMapper;
using Claims.Domain.Contexts;
using Claims.Domain.Interfaces;
using Claims.Domain.Models;
using Claims.Dto.Enumerations;
using Claims.Dto.Requests;
using Claims.Dto.Responses;
using Claims.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Claims.Service.Services
{
    public class CoverService : ICoverService
    {

        private readonly ICoverRepository _coverRepository;
        private readonly ICoverAuditRepository _coverAuditRepository;
        private readonly IMapper _mapper;

        public CoverService(ICoverRepository coverRepository, ICoverAuditRepository coverAuditRepository, IMapper mapper)
        {
            _coverRepository = coverRepository;
            _coverAuditRepository = coverAuditRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CoverResponse>> GetAllAsync()
        {
            return await _coverRepository.GetAllCoverResponses();
        }

        public decimal ComputePremium(DateTime startDate, DateTime endDate, Enumerations.CoverType coverType)
        {
            var multiplier = 1.3m;
            if (coverType == Enumerations.CoverType.Yacht)
            {
                multiplier = 1.1m;
            }

            if (coverType == Enumerations.CoverType.PassengerShip)
            {
                multiplier = 1.2m;
            }

            if (coverType == Enumerations.CoverType.Tanker)
            {
                multiplier = 1.5m;
            }

            var premiumPerDay = 1250 * multiplier;
            var insuranceLength = (endDate - startDate).TotalDays;
            var totalPremium = 0m;

            for (var i = 0; i < insuranceLength; i++)
            {
                if (i < 30) totalPremium += premiumPerDay;
                if (i < 180 && coverType == Enumerations.CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
                else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
                if (i < 365 && coverType != Enumerations.CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
                else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
            }

            return totalPremium;
        }

        public async Task AuditCover(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            await _coverAuditRepository.AddAsync(coverAudit);
        }

        public async Task<CoverResponse> CreateAsync(CoverRequest request, string httpRequestType)
        {
            Validate(request);

            var cover = _mapper.Map<Cover>(request);
            cover.Id = Guid.NewGuid().ToString();
            cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
            await _coverRepository.AddAsync(cover);
            await AuditCover(cover.Id, httpRequestType);

            return _mapper.Map<CoverResponse>(cover);
        }

        private static void Validate(CoverRequest request)
        {
            if (request.StartDate.Date < DateTime.Now.Date)
            {
                throw new ValidationException("StartDate cannot be in the past.");
            }

            if (request.StartDate.AddYears(1) < request.EndDate)
            {
                throw new ValidationException("Total insurance period cannot exceed 1 year.");
            }
        }

        public async Task<CoverResponse> GetByIdAsync(string id)
        {
            return await _coverRepository.GetCoverResponseById(id);
        }

        public async Task DeleteByIdAsync(string id, string httpRequestType)
        {
            await _coverRepository.DeleteByIdAsync(id);
            await AuditCover(id, httpRequestType);
        }
    }
}
