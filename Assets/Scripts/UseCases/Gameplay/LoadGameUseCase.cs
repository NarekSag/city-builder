using System;
using System.Collections.Generic;
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
    public class LoadGameUseCase : ILoadGameUseCase, IInitializable, IDisposable
    {
        [Inject] private Grid _grid;
        [Inject] private IEconomyService _economyService;
        [Inject] private ILoadService _loadService;
        [Inject] private IBuildingConfigService _buildingConfig;
        [Inject] private IPublisher<GameLoadedDTO> _gameLoadedPublisher;
        [Inject] private IPublisher<BuildingPlacedDTO> _buildingPlacedPublisher;
        [Inject] private ISubscriber<LoadGameRequestDTO> _loadGameRequestSubscriber;

        private IDisposable _subscription;

        public void Initialize()
        {
            _subscription = _loadGameRequestSubscriber.Subscribe(Handle);
        }

        private void Handle(LoadGameRequestDTO request)
        {
            LoadGame(request);
        }

        public void LoadGame(LoadGameRequestDTO request)
        {
            if (request == null)
            {
                Debug.LogWarning("[LoadGameUseCase] Request is null");
                return;
            }

            try
            {
                var filePath = _loadService.GetDefaultSavePath();

                if (!_loadService.SaveFileExists(filePath))
                {
                    Debug.LogWarning($"[LoadGameUseCase] Save file does not exist: {filePath}");
                    return;
                }

                var gameState = _loadService.Load(filePath);

                if (gameState == null)
                {
                    Debug.LogWarning("[LoadGameUseCase] Failed to load game state");
                    return;
                }

                // Restore game state
                RestoreGameState(gameState);

                // Publish GameLoadedDTO
                var loadedDto = new GameLoadedDTO
                {
                    BuildingsCount = gameState.Buildings?.Count ?? 0,
                    Gold = gameState.Gold
                };
                _gameLoadedPublisher.Publish(loadedDto);

                Debug.Log($"[LoadGameUseCase] Game loaded successfully. Buildings: {loadedDto.BuildingsCount}, Gold: {loadedDto.Gold}");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[LoadGameUseCase] Error while loading game: {ex.Message}");
            }
        }

        private void RestoreGameState(GameStateDTO gameState)
        {
            // Clear existing grid
            _grid.Clear();

            // Restore Gold
            RestoreGold(gameState.Gold);

            // Restore buildings
            if (gameState.Buildings != null && gameState.Buildings.Count > 0)
            {
                RestoreBuildings(gameState.Buildings);
            }
        }

        private void RestoreGold(int gold)
        {
            if (!_economyService.SetGold(gold))
            {
                Debug.LogWarning($"[LoadGameUseCase] Failed to set gold to {gold}");
            }
        }

        private void RestoreBuildings(List<BuildingDataDTO> buildingsData)
        {
            foreach (var buildingData in buildingsData)
            {
                try
                {
                    var building = ConvertDTOToBuilding(buildingData);
                    var position = new GridPosition(buildingData.PositionX, buildingData.PositionY);

                    if (!_grid.IsValidPosition(position))
                    {
                        Debug.LogWarning($"[LoadGameUseCase] Invalid position for building {buildingData.Id}: ({buildingData.PositionX}, {buildingData.PositionY})");
                        continue;
                    }

                    if (_grid.IsOccupied(position))
                    {
                        Debug.LogWarning($"[LoadGameUseCase] Position already occupied for building {buildingData.Id}: ({buildingData.PositionX}, {buildingData.PositionY})");
                        continue;
                    }

                    if (!_grid.PlaceBuilding(building, position))
                    {
                        Debug.LogWarning($"[LoadGameUseCase] Failed to place building {buildingData.Id} at ({buildingData.PositionX}, {buildingData.PositionY})");
                        continue;
                    }

                    // Publish BuildingPlacedDTO for each restored building
                    var placedDto = new BuildingPlacedDTO
                    {
                        BuildingId = building.Id,
                        BuildingType = building.Type,
                        Position = building.Position
                    };
                    _buildingPlacedPublisher.Publish(placedDto);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[LoadGameUseCase] Error restoring building {buildingData.Id}: {ex.Message}");
                }
            }
        }

        private Building ConvertDTOToBuilding(BuildingDataDTO buildingData)
        {
            var position = new GridPosition(buildingData.PositionX, buildingData.PositionY);
            var income = new Income(buildingData.IncomeAmountPerTick, buildingData.Type);

            return new Building(
                buildingData.Id,
                buildingData.Type,
                buildingData.Level,
                position,
                income
            );
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}

