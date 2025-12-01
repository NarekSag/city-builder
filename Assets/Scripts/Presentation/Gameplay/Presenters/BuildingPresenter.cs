using System;
using ContractsInterfaces.Infrastructure;
using Domain.Gameplay.MessagesDTO;
using MessagePipe;
using Presentation.Gameplay.Views;
using VContainer;
using VContainer.Unity;

namespace Presentation.Gameplay.Presenters
{
    public class BuildingPresenter : IInitializable, IDisposable
    {
        [Inject] private BuildingView _view;
        [Inject] private IBuildingPrefabFactory _prefabFactory;
        [Inject] private ISubscriber<BuildingPlacedDTO> _buildingPlacedSubscriber;
        [Inject] private ISubscriber<BuildingRemovedDTO> _buildingRemovedSubscriber;
        [Inject] private ISubscriber<BuildingMovedDTO> _buildingMovedSubscriber;

        private IDisposable _placedSubscription;
        private IDisposable _removedSubscription;
        private IDisposable _movedSubscription;

        public void Initialize()
        {
            _placedSubscription = _buildingPlacedSubscriber.Subscribe(OnBuildingPlaced);
            _removedSubscription = _buildingRemovedSubscriber.Subscribe(OnBuildingRemoved);
            _movedSubscription = _buildingMovedSubscriber.Subscribe(OnBuildingMoved);
        }

        private void OnBuildingPlaced(BuildingPlacedDTO dto)
        {
            var prefabInstance = _prefabFactory.CreateBuildingPrefab(dto.BuildingType, null);
            _view.CreateBuildingVisual(dto.BuildingId, dto.BuildingType, dto.Position, prefabInstance);
        }

        private void OnBuildingRemoved(BuildingRemovedDTO dto)
        {
            _view.RemoveBuildingVisual(dto.BuildingId);
        }

        private void OnBuildingMoved(BuildingMovedDTO dto)
        {
            _view.MoveBuildingVisual(dto.BuildingId, dto.NewPosition);
        }

        public void Dispose()
        {
            _placedSubscription?.Dispose();
            _removedSubscription?.Dispose();
            _movedSubscription?.Dispose();
        }
    }
}

