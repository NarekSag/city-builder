using UnityEngine;

namespace Domain.Gameplay.Models
{
    public struct GridPosition
    {
        public int X { get; }
        public int Y { get; }

        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public GridPosition(Vector2Int position) : this(position.x, position.y)
        {
        }

        public bool IsValid(int width, int height)
        {
            return X >= 0 && X < width && Y >= 0 && Y < height;
        }

        public Vector2Int ToVector2Int()
        {
            return new Vector2Int(X, Y);
        }

        public static implicit operator Vector2Int(GridPosition position)
        {
            return position.ToVector2Int();
        }

        public static implicit operator GridPosition(Vector2Int vector)
        {
            return new GridPosition(vector.x, vector.y);
        }
    }
}

