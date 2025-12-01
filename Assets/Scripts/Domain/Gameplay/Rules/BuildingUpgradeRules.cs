using System;

namespace Domain.Gameplay.Rules
{
    public static class BuildingUpgradeRules
    {
        private const int HouseBaseIncome = 10;
        private const int FarmBaseIncome = 15;
        private const int MineBaseIncome = 25;

        public static bool CanUpgrade(Models.Building building, int maxLevel = 3)
        {
            if (building == null)
            {
                return false;
            }

            return building.CanUpgrade(maxLevel);
        }

        public static int GetUpgradeCost(Models.BuildingType type, int currentLevel)
        {
            return BuildingCostCalculator.GetUpgradeCost(type, currentLevel);
        }

        public static Models.Income GetUpgradeIncome(Models.BuildingType type, int newLevel)
        {
            if (newLevel < 1 || newLevel > 3)
            {
                throw new ArgumentException("New level must be between 1 and 3", nameof(newLevel));
            }

            var baseIncome = GetBaseIncome(type);
            var incomeAmount = baseIncome * newLevel;

            return new Models.Income(incomeAmount, type);
        }

        private static int GetBaseIncome(Models.BuildingType type)
        {
            return type switch
            {
                Models.BuildingType.House => HouseBaseIncome,
                Models.BuildingType.Farm => FarmBaseIncome,
                Models.BuildingType.Mine => MineBaseIncome,
                _ => throw new ArgumentException($"Unknown building type: {type}", nameof(type))
            };
        }
    }
}

