using UnityEngine;

public class MouseDragSticker : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Camera mainCamera;

    // Snap zone settings
    public Transform snapZone;          // Position of the snap zone
    public float snapDistance = 1.0f;   // How close the sticker needs to be to snap
    private Renderer snapZoneRenderer;  // Renderer for visual feedback (color change)
    public Color snapHoverColor = Color.green;
    private Color originalSnapZoneColor;

    void Start()
    {
        mainCamera = Camera.main;

        // Store the original color of the snap zone
        if (snapZone != null)
        {
            snapZoneRenderer = snapZone.GetComponent<Renderer>();
            if (snapZoneRenderer != null)
            {
                originalSnapZoneColor = snapZoneRenderer.material.color;
            }
        }
    }

    void OnMouseDown()
    {
        // Calculate the offset between the mouse position and the sticker's position
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            // Set the sticker's position to follow the mouse, adjusting for the offset
            transform.position = GetMouseWorldPosition() + offset;

            // Check if the sticker is close to the snap zone
            if (IsNearSnapZone())
            {
                // Change snap zone color to indicate hover
                if (snapZoneRenderer != null)
                {
                    snapZoneRenderer.material.color = snapHoverColor;
                }
            }
            else
            {
                // Revert snap zone color
                if (snapZoneRenderer != null)
                {
                    snapZoneRenderer.material.color = originalSnapZoneColor;
                }
            }
        }
    }

    void OnMouseUp()
    {
        // Stop dragging when the mouse button is released
        isDragging = false;

        // Snap the sticker if it's close to the snap zone
        if (IsNearSnapZone())
        {
            SnapToZone();
        }

        // Reset snap zone color when the sticker is released
        if (snapZoneRenderer != null)
        {
            snapZoneRenderer.material.color = originalSnapZoneColor;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Get the current mouse position in world space
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;  // Keep the same Z depth

        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    private bool IsNearSnapZone()
    {
        // Check if the sticker is within the snap distance of the snap zone
        if (snapZone != null)
        {
            return Vector3.Distance(transform.position, snapZone.position) <= snapDistance;
        }
        return false;
    }

    private void SnapToZone()
    {
        // Snap the sticker to the snap zone's position
        if (snapZone != null)
        {
            transform.position = snapZone.position;
        }
    }
}
