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

        public Building(int id, BuildingType type, int level, GridPosition position, Income income)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            if (level < 1 || level > 3)
            {
                throw new ArgumentException("Level must be between 1 and 3", nameof(level));
            }

            if (income == null)
            {
                throw new ArgumentNullException(nameof(income), "Income cannot be null");
            }

            Id = id;
            Type = type;
            Level = level;
            Position = position;
            CurrentIncome = income;
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

        public bool Upgrade(Income newIncome)
        {
            if (!CanUpgrade())
            {
                return false;
            }

            if (newIncome == null)
            {
                throw new ArgumentNullException(nameof(newIncome), "Income cannot be null");
            }

            Level++;
            CurrentIncome = newIncome;
            return true;
        }
    }
}

