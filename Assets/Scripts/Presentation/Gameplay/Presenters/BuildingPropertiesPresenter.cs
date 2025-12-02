using System;
using ContractsInterfaces.UseCasesGameplay;
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
    public class BuildingPropertiesPresenter : IInitializable, IDisposable
    {
        [Inject] private BuildingPropertiesView _view;
        [Inject] private Grid _grid;
        [Inject] private IBuildingConfigService _buildingConfigService;
        [Inject] private IPublisher<UpgradeBuildingRequestDTO> _upgradeBuildingPublisher;
        [Inject] private IPublisher<RemoveBuildingRequestDTO> _removeBuildingPublisher;
        [Inject] private IPublisher<MoveModeStartedDTO> _moveModeStartedPublisher;
        [Inject] private IPublisher<MoveModeCancelledDTO> _moveModeCancelledPublisher;
        [Inject] private ISubscriber<BuildingSelectedDTO> _buildingSelectedSubscriber;
        [Inject] private ISubscriber<BuildingUpgradedDTO> _buildingUpgradedSubscriber;
        [Inject] private ISubscriber<BuildingRemovedDTO> _buildingRemovedSubscriber;
        [Inject] private ISubscriber<BuildingMovedDTO> _buildingMovedSubscriber;
        [Inject] private ISubscriber<MoveModeCancelledDTO> _moveModeCancelledSubscriber;

        private IDisposable _selectedSubscription;
        private IDisposable _upgradedSubscription;
        private IDisposable _removedSubscription;
        private IDisposable _movedSubscription;
        private IDisposable _moveModeCancelledSubscription;
        private int? _selectedBuildingId;
        private bool _isMoveMode;

        public void Initialize()
        {
            _view.OnUpgradeClicked += HandleUpgradeClicked;
            _view.OnMoveClicked += HandleMoveClicked;
            _view.OnDeleteClicked += HandleDeleteClicked;

            _selectedSubscription = _buildingSelectedSubscriber.Subscribe(OnBuildingSelected);
            _upgradedSubscription = _buildingUpgradedSubscriber.Subscribe(OnBuildingUpgraded);
            _removedSubscription = _buildingRemovedSubscriber.Subscribe(OnBuildingRemoved);
            _movedSubscription = _buildingMovedSubscriber.Subscribe(OnBuildingMoved);
            _moveModeCancelledSubscription = _moveModeCancelledSubscriber.Subscribe(OnMoveModeCancelled);
        }

        private void OnBuildingSelected(BuildingSelectedDTO dto)
        {
            _selectedBuildingId = dto.BuildingId;
            UpdateView(dto.BuildingId);
        }

        private void OnBuildingUpgraded(BuildingUpgradedDTO dto)
        {
            if (_selectedBuildingId.HasValue && _selectedBuildingId.Value == dto.BuildingId)
            {
                UpdateView(dto.BuildingId);
            }
        }

        private void OnBuildingRemoved(BuildingRemovedDTO dto)
        {
            if (_selectedBuildingId.HasValue && _selectedBuildingId.Value == dto.BuildingId)
            {
                _view.Hide();
                _selectedBuildingId = null;
                _isMoveMode = false;
            }
        }

        private void OnBuildingMoved(BuildingMovedDTO dto)
        {
            if (_selectedBuildingId.HasValue && _selectedBuildingId.Value == dto.BuildingId)
            {
                _isMoveMode = false;
                UpdateView(dto.BuildingId);
            }
        }

        private void OnMoveModeCancelled(MoveModeCancelledDTO dto)
        {
            _isMoveMode = false;
        }

        private void UpdateView(int buildingId)
        {
            var position = _grid.GetBuildingGridPosition(buildingId);
            if (!position.HasValue)
            {
                _view.Hide();
                return;
            }

            if (!_grid.GetBuildingAt(position.Value, out var building))
            {
                _view.Hide();
                return;
            }

            var canUpgrade = building.CanUpgrade();
            var upgradeCost = canUpgrade 
                ? _buildingConfigService.GetUpgradeCost(building.Type, building.Level) 
                : 0;

            _view.Show(building, upgradeCost, canUpgrade);
        }

        private void HandleUpgradeClicked()
        {
            if (!_selectedBuildingId.HasValue) return;

            var request = new UpgradeBuildingRequestDTO
            {
                BuildingId = _selectedBuildingId.Value
            };

            _upgradeBuildingPublisher.Publish(request);
        }

        private void HandleMoveClicked()
        {
            if (!_selectedBuildingId.HasValue) return;

            if (_isMoveMode)
            {
                // Cancel move mode
                _isMoveMode = false;
                _moveModeCancelledPublisher.Publish(new MoveModeCancelledDTO());
            }
            else
            {
                // Start move mode
                _isMoveMode = true;
                _moveModeStartedPublisher.Publish(new MoveModeStartedDTO
                {
                    BuildingId = _selectedBuildingId.Value
                });
            }
        }

        private void HandleDeleteClicked()
        {
            if (!_selectedBuildingId.HasValue) return;

            var request = new RemoveBuildingRequestDTO
            {
                BuildingId = _selectedBuildingId.Value
            };

            _removeBuildingPublisher.Publish(request);
        }

        public void Dispose()
        {
            _view.OnUpgradeClicked -= HandleUpgradeClicked;
            _view.OnMoveClicked -= HandleMoveClicked;
            _view.OnDeleteClicked -= HandleDeleteClicked;

            _selectedSubscription?.Dispose();
            _upgradedSubscription?.Dispose();
            _removedSubscription?.Dispose();
            _movedSubscription?.Dispose();
            _moveModeCancelledSubscription?.Dispose();
        }
    }
}

