using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SpatialUIGazeTimeTracker : MonoBehaviour
{
    public Transform cameraTransform;            // The Main Camera under XR Origin -> Camera Offset -> Main Camera
    public GameObject spatialUIParent;           // The parent object of the 3D Spatial UI
    public Button confirmButtonSpatialUI;        // The confirm button for the spatial UI
    public string filePath = "Assets/PrivacyNotifications/Data/SpatialUIGazeDurations.csv";  // Path to save gaze data

    private float totalGazeTime = 0f;             // Total time spent gazing at the Spatial UI
    private bool isTracking = true;               // Whether gaze tracking is active
    private bool isGazing = false;                // Whether the user is currently gazing at the Spatial UI

    void Start()
    {
        // Ensure the directory for saving gaze data exists
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Add the listener for the confirm button
        if (confirmButtonSpatialUI != null)
        {
            confirmButtonSpatialUI.onClick.AddListener(OnConfirmButtonSpatialUIClicked);
        }

        Debug.Log("Spatial UI Gaze Time Tracker initialized. Ready to track gaze data.");
    }

    void Update()
    {
        if (!isTracking) return;  // Stop tracking if confirm button has been pressed

        // Simulate camera rotation with the mouse for testing (for testing in the editor only)
        SimulateMouseCameraRotation();

        // Cast a ray from the camera position to detect the Spatial UI (with BoxCollider)
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        // Perform raycast to detect UI elements within the spatial UI parent
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the raycast hit an object within the spatialUIParent
            if (hit.collider.gameObject.transform.IsChildOf(spatialUIParent.transform))
            {
                if (!isGazing)
                {
                    Debug.Log($"Started gazing at: {hit.collider.gameObject.name} (within Spatial UI)");
                    isGazing = true;
                }

                // Accumulate gaze time while looking at the Spatial UI
                totalGazeTime += Time.deltaTime;
                Debug.Log($"Gazing at {hit.collider.gameObject.name}. Total Gaze Time: {totalGazeTime:F2} seconds");
            }
            else
            {
                // If raycast doesn't hit any element within the spatial UI, stop gaze tracking
                if (isGazing)
                {
                    Debug.Log($"Stopped gazing at the Spatial UI.");
                    isGazing = false;
                }
            }
        }
        else
        {
            // If no object is detected by the raycast (ray missed the spatial UI)
            if (isGazing)
            {
                Debug.Log($"Stopped gazing at the Spatial UI.");
                isGazing = false;
            }
        }

        // Optional: Visualize the ray in the scene for debugging
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 10, Color.red);
    }

    private void SimulateMouseCameraRotation()
    {
        // Rotate the camera based on mouse movement (for testing in the editor only)
        if (Application.isEditor)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            float sensitivity = 2f; // Adjust sensitivity as needed
            cameraTransform.Rotate(Vector3.up, mouseX * sensitivity, Space.World);
            cameraTransform.Rotate(Vector3.left, mouseY * sensitivity);
        }
    }

    private void OnConfirmButtonSpatialUIClicked()
    {
        // Stop tracking when the confirm button is clicked
        isTracking = false;

        Debug.Log($"Spatial UI Confirm clicked. Total Gaze Time: {totalGazeTime:F2} seconds");
        SaveTotalGazeTimeToFile(totalGazeTime);

        Debug.Log("Gaze tracking stopped for Spatial UI.");
    }

    private void SaveTotalGazeTimeToFile(float totalGazeTime)
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
