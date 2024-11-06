using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApplication1.Services.UserService;
using WebApplication1.Services.PostService;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplication1.Services.FileService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=DESKTOP-7CHC0AK;Database=BlogDb;Trusted_Connection=True;TrustServerCertificate=True;")
);

/* Add Identity 
    - AddIdentity<User, IdentityRole> adds the Identity services to the DI container
    - AddEntityFrameworkStores<ApplicationDbContext> adds the EF Core stores for the Identity
    - AddDefaultTokenProviders() adds the default token providers used to generate tokens for password reset, email confirmation, etc.
    - RequireUniqueEmail = true ensures that each user has a unique email address
*/
builder.Services.AddIdentity<User, IdentityRole>(
    // enable unique email
    options => options.User.RequireUniqueEmail = true
    )
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });



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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
