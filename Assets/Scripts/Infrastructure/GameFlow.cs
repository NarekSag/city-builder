using Infrastructure.Grid;
using Infrastructure.Buildings;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameFlow : IStartable
    {
        [Inject] private GridInitializer _gridInitializer;
        [Inject] private BuildingInitializer _buildingInitializer;

        public void Start()
        {
            Debug.Log("[GameFlow] Initializing game flow...");

            _gridInitializer.Initialize();
            _buildingInitializer.Initialize();

            Debug.Log("[GameFlow] Game flow initialized.");
        }
    }
}

