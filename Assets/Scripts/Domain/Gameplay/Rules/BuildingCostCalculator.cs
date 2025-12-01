using System;

namespace Domain.Gameplay.Rules
{
    public static class BuildingCostCalculator
    {
        private const int HouseBaseCost = 100;
        private const int FarmBaseCost = 150;
        private const int MineBaseCost = 200;

        public static int GetCost(Models.BuildingType type, int level)
        {
            if (level < 1 || level > 3)
            {
                throw new ArgumentException("Level must be between 1 and 3", nameof(level));
            }

            var baseCost = GetBaseCost(type);
            return baseCost * level;
        }

        public static int GetUpgradeCost(Models.BuildingType type, int currentLevel)
        {
            if (currentLevel < 1 || currentLevel >= 3)
            {
                throw new ArgumentException("Current level must be between 1 and 2 for upgrade", nameof(currentLevel));
            }

            var baseCost = GetBaseCost(type);
            return baseCost * (currentLevel + 1);
        }

        private static int GetBaseCost(Models.BuildingType type)
        {
            return type switch
            {
                Models.BuildingType.House => HouseBaseCost,
                Models.BuildingType.Farm => FarmBaseCost,
                Models.BuildingType.Mine => MineBaseCost,
                _ => throw new ArgumentException($"Unknown building type: {type}", nameof(type))
            };
        }
    }
}

