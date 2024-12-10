using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class VRGazeTimeTracker : MonoBehaviour
{
    public Transform cameraTransform;  // The Main Camera under XR Origin -> Camera Offset -> Main Camera
    public GameObject uiParent;        // Parent GameObject containing all UI elements
    public Button confirmButton;       // Button to confirm selection and save gaze data
    public string filePath = "Assets/PrivacyNotifications/Data/GazeDurations.csv";  // File to save gaze data

    private float totalGazeTime = 0f;         // Total time spent gazing at all UI elements
    private GameObject currentGazedObject;    // The currently gazed-at UI element
    private bool isTracking = true;           // Toggle for whether gaze tracking is active
    private bool isGazing = false;            // Whether the user is currently looking at a valid UI element

    void Start()
    {
        // Ensure the directory for saving gaze data exists
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Add the listener for the confirm button
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        Debug.Log("Gaze Time Tracker initialized. Ready to track gaze data.");
    }

    void Update()
    {
        if (!isTracking) return;  // Stop tracking if confirm has been pressed

        // Simulate camera rotation with the mouse for testing
        SimulateMouseCameraRotation();

        // Cast a ray from the simulated camera position
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        // Perform raycast to detect objects
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Check if the hit object is part of the UI
            if (IsUIElement(hitObject))
            {
                if (!isGazing)
                {
                    Debug.Log($"Started gazing at: {hitObject.name}");
                    isGazing = true;
                }

                currentGazedObject = hitObject;
                totalGazeTime += Time.deltaTime;  // Accumulate total gaze time
                //Debug.Log($"Gazing at {hitObject.name}. Total Gaze Time: {totalGazeTime:F2} seconds");
            }
            else
            {
                // User is not looking at a valid UI element
                if (isGazing)
                {
                    Debug.Log($"Stopped gazing at: {currentGazedObject?.name}");
                    isGazing = false;
                }
                currentGazedObject = null;
            }
        }
        else
        {
            // No object detected by raycast
            if (isGazing)
            {
                Debug.Log($"Stopped gazing at: {currentGazedObject?.name}");
                isGazing = false;
            }
            currentGazedObject = null;
        }

        // Optional: Visualize the ray in the scene for debugging
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 10, Color.red);
    }

    private void SimulateMouseCameraRotation()
    {
        // Rotate the camera based on mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float sensitivity = 2f; // Adjust sensitivity as needed
        cameraTransform.Rotate(Vector3.up, mouseX * sensitivity, Space.World);
        cameraTransform.Rotate(Vector3.left, mouseY * sensitivity);
    }

    private bool IsUIElement(GameObject obj)
    {
        // Check if the object is part of the UI hierarchy under uiParent
        if (uiParent != null && obj.transform.IsChildOf(uiParent.transform))
        {
            return true;
        }

        return false;
    }

    public void ToggleTracking(bool isActive)
    {
        // Toggle tracking when the user interacts with the book UI
        isTracking = isActive;

        if (isTracking)
        {
            Debug.Log("Gaze tracking resumed.");
        }
        else
        {
            Debug.Log("Gaze tracking paused.");
        }
    }
    private void OnConfirmButtonClicked()
    {
        // Stop tracking when confirm is pressed
        //isTracking = false;

        //Debug.Log($"Confirm clicked. Total Gaze Time: {totalGazeTime:F2} seconds");
        SaveTotalGazeTimeToFile(totalGazeTime);
        Debug.Log($"Confirm clicked. Total Gaze Time so far: {totalGazeTime:F2} seconds");

        //Debug.Log("Gaze tracking stopped.");
    }
    // Method with no arguments - uses the internal totalGazeTime variable
    public void SaveTotalGazeTimeToFile()
    {
        SaveTotalGazeTimeToFile(totalGazeTime); // Call the existing method
    }

    public void SaveTotalGazeTimeToFile(float totalGazeTime)
    {
        // Format the data to append
        string data = $"Total Gaze Time: {totalGazeTime:F2} seconds, Date: {System.DateTime.Now}\n";

        try
        {
            // Append the data to the CSV file
            File.AppendAllText(filePath, data);
            Debug.Log($"Gaze time saved: {data}");
        }
        catch (IOException e)
        {
            Debug.LogError("Error saving gaze time to file: " + e.Message);
        }
    }
}
