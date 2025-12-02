using Application.Services;
using ContractsInterfaces.Infrastructure;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using Infrastructure.Buildings;
using Infrastructure.Prefabs;
using Presentation.Gameplay.Presenters;
using Presentation.Gameplay.Views;
using UseCases.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;

namespace Installers.Gameplay
{
    public class BuildingInstaller : MonoBehaviour, IInstaller
    {
        [Header("Building Prefabs")]
        [SerializeField] private GameObject _buildingPrefab;

        [Header("Building Components")]
        [SerializeField] private BuildingView _buildingView;
        [SerializeField] private BuildingPropertiesView _buildingPropertiesView;
        [SerializeField] private BuildingCatalogView _buildingCatalogView;

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(resolver =>
            {
                var options = resolver.Resolve<MessagePipeOptions>();
                RegisterMessageBrokers(builder, options);
            });

            RegisterUseCases(builder);
            RegisterServices(builder);
            RegisterInfrastructure(builder);
            RegisterViews(builder);
            RegisterPresenters(builder);
            RegisterInitializers(builder);
        }

        private void RegisterMessageBrokers(IContainerBuilder builder, MessagePipeOptions options)
        {
            builder.RegisterMessageBroker<PlaceBuildingRequestDTO>(options);
            builder.RegisterMessageBroker<BuildingPlacedDTO>(options);
            builder.RegisterMessageBroker<InsufficientGoldDTO>(options);
            builder.RegisterMessageBroker<RemoveBuildingRequestDTO>(options);
            builder.RegisterMessageBroker<BuildingRemovedDTO>(options);
            builder.RegisterMessageBroker<MoveBuildingRequestDTO>(options);
            builder.RegisterMessageBroker<BuildingMovedDTO>(options);
            builder.RegisterMessageBroker<UpgradeBuildingRequestDTO>(options);
            builder.RegisterMessageBroker<BuildingUpgradedDTO>(options);
            builder.RegisterMessageBroker<GoldChangedDTO>(options);
            builder.RegisterMessageBroker<BuildingSelectedDTO>(options);
            builder.RegisterMessageBroker<MoveModeStartedDTO>(options);
            builder.RegisterMessageBroker<MoveModeCancelledDTO>(options);
            builder.RegisterMessageBroker<BuildingTypeSelectedDTO>(options);
        }

        private void RegisterUseCases(IContainerBuilder builder)
        {
            builder.Register<IPlaceBuildingUseCase, PlaceBuildingUseCase>(Lifetime.Singleton);
            builder.Register<IRemoveBuildingUseCase, RemoveBuildingUseCase>(Lifetime.Singleton);
            builder.Register<IMoveBuildingUseCase, MoveBuildingUseCase>(Lifetime.Singleton);
            builder.Register<IUpgradeBuildingUseCase, UpgradeBuildingUseCase>(Lifetime.Singleton);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<IBuildingConfigService, BuildingConfigService>(Lifetime.Singleton);
        }

        private void RegisterInfrastructure(IContainerBuilder builder)
        {
            var prefabFactory = new BuildingPrefabFactory(_buildingPrefab);
            builder.RegisterInstance<IBuildingPrefabFactory>(prefabFactory);
        }

        private void RegisterViews(IContainerBuilder builder)
        {
            RegisterBuildingView(builder, _buildingView);
            RegisterBuildingPropertiesView(builder, _buildingPropertiesView);
            RegisterBuildingCatalogView(builder, _buildingCatalogView);
        }

        private void RegisterBuildingView(IContainerBuilder builder, BuildingView view)
        {
            if (view != null)
            {
                builder.RegisterComponent(view);
            }
            else
            {
                Debug.LogWarning("[BuildingInstaller] BuildingView not found! Create BuildingView GameObject in scene.");
            }
        }

        private void RegisterBuildingPropertiesView(IContainerBuilder builder, BuildingPropertiesView view)
        {
            if (view != null)
            {
                builder.RegisterComponent(view);
            }
            else
            {
                Debug.LogWarning("[BuildingInstaller] BuildingPropertiesView not found! Create BuildingPropertiesView GameObject in scene with UIDocument component.");
            }
        }

        private void RegisterBuildingCatalogView(IContainerBuilder builder, BuildingCatalogView view)
        {
            if (view != null)
            {
                builder.RegisterComponent(view);
            }
            else
            {
                Debug.LogWarning("[BuildingInstaller] BuildingCatalogView not found! Create BuildingCatalogView GameObject in scene with UIDocument component.");
            }
        }

        private void RegisterPresenters(IContainerBuilder builder)
        {
            builder.Register<BuildingPresenter>(Lifetime.Singleton);
            builder.Register<BuildingPropertiesPresenter>(Lifetime.Singleton);
            builder.Register<BuildingCatalogPresenter>(Lifetime.Singleton);
        }

        private void RegisterInitializers(IContainerBuilder builder)
        {
            builder.Register<BuildingInitializer>(Lifetime.Singleton);
        }
    }
}

