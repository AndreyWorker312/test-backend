using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Data.Sqlite;
using UsersApp.Domain.Entities;
using UsersApp.Application.Users;
using UsersApp.Domain.Repositories;
using UsersApp.Infrastructure.Data;
using UsersApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure database
ConfigureDatabase(builder);

// Configure dependency injection
ConfigureDependencyInjection(builder.Services);

// Configure Swagger for API documentation
ConfigureSwagger(builder.Services);

var app = builder.Build();

// Initialize database with proper error handling
await InitializeDatabaseAsync(app);

// Configure the HTTP request pipeline
ConfigurePipeline(app);

await app.RunAsync();

// Helper methods for better organization
static void ConfigureDatabase(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<UsersDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Data Source=users.db";
        var csb = new SqliteConnectionStringBuilder(connectionString);
        
        // Ensure relative paths are resolved correctly
        if (!string.IsNullOrWhiteSpace(csb.DataSource) && !Path.IsPathRooted(csb.DataSource))
        {
            csb.DataSource = Path.Combine(builder.Environment.ContentRootPath, csb.DataSource);
        }
        
        options.UseSqlite(csb.ConnectionString);
    });
}

static void ConfigureDependencyInjection(IServiceCollection services)
{
    // Register repositories and services
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IUserService, UserService>();
}

static void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

static async Task InitializeDatabaseAsync(WebApplication app)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        
        // Check if migrations exist and apply them
        var anyMigrations = db.Database.GetMigrations().Any();
        if (anyMigrations)
        {
            var hasPendingMigrations = db.Database.GetPendingMigrations().Any();
            if (hasPendingMigrations)
            {
                await db.Database.MigrateAsync();
            }
        }
        else
        {
            // No migrations: create schema from model
            await db.Database.EnsureCreatedAsync();
        }

        // Seed initial data for development/testing
        if (!db.Users.Any())
        {
            db.Users.Add(new User
            {
                FullName = "Test User",
                Email = "test@example.com",
                Phone = "+10000000000",
                Address = "Sample address"
            });
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database");
        throw;
    }
}

static void ConfigurePipeline(WebApplication app)
{
    // Configure development-specific middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        // Production error handling
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    // Configure middleware pipeline
    app.UseStaticFiles();
    app.UseRouting();
    
    // Configure routing
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Users}/{action=Index}/{id?}");
}