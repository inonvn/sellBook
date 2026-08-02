// BuffManager.cs – handles revenue modifiers from owned Decor and Mementos
using System.Collections.Generic;
using BookSellGame.Core;
using BookSellGame.Data;
using UnityEngine;

namespace BookSellGame.Core
{
    public class BuffManager : MonoBehaviour
    {
        public static BuffManager Instance { get; private set; }

        // Global multiplier (e.g., +5% total revenue)
        private float globalBuff = 0f;
        // Per‑book‑type multiplier
        private Dictionary<typeBook, float> typeBuffs = new Dictionary<typeBook, float>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);

            // Subscribe to events
            EventBus.OnDecorEquipped += HandleDecorEquipped;
            EventBus.OnMementoCollected += HandleMementoCollected;
        }

        private void OnDestroy()
        {
            EventBus.OnDecorEquipped -= HandleDecorEquipped;
            EventBus.OnMementoCollected -= HandleMementoCollected;
        }

        private void HandleDecorEquipped(DecorItem decor)
        {
            // Load decor info from resources (cached config)
            var info = DecorConfig.Instance.GetInfo(decor);
            if (info == null) return;

            // Apply global buff
            globalBuff += info.revenueBuff;

            // Apply per‑type buffs
            if (info.affectedTypes != null)
            {
                foreach (var bt in info.affectedTypes)
                {
                    if (!typeBuffs.ContainsKey(bt)) typeBuffs[bt] = 0f;
                    typeBuffs[bt] += info.revenueBuff;
                }
            }
        }

        private void HandleMementoCollected(MementoItem memento)
        {
            // For now treat a memento like a decor with its own buff (if needed)
            // This placeholder can be extended later.
        }

        /// <summary>
        /// Returns the revenue multiplier for a given book type (1 = no buff).
        /// </summary>
        public float GetRevenueMultiplier(typeBook bookType)
        {
            float multiplier = 1f + globalBuff;
            if (typeBuffs.TryGetValue(bookType, out var extra))
                multiplier += extra;
            return multiplier;
        }
    }
}
