using System;

namespace Domain.Gameplay.Models
{
    public class Income
    {
        public int AmountPerTick { get; }
        public BuildingType Type { get; }

        public Income(int amountPerTick, BuildingType type)
        {
            if (amountPerTick < 0)
            {
                throw new ArgumentException("AmountPerTick must be greater than or equal to 0", nameof(amountPerTick));
            }

            AmountPerTick = amountPerTick;
            Type = type;
        }
    }
}

