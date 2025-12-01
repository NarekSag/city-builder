using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Presentation.Gameplay.Views
{
    public class GridView : MonoBehaviour
    {
        public event Action<Vector3> OnWorldPositionHovered;
        public event Action OnHoverExit;
        public event Action<Vector3> OnWorldPositionClicked;

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
            if (Mouse.current == null) return;

            var worldPos = GetWorldPositionFromRaycast();

            if (worldPos.HasValue)
            {
                OnWorldPositionHovered?.Invoke(worldPos.Value);

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    OnWorldPositionClicked?.Invoke(worldPos.Value);
                }
            }
            else
            {
                if (_lastHoverCell.HasValue)
                {
                    OnHoverExit?.Invoke();
                }
            }
        }

        private Vector3? GetWorldPositionFromRaycast()
        {
            if (Camera.main == null || Mouse.current == null) return null;

            var mousePosition = Mouse.current.position.ReadValue();
            var ray = Camera.main.ScreenPointToRay(mousePosition);

            return Physics.Raycast(ray, out var hit) ? hit.point : null;
        }
    }
}

