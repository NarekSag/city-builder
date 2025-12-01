using NUnit.Framework;
using Domain.Gameplay.Models;
using UnityEngine;

using Grid = Domain.Gameplay.Models.Grid;

namespace Tests.Domain
{
    [TestFixture]
    public class GridTests
    {
        [Test]
        public void Grid_Create_ValidSize()
        {
            var grid = new Grid(32, 32);
            
            Assert.AreEqual(32, grid.Width);
            Assert.AreEqual(32, grid.Height);
            Assert.AreEqual(new Vector2Int(32, 32), grid.Size);
            Assert.AreEqual(1024, grid.TotalCells);
            Assert.AreEqual(0, grid.OccupiedCells);
            Assert.AreEqual(1024, grid.FreeCells);
        }

        [Test]
        public void Grid_Create_InvalidWidth_ThrowsException()
        {
            Assert.Throws<System.ArgumentException>(() => new Grid(0, 32));
            Assert.Throws<System.ArgumentException>(() => new Grid(-1, 32));
        }

        [Test]
        public void Grid_Create_InvalidHeight_ThrowsException()
        {
            Assert.Throws<System.ArgumentException>(() => new Grid(32, 0));
            Assert.Throws<System.ArgumentException>(() => new Grid(32, -1));
        }

        [Test]
        public void Grid_IsValidPosition_ValidPositions()
        {
            var grid = new Grid(32, 32);
            
            Assert.IsTrue(grid.IsValidPosition(0, 0));
            Assert.IsTrue(grid.IsValidPosition(31, 31));
            Assert.IsTrue(grid.IsValidPosition(15, 15));
            Assert.IsTrue(grid.IsValidPosition(new Vector2Int(0, 0)));
            Assert.IsTrue(grid.IsValidPosition(new Vector2Int(31, 31)));
            Assert.IsTrue(grid.IsValidPosition(new GridPosition(0, 0)));
            Assert.IsTrue(grid.IsValidPosition(new GridPosition(31, 31)));
        }

        [Test]
        public void Grid_IsValidPosition_InvalidPositions()
        {
            var grid = new Grid(32, 32);
            
            Assert.IsFalse(grid.IsValidPosition(-1, 0));
            Assert.IsFalse(grid.IsValidPosition(0, -1));
            Assert.IsFalse(grid.IsValidPosition(32, 0));
            Assert.IsFalse(grid.IsValidPosition(0, 32));
            Assert.IsFalse(grid.IsValidPosition(100, 100));
            Assert.IsFalse(grid.IsValidPosition(new Vector2Int(-1, 0)));
            Assert.IsFalse(grid.IsValidPosition(new Vector2Int(32, 32)));
            Assert.IsFalse(grid.IsValidPosition(new GridPosition(-1, 0)));
            Assert.IsFalse(grid.IsValidPosition(new GridPosition(32, 32)));
        }

        [Test]
        public void Grid_PlaceBuilding_Success()
        {
            var grid = new Grid(32, 32);
            var building = new Building(1, BuildingType.House, 1, new GridPosition(5, 5));
            bool result = grid.PlaceBuilding(building, new Vector2Int(5, 5));
            
            Assert.IsTrue(result);
            Assert.IsTrue(grid.IsOccupied(5, 5));
            Assert.IsTrue(grid.IsOccupied(new Vector2Int(5, 5)));
            Assert.AreEqual(1, grid.OccupiedCells);
            Assert.AreEqual(1023, grid.FreeCells);
        }

        [Test]
        public void Grid_PlaceBuilding_InvalidPosition_ReturnsFalse()
        {
            var grid = new Grid(32, 32);
            var building = new Building(1, BuildingType.House, 1, new GridPosition(0, 0));
            
            Assert.IsFalse(grid.PlaceBuilding(building, new Vector2Int(-1, 0)));
            Assert.IsFalse(grid.PlaceBuilding(building, new Vector2Int(32, 32)));
            Assert.IsFalse(grid.PlaceBuilding(building, new Vector2Int(100, 100)));
            Assert.AreEqual(0, grid.OccupiedCells);
        }

        [Test]
        public void Grid_PlaceBuilding_OccupiedCell_ReturnsFalse()
        {
            var grid = new Grid(32, 32);
            var building1 = new Building(1, BuildingType.House, 1, new GridPosition(5, 5));
            grid.PlaceBuilding(building1, new Vector2Int(5, 5));
            
            var building2 = new Building(2, BuildingType.Farm, 1, new GridPosition(5, 5));
            bool result = grid.PlaceBuilding(building2, new Vector2Int(5, 5));
            
            Assert.IsFalse(result);
            Assert.AreEqual(1, grid.OccupiedCells);
        }

        [Test]
        public void Grid_PlaceBuilding_NullBuilding_ReturnsFalse()
        {
            var grid = new Grid(32, 32);
            
            bool result = grid.PlaceBuilding(null, new Vector2Int(5, 5));
            
            Assert.IsFalse(result);
            Assert.AreEqual(0, grid.OccupiedCells);
        }

        [Test]
        public void Grid_GetBuildingAt_Success()
        {
            var grid = new Grid(32, 32);
            var building = new Building(42, BuildingType.House, 1, new GridPosition(10, 15));
            grid.PlaceBuilding(building, new Vector2Int(10, 15));
            
            bool found = grid.GetBuildingAt(new Vector2Int(10, 15), out Building foundBuilding);
            
            Assert.IsTrue(found);
            Assert.IsNotNull(foundBuilding);
            Assert.AreEqual(42, foundBuilding.Id);
            Assert.AreEqual(BuildingType.House, foundBuilding.Type);
        }

        [Test]
        public void Grid_GetBuildingAt_EmptyCell_ReturnsFalse()
        {
            var grid = new Grid(32, 32);
            
            bool found = grid.GetBuildingAt(new Vector2Int(10, 15), out Building building);
            
            Assert.IsFalse(found);
            Assert.IsNull(building);
        }

        [Test]
        public void Grid_GetBuildingAt_InvalidPosition_ReturnsFalse()
        {
            var grid = new Grid(32, 32);
            
            bool found = grid.GetBuildingAt(new Vector2Int(-1, 0), out Building building);
            
            Assert.IsFalse(found);
            Assert.IsNull(building);
        }

        [Test]
        public void Grid_RemoveBuilding_Success()
        {
            var grid = new Grid(32, 32);
            var building = new Building(42, BuildingType.House, 1, new GridPosition(10, 15));
            grid.PlaceBuilding(building, new Vector2Int(10, 15));
            
            bool removed = grid.RemoveBuilding(new Vector2Int(10, 15), out Building removedBuilding);
            
            Assert.IsTrue(removed);
            Assert.IsNotNull(removedBuilding);
            Assert.AreEqual(42, removedBuilding.Id);
            Assert.IsFalse(grid.IsOccupied(10, 15));
            Assert.AreEqual(0, grid.OccupiedCells);
        }

        [Test]
        public void Grid_RemoveBuilding_EmptyCell_ReturnsFalse()
        {
            var grid = new Grid(32, 32);
            
            bool removed = grid.RemoveBuilding(new Vector2Int(10, 15), out Building building);
            
            Assert.IsFalse(removed);
            Assert.IsNull(building);
        }

        [Test]
        public void Grid_RemoveBuilding_InvalidPosition_ReturnsFalse()
        {
            var grid = new Grid(32, 32);
            
            bool removed = grid.RemoveBuilding(new Vector2Int(-1, 0), out Building building);
            
            Assert.IsFalse(removed);
            Assert.IsNull(building);
        }

        [Test]
        public void Grid_MoveBuilding_Success()
        {
            var grid = new Grid(32, 32);
            var building = new Building(42, BuildingType.House, 1, new GridPosition(5, 5));
            grid.PlaceBuilding(building, new Vector2Int(5, 5));
            
            bool moved = grid.MoveBuilding(new Vector2Int(5, 5), new Vector2Int(10, 10), out Building movedBuilding);
            
            Assert.IsTrue(moved);
            Assert.IsNotNull(movedBuilding);
            Assert.AreEqual(42, movedBuilding.Id);
            Assert.IsFalse(grid.IsOccupied(5, 5));
            Assert.IsTrue(grid.IsOccupied(10, 10));
            Assert.AreEqual(1, grid.OccupiedCells);
        }

        [Test]
        public void Grid_MoveBuilding_InvalidFromPosition_ReturnsFalse()
        {
            var grid = new Grid(32, 32);
            
            bool moved = grid.MoveBuilding(new Vector2Int(-1, 0), new Vector2Int(10, 10), out Building building);
            
            Assert.IsFalse(moved);
            Assert.IsNull(building);
        }

        [Test]
        public void Grid_MoveBuilding_InvalidToPosition_ReturnsFalse()
        {
            var grid = new Grid(32, 32);
            var building = new Building(42, BuildingType.House, 1, new GridPosition(5, 5));
            grid.PlaceBuilding(building, new Vector2Int(5, 5));
            
            bool moved = grid.MoveBuilding(new Vector2Int(5, 5), new Vector2Int(100, 100), out Building movedBuilding);
            
            Assert.IsFalse(moved);
            Assert.IsTrue(grid.IsOccupied(5, 5));
        }

        [Test]
        public void Grid_MoveBuilding_ToOccupiedCell_ReturnsFalse()
        {
            var grid = new Grid(32, 32);
            var building1 = new Building(1, BuildingType.House, 1, new GridPosition(5, 5));
            var building2 = new Building(2, BuildingType.Farm, 1, new GridPosition(10, 10));
            grid.PlaceBuilding(building1, new Vector2Int(5, 5));
            grid.PlaceBuilding(building2, new Vector2Int(10, 10));
            
            bool moved = grid.MoveBuilding(new Vector2Int(5, 5), new Vector2Int(10, 10), out Building movedBuilding);
            
            Assert.IsFalse(moved);
            Assert.IsTrue(grid.IsOccupied(5, 5));
            Assert.IsTrue(grid.IsOccupied(10, 10));
        }

        [Test]
        public void Grid_MoveBuilding_FromEmptyCell_ReturnsFalse()
        {
            var grid = new Grid(32, 32);
            
            bool moved = grid.MoveBuilding(new Vector2Int(5, 5), new Vector2Int(10, 10), out Building building);
            
            Assert.IsFalse(moved);
            Assert.IsNull(building);
        }

        [Test]
        public void Grid_GetOccupiedPositions_ReturnsAllPositions()
        {
            var grid = new Grid(32, 32);
            var building1 = new Building(1, BuildingType.House, 1, new GridPosition(5, 5));
            var building2 = new Building(2, BuildingType.Farm, 1, new GridPosition(10, 10));
            var building3 = new Building(3, BuildingType.Mine, 1, new GridPosition(15, 15));
            grid.PlaceBuilding(building1, new Vector2Int(5, 5));
            grid.PlaceBuilding(building2, new Vector2Int(10, 10));
            grid.PlaceBuilding(building3, new Vector2Int(15, 15));
            
            var positions = grid.GetOccupiedPositions();
            
            Assert.AreEqual(3, System.Linq.Enumerable.Count(positions));
            Assert.Contains(new Vector2Int(5, 5), System.Linq.Enumerable.ToList(positions));
            Assert.Contains(new Vector2Int(10, 10), System.Linq.Enumerable.ToList(positions));
            Assert.Contains(new Vector2Int(15, 15), System.Linq.Enumerable.ToList(positions));
        }

        [Test]
        public void Grid_GetAllBuildings_ReturnsAllBuildings()
        {
            var grid = new Grid(32, 32);
            var building1 = new Building(1, BuildingType.House, 1, new GridPosition(5, 5));
            var building2 = new Building(2, BuildingType.Farm, 1, new GridPosition(10, 10));
            grid.PlaceBuilding(building1, new Vector2Int(5, 5));
            grid.PlaceBuilding(building2, new Vector2Int(10, 10));
            
            var buildings = grid.GetAllBuildings();
            var buildingsList = System.Linq.Enumerable.ToList(buildings);
            
            Assert.AreEqual(2, buildingsList.Count);
            Assert.IsTrue(buildingsList.Exists(kvp => kvp.Key == new Vector2Int(5, 5) && kvp.Value.Id == 1));
            Assert.IsTrue(buildingsList.Exists(kvp => kvp.Key == new Vector2Int(10, 10) && kvp.Value.Id == 2));
        }

        [Test]
        public void Grid_Clear_RemovesAllBuildings()
        {
            var grid = new Grid(32, 32);
            var building1 = new Building(1, BuildingType.House, 1, new GridPosition(5, 5));
            var building2 = new Building(2, BuildingType.Farm, 1, new GridPosition(10, 10));
            var building3 = new Building(3, BuildingType.Mine, 1, new GridPosition(15, 15));
            grid.PlaceBuilding(building1, new Vector2Int(5, 5));
            grid.PlaceBuilding(building2, new Vector2Int(10, 10));
            grid.PlaceBuilding(building3, new Vector2Int(15, 15));
            
            Assert.AreEqual(3, grid.OccupiedCells);
            
            grid.Clear();
            
            Assert.AreEqual(0, grid.OccupiedCells);
            Assert.AreEqual(1024, grid.FreeCells);
            Assert.IsFalse(grid.IsOccupied(5, 5));
            Assert.IsFalse(grid.IsOccupied(10, 10));
            Assert.IsFalse(grid.IsOccupied(15, 15));
        }

        [Test]
        public void Grid_HasBuilding_ReturnsTrue_WhenBuildingExists()
        {
            var grid = new Grid(32, 32);
            var building = new Building(42, BuildingType.House, 1, new GridPosition(10, 15));
            grid.PlaceBuilding(building, new Vector2Int(10, 15));
            
            Assert.IsTrue(grid.HasBuilding(42));
        }

        [Test]
        public void Grid_HasBuilding_ReturnsFalse_WhenBuildingNotExists()
        {
            var grid = new Grid(32, 32);
            var building = new Building(1, BuildingType.House, 1, new GridPosition(10, 15));
            grid.PlaceBuilding(building, new Vector2Int(10, 15));
            
            Assert.IsFalse(grid.HasBuilding(42));
            Assert.IsFalse(grid.HasBuilding(0));
        }

        [Test]
        public void Grid_GetBuildingPosition_ReturnsPosition_WhenBuildingExists()
        {
            var grid = new Grid(32, 32);
            var building = new Building(42, BuildingType.House, 1, new GridPosition(10, 15));
            grid.PlaceBuilding(building, new Vector2Int(10, 15));
            
            var position = grid.GetBuildingPosition(42);
            
            Assert.IsNotNull(position);
            Assert.AreEqual(new Vector2Int(10, 15), position.Value);
        }

        [Test]
        public void Grid_GetBuildingPosition_ReturnsNull_WhenBuildingNotExists()
        {
            var grid = new Grid(32, 32);
            var building = new Building(1, BuildingType.House, 1, new GridPosition(10, 15));
            grid.PlaceBuilding(building, new Vector2Int(10, 15));
            
            var position = grid.GetBuildingPosition(42);
            
            Assert.IsNull(position);
        }

        [Test]
        public void Grid_MultipleBuildings_StatisticsCorrect()
        {
            var grid = new Grid(32, 32);
            
            for (int i = 1; i <= 10; i++)
            {
                var building = new Building(i, BuildingType.House, 1, new GridPosition(i, i));
                grid.PlaceBuilding(building, new Vector2Int(i, i));
            }
            
            Assert.AreEqual(10, grid.OccupiedCells);
            Assert.AreEqual(1014, grid.FreeCells);
            Assert.AreEqual(1024, grid.TotalCells);
        }

        [Test]
        public void Grid_DifferentSizes_WorksCorrectly()
        {
            var grid1 = new Grid(10, 10);
            Assert.AreEqual(100, grid1.TotalCells);
            
            var grid2 = new Grid(50, 50);
            Assert.AreEqual(2500, grid2.TotalCells);
            
            var grid3 = new Grid(1, 1);
            Assert.AreEqual(1, grid3.TotalCells);
            var building = new Building(1, BuildingType.House, 1, new GridPosition(0, 0));
            Assert.IsTrue(grid3.PlaceBuilding(building, new Vector2Int(0, 0)));
            Assert.AreEqual(0, grid3.FreeCells);
        }
    }
}
