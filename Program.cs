using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApplication1.Services.UserService;
using WebApplication1.Services.PostService;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplication1.Services.FileService;
using BlogApp.Services.UserService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=DESKTOP-7CHC0AK;Database=BlogDb;Trusted_Connection=True;TrustServerCertificate=True;")
);

// Add Authentication
builder.Services.
AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme
    ).AddCookie(
        CookieAuthenticationDefaults.AuthenticationScheme, options => {
        options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.LogoutPath = "/Auth/Logout";
    });

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add Identity for User
builder.Services.AddIdentity<User, IdentityRole>(
    // enable unique email
    options => options.User.RequireUniqueEmail = true
    )
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<CustomClaimsFactory>(); // Add custom claims factory

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IFileService, FileService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
