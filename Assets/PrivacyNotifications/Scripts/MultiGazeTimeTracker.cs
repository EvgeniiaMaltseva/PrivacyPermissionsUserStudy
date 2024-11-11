using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class MultiGazeTimeTracker : MonoBehaviour
{
    [System.Serializable]
    public class GazeUIView
    {
        public GameObject uiView;            // The entire UI view to track
        public Button confirmButton;         // The confirm button for this specific UI view
        public string viewName;              // A name for this view, to identify it in the log
        public float gazeTime = 0f;          // Time spent gazing at this UI view
        public bool isGazing = false;        // Whether the user is currently gazing at this view
        public bool trackingStopped = false; // Flag to stop tracking when confirm button is clicked
    }

    public List<GazeUIView> gazeUIViews = new List<GazeUIView>(); // List of UI views to track
    private string filePath = "Assets/PrivacyNotifications/Data/GazeDurations.txt"; // Path to save gaze times

    void Start()
    {
        // Attach the click listener for each confirm button in gazeUIViews
        foreach (var view in gazeUIViews)
        {
            view.confirmButton.onClick.AddListener(() => StopTracking(view));
        }
    }

    void Update()
    {
        // Iterate through each UI view to track gaze time
        foreach (var view in gazeUIViews)
        {
            if (!view.trackingStopped)
            {
                if (IsMouseOverUIView(view.uiView))
                {
                    if (!view.isGazing)
                    {
                        // Start the gaze timer for this view
                        view.gazeTime = 0f;
                        view.isGazing = true;
                    }

                    // Increase gaze time for this view
                    view.gazeTime += Time.deltaTime;
                }
                else if (view.isGazing)
                {
                    // Reset gaze time if the mouse leaves the UI view
                    view.isGazing = false;
                    view.gazeTime = 0f;
                }
            }
        }
    }

    private bool IsMouseOverUIView(GameObject uiView)
    {
        // Check if the mouse is over the specific UI view
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    private void StopTracking(GazeUIView view)
    {
        // Set trackingStopped to true to stop further gaze tracking for this view
        view.trackingStopped = true;

        // Log or save the final gaze time for this specific view
        SaveGazeTimeToFile(view.viewName, view.gazeTime);
    }

    private void SaveGazeTimeToFile(string viewName, float gazeTime)
    {
        // Ensure the directory exists
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Format the gaze time text to be saved
        //string gazeTimeText = $"{viewName} - Gaze Time: {Mathf.RoundToInt(gazeTime)} seconds";
        string data = $"{viewName} - Gaze Duration: " + gazeTime + " seconds, Time: " + System.DateTime.Now + "\n";


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
