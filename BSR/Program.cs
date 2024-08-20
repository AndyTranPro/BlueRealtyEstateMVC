using BSR.Models;
using BSR.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq"));
});

builder.Services.AddDbContext<HomeContext>(opt => opt.UseSqlite("Data Source=bsr.db"));

builder.Services.AddScoped<HomeService>();
builder.Services.AddScoped<DataSeedService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HomeContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();

    var dataSeedService = scope.ServiceProvider.GetRequiredService<DataSeedService>();
    dataSeedService.SeedHomes();
}

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Homes}/{action=Index}/{id?}");

app.Run();