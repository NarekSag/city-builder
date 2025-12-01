using Domain.Gameplay.MessagesDTO;

namespace ContractsInterfaces.UseCasesGameplay
{
    public interface IUpgradeBuildingUseCase
    {
        void UpgradeBuilding(UpgradeBuildingRequestDTO request);
    }
}

