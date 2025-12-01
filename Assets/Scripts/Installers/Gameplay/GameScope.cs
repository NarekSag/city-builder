using Infrastructure;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers.Gameplay
{
    public class GameScope : LifetimeScope
    {
        [Header("Installers")]
        [Tooltip("Список MonoBehaviour компонентов, реализующих IInstaller. Добавьте CameraInstaller и другие Installers.")]
        [SerializeField] private CameraInstaller _cameraInstaller;
        [SerializeField] private InputInstaller _inputInstaller;

        protected override void Configure(IContainerBuilder builder)
        {
            _cameraInstaller.Install(builder);
            _inputInstaller.Install(builder);
            
            builder.RegisterEntryPoint<GameFlow>();
        }
    }
}

