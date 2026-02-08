using Microsoft.AspNetCore.Mvc;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Services;

namespace UndergroundGymAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GalleryController : ControllerBase
{
    private readonly IGalleryService _service;
    public GalleryController(IGalleryService service) => _service = service;

    /// <summary>Get all gallery images</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GalleryImageResponse>>> GetAll([FromQuery] bool activeOnly = true)
        => Ok(await _service.GetAllAsync(activeOnly));

    /// <summary>Add a new gallery image</summary>
    [HttpPost]
    public async Task<ActionResult<GalleryImageResponse>> Create([FromBody] CreateGalleryImageRequest request)
    {
        var result = await _service.CreateAsync(request);
        return Created($"/api/gallery/{result.Id}", result);
    }

    /// <summary>Update a gallery image</summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<GalleryImageResponse>> Update(int id, [FromBody] UpdateGalleryImageRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>Soft-delete a gallery image</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}
