using AspNetCoreRateLimit;
using ObstaRace.API.Middleware;
using ObstaRace.Infrastructure.Data;
using ObstaRace.Infrastructure.Seeders;
using ObstaRace.Infrastructure.Extensions;
using ObstaRace.Application.Extensions;
using ObstaRace.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddPresentationServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await DataSeeder.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseIpRateLimiting();

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    if (response.HasStarted) return;
    if (response.StatusCode == 403)
    {
        response.ContentType = "application/json";
        await response.WriteAsJsonAsync(new 
        { 
            status = 403, 
            error = "Forbidden",
            message = "You do not have permission to access this resource."
        });
    }
    else if (response.StatusCode == 401)
    {
        response.ContentType = "application/json";
        await response.WriteAsJsonAsync(new 
        { 
            status = 401, 
            error = "Unauthorized",
            message = "Authentication is required."
        });
    }
});

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<BanCheckMiddleware>();
app.MapControllers();
app.Run();