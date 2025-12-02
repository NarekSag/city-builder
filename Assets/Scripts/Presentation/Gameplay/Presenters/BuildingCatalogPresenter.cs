using System;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
using Presentation.Gameplay.Views;
using VContainer;
using VContainer.Unity;

namespace Presentation.Gameplay.Presenters
{
    public class BuildingCatalogPresenter : IInitializable, IDisposable
    {
        [Inject] private BuildingCatalogView _view;
        [Inject] private IBuildingConfigService _buildingConfigService;
        [Inject] private IPublisher<BuildingTypeSelectedDTO> _buildingTypeSelectedPublisher;
        [Inject] private ISubscriber<BuildingTypeSelectedDTO> _buildingTypeSelectedSubscriber;

        private IDisposable _typeSelectedSubscription;

        public void Initialize()
        {
            // Initialize building costs
            var houseCost = _buildingConfigService.GetCost(BuildingType.House, 1);
            var farmCost = _buildingConfigService.GetCost(BuildingType.Farm, 1);
            var mineCost = _buildingConfigService.GetCost(BuildingType.Mine, 1);
            _view.UpdateBuildingCosts(houseCost, farmCost, mineCost);

            // Subscribe to view events
            _view.OnBuildingTypeSelected += HandleBuildingTypeSelected;

            // Subscribe to external building type selection (e.g., from hotkeys)
            _typeSelectedSubscription = _buildingTypeSelectedSubscriber.Subscribe(OnBuildingTypeSelected);

            // Set House as default selection
            _view.HighlightBuildingType(BuildingType.House);
            
            // Publish default selection to sync with GridPresenter
            var defaultDto = new BuildingTypeSelectedDTO
            {
                BuildingType = BuildingType.House
            };
            _buildingTypeSelectedPublisher.Publish(defaultDto);
        }

        private void HandleBuildingTypeSelected(BuildingType buildingType)
        {
            var dto = new BuildingTypeSelectedDTO
            {
                BuildingType = buildingType
            };
            _buildingTypeSelectedPublisher.Publish(dto);
        }

        private void OnBuildingTypeSelected(BuildingTypeSelectedDTO dto)
        {
            // Update visual highlight when building type is selected externally (e.g., hotkeys)
            _view.HighlightBuildingType(dto.BuildingType);
        }

        public void Dispose()
        {
            _view.OnBuildingTypeSelected -= HandleBuildingTypeSelected;
            _typeSelectedSubscription?.Dispose();
        }
    }
}

