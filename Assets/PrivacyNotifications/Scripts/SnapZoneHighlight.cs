using UnityEngine;

public class SnapZoneHighlight : MonoBehaviour
{
    public Transform allowSticker; // Reference to the "allow" sticker
    public Transform notAllowSticker; // Reference to the "not allow" sticker
    public float highlightDistance = 0.5f; // Distance for snap zone highlighting
    
    private Renderer zoneRenderer;
    private Color originalColor;
    public Color allowHighlightColor = Color.green; // Highlight color for "allow" sticker
    public Color notAllowHighlightColor = Color.red; // Highlight color for "not allow" sticker

    private void Start()
    {
        zoneRenderer = GetComponent<Renderer>();
        
        if (zoneRenderer != null)
        {
            originalColor = zoneRenderer.material.color;
        }
        else
        {
            Debug.LogError("Renderer not found on snap zone object!");
        }
    }

    private void Update()
    {
        if (zoneRenderer == null)
        {
            return; // Exit if the renderer is not assigned
        }

        // Track if a sticker is within the highlight distance
        bool isAllowStickerClose = IsStickerClose(allowSticker);
        bool isNotAllowStickerClose = IsStickerClose(notAllowSticker);

        // Log the results of the proximity checks
        Debug.Log($"Allow Sticker Close: {isAllowStickerClose}, Not Allow Sticker Close: {isNotAllowStickerClose}");

        // Highlight the snap zone based on the closest sticker
        if (isAllowStickerClose)
        {
            zoneRenderer.material.color = allowHighlightColor; // Green highlight for "allow" sticker
            Debug.Log("Highlighting snap zone green for allow sticker");
        }
        else if (isNotAllowStickerClose)
        {
            zoneRenderer.material.color = notAllowHighlightColor; // Red highlight for "not allow" sticker
            Debug.Log("Highlighting snap zone red for not allow sticker");
        }
        else
        {
            zoneRenderer.material.color = originalColor; // Reset to original color if no sticker is close
            Debug.Log("Resetting snap zone color to original");
        }
    }

    // Helper method to check if a given sticker is within the highlight distance
    private bool IsStickerClose(Transform sticker)
    {
        if (sticker == null)
        {
            Debug.LogWarning("Sticker reference is null!");
            return false;
        }

        float distance = Vector3.Distance(sticker.position, transform.position);
        Debug.Log($"Distance to {sticker.name}: {distance}"); // Log distance for checking

        return distance <= highlightDistance;
    }
}
