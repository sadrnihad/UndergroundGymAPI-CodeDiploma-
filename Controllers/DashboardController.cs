using Microsoft.AspNetCore.Mvc;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Services;

namespace UndergroundGymAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly IContactService _contactService;
    public DashboardController(IContactService contactService) => _contactService = contactService;

    /// <summary>Get dashboard statistics</summary>
    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStatsResponse>> GetStats()
        => Ok(await _contactService.GetStatsAsync());
}
