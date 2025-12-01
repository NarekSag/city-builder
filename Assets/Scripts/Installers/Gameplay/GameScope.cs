using Infrastructure;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers.Gameplay
{
    public class GameScope : LifetimeScope
    {
        [Header("Installers")]
        [SerializeField] private CameraInstaller _cameraInstaller;
        [SerializeField] private InputInstaller _inputInstaller;
        [SerializeField] private GridInstaller _gridInstaller;
        [SerializeField] private BuildingInstaller _buildingInstaller;
        [SerializeField] private EconomyInstaller _economyInstaller;

        protected override void Configure(IContainerBuilder builder)
        {
            _cameraInstaller.Install(builder);
            _inputInstaller.Install(builder);
            _gridInstaller.Install(builder);
            _buildingInstaller.Install(builder);
            _economyInstaller.Install(builder);
            
            builder.RegisterEntryPoint<GameFlow>();
        }
    }
}

