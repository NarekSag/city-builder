namespace Domain.Gameplay.Rules
{
    public static class BuildingPlacementRules
    {
        public static bool CanPlace(Models.Grid grid, Models.GridPosition position)
        {
            if (grid == null)
            {
                return false;
            }

            return IsPositionValid(grid, position) && !IsCellOccupied(grid, position);
        }

        public static bool IsPositionValid(Models.Grid grid, Models.GridPosition position)
        {
            if (grid == null)
            {
                return false;
            }

            return grid.IsValidPosition(position);
        }

        public static bool IsCellOccupied(Models.Grid grid, Models.GridPosition position)
        {
            if (grid == null)
            {
                return false;
            }

            return grid.IsOccupied(position);
        }
    }
}

