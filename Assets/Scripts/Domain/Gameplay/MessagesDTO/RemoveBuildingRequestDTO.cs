using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    public class RemoveBuildingRequestDTO
    {
        public int BuildingId { get; set; }
        public GridPosition? Position { get; set; }
    }
}

