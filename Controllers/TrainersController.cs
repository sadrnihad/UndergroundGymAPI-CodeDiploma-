using Microsoft.AspNetCore.Mvc;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Services;

namespace UndergroundGymAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TrainersController : ControllerBase
{
    private readonly ITrainerService _service;
    public TrainersController(ITrainerService service) => _service = service;

    /// <summary>Get all trainers</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainerResponse>>> GetAll([FromQuery] bool activeOnly = true)
        => Ok(await _service.GetAllAsync(activeOnly));

    /// <summary>Get a trainer by ID</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TrainerResponse>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new trainer</summary>
    [HttpPost]
    public async Task<ActionResult<TrainerResponse>> Create([FromBody] CreateTrainerRequest request)
    {
        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update a trainer</summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TrainerResponse>> Update(int id, [FromBody] UpdateTrainerRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>Soft-delete a trainer</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}
