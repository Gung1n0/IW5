using IW5_project.Api.DAL.Common.Entities.Interfaces;
using System.Security.Principal;

namespace IW5_project.Api.DAL.Common.Entities;

public abstract record EntityBase : IEntity
{
    public required Guid Id { get; init; }
    public string? OwnerId { get; set; }
}