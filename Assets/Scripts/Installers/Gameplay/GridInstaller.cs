using ContractsInterfaces.Infrastructure;
using Domain.Gameplay.Models;
using Infrastructure.Grid;
using Presentation.Gameplay.Presenters;
using Presentation.Gameplay.Views;
using Repositories.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Grid = Domain.Gameplay.Models.Grid;

namespace Installers.Gameplay
{
    public class GridInstaller : MonoBehaviour, IInstaller
    {
        [Header("Grid Settings")]
        [SerializeField] private GridSettings _gridSettings;

        [Header("Grid Factory Settings")]
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private Material _baseMaterial;

        [Header("Grid Components")]
        [SerializeField] private GridView _gridView;

        public void Install(IContainerBuilder builder)
        {
            Grid grid;

            if (_gridSettings != null)
            {
                grid = new Grid(_gridSettings.Width, _gridSettings.Height);
                builder.RegisterInstance(_gridSettings);
            }
            else
            {
                grid = new Grid(32, 32);
                Debug.LogWarning("[GridInstaller] GridSettings not assigned! Using default 32x32 grid.");
            }

            builder.RegisterInstance(grid);

            var gridFactory = new GridFactory(_cellPrefab, _baseMaterial);
            builder.RegisterInstance<IGridFactory>(gridFactory);
            
            if (_gridView == null)
            {
                _gridView = FindFirstObjectByType<GridView>();
            }

            if (_gridView != null)
            {
                builder.RegisterComponent(_gridView);
            }
            else
            {
                Debug.LogWarning("[GridInstaller] GridView not found! Create GridView GameObject in scene.");
            }

            builder.Register<GridPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GridPresenter>();

            builder.Register<GridInitializer>(Lifetime.Singleton);
            
            Debug.Log($"[GridInstaller] Grid created: {grid.Width}x{grid.Height}");
        }
    }
}

