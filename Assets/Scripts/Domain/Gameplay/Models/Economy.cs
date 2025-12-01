using System;

namespace Domain.Gameplay.Models
{
    public class Economy
    {
        private int _gold;

        public int Gold => _gold;

        public Economy(int initialGold = 500)
        {
            if (initialGold < 0)
            {
                throw new ArgumentException("Initial gold must be greater than or equal to 0", nameof(initialGold));
            }

            _gold = initialGold;
        }

        public bool AddGold(int amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            _gold += amount;
            return true;
        }

        public bool SpendGold(int amount, out bool success)
        {
            success = false;

            if (amount <= 0)
            {
                return false;
            }

            if (!HasEnoughGold(amount))
            {
                return false;
            }

            _gold -= amount;
            success = true;
            return true;
        }

        public bool HasEnoughGold(int amount)
        {
            return amount > 0 && _gold >= amount;
        }
    }
}

