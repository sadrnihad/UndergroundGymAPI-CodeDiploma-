using Microsoft.EntityFrameworkCore;
using UndergroundGymAPI.Data;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Models;

namespace UndergroundGymAPI.Services;

public class ContactService : IContactService
{
    private readonly GymDbContext _db;
    public ContactService(GymDbContext db) => _db = db;

    public async Task<IEnumerable<ContactSubmissionResponse>> GetAllAsync()
    {
        var items = await _db.ContactSubmissions.OrderByDescending(c => c.CreatedAt).ToListAsync();
        return items.Select(MapToResponse);
    }

    public async Task<ContactSubmissionResponse> SubmitAsync(CreateContactRequest request)
    {
        var entity = new ContactSubmission
        {
            Name = request.Name, Email = request.Email,
            Phone = request.Phone, Subject = request.Subject,
            Message = request.Message,
        };
        _db.ContactSubmissions.Add(entity);
        await _db.SaveChangesAsync();
        return MapToResponse(entity);
    }

    public async Task<bool> MarkReadAsync(int id)
    {
        var entity = await _db.ContactSubmissions.FindAsync(id);
        if (entity == null) return false;
        entity.IsRead = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<DashboardStatsResponse> GetStatsAsync()
    {
        var totalClasses = await _db.GymClasses.CountAsync(c => c.IsActive);
        var totalTrainers = await _db.Trainers.CountAsync(t => t.IsActive);
        var totalBookings = await _db.Bookings.CountAsync();
        var totalMemberships = await _db.UserMemberships.CountAsync();
        var totalContacts = await _db.ContactSubmissions.CountAsync();
        var unreadContacts = await _db.ContactSubmissions.CountAsync(c => !c.IsRead);

        return new DashboardStatsResponse(
            totalClasses, totalTrainers, totalBookings,
            totalMemberships, totalContacts, unreadContacts);
    }

    private static ContactSubmissionResponse MapToResponse(ContactSubmission c) => new(
        c.Id, c.Name, c.Email, c.Phone, c.Subject,
        c.Message, c.IsRead, c.CreatedAt);
}
