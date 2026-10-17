using IW5_project.IW5_project.Common;

namespace IW5_project.IW5_project.Api.DAL.Common.Entities.Interfaces
{
    public interface IEntity : IWithId
    {
        string? OwnerId { get; set; }
    }
}
