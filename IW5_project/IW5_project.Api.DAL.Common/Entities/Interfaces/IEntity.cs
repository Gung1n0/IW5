namespace IW5_project.Api.DAL.Common.Entities.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
        string? OwnerId { get; set; }
    }
}
