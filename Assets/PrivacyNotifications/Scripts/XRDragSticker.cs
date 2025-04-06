using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDragSticker : MonoBehaviour
{
    public Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    public Transform bookSnapZone;
    public Transform boardSnapZone;
    public float snapDistance = 0.5f;
    public bool snappedToBook = false;
    public bool snappedToBoard = false;
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

        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.selectEntered.AddListener(OnGrab);

        // Get the MeshRenderer component of the book snap zone
        if (bookSnapZone != null)
        {
            bookSnapZoneRenderer = bookSnapZone.GetComponent<MeshRenderer>();
        }
    }

    // Allows sticker to be dropped
    // Calculate distances to each snap zone
    // If neither snap zone is within snapping range, return to board zone
    // If close enough to one of the snap zones, snap to it
    private void OnRelease(SelectExitEventArgs args)
    {
        rb.isKinematic = false;

        float distanceToBookSnapZone = Vector3.Distance(transform.position, bookSnapZone.position);
        float distanceToBoardSnapZone = Vector3.Distance(transform.position, boardSnapZone.position);

        if (distanceToBookSnapZone > snapDistance && distanceToBoardSnapZone > snapDistance)
        {
            SnapToBoard();
        }
        else
        {
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

    // Snaps this sticker to its book snap zone, replacing any existing sticker and updating its state
    private void SnapToBook()
    {
        if (snapZoneManager.IsSnapZoneOccupied(bookSnapZone))
        {
            GameObject occupyingSticker = snapZoneManager.GetOccupyingSticker(bookSnapZone);

            if (occupyingSticker != null)
            {
                var stickerComponent = occupyingSticker.GetComponent<XRDragSticker>();
                if (stickerComponent != null)
                {
                    stickerComponent.SnapToBoard();
                }
            }
        }

        transform.position = bookSnapZone.position;
        transform.rotation = bookSnapZone.rotation;

        transform.SetParent(bookSnapZone);

        if (bookSnapZoneRenderer != null)
        {
            bookSnapZoneRenderer.enabled = false;
        }

        Debug.Log("Sticker snapped to book zone at position: " + transform.position.ToString("F3"));

        rb.isKinematic = true;
        snappedToBook = true;
        snappedToBoard = false;

        snapZoneManager.UpdateSnapZoneStatus(bookSnapZone, true, this.gameObject);

        UpdatePermissionBasedOnSticker();
    }

    // Parses the sticker's name to determine permission status and updates the PermissionManager accordingly
    public void UpdatePermissionBasedOnSticker()
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

                Vector3 userPosition = transform.position; 
                Debug.Log($"Checking permissions for user at position: {userPosition}");
                //permissionManager.CheckRoomPermission(userPosition);
            }
        }
    }

    private void UnsnapFromBook()
    {
        grabInteractable.enabled = true;
        rb.isKinematic = false;
        snappedToBook = false;

        if (bookSnapZoneRenderer != null)
        {
            bookSnapZoneRenderer.enabled = true;
        }

        Debug.Log("Sticker unsnapped from the book zone");

        snapZoneManager.UpdateSnapZoneStatus(bookSnapZone, false, null);
    }

    public void SnapToBoard()
    {

        transform.position = boardSnapZone.position;
        transform.rotation = boardSnapZone.rotation;

        transform.SetParent(originalParent);
        transform.localScale = originalScale;

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

        Debug.Log("Sticker unsnapped from the board zone");
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
        grabInteractable.selectExited.RemoveListener(OnRelease);
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }
}