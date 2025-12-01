using System;
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

        private BuildingType _selectedBuildingType = BuildingType.House;

        public void Initialize()
        {
            _view.OnCellHovered += HandleCellHovered;
            _view.OnCellHoverExit += HandleCellHoverExit;
            _view.OnCellClicked += HandleCellClicked;

            Debug.Log("[GridPresenter] Initialized");
        }

        public void SetSelectedBuildingType(BuildingType buildingType)
        {
            _selectedBuildingType = buildingType;
        }

        private void HandleCellHovered(Vector2Int position)
        {
            if (_grid == null) return;

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

        private void HandleCellHoverExit()
        {
            _view.ClearHighlight();
        }

        private void HandleCellClicked(Vector2Int position)
        {
            if (!_grid.IsValidPosition(position))
            {
                Debug.LogWarning($"[GridPresenter] Invalid position clicked: {position}");
                return;
            }

            var gridPosition = new GridPosition(position);
            var request = new PlaceBuildingRequestDTO
            {
                BuildingType = _selectedBuildingType,
                Position = gridPosition
            };

            Debug.Log($"[GridPresenter] Publishing PlaceBuildingRequestDTO: Type={_selectedBuildingType}, Position={gridPosition}");
            _placeBuildingPublisher.Publish(request);
        }

        public void Dispose()
        {
            _view.OnCellHovered -= HandleCellHovered;
            _view.OnCellHoverExit -= HandleCellHoverExit;
            _view.OnCellClicked -= HandleCellClicked;
        }
    }
}

