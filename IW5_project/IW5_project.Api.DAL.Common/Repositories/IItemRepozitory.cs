/*using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.DAL.EF;
using IW5_project.API.BL.Mappers;
using IW5_project.API.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace IW5_project.Api.DAL.Common.Repositories
{
    public class ItemService
    {
        private readonly DatabaseContext _context;
        private readonly TransferObjects _transferObjects;

        public ItemService(DatabaseContext context)
        {
            _context = context;
        }

        //create Item
        public async Task<int> CreateItemAsync(CreateItemDTO Dto, int CurrentUserId)
        {
            var Entity = new ItemEntity
            {
                Id = _context.Items.Count() + 1,
                Name = Dto.Name,
                ImageUrl = Dto.Url,
                Quantity = Dto.Amount,
                ExpirationDate = Dto.ExpirationDate,
                Category = Dto.Category,
                Description = Dto.Text,
                PlaceId = Dto.PlaceId,
                UserId = CurrentUserId
            };

            _context.Items.Add(Entity);

            await _context.SaveChangesAsync();

            return Entity.Id;
        }

        //get all Items
        public async Task<List<ItemDTO>> GetAllItems()
        {
            return await _context.Items.Select(Entity => new ItemDTO
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
            ItemEntity Item = await _context.Items.FirstOrDefaultAsync(p => p.Id == CurrentId);

            if (Item == null)
                return null;

           return _transferObjects.EntityToDTO(Item);
        }

        //updates existing Item
        public async Task<ItemDTO> UpdateItem(int CurrentId, ItemEntity UpdatedItem)
        {
            ItemEntity Item = await _context.Items.FirstOrDefaultAsync(p => p.Id == CurrentId);

            if (Item == null)
                return null;

            Item.Name = UpdatedItem.Name;
            Item.ImageUrl = UpdatedItem.ImageUrl;
            Item.Quantity = UpdatedItem.Quantity;
            Item.ExpirationDate = UpdatedItem.ExpirationDate;
            Item.Category = UpdatedItem.Category;
            Item.Description = UpdatedItem.Description;
            Item.user = UpdatedItem.user; 
            Item.place = UpdatedItem.place;

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
            };

            return ResultDto;
        }

        //deletes Item from db
        public async Task RemoveItem(int CurrentId)
        {
            ItemEntity Item = await _context.Items.FirstOrDefaultAsync(p => p.Id == CurrentId);

            if (Item == null)
                return;

            _context.Items.Remove(Item);
            await _context.SaveChangesAsync();
        }
    }
}*/