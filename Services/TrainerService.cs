using Microsoft.EntityFrameworkCore;
using UndergroundGymAPI.Data;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Models;

namespace UndergroundGymAPI.Services;

public class TrainerService : ITrainerService
{
    private readonly GymDbContext _db;
    public TrainerService(GymDbContext db) => _db = db;

    public async Task<IEnumerable<TrainerResponse>> GetAllAsync(bool activeOnly = true)
    {
        var query = _db.Trainers.AsQueryable();
        if (activeOnly) query = query.Where(t => t.IsActive);
        var trainers = await query.OrderBy(t => t.SortOrder).ToListAsync();
        return trainers.Select(MapToResponse);
    }

    public async Task<TrainerResponse?> GetByIdAsync(int id)
    {
        var t = await _db.Trainers.FindAsync(id);
        return t == null ? null : MapToResponse(t);
    }

    public async Task<TrainerResponse> CreateAsync(CreateTrainerRequest request)
    {
        var entity = new Trainer
        {
            Name = request.Name, Title = request.Title, Bio = request.Bio,
            Specialties = request.Specialties, ImageUrl = request.ImageUrl,
            Email = request.Email, Phone = request.Phone,
            Instagram = request.Instagram, SortOrder = request.SortOrder,
        };
        _db.Trainers.Add(entity);
        await _db.SaveChangesAsync();
        return MapToResponse(entity);
    }

    public async Task<TrainerResponse?> UpdateAsync(int id, UpdateTrainerRequest request)
    {
        var entity = await _db.Trainers.FindAsync(id);
        if (entity == null) return null;
        if (request.Name != null) entity.Name = request.Name;
        if (request.Title != null) entity.Title = request.Title;
        if (request.Bio != null) entity.Bio = request.Bio;
        if (request.Specialties != null) entity.Specialties = request.Specialties;
        if (request.ImageUrl != null) entity.ImageUrl = request.ImageUrl;
        if (request.Email != null) entity.Email = request.Email;
        if (request.Phone != null) entity.Phone = request.Phone;
        if (request.Instagram != null) entity.Instagram = request.Instagram;
        if (request.IsActive.HasValue) entity.IsActive = request.IsActive.Value;
        if (request.SortOrder.HasValue) entity.SortOrder = request.SortOrder.Value;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return MapToResponse(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Trainers.FindAsync(id);
        if (entity == null) return false;
        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static TrainerResponse MapToResponse(Trainer t) => new(
        t.Id, t.Name, t.Title, t.Bio, t.Specialties,
        t.ImageUrl, t.Email, t.Phone, t.Instagram,
        t.IsActive, t.SortOrder, t.CreatedAt);
}
