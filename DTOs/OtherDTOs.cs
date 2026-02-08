namespace UndergroundGymAPI.DTOs;

// Gallery
public record GalleryImageResponse(
    int Id, string? Title, string? Description, string ImageUrl,
    string Category, int SortOrder, bool IsActive, DateTime CreatedAt);

public record CreateGalleryImageRequest(
    string ImageUrl, string? Title = null, string? Description = null,
    string Category = "general", int SortOrder = 0);

public record UpdateGalleryImageRequest(
    string? Title = null, string? Description = null, string? ImageUrl = null,
    string? Category = null, bool? IsActive = null, int? SortOrder = null);

// Testimonial
public record TestimonialResponse(
    int Id, string MemberName, string? MemberTitle, string Content,
    int Rating, string? ImageUrl, int SortOrder, bool IsActive, DateTime CreatedAt);

public record CreateTestimonialRequest(
    string MemberName, string Content, string? MemberTitle = null,
    int Rating = 5, string? ImageUrl = null, int SortOrder = 0);

public record UpdateTestimonialRequest(
    string? MemberName = null, string? MemberTitle = null, string? Content = null,
    int? Rating = null, string? ImageUrl = null, bool? IsActive = null,
    int? SortOrder = null);

// Contact
public record ContactSubmissionResponse(
    int Id, string Name, string Email, string? Phone, string? Subject,
    string Message, bool IsRead, DateTime CreatedAt);

public record CreateContactRequest(
    string Name, string Email, string Message,
    string? Phone = null, string? Subject = null);

// Dashboard
public record DashboardStatsResponse(
    int TotalClasses, int TotalTrainers, int TotalBookings,
    int TotalMemberships, int TotalContacts, int UnreadContacts);
