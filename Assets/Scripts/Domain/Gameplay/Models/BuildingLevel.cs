using System;

namespace Domain.Gameplay.Models
{
    public class BuildingLevel
    {
        public int Level { get; }
        public Cost UpgradeCost { get; }
        public Income Income { get; }

        public BuildingLevel(int level, Cost upgradeCost, Income income)
        {
            if (level < 1 || level > 3)
            {
                throw new ArgumentException("Level must be between 1 and 3", nameof(level));
            }

            Level = level;
            UpgradeCost = upgradeCost;
            Income = income;
        }

        public static BuildingLevel CreateLevel1(BuildingType type)
        {
            var baseCost = GetBaseCost(type);
            var baseIncome = GetBaseIncome(type);

            var upgradeCost = new Cost(baseCost * 2, type);
            var income = new Income(baseIncome, type);

            return new BuildingLevel(1, upgradeCost, income);
        }

        public static BuildingLevel CreateLevel2(BuildingType type)
        {
            var baseCost = GetBaseCost(type);
            var baseIncome = GetBaseIncome(type);

            var upgradeCost = new Cost(baseCost * 3, type);
            var income = new Income(baseIncome * 2, type);

            return new BuildingLevel(2, upgradeCost, income);
        }

        public static BuildingLevel CreateLevel3(BuildingType type)
        {
            var baseCost = GetBaseCost(type);
            var baseIncome = GetBaseIncome(type);

            var upgradeCost = new Cost(baseCost * 4, type);
            var income = new Income(baseIncome * 3, type);

            return new BuildingLevel(3, upgradeCost, income);
        }

        private static int GetBaseCost(BuildingType type)
        {
            return type switch
            {
                BuildingType.House => 100,
                BuildingType.Farm => 150,
                BuildingType.Mine => 200,
                _ => throw new ArgumentException($"Unknown building type: {type}", nameof(type))
            };
        }

        private static int GetBaseIncome(BuildingType type)
        {
            return type switch
            {
                BuildingType.House => 10,
                BuildingType.Farm => 15,
                BuildingType.Mine => 25,
                _ => throw new ArgumentException($"Unknown building type: {type}", nameof(type))
            };
        }
    }
}

