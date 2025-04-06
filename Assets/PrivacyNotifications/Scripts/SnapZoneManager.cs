using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapZoneManager : MonoBehaviour
{
    public List<Transform> snapZones;
    public Button confirmButton;

    private Dictionary<Transform, bool> snapZoneStatuses = new Dictionary<Transform, bool>();
    private Dictionary<Transform, GameObject> snapZoneOccupyingStickers = new Dictionary<Transform, GameObject>();

    private void Start()
    {
        foreach (var snapZone in snapZones)
        {
            snapZoneStatuses[snapZone] = false;
        }

        if (confirmButton != null)
        {
            confirmButton.interactable = false;
        }
    }

    // Updates the occupancy status of a snap zone and stores the occupying sticker if present
    public void UpdateSnapZoneStatus(Transform snapZone, bool isOccupied, GameObject occupyingSticker = null)
    {
        if (snapZoneStatuses.ContainsKey(snapZone))
        {
            snapZoneStatuses[snapZone] = isOccupied;

            if (isOccupied)
            {
                snapZoneOccupyingStickers[snapZone] = occupyingSticker;
            }
            else
            {
                snapZoneOccupyingStickers[snapZone] = null;
            }
        }
        else
        {
            Debug.LogWarning($"SnapZone {snapZone.name} is not registered in SnapZoneManager");
        }
        CheckAllZonesOccupied();
    }

    public GameObject GetOccupyingSticker(Transform snapZone)
    {
        if (snapZoneOccupyingStickers.TryGetValue(snapZone, out GameObject occupyingSticker))
        {
            return occupyingSticker;
        }
        Debug.LogWarning($"SnapZone {snapZone.name} does not have a tracked occupying sticker");
        return null;
    }

    public bool IsSnapZoneOccupied(Transform snapZone)
    {
        if (snapZoneStatuses.TryGetValue(snapZone, out bool isOccupied))
        {
            return isOccupied;
        }
        Debug.LogWarning($"SnapZone {snapZone.name} is not registered in SnapZoneManager");
        return false;
    }

    // Enable the confirm button only if all snap zones are occupied
    private void CheckAllZonesOccupied()
    {
        confirmButton.interactable = snapZoneStatuses.Count > 0 && !snapZoneStatuses.ContainsValue(false);
    }
}
