using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.Concrete;
using Rentaly.BusinessLayer.Mapping;
using Rentaly.BusinessLayer.ValidationRules.RentalValidator;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.UnitOfWork;
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

builder.Services.AddAutoMapper(typeof(GeneralMapping));

builder.Services.AddControllersWithViews(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRentalValidator>();

// DbContext
builder.Services.AddDbContext<RentalyContext>();

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Service katmanı
builder.Services.AddScoped<IAboutService, AboutManager>();
builder.Services.AddScoped<IBannerService, BannerManager>();
builder.Services.AddScoped<IBranchService, BranchManager>();
builder.Services.AddScoped<IBrandService, BrandManager>();
builder.Services.AddScoped<ICarService, CarManager>();
builder.Services.AddScoped<ICarImageService, CarImageManager>();
builder.Services.AddScoped<ICarModelService, CarModelManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<IFaqService, FaqManager>();
builder.Services.AddScoped<IProcessService, ProcessManager>();
builder.Services.AddScoped<IRentalService, RentalManager>();
builder.Services.AddScoped<ITestimonialService, TestimonialManager>();
builder.Services.AddScoped<IMailService, MailManager>();

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

var app = builder.Build();

var cultureInfo = new System.Globalization.CultureInfo("tr-TR");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/Home/PageNotFound");
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