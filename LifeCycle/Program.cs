using CsvHelper.Configuration;
using LifeCycle.DataAccess;
using LifeCycle.DataAccess.Repository;
using LifeCycle.DataAccess.Repository.IRepository;
using LifeCycle.Services;
using LifeCycle.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICsvImportService, CSVImportService>();

//CSV Helper:
builder.Services.AddSingleton<CsvConfiguration>(sp =>
    new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HeaderValidated = null,
        MissingFieldFound = null
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
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
