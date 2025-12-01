using Domain.Gameplay.Models;

namespace ContractsInterfaces.UseCasesGameplay
{
    public interface IBuildingConfigService
    {
        int GetCost(BuildingType type, int level);
        int GetUpgradeCost(BuildingType type, int currentLevel);
        Income CalculateIncome(BuildingType type, int level);
    }
}

