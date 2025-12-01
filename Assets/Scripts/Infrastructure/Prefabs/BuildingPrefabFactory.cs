using ContractsInterfaces.Infrastructure;
using Domain.Gameplay.Models;
using UnityEngine;

namespace Infrastructure.Prefabs
{
    public class BuildingPrefabFactory : IBuildingPrefabFactory
    {
        private readonly GameObject _buildingPrefab;

        public BuildingPrefabFactory(GameObject buildingPrefab)
        {
            _buildingPrefab = buildingPrefab;
        }

        public GameObject CreateBuildingPrefab(BuildingType buildingType, Transform parent)
        {
            if (_buildingPrefab == null)
            {
                Debug.LogWarning($"[BuildingPrefabFactory] Building prefab is null. Creating fallback cube.");
                return CreateFallbackBuilding(buildingType, parent);
            }

            var instance = Object.Instantiate(_buildingPrefab, parent);
            ApplyBuildingColor(instance, buildingType);
            return instance;
        }

        public GameObject GetBuildingPrefab(BuildingType buildingType)
        {
            return _buildingPrefab;
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

        private void ApplyBuildingColor(GameObject buildingInstance, BuildingType buildingType)
        {
            var renderer = buildingInstance.GetComponent<Renderer>();
            if (renderer == null)
            {
                renderer = buildingInstance.GetComponentInChildren<Renderer>();
            }

            if (renderer != null)
            {
                var material = renderer.material;
                material.color = GetBuildingColor(buildingType);
            }
        }

        private Color GetBuildingColor(BuildingType buildingType)
        {
            return buildingType switch
            {
                BuildingType.House => Color.blue,
                BuildingType.Farm => Color.yellow,
                BuildingType.Mine => Color.gray,
                _ => Color.white
            };
        }
    }
}

