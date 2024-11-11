using UnityEngine;
using UnityEngine.UI;
using System.IO;  // Import the System.IO namespace for file operations

public class GazeTimeTracker : MonoBehaviour
{
    public Button confirmButton;

    private float gazeTime = 0f;  // Time spent gazing at the UI window
    private bool isGazing = false;

    private string filePath = "Assets/PrivacyNotifications/Data/GazeDurations.txt";  // Path to the file where the gaze time will be saved

    void Start()
    {
        // Attach the click listener to the confirm button
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
    }

    void Update()
    {
        // Check if the mouse is over the UI window (or the button)
        if (IsMouseOverUI())
        {
            if (!isGazing)
            {
                // Start the gaze timer when the mouse first enters
                gazeTime = 0f;
                isGazing = true;
            }

            // Increase the gaze time while the mouse is over
            gazeTime += Time.deltaTime;
        }
        else
        {
            if (isGazing)
            {
                // Reset gaze time if the mouse leaves the UI
                isGazing = false;
                gazeTime = 0f;
            }
        }
    }

    private bool IsMouseOverUI()
    {
        // Use Unity's EventSystem to check if the mouse is hovering over the UI
        // Returns true if the mouse is over any UI element
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    private void OnConfirmButtonClicked()
    {
        // Save the gaze time to a file when the confirm button is clicked
        SaveGazeTimeToFile(gazeTime);
    }

    private void SaveGazeTimeToFile(float gazeTime)
    {
        // Create the directory if it doesn't exist
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Format the time to save it to the file
        //string gazeTimeText = "Gaze Time: " + Mathf.RoundToInt(gazeTime) + " seconds";

        string data = "Gaze Duration: " + gazeTime + " seconds, Time: " + System.DateTime.Now + "\n";


        // Append the gaze time to the file
        try
        {
            // Using StreamWriter to write the gaze time to the file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(data);  // Append the time with a new line
            }

            Debug.Log("Gaze time saved to file: " + data);
        }
        catch (IOException e)
        {
            Debug.LogError("Error saving gaze time to file: " + e.Message);
        }
    }
}
