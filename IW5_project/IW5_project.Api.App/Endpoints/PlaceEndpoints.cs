using IW5_project.Api.App.Filters;
using IW5_project.Api.BL;
using IW5_project.Api.BL.Facades;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.Place;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IW5_project.Api.App.Endpoints;

public class PlaceEndpoints() : EndpointsBase
{
    public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        var placeEndpoints = endpointRouteBuilder.MapGroup("place")
            .WithTags("place");

        // GET /api/place?search=&ownerId=&sortBy=&sortDirection=&page=&pageSize=
        placeEndpoints.MapGet("", Ok<PagedResult<PlaceListModel>> (
                [AsParameters] PlaceQuery query,                 // must be the first parameter (ValidationFilter)
                IPlaceFacade placeFacade)
            => TypedResults.Ok(placeFacade.GetPaged(query)))
            .AddEndpointFilter<ValidationFilter<PlaceQuery>>();

        // GET /api/place/{id}
        placeEndpoints.MapGet("{id:guid}", Results<Ok<PlaceDetailModel>, NotFound<string>> (
                Guid id,
                IPlaceFacade placeFacade)
            => placeFacade.GetById(id) is { } place
                ? TypedResults.Ok(place)
                : TypedResults.NotFound($"Place {id} was not found."));

        var placeModifyingEndpoints = placeEndpoints.MapGroup("");

        // POST /api/place
        placeModifyingEndpoints.MapPost("", Created<Guid> (
                PlaceDetailModel place,
                IPlaceFacade placeFacade,
                IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = GetUserId(httpContextAccessor);
            var userRoles = GetUserRoles(httpContextAccessor);

            var id = placeFacade.Create(place, userRoles, userId);
            return TypedResults.Created($"/api/place/{id}", id);
        })
            .AddEndpointFilter<ValidationFilter<PlaceDetailModel>>();

        // PUT /api/place   (id is in the body, same as the template)
        placeModifyingEndpoints.MapPut("", Results<Ok<Guid>, NotFound<string>, StatusCodeHttpResult> (
                PlaceDetailModel place,
                IPlaceFacade placeFacade,
                IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = GetUserId(httpContextAccessor);
            var userRoles = GetUserRoles(httpContextAccessor);

            try
            {
                return placeFacade.Update(place, userRoles, userId) is { } id
                    ? TypedResults.Ok(id)
                    : TypedResults.NotFound($"Place {place.Id} was not found.");
            }
            catch (NotFoundException ex)
            {
                return TypedResults.NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return TypedResults.StatusCode(StatusCodes.Status403Forbidden);
            }
        })
            .AddEndpointFilter<ValidationFilter<PlaceDetailModel>>();

        // DELETE /api/place/{id}
        placeModifyingEndpoints.MapDelete("{id:guid}", Results<Ok, NotFound<string>, Conflict<string>, StatusCodeHttpResult> (
            Guid id,
            IPlaceFacade placeFacade,
            IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = GetUserId(httpContextAccessor);
            var userRoles = GetUserRoles(httpContextAccessor);

            try
            {
                placeFacade.Delete(id, userRoles, userId);
                return TypedResults.Ok();
            }
            catch (NotFoundException ex)
            {
                return TypedResults.NotFound(ex.Message);
            }
            catch (ConflictException ex)
            {
                return TypedResults.Conflict(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return TypedResults.StatusCode(StatusCodes.Status403Forbidden);
            }
        });

        return endpointRouteBuilder;
    }
}