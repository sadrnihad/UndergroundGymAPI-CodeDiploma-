using Microsoft.AspNetCore.Mvc;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Services;

namespace UndergroundGymAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContactController : ControllerBase
{
    private readonly IContactService _service;
    public ContactController(IContactService service) => _service = service;

    /// <summary>Get all contact submissions (admin)</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactSubmissionResponse>>> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Submit a contact form</summary>
    [HttpPost]
    public async Task<ActionResult<ContactSubmissionResponse>> Submit([FromBody] CreateContactRequest request)
    {
        var result = await _service.SubmitAsync(request);
        return Created($"/api/contact/{result.Id}", result);
    }

    /// <summary>Mark a contact submission as read</summary>
    [HttpPatch("{id}/read")]
    public async Task<ActionResult> MarkRead(int id)
        => await _service.MarkReadAsync(id) ? Ok() : NotFound();
}
