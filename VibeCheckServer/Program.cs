using System.IO;
using Microsoft.EntityFrameworkCore;
using VibeCheckServer.DB;

var builder = WebApplication.CreateBuilder(args);

//config db
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
File.Delete(connectionString?.Replace("Data Source=", "") ?? string.Empty);

//register services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

// Add services for Razor Pages
builder.Services.AddRazorPages();

//Build App
var app = builder.Build();

//migrate db
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.MapOpenApi();
app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve static files
app.UseRouting();
app.UseCors(cors => cors.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
    endpoints.MapFallbackToFile("index.html"); // Fallback to Blazor WebAssembly app
});

//run app
app.Run();
