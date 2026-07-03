using UnityEngine;

// Represents a single cell on the wagon shelf where a book can be placed.
// Attach this component to the collider of each shelf slot.
public class ShelfCell : MonoBehaviour
{
   
    public string acceptedBookId = "";

  
    public int maxCapacity = 1;

   
    private int currentCount = 0;


    public string placedBookId { get; private set; } = "";

    // Returns true if the cell can accept another book.
    public bool CanAccept()
    {
        return currentCount < maxCapacity && (string.IsNullOrEmpty(acceptedBookId) || acceptedBookId == placedBookId);
    }

    // Call this when a book is placed on the cell.
    public void PlaceBook(string bookId)
    {
        if (!CanAccept())
        {
            Debug.LogWarning($"ShelfCell at {name} cannot accept more books.");
            return;
        }
        currentCount++;
        placedBookId = bookId;
    }

    // Call this when a book is removed (e.g., during drag cancel or pickup).
    public void RemoveBook()
    {
        if (currentCount > 0)
        {
            currentCount--;
            if (currentCount == 0) placedBookId = "";
        }
    }
}
