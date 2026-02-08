using Microsoft.AspNetCore.Mvc;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Services;

namespace UndergroundGymAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MembershipsController : ControllerBase
{
    private readonly IMembershipService _service;
    public MembershipsController(IMembershipService service) => _service = service;

    /// <summary>Get all membership plans</summary>
    [HttpGet("plans")]
    public async Task<ActionResult<IEnumerable<MembershipPlanResponse>>> GetPlans([FromQuery] bool activeOnly = true)
        => Ok(await _service.GetPlansAsync(activeOnly));

    /// <summary>Create a new membership plan</summary>
    [HttpPost("plans")]
    public async Task<ActionResult<MembershipPlanResponse>> CreatePlan([FromBody] CreateMembershipPlanRequest request)
    {
        var result = await _service.CreatePlanAsync(request);
        return Created($"/api/memberships/plans/{result.Id}", result);
    }

    /// <summary>Update a membership plan</summary>
    [HttpPut("plans/{id}")]
    public async Task<ActionResult<MembershipPlanResponse>> UpdatePlan(int id, [FromBody] UpdateMembershipPlanRequest request)
    {
        var result = await _service.UpdatePlanAsync(id, request);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>Soft-delete a membership plan</summary>
    [HttpDelete("plans/{id}")]
    public async Task<ActionResult> DeletePlan(int id)
        => await _service.DeletePlanAsync(id) ? NoContent() : NotFound();

    /// <summary>Subscribe a user to a plan</summary>
    [HttpPost("subscribe")]
    public async Task<ActionResult<UserMembershipResponse>> Subscribe([FromBody] SubscribeRequest request, [FromQuery] int userId = 1)
    {
        try
        {
            var result = await _service.SubscribeAsync(request, userId);
            return Created($"/api/memberships/subscriptions/{result.Id}", result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>Get all subscriptions (admin)</summary>
    [HttpGet("subscriptions")]
    public async Task<ActionResult<IEnumerable<UserMembershipResponse>>> GetAllSubscriptions()
        => Ok(await _service.GetAllSubscriptionsAsync());

    /// <summary>Get subscriptions for a specific user</summary>
    [HttpGet("subscriptions/user/{userId}")]
    public async Task<ActionResult<IEnumerable<UserMembershipResponse>>> GetUserSubscriptions(int userId)
        => Ok(await _service.GetUserMembershipsAsync(userId));

    /// <summary>Update subscription status</summary>
    [HttpPatch("subscriptions/{id}/status")]
    public async Task<ActionResult> UpdateSubscriptionStatus(int id, [FromBody] UpdateBookingStatusRequest request)
        => await _service.UpdateSubscriptionStatusAsync(id, request.Status) ? Ok() : NotFound();
}
