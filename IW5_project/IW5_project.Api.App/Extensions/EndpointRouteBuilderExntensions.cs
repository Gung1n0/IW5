using IW5_project.API.App.Endpoints;

namespace IW5_project.API.App.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder UsePlaceEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        => new PlaceEndpoints().MapEndpoints(endpointRouteBuilder);

    // later: UseShoppingListEndpoints(...)
}