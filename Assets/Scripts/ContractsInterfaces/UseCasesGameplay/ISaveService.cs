using Domain.Gameplay.MessagesDTO;

namespace ContractsInterfaces.UseCasesGameplay
{
    public interface ISaveService
    {
        bool Save(GameStateDTO gameState, string filePath);
    }
}

