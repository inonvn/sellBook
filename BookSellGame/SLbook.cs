using TMPro;

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SLbook : MonoBehaviour
{
    public TextMeshProUGUI SLText;
    public Image SLImage;
    // New field to display category icon
    public Image CategoryIcon;

    // Initialise the UI entry with book data (id and count) and set category based on Id
    public void Setup(KeyValuePair<string, int> data)
    {
        if (SLText != null)
        {
            SLText.text = $"{data.Value}";
        }
        var sprite = Resources.Load<Sprite>(data.Key);
        if (SLImage != null)
        {
            if (sprite != null)
            {
                SLImage.sprite = sprite;
                SLImage.enabled = true;
            }
            else
            {
                SLImage.enabled = false;
            }
        }
        // Load category icon based on book Id (expects sprite under Resources/CategoryIcons)
        if (CategoryIcon != null)
        {
            var categorySprite = Resources.Load<Sprite>($"CategoryIcons/{data.Key}");
            if (categorySprite != null)
            {
                CategoryIcon.sprite = categorySprite;
                CategoryIcon.enabled = true;
            }
            else
            {
                CategoryIcon.enabled = false;
            }
        }
    }
}
