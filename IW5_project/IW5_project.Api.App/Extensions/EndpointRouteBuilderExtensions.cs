using IW5_project.Api.App.Endpoints;

namespace IW5_project.Api.App.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder UsePlaceEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        => new PlaceEndpoints().MapEndpoints(endpointRouteBuilder);

    public static IEndpointRouteBuilder UseShoppingListEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        => new ShoppingListEndpoints().MapEndpoints(endpointRouteBuilder);

    public static IEndpointRouteBuilder UseUserEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        => new UserEndpoints().MapEndpoints(endpointRouteBuilder);
}
