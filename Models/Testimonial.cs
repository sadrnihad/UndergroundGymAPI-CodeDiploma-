using System.ComponentModel.DataAnnotations;

namespace UndergroundGymAPI.Models;

public class Testimonial
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string MemberName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? MemberTitle { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; } = 5;

    public string? ImageUrl { get; set; }

    public int SortOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
