using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WepPha2.Data;
using WepPha2.Helpers;
using WepPha2.Interfaces;
using WepPha2.Models;
using WepPha2.Repository;
using WepPha2.Services;
using WepPha2.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure exception filters based on environment
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddControllersWithViews(options =>
    {
        // Add development exception filter
        options.Filters.Add<DevelopmentExceptionFilter>();
    });
}
else
{
    builder.Services.AddControllersWithViews(options =>
    {
        // Add production exception filter
        options.Filters.Add<ProductionExceptionFilter>();
    });
}

builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPurchaseDetailsRepository, PurchaseDetailsRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IPharmacyRepository,PharmacyRepository>();
builder.Services.AddScoped<IErrorMappingService, ErrorMappingService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddSession();
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebPharmacyConnection"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie();

var app = builder.Build();

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Use Developer Exception Page in development
    app.UseDeveloperExceptionPage();
}
else
{
    // Use custom exception handler in production
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Use Status Code Pages middleware
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Default route with optional parameters and constraints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

public partial class Program { }