using System;
using System.Collections.Generic;

namespace Domain.Gameplay.MessagesDTO
{
    [Serializable]
    public class GameStateDTO
    {
        public int Gold { get; set; }
        public List<BuildingDataDTO> Buildings { get; set; }
        public string SaveTimestamp { get; set; } // ISO 8601 format string
    }
}

