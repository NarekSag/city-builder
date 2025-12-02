using Domain.Gameplay.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.Gameplay.Views
{
    [RequireComponent(typeof(UIDocument))]
    public class BuildingPropertiesView : MonoBehaviour
    {
        private const string PANEL_NAME = "PropertiesPanel";
        private const string BUILDING_TYPE_LABEL_NAME = "BuildingTypeLabel";
        private const string BUILDING_LEVEL_LABEL_NAME = "BuildingLevelLabel";
        private const string BUILDING_INCOME_LABEL_NAME = "BuildingIncomeLabel";
        private const string UPGRADE_COST_LABEL_NAME = "UpgradeCostLabel";
        private const string UPGRADE_BUTTON_NAME = "UpgradeButton";
        private const string MOVE_BUTTON_NAME = "MoveButton";
        private const string DELETE_BUTTON_NAME = "DeleteButton";

        public event System.Action OnUpgradeClicked;
        public event System.Action OnMoveClicked;
        public event System.Action OnDeleteClicked;

        private UIDocument _uiDocument;
        private VisualElement _root;
        private VisualElement _panel;
        private Label _buildingTypeLabel;
        private Label _buildingLevelLabel;
        private Label _buildingIncomeLabel;
        private Label _upgradeCostLabel;
        private Button _upgradeButton;
        private Button _moveButton;
        private Button _deleteButton;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            
            _panel = _root.Q<VisualElement>(PANEL_NAME);
            _buildingTypeLabel = _root.Q<Label>(BUILDING_TYPE_LABEL_NAME);
            _buildingLevelLabel = _root.Q<Label>(BUILDING_LEVEL_LABEL_NAME);
            _buildingIncomeLabel = _root.Q<Label>(BUILDING_INCOME_LABEL_NAME);
            _upgradeCostLabel = _root.Q<Label>(UPGRADE_COST_LABEL_NAME);
            _upgradeButton = _root.Q<Button>(UPGRADE_BUTTON_NAME);
            _moveButton = _root.Q<Button>(MOVE_BUTTON_NAME);
            _deleteButton = _root.Q<Button>(DELETE_BUTTON_NAME);

            if (_upgradeButton != null)
            {
                _upgradeButton.clicked += () => OnUpgradeClicked?.Invoke();
            }

            if (_moveButton != null)
            {
                _moveButton.clicked += () => OnMoveClicked?.Invoke();
            }

            if (_deleteButton != null)
            {
                _deleteButton.clicked += () => OnDeleteClicked?.Invoke();
            }

            Hide();
        }

        public void Show(Building building, int upgradeCost, bool canUpgrade)
        {
            if (_panel == null) return;

            _panel.style.display = DisplayStyle.Flex;

            if (_buildingTypeLabel != null)
            {
                _buildingTypeLabel.text = $"Type: {building.Type}";
            }

            if (_buildingLevelLabel != null)
            {
                _buildingLevelLabel.text = $"Level: {building.Level}/3";
            }

            if (_buildingIncomeLabel != null)
            {
                _buildingIncomeLabel.text = $"Income: {building.CurrentIncome.AmountPerTick} per tick";
            }

            if (_upgradeCostLabel != null)
            {
                if (canUpgrade)
                {
                    _upgradeCostLabel.text = $"Upgrade Cost: {upgradeCost}";
                    _upgradeCostLabel.style.display = DisplayStyle.Flex;
                }
                else
                {
                    _upgradeCostLabel.text = "Max Level";
                    _upgradeCostLabel.style.display = DisplayStyle.Flex;
                }
            }

            if (_upgradeButton != null)
            {
                _upgradeButton.SetEnabled(canUpgrade);
            }
        }

        public void Hide()
        {
            if (_panel != null)
            {
                _panel.style.display = DisplayStyle.None;
            }
        }
    }
}

