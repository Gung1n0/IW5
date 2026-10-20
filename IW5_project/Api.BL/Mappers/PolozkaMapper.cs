using IW5_project.Api.DAL.Common.Entities;
using IW5_project.API.Common.Models;
namespace IW5_project.API.BL.Mappers
{
    public class TransferObjects
    {
        public ItemDTO EntityToDTO(ItemEntity Entity)
        {
            ItemDTO DTO = new ItemDTO
            {
                Id = Entity.Id,
                Name = Entity.Name,
                Text = Entity.Description,
                Url = Entity.ImageUrl,
                PlaceId = Entity.PlaceId,
                Amount = Entity.Quantity,
                Category = Entity.Category,
                ExpirationDate = Entity.ExpirationDate,
                UserId = Entity.UserId
            };
            return DTO;
        }

        public ItemEntity DTOToEntity(ItemDTO DTO)
        {
            ItemEntity Entity = new ItemEntity
            {
                Id = DTO.Id,
                Name = DTO.Name,
                Description = DTO.Text,
                ImageUrl = DTO.Url,
                PlaceId = DTO.PlaceId,
                Quantity = DTO.Amount,
                Category = DTO.Category,
                ExpirationDate = DTO.ExpirationDate,
                UserId = DTO.UserId
            };
            return Entity;
        }

        public ItemEntity CreateDTOToEntity(CreateItemDTO DTO)
        {
            ItemEntity Entity = new ItemEntity
            {
                Name = DTO.Name,
                Description = DTO.Text,
                ImageUrl = DTO.Url,
                PlaceId = DTO.PlaceId,
                Quantity = DTO.Amount,
                Category = DTO.Category,
                ExpirationDate = DTO.ExpirationDate,
                UserId = DTO.UserId
            };
            return Entity;
        }

        public CreateItemDTO EntityToCreateDTO(ItemEntity Entity)
        {
            CreateItemDTO DTO = new CreateItemDTO
            {
                Name = Entity.Name,
                Text = Entity.Description,
                Url = Entity.ImageUrl,
                PlaceId = Entity.PlaceId,
                Amount = Entity.Quantity,
                Category = Entity.Category,
                ExpirationDate = Entity.ExpirationDate,
                UserId = Entity.UserId
            };
            return DTO;
        }
    }
}