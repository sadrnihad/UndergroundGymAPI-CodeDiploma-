using System.ComponentModel.DataAnnotations;

namespace UndergroundGymAPI.Models;

public class GalleryImage
{
    public int Id { get; set; }

    [MaxLength(200)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = "general";

    public int SortOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
