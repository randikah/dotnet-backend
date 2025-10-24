using Microsoft.EntityFrameworkCore;
using LaptopStoreApi.Data;
using LaptopStoreApi.Repositories;
using LaptopStoreApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Entity Framework to use MySQL (Pomelo)
var mySqlConnection = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<LaptopStoreDbContext>(options =>
{
    // ServerVersion will be auto-detected from the connection string
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection));
});

// Register repositories and services
builder.Services.AddScoped<ILaptopRepository, LaptopRepository>();
builder.Services.AddScoped<ILaptopService, LaptopService>();

// Configure OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Laptop Store API",
        Version = "v1",
        Description = "A comprehensive API for managing laptop inventory in a store",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Laptop Store Support",
            Email = "support@laptopstore.com"
        }
    });

    // Include XML comments for better documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Laptop Store API V1");
        c.RoutePrefix = string.Empty; // Makes Swagger UI available at the app's root
    });
}

// Apply database migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LaptopStoreDbContext>();
    try
    {
        context.Database.EnsureCreated();
        
        // Optional: Add seed data if database is empty
        if (!context.Laptops.Any())
        {
            // Data will be seeded automatically through the DbContext configuration
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAllOrigins");

// Enable routing
app.UseRouting();

// Map controllers
app.MapControllers();

// Add sample weather forecast endpoint
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// Add a health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }))
   .WithName("HealthCheck")
   .WithOpenApi();

// Add a DB connectivity test endpoint
app.MapGet("/dbtest", async (IServiceProvider services) =>
{
    try
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LaptopStoreDbContext>();
        // Open a DB connection and run a simple server version query
        var conn = context.Database.GetDbConnection();
        await conn.OpenAsync();
        var serverVersion = conn.ServerVersion;
        await conn.CloseAsync();
        return Results.Ok(new { status = "Connected", serverVersion });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, title: "DB Connection Failed");
    }
})
   .WithName("DbTest")
   .WithOpenApi();

// Add a root endpoint that redirects to Swagger
app.MapGet("/", () => Results.Redirect("/swagger"))
   .WithName("Root")
   .ExcludeFromDescription();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}