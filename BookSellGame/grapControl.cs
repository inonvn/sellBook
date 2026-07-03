
using UnityEngine;

public class grapControl : MonoBehaviour
{
    public GameObject ghostPrefab;           // Prefab containing GhostBook component
    public LayerMask storageLayer;           // Layer for books in storage
    public LayerMask shelfLayer;             // Layer for shelf cells

    private GameObject currentGhost;
    private GameObject selectedBookPrefab;
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryStartDrag();
        else if (Input.GetMouseButtonUp(0) && isDragging)
            TryPlaceBook();
        else if (isDragging)
            UpdateGhostPosition();
    }

    void TryStartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, storageLayer))
        {
            var bookComp = hit.collider.GetComponent<BookComponent>();
            if (bookComp == null) return;
            selectedBookPrefab = bookComp.bookPrefab;
            currentGhost = Instantiate(ghostPrefab);
            var ghost = currentGhost.GetComponent<GhostBook>();
            ghost?.SetPreview(selectedBookPrefab);
            isDragging = true;
        }
    }

    void UpdateGhostPosition()
    {
        if (currentGhost == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, shelfLayer))
        {
            currentGhost.transform.position = hit.point;
        }
    }

    void TryPlaceBook()
    {
        if (!isDragging) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, shelfLayer))
        {
            var cell = hit.collider.GetComponent<ShelfCell>();
            if (cell != null && cell.CanAccept())
            {
                // Place the real book at the shelf cell
                Instantiate(selectedBookPrefab, cell.transform.position, Quaternion.identity);
                // Update storage counts via GameManager singleton
                GameManager.ins.Storage.Add(selectedBookPrefab.name, 1);
                // Notify UI of storage change
                GameManager.NotifyStorageChanged();
            }
        }
        if (currentGhost != null) Destroy(currentGhost);
        isDragging = false;
        selectedBookPrefab = null;
    }
}
