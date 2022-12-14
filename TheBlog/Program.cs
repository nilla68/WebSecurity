using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TheBlog.DataAccess;
using TheBlog.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<FileValidationService>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BlogDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("BlogDbContextConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.ConfigureSameSiteNoneCookies();

builder.Services.AddAuth0WebAppAuthentication(options => {
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();