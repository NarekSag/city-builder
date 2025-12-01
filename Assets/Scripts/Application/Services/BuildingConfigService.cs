using System;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.Models;
using Repositories.Gameplay;
using VContainer;

namespace Application.Services
{
    public class BuildingConfigService : IBuildingConfigService
    {
        [Inject] private EconomySettings _economySettings;

        public int GetCost(BuildingType type, int level)
        {
            if (level < 1 || level > 3)
            {
                throw new ArgumentException("Level must be between 1 and 3", nameof(level));
            }

            var baseCost = GetBaseCost(type);
            return baseCost * level;
        }

        public int GetUpgradeCost(BuildingType type, int currentLevel)
        {
            if (currentLevel < 1 || currentLevel >= 3)
            {
                throw new ArgumentException("Current level must be between 1 and 2 for upgrade", nameof(currentLevel));
            }

            var baseCost = GetBaseCost(type);
            return baseCost * (currentLevel + 1);
        }

        public Income CalculateIncome(BuildingType type, int level)
        {
            if (level < 1 || level > 3)
            {
                throw new ArgumentException("Level must be between 1 and 3", nameof(level));
            }

            var baseIncomePerTick = GetBaseIncomePerTick(type);
            var incomeAmount = baseIncomePerTick * level;
            return new Income(incomeAmount, type);
        }

        private int GetBaseCost(BuildingType type)
        {
            if (_economySettings == null)
            {
                return GetDefaultBaseCost(type);
            }

            return type switch
            {
                BuildingType.House => _economySettings.HouseBaseCost,
                BuildingType.Farm => _economySettings.FarmBaseCost,
                BuildingType.Mine => _economySettings.MineBaseCost,
                _ => throw new ArgumentException($"Unknown building type: {type}", nameof(type))
            };
        }

        private int GetBaseIncomePerTick(BuildingType type)
        {
            if (_economySettings == null)
            {
                return GetDefaultBaseIncomePerTick(type);
            }

            return type switch
            {
                BuildingType.House => _economySettings.HouseBaseIncomePerTick,
                BuildingType.Farm => _economySettings.FarmBaseIncomePerTick,
                BuildingType.Mine => _economySettings.MineBaseIncomePerTick,
                _ => throw new ArgumentException($"Unknown building type: {type}", nameof(type))
            };
        }

        private static int GetDefaultBaseCost(BuildingType type)
        {
            return type switch
            {
                BuildingType.House => 100,
                BuildingType.Farm => 150,
                BuildingType.Mine => 200,
                _ => throw new ArgumentException($"Unknown building type: {type}", nameof(type))
            };
        }

        private static int GetDefaultBaseIncomePerTick(BuildingType type)
        {
            return type switch
            {
                BuildingType.House => 1,
                BuildingType.Farm => 3,
                BuildingType.Mine => 5,
                _ => throw new ArgumentException($"Unknown building type: {type}", nameof(type))
            };
        }
    }
}

