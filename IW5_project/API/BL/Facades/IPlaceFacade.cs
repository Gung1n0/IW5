using IW5_project.API.Common;
using IW5_project.Common.BL.Facades;
using IW5_project.Common.Models.Place;

namespace IW5_project.API.BL.Facades
{
    public interface IPlaceFacade : IAppFacade
    {
        PagedResult<PlaceListModel> GetPaged(PlaceQuery query);
        PlaceDetailModel? GetById(Guid id);
        Guid CreateOrUpdate(PlaceDetailModel placeModel, IList<string> userRoles, string? ownerId = null);
        Guid Create(PlaceDetailModel placeModel, IList<string> userRoles, string? ownerId = null);
        Guid? Update(PlaceDetailModel placeModel, IList<string> userRoles, string? ownerId = null);
        void Delete(Guid id, IList<string> userRoles, string? ownerId = null);
    }
}