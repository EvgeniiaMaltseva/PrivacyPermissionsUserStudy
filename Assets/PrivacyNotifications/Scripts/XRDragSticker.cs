using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDragSticker : MonoBehaviour
{
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    // Snap settings
    public Transform bookSnapZone;        
    public Transform boardSnapZone;       
    public float snapDistance = 0.2f;     
    private bool snappedToBook = false;   
    private bool snappedToBoard = false; 

    private Vector3 originalScale;     
    private Transform originalParent;   

    private MeshRenderer bookSnapZoneRenderer; 
    public SnapZoneManager snapZoneManager;     

    private PermissionManager permissionManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        permissionManager = FindObjectOfType<PermissionManager>();


        // Store original parent and scale
        originalParent = transform.parent;
        originalScale = transform.localScale;

        Debug.Log("Original Parent of " + gameObject.name + " is " + (originalParent != null ? originalParent.name : "None"));

        // Register for the sticker release and grab events
        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.selectEntered.AddListener(OnGrab);

        // Get the MeshRenderer component of the book snap zone
        if (bookSnapZone != null)
        {
            bookSnapZoneRenderer = bookSnapZone.GetComponent<MeshRenderer>();
        }
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
            // If close enough to one of the snap zones, attempt to snap to it
            if (distanceToBookSnapZone <= snapDistance)
            {
                if (snapZoneManager.IsSnapZoneOccupied(bookSnapZone))
                {
                    Debug.Log("Book snap zone is already occupied.");
                    SnapToBoard(); // Return the sticker to the board
                }
                else
                {
                    SnapToBook();
                }
            }
            // If close enough to one of the snap zones, snap to it
            // if (distanceToBookSnapZone <= snapDistance)
            // {
            //     SnapToBook();
            // }
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

        // Hide the book snap zone renderer
        if (bookSnapZoneRenderer != null)
        {
            bookSnapZoneRenderer.enabled = false;
        }

        Debug.Log("Sticker snapped to book zone at position: " + transform.position.ToString("F3"));

        rb.isKinematic = true;
        snappedToBook = true;
        snappedToBoard = false;

        // Notify snap zone manager
        snapZoneManager.UpdateSnapZoneStatus(bookSnapZone, true);

        // Update permission manager dynamically based on sticker type
        UpdatePermissionBasedOnSticker();

    }

    private void UpdatePermissionBasedOnSticker()
    {
        string stickerName = gameObject.name;
        if (stickerName.Contains("_"))
        {
            var parts = stickerName.Split('_');
            if (parts.Length == 2)
            {
                bool isAllowed = parts[0] == "Green";
                string permission = parts[1];
                permissionManager.UpdatePermission(permission, isAllowed);
            }
        }
    }

    private void UnsnapFromBook()
    {
        grabInteractable.enabled = true;
        rb.isKinematic = false;
        snappedToBook = false;

        // Show the book snap zone renderer
        if (bookSnapZoneRenderer != null)
        {
            bookSnapZoneRenderer.enabled = true;
        }

        Debug.Log("Sticker unsnapped from the book zone.");

        // Notify snap zone manager
        snapZoneManager.UpdateSnapZoneStatus(bookSnapZone, false);
    }

    private void SnapToBoard()
    {

        transform.position = boardSnapZone.position;
        transform.rotation = boardSnapZone.rotation;

        // Reset to original parent and original scale
        transform.SetParent(originalParent);
        transform.localScale = originalScale;

        // Show the book snap zone renderer in case it was hidden
        if (bookSnapZoneRenderer != null)
        {
            bookSnapZoneRenderer.enabled = true;
        }

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