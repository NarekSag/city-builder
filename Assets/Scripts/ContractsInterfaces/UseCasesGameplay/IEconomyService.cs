namespace ContractsInterfaces.UseCasesGameplay
{
    public interface IEconomyService
    {
        void Initialize();
        int GetGold();
        bool AddGold(int amount);
        bool SpendGold(int amount, out bool success);
        bool HasEnoughGold(int amount);
    }
}

