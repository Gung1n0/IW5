using IW5_project.Api.DAL.Common.Entities.Interfaces;

namespace IW5_project.Api.DAL.Common.Entities;

public abstract record EntityBase : IEntity
{
    public required Guid Id { get; set; }
    public string? OwnerId { get; set; }
}
