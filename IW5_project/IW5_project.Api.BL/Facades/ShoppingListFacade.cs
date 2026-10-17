using IW5_project.Api.BL.Facades;
using IW5_project.Api.BL.Mappers;
using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.BL;                         
using IW5_project.Api.Common;                     
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.ShoppingList;
using IW5_project.Api.Common.Models.Common;

namespace IW5_project.Api.BL.Facades
{
    public class ShoppingListFacade(
        IShoppingListRepository shoppingListRepository,
        ShoppingListMapper mapper)
        : FacadeBase<IShoppingListRepository, ShoppingListEntity>(shoppingListRepository),
        IShoppingListFacade
    {
        private readonly IShoppingListRepository shoppingListRepository = shoppingListRepository;
        private readonly ShoppingListMapper mapper = mapper;

        public PagedResult<ShoppingListListModel> GetPaged(ShoppingListQuery query)
        {
            return shoppingListRepository.GetPaged(query).Map(mapper.ToListModel);
        }

        public ShoppingListDetailModel? GetById(Guid id)
        {
            var entity = shoppingListRepository.GetById(id);
            return entity is null ? null : mapper.ToDetailModel(entity);
        }

        public Guid CreateOrUpdate(ShoppingListDetailModel model, IList<string> userRoles, string? ownerId = null)
        {
            return shoppingListRepository.Exists(model.Id)
                ? Update(model, userRoles, ownerId)!.Value
                : Create(model, userRoles, ownerId);
        }

        public Guid Create(ShoppingListDetailModel model, IList<string> userRoles, string? ownerId = null)
        {
            var entity = mapper.ToEntity(model, ownerId);
            return shoppingListRepository.Insert(entity);
        }

        public Guid? Update(ShoppingListDetailModel model, IList<string> userRoles, string? ownerId = null)
        {
            if (!shoppingListRepository.Exists(model.Id))
            {
                return null;
            }

            ThrowIfWrongOwnerAndNotAdmin(model.Id, userRoles, ownerId);

            return shoppingListRepository.Update(mapper.ToEntity(model, ownerId));
        }
        public void Delete(Guid id, IList<string> userRoles, string? ownerId = null)
        {
            if (!shoppingListRepository.Exists(id))
            {
                throw new NotFoundException($"Shopping list {id} does not exist.");
            }

            ThrowIfWrongOwnerAndNotAdmin(id, userRoles, ownerId);

            shoppingListRepository.Remove(id);         
        }

        public Guid AddItem(Guid shoppingListId, AddShoppingListItemModel model)
        {
            if (!shoppingListRepository.Exists(shoppingListId))
            {
                throw new NotFoundException($"Shopping list {shoppingListId} does not exist.");
            }

            if (!shoppingListRepository.ItemExists(model.itemId))
            {
                throw new BadRequestException($"Item {model.itemId} does not exist.");
            }

            if (shoppingListRepository.GetLine(shoppingListId, model.itemId) is not null)
            {
                throw new ConflictException("This item is already on the shopping list.");
            }

            var line = mapper.ToLineEntity(model, Guid.NewGuid(), shoppingListId);
            return shoppingListRepository.InsertLine(line);
        }
        
        public Guid UpdateItem(Guid shoppingListId, Guid itemId, UpdateShoppingListItemModel model)
        {
            return shoppingListRepository.UpdateLine(shoppingListId, itemId, model.quantity, model.isBought)
                ?? throw new NotFoundException("This item is not on the shopping list.");
        }

        public void RemoveItem(Guid shoppingListId, Guid itemId)
        {
            if (!shoppingListRepository.RemoveLine(shoppingListId, itemId))
            {
                throw new NotFoundException("This item is not on the shopping list.");
            }
        }
    }
}