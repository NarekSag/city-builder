using ContractsInterfaces.UseCasesGameplay;
using Presentation.Gameplay.Presenters;
using UseCases.Gameplay;
using UnityEngine;
using VContainer;

namespace Infrastructure.Buildings
{
    public class BuildingInitializer
    {
        [Inject] private IPlaceBuildingUseCase _placeBuildingUseCase;
        [Inject] private BuildingPresenter _buildingPresenter;

        public void Initialize()
        {
            if (_placeBuildingUseCase is PlaceBuildingUseCase useCase)
            {
                useCase.Initialize();
                Debug.Log("[BuildingInitializer] PlaceBuildingUseCase initialized");
            }

            if (_buildingPresenter != null)
            {
                _buildingPresenter.Initialize();
                Debug.Log("[BuildingInitializer] BuildingPresenter initialized");
            }
            else
            {
                Debug.LogWarning("[BuildingInitializer] BuildingPresenter is null!");
            }
        }
    }
}

