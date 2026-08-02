// UIKeothabook.cs – attaches to a UI Image/Button and starts drag on click
using UnityEngine;
using UnityEngine.EventSystems;

namespace BookSellGame.UI
{
    // This component should be added to the UI element that represents the book icon.
    // It implements IPointerClickHandler and IPointerDownHandler so Unity will invoke 
    // event callbacks when the user clicks/taps/drags the UI element.
    public class UIKeothabook : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
        // Reference to the grapControl script that handles drag‑and‑drop logic.
        // Assign it in the Inspector, or it will be auto‑found at runtime.
        public grapControl GrapControl;
        public typeBook type;

        private void Awake()
        {
            EnsureGrapControl();
        }

        private void EnsureGrapControl()
        {
            if (GrapControl == null)
            {
                GrapControl = FindFirstObjectByType<grapControl>();
                if (GrapControl == null)
                {
                    GrapControl = FindObjectOfType<grapControl>();
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StartDrag();
        }

        // Unity calls this method when the UI element is clicked.
        public void OnPointerClick(PointerEventData eventData)
        {
            StartDrag();
        }

        private void StartDrag()
        {
            EnsureGrapControl();

            if (GrapControl != null)
            {
                GrapControl.StartDragFromUI(type);
            }
            else
            {
                Debug.LogWarning("UIKeothabook: grapControl reference not set and could not be found.");
            }
        }
    }
}

