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
                    var buildingsCount = gameState.GetBuildingsList().Count;
                    var savedDto = new GameSavedDTO
                    {
                        SaveTimestamp = gameState.SaveTimestamp,
                        BuildingsCount = buildingsCount
                    };
                    _gameSavedPublisher.Publish(savedDto);
                    Debug.Log($"[SaveGameUseCase] Game saved successfully. Buildings: {buildingsCount}, Gold: {gameState.Gold}");
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
                SaveTimestamp = DateTime.UtcNow.ToString("O")
            };

            // Collect all buildings from Grid
            var buildingsList = new List<BuildingDataDTO>();
            var allBuildings = _grid.GetAllBuildings();
            foreach (var kvp in allBuildings)
            {
                var building = kvp.Value;
                var gridPosition = kvp.Key; // Use the key (Vector2Int) from dictionary as the source of truth
                var buildingData = ConvertBuildingToDTO(building, gridPosition);
                buildingsList.Add(buildingData);
            }

            gameState.SetBuildingsList(buildingsList);
            return gameState;
        }

        private BuildingDataDTO ConvertBuildingToDTO(Building building, Vector2Int gridPosition)
        {
            var dto = new BuildingDataDTO();
            dto.Id = building.Id;
            dto.Type = building.Type;
            dto.Level = building.Level;
            dto.PositionX = gridPosition.x; // Use grid position from dictionary key
            dto.PositionY = gridPosition.y; // Use grid position from dictionary key
            dto.IncomeAmountPerTick = building.CurrentIncome.AmountPerTick;
            
            Debug.Log($"[SaveGameUseCase] Converting building {building.Id} at position ({dto.PositionX}, {dto.PositionY})");
            
            return dto;
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}

