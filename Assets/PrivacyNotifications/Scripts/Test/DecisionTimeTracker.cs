using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DecisionTimeTracker : MonoBehaviour
{
    public GameObject permissionWindow;  // The window the user stares at
    public Button confirmButton;         // The confirm button on the permission window

    private float gazeStartTime = 0f;
    private bool isTimingGaze = false;
    private float totalGazeDuration = 0f;

    private string filePath = "Assets/PrivacyNotifications/Data/GazeDurations.txt";

    void Start()
    {
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log("Created directory: " + directoryPath);
        }

        Debug.Log("Data will be saved to: " + filePath);
    }

    private void OnEnable()
    {
        // Register events for gaze interaction and confirm button
        confirmButton.onClick.AddListener(OnConfirmClicked);
    }

    private void OnDisable()
    {
        // Unregister events to prevent memory leaks
        confirmButton.onClick.RemoveListener(OnConfirmClicked);
    }

    public void OnGazeEnter()
    {
        // Start timing gaze if it hasn't started yet
        if (!isTimingGaze)
        {
            gazeStartTime = Time.time;
            isTimingGaze = true;
            Debug.Log("Gaze started on permission window at time: " + gazeStartTime);
        }
    }

    public void OnGazeExit()
    {
        // Optional: Stop timing if gaze exits the permission window
        // You may want to reset timing here if the user stops looking, depending on the use case.
    }

    private void OnConfirmClicked()
    {
        if (isTimingGaze)
        {
            // Calculate the total time spent staring at the window until confirm button click
            totalGazeDuration = Time.time - gazeStartTime;
            isTimingGaze = false;  // Stop timing after confirm is clicked

            Debug.Log("Confirm button clicked. Total gaze duration: " + totalGazeDuration + " seconds");

            // Save the gaze duration to a file
            SaveGazeDurationToFile(totalGazeDuration);
        }

        // Hide the permission window after confirm
        permissionWindow.SetActive(false);
    }

    private void SaveGazeDurationToFile(float duration)
    {
        string data = "Gaze Duration: " + duration + " seconds, Time: " + System.DateTime.Now + "\n";
        try
        {
            // Append data to the file
            File.AppendAllText(filePath, data);
            Debug.Log("Gaze duration saved to file at " + filePath);
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to write to file: " + ex.Message);
        }
    }
}
