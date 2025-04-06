using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class VRGazeTimeTracker : MonoBehaviour
{
    public Transform cameraTransform;
    public GameObject uiParent;
    public Button confirmButton;
    public string filePath = "Assets/PrivacyNotifications/Data/GazeDurations.csv";
    private float totalGazeTime = 0f;
    private GameObject currentGazedObject;
    private bool isTracking = true;
    private bool isGazing = false;

    void Start()
    {
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        Debug.Log("Gaze Time Tracker initialized, ready to track gaze data");
    }

    // Stop tracking if confirm has been pressed
    // Simulate camera rotation with the mouse for testing
    // Perform raycast to detect objects
    void Update()
    {
        if (!isTracking) return; 

        SimulateMouseCameraRotation();

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (IsUIElement(hitObject))
            {
                if (!isGazing)
                {
                    Debug.Log($"Started gazing at: {hitObject.name}");
                    isGazing = true;
                }

                currentGazedObject = hitObject;
                totalGazeTime += Time.deltaTime;
                //Debug.Log($"Gazing at {hitObject.name}. Total Gaze Time: {totalGazeTime:F2} seconds");
            }
            else
            {
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
            if (isGazing)
            {
                Debug.Log($"Stopped gazing at: {currentGazedObject?.name}");
                isGazing = false;
            }
            currentGazedObject = null;
        }
        //Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 10, Color.red);
    }

    // Rotate the camera based on mouse movement
    private void SimulateMouseCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float sensitivity = 2f;
        cameraTransform.Rotate(Vector3.up, mouseX * sensitivity, Space.World);
        cameraTransform.Rotate(Vector3.left, mouseY * sensitivity);
    }

    // Check if the object is part of the UI hierarchy under uiParent
    private bool IsUIElement(GameObject obj)
    {
        if (uiParent != null && obj.transform.IsChildOf(uiParent.transform))
        {
            return true;
        }
        return false;
    }

    // Toggle tracking when the user interacts with the book UI
    public void ToggleTracking(bool isActive)
    {
        isTracking = isActive;

        if (isTracking)
        {
            Debug.Log("Gaze tracking resumed");
        }
        else
        {
            Debug.Log("Gaze tracking paused");
        }
    }

    private void OnConfirmButtonClicked()
    {
        //isTracking = false;
        //Debug.Log($"Confirm clicked. Total Gaze Time: {totalGazeTime:F2} seconds");

        SaveTotalGazeTimeToFile(totalGazeTime);
        Debug.Log($"Confirm clicked. Total Gaze Time so far: {totalGazeTime:F2} seconds");
    }

    // Method with no arguments - uses the internal totalGazeTime variable
    public void SaveTotalGazeTimeToFile()
    {
        SaveTotalGazeTimeToFile(totalGazeTime);
    }

    public void SaveTotalGazeTimeToFile(float totalGazeTime)
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
