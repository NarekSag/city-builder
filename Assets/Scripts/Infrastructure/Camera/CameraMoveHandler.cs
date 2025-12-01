using ContractsInterfaces.Infrastructure;
using Repositories.Gameplay;
using UnityEngine;

namespace Infrastructure.Camera
{
    public class CameraMoveHandler
    {
        private readonly IInputAdapter _inputAdapter;
        private readonly CameraSettings _cameraSettings;

        public CameraMoveHandler(IInputAdapter inputAdapter, CameraSettings cameraSettings)
        {
            _inputAdapter = inputAdapter;
            _cameraSettings = cameraSettings;
        }

        public Vector3 Handle(Vector3 currentPosition, Transform cameraTransform)
        {
            var movement = _inputAdapter.GetCameraMovement();
            
            if (movement.magnitude <= 0.01f)
            {
                return currentPosition;
            }

            var moveDirection = new Vector3(movement.x, 0, movement.y);
            var worldMove = cameraTransform.TransformDirection(moveDirection);
            worldMove.y = 0;
            worldMove.Normalize();

            var moveAmount = worldMove * _cameraSettings.MoveSpeed * Time.deltaTime;
            return currentPosition + moveAmount;
        }

        public Vector3 MoveCamera(Vector2 movement, Vector3 currentPosition, Transform cameraTransform)
        {
            var moveDirection = new Vector3(movement.x, 0, movement.y);
            var worldMove = cameraTransform.TransformDirection(moveDirection);
            worldMove.y = 0;
            worldMove.Normalize();

            var moveAmount = worldMove * _cameraSettings.MoveSpeed * Time.deltaTime;
            return currentPosition + moveAmount;
        }
    }
}

