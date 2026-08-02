// MementoItem.cs – simple representation of a collected memento
using System;

namespace BookSellGame.Data
{
    [Serializable]
    public class MementoItem
    {
        // Unique identifier for the memento (could be enum or string)
        public string id;
        // Optional revenue multiplier provided by this memento (e.g., 0.05 for +5%)
        public float revenueBuff;
        // Additional data can be added as needed
        public MementoItem(string id, float revenueBuff = 0f)
        {
            this.id = id;
            this.revenueBuff = revenueBuff;
        }
    }
}
