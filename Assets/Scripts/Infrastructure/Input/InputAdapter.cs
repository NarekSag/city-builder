using ContractsInterfaces.Infrastructure;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Input
{
    public class InputAdapter : IInputAdapter
    {
        private const string CameraActionMapName = "Camera";
        private const string HotkeysActionMapName = "Hotkeys";
        private const string MoveActionName = "Move";
        private const string ZoomActionName = "Zoom";
        private const string DragActionName = "Drag";
        private const string Building1ActionName = "Building1";
        private const string Building2ActionName = "Building2";
        private const string Building3ActionName = "Building3";
        private const string DeleteActionName = "Delete";
        private const string RotateActionName = "Rotate";

        private InputActionMap _cameraActionMap;
        private InputActionMap _hotkeysActionMap;
        private InputAction _moveAction;
        private InputAction _zoomAction;
        private InputAction _dragAction;
        private InputAction _building1Action;
        private InputAction _building2Action;
        private InputAction _building3Action;
        private InputAction _deleteAction;
        private InputAction _rotateAction;

        private Vector2 _lastMousePosition;
        private bool _isDragging;

        public InputAdapter(InputActionAsset inputActions)
        {
            if (inputActions == null)
            {
                Debug.LogError("[InputAdapter] Input Actions Asset is not assigned!");
                return;
            }

            _cameraActionMap = inputActions.FindActionMap(CameraActionMapName);
            
            if (_cameraActionMap == null)
            {
                Debug.LogError($"[InputAdapter] Action Map '{CameraActionMapName}' not found in Input Actions!");
                return;
            }

            _hotkeysActionMap = inputActions.FindActionMap(HotkeysActionMapName);
            
            if (_hotkeysActionMap == null)
            {
                Debug.LogWarning($"[InputAdapter] Action Map '{HotkeysActionMapName}' not found in Input Actions! Hotkeys may not work.");
            }

            _moveAction = _cameraActionMap.FindAction(MoveActionName);
            _zoomAction = _cameraActionMap.FindAction(ZoomActionName);
            _dragAction = _cameraActionMap.FindAction(DragActionName);
            
            if (_hotkeysActionMap != null)
            {
                _building1Action = _hotkeysActionMap.FindAction(Building1ActionName);
                _building2Action = _hotkeysActionMap.FindAction(Building2ActionName);
                _building3Action = _hotkeysActionMap.FindAction(Building3ActionName);
                _deleteAction = _hotkeysActionMap.FindAction(DeleteActionName);
                _rotateAction = _hotkeysActionMap.FindAction(RotateActionName);
            }

            _cameraActionMap?.Enable();
            _hotkeysActionMap?.Enable();
        }

        public Vector2 GetCameraMovement()
        {
            if (_moveAction == null) return Vector2.zero;
            return _moveAction.ReadValue<Vector2>();
        }

        public float GetZoom()
        {
            if (_zoomAction == null || Mouse.current == null) return 0f;
            var scrollValue = _zoomAction.ReadValue<Vector2>();
            return scrollValue.y;
        }

        public Vector2 GetMouseDrag()
        {
            if (_dragAction == null || Mouse.current == null) return Vector2.zero;

            var isPressed = _dragAction.IsPressed();
            var currentMousePosition = Mouse.current.position.ReadValue();

            if (isPressed && !_isDragging)
            {
                _isDragging = true;
                _lastMousePosition = currentMousePosition;
                return Vector2.zero;
            }

            if (isPressed && _isDragging)
            {
                var delta = currentMousePosition - _lastMousePosition;
                _lastMousePosition = currentMousePosition;
                return delta;
            }

            if (!isPressed && _isDragging)
            {
                _isDragging = false;
            }

            return Vector2.zero;
        }

        public bool IsBuilding1Pressed()
        {
            return _building1Action?.WasPressedThisFrame() ?? false;
        }

        public bool IsBuilding2Pressed()
        {
            return _building2Action?.WasPressedThisFrame() ?? false;
        }

        public bool IsBuilding3Pressed()
        {
            return _building3Action?.WasPressedThisFrame() ?? false;
        }

        public bool IsDeletePressed()
        {
            return _deleteAction?.WasPressedThisFrame() ?? false;
        }

        public bool IsRotatePressed()
        {
            return _rotateAction?.WasPressedThisFrame() ?? false;
        }
    }
}

