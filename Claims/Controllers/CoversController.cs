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
    private readonly ICoverService _coverService;

    public CoversController(ICoverService coverService)
    {
        _coverService = coverService;
    }

    [HttpPost("compute")]
    public ActionResult ComputePremiumAsync(DateTime startDate, DateTime endDate, Enumerations.CoverType coverType)
    {
        var premium = _coverService.ComputePremium(startDate, endDate, coverType);
        return Ok(premium);
    }

    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var response = await _coverService.GetAllAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(string id)
    {
        var response = await _coverService.GetByIdAsync(id);
        return response is not null ? Ok(response) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(CoverRequest request)
    {
        var response = await _coverService.CreateAsync(request, Request.Method);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await _coverService.DeleteByIdAsync(id, Request.Method);
        return NoContent();
    }
}
