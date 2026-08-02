// ShopItemUI.cs – UI component for a single decor item in the shop
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BookSellGame.Core;
using BookSellGame.Data;

namespace BookSellGame.UI
{
    public class ShopItemUI : MonoBehaviour
    {
        // UI references – assign in the prefab inspector
        public Image decorImage;          // Optional visual preview (sprite or thumbnail)
        public TextMeshProUGUI nameText;  // Display name of the decor
        public TextMeshProUGUI priceText; // Price label
        public Button purchaseButton;    // Purchase action button

        private DecorInfo _decorInfo;

        /// <summary>
        /// Called by ShopUI when populating the shop list.
        /// </summary>
        public void Setup(DecorInfo info)
        {
            _decorInfo = info;

            // Set name and price UI
            if (nameText != null)
                nameText.text = info.displayName;
            if (priceText != null)
                priceText.text = $"${info.price}";

            // Load a thumbnail sprite if a path is provided (optional)
            if (decorImage != null && !string.IsNullOrEmpty(info.meshPath))
            {
                // Expect a sprite with the same name as the mesh (convention)
                var sprite = Resources.Load<Sprite>(info.meshPath);
                if (sprite != null)
                    decorImage.sprite = sprite;
            }

            // Wire up purchase button – ensure we don't add duplicate listeners
            if (purchaseButton != null)
            {
                purchaseButton.onClick.RemoveAllListeners();
                purchaseButton.onClick.AddListener(PurchaseDecor);
            }
        }

        private void PurchaseDecor()
        {
            if (_decorInfo == null)
                return;

            // Notify the rest of the system that a decor item was equipped/purchased.
            EventBus.RaiseDecorEquipped(_decorInfo.id);

            // Future: invoke SaveManager to persist owned decor.
        }
    }
}
