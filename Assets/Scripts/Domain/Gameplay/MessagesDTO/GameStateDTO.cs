using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Gameplay.MessagesDTO
{
    [Serializable]
    public class GameStateDTO
    {
        public int Gold;
        public BuildingDataList Buildings;
        public string SaveTimestamp; // ISO 8601 format string

        // Helper property for easier access
        public List<BuildingDataDTO> GetBuildingsList()
        {
            if (Buildings?.Items == null)
            {
                return new List<BuildingDataDTO>();
            }
            return new List<BuildingDataDTO>(Buildings.Items);
        }

        public void SetBuildingsList(List<BuildingDataDTO> buildings)
        {
            if (Buildings == null)
            {
                Buildings = new BuildingDataList();
            }
            Buildings.Items = buildings?.ToArray() ?? new BuildingDataDTO[0];
        }
    }

    [Serializable]
    public class BuildingDataList
    {
        public BuildingDataDTO[] Items;
    }
}

