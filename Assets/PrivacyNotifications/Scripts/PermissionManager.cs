using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermissionManager : MonoBehaviour
{
    [Header("Sticker Settings")]
    public List<GameObject> stickerDisplays; // Static sticker displays in each room
    public List<StickerMaterialPair> stickerMaterialPairs; // Configurable via Inspector

    [Header("UI Settings")]
    public GameObject notificationPanel; // Notification UI Panel
    public Button skipButton; // Skip Room button
    public Button settingsButton; // Open Settings button
    public TMPro.TextMeshProUGUI notificationText; // Text to display permission info

    [Header("Room Settings")]
    public float roomRange = 5f; // Range for detecting rooms
    public Dictionary<Vector3, string> roomRequirements = new Dictionary<Vector3, string>(); // Room permissions

    private Dictionary<string, Material> stickerMaterials = new Dictionary<string, Material>();
    private Dictionary<string, bool> permissions = new Dictionary<string, bool>();
    private Teleporter teleporter;

    private ToggleBookVisibility toggleBookVisibility;

    // Tracks the currently active sticker display
    public GameObject currentStickerDisplay;

    // New: List of skipped rooms
    private HashSet<Vector3> skippedRooms = new HashSet<Vector3>();

    public VRGazeTimeTracker gazeTracker;  // Reference to the VRGazeTimeTracker script

    void Start()
    {
        toggleBookVisibility = FindObjectOfType<ToggleBookVisibility>();
        teleporter = FindObjectOfType<Teleporter>();
        if (teleporter == null)
        {
            Debug.LogError("Teleporter not found in the scene!");
        }

        // Populate the sticker materials dictionary
        foreach (var pair in stickerMaterialPairs)
        {
            stickerMaterials[pair.stickerName] = pair.stickerMaterial;
        }

        // Initialize permissions
        permissions["Eye Tracking"] = false;
        permissions["Body Tracking"] = false;
        permissions["Personal Data"] = false;
        permissions["Cognitive Performance"] = false;
        permissions["Voice Recording"] = false;
        permissions["Location"] = false;

        // Configure rooms with their required permissions
        roomRequirements[new Vector3(-10.701f, 0f, -4.24f)] = "Eye Tracking";
        roomRequirements[new Vector3(-15.587f, 0f, -17.425f)] = "Body Tracking";
        roomRequirements[new Vector3(-22.149f, 3.5f, -17.242f)] = "Personal Data";
        roomRequirements[new Vector3(12.85f, 3.5f, -15.76f)] = "Cognitive Performance";
        roomRequirements[new Vector3(24.61f, 3.5f, -1.98f)] = "Voice Recording";
        roomRequirements[new Vector3(23.03f, 3.5f, -15.95f)] = "Location";

        // Assign button listeners
        skipButton.onClick.AddListener(SkipRoom);
        settingsButton.onClick.AddListener(OpenSettings);

        // Hide notification at the start
        notificationPanel.SetActive(false);

        // Ensure all sticker displays are initially cleared
        ClearAllStickers();
    }

    public void CheckRoomPermission(Vector3 userPosition)
    {
        ClearAllStickers(); // Reset all displays at the start
        notificationPanel.SetActive(false);
        currentStickerDisplay = null;

        foreach (var room in roomRequirements)
        {
            float distance = Vector3.Distance(userPosition, room.Key);
            if (distance <= roomRange) // If within range of the room
            {
                // Skip notification if room was already skipped
                if (skippedRooms.Contains(room.Key))
                {
                    Debug.Log($"Room at {room.Key} was previously skipped. No notification.");
                    return; // Exit immediately to avoid further processing
                }

                string requiredPermission = room.Value;
                Debug.Log($"Checking room at {room.Key}. Required permission: {requiredPermission}");

                if (permissions.TryGetValue(requiredPermission, out bool isAllowed))
                {
                    if (!isAllowed)
                    {
                        // Show notification
                        notificationText.text = $"This room requires {requiredPermission}";
                        notificationPanel.SetActive(true);
                        toggleBookVisibility.bookUI.SetActive(false);
                        toggleBookVisibility.bookIcon.SetActive(false);
                        toggleBookVisibility.gazeTracker.ToggleTracking(false);
                        gazeTracker.SaveTotalGazeTimeToFile();

                        Debug.Log($"Permission {requiredPermission} is required but not granted.");

                        // Show red sticker
                        ActivateStickerDisplay(room.Key, $"Red_{requiredPermission}");
                        return;
                    }
                    else
                    {
                        Debug.Log($"Permission {requiredPermission} is granted.");

                        // Show green sticker
                        ActivateStickerDisplay(room.Key, $"Green_{requiredPermission}");
                    }
                }
                else
                {
                    Debug.LogError($"Permission {requiredPermission} is not defined.");
                }
                return;
            }
        }

        // Clear stickers if not in any room range
        Debug.Log("No room in range. Clearing stickers.");
        ClearAllStickers();
    }

    private void ActivateStickerDisplay(Vector3 roomPosition, string stickerName)
    {
        // Find the nearest sticker display to the room position
        float minDistance = float.MaxValue;

        foreach (var sticker in stickerDisplays)
        {
            float distance = Vector3.Distance(sticker.transform.position, roomPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentStickerDisplay = sticker;
            }
        }

        if (currentStickerDisplay != null)
        {
            MeshRenderer renderer = currentStickerDisplay.GetComponent<MeshRenderer>();
            if (stickerMaterials.TryGetValue(stickerName, out Material material))
            {
                renderer.material = material;
                currentStickerDisplay.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("No suitable sticker display found for this room.");
        }
    }

    public void ClearAllStickers()
    {
        foreach (var sticker in stickerDisplays)
        {
            if (sticker != null)
            {
                sticker.SetActive(false);
                MeshRenderer renderer = sticker.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = null;
                }
            }
        }
        currentStickerDisplay = null;
    }

    public void UpdatePermission(string permission, bool isAllowed)
    {
        if (permissions.ContainsKey(permission))
        {
            permissions[permission] = isAllowed;
            Debug.Log($"Permission {permission} updated to {isAllowed}");
        }
    }

    public void SkipRoom()
    {
        toggleBookVisibility.bookIcon.SetActive(true);

        if (teleporter != null && teleporter.CanSkipToNextRoom())
        {
            // Mark the current room as skipped
            Vector3 currentRoomPosition = teleporter.GetCurrentRoomPosition();
            skippedRooms.Add(currentRoomPosition);
            Debug.Log($"Room at {currentRoomPosition} marked as skipped.");

            Vector3 nextRoomPosition = teleporter.GetNextPosition();
            teleporter.MoveToNextRoom();
            CheckRoomPermission(nextRoomPosition);

            Debug.Log($"Skipped to the next room at position {nextRoomPosition}");
        }
        else
        {
            Debug.Log("No more rooms to skip to.");
        }
    }

    void OpenSettings()
    {
        // Open book or settings UI for sticker change
        Debug.Log("Opening book for sticker selection...");
        notificationPanel.SetActive(false);
    }
}
