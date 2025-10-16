using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using tvmaze_scraper.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TvMazeScraperContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TvMazeScraperContext") ?? throw new InvalidOperationException("Connection string 'TvMazeScraperContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<ScraperOptions>(
    builder.Configuration.GetSection("Scraper"));

// HttpClient configured for TVMaze
builder.Services.AddHttpClient("tvmaze", client =>
{
    var opts = builder.Configuration.GetSection("Scraper").Get<ScraperOptions>() ?? new();
    client.BaseAddress = null; // absolute URL in options
    client.DefaultRequestHeaders.UserAgent.Clear();
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(opts.UserAgent, null));
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.Timeout = TimeSpan.FromSeconds(30);
});

// DI: repository + hosted service
builder.Services.AddSingleton<IShowRepository, InMemoryShowRepository>();
builder.Services.AddHostedService<TvMazeScraperService>();

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
