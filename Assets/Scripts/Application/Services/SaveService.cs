using System;
using System.IO;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using UnityEngine;
using System.Collections.Generic;

namespace Application.Services
{
    public class SaveService : ISaveService
    {
        private const string SaveFileName = "savegame.json";

        public bool Save(GameStateDTO gameState, string filePath)
        {
            if (gameState == null)
            {
                Debug.LogWarning("[SaveService] GameStateDTO is null, cannot save.");
                return false;
            }

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GetDefaultSavePath();
            }

            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (string.IsNullOrEmpty(gameState.SaveTimestamp))
                {
                    gameState.SaveTimestamp = DateTime.UtcNow.ToString("O");
                }

                if (gameState.Buildings == null || gameState.Buildings.Items == null)
                {
                    gameState.SetBuildingsList(new List<BuildingDataDTO>());
                }

                var json = JsonUtility.ToJson(gameState, prettyPrint: true);

                File.WriteAllText(filePath, json);

                var buildingsCount = gameState.Buildings?.Items?.Length ?? 0;
                Debug.Log($"[SaveService] Game saved successfully to: {filePath} (Buildings: {buildingsCount}, Gold: {gameState.Gold})");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[SaveService] Failed to save game to {filePath}: {ex.Message}");
                return false;
            }
        }

        public string GetDefaultSavePath()
        {
            return Path.Combine(UnityEngine.Application.persistentDataPath, SaveFileName);
        }
    }
}

