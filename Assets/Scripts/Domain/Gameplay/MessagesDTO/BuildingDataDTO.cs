using System.Collections.Generic;
using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    public class BuildingDataDTO
    {
        public int Id { get; set; }
        public BuildingType Type { get; set; }
        public int Level { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int IncomeAmountPerTick { get; set; }
    }
}

