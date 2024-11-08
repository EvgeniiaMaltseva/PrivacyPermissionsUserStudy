using UnityEngine;

public class SnapZoneVR : MonoBehaviour
{
    public Transform snapPosition;  // The position where the sticker will snap
    public bool isOccupied = false; // To prevent multiple stickers from snapping to the same spot

    private Renderer snapZoneRenderer;
    public Color snapHoverColor = Color.green;
    private Color originalColor;

    void Start()
    {
        snapZoneRenderer = GetComponent<Renderer>();
        if (snapZoneRenderer != null)
        {
            originalColor = snapZoneRenderer.material.color;
        }
    }

    public void HighlightZone(bool highlight)
    {
        // Highlight snap zone
        if (snapZoneRenderer != null)
        {
            snapZoneRenderer.material.color = highlight ? snapHoverColor : originalColor;
        }
    }
}
