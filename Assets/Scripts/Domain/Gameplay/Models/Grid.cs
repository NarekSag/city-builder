using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Domain.Gameplay.Models
{
    public class Grid
    {
        private readonly Dictionary<Vector2Int, int> _buildings;
        private readonly int _width;
        private readonly int _height;

        public Grid(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentException("Width must be greater than 0", nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentException("Height must be greater than 0", nameof(height));
            }

            _width = width;
            _height = height;
            _buildings = new Dictionary<Vector2Int, int>();
        }

        public int Width => _width;
        public int Height => _height;
        public Vector2Int Size => new Vector2Int(_width, _height);
        public int TotalCells => _width * _height;
        public int OccupiedCells => _buildings.Count;
        public int FreeCells => TotalCells - OccupiedCells;

        public bool IsValidPosition(Vector2Int position)
        {
            return position.x >= 0 && position.x < _width && position.y >= 0 && position.y < _height;
        }

        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }

        public bool IsOccupied(Vector2Int position)
        {
            return _buildings.ContainsKey(position);
        }

        public bool IsOccupied(int x, int y)
        {
            return IsOccupied(new Vector2Int(x, y));
        }

        public bool GetBuildingAt(Vector2Int position, out int buildingId)
        {
            buildingId = 0;

            if (!IsValidPosition(position))
            {
                return false;
            }

            if (_buildings.TryGetValue(position, out var id))
            {
                buildingId = id;
                return true;
            }

            return false;
        }

        public bool PlaceBuilding(int buildingId, Vector2Int position)
        {
            if (buildingId <= 0)
            {
                return false;
            }

            if (!IsValidPosition(position))
            {
                return false;
            }

            if (IsOccupied(position))
            {
                return false;
            }

            _buildings[position] = buildingId;
            return true;
        }

        public bool RemoveBuilding(Vector2Int position, out int buildingId)
        {
            buildingId = 0;

            if (!IsValidPosition(position))
            {
                return false;
            }

            if (!_buildings.TryGetValue(position, out buildingId))
            {
                return false;
            }

            _buildings.Remove(position);
            return true;
        }

        public bool MoveBuilding(Vector2Int from, Vector2Int to, out int buildingId)
        {
            buildingId = 0;

            if (!IsValidPosition(from) || !IsValidPosition(to))
            {
                return false;
            }

            if (!GetBuildingAt(from, out buildingId))
            {
                return false;
            }

            if (IsOccupied(to))
            {
                return false;
            }

            _buildings.Remove(from);
            _buildings[to] = buildingId;
            return true;
        }

        public IEnumerable<Vector2Int> GetOccupiedPositions()
        {
            return _buildings.Keys;
        }

        public IEnumerable<KeyValuePair<Vector2Int, int>> GetAllBuildings()
        {
            return _buildings;
        }

        public void Clear()
        {
            _buildings.Clear();
        }

        public bool HasBuilding(int buildingId)
        {
            return _buildings.Values.Contains(buildingId);
        }

        public Vector2Int? GetBuildingPosition(int buildingId)
        {
            foreach (var kvp in _buildings)
            {
                if (kvp.Value == buildingId)
                {
                    return kvp.Key;
                }
            }

            return null;
        }
    }
}

