using CookBook.Api.DAL.EF.Repositories;
using IW5_project.Api.DAL.Common.Entities;
using IW5_project.API.Common;
using IW5_project.API.DAL.Common.Entities;
using IW5_project.API.DAL.Common.Mappers;
using IW5_project.API.DAL.Common.Repositories;
using IW5_project.API.DAL.EF;
using IW5_project.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IW5_project.Api.DAL.EF.Repositories
{
    public class ShoppingListRepository(ShoppingListMapper shoppingListMapper, DatabaseContext dbContext) : RepositoryBase<ShoppingListEntity>(dbContext), IShoppingListRepository
    {
        public PagedResult<ShoppingListEntity> GetPaged(ShoppingListQuery query)
        {
            IQueryable<ShoppingListEntity> shoppingLists = dbContext.ShoppingLists.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.search))
            {
                var term = query.search.Trim();
                shoppingLists = shoppingLists.Where(sl =>
                    sl.name.Contains(term) ||
                    (sl.description != null && sl.description.Contains(term)));
            }

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
        private static IQueryable<ShoppingListEntity> ApplySort(
           IQueryable<ShoppingListEntity> shoppingLists, ShoppingListQuery query)
        {
            var descending = query.sortDirection == SortDirection.Descending;

            var sorted = query.SortBy switch
            {
                ShoppingListSortBy.ItemCount => descending
                    ? shoppingLists.OrderByDescending(sl => sl.items.Count)
                    : shoppingLists.OrderBy(sl => sl.items.Count),
                _ => descending
                    ? shoppingLists.OrderByDescending(sl => sl.name)
                    : shoppingLists.OrderBy(sl => sl.name)
            };

            return sorted.ThenBy(sl => sl.Id);
        }
    }
}