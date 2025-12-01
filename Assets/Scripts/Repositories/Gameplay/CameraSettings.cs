using UnityEngine;

namespace Repositories.Gameplay
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/Camera Settings")]
    public class CameraSettings : ScriptableObject
    {
        [Header("Movement Settings")]
        [Tooltip("Скорость перемещения камеры при использовании WASD/стрелок.")]
        [SerializeField] private float _moveSpeed = 10f;

        [Header("Zoom Settings")]
        [Tooltip("Скорость изменения зума при прокрутке колеса мыши.")]
        [SerializeField] private float _zoomSpeed = 2f;

        [Tooltip("Минимальный размер ортографической камеры (максимальное приближение).")]
        [SerializeField] private float _minZoom = 5f;

        [Tooltip("Максимальный размер ортографической камеры (максимальное отдаление).")]
        [SerializeField] private float _maxZoom = 30f;

        [Header("Drag Settings")]
        [Tooltip("Скорость перемещения камеры при перетаскивании правой кнопкой мыши.")]
        [SerializeField] private float _dragSpeed = 5f;

        [Header("Boundaries")]
        [Tooltip("Минимальная позиция камеры по X (левая граница карты).")]
        [SerializeField] private float _minX = -32f;

        [Tooltip("Максимальная позиция камеры по X (правая граница карты).")]
        [SerializeField] private float _maxX = 32f;

        [Tooltip("Минимальная позиция камеры по Z (нижняя граница карты).")]
        [SerializeField] private float _minZ = -32f;

        [Tooltip("Максимальная позиция камеры по Z (верхняя граница карты).")]
        [SerializeField] private float _maxZ = 32f;

        [Tooltip("Минимальная высота камеры.")]
        [SerializeField] private float _minY = 10f;

        [Tooltip("Максимальная высота камеры.")]
        [SerializeField] private float _maxY = 50f;

        public float MoveSpeed => _moveSpeed;
        public float ZoomSpeed => _zoomSpeed;
        public float MinZoom => _minZoom;
        public float MaxZoom => _maxZoom;
        public float DragSpeed => _dragSpeed;
        public float MinX => _minX;
        public float MaxX => _maxX;
        public float MinZ => _minZ;
        public float MaxZ => _maxZ;
        public float MinY => _minY;
        public float MaxY => _maxY;
    }
}

