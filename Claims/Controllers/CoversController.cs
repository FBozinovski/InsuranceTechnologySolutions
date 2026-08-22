using Claims.Dto.Enumerations;
using Claims.Dto.Requests;
using Claims.Dto.Responses;
using Claims.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ILogger<CoversController> _logger;
    private readonly ICoverService _coverService;

    public CoversController(ILogger<CoversController> logger, ICoverService coverService)
    {
        _logger = logger;
        _coverService = coverService;
    }

    [HttpPost("compute")]
    public async Task<ActionResult> ComputePremiumAsync(DateTime startDate, DateTime endDate, Enumerations.CoverType coverType)
    {
        try
        {
            var premium = _coverService.ComputePremium(startDate, endDate, coverType);
            return Ok(premium);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        IEnumerable<CoverResponse> response;
        try
        {
            response = await _coverService.GetAllAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(string id)
    {
        CoverResponse response;
        try
        {
            response = await _coverService.GetByIdAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return response is not null ? Ok(response) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(CoverRequest request)
    {
        CoverResponse response;
        try
        {
            response = await _coverService.CreateAsync(request, Request.Method);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        try
        {
            await _coverService.DeleteByIdAsync(id, Request.Method);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }
}
