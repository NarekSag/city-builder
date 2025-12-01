using System;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Grid = Domain.Gameplay.Models.Grid;

namespace UseCases.Gameplay
{
    public class RemoveBuildingUseCase : IRemoveBuildingUseCase, IInitializable, IDisposable
    {
        [Inject] private Grid _grid;
        [Inject] private IPublisher<BuildingRemovedDTO> _buildingRemovedPublisher;
        [Inject] private ISubscriber<RemoveBuildingRequestDTO> _removeBuildingRequestSubscriber;

        private IDisposable _subscription;

        public void Initialize()
        {
            _subscription = _removeBuildingRequestSubscriber.Subscribe(Handle);
            Debug.Log("[RemoveBuildingUseCase] Initialized and subscribed to RemoveBuildingRequestDTO");
        }

        private void Handle(RemoveBuildingRequestDTO request)
        {
            Debug.Log($"[RemoveBuildingUseCase] Received RemoveBuildingRequestDTO: BuildingId={request?.BuildingId}, Position={request?.Position}");
            RemoveBuilding(request);
        }

        public void RemoveBuilding(RemoveBuildingRequestDTO request)
        {
            if (request == null)
            {
                Debug.LogWarning("[RemoveBuildingUseCase] Request is null");
                return;
            }

            Building building = null;
            GridPosition position;

            if (request.Position.HasValue)
            {
                position = request.Position.Value;
                if (!_grid.RemoveBuilding(position, out building))
                {
                    Debug.LogWarning($"[RemoveBuildingUseCase] Failed to remove building at {position}");
                    return;
                }
            }
            else if (request.BuildingId > 0)
            {
                var buildingPosition = _grid.GetBuildingGridPosition(request.BuildingId);
                if (!buildingPosition.HasValue)
                {
                    Debug.LogWarning($"[RemoveBuildingUseCase] Building with ID {request.BuildingId} not found");
                    return;
                }

                position = buildingPosition.Value;
                if (!_grid.RemoveBuilding(position, out building))
                {
                    Debug.LogWarning($"[RemoveBuildingUseCase] Failed to remove building at {position}");
                    return;
                }
            }
            else
            {
                Debug.LogWarning("[RemoveBuildingUseCase] Request must have either BuildingId or Position");
                return;
            }

            if (building == null)
            {
                Debug.LogWarning("[RemoveBuildingUseCase] Building is null after removal");
                return;
            }

            var removedDto = new BuildingRemovedDTO
            {
                BuildingId = building.Id,
                BuildingType = building.Type,
                Position = position
            };

            Debug.Log($"[RemoveBuildingUseCase] Publishing BuildingRemovedDTO: Id={removedDto.BuildingId}, Type={removedDto.BuildingType}, Position={removedDto.Position}");
            _buildingRemovedPublisher.Publish(removedDto);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}

