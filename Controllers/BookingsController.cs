using Microsoft.AspNetCore.Mvc;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Services;

namespace UndergroundGymAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _service;
    public BookingsController(IBookingService service) => _service = service;

    /// <summary>Get all bookings (admin)</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingResponse>>> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Get bookings for a specific user</summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<BookingResponse>>> GetByUser(int userId)
        => Ok(await _service.GetByUserAsync(userId));

    /// <summary>Create a new booking</summary>
    [HttpPost]
    public async Task<ActionResult<BookingResponse>> Create([FromBody] CreateBookingRequest request)
    {
        try
        {
            var result = await _service.CreateAsync(request);
            return Created($"/api/bookings/{result.Id}", result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>Update booking status</summary>
    [HttpPatch("{id}/status")]
    public async Task<ActionResult> UpdateStatus(int id, [FromBody] UpdateBookingStatusRequest request)
        => await _service.UpdateStatusAsync(id, request.Status) ? Ok() : NotFound();

    /// <summary>Cancel a booking</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Cancel(int id)
        => await _service.CancelAsync(id) ? NoContent() : NotFound();
}
