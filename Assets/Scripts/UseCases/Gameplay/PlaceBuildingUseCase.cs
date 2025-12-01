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
    public class PlaceBuildingUseCase : IPlaceBuildingUseCase, IInitializable, IDisposable
    {
        [Inject] private Grid _grid;
        [Inject] private IEconomyService _economyService;
        [Inject] private IPublisher<BuildingPlacedDTO> _buildingPlacedPublisher;
        [Inject] private IPublisher<InsufficientGoldDTO> _insufficientGoldPublisher;
        [Inject] private ISubscriber<PlaceBuildingRequestDTO> _placeBuildingRequestSubscriber;

        private int _nextBuildingId = 1;
        private IDisposable _subscription;

        public void Initialize()
        {
            _subscription = _placeBuildingRequestSubscriber.Subscribe(Handle);
            Debug.Log("[PlaceBuildingUseCase] Initialized and subscribed to PlaceBuildingRequestDTO");
        }

        private void Handle(PlaceBuildingRequestDTO request)
        {
            Debug.Log($"[PlaceBuildingUseCase] Received PlaceBuildingRequestDTO: Type={request?.BuildingType}, Position={request?.Position}");
            PlaceBuilding(request);
        }

        public void PlaceBuilding(PlaceBuildingRequestDTO request)
        {
            if (request == null)
            {
                Debug.LogWarning("[PlaceBuildingUseCase] Request is null");
                return;
            }

            if (!BuildingPlacementRules.CanPlace(_grid, request.Position))
            {
                Debug.LogWarning($"[PlaceBuildingUseCase] Cannot place building at {request.Position} - position invalid or cell occupied");
                return;
            }

            var cost = BuildingCostCalculator.GetCost(request.BuildingType, 1);
            var currentGold = _economyService.GetGold();

            if (!_economyService.HasEnoughGold(cost))
            {
                Debug.LogWarning($"[PlaceBuildingUseCase] Insufficient gold! Required: {cost}, Current: {currentGold}");
                _insufficientGoldPublisher.Publish(new InsufficientGoldDTO
                {
                    RequiredAmount = cost,
                    CurrentAmount = currentGold
                });
                return;
            }

            if (!_economyService.SpendGold(cost, out var success) || !success)
            {
                Debug.LogWarning($"[PlaceBuildingUseCase] Failed to spend gold! Cost: {cost}, Current: {_economyService.GetGold()}");
                return;
            }

            var building = new Building(_nextBuildingId++, request.BuildingType, 1, request.Position);

            if (!_grid.PlaceBuilding(building, request.Position))
            {
                Debug.LogWarning($"[PlaceBuildingUseCase] Failed to place building on grid at {request.Position} - refunding {cost} gold");
                _economyService.AddGold(cost);
                return;
            }

            var placedDto = new BuildingPlacedDTO
            {
                BuildingId = building.Id,
                BuildingType = building.Type,
                Position = building.Position
            };

            Debug.Log($"[PlaceBuildingUseCase] Publishing BuildingPlacedDTO: Id={placedDto.BuildingId}, Type={placedDto.BuildingType}, Position={placedDto.Position}");
            _buildingPlacedPublisher.Publish(placedDto);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}

