using UnityEngine;

[CreateAssetMenu(menuName = "BookSellGame/LocationData", fileName = "NewLocationData", order = 0)]
public class LocationData : ScriptableObject
{
    public int locationId;
    public string displayName;
    public Sprite thumbnail;
    public int unlockCost; // Money needed to unlock this location
    public int travelFee;   // Fuel cost each time you travel here
    public string sceneName; // Name of the Unity scene to load
    public RegionConfig regionConfig; // Customer preferences for this region
}
