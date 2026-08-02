

using System.Collections.Generic;
using UnityEngine;

public class grapControl : MonoBehaviour
{
    public GameObject ghostPrefab;           
    public LayerMask storageLayer;          
    public LayerMask shelfLayer;
    public bool working;
    public typeBook thisType;
    public List<BookComponent> books;

    private GhostBook currentGhost;
    private GameObject draggingBook; // Real book being dragged
    private GameObject selectedBookPrefab;
    private typeBook selectedBookType;
    private bool isDragging = false;

    void Start()
    {
        if (currentGhost != null) currentGhost.SetVisualActive(false);
    }

    void Update()
    {
        if (working && isDragging)
        {
            if (Input.GetMouseButtonUp(0))
                TryPlaceBook();
            else
                UpdateGhostPosition();
        }
        else if (working && Input.GetMouseButtonDown(0))
        {
            TryStartDrag();
        }
    }

    public void TryStartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, storageLayer))
        {
            StartDrag();
        }
    }

    public void StartDragFromUI(typeBook type)
    {
        thisType = type;
        working = true;
        StartDrag();
    }

    private void StartDrag()
    {
        if (books == null) return;
        var bookComp = books.Find(x => x != null && x.type == thisType);
        if (bookComp == null)
        {
            Debug.LogWarning($"grapControl: No BookComponent found matching type {thisType}");
            return;
        }

        selectedBookPrefab = bookComp.gameObject;
        selectedBookType = bookComp.type;
        isDragging = true;

        if (draggingBook != null) Destroy(draggingBook);
        draggingBook = Instantiate(selectedBookPrefab);
        Vector3 initialPos = draggingBook.transform.position;
        initialPos.z = -8f;
        draggingBook.transform.position = initialPos;
        draggingBook.SetActive(true);
        if (currentGhost != null) currentGhost.SetVisualActive(false);
    }

    void UpdateGhostPosition()
    {
        if (draggingBook == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, shelfLayer, QueryTriggerInteraction.Collide);
        
        // Diagnostic Logging for Raycast
        if (hits.Length > 0)
        {
            Debug.Log($"[grapControl] Raycast 3D HIT {hits.Length} object(s):");
            foreach (var h in hits)
            {
                var gComp = GetGhostBookFromHit(h.collider);
                Debug.Log($"   -> Hit Object: '{h.collider.gameObject.name}', Layer: {LayerMask.LayerToName(h.collider.gameObject.layer)}, Distance: {h.distance:F2}, Has GhostBook: {(gComp != null ? $"Yes (on '{gComp.gameObject.name}', itCanShow={gComp.itCanShow})" : "No (Parent Container or Invalid)")}");
            }
        }
        else
        {
            // Check if user is using 2D Colliders instead of 3D Colliders
            RaycastHit2D[] hits2D = Physics2D.GetRayIntersectionAll(ray, 1000f, shelfLayer);
            if (hits2D.Length > 0)
            {
                Debug.LogWarning($"[grapControl] 3D Raycast hit 0 objects, BUT 2D Raycast hit {hits2D.Length} object(s)! Your scene is using 2D Colliders (e.g. BoxCollider2D). Hit 2D object: '{hits2D[0].collider.gameObject.name}'");
            }
            else
            {
                Debug.Log($"[grapControl] Raycast 3D hit NOTHING (0 hits). Ray origin: {ray.origin}, direction: {ray.direction}, LayerMask value: {shelfLayer.value}");
            }
        }

        GhostBook foundGhost = null;
        Vector3 targetPos = Vector3.zero;
        bool hitAny = false;

        // Sort hits by distance to prioritize the closest valid target
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;
        
            // Ignore draggingBook itself
            if (hit.collider.gameObject == draggingBook || hit.collider.transform.IsChildOf(draggingBook.transform))
                continue;

            if (!hitAny)
            {
                targetPos = hit.point;
                hitAny = true;
            }

            var ghostComp = GetGhostBookFromHit(hit.collider);

            if (ghostComp != null && ghostComp.itCanShow)
            {
                foundGhost = ghostComp;
                targetPos = ghostComp.transform.position;
                break;
            }
        }

        if (hitAny)
        {
            targetPos.z = -8f;
            draggingBook.transform.position = targetPos;
            if (foundGhost != null)
                draggingBook.transform.rotation = foundGhost.transform.rotation;
            else
                draggingBook.transform.rotation = Quaternion.identity;
        }
        else
        {
            // Fallback raycast to Z = -8 plane so draggingBook follows cursor smoothly anywhere
            Plane dragPlane = new Plane(Vector3.forward, new Vector3(0f, 0f, -8f));
            if (dragPlane.Raycast(ray, out float enter))
            {
                targetPos = ray.GetPoint(enter);
                targetPos.z = -8f;
                draggingBook.transform.position = targetPos;
                draggingBook.transform.rotation = Quaternion.identity;
            }
        }

        // Manage ghost active states
        if (foundGhost != null)
        {
            if (currentGhost != null && currentGhost != foundGhost)
            {
                currentGhost.SetVisualActive(false);
            }
            currentGhost = foundGhost;
            currentGhost.SetVisualActive(true);
        }
        else
        {
            if (currentGhost != null)
            {
                currentGhost.SetVisualActive(false);
                currentGhost = null;
            }
        }
    }

    private GhostBook GetGhostBookFromHit(Collider col)
    {
        if (col == null) return null;
        var ghost = col.GetComponent<GhostBook>();
        if (ghost == null) ghost = col.GetComponentInParent<GhostBook>();
        return ghost;
    }

    private ShelfCell GetShelfCellFromHit(Collider col)
    {
        if (col == null) return null;
        var cell = col.GetComponent<ShelfCell>();
        if (cell == null) cell = col.GetComponentInParent<ShelfCell>();
        return cell;
    }

    void TryPlaceBook()
    {
        if (!isDragging) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, shelfLayer, QueryTriggerInteraction.Collide);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        GhostBook foundGhost = null;
        ShelfCell foundCell = null;

        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.collider.gameObject == draggingBook || hit.collider.transform.IsChildOf(draggingBook.transform))
                continue;

            var ghostComp = GetGhostBookFromHit(hit.collider);

            if (ghostComp != null)
            {
                foundGhost = ghostComp;
                break;
            }

            var cell = GetShelfCellFromHit(hit.collider);

            if (cell != null && cell.CanAccept())
            {
                foundCell = cell;
                break;
            }
        }

        if (foundGhost != null)
        {
            Vector3 placePos = foundGhost.transform.position;
            Quaternion placeRot = foundGhost.transform.rotation;
            foundGhost.SetVisualActive(false);
            foundGhost.itCanShow = false;
            Instantiate(selectedBookPrefab, placePos, placeRot);
            GameManager.ins.Storage.Add(selectedBookType, 1);
            GameManager.NotifyStorageChanged();
        }
        else if (foundCell != null)
        {
            Vector3 placePos = foundCell.transform.position;
            Quaternion placeRot = foundCell.transform.rotation;
            Instantiate(selectedBookPrefab, placePos, placeRot);
            GameManager.ins.Storage.Add(selectedBookType, 1);
            GameManager.NotifyStorageChanged();
        }

        if (currentGhost != null)
        {
            currentGhost.SetVisualActive(false);
            currentGhost = null;
        }
        if (draggingBook != null) Destroy(draggingBook);
        isDragging = false;
        working = false;
        selectedBookPrefab = null;
    }
}
