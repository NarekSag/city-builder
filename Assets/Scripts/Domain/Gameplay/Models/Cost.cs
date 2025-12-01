using System;

namespace Domain.Gameplay.Models
{
    public class Cost
    {
        public int Amount { get; }
        public BuildingType Type { get; }

        public Cost(int amount, BuildingType type)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than 0", nameof(amount));
            }

            Amount = amount;
            Type = type;
        }
    }
}

