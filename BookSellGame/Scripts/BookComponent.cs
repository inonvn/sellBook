using UnityEngine;

// Component attached to each book prefab in the storage area.
// It provides a reference to the actual book prefab that should be instantiated when placed.
public class BookComponent : MonoBehaviour
{
    // The prefab to instantiate for the real book (assign in inspector)
    public GameObject bookPrefab;
}
