using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerBatchManager : MonoBehaviour
{
    public List<XRDragSticker> stickers; // List of all stickers in the scene
    public Button acceptAllButton;       // Reference to the Accept All button
    public Button rejectAllButton;       // Reference to the Reject All button


    private void Start()
    {
        // Ensure buttons have listeners
        if (acceptAllButton != null)
        {
            acceptAllButton.onClick.AddListener(AcceptAllStickers);
        }

        if (rejectAllButton != null)
        {
            rejectAllButton.onClick.AddListener(RejectAllStickers);
        }
    }

    public void AcceptAllStickers()
    {
        foreach (var sticker in stickers)
        {
            if (sticker.gameObject.name.Contains("Green"))
            {
                SnapStickerToBook(sticker);
            }
        }
    }

    public void RejectAllStickers()
    {
        foreach (var sticker in stickers)
        {
            if (sticker.gameObject.name.Contains("Red"))
            {
                SnapStickerToBook(sticker);
            }
        }
    }

    private void SnapStickerToBook(XRDragSticker sticker)
    {
        // Ensure the sticker has a valid book snap zone
        if (sticker.bookSnapZone != null)
        {
            // If the snap zone is occupied, move the occupying sticker back to the board
            if (sticker.snapZoneManager.IsSnapZoneOccupied(sticker.bookSnapZone))
            {
                var occupyingSticker = sticker.snapZoneManager.GetOccupyingSticker(sticker.bookSnapZone);
                if (occupyingSticker != null)
                {
                    var occupyingStickerComponent = occupyingSticker.GetComponent<XRDragSticker>();
                    if (occupyingStickerComponent != null)
                    {
                        occupyingStickerComponent.SnapToBoard();
                    }
                }
            }

            // Snap this sticker to the book snap zone
            sticker.transform.position = sticker.bookSnapZone.position;
            sticker.transform.rotation = sticker.bookSnapZone.rotation;

            // Set the sticker's parent to the snap zone
            sticker.transform.SetParent(sticker.bookSnapZone);

            // Hide the snap zone renderer if applicable
            if (sticker.bookSnapZone.GetComponent<MeshRenderer>() is MeshRenderer renderer)
            {
                renderer.enabled = false;
            }

            // Notify the SnapZoneManager of the updated status
            sticker.snapZoneManager.UpdateSnapZoneStatus(sticker.bookSnapZone, true, sticker.gameObject);
            sticker.UpdatePermissionBasedOnSticker();

            // Update the sticker's internal state
            sticker.rb.isKinematic = true;
            sticker.snappedToBook = true;
            sticker.snappedToBoard = false;

            Debug.Log($"Sticker {sticker.gameObject.name} snapped to book zone.");
        }
        else
        {
            Debug.LogWarning($"Sticker {sticker.gameObject.name} does not have a valid book snap zone.");
        }
    }
}
