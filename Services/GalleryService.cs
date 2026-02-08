using Microsoft.EntityFrameworkCore;
using UndergroundGymAPI.Data;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Models;

namespace UndergroundGymAPI.Services;

public class GalleryService : IGalleryService
{
    private readonly GymDbContext _db;
    public GalleryService(GymDbContext db) => _db = db;

    public async Task<IEnumerable<GalleryImageResponse>> GetAllAsync(bool activeOnly = true)
    {
        var query = _db.GalleryImages.AsQueryable();
        if (activeOnly) query = query.Where(g => g.IsActive);
        var images = await query.OrderBy(g => g.SortOrder).ToListAsync();
        return images.Select(MapToResponse);
    }

    public async Task<GalleryImageResponse> CreateAsync(CreateGalleryImageRequest request)
    {
        var entity = new GalleryImage
        {
            Title = request.Title, Description = request.Description,
            ImageUrl = request.ImageUrl, Category = request.Category,
            SortOrder = request.SortOrder,
        };
        _db.GalleryImages.Add(entity);
        await _db.SaveChangesAsync();
        return MapToResponse(entity);
    }

    public async Task<GalleryImageResponse?> UpdateAsync(int id, UpdateGalleryImageRequest request)
    {
        var entity = await _db.GalleryImages.FindAsync(id);
        if (entity == null) return null;
        if (request.Title != null) entity.Title = request.Title;
        if (request.Description != null) entity.Description = request.Description;
        if (request.ImageUrl != null) entity.ImageUrl = request.ImageUrl;
        if (request.Category != null) entity.Category = request.Category;
        if (request.IsActive.HasValue) entity.IsActive = request.IsActive.Value;
        if (request.SortOrder.HasValue) entity.SortOrder = request.SortOrder.Value;
        await _db.SaveChangesAsync();
        return MapToResponse(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.GalleryImages.FindAsync(id);
        if (entity == null) return false;
        entity.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }

    private static GalleryImageResponse MapToResponse(GalleryImage g) => new(
        g.Id, g.Title, g.Description, g.ImageUrl, g.Category,
        g.SortOrder, g.IsActive, g.CreatedAt);
}
