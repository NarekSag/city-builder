using System.Collections.Generic;
using ContractsInterfaces.Infrastructure;
using UnityEngine;

namespace Infrastructure.Grid
{
    public class GridFactory : IGridFactory
    {
        private readonly GameObject _cellPrefab;
        private readonly Material _baseMaterial;

        public GridFactory(GameObject cellPrefab, Material baseMaterial)
        {
            _cellPrefab = cellPrefab;
            _baseMaterial = baseMaterial;
        }

        public Dictionary<Vector2Int, Renderer> CreateGridCells(Domain.Gameplay.Models.Grid grid, Transform parent)
        {
            var cellRenderers = new Dictionary<Vector2Int, Renderer>();
            var defaultMaterial = GetDefaultMaterial();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    var pos = new Vector2Int(x, y);
                    var cellRenderer = CreateCell(pos, grid, parent, defaultMaterial);
                    cellRenderers[pos] = cellRenderer;
                }
            }

            return cellRenderers;
        }

        private Renderer CreateCell(Vector2Int pos, Domain.Gameplay.Models.Grid grid, Transform parent, Material material)
        {
            GameObject cell;

            if (_cellPrefab != null)
            {
                cell = Object.Instantiate(_cellPrefab, parent);
            }
            else
            {
                cell = GameObject.CreatePrimitive(PrimitiveType.Quad);
            }

            cell.name = $"Cell_{pos.x}_{pos.y}";

            var wx = pos.x - (grid.Width - 1) * 0.5f;
            var wz = pos.y - (grid.Height - 1) * 0.5f;
            cell.transform.position = new Vector3(wx, 0f, wz);
            cell.transform.rotation = Quaternion.Euler(90, 0, 0);

            var cellRenderer = cell.GetComponent<Renderer>();
            if (cellRenderer != null && material != null)
            {
                cellRenderer.material = material;
            }

            return cellRenderer;
        }

        private Material GetDefaultMaterial()
        {
            if (_baseMaterial != null)
            {
                return _baseMaterial;
            }

            var material = new Material(Shader.Find("Standard"));
            material.color = new Color(0.2f, 0.2f, 0.2f, 0.3f);
            return material;
        }
    }
}

