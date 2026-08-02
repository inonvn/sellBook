// ShopUI.cs – populates shop UI with decor items from DecorConfig
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BookSellGame.Core;
using BookSellGame.Data;

namespace BookSellGame.UI
{
    public class ShopUI : MonoBehaviour
    {
        // Parent UI element (vertical layout) where item UI prefabs will be instantiated
        public Transform contentParent;
        // Prefab for a single shop item UI (should contain ShopItemUI component)
        public GameObject shopItemPrefab;

        private void Start()
        {
            PopulateShop();
        }

        private void PopulateShop()
        {
            if (contentParent == null || shopItemPrefab == null)
            {
                Debug.LogError("ShopUI: contentParent or shopItemPrefab not assigned.");
                return;
            }

            var config = DecorConfig.Instance;
            if (config == null)
            {
                Debug.LogError("ShopUI: DecorConfig instance not found.");
                return;
            }

            // Assuming DecorConfig holds a collection of all decor infos; we expose via a public method.
            foreach (var decorInfo in config.GetAllDecorInfos())
            {
                var go = Instantiate(shopItemPrefab, contentParent);
                var itemUI = go.GetComponent<ShopItemUI>();
                if (itemUI != null)
                {
                    itemUI.Setup(decorInfo);
                }
                else
                {
                    Debug.LogWarning("ShopItem prefab missing ShopItemUI component.");
                }
            }
        }
    }
}
