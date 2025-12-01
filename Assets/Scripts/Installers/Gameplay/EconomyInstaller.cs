using Application.Services;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.Models;
using Repositories.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers.Gameplay
{
    public class EconomyInstaller : MonoBehaviour, IInstaller
    {
        [Header("Economy Settings")]
        [SerializeField] private EconomySettings _economySettings;

        public void Install(IContainerBuilder builder)
        {
            if (_economySettings != null)
            {
                builder.RegisterInstance(_economySettings);
            }
            else
            {
                Debug.LogWarning("[EconomyInstaller] EconomySettings not assigned! Using default values.");
            }

            var initialGold = _economySettings != null ? _economySettings.InitialGold : 500;
            var economy = new Economy(initialGold);
            builder.RegisterInstance(economy);

            builder.Register<IEconomyService, EconomyService>(Lifetime.Singleton);
        }
    }
}

