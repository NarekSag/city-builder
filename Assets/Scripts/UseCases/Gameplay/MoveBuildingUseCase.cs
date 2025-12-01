using System;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using Domain.Gameplay.Rules;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Grid = Domain.Gameplay.Models.Grid;

namespace UseCases.Gameplay
{
    public class MoveBuildingUseCase : IMoveBuildingUseCase, IInitializable, IDisposable
    {
        [Inject] private Grid _grid;
        [Inject] private IPublisher<BuildingMovedDTO> _buildingMovedPublisher;
        [Inject] private ISubscriber<MoveBuildingRequestDTO> _moveBuildingRequestSubscriber;

        private IDisposable _subscription;

        public void Initialize()
        {
            _subscription = _moveBuildingRequestSubscriber.Subscribe(Handle);
            Debug.Log("[MoveBuildingUseCase] Initialized and subscribed to MoveBuildingRequestDTO");
        }

        private void Handle(MoveBuildingRequestDTO request)
        {
            Debug.Log($"[MoveBuildingUseCase] Received MoveBuildingRequestDTO: BuildingId={request?.BuildingId}, NewPosition={request?.NewPosition}");
            MoveBuilding(request);
        }

        public void MoveBuilding(MoveBuildingRequestDTO request)
        {
            if (request == null)
            {
                Debug.LogWarning("[MoveBuildingUseCase] Request is null");
                return;
            }

            if (request.BuildingId <= 0)
            {
                Debug.LogWarning("[MoveBuildingUseCase] Invalid BuildingId");
                return;
            }

            var oldPosition = _grid.GetBuildingGridPosition(request.BuildingId);
            if (!oldPosition.HasValue)
            {
                Debug.LogWarning($"[MoveBuildingUseCase] Building with ID {request.BuildingId} not found");
                return;
            }

            if (!BuildingPlacementRules.CanPlace(_grid, request.NewPosition))
            {
                Debug.LogWarning($"[MoveBuildingUseCase] Cannot move building to {request.NewPosition} - position invalid or cell occupied");
                return;
            }

            Building building = null;
            if (!_grid.MoveBuilding(oldPosition.Value, request.NewPosition, out building))
            {
                Debug.LogWarning($"[MoveBuildingUseCase] Failed to move building from {oldPosition.Value} to {request.NewPosition}");
                return;
            }

            if (building == null)
            {
                Debug.LogWarning("[MoveBuildingUseCase] Building is null after move");
                return;
            }

            var movedDto = new BuildingMovedDTO
            {
                BuildingId = building.Id,
                BuildingType = building.Type,
                OldPosition = oldPosition.Value,
                NewPosition = request.NewPosition
            };

            Debug.Log($"[MoveBuildingUseCase] Publishing BuildingMovedDTO: Id={movedDto.BuildingId}, From={movedDto.OldPosition}, To={movedDto.NewPosition}");
            _buildingMovedPublisher.Publish(movedDto);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}

