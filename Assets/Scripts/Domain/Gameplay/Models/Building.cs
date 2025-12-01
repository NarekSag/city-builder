using System;

namespace Domain.Gameplay.Models
{
    public class Building
    {
        public int Id { get; }
        public BuildingType Type { get; }
        public int Level { get; private set; }
        public GridPosition Position { get; set; }
        public Income CurrentIncome { get; private set; }

        public Building(int id, BuildingType type, int level, GridPosition position)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            if (level < 1 || level > 3)
            {
                throw new ArgumentException("Level must be between 1 and 3", nameof(level));
            }

            Id = id;
            Type = type;
            Level = level;
            Position = position;
            CurrentIncome = CalculateIncome(type, level);
        }

        public bool IsValid()
        {
            return Id > 0 && Level >= 1 && Level <= 3;
        }

        public bool CanUpgrade(int maxLevel = 3)
        {
            return Level < maxLevel;
        }

        public Income GetCurrentIncome()
        {
            return CurrentIncome;
        }

        public bool Upgrade()
        {
            if (!CanUpgrade())
            {
                return false;
            }

            Level++;
            CurrentIncome = CalculateIncome(Type, Level);
            return true;
        }

        private static Income CalculateIncome(BuildingType type, int level)
        {
            var baseIncome = type switch
            {
                BuildingType.House => 10,
                BuildingType.Farm => 15,
                BuildingType.Mine => 25,
                _ => throw new ArgumentException($"Unknown building type: {type}", nameof(type))
            };

            var incomeAmount = baseIncome * level;
            return new Income(incomeAmount, type);
        }
    }
}

