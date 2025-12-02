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
        [Inject] private IRemoveBuildingUseCase _removeBuildingUseCase;
        [Inject] private IMoveBuildingUseCase _moveBuildingUseCase;
        [Inject] private IUpgradeBuildingUseCase _upgradeBuildingUseCase;
        [Inject] private BuildingPresenter _buildingPresenter;
        [Inject] private BuildingPropertiesPresenter _buildingPropertiesPresenter;
        [Inject] private BuildingCatalogPresenter _buildingCatalogPresenter;

        public void Initialize()
        {
            if (_placeBuildingUseCase is PlaceBuildingUseCase placeUseCase)
            {
                placeUseCase.Initialize();
                Debug.Log("[BuildingInitializer] PlaceBuildingUseCase initialized");
            }

            if (_removeBuildingUseCase is RemoveBuildingUseCase removeUseCase)
            {
                removeUseCase.Initialize();
                Debug.Log("[BuildingInitializer] RemoveBuildingUseCase initialized");
            }

            if (_moveBuildingUseCase is MoveBuildingUseCase moveUseCase)
            {
                moveUseCase.Initialize();
                Debug.Log("[BuildingInitializer] MoveBuildingUseCase initialized");
            }

            if (_upgradeBuildingUseCase is UpgradeBuildingUseCase upgradeUseCase)
            {
                upgradeUseCase.Initialize();
                Debug.Log("[BuildingInitializer] UpgradeBuildingUseCase initialized");
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

            if (_buildingPropertiesPresenter != null)
            {
                _buildingPropertiesPresenter.Initialize();
                Debug.Log("[BuildingInitializer] BuildingPropertiesPresenter initialized");
            }
            else
            {
                Debug.LogWarning("[BuildingInitializer] BuildingPropertiesPresenter is null!");
            }

            if (_buildingCatalogPresenter != null)
            {
                _buildingCatalogPresenter.Initialize();
                Debug.Log("[BuildingInitializer] BuildingCatalogPresenter initialized");
            }
            else
            {
                Debug.LogWarning("[BuildingInitializer] BuildingCatalogPresenter is null!");
            }
        }
    }
}

