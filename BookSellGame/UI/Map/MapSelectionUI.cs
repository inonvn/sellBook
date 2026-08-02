using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

using BookSellGame.Core.Travel;

namespace BookSellGame.UI.Map
{
    // This component should be attached to a Canvas GameObject that holds 4 Button children.
    // It loads LocationData ScriptableObjects (placed in Resources/Locations) and creates a button for each.
    public class MapSelectionUI : MonoBehaviour
    {
        [Header("UI References")] 
        [Tooltip("Parent Transform that will hold the generated location buttons.")]
        public Transform buttonContainer;
        [Tooltip("Prefab for each location button (must contain a Button component and an Image for the thumbnail.)")]
        public GameObject buttonPrefab;

        private List<LocationData> locations = new List<LocationData>();

        private void Awake()
        {
            LoadLocations();
            PopulateUI();
        }

        private void LoadLocations()
        {
            // Expect LocationData assets under Resources/Locations folder.
            var loaded = Resources.LoadAll<LocationData>("Locations");
            if (loaded == null || loaded.Length == 0)
            {
                Debug.LogWarning("MapSelectionUI: No LocationData assets found in Resources/Locations.");
                return;
            }
            locations.AddRange(loaded);
        }

        private void PopulateUI()
        {
            if (buttonContainer == null || buttonPrefab == null) return;
            foreach (var loc in locations)
            {
                var btnObj = Instantiate(buttonPrefab, buttonContainer);
                var btn = btnObj.GetComponent<Button>();
                var img = btnObj.GetComponent<Image>();
                if (img != null && loc.thumbnail != null) img.sprite = loc.thumbnail;
                // Set button text if there is a Text component under the prefab.
                var txt = btnObj.GetComponentInChildren<Text>();
                if (txt != null) txt.text = loc.displayName;

                // Capture locals for the click handler.
                btn.onClick.AddListener(() => OnLocationClicked(loc));

                // Visual lock overlay if not unlocked.
                if (GameManager.ins.PlayerWallet < loc.unlockCost)
                {
                    // Simple visual: dim the button.
                    var canvasGroup = btnObj.AddComponent<CanvasGroup>();
                    canvasGroup.interactable = false;
                    canvasGroup.alpha = 0.5f;
                }
            }
        }

        private void OnLocationClicked(LocationData location)
        {
            // Verify unlock condition.
            if (GameManager.ins.PlayerWallet < location.unlockCost)
            {
                Debug.Log("Location locked: need " + location.unlockCost + " money.");
                return;
            }

            // Deduct travel fee.
            if (GameManager.ins.TrySpendTravelFee(location.travelFee) == false)
            {
                // Not enough money or deduction failed.
                return;
            }

            // Initiate scene transition via TravelManager.
            TravelManager.Instance.TravelToLocation(location);
        }
    }
}
