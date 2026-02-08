namespace UndergroundGymAPI.DTOs;

public record ClassResponse(
    int Id, string Name, string? Description, string Type, int? TrainerId,
    int DayOfWeek, string StartTime, string EndTime, int MaxCapacity,
    int CurrentEnrollment, string Location, bool IsActive, string? ImageUrl,
    DateTime CreatedAt, DateTime UpdatedAt);

public record CreateClassRequest(
    string Name, string? Description, string Type, int? TrainerId,
    int DayOfWeek, string StartTime, string EndTime,
    int MaxCapacity = 20, string Location = "Main Floor", string? ImageUrl = null);

public record UpdateClassRequest(
    string? Name = null, string? Description = null, string? Type = null,
    int? TrainerId = null, int? DayOfWeek = null, string? StartTime = null,
    string? EndTime = null, int? MaxCapacity = null, string? Location = null,
    bool? IsActive = null, string? ImageUrl = null);
