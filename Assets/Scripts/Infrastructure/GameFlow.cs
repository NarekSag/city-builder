using Infrastructure.Grid;
using Infrastructure.Buildings;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ContractsInterfaces.UseCasesGameplay;

namespace Infrastructure
{
    public class GameFlow : IStartable
    {
        [Inject] private GridInitializer _gridInitializer;
        [Inject] private BuildingInitializer _buildingInitializer;
        [Inject] private IEconomyService _economyService;
        public void Start()
        {
            Debug.Log("[GameFlow] Initializing game flow...");

            _gridInitializer.Initialize();
            _buildingInitializer.Initialize();
            _economyService.Initialize();

            Debug.Log("[GameFlow] Game flow initialized.");
        }
    }
}

