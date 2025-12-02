using System;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using MessagePipe;
using Presentation.Gameplay.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Presentation.Gameplay.Presenters
{
    public class HudPresenter : IInitializable, IDisposable
    {
        [Inject] private HudView _view;
        [Inject] private IEconomyService _economyService;
        [Inject] private ISubscriber<GoldChangedDTO> _goldChangedSubscriber;
        [Inject] private ISubscriber<InsufficientGoldDTO> _insufficientGoldSubscriber;
        [Inject] private ISubscriber<GameSavedDTO> _gameSavedSubscriber;
        [Inject] private ISubscriber<GameLoadedDTO> _gameLoadedSubscriber;
        [Inject] private IPublisher<SaveGameRequestDTO> _saveGamePublisher;
        [Inject] private IPublisher<LoadGameRequestDTO> _loadGamePublisher;

        private IDisposable _goldChangedSubscription;
        private IDisposable _insufficientGoldSubscription;
        private IDisposable _gameSavedSubscription;
        private IDisposable _gameLoadedSubscription;

        public void Initialize()
        {
            // Initialize with current gold value
            var currentGold = _economyService.GetGold();
            _view.UpdateGold(currentGold);

            // Subscribe to events
            _goldChangedSubscription = _goldChangedSubscriber.Subscribe(OnGoldChanged);
            _insufficientGoldSubscription = _insufficientGoldSubscriber.Subscribe(OnInsufficientGold);
            _gameSavedSubscription = _gameSavedSubscriber.Subscribe(OnGameSaved);
            _gameLoadedSubscription = _gameLoadedSubscriber.Subscribe(OnGameLoaded);

            // Subscribe to button clicks
            if (_view.SaveButton != null)
            {
                _view.SaveButton.clicked += OnSaveButtonClicked;
            }
            else
            {
                Debug.LogWarning("[HudPresenter] SaveButton not found in HudView!");
            }

            if (_view.LoadButton != null)
            {
                _view.LoadButton.clicked += OnLoadButtonClicked;
            }
            else
            {
                Debug.LogWarning("[HudPresenter] LoadButton not found in HudView!");
            }
        }

        private void OnGoldChanged(GoldChangedDTO dto)
        {
            _view.UpdateGold(dto.NewAmount);
        }

        private void OnInsufficientGold(InsufficientGoldDTO dto)
        {
            _view.ShowInsufficientGoldNotification(dto.RequiredAmount, dto.CurrentAmount);
        }

        private void OnSaveButtonClicked()
        {
            _saveGamePublisher.Publish(new SaveGameRequestDTO());
        }

        private void OnLoadButtonClicked()
        {
            _loadGamePublisher.Publish(new LoadGameRequestDTO());
        }

        private void OnGameSaved(GameSavedDTO dto)
        {
            Debug.Log($"[HudPresenter] Game saved! Timestamp: {dto.SaveTimestamp}, Buildings: {dto.BuildingsCount}");
            // TODO: Show save confirmation notification in UI
        }

        private void OnGameLoaded(GameLoadedDTO dto)
        {
            Debug.Log($"[HudPresenter] Game loaded! Buildings: {dto.BuildingsCount}, Gold: {dto.Gold}");
            // Update gold display after load
            _view.UpdateGold(dto.Gold);
            // TODO: Show load confirmation notification in UI
        }

        public void Dispose()
        {
            _goldChangedSubscription?.Dispose();
            _insufficientGoldSubscription?.Dispose();
            _gameSavedSubscription?.Dispose();
            _gameLoadedSubscription?.Dispose();

            if (_view.SaveButton != null)
            {
                _view.SaveButton.clicked -= OnSaveButtonClicked;
            }

            if (_view.LoadButton != null)
            {
                _view.LoadButton.clicked -= OnLoadButtonClicked;
            }
        }
    }
}

