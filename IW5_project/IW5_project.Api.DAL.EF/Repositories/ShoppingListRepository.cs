using IW5_project.Api.DAL.EF.Repositories;
using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.Common;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.Api.DAL.EF;
using IW5_project.Common.Enums;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.Enums;
using IW5_project.Api.DAL.Common.Mappers;
using Microsoft.EntityFrameworkCore;
using IW5_project.Api.Common.Models.Common;

namespace IW5_project.Api.DAL.EF.Repositories
{
    public class ShoppingListRepository(
        ShoppingListMapper shoppingListMapper,
        DatabaseContext dbContext)
        : RepositoryBase<ShoppingListEntity>(dbContext), IShoppingListRepository
    {

        public PagedResult<ShoppingListEntity> GetPaged(ShoppingListQuery query)
        {
            IQueryable<ShoppingListEntity> shoppingLists = dbContext.ShoppingLists.AsNoTracking();

            // search in name, description and store
            if (!string.IsNullOrWhiteSpace(query.search))
            {
                var term = query.search.Trim();
                shoppingLists = shoppingLists.Where(sl =>
                    sl.name.Contains(term) ||
                    sl.store.Contains(term) ||
                    (sl.description != null && sl.description.Contains(term)));
            }

            // filter by store
            if (!string.IsNullOrWhiteSpace(query.store))
            {
                var store = query.store.Trim();
                shoppingLists = shoppingLists.Where(sl => sl.store.Contains(store));
            }

            // filter by owner
            if (query.OwnerId is not null)
            {
                shoppingLists = shoppingLists.Where(sl => sl.OwnerId == query.OwnerId);
            }

            var totalCount = shoppingLists.Count();

            shoppingLists = ApplySort(shoppingLists, query);

            var items = shoppingLists
                .Skip((query.page - 1) * query.pageSize)
                .Take(query.pageSize)
                .Include(sl => sl.items)
                .ToList();

            return new PagedResult<ShoppingListEntity>
            {
                Items = items,
                Page = query.page,
                PageSize = query.pageSize,
                TotalCount = totalCount
            };
        }

        public override ShoppingListEntity? GetById(Guid id)
        {
            return dbContext.ShoppingLists
                .Include(sl => sl.items)
                    .ThenInclude(l => l.item)
                .SingleOrDefault(sl => sl.Id == id);
        }

        public override Guid? Update(ShoppingListEntity shoppingListUpdated)
        {
            var shoppingListExisting = dbContext.ShoppingLists
                .SingleOrDefault(sl => sl.Id == shoppingListUpdated.Id);

            if (shoppingListExisting is null)
            {
                return null;
            }

            shoppingListMapper.UpdateEntity(shoppingListUpdated, shoppingListExisting);
            dbContext.SaveChanges();

            return shoppingListExisting.Id;
        }


        public bool ItemExists(Guid itemId)
            => dbContext.Items.Any(i => i.Id == itemId);

        public ShoppingListItemEntity? GetLine(Guid shoppingListId, Guid itemId)
            => dbContext.ShoppingListItems
                .SingleOrDefault(l => l.shoppingListId == shoppingListId && l.itemId == itemId);

        public Guid InsertLine(ShoppingListItemEntity item)
        {
            dbContext.ShoppingListItems.Add(item);
            dbContext.SaveChanges();
            return item.Id;
        }

        public Guid? UpdateLine(Guid shoppingListId, Guid itemId, int quantity, bool isBought)
        {
            var item = GetLine(shoppingListId, itemId);
            if (item is null)
            {
                return null;
            }

            item.quantity = quantity;
            item.isBought = isBought;
            dbContext.SaveChanges();

            return item.Id;
        }

        public bool RemoveLine(Guid shoppingListId, Guid itemId)
        {
            var line = GetLine(shoppingListId, itemId);
            if (line is null)
            {
                return false;
            }

            dbContext.ShoppingListItems.Remove(line);
            dbContext.SaveChanges();
            return true;
        }

        private static IQueryable<ShoppingListEntity> ApplySort(
            IQueryable<ShoppingListEntity> shoppingLists, ShoppingListQuery query)
        {
            var descending = query.sortDirection == SortDirection.Descending;

            var sorted = query.SortBy switch
            {
                ShoppingListSortBy.ItemCount => descending
                    ? shoppingLists.OrderByDescending(sl => sl.items.Count)
                    : shoppingLists.OrderBy(sl => sl.items.Count),
                ShoppingListSortBy.Store => descending
                    ? shoppingLists.OrderByDescending(sl => sl.store)
                    : shoppingLists.OrderBy(sl => sl.store),
                _ => descending
                    ? shoppingLists.OrderByDescending(sl => sl.name)
                    : shoppingLists.OrderBy(sl => sl.name)
            };

            return sorted.ThenBy(sl => sl.Id);
        }
    }
}