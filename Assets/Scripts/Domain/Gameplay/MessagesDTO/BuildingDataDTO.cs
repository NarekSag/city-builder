using System;
using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    [Serializable]
    public class BuildingDataDTO
    {
        public int Id;
        public BuildingType Type;
        public int Level;
        public int PositionX;
        public int PositionY;
        public int IncomeAmountPerTick;
    }
}

