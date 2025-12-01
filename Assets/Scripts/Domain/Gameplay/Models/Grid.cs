#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Domain.Gameplay.Models
{
    public class Grid
    {
        private readonly Dictionary<Vector2Int, Building> _buildings;
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
            _buildings = new Dictionary<Vector2Int, Building>();
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

        public bool IsValidPosition(GridPosition position)
        {
            return position.IsValid(_width, _height);
        }

        public bool IsOccupied(Vector2Int position)
        {
            return _buildings.ContainsKey(position);
        }

        public bool IsOccupied(int x, int y)
        {
            return IsOccupied(new Vector2Int(x, y));
        }

        public bool IsOccupied(GridPosition position)
        {
            return IsOccupied(position.X, position.Y);
        }

        public bool GetBuildingAt(Vector2Int position, out Building building)
        {
            building = null;

            if (!IsValidPosition(position))
            {
                return false;
            }

            if (_buildings.TryGetValue(position, out var foundBuilding))
            {
                building = foundBuilding;
                return true;
            }

            return false;
        }

        public bool GetBuildingAt(GridPosition position, out Building building)
        {
            return GetBuildingAt(new Vector2Int(position.X, position.Y), out building);
        }

        public bool PlaceBuilding(Building building, Vector2Int position)
        {
            if (building == null)
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

            building.Position = new GridPosition(position);
            _buildings[position] = building;
            return true;
        }

        public bool PlaceBuilding(Building building, GridPosition position)
        {
            return PlaceBuilding(building, new Vector2Int(position.X, position.Y));
        }

        public bool RemoveBuilding(Vector2Int position, out Building building)
        {
            building = null;

            if (!IsValidPosition(position))
            {
                return false;
            }

            if (!_buildings.TryGetValue(position, out building))
            {
                return false;
            }

            _buildings.Remove(position);
            return true;
        }

        public bool RemoveBuilding(GridPosition position, out Building building)
        {
            return RemoveBuilding(new Vector2Int(position.X, position.Y), out building);
        }

        public bool MoveBuilding(Vector2Int from, Vector2Int to, out Building building)
        {
            building = null;

            if (!IsValidPosition(from) || !IsValidPosition(to))
            {
                return false;
            }

            if (!GetBuildingAt(from, out building))
            {
                return false;
            }

            if (IsOccupied(to))
            {
                return false;
            }

            _buildings.Remove(from);
            building.Position = new GridPosition(to);
            _buildings[to] = building;
            return true;
        }

        public bool MoveBuilding(GridPosition from, GridPosition to, out Building building)
        {
            return MoveBuilding(new Vector2Int(from.X, from.Y), new Vector2Int(to.X, to.Y), out building);
        }

        public IEnumerable<Vector2Int> GetOccupiedPositions()
        {
            return _buildings.Keys;
        }

        public IEnumerable<KeyValuePair<Vector2Int, Building>> GetAllBuildings()
        {
            return _buildings;
        }

        public void Clear()
        {
            _buildings.Clear();
        }

        public bool HasBuilding(int buildingId)
        {
            return _buildings.Values.Any(b => b.Id == buildingId);
        }

        public Vector2Int? GetBuildingPosition(int buildingId)
        {
            foreach (var kvp in _buildings)
            {
                if (kvp.Value.Id == buildingId)
                {
                    return kvp.Key;
                }
            }

            return null;
        }

        public GridPosition? GetBuildingGridPosition(int buildingId)
        {
            var position = GetBuildingPosition(buildingId);
            return position.HasValue ? new GridPosition(position.Value) : null;
        }
    }
}
