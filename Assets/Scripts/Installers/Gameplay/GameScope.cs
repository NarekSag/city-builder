using Application.Services;
using Infrastructure;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Domain.Gameplay.Models;
using ContractsInterfaces.UseCasesGameplay;

namespace Installers.Gameplay
{
    public class GameScope : LifetimeScope
    {
        [Header("Installers")]
        [SerializeField] private CameraInstaller _cameraInstaller;
        [SerializeField] private InputInstaller _inputInstaller;
        [SerializeField] private GridInstaller _gridInstaller;
        [SerializeField] private BuildingInstaller _buildingInstaller;

        protected override void Configure(IContainerBuilder builder)
        {
            _cameraInstaller.Install(builder);
            _inputInstaller.Install(builder);
            _gridInstaller.Install(builder);
            _buildingInstaller.Install(builder);

            var economy = new Economy(500);
            builder.RegisterInstance(economy);
            builder.Register<IEconomyService, EconomyService>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<GameFlow>();
        }
    }
}

