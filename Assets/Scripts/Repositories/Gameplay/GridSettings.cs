using UnityEngine;

namespace Repositories.Gameplay
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "Settings/Grid Settings")]
    public class GridSettings : ScriptableObject
    {
        [Header("Grid Dimensions")]
        [SerializeField] private int _width = 32;
        [SerializeField] private int _height = 32;

        public int Width => _width;
        public int Height => _height;
    }
}

