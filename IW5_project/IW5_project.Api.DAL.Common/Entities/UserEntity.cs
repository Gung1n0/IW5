namespace IW5_project.API.DAL.Common.Entities;

public record UserEntity : EntityBase
{
    public required string Name { get; set; }
    public string? PhotoUrl { get; set; }
    public required string Role { get; set; } // USER or ADMIN
}