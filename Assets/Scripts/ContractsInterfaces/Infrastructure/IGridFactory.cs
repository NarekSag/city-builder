using System.Collections.Generic;
using UnityEngine;
using Grid = Domain.Gameplay.Models.Grid;

namespace ContractsInterfaces.Infrastructure
{
    public interface IGridFactory
    {
        Dictionary<Vector2Int, Renderer> CreateGridCells(Grid grid, Transform parent);
    }
}

