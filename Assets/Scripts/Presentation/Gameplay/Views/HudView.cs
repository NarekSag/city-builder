using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.Gameplay.Views
{
    [RequireComponent(typeof(UIDocument))]
    public class HudView : MonoBehaviour
    {
        private const string GoldLabelName = "GoldLabel";
        private const string SaveButtonName = "SaveButton";
        private const string LoadButtonName = "LoadButton";

        private UIDocument _uiDocument;
        private VisualElement _root;
        private Label _goldLabel;
        private Button _saveButton;
        private Button _loadButton;

        public Button SaveButton => _saveButton;
        public Button LoadButton => _loadButton;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            _goldLabel = _root.Q<Label>(GoldLabelName);
            _saveButton = _root.Q<Button>(SaveButtonName);
            _loadButton = _root.Q<Button>(LoadButtonName);
        }

        public void UpdateGold(int amount)
        {
            if (_goldLabel != null)
            {
                _goldLabel.text = $"Gold: {amount}";
            }
        }

        public void ShowInsufficientGoldNotification(int required, int current)
        {
            // TODO: Implement notification system (optional)
            Debug.LogWarning($"[HudView] Insufficient gold! Required: {required}, Current: {current}");
        }
    }
}

