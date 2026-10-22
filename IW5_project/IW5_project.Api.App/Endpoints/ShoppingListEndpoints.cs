using IW5_project.Api.App.Filters;
using IW5_project.Api.BL;
using IW5_project.Api.BL.Facades;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.ShoppingList;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IW5_project.Api.App.Endpoints;

public class ShoppingListEndpoints : EndpointsBase
{
    public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
    {
        var shoppingListEndpoints = endpointRouteBuilder.MapGroup("shopping-list")
            .WithTags("shopping-list");


        // GET /api/shopping-list?search=&store=&OwnerId=&SortBy=&sortDirection=&page=&pageSize=
        shoppingListEndpoints.MapGet("", Ok<PagedResult<ShoppingListListModel>> (
                [AsParameters] ShoppingListQuery query,
                IShoppingListFacade shoppingListFacade)
            => TypedResults.Ok(shoppingListFacade.GetPaged(query)))
            .AddEndpointFilter<ValidationFilter<ShoppingListQuery>>()
            .WithName("GetShoppingLists")
            .WithSummary("Lists shopping lists")
            .WithDescription("Paged list with server-side search (name, store, description), " +
                             "filtering by store and owner, and sorting by name, store or item count.")
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);

        shoppingListEndpoints.MapGet("{id:guid}", Results<Ok<ShoppingListDetailModel>, NotFound<string>> (
                Guid id,
                IShoppingListFacade shoppingListFacade)
            => shoppingListFacade.GetById(id) is { } shoppingList
                ? TypedResults.Ok(shoppingList)
                : TypedResults.NotFound($"Shopping list {id} was not found."))
            .WithName("GetShoppingListById")
            .WithSummary("Gets a shopping list with its items");

        var modifyingEndpoints = shoppingListEndpoints.MapGroup("");

        // POST /api/shopping-list
        modifyingEndpoints.MapPost("", Created<Guid> (
                ShoppingListDetailModel shoppingList,
                IShoppingListFacade shoppingListFacade,
                IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = GetUserId(httpContextAccessor);
            var userRoles = GetUserRoles(httpContextAccessor);

            var id = shoppingListFacade.Create(shoppingList, userRoles, userId);
            return TypedResults.Created($"/api/shopping-list/{id}", id);
        })
            .AddEndpointFilter<ValidationFilter<ShoppingListDetailModel>>()
            .WithName("CreateShoppingList")
            .WithSummary("Creates a shopping list")
            .WithDescription("Any items in the request body are ignored; add them via POST {id}/items.")
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);

        // PUT /api/shopping-list
        modifyingEndpoints.MapPut("", Results<Ok<Guid>, NotFound<string>, ForbidHttpResult> (
                ShoppingListDetailModel shoppingList,
                IShoppingListFacade shoppingListFacade,
                IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = GetUserId(httpContextAccessor);
            var userRoles = GetUserRoles(httpContextAccessor);

            try
            {
                return shoppingListFacade.Update(shoppingList, userRoles, userId) is { } id
                    ? TypedResults.Ok(id)
                    : TypedResults.NotFound($"Shopping list {shoppingList.Id} was not found.");
            }
            catch (NotFoundException ex)
            {
                return TypedResults.NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return TypedResults.Forbid();
            }
        })
            .AddEndpointFilter<ValidationFilter<ShoppingListDetailModel>>()
            .WithName("UpdateShoppingList")
            .WithSummary("Updates name, store and description of a shopping list")
            .WithDescription("Items are not touched; use the items endpoints for that.")
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);

        // DELETE /api/shopping-list/{id}
        modifyingEndpoints.MapDelete("{id:guid}", Results<Ok, NotFound<string>, ForbidHttpResult> (
                Guid id,
                IShoppingListFacade shoppingListFacade,
                IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = GetUserId(httpContextAccessor);
            var userRoles = GetUserRoles(httpContextAccessor);

            try
            {
                shoppingListFacade.Delete(id, userRoles, userId);
                return TypedResults.Ok();
            }
            catch (NotFoundException ex)
            {
                return TypedResults.NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return TypedResults.Forbid();
            }
        })
            .WithName("DeleteShoppingList")
            .WithSummary("Deletes a shopping list");

        // POST /api/shopping-list/{id}/items
        modifyingEndpoints.MapPost("{id:guid}/items",
            Results<Created<Guid>, NotFound<string>, BadRequest<string>, Conflict<string>> (
                AddShoppingListItemModel model,
                Guid id,
                IShoppingListFacade shoppingListFacade) =>
            {
                try
                {
                    var lineId = shoppingListFacade.AddItem(id, model);
                    return TypedResults.Created($"/api/shopping-list/{id}", lineId);
                }
                catch (NotFoundException ex)
                {
                    return TypedResults.NotFound(ex.Message);
                }
                catch (BadRequestException ex)
                {
                    return TypedResults.BadRequest(ex.Message);
                }
                catch (ConflictException ex)
                {
                    return TypedResults.Conflict(ex.Message);
                }
            })
            .AddEndpointFilter<ValidationFilter<AddShoppingListItemModel>>()
            .WithName("AddShoppingListItem")
            .WithSummary("Adds an item to a shopping list")
            .WithDescription("404 – list not found, 400 – item not found, 409 – item is already on the list.")
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);

        // PUT /api/shopping-list/{id}/items/{itemId}
        modifyingEndpoints.MapPut("{id:guid}/items/{itemId:guid}",
            Results<Ok<Guid>, NotFound<string>> (
                UpdateShoppingListItemModel model,
                Guid id,
                Guid itemId,
                IShoppingListFacade shoppingListFacade) =>
            {
                try
                {
                    return TypedResults.Ok(shoppingListFacade.UpdateItem(id, itemId, model));
                }
                catch (NotFoundException ex)
                {
                    return TypedResults.NotFound(ex.Message);
                }
            })
            .AddEndpointFilter<ValidationFilter<UpdateShoppingListItemModel>>()
            .WithName("UpdateShoppingListItem")
            .WithSummary("Changes quantity and bought flag of an item on a shopping list")
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);

        // DELETE /api/shopping-list/{id}/items/{itemId}
        modifyingEndpoints.MapDelete("{id:guid}/items/{itemId:guid}",
            Results<Ok, NotFound<string>> (
                Guid id,
                Guid itemId,
                IShoppingListFacade shoppingListFacade) =>
            {
                try
                {
                    shoppingListFacade.RemoveItem(id, itemId);
                    return TypedResults.Ok();
                }
                catch (NotFoundException ex)
                {
                    return TypedResults.NotFound(ex.Message);
                }
            })
            .WithName("RemoveShoppingListItem")
            .WithSummary("Removes an item from a shopping list");

        return endpointRouteBuilder;
    }
}
