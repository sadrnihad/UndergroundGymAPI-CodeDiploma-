using Microsoft.EntityFrameworkCore;
using UndergroundGymAPI.Data;
using UndergroundGymAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Underground Gym API",
        Version = "v1",
        Description = "RESTful API for Underground Gym - Manage classes, memberships, trainers, bookings, and more.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Underground Gym",
            Email = "info@undergroundgym.com"
        }
    });
});

// Database context - defaults to InMemory for easy development
// Switch to SQL Server by updating the connection string in appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<GymDbContext>(options =>
        options.UseInMemoryDatabase("UndergroundGymDb"));
}
else
{
    builder.Services.AddDbContext<GymDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// Register services
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();
builder.Services.AddScoped<ITestimonialService, TestimonialService>();
builder.Services.AddScoped<IContactService, ContactService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Seed data in development
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GymDbContext>();
    context.Database.EnsureCreated();
    DbSeeder.Seed(context);
}

// Configure the HTTP request pipeline

// Swagger is available at /swagger in all environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Underground Gym API v1");
    c.RoutePrefix = "swagger"; // Swagger at /swagger (not root)
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Serve static files from wwwroot (CSS, JS, images)
app.UseDefaultFiles(); // Serves index.html as the default page at "/"
app.UseStaticFiles();  // Serves all files from wwwroot

app.UseAuthorization();
app.MapControllers();

app.Run();
