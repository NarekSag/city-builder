using ContractsInterfaces.Infrastructure;
using Repositories.Gameplay;
using UnityEngine;
using VContainer;

namespace Infrastructure
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraController : MonoBehaviour
    {
        [Inject] private IInputAdapter _inputAdapter;
        [Inject] private CameraSettings _cameraSettings;

        private UnityEngine.Camera _camera;
        private Camera.CameraMoveHandler _moveHandler;
        private Camera.CameraZoomHandler _zoomHandler;
        private Camera.CameraDragHandler _dragHandler;
        private Camera.CameraBoundaryHandler _boundaryHandler;

        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }

        private void Start()
        {
            if (_inputAdapter == null || _cameraSettings == null)
            {
                Debug.LogError("[CameraController] IInputAdapter or CameraSettings are not assigned!");
                return;
            }

            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            _moveHandler = new Camera.CameraMoveHandler(_inputAdapter, _cameraSettings);
            _zoomHandler = new Camera.CameraZoomHandler(_inputAdapter, _cameraSettings);
            _dragHandler = new Camera.CameraDragHandler(_inputAdapter, _cameraSettings);
            _boundaryHandler = new Camera.CameraBoundaryHandler(_cameraSettings);
        }

        private void Update()
        {
            if (_moveHandler == null || _zoomHandler == null || _dragHandler == null || _boundaryHandler == null)
            {
                return;
            }

            var position = transform.position;

            position = _moveHandler.Handle(position, transform);
            position = _dragHandler.Handle(position, transform);
            position = _boundaryHandler.Clamp(position);
            transform.position = position;

            var newSize = _zoomHandler.Handle(_camera.orthographicSize);
            _camera.orthographicSize = newSize;
        }

        public void MoveCamera(Vector2 movement)
        {
            if (_moveHandler == null) return;
            
            var newPosition = _moveHandler.MoveCamera(movement, transform.position, transform);
            transform.position = _boundaryHandler?.Clamp(newPosition) ?? newPosition;
        }

        public void ZoomCamera(float zoomDelta)
        {
            if (_zoomHandler == null) return;
            
            var newSize = _zoomHandler.ZoomCamera(zoomDelta, _camera.orthographicSize);
            _camera.orthographicSize = newSize;
        }
    }
}

