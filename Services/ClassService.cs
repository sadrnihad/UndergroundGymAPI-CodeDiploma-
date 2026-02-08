using Microsoft.EntityFrameworkCore;
using UndergroundGymAPI.Data;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Models;

namespace UndergroundGymAPI.Services;

public class ClassService : IClassService
{
    private readonly GymDbContext _db;
    public ClassService(GymDbContext db) => _db = db;

    public async Task<IEnumerable<ClassResponse>> GetAllAsync(bool activeOnly = true)
    {
        var query = _db.GymClasses.AsQueryable();
        if (activeOnly) query = query.Where(c => c.IsActive);
        var classes = await query.OrderBy(c => c.DayOfWeek).ThenBy(c => c.StartTime).ToListAsync();
        return classes.Select(MapToResponse);
    }

    public async Task<ClassResponse?> GetByIdAsync(int id)
    {
        var c = await _db.GymClasses.FindAsync(id);
        return c == null ? null : MapToResponse(c);
    }

    public async Task<ClassResponse> CreateAsync(CreateClassRequest request)
    {
        var entity = new GymClass
        {
            Name = request.Name,
            Description = request.Description,
            Type = request.Type,
            TrainerId = request.TrainerId,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            MaxCapacity = request.MaxCapacity,
            Location = request.Location,
            ImageUrl = request.ImageUrl,
        };
        _db.GymClasses.Add(entity);
        await _db.SaveChangesAsync();
        return MapToResponse(entity);
    }

    public async Task<ClassResponse?> UpdateAsync(int id, UpdateClassRequest request)
    {
        var entity = await _db.GymClasses.FindAsync(id);
        if (entity == null) return null;

        if (request.Name != null) entity.Name = request.Name;
        if (request.Description != null) entity.Description = request.Description;
        if (request.Type != null) entity.Type = request.Type;
        if (request.TrainerId.HasValue) entity.TrainerId = request.TrainerId;
        if (request.DayOfWeek.HasValue) entity.DayOfWeek = request.DayOfWeek.Value;
        if (request.StartTime != null) entity.StartTime = request.StartTime;
        if (request.EndTime != null) entity.EndTime = request.EndTime;
        if (request.MaxCapacity.HasValue) entity.MaxCapacity = request.MaxCapacity.Value;
        if (request.Location != null) entity.Location = request.Location;
        if (request.IsActive.HasValue) entity.IsActive = request.IsActive.Value;
        if (request.ImageUrl != null) entity.ImageUrl = request.ImageUrl;
        entity.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return MapToResponse(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.GymClasses.FindAsync(id);
        if (entity == null) return false;
        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static ClassResponse MapToResponse(GymClass c) => new(
        c.Id, c.Name, c.Description, c.Type, c.TrainerId,
        c.DayOfWeek, c.StartTime, c.EndTime, c.MaxCapacity,
        c.CurrentEnrollment, c.Location, c.IsActive, c.ImageUrl,
        c.CreatedAt, c.UpdatedAt);
}
