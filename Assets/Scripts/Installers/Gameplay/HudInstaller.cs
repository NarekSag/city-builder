using Presentation.Gameplay.Presenters;
using Presentation.Gameplay.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers.Gameplay
{
    public class HudInstaller : MonoBehaviour, IInstaller
    {
        [Header("HUD Components")]
        [SerializeField] private HudView _hudView;

        public void Install(IContainerBuilder builder)
        {
            // MessagePipe and message brokers (GoldChangedDTO, InsufficientGoldDTO) 
            // are already registered by BuildingInstaller, so we don't need to register them again

            if (_hudView == null)
            {
                _hudView = FindFirstObjectByType<HudView>();
            }

            if (_hudView != null)
            {
                builder.RegisterComponent(_hudView);
            }
            else
            {
                Debug.LogWarning("[HudInstaller] HudView not found! Create HudView GameObject in scene with UIDocument component.");
            }

            builder.Register<HudPresenter>(Lifetime.Singleton);

            builder.Register<Infrastructure.UI.HudInitializer>(Lifetime.Singleton);
        }
    }
}

