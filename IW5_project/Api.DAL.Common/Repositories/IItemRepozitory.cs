using IW5_project.Api.DAL.Common.Entities;
using IW5_project.API.BL.Mappers;
using IW5_project.API.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IW5_project.Api.DAL.Common.Repositories
{
    // --- TEMPORARY STUBS (Delete these when AppDbContext available) ---
    public class AppDbContext
    {
        public List<ItemEntity> Item { get; set; } = new();
        public Task<int> SaveChangesAsync() => Task.FromResult(1);
    }

    public static class DbSetExtensions
    {
        // Allows .ToListAsync() and .FirstOrDefaultAsync() to work on simple lists for testing
        public static Task<List<T>> ToListAsync<T>(this IEnumerable<T> source) => Task.FromResult(source.ToList());
        public static Task<T?> FirstOrDefaultAsync<T>(this IEnumerable<T> source, Func<T, bool> predicate) => Task.FromResult(source.FirstOrDefault(predicate));
    }
    // -------------------------------------------------------------------------------

    public class ItemService
    {
        private readonly TransferObjects _transferObjects;
        private readonly AppDbContext _context;

        public ItemService(AppDbContext context)
        {
            _context = context;
        }

        //create Item
        public async Task<int> CreateItemAsync(CreateItemDTO Dto, int CurrentUserId)
        {
            var Entity = new ItemEntity
            {
                Id = _context.Item.Count + 1,
                Name = Dto.Name,
                ImageUrl = Dto.Url,
                Quantity = Dto.Amount,
                ExpirationDate = Dto.ExpirationDate,
                Category = Dto.Category,
                Description = Dto.Text,
                PlaceId = Dto.PlaceId,
                UserId = CurrentUserId
            };

            _context.Item.Add(Entity);

            await _context.SaveChangesAsync();

            return Entity.Id;
        }

        //get all Items
        public async Task<List<ItemDTO>> GetAllItems()
        {
            return await _context.Item.Select(Entity => new ItemDTO
            {
                Id = Entity.Id,
                Name = Entity.Name,
                Url = Entity.ImageUrl,
                Amount = Entity.Quantity,
                ExpirationDate = Entity.ExpirationDate,
                Category = Entity.Category,
                Text = Entity.Description,
                UserId = Entity.UserId,
                PlaceId = Entity.PlaceId
            }).ToListAsync();
        }

        //get item by id
        public async Task<ItemDTO> GetByIdAsync(int CurrentId)
        {
            ItemEntity Item = await _context.Item.FirstOrDefaultAsync(p => p.Id == CurrentId);

            if (Item == null)
                return null;

           return _transferObjects.EntityToDTO(Item);
        }

        //updates existing Item
        public async Task<ItemDTO> UpdateItem(int CurrentId, ItemEntity UpdatedItem)
        {
            ItemEntity Item = await _context.Item.FirstOrDefaultAsync(p => p.Id == CurrentId);

            if (Item == null)
                return null;

            Item.Name = UpdatedItem.Name;
            Item.ImageUrl = UpdatedItem.ImageUrl;
            Item.Quantity = UpdatedItem.Quantity;
            Item.ExpirationDate = UpdatedItem.ExpirationDate;
            Item.Category = UpdatedItem.Category;
            Item.Description = UpdatedItem.Description;
            Item.UserId = UpdatedItem.UserId;
            Item.PlaceId = UpdatedItem.PlaceId;
            //Item.user = UpdatedItem.user; 
            //Item.place = UpdatedItem.place;

            await _context.SaveChangesAsync();

            var ResultDto = new ItemDTO
            {
                Id = Item.Id,
                Name = Item.Name,
                Url = Item.ImageUrl,
                Amount = Item.Quantity,
                ExpirationDate = Item.ExpirationDate,
                Category = Item.Category,
                Text = Item.Description,
                UserId = Item.UserId,
                PlaceId = Item.PlaceId
            };

            return ResultDto;
        }

        //deletes Item from db
        public async Task RemoveItem(int CurrentId)
        {
            ItemEntity Item = await _context.Item.FirstOrDefaultAsync(p => p.Id == CurrentId);

            if (Item == null)
                return;

            _context.Item.Remove(Item);
            await _context.SaveChangesAsync();
        }
    }
}