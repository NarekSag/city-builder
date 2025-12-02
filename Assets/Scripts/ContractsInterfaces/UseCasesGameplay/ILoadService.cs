using Domain.Gameplay.MessagesDTO;

namespace ContractsInterfaces.UseCasesGameplay
{
    public interface ILoadService
    {
        GameStateDTO? Load(string filePath);
        bool SaveFileExists(string filePath);
        string GetDefaultSavePath();
    }
}

