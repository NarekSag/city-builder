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
            // Register MessagePipe once at the top level
            var options = builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
            
            // Register all message brokers here to avoid conflicts
            RegisterAllMessageBrokers(builder, options);
            
            // Install in order
            _cameraInstaller.Install(builder);
            _inputInstaller.Install(builder);
            _gridInstaller.Install(builder);
            _buildingInstaller.Install(builder);
            _economyInstaller.Install(builder);
            _hudInstaller.Install(builder);
            new SaveLoadInstaller().Install(builder); // Save/Load services and Use Cases
            
            builder.RegisterEntryPoint<GameFlow>();
        }

        private void RegisterAllMessageBrokers(IContainerBuilder builder, MessagePipeOptions options)
        {
            // Building-related message brokers
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.PlaceBuildingRequestDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.BuildingPlacedDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.InsufficientGoldDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.RemoveBuildingRequestDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.BuildingRemovedDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.MoveBuildingRequestDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.BuildingMovedDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.UpgradeBuildingRequestDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.BuildingUpgradedDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.GoldChangedDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.BuildingSelectedDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.MoveModeStartedDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.MoveModeCancelledDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.BuildingTypeSelectedDTO>(options);
            
            // Save/Load message brokers
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.SaveGameRequestDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.LoadGameRequestDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.GameSavedDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.GameLoadedDTO>(options);
        }
    }
}

