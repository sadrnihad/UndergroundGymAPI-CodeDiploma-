using UndergroundGymAPI.Models;

namespace UndergroundGymAPI.Data;

public static class DbSeeder
{
    public static void Seed(GymDbContext context)
    {
        if (context.Trainers.Any()) return; // Already seeded

        // Trainers
        var trainers = new List<Trainer>
        {
            new() { Name = "Marcus Steel", Title = "Head Coach", Bio = "15+ years of competitive powerlifting and coaching experience. NSCA-CSCS certified.", Specialties = "[\"Powerlifting\",\"Strength Training\",\"Competition Prep\"]", Email = "marcus@undergroundgym.com", SortOrder = 1 },
            new() { Name = "Sarah Chen", Title = "HIIT & Cardio Specialist", Bio = "Former D1 athlete with a passion for high-intensity functional training.", Specialties = "[\"HIIT\",\"Cardio\",\"Functional Training\"]", Email = "sarah@undergroundgym.com", SortOrder = 2 },
            new() { Name = "Diego Ramirez", Title = "Boxing Coach", Bio = "Professional boxing background with 8 years of coaching. Golden Gloves champion.", Specialties = "[\"Boxing\",\"Kickboxing\",\"Self-Defense\"]", Email = "diego@undergroundgym.com", SortOrder = 3 },
            new() { Name = "Ava Williams", Title = "Yoga & Recovery", Bio = "RYT-500 certified yoga instructor specializing in mobility and athletic recovery.", Specialties = "[\"Yoga\",\"Mobility\",\"Recovery\",\"Meditation\"]", Email = "ava@undergroundgym.com", SortOrder = 4 },
        };
        context.Trainers.AddRange(trainers);
        context.SaveChanges();

        // Classes
        var classes = new List<GymClass>
        {
            new() { Name = "Iron Hour", Type = "Strength", DayOfWeek = 1, StartTime = "06:00", EndTime = "07:00", MaxCapacity = 20, Location = "Weight Room", TrainerId = trainers[0].Id, Description = "Heavy compound lifts with progressive overload programming." },
            new() { Name = "Burn Circuit", Type = "HIIT", DayOfWeek = 1, StartTime = "07:30", EndTime = "08:15", MaxCapacity = 25, Location = "Main Floor", TrainerId = trainers[1].Id, Description = "High-intensity interval training to torch calories and build endurance." },
            new() { Name = "Fight Club", Type = "Boxing", DayOfWeek = 1, StartTime = "17:00", EndTime = "18:00", MaxCapacity = 15, Location = "Boxing Ring", TrainerId = trainers[2].Id, Description = "Boxing fundamentals, bag work, and sparring drills." },
            new() { Name = "Zen Flow", Type = "Yoga", DayOfWeek = 1, StartTime = "19:00", EndTime = "20:00", MaxCapacity = 20, Location = "Studio B", TrainerId = trainers[3].Id, Description = "Vinyasa flow with emphasis on flexibility and mindfulness." },
            new() { Name = "Power Hour", Type = "Strength", DayOfWeek = 2, StartTime = "06:00", EndTime = "07:00", MaxCapacity = 20, Location = "Weight Room", TrainerId = trainers[0].Id, Description = "Deadlifts, squats, and bench press focused session." },
            new() { Name = "Cardio Blast", Type = "Cardio", DayOfWeek = 2, StartTime = "08:00", EndTime = "08:45", MaxCapacity = 30, Location = "Main Floor", TrainerId = trainers[1].Id, Description = "Heart-pumping cardio session mixing running, rowing, and cycling." },
            new() { Name = "CrossFit WOD", Type = "CrossFit", DayOfWeek = 3, StartTime = "06:00", EndTime = "07:00", MaxCapacity = 15, Location = "CrossFit Zone", TrainerId = trainers[0].Id, Description = "Workout of the Day - varied functional movements at high intensity." },
            new() { Name = "Kickboxing", Type = "Boxing", DayOfWeek = 3, StartTime = "17:30", EndTime = "18:30", MaxCapacity = 20, Location = "Boxing Ring", TrainerId = trainers[2].Id, Description = "Kickboxing combinations and heavy bag work." },
            new() { Name = "Morning HIIT", Type = "HIIT", DayOfWeek = 4, StartTime = "06:30", EndTime = "07:15", MaxCapacity = 25, Location = "Main Floor", TrainerId = trainers[1].Id, Description = "Wake up and crush it with this early morning HIIT session." },
            new() { Name = "Recovery Yoga", Type = "Yoga", DayOfWeek = 4, StartTime = "19:00", EndTime = "20:00", MaxCapacity = 20, Location = "Studio B", TrainerId = trainers[3].Id, Description = "Restorative yoga focused on deep stretching and recovery." },
            new() { Name = "Deadlift Day", Type = "Strength", DayOfWeek = 5, StartTime = "06:00", EndTime = "07:00", MaxCapacity = 15, Location = "Weight Room", TrainerId = trainers[0].Id, Description = "Deadlift variations and posterior chain development." },
            new() { Name = "Saturday Throwdown", Type = "CrossFit", DayOfWeek = 6, StartTime = "09:00", EndTime = "10:00", MaxCapacity = 20, Location = "CrossFit Zone", TrainerId = trainers[0].Id, Description = "Weekend warrior workout - team-based CrossFit challenge." },
        };
        context.GymClasses.AddRange(classes);

        // Membership Plans
        var plans = new List<MembershipPlan>
        {
            new() { Name = "STARTER", Price = 29.00m, Duration = "monthly", Description = "Perfect for beginners starting their fitness journey.", Features = "[\"Access to gym floor\",\"Locker room access\",\"2 group classes/week\",\"Basic fitness assessment\"]", SortOrder = 1 },
            new() { Name = "PRO", Price = 59.00m, Duration = "monthly", Description = "Our most popular plan for dedicated athletes.", Features = "[\"Unlimited gym access\",\"All group classes\",\"1 PT session/month\",\"Nutrition guidance\",\"Sauna & recovery zone\"]", IsPopular = true, SortOrder = 2 },
            new() { Name = "ELITE", Price = 99.00m, Duration = "monthly", Description = "The ultimate training experience with premium perks.", Features = "[\"Everything in Pro\",\"4 PT sessions/month\",\"Custom meal plans\",\"Priority class booking\",\"Guest passes (2/month)\",\"24/7 access\"]", SortOrder = 3 },
        };
        context.MembershipPlans.AddRange(plans);

        // Testimonials
        var testimonials = new List<Testimonial>
        {
            new() { MemberName = "Jake Morrison", MemberTitle = "Member since 2023", Content = "Underground completely transformed my approach to fitness. The coaches push you beyond what you think is possible. Down 30lbs and stronger than ever.", Rating = 5, SortOrder = 1 },
            new() { MemberName = "Lisa Park", MemberTitle = "Pro Member", Content = "The community here is unmatched. Every session feels like training with family. The HIIT classes with Sarah are absolutely killer.", Rating = 5, SortOrder = 2 },
            new() { MemberName = "Marcus Johnson", MemberTitle = "Elite Member", Content = "Best investment I've ever made. The personal training sessions and nutrition guidance have taken my performance to the next level.", Rating = 5, SortOrder = 3 },
            new() { MemberName = "Emma Rodriguez", MemberTitle = "Member since 2024", Content = "I was intimidated at first, but the Underground crew made me feel welcome from day one. The yoga recovery classes are a game changer.", Rating = 4, SortOrder = 4 },
        };
        context.Testimonials.AddRange(testimonials);

        context.SaveChanges();
    }
}
