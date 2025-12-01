using System;
using ContractsInterfaces.Infrastructure;
using Domain.Gameplay.MessagesDTO;
using MessagePipe;
using Presentation.Gameplay.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Presentation.Gameplay.Presenters
{
    public class BuildingPresenter : IInitializable, IDisposable
    {
        [Inject] private BuildingView _view;
        [Inject] private IBuildingPrefabFactory _prefabFactory;
        [Inject] private ISubscriber<BuildingPlacedDTO> _buildingPlacedSubscriber;

        private IDisposable _subscription;

        public void Initialize()
        {
            _subscription = _buildingPlacedSubscriber.Subscribe(OnBuildingPlaced);
            Debug.Log("[BuildingPresenter] Initialized");
        }

        private void OnBuildingPlaced(BuildingPlacedDTO dto)
        {
            var prefabInstance = _prefabFactory.CreateBuildingPrefab(dto.BuildingType, null);
            _view.CreateBuildingVisual(dto.BuildingId, dto.BuildingType, dto.Position, prefabInstance);
            Debug.Log($"[BuildingPresenter] Building {dto.BuildingId} ({dto.BuildingType}) placed at {dto.Position}");
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}

