using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.Gameplay.Views
{
    [RequireComponent(typeof(UIDocument))]
    public class HudView : MonoBehaviour
    {
        private const string GOLD_LABEL_NAME = "GoldLabel";

        private UIDocument _uiDocument;
        private VisualElement _root;
        private Label _goldLabel;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            _goldLabel = _root.Q<Label>(GOLD_LABEL_NAME);
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

