using Domain.Gameplay.MessagesDTO;

namespace ContractsInterfaces.UseCasesGameplay
{
    public interface IRemoveBuildingUseCase
    {
        void RemoveBuilding(RemoveBuildingRequestDTO request);
    }
}

