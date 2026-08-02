using UnityEngine;

// This script handles the visual ghost preview of a book during drag operations.
public class GhostBook : MonoBehaviour
{
    public bool itCanShow = true;

    private Renderer rend;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        CacheRenderers();
    }

    private void Start()
    {
        // Ensure GameObject (and Collider) is ACTIVE for Raycast to hit it,
        // but hide the visual representation until hovered during drag.
        gameObject.SetActive(true);
        SetVisualActive(false);
    }

    private void CacheRenderers()
    {
        if (rend == null) rend = GetComponent<Renderer>() ?? GetComponentInChildren<Renderer>(true);
        if (spriteRend == null) spriteRend = GetComponent<SpriteRenderer>() ?? GetComponentInChildren<SpriteRenderer>(true);
    }

    // Enables/disables only the visual renderer so Colliders remain active for Raycasts.
    public void SetVisualActive(bool visible)
    {
        CacheRenderers();
        if (rend != null) rend.enabled = visible;
        if (spriteRend != null) spriteRend.enabled = visible;
    }
}

