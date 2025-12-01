using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    public class PlaceBuildingRequestDTO
    {
        public BuildingType BuildingType { get; set; }
        public GridPosition Position { get; set; }
    }
}

