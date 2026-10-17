using IW5_project.Common;

namespace IW5_project.API.DAL.Common.Entities.Interfaces
{
    public interface IEntity : IWithId
    {
        string? OwnerId { get; set; }
    }
}
