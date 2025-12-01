using System.Collections.Generic;
using Domain.Gameplay.Models;
using UnityEngine;
using VContainer;
using Grid = Domain.Gameplay.Models.Grid;

namespace Presentation.Gameplay.Views
{
    public class BuildingView : MonoBehaviour
    {
        [Inject] private Grid _grid;

        private Dictionary<int, GameObject> _buildingGameObjects = new();
        private Transform _buildingsParent;

        public void Initialize(Transform buildingsParent)
        {
            _buildingsParent = buildingsParent;
        }

        public void CreateBuildingVisual(int buildingId, BuildingType buildingType, GridPosition position, GameObject prefabInstance)
        {
            if (_buildingGameObjects.ContainsKey(buildingId))
            {
                Debug.LogWarning($"[BuildingView] Building {buildingId} already has visual representation.");
                return;
            }

            var worldPosition = GridPositionToWorld(position);
            prefabInstance.transform.position = worldPosition;
            prefabInstance.transform.SetParent(_buildingsParent);
            prefabInstance.name = $"{buildingType}_{buildingId}";

            _buildingGameObjects[buildingId] = prefabInstance;
        }

        public void RemoveBuildingVisual(int buildingId)
        {
            if (_buildingGameObjects.TryGetValue(buildingId, out var gameObject))
            {
                Object.Destroy(gameObject);
                _buildingGameObjects.Remove(buildingId);
            }
        }

        public void MoveBuildingVisual(int buildingId, GridPosition newPosition)
        {
            if (_buildingGameObjects.TryGetValue(buildingId, out var gameObject))
            {
                var worldPosition = GridPositionToWorld(newPosition);
                gameObject.transform.position = worldPosition;
            }
        }

        private Vector3 GridPositionToWorld(GridPosition position)
        {
            if (_grid == null) return Vector3.zero;

            var wx = position.X - (_grid.Width - 1) * 0.5f;
            var wz = position.Y - (_grid.Height - 1) * 0.5f;
            return new Vector3(wx, 0.5f, wz);
        }
    }
}

