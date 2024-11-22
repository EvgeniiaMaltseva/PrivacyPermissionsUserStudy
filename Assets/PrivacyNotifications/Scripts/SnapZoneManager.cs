using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapZoneManager : MonoBehaviour
{
    public List<Transform> snapZones;    // List of all snap zones
    public Button confirmButton;         // Reference to the confirm button

    private HashSet<Transform> occupiedZones = new HashSet<Transform>(); // Tracks occupied zones

    private void Start()
    {
        if (confirmButton != null)
        {
            confirmButton.interactable = false; // Disable confirm button initially
        }
    }

    public void UpdateSnapZoneStatus(Transform snapZone, bool isOccupied)
    {
        if (isOccupied)
        {
            occupiedZones.Add(snapZone);
        }
        else
        {
            occupiedZones.Remove(snapZone);
        }

        // Check if all snap zones are occupied
        CheckAllZonesOccupied();
    }

    private void CheckAllZonesOccupied()
    {
        if (occupiedZones.Count == snapZones.Count)
        {
            confirmButton.interactable = true; // Enable the confirm button
        }
        else
        {
            confirmButton.interactable = false; // Disable the confirm button
        }
    }
}
