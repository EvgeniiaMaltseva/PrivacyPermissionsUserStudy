using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class MultiGazeTimeTracker2 : MonoBehaviour
{
    [System.Serializable]
    public class GazeUIView
    {
        public GameObject uiView;            // The entire UI view or object to track (UI or Book + Board)
        public Button confirmButton;         // The confirm button for this specific UI view or object
        public string viewName;              // A name for this view, to identify it in the log
        public float gazeTime = 0f;          // Time spent gazing at this UI view
        public bool isGazing = false;        // Whether the user is currently gazing at this view
        public bool trackingStopped = false; // Flag to stop tracking when confirm button is clicked
    }

    public List<GazeUIView> gazeUIViews = new List<GazeUIView>(); // List of UI views to track
    private string filePath = "Assets/PrivacyNotifications/Data/GazeDurations.txt"; // Path to save gaze times

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Access the main camera (headset's camera)

        // Attach the click listener for each confirm button in gazeUIViews
        foreach (var view in gazeUIViews)
        {
            view.confirmButton.onClick.AddListener(() => StopTracking(view));
        }
    }

    void Update()
    {
        // Iterate through each UI view or object to track gaze time
        foreach (var view in gazeUIViews)
        {
            if (!view.trackingStopped)
            {
                if (IsUserGazingAtUIView(view.uiView))
                {
                    // Start tracking time when the user starts gazing at this view
                    if (!view.isGazing)
                    {
                        view.gazeTime = 0f; // Reset gaze time when first starting to gaze
                        view.isGazing = true;
                    }

                    // Increase gaze time for this view
                    view.gazeTime += Time.deltaTime;
                }
                else if (view.isGazing)
                {
                    // Reset gaze time if the user looks away from the UI view
                    view.isGazing = false;
                    SaveGazeTimeToFile(view.viewName, view.gazeTime); // Save time when gaze stops
                    view.gazeTime = 0f; // Reset gaze time
                }
            }
        }
    }

    // Check if the user's gaze is currently over the specific UI view or object
    private bool IsUserGazingAtUIView(GameObject uiView)
    {
        RaycastHit hit;
        Ray gazeRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward); // Ray from the camera (head)

        // Raycast to check if the user is gazing at the UI or 3D object
        if (Physics.Raycast(gazeRay, out hit))
        {
            if (hit.transform.gameObject == uiView)
            {
                return true; // User is gazing at this UI view or object
            }
        }

        return false; // User is not gazing at this UI view or object
    }

    // Stop tracking when the confirm button is clicked
    private void StopTracking(GazeUIView view)
    {
        view.trackingStopped = true;
        SaveGazeTimeToFile(view.viewName, view.gazeTime);
    }

    // Save gaze time to a file
    private void SaveGazeTimeToFile(string viewName, float gazeTime)
    {
        // Ensure the directory exists
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Format the gaze time text to be saved
        string data = $"{viewName} - Gaze Duration: {gazeTime} seconds, Time: {System.DateTime.Now}\n";

        // Append the gaze time to the file
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(data);
            }
            Debug.Log("Gaze time saved to file: " + data);
        }
        catch (IOException e)
        {
            Debug.LogError("Error saving gaze time to file: " + e.Message);
        }
    }
}
