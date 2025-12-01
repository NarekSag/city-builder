using ContractsInterfaces.Infrastructure;
using Repositories.Gameplay;
using UnityEngine;

namespace Infrastructure.Camera
{
    public class CameraDragHandler
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly CameraSettings _cameraSettings;

        public CameraDragHandler(IInputAdapter inputAdapter, CameraSettings cameraSettings)
        {
            _inputAdapter = inputAdapter;
            _cameraSettings = cameraSettings;
        }

        public Vector3 Handle(Vector3 currentPosition, Transform cameraTransform)
        {
            var dragDelta = _inputAdapter.GetMouseDrag();
            
            if (dragDelta.magnitude <= 0.01f)
            {
                return currentPosition;
            }

            var dragDirection = new Vector3(-dragDelta.x, 0, -dragDelta.y);
            var worldDrag = cameraTransform.TransformDirection(dragDirection);
            worldDrag.y = 0;
            worldDrag.Normalize();

            var dragAmount = worldDrag * _cameraSettings.DragSpeed;
            return currentPosition + dragAmount;
        }
    }
}

