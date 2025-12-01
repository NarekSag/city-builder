using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    public class BuildingPlacedDTO
    {
        public int BuildingId { get; set; }
        public BuildingType BuildingType { get; set; }
        public GridPosition Position { get; set; }
    }
}

