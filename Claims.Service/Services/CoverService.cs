using AutoMapper;
using Claims.Domain.Contexts;
using Claims.Domain.Interfaces;
using Claims.Domain.Models;
using Claims.Dto.Enumerations;
using Claims.Dto.Requests;
using Claims.Dto.Responses;
using Claims.Service.BackgroundProcessing;
using Claims.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace Claims.Service.Services
{
    public class CoverService : ICoverService
    {

        private readonly ICoverRepository _coverRepository;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IMapper _mapper;

        public CoverService(ICoverRepository coverRepository, IBackgroundTaskQueue taskQueue, IMapper mapper)
        {
            _coverRepository = coverRepository;
            _taskQueue = taskQueue;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CoverResponse>> GetAllAsync()
        {
            return await _coverRepository.GetAllCoverResponses();
        }

        private const decimal BaseDailyRate = 1250m;
        private const int FirstTierDays = 30;
        private const int SecondTierDays = 150;

        public decimal ComputePremium(DateTime startDate, DateTime endDate, Enumerations.CoverType coverType)
        {
            var dailyRate = BaseDailyRate * GetTypeMultiplier(coverType);
            var secondTierDiscount = GetSecondTierDiscount(coverType);
            var thirdTierDiscount = GetThirdTierDiscount(coverType);

            var totalDays = Math.Max((endDate.Date - startDate.Date).Days, 0);
            var firstTierDays = Math.Min(totalDays, FirstTierDays);
            var secondTierDays = Math.Min(Math.Max(totalDays - FirstTierDays, 0), SecondTierDays);
            var thirdTierDays = Math.Max(totalDays - FirstTierDays - SecondTierDays, 0);

            return firstTierDays * dailyRate
                 + secondTierDays * dailyRate * (1 - secondTierDiscount)
                 + thirdTierDays * dailyRate * (1 - thirdTierDiscount);
        }

        private static decimal GetTypeMultiplier(Enumerations.CoverType coverType)
        {
            if (coverType == Enumerations.CoverType.Yacht)
            {
                return 1.10m; 
            }

            if (coverType == Enumerations.CoverType.PassengerShip)
            {
                return 1.20m;
            }

            if (coverType == Enumerations.CoverType.Tanker)
            {
                return 1.50m;
            }

            return 1.30m;
        }

        private static decimal GetSecondTierDiscount(Enumerations.CoverType coverType)
        {
            if (coverType == Enumerations.CoverType.Yacht)
            {
                return 0.05m; // 5% discount
            }

            return 0.02m; // 2% discount
        }


        private static decimal GetThirdTierDiscount(Enumerations.CoverType coverType)
        {
            if (coverType == Enumerations.CoverType.Yacht)
            {
                return 0.08m; // second tier's 5% plus an additional 3%
            }

            return 0.03m; // second tier's 2% plus an additional 1%
        }

        private async Task AuditCover(string id, string httpRequestType)
        {
            await _taskQueue.WriteToQueueAsync(async (serviceProvider, cancellationToken) =>
            {
                var coverAuditRepository = serviceProvider.GetRequiredService<ICoverAuditRepository>();
                var coverAudit = new CoverAudit()
                {
                    Created = DateTime.Now,
                    HttpRequestType = httpRequestType,
                    CoverId = id
                };

                await coverAuditRepository.AddAsync(coverAudit);
            });
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
