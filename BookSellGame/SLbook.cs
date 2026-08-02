using TMPro;

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SLbook : MonoBehaviour
{
    public TextMeshProUGUI SLText;
    public Image SLImage;
  

   
    public void Setup(KeyValuePair<typeBook, int> data)
    {
        if (SLText != null)
        {
            SLText.text = $"{data.Value}";
        }

        var e = UIManager.Instance.SachIconA.Find(o => o.Name == data.Key);
        SLImage.sprite = e.sprite;
        
       

    }
}
