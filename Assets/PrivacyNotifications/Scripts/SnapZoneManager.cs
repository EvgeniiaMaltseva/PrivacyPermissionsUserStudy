using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapZoneManager : MonoBehaviour
{
    public List<Transform> snapZones;    // List of all snap zones
    public Button confirmButton;         // Reference to the confirm button

    private Dictionary<Transform, bool> snapZoneStatuses = new Dictionary<Transform, bool>(); // Tracks occupied zones

    private void Start()
    {
        // Initialize all snap zones as unoccupied
        foreach (var snapZone in snapZones)
        {
            snapZoneStatuses[snapZone] = false;
        }

        if (confirmButton != null)
        {
            confirmButton.interactable = false; // Disable confirm button initially
        }
    }

    public void UpdateSnapZoneStatus(Transform snapZone, bool isOccupied)
    {
        if (snapZoneStatuses.ContainsKey(snapZone))
        {
            snapZoneStatuses[snapZone] = isOccupied;
        }
        else
        {
            Debug.LogWarning($"SnapZone {snapZone.name} is not registered in SnapZoneManager!");
            return;
        }

        // Check if all snap zones are occupied
        CheckAllZonesOccupied();
    }

    public bool IsSnapZoneOccupied(Transform snapZone)
    {
        if (snapZoneStatuses.TryGetValue(snapZone, out bool isOccupied))
        {
            return isOccupied;
        }

        Debug.LogWarning($"SnapZone {snapZone.name} is not registered in SnapZoneManager!");
        return false;
    }

    private void CheckAllZonesOccupied()
    {
        // Enable the confirm button only if all snap zones are occupied
        confirmButton.interactable = snapZoneStatuses.Count > 0 && !snapZoneStatuses.ContainsValue(false);
    }
}
