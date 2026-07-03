using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "GameStartSetting", menuName = "Scriptable Objects/GameStartSetting")]
public class GameStartSetting : ScriptableObject
{
    // Starting money for the player
    public int moneyStart;

    // List of crate configurations used in the game
    public List<CratePriceConfig> crateConfigs = new List<CratePriceConfig>();
}
