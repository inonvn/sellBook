// SaveData.cs - root serializable container for all persistent game data
using System.Collections.Generic;
using UnityEngine;

namespace BookSellGame.Data
{
    [System.Serializable]
    public class SaveData
    {
        public int version = 1; // future migration support
        public int wallet = 0;
        public Dictionary<typeBook, int> bookCounts = new Dictionary<typeBook, int>();
        public List<typeBook> ownedDecors = new List<typeBook>(); // using enum for decor ids
        public List<string> ownedMementos = new List<string>(); // simple string ids for mementos
        public Dictionary<string, bool> mapUnlocks = new Dictionary<string, bool>();
    }
}
