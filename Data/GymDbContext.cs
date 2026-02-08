using Microsoft.EntityFrameworkCore;
using UndergroundGymAPI.Models;

namespace UndergroundGymAPI.Data;

public class GymDbContext : DbContext
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }

    public DbSet<GymClass> GymClasses => Set<GymClass>();
    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    public DbSet<UserMembership> UserMemberships => Set<UserMembership>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<GalleryImage> GalleryImages => Set<GalleryImage>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<ContactSubmission> ContactSubmissions => Set<ContactSubmission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // GymClass -> Trainer relationship
        modelBuilder.Entity<GymClass>()
            .HasOne(c => c.Trainer)
            .WithMany(t => t.Classes)
            .HasForeignKey(c => c.TrainerId)
            .OnDelete(DeleteBehavior.SetNull);

        // Booking -> GymClass relationship
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.GymClass)
            .WithMany()
            .HasForeignKey(b => b.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserMembership -> MembershipPlan relationship
        modelBuilder.Entity<UserMembership>()
            .HasOne(um => um.Plan)
            .WithMany()
            .HasForeignKey(um => um.PlanId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        modelBuilder.Entity<GymClass>().HasIndex(c => c.DayOfWeek);
        modelBuilder.Entity<GymClass>().HasIndex(c => c.IsActive);
        modelBuilder.Entity<Booking>().HasIndex(b => b.UserId);
        modelBuilder.Entity<Booking>().HasIndex(b => b.Status);
    }
}
