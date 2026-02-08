using UndergroundGymAPI.DTOs;

namespace UndergroundGymAPI.Services;

public interface IClassService
{
    Task<IEnumerable<ClassResponse>> GetAllAsync(bool activeOnly = true);
    Task<ClassResponse?> GetByIdAsync(int id);
    Task<ClassResponse> CreateAsync(CreateClassRequest request);
    Task<ClassResponse?> UpdateAsync(int id, UpdateClassRequest request);
    Task<bool> DeleteAsync(int id);
}

public interface IMembershipService
{
    Task<IEnumerable<MembershipPlanResponse>> GetPlansAsync(bool activeOnly = true);
    Task<MembershipPlanResponse> CreatePlanAsync(CreateMembershipPlanRequest request);
    Task<MembershipPlanResponse?> UpdatePlanAsync(int id, UpdateMembershipPlanRequest request);
    Task<bool> DeletePlanAsync(int id);
    Task<UserMembershipResponse> SubscribeAsync(SubscribeRequest request, int userId);
    Task<IEnumerable<UserMembershipResponse>> GetUserMembershipsAsync(int userId);
    Task<IEnumerable<UserMembershipResponse>> GetAllSubscriptionsAsync();
    Task<bool> UpdateSubscriptionStatusAsync(int id, string status);
}

public interface ITrainerService
{
    Task<IEnumerable<TrainerResponse>> GetAllAsync(bool activeOnly = true);
    Task<TrainerResponse?> GetByIdAsync(int id);
    Task<TrainerResponse> CreateAsync(CreateTrainerRequest request);
    Task<TrainerResponse?> UpdateAsync(int id, UpdateTrainerRequest request);
    Task<bool> DeleteAsync(int id);
}

public interface IBookingService
{
    Task<IEnumerable<BookingResponse>> GetAllAsync();
    Task<IEnumerable<BookingResponse>> GetByUserAsync(int userId);
    Task<BookingResponse> CreateAsync(CreateBookingRequest request);
    Task<bool> UpdateStatusAsync(int id, string status);
    Task<bool> CancelAsync(int id);
}

public interface IGalleryService
{
    Task<IEnumerable<GalleryImageResponse>> GetAllAsync(bool activeOnly = true);
    Task<GalleryImageResponse> CreateAsync(CreateGalleryImageRequest request);
    Task<GalleryImageResponse?> UpdateAsync(int id, UpdateGalleryImageRequest request);
    Task<bool> DeleteAsync(int id);
}

public interface ITestimonialService
{
    Task<IEnumerable<TestimonialResponse>> GetAllAsync(bool activeOnly = true);
    Task<TestimonialResponse> CreateAsync(CreateTestimonialRequest request);
    Task<TestimonialResponse?> UpdateAsync(int id, UpdateTestimonialRequest request);
    Task<bool> DeleteAsync(int id);
}

public interface IContactService
{
    Task<IEnumerable<ContactSubmissionResponse>> GetAllAsync();
    Task<ContactSubmissionResponse> SubmitAsync(CreateContactRequest request);
    Task<bool> MarkReadAsync(int id);
    Task<DashboardStatsResponse> GetStatsAsync();
}
