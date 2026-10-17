using System.Security.Claims;

namespace IW5_project.Api.App.Endpoints;

public abstract class EndpointsBase
{
    abstract public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder);

    public string? GetUserId(IHttpContextAccessor httpContextAccessor)
        => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public bool IsUserInRole(IHttpContextAccessor httpContextAccessor, string role)
        => httpContextAccessor.HttpContext?.User.IsInRole(role)
           ?? false;

    public IList<string> GetUserRoles(IHttpContextAccessor httpContextAccessor)
        => httpContextAccessor.HttpContext?.User.Claims
               .Where(c => c.Type == ClaimTypes.Role)
               .Select(c => c.Value)
               .ToList()
           ?? [];
}