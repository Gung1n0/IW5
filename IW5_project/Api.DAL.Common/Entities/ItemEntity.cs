namespace IW5_project.Api.DAL.Common.Entities
{
    public enum ItemCategories
    {
        Potraviny,
        Drogerie 
    }
    public class ItemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public float Quantity { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public ItemCategories Category { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; } //connection to user
        //  public User user { get; set; } = null!
        public int PlaceId { get; set; } // connection to place
        //public Place place {get; set;} = null!
    }
}
