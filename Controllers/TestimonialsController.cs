using Microsoft.AspNetCore.Mvc;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Services;

namespace UndergroundGymAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TestimonialsController : ControllerBase
{
    private readonly ITestimonialService _service;
    public TestimonialsController(ITestimonialService service) => _service = service;

    /// <summary>Get all testimonials</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestimonialResponse>>> GetAll([FromQuery] bool activeOnly = true)
        => Ok(await _service.GetAllAsync(activeOnly));

    /// <summary>Create a new testimonial</summary>
    [HttpPost]
    public async Task<ActionResult<TestimonialResponse>> Create([FromBody] CreateTestimonialRequest request)
    {
        var result = await _service.CreateAsync(request);
        return Created($"/api/testimonials/{result.Id}", result);
    }

    /// <summary>Update a testimonial</summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TestimonialResponse>> Update(int id, [FromBody] UpdateTestimonialRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>Soft-delete a testimonial</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}
