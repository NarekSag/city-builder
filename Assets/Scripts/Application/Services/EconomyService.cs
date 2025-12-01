using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.Models;

namespace Application.Services
{
    public class EconomyService : IEconomyService
    {
        private readonly Economy _economy;

        public EconomyService(Economy economy)
        {
            _economy = economy;
        }

        public int GetGold()
        {
            return _economy.Gold;
        }

        public bool AddGold(int amount)
        {
            return _economy.AddGold(amount);
        }

        public bool SpendGold(int amount, out bool success)
        {
            return _economy.SpendGold(amount, out success);
        }

        public bool HasEnoughGold(int amount)
        {
            return _economy.HasEnoughGold(amount);
        }
    }
}

