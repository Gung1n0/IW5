using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.DAL.Common.Mappers;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.Enums;
using Microsoft.EntityFrameworkCore;

// TODO: add normalized searching in text (e.g. s vs š, etc.)

namespace IW5_project.Api.DAL.EF.Repositories
{
    public class PlaceRepository(PlaceMapper placeMapper, DatabaseContext dbContext) : RepositoryBase<PlaceEntity>(dbContext), IPlaceRepository
    {
        public PagedResult<PlaceEntity> GetPaged(PlaceQuery query)
        {
            IQueryable<PlaceEntity> places = dbContext.Places.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.search))
            {
                var term = query.search.Trim();
                places = places.Where(p =>
                    p.name.Contains(term) ||
                    (p.description != null && p.description.Contains(term)));
            }

            if (query.OwnerId is not null)
            {
                places = places.Where(p => p.OwnerId == query.OwnerId);
            }

            var totalCount = places.Count();

            places = ApplySort(places, query);

            var items = places
                .Skip((query.page - 1) * query.pageSize)
                .Take(query.pageSize)
                .Include(p => p.items)          
                .ToList();                      

            return new PagedResult<PlaceEntity>
            {
                Items = items,
                Page = query.page,
                PageSize = query.pageSize,
                TotalCount = totalCount
            };
        }

        public override PlaceEntity? GetById(Guid id)
        {
            return dbContext.Places
                .Include(p => p.items)
                .SingleOrDefault(p => p.Id == id);
        }

        public override Guid? Update(PlaceEntity placeUpdated)
        {
            var placeExisting = dbContext.Places
                .SingleOrDefault(p => p.Id == placeUpdated.Id);

            if (placeExisting is null)
            {
                return null;
            }

            placeMapper.UpdateEntity(placeUpdated, placeExisting);
            dbContext.SaveChanges();

            return placeExisting.Id;
        }

        private static IQueryable<PlaceEntity> ApplySort(
            IQueryable<PlaceEntity> places, PlaceQuery query)
        {
            var descending = query.sortDirection == SortDirection.Descending;

            var sorted = query.SortBy switch
            {
                PlaceSortBy.ItemCount => descending
                    ? places.OrderByDescending(p => p.items.Count)
                    : places.OrderBy(p => p.items.Count),
                _ => descending
                    ? places.OrderByDescending(p => p.name)
                    : places.OrderBy(p => p.name)
            };

            return sorted.ThenBy(p => p.Id);
        }
    }
}
