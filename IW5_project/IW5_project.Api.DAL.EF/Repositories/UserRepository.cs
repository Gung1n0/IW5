using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.DAL.Common.Mappers;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace IW5_project.Api.DAL.EF.Repositories
{
    public class UserRepository(UserMapper userMapper, DatabaseContext dbContext)
        : RepositoryBase<UserEntity>(dbContext), IUserRepository
    {
        public PagedResult<UserEntity> GetPaged(UserQuery query)
        {
            IQueryable<UserEntity> users = dbContext.Users.AsNoTracking();

            // Text search by name directly in an SQL database
            if (!string.IsNullOrWhiteSpace(query.search))
            {
                var term = query.search.Trim();
                users = users.Where(u => u.Name.Contains(term));
            }

            var totalCount = users.Count();

            // Sorting
            var descending = query.sortDirection == SortDirection.Descending;
            var sorted = descending
                ? users.OrderByDescending(u => u.Name)
                : users.OrderBy(u => u.Name);

            // Pagination Directly in the Database
            var items = sorted
                .ThenBy(u => u.Id)
                .Skip((query.page - 1) * query.pageSize)
                .Take(query.pageSize)
                .ToList();

            return new PagedResult<UserEntity>
            {
                Items = items,
                Page = query.page,
                PageSize = query.pageSize,
                TotalCount = totalCount
            };
        }

        public override Guid? Update(UserEntity userUpdated)
        {
            var userExisting = dbContext.Users.SingleOrDefault(u => u.Id == userUpdated.Id);
            if (userExisting is null)
            {
                return null;
            }

            userMapper.UpdateEntity(userUpdated, userExisting);
            dbContext.SaveChanges();

            return userExisting.Id;
        }

        public void TransferOwnership(Guid fromUserId, string toUserId)
        {
            var oldOwnerId = fromUserId.ToString();

            var places = dbContext.Places.Where(p => p.OwnerId == oldOwnerId).ToList();
            foreach (var place in places)
            {
                place.OwnerId = toUserId;
            }

            var items = dbContext.Items.Where(i => i.OwnerId == oldOwnerId).ToList();
            foreach (var item in items)
            {
                item.OwnerId = toUserId;
            }

            var shoppingLists = dbContext.ShoppingLists.Where(sl => sl.OwnerId == oldOwnerId).ToList();
            foreach (var list in shoppingLists)
            {
                list.OwnerId = toUserId;
            }

            dbContext.SaveChanges();
        }
    }
}