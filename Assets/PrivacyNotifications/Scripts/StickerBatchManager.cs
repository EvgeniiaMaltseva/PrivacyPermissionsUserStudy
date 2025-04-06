using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerBatchManager : MonoBehaviour
{
    public List<XRDragSticker> stickers;
    public Button acceptAllButton;
    public Button rejectAllButton;


    private void Start()
    {
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

    // Snaps a sticker to its book snap zone, replacing any existing sticker in that zone and updating its state
    private void SnapStickerToBook(XRDragSticker sticker)
    {
        if (sticker.bookSnapZone != null)
        {
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
            sticker.transform.position = sticker.bookSnapZone.position;
            sticker.transform.rotation = sticker.bookSnapZone.rotation;
            sticker.transform.SetParent(sticker.bookSnapZone);

            if (sticker.bookSnapZone.GetComponent<MeshRenderer>() is MeshRenderer renderer)
            {
                renderer.enabled = false;
            }

            sticker.snapZoneManager.UpdateSnapZoneStatus(sticker.bookSnapZone, true, sticker.gameObject);
            sticker.UpdatePermissionBasedOnSticker();

            sticker.rb.isKinematic = true;
            sticker.snappedToBook = true;
            sticker.snappedToBoard = false;

            Debug.Log($"Sticker {sticker.gameObject.name} snapped to book zone");
        }
        else
        {
            Debug.LogWarning($"Sticker {sticker.gameObject.name} does not have a valid book snap zone");
        }
    }
}
