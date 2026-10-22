using System.Security.Claims;

namespace IW5_project.Api.App.Endpoints;

public abstract class EndpointsBase
{
    public abstract IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder);

    public string? GetUserId(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return null;
        }

        // Try a standard claim
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrWhiteSpace(userId))
        {
            return userId;
        }

        // For Phase 1, we read from the HTTP header
        if (httpContext.Request.Headers.TryGetValue("X-User-Id", out var headerId) && !string.IsNullOrWhiteSpace(headerId))
        {
            return headerId.ToString();
        }

        return null;
    }

    public bool IsUserInRole(IHttpContextAccessor httpContextAccessor, string role)
        => GetUserRoles(httpContextAccessor).Contains(role, StringComparer.OrdinalIgnoreCase);

    public IList<string> GetUserRoles(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return [];
        }

        // Try a standard claim
        var rolesFromClaims = httpContext.User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        if (rolesFromClaims.Count > 0)
        {
            return rolesFromClaims;
        }

        // For Phase 1, we read from the HTTP header
        if (httpContext.Request.Headers.TryGetValue("X-User-Role", out var headerRoles) && !string.IsNullOrWhiteSpace(headerRoles))
        {
            return headerRoles.ToString()
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();
        }

        return [];
    }
}