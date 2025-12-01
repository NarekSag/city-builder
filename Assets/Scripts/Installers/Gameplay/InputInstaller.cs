using ContractsInterfaces.Infrastructure;
using Infrastructure.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Installers.Gameplay
{
    public class InputInstaller : MonoBehaviour, IInstaller
    {
        [Header("Input Actions")]
        [Tooltip("Input Actions Asset с настроенными Action Maps (Camera и Hotkeys).")]
        [SerializeField] private InputActionAsset _inputActions;

        public void Install(IContainerBuilder builder)
        {
            if (_inputActions == null)
            {
                Debug.LogError("[InputInstaller] Input Actions Asset is not assigned! Input will not work.");
                return;
            }

            builder.RegisterInstance(_inputActions);

            builder.Register<IInputAdapter, InputAdapter>(Lifetime.Singleton);
        }
    }
}

