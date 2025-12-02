using Domain.Gameplay.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.Gameplay.Views
{
    [RequireComponent(typeof(UIDocument))]
    public class BuildingCatalogView : MonoBehaviour
    {
        private const string CATALOG_PANEL_NAME = "CatalogPanel";
        private const string HOUSE_BUTTON_NAME = "HouseButton";
        private const string FARM_BUTTON_NAME = "FarmButton";
        private const string MINE_BUTTON_NAME = "MineButton";
        private const string HOUSE_COST_LABEL_NAME = "HouseCostLabel";
        private const string FARM_COST_LABEL_NAME = "FarmCostLabel";
        private const string MINE_COST_LABEL_NAME = "MineCostLabel";

        public event System.Action<BuildingType> OnBuildingTypeSelected;

        private UIDocument _uiDocument;
        private VisualElement _root;
        private VisualElement _catalogPanel;
        private Button _houseButton;
        private Button _farmButton;
        private Button _mineButton;
        private Label _houseCostLabel;
        private Label _farmCostLabel;
        private Label _mineCostLabel;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;

            _catalogPanel = _root.Q<VisualElement>(CATALOG_PANEL_NAME);
            _houseButton = _root.Q<Button>(HOUSE_BUTTON_NAME);
            _farmButton = _root.Q<Button>(FARM_BUTTON_NAME);
            _mineButton = _root.Q<Button>(MINE_BUTTON_NAME);
            _houseCostLabel = _root.Q<Label>(HOUSE_COST_LABEL_NAME);
            _farmCostLabel = _root.Q<Label>(FARM_COST_LABEL_NAME);
            _mineCostLabel = _root.Q<Label>(MINE_COST_LABEL_NAME);

            if (_houseButton != null)
            {
                _houseButton.clicked += () => OnBuildingTypeSelected?.Invoke(BuildingType.House);
            }

            if (_farmButton != null)
            {
                _farmButton.clicked += () => OnBuildingTypeSelected?.Invoke(BuildingType.Farm);
            }

            if (_mineButton != null)
            {
                _mineButton.clicked += () => OnBuildingTypeSelected?.Invoke(BuildingType.Mine);
            }
        }

        public void UpdateBuildingCosts(int houseCost, int farmCost, int mineCost)
        {
            if (_houseCostLabel != null)
            {
                _houseCostLabel.text = $"Cost: {houseCost}";
            }

            if (_farmCostLabel != null)
            {
                _farmCostLabel.text = $"Cost: {farmCost}";
            }

            if (_mineCostLabel != null)
            {
                _mineCostLabel.text = $"Cost: {mineCost}";
            }
        }

        public void HighlightBuildingType(BuildingType buildingType)
        {
            ClearAllHighlights();

            Button buttonToHighlight = null;
            switch (buildingType)
            {
                case BuildingType.House:
                    buttonToHighlight = _houseButton;
                    break;
                case BuildingType.Farm:
                    buttonToHighlight = _farmButton;
                    break;
                case BuildingType.Mine:
                    buttonToHighlight = _mineButton;
                    break;
            }

            if (buttonToHighlight != null)
            {
                buttonToHighlight.style.backgroundColor = new StyleColor(new Color(0.2f, 0.6f, 1f, 0.5f));
            }
        }

        private void ClearAllHighlights()
        {
            if (_houseButton != null)
            {
                _houseButton.style.backgroundColor = StyleKeyword.Null;
            }

            if (_farmButton != null)
            {
                _farmButton.style.backgroundColor = StyleKeyword.Null;
            }

            if (_mineButton != null)
            {
                _mineButton.style.backgroundColor = StyleKeyword.Null;
            }
        }
    }
}

