using Microsoft.EntityFrameworkCore;
using UndergroundGymAPI.Data;
using UndergroundGymAPI.DTOs;
using UndergroundGymAPI.Models;

namespace UndergroundGymAPI.Services;

public class TestimonialService : ITestimonialService
{
    private readonly GymDbContext _db;
    public TestimonialService(GymDbContext db) => _db = db;

    public async Task<IEnumerable<TestimonialResponse>> GetAllAsync(bool activeOnly = true)
    {
        var query = _db.Testimonials.AsQueryable();
        if (activeOnly) query = query.Where(t => t.IsActive);
        var items = await query.OrderBy(t => t.SortOrder).ToListAsync();
        return items.Select(MapToResponse);
    }

    public async Task<TestimonialResponse> CreateAsync(CreateTestimonialRequest request)
    {
        var entity = new Testimonial
        {
            MemberName = request.MemberName, MemberTitle = request.MemberTitle,
            Content = request.Content, Rating = request.Rating,
            ImageUrl = request.ImageUrl, SortOrder = request.SortOrder,
        };
        _db.Testimonials.Add(entity);
        await _db.SaveChangesAsync();
        return MapToResponse(entity);
    }

    public async Task<TestimonialResponse?> UpdateAsync(int id, UpdateTestimonialRequest request)
    {
        var entity = await _db.Testimonials.FindAsync(id);
        if (entity == null) return null;
        if (request.MemberName != null) entity.MemberName = request.MemberName;
        if (request.MemberTitle != null) entity.MemberTitle = request.MemberTitle;
        if (request.Content != null) entity.Content = request.Content;
        if (request.Rating.HasValue) entity.Rating = request.Rating.Value;
        if (request.ImageUrl != null) entity.ImageUrl = request.ImageUrl;
        if (request.IsActive.HasValue) entity.IsActive = request.IsActive.Value;
        if (request.SortOrder.HasValue) entity.SortOrder = request.SortOrder.Value;
        await _db.SaveChangesAsync();
        return MapToResponse(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Testimonials.FindAsync(id);
        if (entity == null) return false;
        entity.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }

    private static TestimonialResponse MapToResponse(Testimonial t) => new(
        t.Id, t.MemberName, t.MemberTitle, t.Content, t.Rating,
        t.ImageUrl, t.SortOrder, t.IsActive, t.CreatedAt);
}
