using System;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Application.Services
{
    public class AutoSaveService : IInitializable, IDisposable
    {
        [Inject] private ISaveGameUseCase _saveGameUseCase;

        private const int AutoSaveIntervalSeconds = 30;
        private UniTask _autoSaveTask;
        private bool _isDisposed;

        public void Initialize()
        {
            _autoSaveTask = AutoSaveLoop();
        }

        private async UniTask AutoSaveLoop()
        {
            while (!_isDisposed)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(AutoSaveIntervalSeconds));

                if (_isDisposed || _saveGameUseCase == null)
                {
                    break;
                }

                try
                {
                    var request = new SaveGameRequestDTO();
                    _saveGameUseCase.SaveGame(request);
                    Debug.Log($"[AutoSaveService] Auto-save completed at {DateTime.UtcNow:O}");
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[AutoSaveService] Error during auto-save: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}

