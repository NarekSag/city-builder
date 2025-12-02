using System;
using ContractsInterfaces.Infrastructure;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
using Presentation.Gameplay.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Grid = Domain.Gameplay.Models.Grid;

namespace Presentation.Gameplay.Presenters
{
    public class GridPresenter : IInitializable, IDisposable
    {
        [Inject] private Grid _grid;
        [Inject] private GridView _view;
        [Inject] private IPublisher<PlaceBuildingRequestDTO> _placeBuildingPublisher;
        [Inject] private IPublisher<BuildingSelectedDTO> _buildingSelectedPublisher;
        [Inject] private IInputAdapter _inputAdapter;

        private BuildingType _selectedBuildingType = BuildingType.House;

        public void Initialize()
        {
            _view.OnWorldPositionHovered += HandleWorldPositionHovered;
            _view.OnHoverExit += HandleHoverExit;
            _view.OnWorldPositionClicked += HandleWorldPositionClicked;
        }

        private void CheckHotkeys()
        {
            if (_inputAdapter == null)
            {
                return;
            }

            if (_inputAdapter.IsBuilding1Pressed())
            {
                _selectedBuildingType = BuildingType.House;
            }
            else if (_inputAdapter.IsBuilding2Pressed())
            {
                _selectedBuildingType = BuildingType.Farm;
            }
            else if (_inputAdapter.IsBuilding3Pressed())
            {
                _selectedBuildingType = BuildingType.Mine;
            }
        }

        private void HandleWorldPositionHovered(Vector3 worldPosition)
        {
            CheckHotkeys();
            
            var gridPosition = WorldToGridPosition(worldPosition);
            if (!gridPosition.HasValue)
            {
                return;
            }

            var position = gridPosition.Value;
            Color color;
            if (!_grid.IsValidPosition(position) || _grid.IsOccupied(position))
            {
                color = Color.red;
            }
            else
            {
                color = Color.green;
            }

            _view.HighlightCell(position, color);
        }

        private void HandleHoverExit()
        {
            _view.ClearHighlight();
        }

        private void HandleWorldPositionClicked(Vector3 worldPosition)
        {
            CheckHotkeys();
            
            var gridPosition = WorldToGridPosition(worldPosition);
            if (!gridPosition.HasValue)
            {
                return;
            }

            var position = gridPosition.Value;
            if (!_grid.IsValidPosition(position))
            {
                return;
            }

            // Check if there's a building at this position
            if (_grid.GetBuildingAt(position, out var building))
            {
                // Clicked on existing building - select it
                var selectedDto = new BuildingSelectedDTO
                {
                    BuildingId = building.Id
                };
                _buildingSelectedPublisher.Publish(selectedDto);
                return;
            }

            // Empty cell - place new building
            var gridPos = new GridPosition(position);
            var request = new PlaceBuildingRequestDTO
            {
                BuildingType = _selectedBuildingType,
                Position = gridPos
            };

            _placeBuildingPublisher.Publish(request);
        }

        private Vector2Int? WorldToGridPosition(Vector3 worldPosition)
        {
            if (_grid == null) return null;

            int gx = Mathf.RoundToInt(worldPosition.x + (_grid.Width - 1) * 0.5f);
            int gy = Mathf.RoundToInt(worldPosition.z + (_grid.Height - 1) * 0.5f);

            return new Vector2Int(gx, gy);
        }

        public void Dispose()
        {
            _view.OnWorldPositionHovered -= HandleWorldPositionHovered;
            _view.OnHoverExit -= HandleHoverExit;
            _view.OnWorldPositionClicked -= HandleWorldPositionClicked;
        }
    }
}

