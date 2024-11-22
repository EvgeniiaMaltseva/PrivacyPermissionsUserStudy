using UnityEngine;
using UnityEngine.UI;

public class ToggleBookVisibility : MonoBehaviour
{
    public GameObject bookUI;              // The book UI element to show/hide
    public GameObject bookIcon;            // The icon to show when the book UI is hidden
    public Button checkMarkButton;         // The check mark button inside the book UI
    public Button iconButton;              // The button component on the book icon

    public UIFollowPlayer uiFollowPlayer; // Reference to the UIFollowPlayer script


    private void Start()
    {
        // Ensure references are set up correctly
        if (bookUI == null)
        {
            Debug.LogError("Book UI is not assigned in the inspector.");
        }

        if (bookIcon == null)
        {
            Debug.LogError("Book Icon is not assigned in the inspector.");
        }

        if (checkMarkButton == null)
        {
            Debug.LogError("Check Mark Button is not assigned in the inspector.");
        }
        else
        {
            // Add a listener for the check mark button click event
            checkMarkButton.onClick.AddListener(OnCheckMarkClicked);
        }

        if (iconButton == null)
        {
            Debug.LogError("Icon Button is not assigned in the inspector.");
        }
        else
        {
            // Add a listener for the icon button click event
            iconButton.onClick.AddListener(OnIconClicked);
        }

        // Initially hide the icon and show the book UI
        bookIcon.SetActive(false);
        bookUI.SetActive(true);
    }

    public void OnCheckMarkClicked()
    {
        // Hide the book UI and show the icon
        bookUI.SetActive(false);
        bookIcon.SetActive(true);
        Debug.Log("Book UI hidden and icon displayed.");
    }

    public void OnIconClicked()
    {
        // Show the book UI and hide the icon
        bookUI.SetActive(true);
        bookIcon.SetActive(false);
        Debug.Log("Book UI displayed and icon hidden.");
        // Trigger the UI to start following the player
        uiFollowPlayer.StartFollowingPlayer();
    }
}
