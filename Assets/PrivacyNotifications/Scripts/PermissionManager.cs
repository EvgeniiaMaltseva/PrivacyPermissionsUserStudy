using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermissionManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI notificationText;
    private Dictionary<string, bool> permissions = new Dictionary<string, bool>();
    private Dictionary<Vector3, string> roomRequirements = new Dictionary<Vector3, string>(); // Room-to-permission mapping
    private Coroutine hideNotificationCoroutine;

    void Start()
    {
        // Initialize permissions
        permissions["Eye Tracking"] = false;
        permissions["Body Tracking"] = false;
        permissions["Personal Data"] = false;
        permissions["Cognitive Perfomance"] = false;
        permissions["Voice Recording"] = false;
        permissions["Location"] = false;

        if (notificationText != null)
        {
            ClearNotification();
        }

        // Set up room-to-permission mappings
        roomRequirements[new Vector3(-10.701f, 0f, -4.24f)] = "Eye Tracking"; // Room-4 find letters
        roomRequirements[new Vector3(-15.587f, 0f, -17.425f)] = "Body Tracking"; // Room-9 fitness
        roomRequirements[new Vector3(-22.149f, 3.5f, -17.242f)] = "Personal Data"; // Room-13 language
        roomRequirements[new Vector3(-10.84f, 3.5f, -3.78f)] = "Cognitive Perfomance"; // Room-16 naming animals
        roomRequirements[new Vector3(24.61f, 3.5f, -0.98f)] = "Voice Recording"; // Room-20 repeat after robot
        roomRequirements[new Vector3(23.03f, 3.5f, -15.95f)] = "Location"; // Room-23 location
    }

    // Update permission status based on the sticker placed
    public void UpdatePermission(string permission, bool isAllowed)
    {
        if (permissions.ContainsKey(permission))
        {
            permissions[permission] = isAllowed;
            Debug.Log($"Permission {permission} updated to {isAllowed}");
            // Check if the notification is about the missing permission
            if (notificationText != null && notificationText.text.Contains($"Permission '{permission}' is required"))
            {
                ClearNotification();
            }
        }
    }

    // Check permissions when entering a room
    public void CheckRoomPermission(Vector3 roomPosition)
    {
        if (roomRequirements.TryGetValue(roomPosition, out string requiredPermission))
        {
            if (permissions.TryGetValue(requiredPermission, out bool isAllowed))
            {
                if (!isAllowed)
                {
                    // Notify user about the required permission if not allowed
                    NotifyUser($"Permission '{requiredPermission}' is required to proceed in this room", Color.red, 0.06f);
                }
                else
                {
                    // Notify user that data recording has started
                    NotifyUser($"Data recording for '{requiredPermission}' has started", Color.green, 0.05f);
                    // Hide the notification after 5 seconds
                    if (hideNotificationCoroutine != null)
                    {
                        StopCoroutine(hideNotificationCoroutine);
                    }
                    hideNotificationCoroutine = StartCoroutine(HideNotificationAfterDelay(5f));
                }
            }
            else
            {
                Debug.LogError($"Permission {requiredPermission} is not defined.");
            }
        }
        else
        {
            Debug.Log($"Room at position {roomPosition} does not require any specific permission.");
        }
    }

    // Display a notification to the user
    private void NotifyUser(string message, Color color, float fontSize)
    {
        if (notificationText != null)
        {
            notificationText.text = message;
            notificationText.color = color;       
            notificationText.fontSize = fontSize;   
        }

        Debug.Log(message);
    }

    // Coroutine to hide the notification after a delay
    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearNotification();
    }

    public void ClearNotification()
    {
        if (notificationText != null)
        {
            notificationText.text = "";
        }

        Debug.Log("Notification cleared.");
    }
}
