using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SpatialUIGazeTimeTracker : MonoBehaviour
{
    public Transform cameraTransform;
    public GameObject spatialUIParent;
    public Button confirmButtonSpatialUI; 
    public string filePath = "Assets/PrivacyNotifications/Data/SpatialUIGazeDurations.csv";
    private float totalGazeTime = 0f;
    private bool isTracking = true;
    private bool isGazing = false;

    void Start()
    {
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (confirmButtonSpatialUI != null)
        {
            confirmButtonSpatialUI.onClick.AddListener(OnConfirmButtonSpatialUIClicked);
        }

        Debug.Log("Spatial UI Gaze Time Tracker initialized, ready to track gaze data");
    }

    // Tracks user gaze on Spatial UI using raycasting from the camera
    // Simulates camera movement with the mouse in the editor and accumulates gaze time when targeting UI elements.
    void Update()
    {
        if (!isTracking) return;

        SimulateMouseCameraRotation();

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.transform.IsChildOf(spatialUIParent.transform))
            {
                if (!isGazing)
                {
                    Debug.Log($"Started gazing at: {hit.collider.gameObject.name} within Spatial UI");
                    isGazing = true;
                }
                totalGazeTime += Time.deltaTime;
                Debug.Log($"Gazing at {hit.collider.gameObject.name}. Total Gaze Time: {totalGazeTime:F2} seconds");
            }
            else
            {
                if (isGazing)
                {
                    Debug.Log($"Stopped gazing at the Spatial UI.");
                    isGazing = false;
                }
            }
        }
        else
        {
            if (isGazing)
            {
                Debug.Log($"Stopped gazing at the Spatial UI");
                isGazing = false;
            }
        }
        //Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 10, Color.red);
    }

    // Rotate the camera based on mouse movement for testing in the editor only
    private void SimulateMouseCameraRotation()
    {
        if (Application.isEditor)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            float sensitivity = 2f;
            cameraTransform.Rotate(Vector3.up, mouseX * sensitivity, Space.World);
            cameraTransform.Rotate(Vector3.left, mouseY * sensitivity);
        }
    }

    // Stop tracking when the confirm button is clicked
    private void OnConfirmButtonSpatialUIClicked()
    {
        isTracking = false;
        Debug.Log($"Spatial UI Confirm clicked. Total Gaze Time: {totalGazeTime:F2} seconds");
        SaveTotalGazeTimeToFile(totalGazeTime);
        Debug.Log("Gaze tracking stopped for Spatial UI");
    }

    private void SaveTotalGazeTimeToFile(float totalGazeTime)
    {
        string data = $"Total Gaze Time: {totalGazeTime:F2} seconds, Date: {System.DateTime.Now}\n";
        try
        {
            File.AppendAllText(filePath, data);
            Debug.Log($"Gaze time saved: {data}");
        }
        catch (IOException e)
        {
            Debug.LogError("Error saving gaze time to file: " + e.Message);
        }
    }
}
