using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.Concrete;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Kültür ayarı — ondalık sayıların doğru parse edilmesi için
var invariantCulture = new[] { CultureInfo.InvariantCulture };
builder.Services.Configure<RequestLocalizationOptions>(opts =>
{
    opts.DefaultRequestCulture = new RequestCulture("en-US");
    opts.SupportedCultures = invariantCulture;
    opts.SupportedUICultures = invariantCulture;
});

builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<RentalyContext>();

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Service katmanı
builder.Services.AddScoped<IBranchService, BranchManager>();
builder.Services.AddScoped<IBrandService, BrandManager>();
builder.Services.AddScoped<ICarService, CarManager>();
builder.Services.AddScoped<ICarModelService, CarModelManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<IRentalService, RentalManager>();
builder.Services.AddScoped<ICarImageService, CarImageManager>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();