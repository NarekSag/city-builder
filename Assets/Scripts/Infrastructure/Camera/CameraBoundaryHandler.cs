using Repositories.Gameplay;
using UnityEngine;

namespace Infrastructure.Camera
{
    public class CameraBoundaryHandler
    {
        private readonly CameraSettings _cameraSettings;

        public CameraBoundaryHandler(CameraSettings cameraSettings)
        {
            _cameraSettings = cameraSettings;
        }

        public Vector3 Clamp(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, _cameraSettings.MinX, _cameraSettings.MaxX);
            position.z = Mathf.Clamp(position.z, _cameraSettings.MinZ, _cameraSettings.MaxZ);
            position.y = Mathf.Clamp(position.y, _cameraSettings.MinY, _cameraSettings.MaxY);
            
            return position;
        }
    }
}

