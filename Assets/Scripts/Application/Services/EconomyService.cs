using System;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Grid = Domain.Gameplay.Models.Grid;

namespace Application.Services
{
    public class EconomyService : IEconomyService, IInitializable, IDisposable
    {
        private readonly Economy _economy;
        [Inject] private Grid _grid;
        [Inject] private IPublisher<GoldChangedDTO> _goldChangedPublisher;

        private const int IncomeIntervalSeconds = 10;
        private UniTask _incomeTask;
        private bool _isDisposed;

        public EconomyService(Economy economy)
        {
            _economy = economy;
        }

        public void Initialize()
        {
            _incomeTask = PassiveIncomeLoop();
        }

        private async UniTask PassiveIncomeLoop()
        {
            while (!_isDisposed)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(IncomeIntervalSeconds));

                if (_isDisposed || _grid == null)
                {
                    break;
                }

                var totalIncome = CalculateTotalIncome();
                if (totalIncome > 0)
                {
                    Debug.Log($"[EconomyService] Passive income: +{totalIncome} gold (Total: {_economy.Gold + totalIncome})");
                    AddGold(totalIncome);
                }
            }
        }

        private int CalculateTotalIncome()
        {
            if (_grid == null)
            {
                return 0;
            }

            var totalIncomePerTick = 0;
            foreach (var buildingPair in _grid.GetAllBuildings())
            {
                var building = buildingPair.Value;
                if (building != null && building.CurrentIncome != null)
                {
                    totalIncomePerTick += building.CurrentIncome.AmountPerTick;
                }
            }

            return totalIncomePerTick;
        }

        public int GetGold()
        {
            return _economy.Gold;
        }

        public bool AddGold(int amount)
        {
            var oldGold = _economy.Gold;
            var result = _economy.AddGold(amount);
            
            if (result)
            {
                PublishGoldChanged(_economy.Gold, _economy.Gold - oldGold);
            }
            
            return result;
        }

        public bool SpendGold(int amount, out bool success)
        {
            var oldGold = _economy.Gold;
            var result = _economy.SpendGold(amount, out success);
            
            if (result && success)
            {
                PublishGoldChanged(_economy.Gold, _economy.Gold - oldGold);
            }
            
            return result;
        }

        public bool HasEnoughGold(int amount)
        {
            return _economy.HasEnoughGold(amount);
        }

        private void PublishGoldChanged(int newAmount, int delta)
        {
            if (_goldChangedPublisher != null)
            {
                _goldChangedPublisher.Publish(new GoldChangedDTO
                {
                    NewAmount = newAmount,
                    Delta = delta
                });
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}

