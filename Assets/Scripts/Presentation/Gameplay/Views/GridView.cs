using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using Grid = Domain.Gameplay.Models.Grid;

namespace Presentation.Gameplay.Views
{
    public class GridView : MonoBehaviour
    {
        public event Action<Vector2Int> OnCellHovered;
        public event Action OnCellHoverExit;
        public event Action<Vector2Int> OnCellClicked;

        [Inject] private Grid _grid;

        private Dictionary<Vector2Int, Renderer> _cellRenderers;
        private Vector2Int? _lastHoverCell;
        private Material _defaultMaterial;

        private Material GetDefaultMaterial()
        {
            if (_cellRenderers.Count == 0) return null;

            var firstRenderer = _cellRenderers.Values.GetEnumerator();
            firstRenderer.MoveNext();
            return firstRenderer.Current?.material;
        }

        public void HighlightCell(Vector2Int pos, Color color)
        {
            if (_cellRenderers.TryGetValue(pos, out var cellRenderer))
            {
                cellRenderer.material.color = color;
                _lastHoverCell = pos;
            }
        }

        public void ClearHighlight()
        {
            if (_lastHoverCell.HasValue &&
                _cellRenderers.TryGetValue(_lastHoverCell.Value, out var cellRenderer) &&
                _defaultMaterial != null)
            {
                cellRenderer.material = _defaultMaterial;
            }

            _lastHoverCell = null;
        }

        public void SetCellRenderers(Dictionary<Vector2Int, Renderer> cellRenderers)
        {
            _cellRenderers = cellRenderers;
            _defaultMaterial = GetDefaultMaterial();
        }

        private void Update()
        {
            if (_grid == null || Mouse.current == null) return;

            var gridPos = GetGridPositionFromRaycast();

            if (gridPos.HasValue)
            {
                if (_lastHoverCell != gridPos)
                    OnCellHovered?.Invoke(gridPos.Value);

                if (Mouse.current.leftButton.wasPressedThisFrame)
                    OnCellClicked?.Invoke(gridPos.Value);
            }
            else
            {
                if (_lastHoverCell.HasValue)
                    OnCellHoverExit?.Invoke();
            }
        }

        private Vector2Int? GetGridPositionFromRaycast()
        {
            if (Camera.main == null || Mouse.current == null) return null;

            var mousePosition = Mouse.current.position.ReadValue();
            var ray = Camera.main.ScreenPointToRay(mousePosition);

            return Physics.Raycast(ray, out var hit)
                ? GetGridPositionFromWorld(hit.point)
                : null;
        }

        private Vector2Int? GetGridPositionFromWorld(Vector3 worldPosition)
        {
            if (_grid == null) return null;

            int gx = Mathf.RoundToInt(worldPosition.x + (_grid.Width - 1) * 0.5f);
            int gy = Mathf.RoundToInt(worldPosition.z + (_grid.Height - 1) * 0.5f);

            var position = new Vector2Int(gx, gy);

            return _grid.IsValidPosition(position) ? position : null;
        }
    }
}

