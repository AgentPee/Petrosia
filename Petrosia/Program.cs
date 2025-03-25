using Microsoft.EntityFrameworkCore;
using Petrosia.Models;
using System.Configuration;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UserManagement1Context>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21)),
        mysqlOptions => mysqlOptions.EnableRetryOnFailure()
        ));



builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/SignIn"; // Redirect to login page
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Redirect if unauthorized
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserManagement1Context>();
    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("Database connection successful!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database connection failed: " + ex);
    }
}

//create admin
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserManagement1Context>();

    var passwordHasher = new PasswordHasher<Admin>();

    // ✅ List of Admins to Add
    var adminsToAdd = new List<Admin>
    {
        new Admin
        {
            FirstName = "Zoro",
            LastName = "Roronoa",
            Email = "santoryo@gmail.com",
            PhoneNumber = "1234567890",
            Password = passwordHasher.HashPassword(null, "password"),
            Role = "Admin"
        },
        new Admin
        {
            FirstName = "admin",
            LastName = "user",
            Email = "admin@gmail.com",
            PhoneNumber = "0987654321",
            Password = passwordHasher.HashPassword(null, "password123"),
            Role = "Admin"
        }
    };

    // ✅ Check if Admins Already Exist
    foreach (var admin in adminsToAdd)
    {
        if (!dbContext.Admins.Any(a => a.Email == admin.Email)) // Avoid duplicates
        {
            dbContext.Admins.Add(admin);
            Console.WriteLine($"✅ Admin Added: {admin.Email}");
        }
        else
        {
            Console.WriteLine($"⚠️ Admin Already Exists: {admin.Email}");
        }
    }

    dbContext.SaveChanges();
}



var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Application has started!");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


builder.Services.AddAuthorization();


app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


