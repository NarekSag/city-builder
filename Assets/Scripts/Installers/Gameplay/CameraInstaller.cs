using Infrastructure;
using Repositories.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers.Gameplay
{
    public class CameraInstaller : MonoBehaviour, IInstaller
    {
        [Header("Camera Settings")]
        [Tooltip("ScriptableObject с настройками камеры. Создайте через меню: Assets > Create > Settings > Camera Settings")]
        [SerializeField] private CameraSettings _cameraSettings;

        [Header("Camera Controller")]
        [Tooltip("CameraController компонент на Main Camera. Если не назначен, будет найден автоматически.")]
        [SerializeField] private CameraController _cameraController;

        public void Install(IContainerBuilder builder)
        {
            if (_cameraSettings != null)
            {
                builder.RegisterInstance(_cameraSettings);
            }
            else
            {
                Debug.LogWarning("[CameraInstaller] CameraSettings is not assigned! Camera may not work correctly.");
            }

            if (_cameraController == null)
            {
                _cameraController = FindFirstObjectByType<CameraController>();
            }

            if (_cameraController != null)
            {
                builder.RegisterComponent(_cameraController);
            }
            else
            {
                Debug.LogWarning("[CameraInstaller] CameraController not found! " +
                               "Add CameraController component to Main Camera and assign it here.");
            }
        }
    }
}

