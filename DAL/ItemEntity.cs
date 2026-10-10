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

    public class CreatePolozkaDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public float Amount { get; set; }
        public DateTime? DateExpiration { get; set; }
        public TypeOfCategory Category { get; set; }
        public string? Text { get; set; }
        public int PlaceId { get; set; } // connection to place
        //public Place place {get; set;} = null!
    }
}
