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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

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
            ReturnToBoard();
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
    private void ReturnToBoard()
    {
        // Directly move the sticker back to the board snap zone if it's too far from both snap zones
        transform.position = boardSnapZone.position;
        transform.rotation = boardSnapZone.rotation;

        Debug.Log("Sticker returned to the board zone because it was too far from both snap zones.");

        rb.isKinematic = true;
        snappedToBoard = true;
        snappedToBook = false;
    }

    private void SnapToBook()
    {
        transform.position = bookSnapZone.position;
        transform.rotation = bookSnapZone.rotation;

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