using Domain.Gameplay.MessagesDTO;

namespace ContractsInterfaces.UseCasesGameplay
{
    public interface ISaveGameUseCase
    {
        void SaveGame(SaveGameRequestDTO request);
    }
}

