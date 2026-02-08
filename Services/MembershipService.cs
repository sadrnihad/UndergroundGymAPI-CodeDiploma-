using Microsoft.EntityFrameworkCore;
using UndergroundGymAPI.Data;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Models;

namespace UndergroundGymAPI.Services;

public class MembershipService : IMembershipService
{
    private readonly GymDbContext _db;
    public MembershipService(GymDbContext db) => _db = db;

    public async Task<IEnumerable<MembershipPlanResponse>> GetPlansAsync(bool activeOnly = true)
    {
        var query = _db.MembershipPlans.AsQueryable();
        if (activeOnly) query = query.Where(p => p.IsActive);
        var plans = await query.OrderBy(p => p.SortOrder).ToListAsync();
        return plans.Select(MapPlanResponse);
    }

    public async Task<MembershipPlanResponse> CreatePlanAsync(CreateMembershipPlanRequest request)
    {
        var entity = new MembershipPlan
        {
            Name = request.Name, Description = request.Description,
            Price = request.Price, Duration = request.Duration,
            Features = request.Features, IsPopular = request.IsPopular,
            SortOrder = request.SortOrder,
        };
        _db.MembershipPlans.Add(entity);
        await _db.SaveChangesAsync();
        return MapPlanResponse(entity);
    }

    public async Task<MembershipPlanResponse?> UpdatePlanAsync(int id, UpdateMembershipPlanRequest request)
    {
        var entity = await _db.MembershipPlans.FindAsync(id);
        if (entity == null) return null;
        if (request.Name != null) entity.Name = request.Name;
        if (request.Description != null) entity.Description = request.Description;
        if (request.Price.HasValue) entity.Price = request.Price.Value;
        if (request.Duration != null) entity.Duration = request.Duration;
        if (request.Features != null) entity.Features = request.Features;
        if (request.IsPopular.HasValue) entity.IsPopular = request.IsPopular.Value;
        if (request.IsActive.HasValue) entity.IsActive = request.IsActive.Value;
        if (request.SortOrder.HasValue) entity.SortOrder = request.SortOrder.Value;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return MapPlanResponse(entity);
    }

    public async Task<bool> DeletePlanAsync(int id)
    {
        var entity = await _db.MembershipPlans.FindAsync(id);
        if (entity == null) return false;
        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<UserMembershipResponse> SubscribeAsync(SubscribeRequest request, int userId)
    {
        var plan = await _db.MembershipPlans.FindAsync(request.PlanId)
            ?? throw new InvalidOperationException("Plan not found");

        var startDate = DateTime.UtcNow;
        var endDate = plan.Duration switch
        {
            "quarterly" => startDate.AddMonths(3),
            "yearly" => startDate.AddYears(1),
            _ => startDate.AddMonths(1),
        };

        var entity = new UserMembership
        {
            UserId = userId, PlanId = request.PlanId,
            Status = "pending", StartDate = startDate, EndDate = endDate,
        };
        _db.UserMemberships.Add(entity);
        await _db.SaveChangesAsync();
        return MapMembershipResponse(entity);
    }

    public async Task<IEnumerable<UserMembershipResponse>> GetUserMembershipsAsync(int userId)
    {
        var list = await _db.UserMemberships
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.CreatedAt).ToListAsync();
        return list.Select(MapMembershipResponse);
    }

    public async Task<IEnumerable<UserMembershipResponse>> GetAllSubscriptionsAsync()
    {
        var list = await _db.UserMemberships.OrderByDescending(m => m.CreatedAt).ToListAsync();
        return list.Select(MapMembershipResponse);
    }

    public async Task<bool> UpdateSubscriptionStatusAsync(int id, string status)
    {
        var entity = await _db.UserMemberships.FindAsync(id);
        if (entity == null) return false;
        entity.Status = status;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static MembershipPlanResponse MapPlanResponse(MembershipPlan p) => new(
        p.Id, p.Name, p.Description, p.Price, p.Duration,
        p.Features, p.IsPopular, p.IsActive, p.SortOrder, p.CreatedAt, p.UpdatedAt);

    private static UserMembershipResponse MapMembershipResponse(UserMembership m) => new(
        m.Id, m.UserId, m.PlanId, m.Status, m.StartDate, m.EndDate, m.CreatedAt);
}
