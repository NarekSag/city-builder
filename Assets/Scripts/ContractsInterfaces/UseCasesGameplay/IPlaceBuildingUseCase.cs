using Domain.Gameplay.MessagesDTO;

namespace ContractsInterfaces.UseCasesGameplay
{
    public interface IPlaceBuildingUseCase
    {
        void PlaceBuilding(PlaceBuildingRequestDTO request);
    }
}

