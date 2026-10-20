using DAL.Models;
namespace DAL.Mappers
{
    public class TransferObjects
    {
        public PolozkaDTO EntityToDTO(PolozkaEntity Entity)
        {
            PolozkaDTO DTO = new PolozkaDTO
            {
                Id = Entity.Id,
                Name = Entity.Name,
                Text = Entity.Text,
                Url = Entity.Url,
                PlaceId = Entity.PlaceId,
                Amount = Entity.Amount,
                Category = Entity.Category,
                DateExpiration = Entity.DateExpiration,
                UserId = Entity.UserId
            };
            return DTO;
        }

        public PolozkaEntity DTOToEntity(PolozkaDTO DTO)
        {
            PolozkaEntity Entity = new PolozkaEntity
            {
                Id = DTO.Id,
                Name = DTO.Name,
                Text = DTO.Text,
                Url = DTO.Url,
                PlaceId = DTO.PlaceId,
                Amount = DTO.Amount,
                Category = DTO.Category,
                DateExpiration = DTO.DateExpiration,
                UserId = DTO.UserId
            };
            return Entity;
        }

        public PolozkaEntity CreateDTOToEntity(CreatePolozkaDTO DTO)
        {
            PolozkaEntity Entity = new PolozkaEntity
            {
                Name = DTO.Name,
                Text = DTO.Text,
                Url = DTO.Url,
                PlaceId = DTO.PlaceId,
                Amount = DTO.Amount,
                Category = DTO.Category,
                DateExpiration = DTO.DateExpiration,
                UserId = DTO.UserId
            };
            return Entity;
        }

        public CreatePolozkaDTO EntityToCreateDTO(PolozkaEntity Entity)
        {
            CreatePolozkaDTO DTO = new CreatePolozkaDTO
            {
                Name = Entity.Name,
                Text = Entity.Text,
                Url = Entity.Url,
                PlaceId = Entity.PlaceId,
                Amount = Entity.Amount,
                Category = Entity.Category,
                DateExpiration = Entity.DateExpiration,
                UserId = Entity.UserId
            };
            return DTO;
        }
    }
}