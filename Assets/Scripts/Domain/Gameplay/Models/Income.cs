using System;

namespace Domain.Gameplay.Models
{
    public class Income
    {
        public int AmountPerMinute { get; }
        public BuildingType Type { get; }

        public Income(int amountPerMinute, BuildingType type)
        {
            if (amountPerMinute < 0)
            {
                throw new ArgumentException("AmountPerMinute must be greater than or equal to 0", nameof(amountPerMinute));
            }

            AmountPerMinute = amountPerMinute;
            Type = type;
        }
    }
}

