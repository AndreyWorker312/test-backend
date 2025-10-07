using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Data.Sqlite;
using UsersApp.Domain.Entities;
using UsersApp.Application.Users;
using UsersApp.Domain.Repositories;
using UsersApp.Infrastructure.Data;
using UsersApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// DbContext + SQLite
builder.Services.AddDbContext<UsersDbContext>(options =>
{
    var raw = builder.Configuration.GetConnectionString("Default") ?? "Data Source=users.db";
    var csb = new SqliteConnectionStringBuilder(raw);
    if (!string.IsNullOrWhiteSpace(csb.DataSource) && !Path.IsPathRooted(csb.DataSource))
    {
        csb.DataSource = Path.Combine(builder.Environment.ContentRootPath, csb.DataSource);
    }
    options.UseSqlite(csb.ConnectionString);
});

// DI: ����������� � ������
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Swagger ����������� ��� API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize database: apply migrations if present; otherwise ensure schema is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    var anyMigrations = db.Database.GetMigrations().Any();
    if (anyMigrations)
    {
        var hasPendingMigrations = db.Database.GetPendingMigrations().Any();
        if (hasPendingMigrations)
        {
            db.Database.Migrate();
        }
    }
    else
    {
        // No migrations in assembly: ensureCreated will create any missing tables from the model
        db.Database.EnsureCreated();
    }

    // Seed initial data for UI testing
    if (!db.Users.Any())
    {
        db.Users.Add(new User
        {
            FullName = "Test User",
            Email = "test@example.com",
            Phone = "+10000000000",
            Address = "Sample address"
        });
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}");

app.Run();
