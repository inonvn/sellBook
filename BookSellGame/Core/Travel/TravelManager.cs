using UnityEngine;
using UnityEngine.SceneManagement;

namespace BookSellGame.Core.Travel
{
    /// <summary>
    /// Manages traveling between map locations.
    /// </summary>
    public class TravelManager : MonoBehaviour
    {
        public static TravelManager Instance;
        // Reference to the GameManager singleton for wallet and region handling.
        private GameManager _gm;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _gm = GameManager.ins;
            if (_gm == null)
            {
                Debug.LogError("TravelManager: GameManager instance not found.");
            }
        }

        /// <summary>
        /// Attempts to travel to the specified location.
        /// </summary>
        /// <param name="location">The location data to travel to.</param>
        public void TravelTo(LocationData location)
        {
            if (location == null)
            {
                Debug.LogWarning("TravelManager: Null location passed.");
                return;
            }

            // Unlock check – if player hasn't unlocked this location yet, deduct unlock cost.
            if (!_gm.Storage.CanAdd(0)) // placeholder for unlock logic; replace with real check.
            {
                // For now we simply require enough wallet for travel fee.
            }

            // Ensure the player has enough money for the travel fee.
            if (_gm.PlayerWallet < location.travelFee)
            {
                Debug.LogWarning($"Not enough money to travel to {location.displayName}. Required: {location.travelFee}, Current: {_gm.PlayerWallet}");
                return;
            }

            // Deduct travel fee and notify UI via EventBus.
            _gm.PlayerWallet -= location.travelFee;
            EventBus.RaiseWalletChanged(_gm.PlayerWallet);

            // Update current region configuration.
            _gm.CurrentRegionConfig = location.regionConfig;
            GameManager.NotifyRegionChanged(location.regionConfig);

            // Load the target scene asynchronously.
            if (!string.IsNullOrEmpty(location.sceneName))
            {
                // Optional fade transition could be added here.
                SceneManager.LoadSceneAsync(location.sceneName);
            }
            else
            {
                Debug.LogError($"Location {location.displayName} does not specify a scene name.");
            }
        }

        // Wrapper method matching UI calls
        public void TravelToLocation(LocationData location)
        {
            TravelTo(location);
        }
    }
}
