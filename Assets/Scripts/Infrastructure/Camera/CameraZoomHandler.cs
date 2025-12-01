using ContractsInterfaces.Infrastructure;
using Repositories.Gameplay;
using UnityEngine;

namespace Infrastructure.Camera
{
    public class CameraZoomHandler
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly CameraSettings _cameraSettings;

        public CameraZoomHandler(IInputAdapter inputAdapter, CameraSettings cameraSettings)
        {
            _inputAdapter = inputAdapter;
            _cameraSettings = cameraSettings;
        }

        public float Handle(float currentSize)
        {
            var zoomDelta = _inputAdapter.GetZoom();
            
            if (Mathf.Abs(zoomDelta) <= 0.01f)
            {
                return currentSize;
            }

            var newSize = currentSize - (zoomDelta * _cameraSettings.ZoomSpeed);
            return Mathf.Clamp(newSize, _cameraSettings.MinZoom, _cameraSettings.MaxZoom);
        }

        public float ZoomCamera(float zoomDelta, float currentSize)
        {
            var newSize = currentSize - (zoomDelta * _cameraSettings.ZoomSpeed * Time.deltaTime);
            return Mathf.Clamp(newSize, _cameraSettings.MinZoom, _cameraSettings.MaxZoom);
        }
    }
}

