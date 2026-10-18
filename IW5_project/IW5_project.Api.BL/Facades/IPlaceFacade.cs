using IW5_project.Common.Models;
using IW5_project.Common.Models.Common;
using IW5_project.Common.Models.Place;

namespace IW5_project.Api.BL.Facades
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