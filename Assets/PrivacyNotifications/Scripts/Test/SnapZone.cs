using UnityEngine;
using UnityEngine; // Add this for XRGrabInteractable


public class SnapZone : MonoBehaviour
{
    public Transform snapPosition;  // The position where the sticker will snap
    public bool isOccupied = false; // To prevent multiple stickers from snapping to the same spot

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is a sticker and the snap zone isn't occupied
        if (other.CompareTag("Sticker") && !isOccupied)
        {
            SnapSticker(other.transform);
            // Highlight the snap zone, maybe by changing its material color
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    void SnapSticker(Transform sticker)
    {
        // Set the sticker's position and rotation to match the snap position
        sticker.position = snapPosition.position;
        sticker.rotation = snapPosition.rotation;

        // Mark this snap zone as occupied
        isOccupied = true;

        // Optionally: You can disable the XRGrabInteractable so that once snapped, the sticker cannot be grabbed again
        var interactable = sticker.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (interactable != null)
        {
            interactable.enabled = false;
        }
    }

    // Optional: Reset the zone when sticker is removed
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sticker"))
        {
            isOccupied = false;
            // Reset the material color when the sticker leaves the zone
            GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
