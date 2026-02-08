namespace UndergroundGymAPI.DTOs;

public record TrainerResponse(
    int Id, string Name, string? Title, string? Bio, string? Specialties,
    string? ImageUrl, string? Email, string? Phone, string? Instagram,
    bool IsActive, int SortOrder, DateTime CreatedAt);

public record CreateTrainerRequest(
    string Name, string? Title = null, string? Bio = null, string? Specialties = null,
    string? ImageUrl = null, string? Email = null, string? Phone = null,
    string? Instagram = null, int SortOrder = 0);

public record UpdateTrainerRequest(
    string? Name = null, string? Title = null, string? Bio = null,
    string? Specialties = null, string? ImageUrl = null, string? Email = null,
    string? Phone = null, string? Instagram = null, bool? IsActive = null,
    int? SortOrder = null);
