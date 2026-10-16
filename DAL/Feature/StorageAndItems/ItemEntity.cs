using System.Collections.IENumerable;

namespace DAL
{
    public enum TypeOfCategory
    {
        Potraviny,
        drogeria
    }
    public class PolozkaEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public float Amount { get; set; }
        public DateTime? DateExpiration { get; set; }
        public TypeOfCategory Category { get; set; }
        public string? Text { get; set; }
        public int UserId { get; set; } //connection to user
        //  public User user { get; set; } = null!
        public int PlaceId { get; set; } // connection to place
        //public Place place {get; set;} = null!
    }

    public class PolozkaDTO
        {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public float Amount { get; set; }
        public DateTime? DateExpiration { get; set; }
        public TypeOfCategory Category { get; set; }
        public string? Text { get; set; }
        public int UserId { get; set; } //connection to user
        public int PlaceId { get; set; } // connection to place
    }

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
    }
}
