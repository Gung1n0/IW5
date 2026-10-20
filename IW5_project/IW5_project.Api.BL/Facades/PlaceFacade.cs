using IW5_project.Api.BL.Mappers;
using IW5_project.Api.Common;
using IW5_project.Api.Common.Models.Common;
using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.Common.Models.Place;

namespace IW5_project.Api.BL.Facades
{
    public class PlaceFacade(
        IPlaceRepository placeRepository,
        PlaceMapper mapper)
        : FacadeBase<IPlaceRepository, PlaceEntity>(placeRepository),
        IPlaceFacade
    {
        private readonly IPlaceRepository placeRepository = placeRepository;
        private readonly PlaceMapper mapper = mapper;

        public PagedResult<PlaceListModel> GetPaged(PlaceQuery query)
        {
            return placeRepository.GetPaged(query).Map(mapper.ToListModel);
        }

        public PlaceDetailModel? GetById(Guid id)
        {
            var placeEntity = placeRepository.GetById(id);
            return placeEntity is null ? null : mapper.ToDetailModel(placeEntity);
        }

        public Guid CreateOrUpdate(PlaceDetailModel placeModel, IList<string> userRoles, string? ownerId = null)
        {
            return placeRepository.Exists(placeModel.Id)
                ? Update(placeModel, userRoles, ownerId)!.Value
                : Create(placeModel, userRoles, ownerId);
        }

        public Guid Create(PlaceDetailModel placeModel, IList<string> userRoles, string? ownerId = null)
        {
            var placeEntity = mapper.ToEntity(placeModel, ownerId);
            return placeRepository.Insert(placeEntity);
        }

        public Guid? Update(PlaceDetailModel placeModel, IList<string> userRoles, string? ownerId = null)
        {
            if (!placeRepository.Exists(placeModel.Id))
            {
                return null;
            }

            ThrowIfWrongOwnerAndNotAdmin(placeModel.Id, userRoles, ownerId);

            var placeEntity = mapper.ToEntity(placeModel);    
            return placeRepository.Update(placeEntity);
        }

        public void Delete(Guid id, IList<string> userRoles, string? ownerId = null)
        {
            var place = placeRepository.GetById(id)
                ?? throw new NotFoundException($"Place {id} does not exist.");

            ThrowIfWrongOwnerAndNotAdmin(id, userRoles, ownerId);

            if (place.Items.Count > 0)
            {
                throw new ConflictException("The place still contains items. Move or delete them first.");
            }

            placeRepository.Remove(id);
        }
    }
}