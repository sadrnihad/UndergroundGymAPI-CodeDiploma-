namespace UndergroundGymAPI.DTOs;

public record MembershipPlanResponse(
    int Id, string Name, string? Description, decimal Price, string Duration,
    string? Features, bool IsPopular, bool IsActive, int SortOrder,
    DateTime CreatedAt, DateTime UpdatedAt);

public record CreateMembershipPlanRequest(
    string Name, string? Description, decimal Price, string Duration = "monthly",
    string? Features = null, bool IsPopular = false, int SortOrder = 0);

public record UpdateMembershipPlanRequest(
    string? Name = null, string? Description = null, decimal? Price = null,
    string? Duration = null, string? Features = null, bool? IsPopular = null,
    bool? IsActive = null, int? SortOrder = null);

public record SubscribeRequest(int PlanId);

public record UserMembershipResponse(
    int Id, int UserId, int PlanId, string Status,
    DateTime? StartDate, DateTime? EndDate, DateTime CreatedAt);
