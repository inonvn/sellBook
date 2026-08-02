// DecorDisplay.cs – spawns decor meshes when equipped
using UnityEngine;
using BookSellGame.Core;
using BookSellGame.Data;

namespace BookSellGame.Core
{
    public class DecorDisplay : MonoBehaviour
    {
        // Parent under which decor prefabs will be instantiated. Assign in inspector or find at runtime.
        public Transform decorRoot;

        private void Awake()
        {
            // Subscribe to decor equipped events
            EventBus.OnDecorEquipped += HandleDecorEquipped;
        }

        private void OnDestroy()
        {
            EventBus.OnDecorEquipped -= HandleDecorEquipped;
        }

        private void HandleDecorEquipped(DecorItem decor)
        {
            var info = DecorConfig.Instance?.GetInfo(decor);
            if (info == null)
            {
                Debug.LogWarning($"DecorDisplay: No config found for {decor}");
                return;
            }

            if (string.IsNullOrEmpty(info.meshPath))
            {
                Debug.LogWarning($"DecorDisplay: meshPath missing for {decor}");
                return;
            }

            // Load prefab from Resources folder
            var prefab = Resources.Load<GameObject>(info.meshPath);
            if (prefab == null)
            {
                Debug.LogError($"DecorDisplay: Failed to load prefab at {info.meshPath} for {decor}");
                return;
            }

            // Determine parent transform
            Transform parent = decorRoot;
            if (parent == null)
            {
                // Fallback: try to find a GameObject named "DecorRoot" in the scene
                var go = GameObject.Find("DecorRoot");
                if (go != null) parent = go.transform;
            }

            // Instantiate the decor prefab
            var instance = Instantiate(prefab, parent);
            instance.name = decor.ToString();
        }
    }
}
