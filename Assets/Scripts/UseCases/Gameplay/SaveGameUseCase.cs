using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SaveGameUseCase : ISaveGameUseCase, IInitializable, IDisposable
    {
        [Inject] private Grid _grid;
        [Inject] private IEconomyService _economyService;
        [Inject] private ISaveService _saveService;
        [Inject] private IPublisher<GameSavedDTO> _gameSavedPublisher;
        [Inject] private ISubscriber<SaveGameRequestDTO> _saveGameRequestSubscriber;

        private IDisposable _subscription;

        public void Initialize()
        {
            _subscription = _saveGameRequestSubscriber.Subscribe(Handle);
        }

        private void Handle(SaveGameRequestDTO request)
        {
            SaveGame(request);
        }

        public void SaveGame(SaveGameRequestDTO request)
        {
            if (request == null)
            {
                Debug.LogWarning("[SaveGameUseCase] Request is null");
                return;
            }

            try
            {
                var gameState = CollectGameState();
                var filePath = _saveService.GetDefaultSavePath();

                var success = _saveService.Save(gameState, filePath);

                if (success)
                {
                    var savedDto = new GameSavedDTO
                    {
                        SaveTimestamp = gameState.SaveTimestamp,
                        BuildingsCount = gameState.Buildings?.Count ?? 0
                    };
                    _gameSavedPublisher.Publish(savedDto);
                    Debug.Log($"[SaveGameUseCase] Game saved successfully. Buildings: {savedDto.BuildingsCount}, Gold: {gameState.Gold}");
                }
                else
                {
                    Debug.LogWarning("[SaveGameUseCase] Failed to save game");
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[SaveGameUseCase] Error while saving game: {ex.Message}");
            }
        }

        private GameStateDTO CollectGameState()
        {
            var gameState = new GameStateDTO
            {
                Gold = _economyService.GetGold(),
                Buildings = new List<BuildingDataDTO>(),
                SaveTimestamp = DateTime.UtcNow.ToString("O")
            };

            // Collect all buildings from Grid
            var allBuildings = _grid.GetAllBuildings();
            foreach (var kvp in allBuildings)
            {
                var building = kvp.Value;
                var buildingData = ConvertBuildingToDTO(building);
                gameState.Buildings.Add(buildingData);
            }

            return gameState;
        }

        private BuildingDataDTO ConvertBuildingToDTO(Building building)
        {
            return new BuildingDataDTO
            {
                Id = building.Id,
                Type = building.Type,
                Level = building.Level,
                PositionX = building.Position.X,
                PositionY = building.Position.Y,
                IncomeAmountPerTick = building.CurrentIncome.AmountPerTick
            };
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}

