using UnityEngine;

// This script handles the visual ghost preview of a book during drag operations.
// It expects a reference to the real book prefab so that the ghost can copy its mesh.
public class GhostBook : MonoBehaviour
{
    // Transparent material to apply to the ghost mesh (assign in inspector)
    public Material ghostMaterial;

    // Called by GrapControl to set which book to preview.
    public void SetPreview(GameObject realBookPrefab)
    {
        // Destroy any existing child mesh
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Instantiate the visual mesh as a child (without colliders or scripts)
        GameObject preview = Instantiate(realBookPrefab, transform);
        // Remove any unnecessary components from the preview instance
        foreach (var comp in preview.GetComponentsInChildren<MonoBehaviour>())
        {
            // Keep Transform and Renderer, destroy others
            if (!(comp is Transform) && !(comp is Renderer))
            {
                GameObject.Destroy(comp);
            }
        }

        // Apply ghost material to all renderers
        foreach (Renderer rend in preview.GetComponentsInChildren<Renderer>())
        {
            rend.material = ghostMaterial;
        }
    }
}
