using System.ComponentModel.DataAnnotations;

namespace UndergroundGymAPI.Models;

public class GymClass
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [MaxLength(100)]
    public string Type { get; set; } = string.Empty;

    public int? TrainerId { get; set; }

    [Range(0, 6)]
    public int DayOfWeek { get; set; }

    [Required]
    [MaxLength(5)]
    public string StartTime { get; set; } = string.Empty;

    [Required]
    [MaxLength(5)]
    public string EndTime { get; set; } = string.Empty;

    public int MaxCapacity { get; set; } = 20;

    public int CurrentEnrollment { get; set; } = 0;

    [MaxLength(200)]
    public string Location { get; set; } = "Main Floor";

    public bool IsActive { get; set; } = true;

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Trainer? Trainer { get; set; }
}
