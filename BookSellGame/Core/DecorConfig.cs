// DecorConfig.cs – loads decor definitions from Resources/decorConfig.json
using System.Collections.Generic;
using UnityEngine;
using BookSellGame.Data;

namespace BookSellGame.Core
{
    public class DecorConfig : MonoBehaviour
    {
        public static DecorConfig Instance { get; private set; }
        private Dictionary<DecorItem, DecorInfo> decorMap = new Dictionary<DecorItem, DecorInfo>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
            LoadConfig();
        }

        private void LoadConfig()
        {
            TextAsset json = Resources.Load<TextAsset>("decorConfig");
            if (json == null)
            {
                Debug.LogError("Decor config JSON not found in Resources.");
                return;
            }
            DecorInfo[] arr = JsonUtility.FromJson<DecorInfoArray>(json.text).items;
            foreach (var info in arr)
            {
                decorMap[info.id] = info;
            }
        }

        public DecorInfo GetInfo(DecorItem id)
        {
            decorMap.TryGetValue(id, out var info);
            return info;
        }

        // Returns all decor infos for UI population
        public IEnumerable<DecorInfo> GetAllDecorInfos()
        {
            return decorMap.Values;
        }

        // Helper wrapper for JsonUtility array deserialization
        [System.Serializable]
        private class DecorInfoArray
        {
            public DecorInfo[] items;
        }
    }
}
