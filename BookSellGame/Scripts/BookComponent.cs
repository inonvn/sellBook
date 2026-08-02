using UnityEngine;

// Component attached to each book prefab in the storage area.
// It provides a reference to the actual book prefab that should be instantiated when placed.
public class BookComponent : MonoBehaviour
{
    public typeBook type;
    public sizeBook size;
}
