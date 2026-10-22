using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.DAL.Common.Mappers;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.Enums;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IW5_project.Api.DAL.EF.Repositories
{
    public class ItemRepository(ItemMapper itemMapper, DatabaseContext dbContext) : RepositoryBase<ItemEntity>(dbContext), IItemRepository
    {
        public PagedResult<ItemEntity> GetPaged(ItemQuery query)
        {
            IQueryable<ItemEntity> items = dbContext.Items.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(query.search))
            {
                var term = query.search.Trim();
                items = items.Where(i =>
                    i.Name.Contains(term) ||
                    (i.Description != null && i.Description.Contains(term)));
            }

            if (query.PlaceId is not null)
            {
                items = items.Where(i => i.PlaceId == query.PlaceId);
            }

            if (query.Category is not null)
            {
                items = items.Where(i => i.Category == query.Category);
            }

            var totalCount = items.Count();

            items = ApplySort(items, query);

            var data = items
                .Skip((query.page - 1) * query.pageSize)
                .Take(query.pageSize)
                .Include(i => i.Place)
                .ToList();

            return new PagedResult<ItemEntity>
            {
                Items = data,
                Page = query.page,
                PageSize = query.pageSize,
                TotalCount = totalCount
            };
        }

        public override ItemEntity? GetById(Guid id)
        {
            return dbContext.Items
                .Include(p => p.Place)
                .SingleOrDefault(p => p.Id == id);
        }
        public override Guid? Update(ItemEntity entity)
        {
            var ItemExisting = dbContext.Items
                .SingleOrDefault(p => p.Id == entity.Id);

            if (ItemExisting is null)
            {
                return null;
            }

            itemMapper.UpdateEntity(entity, ItemExisting);
            dbContext.SaveChanges();

            return ItemExisting.Id;
        }

        private static IQueryable<ItemEntity> ApplySort(IQueryable<ItemEntity> item, ItemQuery query)
        {
            var descending = query.sortDirection == SortDirection.Descending;

            var sorted = query.Sort switch
            {
                ItemSortBy.Category => descending
                    ? item.OrderByDescending(p => p.Category)
                    : item.OrderBy(p => p.Category),
                ItemSortBy.ExpirationDate => descending
                    ? item.OrderByDescending(p => p.ExpirationDate)
                    : item.OrderBy(p => p.ExpirationDate),
                ItemSortBy.Quantity => descending
                    ? item.OrderByDescending(p=>p.Quantity)
                    : item.OrderBy(p => p.Quantity),
                _ => descending
                    ? item.OrderByDescending(p => p.Name)
                    : item.OrderBy(p => p.Name)
            };

            return sorted.ThenBy(p => p.Id);
        }
    }
}
