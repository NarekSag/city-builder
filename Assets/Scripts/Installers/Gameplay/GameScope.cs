using Infrastructure;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;

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
        [SerializeField] private HudInstaller _hudInstaller;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
            
            // Install in order: BuildingInstaller registers MessagePipe first
            _cameraInstaller.Install(builder);
            _inputInstaller.Install(builder);
            _gridInstaller.Install(builder);
            _buildingInstaller.Install(builder); // This registers MessagePipe
            _economyInstaller.Install(builder);
            _hudInstaller.Install(builder); // This uses already registered MessagePipe
            new SaveLoadInstaller().Install(builder); // Save/Load services and Use Cases
            
            builder.RegisterEntryPoint<GameFlow>();
        }
    }
}

