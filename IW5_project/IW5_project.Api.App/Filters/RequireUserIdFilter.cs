using System.Security.Claims;

namespace IW5_project.Api.App.Filters;

// Ensures mutating requests identify the calling user.
// Checks standard claims first (for future Identity Providers in Phase 2)
// and falls back to the 'X-User-Id' HTTP header (for Phase 1).
// Returns 401 Unauthorized if neither is present or non-empty.
public class RequireUserIdFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;

        // 1. Claims (standard ASP.NET Core identity)
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // 2. HTTP header (Phase 1 testing)
        if (string.IsNullOrWhiteSpace(userId))
        {
            if (httpContext.Request.Headers.TryGetValue("X-User-Id", out var headerValue) &&
                !string.IsNullOrWhiteSpace(headerValue))
            {
                userId = headerValue.ToString();
            }
        }

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Results.StatusCode(StatusCodes.Status401Unauthorized);
        }

        return await next.Invoke(context);
    }
}
