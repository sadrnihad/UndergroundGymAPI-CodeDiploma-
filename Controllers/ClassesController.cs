using Microsoft.AspNetCore.Mvc;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Services;

namespace UndergroundGymAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClassesController : ControllerBase
{
    private readonly IClassService _service;
    public ClassesController(IClassService service) => _service = service;

    /// <summary>Get all active classes</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassResponse>>> GetAll([FromQuery] bool activeOnly = true)
        => Ok(await _service.GetAllAsync(activeOnly));

    /// <summary>Get a class by ID</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClassResponse>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new class</summary>
    [HttpPost]
    public async Task<ActionResult<ClassResponse>> Create([FromBody] CreateClassRequest request)
    {
        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update an existing class</summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ClassResponse>> Update(int id, [FromBody] UpdateClassRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>Soft-delete a class</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}
