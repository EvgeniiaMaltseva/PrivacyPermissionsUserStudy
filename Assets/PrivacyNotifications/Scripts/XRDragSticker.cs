using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDragSticker : MonoBehaviour
{
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    // Snap settings
    public Transform bookSnapZone;        // Reference to the book snap zone
    public Transform boardSnapZone;       // Reference to the board snap zone
    public float snapDistance = 0.2f;     // Maximum distance for snapping

    private bool snappedToBook = false;   // Track if snapped to book
    private bool snappedToBoard = false;  // Track if snapped to board

    private Vector3 originalScale;       // To store the original scale
    private Transform originalParent;    // To store the original parent

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // Store original parent and scale
        originalParent = transform.parent;
        originalScale = transform.localScale; 

        Debug.Log("Original Parent of " + gameObject.name + " is " + (originalParent != null ? originalParent.name : "None"));

        // Register for the sticker release and grab events
        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        rb.isKinematic = false; // Allows sticker to be dropped

        // Calculate distances to each snap zone
        float distanceToBookSnapZone = Vector3.Distance(transform.position, bookSnapZone.position);
        float distanceToBoardSnapZone = Vector3.Distance(transform.position, boardSnapZone.position);

        // If neither snap zone is within snapping range, return to board zone
        if (distanceToBookSnapZone > snapDistance && distanceToBoardSnapZone > snapDistance)
        {
            SnapToBoard();
        }
        else
        {
            // If close enough to one of the snap zones, snap to it
            if (distanceToBookSnapZone <= snapDistance)
            {
                SnapToBook();
            }
            else if (distanceToBoardSnapZone <= snapDistance)
            {
                SnapToBoard();
            }
        }
    }

    private void SnapToBook()
    {

        transform.position = bookSnapZone.position;
        transform.rotation = bookSnapZone.rotation;
        
                // Set the book snap zone as the parent and reset scale
        transform.SetParent(bookSnapZone);

        Debug.Log("Sticker snapped to book zone at position: " + transform.position.ToString("F3"));

        rb.isKinematic = true;
        snappedToBook = true;
        snappedToBoard = false;
    }

    private void UnsnapFromBook()
    {
        grabInteractable.enabled = true;
        rb.isKinematic = false;
        snappedToBook = false;

        Debug.Log("Sticker unsnapped from the book zone.");
    }

    private void SnapToBoard()
    {

        transform.position = boardSnapZone.position;
        transform.rotation = boardSnapZone.rotation;

                // Reset to original parent and original scale
        transform.SetParent(originalParent);
        transform.localScale = originalScale;

        Debug.Log("Sticker snapped to board zone at position: " + transform.position.ToString("F3"));

        rb.isKinematic = true;
        snappedToBoard = true;
        snappedToBook = false;
    }

    private void UnsnapFromBoard()
    {
        grabInteractable.enabled = true;
        rb.isKinematic = false;
        snappedToBoard = false;

        Debug.Log("Sticker unsnapped from the board zone.");
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (snappedToBook)
        {
            UnsnapFromBook();
        }
        else if (snappedToBoard)
        {
            UnsnapFromBoard();
        }
    }

    private void OnDestroy()
    {
        // Remove listeners to prevent memory leaks
        grabInteractable.selectExited.RemoveListener(OnRelease);
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }
}