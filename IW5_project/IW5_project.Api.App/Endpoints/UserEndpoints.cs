using IW5_project.Api.App.Filters;
using IW5_project.Api.BL;
using IW5_project.Api.BL.Facades;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.User;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IW5_project.Api.App.Endpoints;

public class UserEndpoints : EndpointsBase
{
    public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        var userEndpoints = endpointRouteBuilder.MapGroup("user")
            .WithTags("user");

        // GET /api/user?search=&page=&pageSize=
        userEndpoints.MapGet("", Ok<PagedResult<UserListModel>> (
                [AsParameters] UserQuery query,
                IUserFacade userFacade)
            => TypedResults.Ok(userFacade.GetPaged(query)))
            .AddEndpointFilter<ValidationFilter<UserQuery>>();

        // GET /api/user/{id}
        userEndpoints.MapGet("{id:guid}", Results<Ok<UserDetailModel>, NotFound<string>> (
                Guid id,
                IUserFacade userFacade)
            => userFacade.GetById(id) is { } user
                ? TypedResults.Ok(user)
                : TypedResults.NotFound($"User {id} was not found."));

        var userModifyingEndpoints = userEndpoints.MapGroup("")
            .AddEndpointFilter<RequireUserIdFilter>();

        // POST /api/user
        userModifyingEndpoints.MapPost("", Results<Created<Guid>, StatusCodeHttpResult> (
                UserDetailModel user,
                IUserFacade userFacade,
                IHttpContextAccessor httpContextAccessor) =>
        {
            var currentUserId = GetUserId(httpContextAccessor);
            var userRoles = GetUserRoles(httpContextAccessor);

            try
            {
                var id = userFacade.Create(user, userRoles, currentUserId);
                return TypedResults.Created($"/api/user/{id}", id);
            }
            catch (UnauthorizedAccessException)
            {
                return TypedResults.StatusCode(StatusCodes.Status403Forbidden);
            }
        })
            .AddEndpointFilter<ValidationFilter<UserDetailModel>>();

        // PUT /api/user
        userModifyingEndpoints.MapPut("", Results<Ok<Guid>, NotFound<string>, StatusCodeHttpResult> (
                UserDetailModel user,
                IUserFacade userFacade,
                IHttpContextAccessor httpContextAccessor) =>
        {
            var currentUserId = GetUserId(httpContextAccessor);
            var userRoles = GetUserRoles(httpContextAccessor);

            try
            {
                return userFacade.Update(user, userRoles, currentUserId) is { } id
                    ? TypedResults.Ok(id)
                    : TypedResults.NotFound($"User {user.Id} was not found.");
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
            .AddEndpointFilter<ValidationFilter<UserDetailModel>>();

        // DELETE /api/user/{id}
        userModifyingEndpoints.MapDelete("{id:guid}", Results<Ok, NotFound<string>, StatusCodeHttpResult> (
            Guid id,
            IUserFacade userFacade,
            IHttpContextAccessor httpContextAccessor) =>
        {
            var currentUserId = GetUserId(httpContextAccessor);
            var userRoles = GetUserRoles(httpContextAccessor);

            try
            {
                userFacade.Delete(id, userRoles, currentUserId);
                return TypedResults.Ok();
            }
            catch (NotFoundException ex)
            {
                return TypedResults.NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return TypedResults.StatusCode(StatusCodes.Status403Forbidden);
            }
        });

        return endpointRouteBuilder;
    }
}