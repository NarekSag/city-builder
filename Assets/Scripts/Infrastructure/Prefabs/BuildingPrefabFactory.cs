using ContractsInterfaces.Infrastructure;
using Domain.Gameplay.Models;
using UnityEngine;

namespace Infrastructure.Prefabs
{
    public class BuildingPrefabFactory : IBuildingPrefabFactory
    {
        private readonly GameObject _housePrefab;

        public BuildingPrefabFactory(GameObject housePrefab)
        {
            _housePrefab = housePrefab;
        }

        public GameObject CreateBuildingPrefab(BuildingType buildingType, Transform parent)
        {
            var prefab = GetBuildingPrefab(buildingType);
            if (prefab == null)
            {
                Debug.LogWarning($"[BuildingPrefabFactory] Prefab for {buildingType} is null. Creating fallback cube.");
                return CreateFallbackBuilding(buildingType, parent);
            }

            return Object.Instantiate(prefab, parent);
        }

        public GameObject GetBuildingPrefab(BuildingType buildingType)
        {
            return buildingType switch
            {
                BuildingType.House => _housePrefab,
                BuildingType.Farm => null,
                BuildingType.Mine => null,
                _ => null
            };
        }

        private GameObject CreateFallbackBuilding(BuildingType buildingType, Transform parent)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = $"{buildingType}_Fallback";
            cube.transform.SetParent(parent);
            
            var renderer = cube.GetComponent<Renderer>();
            renderer.material.color = GetBuildingColor(buildingType);
            
            return cube;
        }

        private Color GetBuildingColor(BuildingType buildingType)
        {
            return buildingType switch
            {
                BuildingType.House => Color.blue,
                BuildingType.Farm => Color.green,
                BuildingType.Mine => Color.gray,
                _ => Color.white
            };
        }
    }
}

