using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "BookSellGame/RegionConfig", fileName = "NewRegionConfig", order = 0)]
public class RegionConfig : ScriptableObject
{
    // Define customer group preferences for this region.
    // Example: genre weightings, probability of specific book types.
    public List<GenrePreference> genrePreferences = new List<GenrePreference>();
}

[System.Serializable]
public class GenrePreference
{
    public string genreName; // e.g., "Fantasy", "Science", "Philosophy"
    public float weight;    // Relative likelihood (0..1)
}
