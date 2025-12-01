using Application.Services;
using ContractsInterfaces.Infrastructure;
using ContractsInterfaces.UseCasesGameplay;
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
        [SerializeField] private GameObject _housePrefab;

        [Header("Building Components")]
        [SerializeField] private BuildingView _buildingView;

        public void Install(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();

            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.PlaceBuildingRequestDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.BuildingPlacedDTO>(options);
            builder.RegisterMessageBroker<Domain.Gameplay.MessagesDTO.InsufficientGoldDTO>(options);

            var economy = new Economy(500);
            builder.RegisterInstance(economy);

            builder.Register<IEconomyService, EconomyService>(Lifetime.Singleton);

            builder.Register<IPlaceBuildingUseCase, PlaceBuildingUseCase>(Lifetime.Singleton);

            var prefabFactory = new BuildingPrefabFactory(_housePrefab);
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

