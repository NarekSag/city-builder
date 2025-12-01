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

        public void Install(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();

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

            builder.Register<IPlaceBuildingUseCase, PlaceBuildingUseCase>(Lifetime.Singleton);
            builder.Register<IRemoveBuildingUseCase, RemoveBuildingUseCase>(Lifetime.Singleton);
            builder.Register<IMoveBuildingUseCase, MoveBuildingUseCase>(Lifetime.Singleton);
            builder.Register<IUpgradeBuildingUseCase, UpgradeBuildingUseCase>(Lifetime.Singleton);

            builder.Register<IBuildingConfigService, BuildingConfigService>(Lifetime.Singleton);

            var prefabFactory = new BuildingPrefabFactory(_buildingPrefab);
            builder.RegisterInstance<IBuildingPrefabFactory>(prefabFactory);

            if (_buildingView == null)
            {
                _buildingView = FindFirstObjectByType<BuildingView>();
            }

            if (_buildingView != null)
            {
                builder.RegisterComponent(_buildingView);
            }
            else
            {
                Debug.LogWarning("[BuildingInstaller] BuildingView not found! Create BuildingView GameObject in scene.");
            }

            builder.Register<BuildingPresenter>(Lifetime.Singleton);

            builder.Register<BuildingInitializer>(Lifetime.Singleton);
        }
    }
}

