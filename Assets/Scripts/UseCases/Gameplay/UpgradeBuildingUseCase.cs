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
    public class UpgradeBuildingUseCase : IUpgradeBuildingUseCase, IInitializable, IDisposable
    {
        [Inject] private Grid _grid;
        [Inject] private IEconomyService _economyService;
        [Inject] private IBuildingConfigService _buildingConfig;
        [Inject] private IPublisher<BuildingUpgradedDTO> _buildingUpgradedPublisher;
        [Inject] private IPublisher<InsufficientGoldDTO> _insufficientGoldPublisher;
        [Inject] private ISubscriber<UpgradeBuildingRequestDTO> _upgradeBuildingRequestSubscriber;

        private IDisposable _subscription;

        public void Initialize()
        {
            _subscription = _upgradeBuildingRequestSubscriber.Subscribe(Handle);
        }

        private void Handle(UpgradeBuildingRequestDTO request)
        {
            UpgradeBuilding(request);
        }

        public void UpgradeBuilding(UpgradeBuildingRequestDTO request)
        {
            if (request == null)
            {
                Debug.LogWarning("[UpgradeBuildingUseCase] Request is null");
                return;
            }

            if (request.BuildingId <= 0)
            {
                Debug.LogWarning("[UpgradeBuildingUseCase] Invalid BuildingId");
                return;
            }

            var buildingPosition = _grid.GetBuildingGridPosition(request.BuildingId);
            if (!buildingPosition.HasValue)
            {
                Debug.LogWarning($"[UpgradeBuildingUseCase] Building with ID {request.BuildingId} not found");
                return;
            }

            if (!_grid.GetBuildingAt(buildingPosition.Value, out var building))
            {
                Debug.LogWarning($"[UpgradeBuildingUseCase] Failed to get building at position {buildingPosition.Value}");
                return;
            }

            if (!BuildingUpgradeRules.CanUpgrade(building))
            {
                Debug.LogWarning($"[UpgradeBuildingUseCase] Building {request.BuildingId} cannot be upgraded (max level reached)");
                return;
            }

            var upgradeCost = _buildingConfig.GetUpgradeCost(building.Type, building.Level);
            var currentGold = _economyService.GetGold();

            if (!_economyService.HasEnoughGold(upgradeCost))
            {
                Debug.LogWarning($"[UpgradeBuildingUseCase] Insufficient gold! Required: {upgradeCost}, Current: {currentGold}");
                _insufficientGoldPublisher.Publish(new InsufficientGoldDTO
                {
                    RequiredAmount = upgradeCost,
                    CurrentAmount = currentGold
                });
                return;
            }

            if (!_economyService.SpendGold(upgradeCost, out var success) || !success)
            {
                Debug.LogWarning($"[UpgradeBuildingUseCase] Failed to spend gold! Cost: {upgradeCost}, Current: {_economyService.GetGold()}");
                return;
            }

            var newIncome = _buildingConfig.CalculateIncome(building.Type, building.Level + 1);
            if (!building.Upgrade(newIncome))
            {
                Debug.LogWarning($"[UpgradeBuildingUseCase] Failed to upgrade building {request.BuildingId} - refunding {upgradeCost} gold");
                _economyService.AddGold(upgradeCost);
                return;
            }

            var upgradedDto = new BuildingUpgradedDTO
            {
                BuildingId = building.Id,
                NewLevel = building.Level,
                NewIncome = building.CurrentIncome.AmountPerTick
            };

            _buildingUpgradedPublisher.Publish(upgradedDto);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}

