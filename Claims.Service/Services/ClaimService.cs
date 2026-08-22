
using AutoMapper;
using Azure;
using Claims.Domain.Contexts;
using Claims.Domain.Interfaces;
using Claims.Domain.Models;
using Claims.Dto.Requests;
using Claims.Dto.Responses;
using Claims.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Claims.Service.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IClaimAuditRepository _claimAuditRepository;
        private readonly IMapper _mapper;

        public ClaimService(IClaimRepository claimRepository, IClaimAuditRepository claimAuditRepository,
            IMapper mapper, ILogger<ClaimService> logger)
        {
            _claimRepository = claimRepository;
            _claimAuditRepository = claimAuditRepository;
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
            var claim = _mapper.Map<Claim>(request);
            claim.Id = Guid.NewGuid().ToString();
            await _claimRepository.AddAsync(claim);
            await AuditClaim(claim.Id, httpRequestType);

            return _mapper.Map<ClaimResponse>(claim);
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