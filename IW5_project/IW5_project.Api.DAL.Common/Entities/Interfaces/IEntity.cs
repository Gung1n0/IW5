using IW5_project.Common.Models;

namespace IW5_project.Api.DAL.Common.Entities.Interfaces
{
    public interface IEntity : IWithId
    {
        string? OwnerId { get; set; }
    }
}
