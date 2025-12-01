using Domain.Gameplay.Models;
using UnityEngine;

namespace ContractsInterfaces.Infrastructure
{
    public interface IBuildingPrefabFactory
    {
        GameObject CreateBuildingPrefab(BuildingType buildingType, Transform parent);
        GameObject GetBuildingPrefab(BuildingType buildingType);
    }
}

