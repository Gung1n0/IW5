using IW5_project.API.DAL.Common.Entities.Interfaces;

namespace IW5_project.API.DAL.Common.Entities
{
    public abstract record EntityBase : IEntity
    {
        public required Guid Id { get; init; }
        public string? OwnerId { get; set; }
    }
}
