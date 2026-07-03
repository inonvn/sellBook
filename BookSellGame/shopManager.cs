using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class shopManager : MonoBehaviour
{
    public TextMeshProUGUI NameCrate;
    public TextMeshProUGUI DescriptionCrate;
    public TextMeshProUGUI costCrate;
    public GameObject ShowNewIcon;
    public GameObject ShowSoldIcon;
    public int IdCrate;
    public bool hasBuy;
    public void buyCrate()
    {
        if (hasBuy == false )
        {
           var e = GameManager.ins.PurchaseCrate(IdCrate);
            if (e == true)
            {
                ShowNewIcon.SetActive(false);
                ShowSoldIcon.SetActive(true);
                hasBuy = true;
            }
        }
    }    
}
