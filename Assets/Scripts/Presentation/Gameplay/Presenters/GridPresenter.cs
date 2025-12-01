using System;
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

        public void Initialize()
        {
            _view.OnCellHovered += HandleCellHovered;
            _view.OnCellHoverExit += HandleCellHoverExit;
            _view.OnCellClicked += HandleCellClicked;

            Debug.Log("[GridPresenter] Initialized");
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
            Debug.Log($"Clicked on cell {position}");
        }

        public void Dispose()
        {
            _view.OnCellHovered -= HandleCellHovered;
            _view.OnCellHoverExit -= HandleCellHoverExit;
            _view.OnCellClicked -= HandleCellClicked;
        }
    }
}

