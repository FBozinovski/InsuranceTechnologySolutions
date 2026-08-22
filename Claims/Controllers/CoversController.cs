using Claims.Auditing;
using Claims.Domain.Contexts;
using Claims.Domain.Models;
using Claims.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult> ComputePremiumAsync(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        return Ok(_coverService.ComputePremium(startDate, endDate, coverType));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cover>>> GetAsync()
    {
        var results = await _coverService.GetAllAsync();
        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cover>> GetAsync(string id)
    {
        var cover = await _coverService.GetByIdAsync(id);
        return Ok(cover);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Cover request)
    {
        var cover = await _coverService.CreateAsync(request, Request.Method);
        return Ok(cover);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(string id)
    {
        await _coverService.DeleteByIdAsync(id, Request.Method);
    }
}
