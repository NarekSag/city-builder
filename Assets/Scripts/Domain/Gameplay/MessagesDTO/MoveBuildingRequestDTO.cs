using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    public class MoveBuildingRequestDTO
    {
        public int BuildingId { get; set; }
        public GridPosition NewPosition { get; set; }
    }
}

