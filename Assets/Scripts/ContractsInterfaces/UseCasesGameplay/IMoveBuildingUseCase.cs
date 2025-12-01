using Domain.Gameplay.MessagesDTO;

namespace ContractsInterfaces.UseCasesGameplay
{
    public interface IMoveBuildingUseCase
    {
        void MoveBuilding(MoveBuildingRequestDTO request);
    }
}

