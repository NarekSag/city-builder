using Presentation.Gameplay.Presenters;
using UnityEngine;
using VContainer;

namespace Infrastructure.UI
{
    public class HudInitializer
    {
        [Inject] private HudPresenter _hudPresenter;

        public void Initialize()
        {
            if (_hudPresenter != null)
            {
                _hudPresenter.Initialize();
            }
            else
            {
                Debug.LogWarning("[HudInitializer] HudPresenter is null!");
            }
        }
    }
}

