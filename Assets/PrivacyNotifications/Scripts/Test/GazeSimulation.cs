using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GazeSimulation : MonoBehaviour
{
    public GameObject permissionWindow;
    public Button confirmButton;

    private float gazeStartTime = 0f;
    private bool isGazingAtWindow = false;
    private float gazeDuration = 0f;

    // Define the file path directly within the Assets folder
    private string filePath = "Assets/PrivacyNotifications/Data/GazeDurations.txt";

    void Start()
    {
        // Ensure the directory exists
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log("Created directory: " + directoryPath);
        }

        Debug.Log("Data will be saved to: " + filePath);
    }

    void Update()
    {
#if UNITY_EDITOR
        // Simulate gaze with mouse position for testing in the editor
        Ray gazeRay = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
        // Use actual gaze data from Varjo on headset
        Ray gazeRay = VarjoEyeTracking.GetGazeRay();
#endif
        RaycastHit hit;
        if (Physics.Raycast(gazeRay, out hit))
        {
            if (hit.collider.gameObject == permissionWindow)
            {
                if (!isGazingAtWindow)
                {
                    // Start gaze timer
                    isGazingAtWindow = true;
                    gazeStartTime = Time.time;
                }
            }
            else if (isGazingAtWindow)
            {
                // Stop gaze timer and calculate duration
                isGazingAtWindow = false;
                gazeDuration += Time.time - gazeStartTime;
                Debug.Log("Simulated gaze duration so far: " + gazeDuration + " seconds");
            }
        }
    }

    private void OnEnable()
    {
        confirmButton.onClick.AddListener(OnConfirmClicked);
    }

    private void OnDisable()
    {
        confirmButton.onClick.RemoveListener(OnConfirmClicked);
    }

    private void OnConfirmClicked()
    {
        if (isGazingAtWindow)
        {
            // Calculate final gaze duration if user is still looking
            gazeDuration += Time.time - gazeStartTime;
            isGazingAtWindow = false;
        }

        Debug.Log("Confirm button clicked. Final gaze duration: " + gazeDuration + " seconds");

        // Save the gaze duration to a file
        SaveGazeDurationToFile(gazeDuration);

        // Reset the gaze duration
        gazeDuration = 0f;
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
