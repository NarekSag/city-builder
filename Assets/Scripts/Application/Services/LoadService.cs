using System;
using System.IO;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using UnityEngine;

namespace Application.Services
{
    public class LoadService : ILoadService
    {
        private const string SaveFileName = "savegame.json";

        public GameStateDTO? Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GetDefaultSavePath();
            }

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[LoadService] Save file does not exist: {filePath}");
                return null;
            }

            try
            {
                var json = File.ReadAllText(filePath);

                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogWarning($"[LoadService] Save file is empty: {filePath}");
                    return null;
                }

                var gameState = JsonUtility.FromJson<GameStateDTO>(json);

                if (gameState == null)
                {
                    Debug.LogWarning($"[LoadService] Failed to deserialize save file: {filePath}");
                    return null;
                }

                if (!ValidateGameState(gameState))
                {
                    Debug.LogWarning($"[LoadService] Invalid game state data in file: {filePath}");
                    return null;
                }

                Debug.Log($"[LoadService] Game loaded successfully from: {filePath} (Buildings: {gameState.Buildings?.Count ?? 0}, Gold: {gameState.Gold})");
                return gameState;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[LoadService] Failed to load game from {filePath}: {ex.Message}");
                return null;
            }
        }

        public bool SaveFileExists(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GetDefaultSavePath();
            }

            return File.Exists(filePath);
        }

        public string GetDefaultSavePath()
        {
            return Path.Combine(UnityEngine.Application.persistentDataPath, SaveFileName);
        }

        private bool ValidateGameState(GameStateDTO gameState)
        {
            if (gameState == null)
            {
                return false;
            }

            if (gameState.Gold < 0)
            {
                Debug.LogWarning("[LoadService] Invalid Gold value in save file");
                return false;
            }

            if (gameState.Buildings == null)
            {
                gameState.Buildings = new System.Collections.Generic.List<BuildingDataDTO>();
                return true;
            }

            foreach (var building in gameState.Buildings)
            {
                if (!ValidateBuildingData(building))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidateBuildingData(BuildingDataDTO building)
        {
            if (building == null)
            {
                Debug.LogWarning("[LoadService] Null building data in save file");
                return false;
            }

            if (building.Id <= 0)
            {
                Debug.LogWarning($"[LoadService] Invalid building ID: {building.Id}");
                return false;
            }

            if (building.Level < 1 || building.Level > 3)
            {
                Debug.LogWarning($"[LoadService] Invalid building level: {building.Level} (must be 1-3)");
                return false;
            }

            if (building.PositionX < 0 || building.PositionY < 0)
            {
                Debug.LogWarning($"[LoadService] Invalid building position: ({building.PositionX}, {building.PositionY})");
                return false;
            }

            if (building.IncomeAmountPerTick < 0)
            {
                Debug.LogWarning($"[LoadService] Invalid income amount: {building.IncomeAmountPerTick}");
                return false;
            }

            return true;
        }
    }
}

