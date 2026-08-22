
using AutoMapper;
using Azure;
using Claims.Domain.Contexts;
using Claims.Domain.Interfaces;
using Claims.Domain.Models;
using Claims.Dto.Requests;
using Claims.Dto.Responses;
using Claims.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Claims.Service.Services
{
    public class ClaimService : IClaimService
    {
        private const decimal MaxDamageCost = 100_000m;

        private readonly IClaimRepository _claimRepository;
        private readonly IClaimAuditRepository _claimAuditRepository;
        private readonly ICoverRepository _coverRepository;
        private readonly IMapper _mapper;

        public ClaimService(IClaimRepository claimRepository, IClaimAuditRepository claimAuditRepository,
            ICoverRepository coverRepository, IMapper mapper, ILogger<ClaimService> logger)
        {
            _claimRepository = claimRepository;
            _claimAuditRepository = claimAuditRepository;
            _coverRepository = coverRepository;
            _mapper = mapper;
        }

        public async Task AuditClaim(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            await _claimAuditRepository.AddAsync(claimAudit);
        }

        public async Task<ClaimResponse> CreateAsync(ClaimRequest request, string httpRequestType)
        {
            await ValidateAsync(request);

            var claim = _mapper.Map<Claim>(request);
            claim.Id = Guid.NewGuid().ToString();
            await _claimRepository.AddAsync(claim);
            await AuditClaim(claim.Id, httpRequestType);

            return _mapper.Map<ClaimResponse>(claim);
        }

        private async Task ValidateAsync(ClaimRequest request)
        {
            if (request.DamageCost > MaxDamageCost)
            {
                throw new ValidationException($"DamageCost cannot exceed {MaxDamageCost:N0}.");
            }

            var cover = await _coverRepository.GetByIdAsync(request.CoverId);
            if (cover is null)
            {
                throw new ValidationException($"Cover with id '{request.CoverId}' was not found.");
            }

            if (request.Created.Date < cover.StartDate.Date || request.Created.Date > cover.EndDate.Date)
            {
                throw new ValidationException("Created date must be within the period of the related Cover.");
            }
        }

        public async Task<IEnumerable<ClaimResponse>> GetAllAsync()
        {
            return await _claimRepository.GetAllClaimResponses();
        }

        public async Task DeleteByIdAsync(string id, string httpRequestType)
        {
            await _claimRepository.DeleteByIdAsync(id);
            await AuditClaim(id, httpRequestType);
        }

        public async Task<ClaimResponse> GetByIdAsync(string id)
        {
            return await _claimRepository.GetClaimResponseById(id);
        }
    }
}