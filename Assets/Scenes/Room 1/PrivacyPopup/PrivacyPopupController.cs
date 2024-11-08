using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PrivacyPopupController : MonoBehaviour
{
    public GameObject panel;  // Reference to the UI Panel (which contains the text and buttons)

    // This method will be called when the Agree button is clicked
    public void OnAgreeClicked()
    {
        // Hide the panel
        panel.SetActive(false);
    }

    // This method will be called when the Disagree button is clicked
    public void OnDisagreeClicked()
    {
        // Hide the panel
        panel.SetActive(false);

        // Optionally, you could trigger some other action here (like exiting the game or showing a warning)
        // Application.Quit(); // Uncomment if you want to exit the application on Disagree
    }
}
