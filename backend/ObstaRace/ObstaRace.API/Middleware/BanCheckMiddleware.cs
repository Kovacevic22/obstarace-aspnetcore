using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ObstaRace.Application.Interfaces.Repositories;

namespace ObstaRace.API.Middleware;

internal sealed class BanCheckMiddleware(RequestDelegate next, ILogger<BanCheckMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        logger.LogInformation("BanCheck middleware triggered, IsAuthenticated: {IsAuth}", 
            httpContext.User.Identity?.IsAuthenticated);
        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                int userId = int.Parse(userIdClaim.Value);
                var cache = httpContext.RequestServices.GetRequiredService<IDistributedCache>();
                var cacheKey = $"ban_{userId}";
                var cachedBan = await cache.GetStringAsync(cacheKey);
                bool isBanned;
                if (cachedBan == null)
                {
                    var userRepo = httpContext.RequestServices.GetRequiredService<IUserRepository>();
                    isBanned = await userRepo.IsBanned(userId);
                    await cache.SetStringAsync(cacheKey, isBanned.ToString(), new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow   = TimeSpan.FromMinutes(5)
                    });
                }
                else
                {
                    isBanned = bool.Parse(cachedBan);
                }
                if (isBanned)
                {
                    logger.LogWarning("Banned user {UserId} attempted to access the server", userId);
                    httpContext.Response.Cookies.Delete("X-Access-Token");
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Title = "Unauthorized",
                        Detail = "You are banned from this server"
                    });
                    return;
                }
            }
        }
        await next(httpContext);
    }
}