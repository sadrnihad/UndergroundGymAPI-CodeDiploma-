using System.ComponentModel.DataAnnotations;

namespace UndergroundGymAPI.Models;

public class Trainer
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Title { get; set; }

    public string? Bio { get; set; }

    /// <summary>JSON array of specialties</summary>
    public string? Specialties { get; set; }

    public string? ImageUrl { get; set; }

    [MaxLength(320)]
    public string? Email { get; set; }

    [MaxLength(30)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Instagram { get; set; }

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<GymClass> Classes { get; set; } = new List<GymClass>();
}
