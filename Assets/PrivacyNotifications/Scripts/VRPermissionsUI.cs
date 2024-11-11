using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRPermissionsUI : MonoBehaviour
{
    // Slider references for each option
    public Slider acceptAllSlider;
    public Slider bodyTrackingSlider;
    public Slider videoRecordingSlider;
    public Slider eyeTrackingSlider;
    public Slider locationSlider;
    public Slider voiceRecordingSlider;

    // Confirm button and the UI panel
    public Button confirmButton;
    public GameObject uiPanel;

    // List to hold all sliders except "Accept All" for easy management
    private List<Slider> allSliders;
    
    // Flag to prevent recursive updates
    private bool isUpdating = false;

    void Start()
    {
        // Initialize the list with all individual sliders
        allSliders = new List<Slider> { bodyTrackingSlider, videoRecordingSlider, eyeTrackingSlider, locationSlider, voiceRecordingSlider };

        // Subscribe to slider and button events
        acceptAllSlider.onValueChanged.AddListener(OnAcceptAllSlid);
        confirmButton.onClick.AddListener(OnConfirmClicked);

        // Add listeners for each individual slider to detect manual changes
        foreach (var slider in allSliders)
        {
            slider.onValueChanged.AddListener(OnIndividualSliderChanged);
        }
    }

    // Called when "Accept All" slider is changed
    private void OnAcceptAllSlid(float value)
    {
        if (isUpdating) return;  // Ignore if we're in the middle of a programmatic update

        bool isOn = value >= 0.5f; // Treat slider values >= 0.5 as "on"

        // Set all individual sliders to match the "Accept All" state
        isUpdating = true; // Prevent recursive triggering
        foreach (var slider in allSliders)
        {
            slider.value = isOn ? 1 : 0; // Set each slider to max (1) or min (0) based on Accept All slider
        }
        isUpdating = false;
    }

    // Called when any individual slider is changed
    private void OnIndividualSliderChanged(float value)
    {
        if (isUpdating) return; // Ignore if we're in the middle of a programmatic update

        // Check if all sliders are at max (1)
        bool allAreOn = true;
        foreach (var slider in allSliders)
        {
            if (slider.value < 1)
            {
                allAreOn = false;
                break;
            }
        }

        // If not all sliders are on, turn off the "Accept All" slider
        if (!allAreOn)
        {
            isUpdating = true; // Prevent recursive triggering
            acceptAllSlider.value = 0;
            isUpdating = false;
        }
    }

    // Called when the "Confirm" button is clicked
    private void OnConfirmClicked()
    {
        uiPanel.SetActive(false); // Hides the UI panel
    }
}
