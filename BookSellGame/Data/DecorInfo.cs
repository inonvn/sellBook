// DecorInfo.cs – serializable definition for each decor item
using System.Collections.Generic;
using BookSellGame.Data;

namespace BookSellGame.Data
{
    [System.Serializable]
    public class DecorInfo
    {
        public DecorItem id;
        public string displayName;
        public string meshPath; // Resources path to the prefab mesh
        public int price;
        public float revenueBuff; // e.g., 0.05 for +5%
        public List<typeBook> affectedTypes; // optional list of book types this decor boosts
    }
}
