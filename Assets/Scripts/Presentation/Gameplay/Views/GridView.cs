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
        private Dictionary<Vector2Int, Material> _originalMaterials;
        private Vector2Int? _lastHoverCell;

        public void HighlightCell(Vector2Int pos, Color color)
        {
            if (_cellRenderers.TryGetValue(pos, out var cellRenderer))
            {
                if (_lastHoverCell.HasValue && _lastHoverCell.Value != pos)
                {
                    ClearPreviousHighlight();
                }

                if (!_originalMaterials.ContainsKey(pos))
                {
                    _originalMaterials[pos] = cellRenderer.material;
                }

                cellRenderer.material.color = color;
                _lastHoverCell = pos;
            }
        }

        public void ClearHighlight()
        {
            ClearPreviousHighlight();
            _lastHoverCell = null;
        }

        private void ClearPreviousHighlight()
        {
            if (_lastHoverCell.HasValue &&
                _cellRenderers.TryGetValue(_lastHoverCell.Value, out var cellRenderer) &&
                _originalMaterials.TryGetValue(_lastHoverCell.Value, out var originalMaterial))
            {
                if (cellRenderer != null && originalMaterial != null)
                {
                    cellRenderer.material.color = originalMaterial.color;
                }
            }
        }

        public void SetCellRenderers(Dictionary<Vector2Int, Renderer> cellRenderers)
        {
            _cellRenderers = cellRenderers;
            _originalMaterials = new Dictionary<Vector2Int, Material>();

            foreach (var kvp in _cellRenderers)
            {
                if (kvp.Value != null && kvp.Value.material != null)
                {
                    var originalColor = kvp.Value.material.color;
                    var materialCopy = new Material(kvp.Value.material);
                    materialCopy.color = originalColor;
                    _originalMaterials[kvp.Key] = materialCopy;
                }
            }
        }

        private void Update()
        {
            if (_grid == null || Mouse.current == null) return;

            var gridPos = GetGridPositionFromRaycast();

            if (gridPos.HasValue)
            {
                if (_lastHoverCell != gridPos)
                {
                    if (_lastHoverCell.HasValue)
                    {
                        ClearPreviousHighlight();
                    }
                    OnCellHovered?.Invoke(gridPos.Value);
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    OnCellClicked?.Invoke(gridPos.Value);
                }
            }
            else
            {
                if (_lastHoverCell.HasValue)
                {
                    OnCellHoverExit?.Invoke();
                }
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

