using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using IOC;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();
// Add services to the container.
builder.Services.RegisterService(builder.Configuration);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;         
    options.Password.RequireLowercase = true;      
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
})
               .AddEntityFrameworkStores<AppIdentityDbContext>()
               .AddDefaultTokenProviders();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await IdentitySeeder.SeedRoles(roleManager);
}

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
