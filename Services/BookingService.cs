using Microsoft.EntityFrameworkCore;
using UndergroundGymAPI.Data;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Models;

namespace UndergroundGymAPI.Services;

public class BookingService : IBookingService
{
    private readonly GymDbContext _db;
    public BookingService(GymDbContext db) => _db = db;

    public async Task<IEnumerable<BookingResponse>> GetAllAsync()
    {
        var bookings = await _db.Bookings.OrderByDescending(b => b.CreatedAt).ToListAsync();
        return bookings.Select(MapToResponse);
    }

    public async Task<IEnumerable<BookingResponse>> GetByUserAsync(int userId)
    {
        var bookings = await _db.Bookings
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate).ToListAsync();
        return bookings.Select(MapToResponse);
    }

    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
    {
        var gymClass = await _db.GymClasses.FindAsync(request.ClassId)
            ?? throw new InvalidOperationException("Class not found");

        if (gymClass.CurrentEnrollment >= gymClass.MaxCapacity)
            throw new InvalidOperationException("Class is full");

        var entity = new Booking
        {
            UserId = request.UserId,
            ClassId = request.ClassId,
            BookingDate = request.BookingDate,
            Status = "confirmed",
        };
        _db.Bookings.Add(entity);
        gymClass.CurrentEnrollment++;
        await _db.SaveChangesAsync();
        return MapToResponse(entity);
    }

    public async Task<bool> UpdateStatusAsync(int id, string status)
    {
        var entity = await _db.Bookings.FindAsync(id);
        if (entity == null) return false;
        entity.Status = status;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelAsync(int id)
    {
        var entity = await _db.Bookings.FindAsync(id);
        if (entity == null) return false;
        entity.Status = "cancelled";
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static BookingResponse MapToResponse(Booking b) => new(
        b.Id, b.UserId, b.ClassId, b.Status, b.BookingDate, b.CreatedAt);
}
