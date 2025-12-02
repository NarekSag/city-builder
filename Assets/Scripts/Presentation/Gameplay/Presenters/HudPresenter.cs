using System;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using MessagePipe;
using Presentation.Gameplay.Views;
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

        private IDisposable _goldChangedSubscription;
        private IDisposable _insufficientGoldSubscription;

        public void Initialize()
        {
            // Initialize with current gold value
            var currentGold = _economyService.GetGold();
            _view.UpdateGold(currentGold);

            // Subscribe to events
            _goldChangedSubscription = _goldChangedSubscriber.Subscribe(OnGoldChanged);
            _insufficientGoldSubscription = _insufficientGoldSubscriber.Subscribe(OnInsufficientGold);
        }

        private void OnGoldChanged(GoldChangedDTO dto)
        {
            _view.UpdateGold(dto.NewAmount);
        }

        private void OnInsufficientGold(InsufficientGoldDTO dto)
        {
            _view.ShowInsufficientGoldNotification(dto.RequiredAmount, dto.CurrentAmount);
        }

        public void Dispose()
        {
            _goldChangedSubscription?.Dispose();
            _insufficientGoldSubscription?.Dispose();
        }
    }
}

