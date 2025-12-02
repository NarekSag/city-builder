using System;
using System.Collections.Generic;

namespace Domain.Gameplay.MessagesDTO
{
    public class GameStateDTO
    {
        public int Gold { get; set; }
        public List<BuildingDataDTO> Buildings { get; set; }
        public DateTime SaveTimestamp { get; set; }
    }
}

