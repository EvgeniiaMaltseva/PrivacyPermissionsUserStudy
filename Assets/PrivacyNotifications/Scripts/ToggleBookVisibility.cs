using UnityEngine;
using UnityEngine.UI;

public class ToggleBookVisibility : MonoBehaviour
{
    public GameObject bookUI;
    public GameObject bookIcon;
    public Button confirmButton;
    public Button iconButton;

    public UIFollowPlayer uiFollowPlayer;

    public VRGazeTimeTracker gazeTracker;

    private PermissionManager permissionManager;

    private void Start()
    {
        permissionManager = FindObjectOfType<PermissionManager>();
        if (bookUI == null)
        {
            Debug.LogError("Book UI is not assigned in the inspector");
        }

        if (bookIcon == null)
        {
            Debug.LogError("Book Icon is not assigned in the inspector");
        }

        if (confirmButton == null)
        {
            Debug.LogError("Confirm Button is not assigned in the inspector");
        }
        else
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        if (iconButton == null)
        {
            Debug.LogError("Icon Button is not assigned in the inspector");
        }
        else
        {
            iconButton.onClick.AddListener(OnIconClicked);
        }
        bookIcon.SetActive(false);
        bookUI.SetActive(true);

        if (gazeTracker != null)
        {
            gazeTracker.ToggleTracking(true);
        }
    }

    // Hide the book UI and show the icon
    // Pause gaze tracking
    public void OnConfirmButtonClicked()
    {

        bookUI.SetActive(false);
        bookIcon.SetActive(true);
        permissionManager.CheckRoomPermission(this.transform.position);
        //Debug.Log("Book UI hidden and icon displayed");

        if (gazeTracker != null)
        {
            gazeTracker.ToggleTracking(false);
        }
        Debug.Log("Book UI hidden, icon displayed, and gaze tracking paused");
    }

    // Show the book UI and hide the icon
    // Resume gaze tracking
    // Trigger the UI to start following the player
    public void OnIconClicked()
    {
        bookUI.SetActive(true);
        bookIcon.SetActive(false);

        if (permissionManager.currentStickerDisplay != null)
        {
            permissionManager.currentStickerDisplay.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No suitable sticker display found for this room");
        }

        if (gazeTracker != null)
        {
            gazeTracker.ToggleTracking(true);
        }
        Debug.Log("Book UI displayed, icon hidden, and gaze tracking resumed");

        uiFollowPlayer.StartFollowingPlayer();
    }
}
