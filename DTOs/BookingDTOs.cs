namespace UndergroundGymAPI.DTOs;

public record BookingResponse(
    int Id, int UserId, int ClassId, string Status,
    DateTime BookingDate, DateTime CreatedAt);

public record CreateBookingRequest(int ClassId, int UserId, DateTime BookingDate);

public record UpdateBookingStatusRequest(string Status);
