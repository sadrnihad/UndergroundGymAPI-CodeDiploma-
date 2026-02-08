using System.ComponentModel.DataAnnotations;

namespace UndergroundGymAPI.Models;

public class Booking
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ClassId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "confirmed"; // confirmed, cancelled, waitlisted, completed

    public DateTime BookingDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public GymClass? GymClass { get; set; }
}
