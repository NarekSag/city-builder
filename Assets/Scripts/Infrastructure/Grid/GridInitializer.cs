#nullable enable
using ContractsInterfaces.Infrastructure;
using Presentation.Gameplay.Presenters;
using Presentation.Gameplay.Views;
using UnityEngine;
using VContainer;

namespace Infrastructure.Grid
{
    public class GridInitializer
    {
        [Inject] private Domain.Gameplay.Models.Grid _grid;
        [Inject] private IGridFactory _gridFactory;
        [Inject] private GridView _gridView;
        [Inject] private GridPresenter _gridPresenter;
        [Inject] private BuildingView _buildingView;

        public void Initialize()
        {
            if (_gridView == null || _gridFactory == null)
            {
                Debug.LogError("[GridInitializer] GridView or GridFactory is null!");
                return;
            }

            var parent = new GameObject("GridCells");
            parent.transform.SetParent(_gridView.transform);
            parent.transform.localPosition = Vector3.zero;

            var cellRenderers = _gridFactory.CreateGridCells(_grid, parent.transform);
            _gridView.SetCellRenderers(cellRenderers);

            if (_buildingView != null)
            {
                var buildingsParent = new GameObject("Buildings");
                buildingsParent.transform.SetParent(_gridView.transform);
                buildingsParent.transform.localPosition = Vector3.zero;
                _buildingView.Initialize(buildingsParent.transform);
            }

            Debug.Log($"[GridInitializer] Grid initialized: {_grid.Width}x{_grid.Height}");
        }
    }
}

