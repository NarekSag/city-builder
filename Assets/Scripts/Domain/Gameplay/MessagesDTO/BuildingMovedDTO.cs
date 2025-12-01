using Domain.Gameplay.Models;

namespace Domain.Gameplay.MessagesDTO
{
    public class BuildingMovedDTO
    {
        public int BuildingId { get; set; }
        public BuildingType BuildingType { get; set; }
        public GridPosition OldPosition { get; set; }
        public GridPosition NewPosition { get; set; }
    }
}

