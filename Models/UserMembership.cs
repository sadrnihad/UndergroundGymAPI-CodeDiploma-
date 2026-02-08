using System.ComponentModel.DataAnnotations;

namespace UndergroundGymAPI.Models;

public class UserMembership
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int PlanId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "pending"; // active, expired, cancelled, pending

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public MembershipPlan? Plan { get; set; }
}
