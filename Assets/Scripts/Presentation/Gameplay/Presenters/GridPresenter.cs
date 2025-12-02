using System;
using ContractsInterfaces.Infrastructure;
using Domain.Gameplay.MessagesDTO;
using Domain.Gameplay.Models;
using MessagePipe;
using Presentation.Gameplay.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Grid = Domain.Gameplay.Models.Grid;

namespace Presentation.Gameplay.Presenters
{
    public class GridPresenter : IInitializable, IDisposable
    {
        [Inject] private Grid _grid;
        [Inject] private GridView _view;
        [Inject] private IPublisher<PlaceBuildingRequestDTO> _placeBuildingPublisher;
        [Inject] private IPublisher<BuildingSelectedDTO> _buildingSelectedPublisher;
        [Inject] private IPublisher<MoveBuildingRequestDTO> _moveBuildingPublisher;
        [Inject] private IPublisher<MoveModeCancelledDTO> _moveModeCancelledPublisher;
        [Inject] private ISubscriber<MoveModeStartedDTO> _moveModeStartedSubscriber;
        [Inject] private ISubscriber<MoveModeCancelledDTO> _moveModeCancelledSubscriber;
        [Inject] private ISubscriber<BuildingTypeSelectedDTO> _buildingTypeSelectedSubscriber;
        [Inject] private IPublisher<BuildingTypeSelectedDTO> _buildingTypeSelectedPublisher;
        [Inject] private IInputAdapter _inputAdapter;

        private BuildingType _selectedBuildingType = BuildingType.House;
        private int? _buildingToMoveId;
        private IDisposable _moveModeStartedSubscription;
        private IDisposable _moveModeCancelledSubscription;
        private IDisposable _buildingTypeSelectedSubscription;

        public void Initialize()
        {
            _view.OnWorldPositionHovered += HandleWorldPositionHovered;
            _view.OnHoverExit += HandleHoverExit;
            _view.OnWorldPositionClicked += HandleWorldPositionClicked;

            _moveModeStartedSubscription = _moveModeStartedSubscriber.Subscribe(OnMoveModeStarted);
            _moveModeCancelledSubscription = _moveModeCancelledSubscriber.Subscribe(OnMoveModeCancelled);
            _buildingTypeSelectedSubscription = _buildingTypeSelectedSubscriber.Subscribe(OnBuildingTypeSelected);
        }

        private void OnMoveModeStarted(MoveModeStartedDTO dto)
        {
            _buildingToMoveId = dto.BuildingId;
        }

        private void OnMoveModeCancelled(MoveModeCancelledDTO dto)
        {
            _buildingToMoveId = null;
        }

        private void OnBuildingTypeSelected(BuildingTypeSelectedDTO dto)
        {
            _selectedBuildingType = dto.BuildingType;
        }

        private void CheckHotkeys()
        {
            if (_inputAdapter == null)
            {
                return;
            }

            if (_inputAdapter.IsBuilding1Pressed())
            {
                _selectedBuildingType = BuildingType.House;
                PublishBuildingTypeSelected(BuildingType.House);
            }
            else if (_inputAdapter.IsBuilding2Pressed())
            {
                _selectedBuildingType = BuildingType.Farm;
                PublishBuildingTypeSelected(BuildingType.Farm);
            }
            else if (_inputAdapter.IsBuilding3Pressed())
            {
                _selectedBuildingType = BuildingType.Mine;
                PublishBuildingTypeSelected(BuildingType.Mine);
            }
        }

        private void PublishBuildingTypeSelected(BuildingType buildingType)
        {
            var dto = new BuildingTypeSelectedDTO
            {
                BuildingType = buildingType
            };
            _buildingTypeSelectedPublisher.Publish(dto);
        }

        private void HandleWorldPositionHovered(Vector3 worldPosition)
        {
            CheckHotkeys();
            
            var gridPosition = WorldToGridPosition(worldPosition);
            if (!gridPosition.HasValue)
            {
                return;
            }

            var position = gridPosition.Value;
            Color color;

            if (_buildingToMoveId.HasValue)
            {
                // Move mode: highlight differently
                var buildingPosition = _grid.GetBuildingGridPosition(_buildingToMoveId.Value);
                if (buildingPosition.HasValue && position == new Vector2Int(buildingPosition.Value.X, buildingPosition.Value.Y))
                {
                    // Current position of building being moved
                    color = Color.yellow;
                }
                else if (!_grid.IsValidPosition(position) || _grid.IsOccupied(position))
                {
                    color = Color.red;
                }
                else
                {
                    color = Color.cyan; // Valid position for move
                }
            }
            else
            {
                // Normal mode
                if (!_grid.IsValidPosition(position) || _grid.IsOccupied(position))
                {
                    color = Color.red;
                }
                else
                {
                    color = Color.green;
                }
            }

            _view.HighlightCell(position, color);
        }

        private void HandleHoverExit()
        {
            _view.ClearHighlight();
        }

        private void HandleWorldPositionClicked(Vector3 worldPosition)
        {
            CheckHotkeys();
            
            var gridPosition = WorldToGridPosition(worldPosition);
            if (!gridPosition.HasValue)
            {
                return;
            }

            var position = gridPosition.Value;
            if (!_grid.IsValidPosition(position))
            {
                return;
            }

            // Check if we're in move mode
            if (_buildingToMoveId.HasValue)
            {
                var buildingPosition = _grid.GetBuildingGridPosition(_buildingToMoveId.Value);
                if (buildingPosition.HasValue)
                {
                    var currentPos = new Vector2Int(buildingPosition.Value.X, buildingPosition.Value.Y);
                    
                    // If clicked on the same position, cancel move mode
                    if (position == currentPos)
                    {
                        _buildingToMoveId = null;
                        _moveModeCancelledPublisher.Publish(new MoveModeCancelledDTO());
                        return;
                    }

                    // If clicked on valid empty cell, move the building
                    if (_grid.IsValidPosition(position) && !_grid.IsOccupied(position))
                    {
                        var moveRequest = new MoveBuildingRequestDTO
                        {
                            BuildingId = _buildingToMoveId.Value,
                            NewPosition = new GridPosition(position)
                        };
                        _moveBuildingPublisher.Publish(moveRequest);
                        _buildingToMoveId = null;
                        return;
                    }
                }
            }

            // Check if there's a building at this position
            if (_grid.GetBuildingAt(position, out var building))
            {
                // Clicked on existing building - select it
                var selectedDto = new BuildingSelectedDTO
                {
                    BuildingId = building.Id
                };
                _buildingSelectedPublisher.Publish(selectedDto);
                return;
            }

            // Empty cell - place new building
            var gridPos = new GridPosition(position);
            var request = new PlaceBuildingRequestDTO
            {
                BuildingType = _selectedBuildingType,
                Position = gridPos
            };

            _placeBuildingPublisher.Publish(request);
        }

        private Vector2Int? WorldToGridPosition(Vector3 worldPosition)
        {
            if (_grid == null) return null;

            int gx = Mathf.RoundToInt(worldPosition.x + (_grid.Width - 1) * 0.5f);
            int gy = Mathf.RoundToInt(worldPosition.z + (_grid.Height - 1) * 0.5f);

            return new Vector2Int(gx, gy);
        }

        public void Dispose()
        {
            _view.OnWorldPositionHovered -= HandleWorldPositionHovered;
            _view.OnHoverExit -= HandleHoverExit;
            _view.OnWorldPositionClicked -= HandleWorldPositionClicked;

            _moveModeStartedSubscription?.Dispose();
            _moveModeCancelledSubscription?.Dispose();
            _buildingTypeSelectedSubscription?.Dispose();
        }
    }
}

